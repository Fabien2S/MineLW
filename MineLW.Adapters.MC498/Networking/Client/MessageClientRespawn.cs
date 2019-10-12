using DotNetty.Buffers;
using MineLW.API.Entities.Living.Player;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientRespawn : MessageSerializer<MessageClientRespawn.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteInt(message.Environment);
            buffer.WriteByte((byte) message.PlayerMode);
            buffer.WriteUtf8(message.LevelType);
        }
        
        public struct Message : IMessage
        {
            public readonly int Environment;
            public readonly PlayerMode PlayerMode;
            public readonly string LevelType;

            public Message(int environment, PlayerMode playerMode, string levelType)
            {
                Environment = environment;
                PlayerMode = playerMode;
                LevelType = levelType;
            }
        }
    }
}