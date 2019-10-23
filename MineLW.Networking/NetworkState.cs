using System;
using DotNetty.Buffers;
using MineLW.API.Text;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking
{
    public abstract class NetworkState
    {
        public readonly bool Async;

        private readonly IMessageSerializer[] _serializers;
        private readonly IMessageDeserializer[] _deserializers;

        protected NetworkState(bool async = false)
        {
            Async = async;

            // ReSharper disable VirtualMemberCallInConstructor
            _serializers = GetSerializers();
            _deserializers = GetDeserializers();
        }

        public abstract IMessage CreateDisconnectMessage(TextComponent reason);
        protected abstract IMessageSerializer[] GetSerializers();
        protected abstract IMessageDeserializer[] GetDeserializers();
        public abstract MessageController CreateController(NetworkClient client);

        public void Serialize(IByteBuffer buffer, IMessage message)
        {
            for (var i = 0; i < _serializers.Length; i++)
            {
                var serializer = _serializers[i];
                if (serializer == null || !serializer.CanSerialize(message))
                    continue;

                buffer.WriteVarInt32(i);
                serializer.Serialize(buffer, message);
                return;
            }

            throw new NotSupportedException("No serializer for message " + message);
        }

        public IMessageDeserializer GetDeserializer(int id)
        {
            return id < 0 || id >= _deserializers.Length ? null : _deserializers[id];
        }

        public void Handle(MessageController controller, IMessage message)
        {
            foreach (var deserializer in _deserializers)
                deserializer?.Handle(controller, message);
        }
    }
}