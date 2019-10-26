using System.Numerics;
using DotNetty.Buffers;
using MineLW.API.Entities;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientRotateToward : MessageSerializer<MessageClientRotateToward.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32((int) message.ReferencePoint);
            buffer.WriteVector3D(message.TargetPosition);
            
            if (message.HasTargetEntity)
            {
                buffer.WriteBoolean(true);
                buffer.WriteVarInt32(message.TargetEntity);
                buffer.WriteVarInt32((int) message.TargetBodyPart);
            }
            else
                buffer.WriteBoolean(false);
        }
        
        public struct Message : IMessage
        {
            public readonly EntityReference ReferencePoint;
            public readonly Vector3 TargetPosition;

            public readonly bool HasTargetEntity;
            public readonly int TargetEntity;
            public readonly EntityReference TargetBodyPart;

            public Message(EntityReference referencePoint, Vector3 targetPosition, bool hasTargetEntity, int targetEntity, EntityReference targetBodyPart)
            {
                ReferencePoint = referencePoint;
                TargetPosition = targetPosition;
                TargetEntity = targetEntity;
                TargetBodyPart = targetBodyPart;
                HasTargetEntity = hasTargetEntity;
            }
        }
    }
}