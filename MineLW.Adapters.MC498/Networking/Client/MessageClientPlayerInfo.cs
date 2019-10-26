using DotNetty.Buffers;
using MineLW.API.Entities.Living.Player;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientPlayerInfo : MessageSerializer<MessageClientPlayerInfo.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            var action =  message.Action;
            buffer.WriteVarInt32((int) action);

            var players = message.Players;
            buffer.WriteVarInt32(players.Length);
            
            foreach (var player in players)
            {
                buffer.WriteGuid(player.Uuid);

                if (action == Action.AddPlayer)
                {
                    var profile = player.Profile;
                    
                    // name
                    buffer.WriteUtf8(profile.Name);
                    
                    // properties
                    var properties = profile.Properties;
                    buffer.WriteVarInt32(properties.Length);
                    foreach (var property in properties)
                    {
                        buffer.WriteUtf8(property.Name);
                        buffer.WriteUtf8(property.Value);

                        var signature = property.Signature;
                        if (signature != null)
                        {
                            buffer.WriteBoolean(true);
                            buffer.WriteUtf8(signature);
                        }
                        else
                            buffer.WriteBoolean(false);
                    }
                }

                if (action == Action.AddPlayer || action == Action.UpdateGameMode)
                {
                    var gameMode = player.GameMode;
                    buffer.WriteVarInt32((int) gameMode);
                }

                if (action == Action.AddPlayer || action == Action.UpdateLatency)
                {
                    var latency = player.HasClient ? player.Client.Latency : 0;
                    buffer.WriteVarInt32((int) latency);
                }

                // ReSharper disable once InvertIf
                if (action == Action.AddPlayer || action == Action.UpdateDisplayName)
                {
                    var displayName = player.DisplayName;
                    if (displayName != null)
                    {
                        buffer.WriteBoolean(true);
                        buffer.WriteJson(displayName);
                    }
                    else
                        buffer.WriteBoolean(false);
                }
            }
        }
        
        public struct Message : IMessage
        {
            public readonly Action Action;
            public readonly IEntityPlayer[] Players;

            public Message(Action action, IEntityPlayer[] players)
            {
                Action = action;
                Players = players;
            }
        }
        
        public enum Action
        {
            AddPlayer,
            UpdateGameMode,
            UpdateLatency,
            UpdateDisplayName,
            RemovePlayer
        }
    }
}