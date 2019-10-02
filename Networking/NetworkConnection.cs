using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MineLW.Debugging;
using MineLW.Networking.IO;

namespace MineLW.Networking
{
    public class NetworkConnection
    {
        private static readonly Logger Logger = LogManager.GetLogger<NetworkConnection>();

        public bool Connected => _socket.Connected && !_closed;

        public NetworkController Controller { get; private set; }

        private readonly Socket _socket;
        private readonly Thread _networkThread;

        private readonly MessageManager _messageManager = new MessageManager();

        // encryption
        private IBufferedCipher _encryptCipher;
        private IBufferedCipher _decryptCipher;

        // compression
        private int _compressionThreshold = -1;

        private bool _closed = false;
        private bool _autoRead = false;

        private Stream _readStream;

        public NetworkConnection(EndPoint endPoint)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(endPoint);

            _networkThread = new Thread(ListenForData)
            {
                Name = "Network"
            };

            UpdateStreams();
            UpdateState(NetworkState.Handshake);
        }

        private void UpdateStreams()
        {
            var readStream = new NetworkStream(
                _socket,
                FileAccess.Read,
                false
            );
            _readStream = _decryptCipher != null
                ? (Stream) new CipherStream(readStream, _decryptCipher, null)
                : readStream;
        }

        private BitStream CreateReader(MemoryStream stream)
        {
            if (_compressionThreshold < 0) // compression disabled
                return new BitStream(stream);

            var uncompressedLength = stream.ReadInt32();
            if (uncompressedLength == 0) // message not compressed
                return new BitStream(stream);

            if (uncompressedLength >= _compressionThreshold)
                return new BitStream(stream, true);

            Disconnect("Badly compressed message");
            return null;
        }

        private void ListenForData()
        {
            while (_autoRead)
            {
                Logger.Debug("Waiting for messages...");

                var length = _readStream.ReadInt32();
                var buffer = new byte[length];

                var totalRead = 0;
                while (totalRead < length)
                    totalRead += _readStream.Read(buffer, totalRead, length - totalRead);

                Logger.Debug("Message of length {0} received", length);

                using var stream = new MemoryStream(buffer, false);
                using var reader = CreateReader(stream);
                if (!_messageManager.DeserializeMessage(reader, out var message, out var processor))
                {
#if DEBUG
                    continue;
#else
                        Disconnect("Invalid message received");
                        return;
#endif
                }

                Logger.Debug("Received message \"{0}\"", message);
                ProcessMessage(processor, message);
            }
        }

        private void ProcessMessage(IMessageDeserializer processor, object message)
        {
            try
            {
                processor.Handle(Controller, message);
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while handling message {0}: {1}", message, e);
            }
        }

        public void UpdateState(NetworkState state)
        {
            Logger.Debug("State of {0} is now {1}", this, state);

            _messageManager.UpdateProtocol(state);

            var protocolInfo = ProtocolInfo.Instance;
            Controller = protocolInfo[state].CreateController(this);
        }

        public void SetAutoRead(bool autoRead)
        {
            if (_autoRead == autoRead)
                return;

            if (autoRead)
            {
                _networkThread.Start();
                _autoRead = true;
            }
            else
                _autoRead = false;
        }

        public void EnableEncryption(byte[] sharedSecret)
        {
            Logger.Debug("Enabling encryption");

            _encryptCipher = NetworkHelper.CreateCipher(sharedSecret, true);
            _decryptCipher = NetworkHelper.CreateCipher(sharedSecret, false);
            UpdateStreams();
        }

        public void EnableCompression(int threshold)
        {
            Logger.Debug("Enabling compression (threshold: {0})", threshold);

            _compressionThreshold = threshold;
        }

        public void SendMessage(object message, Action andThen = null, bool async = true)
        {
            if (!_socket.Connected)
            {
                Logger.Error("Unable to send the message {0}: Not connected", message);
                return;
            }

            if (!_messageManager.SerializeMessage(message, out var messageContent))
            {
                Disconnect("Message serialization error");
                return;
            }

            var sourceStream = new MemoryStream();
            Stream layeredStream = sourceStream;

            // compression active
            if (_compressionThreshold >= 0)
            {
                var uncompressedLength = messageContent.Length;
                if (uncompressedLength >= _compressionThreshold)
                {
                    var uncompressedLengthBuffer = VarInt.SerializeInt32(uncompressedLength);
                    layeredStream.Write(uncompressedLengthBuffer, 0, uncompressedLengthBuffer.Length);
                    layeredStream = new ZlibStream(layeredStream, CompressionMode.Compress, CompressionLevel.Default);
                }
                else
                {
                    var notCompressedLengthBuffer = NetworkHelper.SerializeVarInt(0);
                    layeredStream.Write(notCompressedLengthBuffer, 0, notCompressedLengthBuffer.Length);
                }
            }

            layeredStream.Write(messageContent, 0, messageContent.Length);
            layeredStream.Close();

            // creating byte buffer
            var messageBodyBuffer = sourceStream.ToArray();
            var messageBodyLength = messageBodyBuffer.Length;

            // computing length
            var messageLengthBuffer = NetworkHelper.SerializeVarInt(messageBodyLength);
            var messageLength = messageLengthBuffer.Length;

            // computing final message
            var finalMessage = new byte[messageBodyLength + messageLength];
            Buffer.BlockCopy(messageLengthBuffer, 0, finalMessage, 0, messageLength);
            Buffer.BlockCopy(messageBodyBuffer, 0, finalMessage, messageLength, messageBodyLength);

            if (_encryptCipher != null)
                finalMessage = _encryptCipher.ProcessBytes(finalMessage);

            Logger.Debug("Sending message \"{0}\"", message);

            if (async)
                _socket.BeginSend(finalMessage, 0, finalMessage.Length, SocketFlags.None, SendCallback, andThen);
            else
            {
                _socket.Send(finalMessage);
                andThen?.Invoke();
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            _socket.EndSend(result);
            var andThen = result.AsyncState as Action;
            andThen?.Invoke();
        }

        public void Disconnect(string reason)
        {
            if (_closed)
                return;

            SetAutoRead(false);
            _closed = true;

            /*switch (Controller)
            {
                case InGameController _:
                    SendMessage(new MessageClientDisconnect.Message(new TextComponentString(reason)), async: false);
                    break;
                case LoginController _:
                    SendMessage(new Protocols.Login.Client.MessageClientDisconnect.Message(new TextComponentString(reason)), async: false);
                    break;
            }*/

            Logger.Info("Disconnected by the server (reason: {0})", reason);
            _socket.Close();
        }

        public override string ToString() => _socket.RemoteEndPoint.ToString();

        private struct QueuedMessage
        {
            public readonly IMessageDeserializer processor;
            public readonly object message;

            public QueuedMessage(IMessageDeserializer processor, object message)
            {
                this.processor = processor;
                this.message = message;
            }
        }
    }
}