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
        private MovementPacketFromClient previousMovementPacket;
        private MovementPacketFromClient agregatePacket;
        private int possibleLostPackets;
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
            movementPacketsQueue.Clear();
            currentMovementPacket.Clear();
            previousMovementPacket.Clear();
            agregatePacket.Clear();
        }

        public void AddMovementPacket(MovementPacketFromClient movementPacket)
        {
            //Console.WriteLine(movementPacket.PacketId + " = " + movementPacket.Horizontal + " = " + movementPacket.Vertical + " = " + Globals.GlobalTimer.ElapsedMilliseconds);
            if (movementPacket.Horizontal >= 999) movementPacket.Horizontal = 0;            
            movementPacketsQueue.Enqueue(movementPacket);
        }

        public void HandleMovementPacketsQueue(Action<PlayerConnection, MovementPacketFromClient> handleMovement)
        {
            int movementPacketsQueueCount = movementPacketsQueue.Count;

            if (movementPacketsQueueCount > 0)
            {
                if (possibleLostPackets == 0)
                {                    
                    movementPacketsQueue.TryDequeue(out currentMovementPacket);
                    previousMovementPacket = currentMovementPacket;
                }
                else
                {
                    int packetsToTake = (possibleLostPackets + 1) >= movementPacketsQueueCount ? movementPacketsQueueCount : (possibleLostPackets + 1);

                    agregatePacket.Clear();
                    currentMovementPacket.Clear();
                    //Console.WriteLine( "decision: queue: " + movementPacketsQueueCount + "   cur queu: " + movementPacketsQueue.Count + "  taken:" + packetsToTake);
                    foreach (var item in movementPacketsQueue)
                    {
                        //Console.WriteLine(packetsToTake + " ----------");
                        movementPacketsQueue.TryDequeue(out agregatePacket);                        
                        previousMovementPacket = agregatePacket;

                        currentMovementPacket.PacketId = agregatePacket.PacketId;
                        currentMovementPacket.Horizontal += agregatePacket.Horizontal;
                        currentMovementPacket.Vertical += agregatePacket.Vertical;
                        packetsToTake--;
                        if (packetsToTake <= 0) break;
                    }

                    possibleLostPackets = 0;
                }
             
                if (currentMovementPacket.Horizontal != 0 || currentMovementPacket.Vertical != 0)
                {
                    handleMovement?.Invoke(this, currentMovementPacket);                    
                    SendUpdatedMovementToPlayer(currentMovementPacket);                        
                }

                movementPacketsQueue.Clear();

            }
            else if (previousMovementPacket.Horizontal != 0 || previousMovementPacket.Vertical != 0)
            {                
                possibleLostPackets++;
                //Console.WriteLine("PREDICTION!!! - " + possibleLostPackets);
            }            
        }

        public void SendUpdatedMovementToPlayer(MovementPacketFromClient packet)
        {
            if (endPoint != null)
            {
                movementPacketFromServer.Update(
                PlayerCharacter.ObjectId,
                (uint)packet.PacketId,
                PlayerCharacter.Position.X,
                PlayerCharacter.Position.Y,
                PlayerCharacter.Position.Z,
                PlayerCharacter.Rotation.Y);

                connections.SendUDPMovementPacketFromServer(movementPacketFromServer, SecretKey, endPoint);
            }                
        }

        public async void SendMainPlayerData()
        {
            connections.SendTCPInitialPlayerData(
                new PlayerDataInitial(PlayerCharacter), SecretKey, guid);
            
            for (float i = 0; i < 5000; i++)
            {
                if (endPoint != null)
                {
                    movementPacketFromServer.Update(
                        PlayerCharacter.ObjectId,
                        0,
                        PlayerCharacter.Position.X,
                        PlayerCharacter.Position.Y,
                        PlayerCharacter.Position.Z,
                        PlayerCharacter.Rotation.Y);

                    //Console.WriteLine("started at: " + PlayerCharacter.Position);

                    connections.SendUDPMovementPacketFromServer(movementPacketFromServer, SecretKey, endPoint);
                    break;
                }
                
                await Task.Delay(Globals.TICKi);
            }            
        }

    }
}
