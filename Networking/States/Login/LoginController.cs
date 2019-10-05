using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotNetty.Common.Internal;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Debugging;
using MineLW.Networking.Messages;
using MineLW.Networking.States.Login.Client;

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

        private PlayerProfile _profile;

        public LoginController(NetworkClient client) : base(client)
        {
        }

        public void HandleLoginRequest(string username)
        {
            if (!NetworkAdapter.IsSupported(Client.Version))
            {
                var reason = new TextComponentString()
                    .WithValue("Unsupported version " + Client.Version.Protocol)
                    .WithColor(TextColor.Red);
                Client
                    .Send(new MessageClientDisconnect.Message(reason))
                    .ContinueWith(task => Client.Disconnect((string) reason));
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
                Client.Disconnect("Invalid signature");
                return;
            }

            _sharedSecret = Cryptography.CryptoServiceProvider.Decrypt(encryptedSharedSecret, false);
            Client.EnableEncryption(_sharedSecret);

            Client
                .Send(new MessageClientEnableCompression.Message(CompressionThreshold))
                .ContinueWith(task =>
                {
                    Client.EnableCompression(CompressionThreshold);
                });

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
            urlBuilder.Query = queryString.ToString();

            Logger.Debug("Requesting player profile for user \"{0}\"", _username);
            HttpClient
                .GetAsync(urlBuilder.Uri)
                .ContinueWith(HandleSessionResponse);
        }

        private void HandleSessionResponse(Task<HttpResponseMessage> response)
        {
            var responseMessage = response.Result;
            if (!responseMessage.IsSuccessStatusCode)
            {
                Client.Disconnect("Invalid profile");
                return;
            }

            var httpContent = responseMessage.Content;
            var responseContent = httpContent.ReadAsStringAsync().Result;
            //_profile = JsonConvert.DeserializeObject<PlayerProfile>(responseContent);
            _profile = new PlayerProfile("TheWhoosher", Guid.NewGuid());

            if (!_username.Equals(_profile.Name))
            {
                Client.Disconnect("Invalid username");
                return;
            }

            Logger.Info("UUID of {0} is {1}", _profile.Name, _profile.Uuid);

            Client
                .Send(new MessageClientLoginResponse.Message(
                    _profile.Uuid.ToString(),
                    _profile.Name
                )).ContinueWith(task =>
                {
                    Client.State = NetworkAdapter.Resolve(Client.Version);
                    Client.AddTask(FinalizeLogin);
                });
        }

        private void FinalizeLogin()
        {
            /*var playerManager = server.PlayerManager;
            playerManager.InitializePlayer(_profile, Connection);*/
        }
    }
}