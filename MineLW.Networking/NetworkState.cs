using System;
using MineLW.API.Text;
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

        public IMessageSerializer GetSerializer(IMessage message, out int id)
        {
            for (id = 0; id < _serializers.Length; id++)
            {
                var serializer = _serializers[id];
                if (serializer == null || !serializer.CanSerialize(message))
                    continue;

                return serializer;
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