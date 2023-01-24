using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class PlayerConnection
    {
        public readonly int TicketID;
        public byte[] NetworkID { get; private set; }
        public byte[] SecretKey { get; private set; }
        public bool isUpdated;
        public Action<PlayerConnection, MovementPacketFromClient> HandleMovement;
        
        private Guid guid;
        private EndPoint endPoint;
        private System.Timers.Timer updateTimer;
        private ConcurrentQueue<MovementPacketFromClient> movementPackets;
        private MovementPacketFromClient currentMovementPacket;

        private Servers connections;

        public PlayerCharacter PlayerCharacter { get; private set; }

        public PlayerConnection(int ticket, PlayerCharacter playerCharacter)
        {
            TicketID = ticket;
            PlayerCharacter = playerCharacter;

            updateTimer = new System.Timers.Timer(Globals.TICKi);
            updateTimer.Elapsed += delegate {
                updateEveryTick();
            };
            updateTimer.AutoReset = true;            

            movementPackets = new ConcurrentQueue<MovementPacketFromClient>();

            connections = Servers.GetInstance();
        }

        public void SetConnectionData(byte[] secretKey, Guid id)
        {
            SecretKey = secretKey;
            guid = id;
        }
        public void SetEndpointForUDP(EndPoint point)
        {
            endPoint = point;
            updateTimer.Enabled = true;
        }


        public void AddMovementPacket(MovementPacketFromClient movementPacket)
        {            
            movementPackets.Enqueue(movementPacket);
        }

        private void updateEveryTick()
        {            
            if (movementPackets.Count > 0)
            {
                bool result = movementPackets.TryDequeue(out currentMovementPacket);

                if (result)
                {
                    HandleMovement?.Invoke(this, currentMovementPacket);                    
                    isUpdated = true;
                }
                movementPackets.Clear();
                currentMovementPacket.Horizontal = 0;
                currentMovementPacket.Vertical = 0;
            }

            if (isUpdated)
            {
                MovementPacketFromServer mover = new MovementPacketFromServer(
                    PlayerCharacter.Position.X,
                    PlayerCharacter.Position.Y,
                    PlayerCharacter.Position.Z,
                    PlayerCharacter.Rotation.X,
                    PlayerCharacter.Rotation.Y,
                    PlayerCharacter.Rotation.Z);
                
                connections.SendUDP(ProtobufSchemes.SerializeProtoBuf<MovementPacketFromServer>(mover), SecretKey, endPoint, Globals.PacketCode.MoveFromServer);
                isUpdated = false;
            }
            
        }


    }
}
