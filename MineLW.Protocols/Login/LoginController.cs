using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotNetty.Common.Internal;
using MineLW.Adapters;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking;
using MineLW.Networking.Messages;
using MineLW.Protocols.Login.Client;
using Newtonsoft.Json;
using NLog;

namespace MineLW.Protocols.Login
{
    public class LoginController : MessageController
    {
        private const string HasJoinedUrl = "https://sessionserver.mojang.com/session/minecraft/hasJoined";
        private const int CompressionThreshold = 256;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly HttpClient HttpClient = new HttpClient();

        private string _username;
        private byte[] _signature;
        private byte[] _sharedSecret;

        private PlayerProfile _profile;
        private IGameAdapter _adapter;

        public LoginController(NetworkClient client) : base(client)
        {
        }

        public void HandleLoginRequest(string username)
        {
            // "Login request" message already received
            if (_username != null)
                return;

            if (!GameAdapter.IsSupported(Client.Version))
            {
                Client.Disconnect(new TextComponentString("Unsupported version " + Client.Version.Protocol)
                {
                    Color = TextColor.Red
                });
                return;
            }

            _username = username;

            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                _signature = new byte[4];
                rngProvider.GetBytes(_signature);
            }

            Client.Send(new MessageClientEncryptionRequest.Message(
                string.Empty,
                Cryptography.PublicKey,
                _signature
            ));
        }

        public void HandleEncryptionResponse(byte[] encryptedSharedSecret, byte[] encryptedSignature)
        {
            // "Login request" message not received OR "Encryption response" already received
            if (_username == null || _signature == null)
                return;

            var decryptedSignature = Cryptography.CryptoServiceProvider.Decrypt(encryptedSignature, false);
            if (!decryptedSignature.SequenceEqual(_signature))
            {
                Client.Close("Invalid signature");
                return;
            }

            _signature = null;
            _sharedSecret = Cryptography.CryptoServiceProvider.Decrypt(encryptedSharedSecret, false);
            Client.EnableEncryption(_sharedSecret);

            Client
                .Send(new MessageClientEnableCompression.Message(CompressionThreshold))
                .ContinueWith(task => { Client.EnableCompression(CompressionThreshold); });

            RequestSession();
        }

        private void RequestSession(string ip = null)
        {
            var serverHash = Cryptography.GetServerHash(
                EmptyArrays.EmptyBytes,
                _sharedSecret,
                Cryptography.PublicKey
            );

            var urlBuilder = new UriBuilder(HasJoinedUrl);

            var queryString = new NameValueCollection
            {
                ["username"] = _username,
                ["serverId"] = serverHash
            };
            if (ip != null)
                queryString["ip"] = ip;
            urlBuilder.Query = string.Join("&", queryString.AllKeys.Select(key => key + '=' + queryString.Get(key)));

            Logger.Debug("Requesting player profile for user \"{0}\" (query: {1})", _username, urlBuilder.Query);

            HttpClient
                .GetAsync(urlBuilder.Uri)
                .ContinueWith(requestTask =>
                {
                    if (requestTask.IsFaulted)
                        throw requestTask.Exception;

                    var responseMessage = requestTask.Result;
                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        Client.Disconnect(new TextComponentTranslate("multiplayer.disconnect.authservers_down")
                        {
                            Color = TextColor.Red
                        });
                        return;
                    }

                    var httpContent = responseMessage.Content;
                    httpContent
                        .ReadAsStringAsync()
                        .ContinueWith(readTask =>
                        {
                            _profile = JsonConvert.DeserializeObject<PlayerProfile>(readTask.Result);

                            if (!_username.Equals(_profile.Name))
                            {
                                Client.Disconnect(
                                    new TextComponentTranslate("multiplayer.disconnect.unverified_username")
                                    {
                                        Color = TextColor.Red
                                    });
                                return;
                            }

                            Logger.Info("UUID of player {0} is {1}", _profile.Name, _profile.Id);

                            Client
                                .Send(new MessageClientLoginResponse.Message(
                                    _profile.Id.ToString(),
                                    _profile.Name
                                ))
                                .ContinueWith(CheckErrors)
                                .ContinueWith(task =>
                                {
                                    // if an error occurred
                                    if (Client.Closed)
                                        return;

                                    _adapter = GameAdapter.Resolve(Client.Version);
                                    Client.State = _adapter.NetworkState;
                                    Client.AddTask(FinalizeLogin);
                                });
                        }).ContinueWith(CheckErrors);
                }).ContinueWith(CheckErrors);
        }

        private void CheckErrors(Task task)
        {
            if (!task.IsFaulted)
                return;

            var taskException = task.Exception;
            var exception = taskException.InnerException;

            Logger.Error(exception, "Unable to complete the login sequence of {0}. Exception: {1}", _profile);
            Client.Disconnect();
        }

        private void FinalizeLogin()
        {
            var gameServer = Client.Server;
            var clientManager = gameServer.ClientManager;
            
            var gameClient = _adapter.CreateClient(_profile, Client);
            Logger.Info("{0} logged on successfully in {1}", gameClient, _adapter.Version);
            
            clientManager.Initialize(gameClient);
        }
    }
}