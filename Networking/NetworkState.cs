using DotNetty.Buffers;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking
{
    public abstract class NetworkState
    {
        private readonly IMessageSerializer[] _serializers;
        private readonly IMessageDeserializer[] _deserializers;

        public NetworkState()
        {
            // ReSharper disable VirtualMemberCallInConstructor
            _serializers = GetSerializers();
            _deserializers = GetDeserializers();
        }

        protected abstract IMessageSerializer[] GetSerializers();
        protected abstract IMessageDeserializer[] GetDeserializers();
        public abstract MessageController CreateController(NetworkClient client);

        public void Serialize(IByteBuffer buffer, IMessage message)
        {
            foreach (var serializer in _serializers)
                serializer.Serialize(buffer, message);
        }

        public IMessage Deserialize(IByteBuffer buffer, int id)
        {
            var deserializer = _deserializers[id];
            return deserializer.Deserialize(buffer);
        }
    }
}