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
                
#if DEBUG
                if(serializer == null)
                    continue;
#endif
                
                if (!serializer.CanSerialize(message))
                    continue;

                buffer.WriteVarInt32(i);
                serializer.Serialize(buffer, message);
                return;
            }

            throw new NotSupportedException("No serializer for message " + message);
        }

        public IMessage Deserialize(IByteBuffer buffer, int id)
        {
            if (id < 0 || id >= _deserializers.Length)
                throw new NullReferenceException("No deserializer for id " + id);

            var deserializer = _deserializers[id];
            if (deserializer == null)
                throw new NullReferenceException("No deserializer for id " + id);

            return deserializer.Deserialize(buffer);
        }

        public void Handle(MessageController controller, IMessage message)
        {
            foreach (var deserializer in _deserializers)
                deserializer.Handle(controller, message);
        }
    }
}