using DotNetty.Buffers;
using MineLW.API.Players;
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
            buffer.WriteByte((byte) message.GameMode);
            buffer.WriteUtf8(message.LevelType);
        }
        
        public struct Message : IMessage
        {
            public readonly int Environment;
            public readonly GameMode GameMode;
            public readonly string LevelType;

            public Message(int environment, GameMode gameMode, string levelType)
            {
                Environment = environment;
                GameMode = gameMode;
                LevelType = levelType;
            }
        }
    }
}