using DotNetty.Buffers;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.States.Status.Client
{
    public class MessageClientServerInfo : MessageSerializer<MessageClientServerInfo.Message>
    {
        public override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteUtf8(@"{
    ""version"": {
            ""name"": ""19w40a"",
            ""protocol"": 557
        },
        ""players"": {
            ""max"": 20,
            ""online"": 5,
            ""sample"": []
        },	
        ""description"": {
            ""text"": ""Hello world""
        }
    }");
        }

        public struct Message : IMessage
        {
            public readonly Version Version;

            public readonly int OnlinePlayers;
            public readonly int MaxPlayers;
            public readonly PlayerProfile[] PlayerProfiles;

            public readonly ITextComponent Component;
            public readonly string Favicon;

            public Message(Version version, int onlinePlayers, int maxPlayers, PlayerProfile[] playerProfiles,
                ITextComponent component, string favicon)
            {
                Version = version;
                OnlinePlayers = onlinePlayers;
                MaxPlayers = maxPlayers;
                PlayerProfiles = playerProfiles;
                Component = component;
                Favicon = favicon;
            }
        }
    }
}