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
            buffer.WriteVarInt32(message.NetId);
            buffer.WriteByte(message.GameMode);
            buffer.WriteInt(message.Dimension);
            buffer.WriteLong(message.UnknownLong);
            buffer.WriteByte(message.MaxPlayers);
            buffer.WriteUtf8(message.LevelType);
            buffer.WriteVarInt32(message.ViewDistance);
            buffer.WriteBoolean(message.ReducedDebugInfo);
            buffer.WriteBoolean(message.UnknownBool);
        }

        public struct Message : IMessage
        {
            public readonly int NetId;
            public readonly byte GameMode;
            public readonly int Dimension;
            public readonly long UnknownLong;
            public readonly byte MaxPlayers;
            public readonly string LevelType;
            public readonly int ViewDistance;
            public readonly bool ReducedDebugInfo;
            public readonly bool UnknownBool;

            public Message(int netId, byte gameMode, int dimension, long unknownLong, byte maxPlayers, string levelType,
                int viewDistance, bool reducedDebugInfo, bool unknownBool)
            {
                NetId = netId;
                GameMode = gameMode;
                Dimension = dimension;
                UnknownLong = unknownLong;
                MaxPlayers = maxPlayers;
                LevelType = levelType;
                ViewDistance = viewDistance;
                ReducedDebugInfo = reducedDebugInfo;
                UnknownBool = unknownBool;
            }
        }
    }
}