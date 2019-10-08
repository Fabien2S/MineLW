using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Client.MC498.Client
{
    public class MessageClientInitGame : MessageSerializer<MessageClientInitGame.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteInt(message.NetId);
            buffer.WriteByte(message.GameMode);
            buffer.WriteInt(message.Dimension);
            buffer.WriteByte(message.MaxPlayers);
            buffer.WriteUtf8(message.LevelType);
            buffer.WriteVarInt32(message.ViewDistance);
            buffer.WriteBoolean(message.ReducedDebugInfo);
        }

        public struct Message : IMessage
        {
            public readonly int NetId;
            public readonly byte GameMode;
            public readonly int Dimension;
            public readonly byte MaxPlayers;
            public readonly string LevelType;
            public readonly int ViewDistance;
            public readonly bool ReducedDebugInfo;

            public Message(int netId, byte gameMode, int dimension, byte maxPlayers, string levelType, int viewDistance,
                bool reducedDebugInfo)
            {
                NetId = netId;
                GameMode = gameMode;
                Dimension = dimension;
                MaxPlayers = maxPlayers;
                LevelType = levelType;
                ViewDistance = viewDistance;
                ReducedDebugInfo = reducedDebugInfo;
            }
        }
    }
}