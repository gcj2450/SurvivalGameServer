using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SurvivalGameServer
{
    public class PlayerConnection
    {
        public readonly int TicketID;
        public byte[] NetworkID { get; private set; }
        public byte[] SecretKey { get; private set; }
        
        private Guid guid;
        private EndPoint endPoint;

        private ConcurrentQueue<MovementPacketFromClient> movementPacketsQueue;
        private MovementPacketFromClient currentMovementPacket;
        private MovementPacketFromServer movementPacketFromServer;

        private Servers connections;

        public PlayerCharacter PlayerCharacter { get; private set; }

        public PlayerConnection(int ticket, PlayerCharacter playerCharacter)
        {
            TicketID = ticket;
            PlayerCharacter = playerCharacter;

            movementPacketsQueue = new ConcurrentQueue<MovementPacketFromClient>();
            movementPacketFromServer = new MovementPacketFromServer();
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
        }

        public void AddMovementPacket(MovementPacketFromClient movementPacket)
        {            
            movementPacketsQueue.Enqueue(movementPacket);
        }

        public void HandleMovementPacketsQueue(Action<PlayerConnection, MovementPacketFromClient> handleMovement)
        {            
            if (movementPacketsQueue.Count > 0)
            {                
                bool result = movementPacketsQueue.TryDequeue(out currentMovementPacket);

                if (result)
                {                    
                    handleMovement?.Invoke(this, currentMovementPacket);
                }

                movementPacketsQueue.Clear();
                //currentMovementPacket.Horizontal = 0;
                //currentMovementPacket.Vertical = 0;
                SendUpdatedMovementToPlayer();
            }
        }

        public void SendUpdatedMovementToPlayer()
        {            
            movementPacketFromServer.Update(
                PlayerCharacter.ObjectId,
                PlayerCharacter.Position.X,
                PlayerCharacter.Position.Y,
                PlayerCharacter.Position.Z,
                PlayerCharacter.Rotation.Y);

            connections.SendUDPMovementPacketFromServer(movementPacketFromServer, SecretKey, endPoint);
        }

        public async void SendMainPlayerData()
        {
            connections.SendTCPInitialPlayerData(
                new PlayerDataInitial(PlayerCharacter), SecretKey, guid);

            await Task.Delay(Globals.TICKi);

            connections.SendUDPMovementPacketFromServer(movementPacketFromServer, SecretKey, endPoint);
        }

    }
}
