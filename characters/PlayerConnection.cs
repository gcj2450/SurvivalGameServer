using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SurvivalGameServer
{
    public class PlayerConnection
    {
        public readonly int TicketID;
        public byte[] NetworkID { get; private set; }
        public byte[] SecretKey { get; private set; }
        public ListOfMovementPacketsFromServer ListOfMovementPackets { get; private set; }

        private Guid guid;
        private EndPoint endPoint;

        private ConcurrentQueue<MovementPacketFromClient> movementPacketsQueue;
        private MovementPacketFromClient currentMovementPacket;
        private MovementPacketFromClient previousMovementPacket;
        private MovementPacketFromClient agregatePacket;
        private int possibleLostPackets;
        private bool isZeroMovementPacketProcessed;
        public bool IsMovementDirty;

        public MovementPacketFromServer movementPacketFromServer { get; private set; }

        private Servers connections;

        public PlayerCharacter CurrentPlayerCharacter { get; private set; }

        public PlayerConnection(int ticket, PlayerCharacter playerCharacter)
        {
            TicketID = ticket;
            CurrentPlayerCharacter = playerCharacter;

            movementPacketsQueue = new ConcurrentQueue<MovementPacketFromClient>();
            movementPacketFromServer = new MovementPacketFromServer(CurrentPlayerCharacter.ObjectId);
            ListOfMovementPackets = new ListOfMovementPacketsFromServer(1);
            connections = Servers.GetInstance();
            isZeroMovementPacketProcessed = false;
            IsMovementDirty = false;
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
            ListOfMovementPackets.Clear();
        }

        public void AddMovementPacket(MovementPacketFromClient movementPacket)
        {
            //Console.WriteLine(movementPacket.PacketId + " = " + movementPacket.Horizontal + " = " + movementPacket.Vertical + " = " + Globals.GlobalTimer.ElapsedMilliseconds);
            if (movementPacket.Horizontal == 999.9f || movementPacket.Vertical == 0)
            {                
                movementPacket.Horizontal = 0;
            }
                            
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
                    //int packetsToTake = (possibleLostPackets + 1) >= movementPacketsQueueCount ? movementPacketsQueueCount : (possibleLostPackets + 1);
                    int packetsToTake = movementPacketsQueueCount;

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
                        //packetsToTake--;
                        //if (packetsToTake <= 0) break;
                    }

                    
                    possibleLostPackets = 0;
                }
             
                if (currentMovementPacket.Horizontal != 0 || currentMovementPacket.Vertical != 0)
                {
                    handleMovement?.Invoke(this, currentMovementPacket);
                    //SendUpdatedMovementToPlayer(currentMovementPacket);                        
                    movementPacketFromServer.Update(
                        CurrentPlayerCharacter.ObjectId,
                        (uint)currentMovementPacket.PacketId,
                        CurrentPlayerCharacter.Position.X,
                        CurrentPlayerCharacter.Position.Y,
                        CurrentPlayerCharacter.Position.Z,
                        CurrentPlayerCharacter.Rotation.Y);
                    
                    IsMovementDirty = true;
                    isZeroMovementPacketProcessed = false;
                }
                else
                {
                    movementPacketFromServer.Update(
                        CurrentPlayerCharacter.ObjectId,
                        (uint)currentMovementPacket.PacketId,
                        0,
                        0,
                        0,
                        0);
                    IsMovementDirty = true;
                    
                }

                movementPacketsQueue.Clear();
            }
            else if (previousMovementPacket.Horizontal != 0 || previousMovementPacket.Vertical != 0)
            {                
                possibleLostPackets++;
                
            }            
        }

        public void SendUpdatedMovementToPlayer()
        {
            if (endPoint != null)
            {                
                //ListOfMovementPackets.AddOrUpdate(movementPacketFromServer);
                //Console.WriteLine(PlayerCharacter.ObjectId + " = " + movementPacketFromServer.PacketOrder);
                connections.SendListOfMovementPacketsFromServer(ListOfMovementPackets, SecretKey, endPoint);
                //connections.SendUDPMovementPacketFromServer(movementPacketFromServer, SecretKey, endPoint);
            }                
        }

        public async void SendMainPlayerData()
        {
            connections.SendTCPInitialPlayerData(
                new PlayerDataInitial(CurrentPlayerCharacter), SecretKey, guid);
            
            for (float i = 0; i < 5000; i++)
            {
                if (endPoint != null)
                {
                    movementPacketFromServer.Update(
                        CurrentPlayerCharacter.ObjectId,
                        0,
                        CurrentPlayerCharacter.Position.X,
                        CurrentPlayerCharacter.Position.Y,
                        CurrentPlayerCharacter.Position.Z,
                        CurrentPlayerCharacter.Rotation.Y);

                    //Console.WriteLine("started at: " + CurrentPlayerCharacter.Position);

                    connections.SendUDPMovementPacketFromServer(movementPacketFromServer, SecretKey, endPoint);
                    break;
                }
                
                await Task.Delay(Globals.TICKi);
            }            
        }

    }
}
