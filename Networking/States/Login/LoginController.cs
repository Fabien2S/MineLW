using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using DotNetty.Common.Internal;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Debugging;
using MineLW.Networking.Messages;
using MineLW.Networking.States.Login.Client;
using Newtonsoft.Json;

namespace MineLW.Networking.States.Login
{
    public class LoginController : MessageController
    {
        private const string HasJoinedUrl = "https://sessionserver.mojang.com/session/minecraft/hasJoined";
        private const int CompressionThreshold = 256;

        private static readonly Logger Logger = LogManager.GetLogger<LoginController>();

        private static readonly HttpClient HttpClient = new HttpClient();

        private string _username;
        private byte[] _signature;
        private byte[] _sharedSecret;

        public LoginController(NetworkClient client) : base(client)
        {
        }

        public void HandleLoginRequest(string username)
        {
            if (!NetworkAdapter.IsSupported(Client.Version))
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
            var decryptedSignature = Cryptography.CryptoServiceProvider.Decrypt(encryptedSignature, false);
            if (!decryptedSignature.SequenceEqual(_signature))
            {
                Client.Close("Invalid signature");
                return;
            }

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
                        throw new HttpRequestException("Unexpected response from the session server (" +
                                                       responseMessage.StatusCode + ')');

                    var httpContent = responseMessage.Content;
                    httpContent
                        .ReadAsStringAsync()
                        .ContinueWith(readTask =>
                        {
                            var profile = JsonConvert.DeserializeObject<PlayerProfile>(readTask.Result);

                            if (!_username.Equals(profile.Name))
                            {
                                Client.Close("Invalid username");
                                return;
                            }

                            Client.Profile = profile;
                            Logger.Info("UUID of {0} is {1}", profile.Name, profile.Id);

                            Client
                                .Send(new MessageClientLoginResponse.Message(
                                    profile.Id.ToString("D"),
                                    profile.Name
                                )).ContinueWith(task =>
                                {
                                    Client.State = NetworkAdapter.Resolve(Client.Version);
                                    Client.AddTask(FinalizeLogin);
                                });
                        });
                }).ContinueWith(task =>
                {
                    if (!task.IsFaulted)
                        return;

                    var taskException = task.Exception;
                    var exception = taskException.InnerException;

                    Logger.Error("Unable to complete the login sequence of {0}. Exception: {1}", Client, exception);
                    Client.Close("Error");
                });
        }

        private void FinalizeLogin()
        {
            Logger.Info("Logged in as {0} from {1}", Client.Profile, Client);
        }
    }
}