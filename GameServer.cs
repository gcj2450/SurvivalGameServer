using System;
using System.Net;
using System.Numerics;

namespace SurvivalGameServer
{
    internal class GameServer
    {
        private Lands lands;
        private List<PlayerCharacter> playerCharacters;
        private static GameServer gameServerInstance;
        
        //timer for movement update control
        private System.Timers.Timer updateMovementTimer;

        private GameServer()
        {
            lands = new Lands();
            playerCharacters = new List<PlayerCharacter>();

            updateMovementTimer = new System.Timers.Timer(Globals.TICKi);
            updateMovementTimer.Elapsed += delegate {
                updateMovementForPlayers();
            };
            updateMovementTimer.AutoReset = true;
            updateMovementTimer.Enabled = true;
        }

        private void updateMovementForPlayers()
        {
            for (int i = 0; i < playerCharacters.Count; i++)
            {
                
                //playerCharacters[i].Connection.HandleMovementPacketsQueue(SetPositionForPlayerCharacter);
                playerCharacters[i].Connection.ListOfMovementPackets.Clear();

                for (int j = 0; j < playerCharacters.Count; j++)
                {   
                    playerCharacters[i].Connection.ListOfMovementPackets.AddOrUpdate(playerCharacters[j].Connection.MovementPacketFromServer);                    
                }
                
                if (playerCharacters[i].Connection.ListOfMovementPackets.ListOfPackets.Count > 0) playerCharacters[i].Connection.SendUpdatedMovementToPlayer();
                
            }

            for (int i = 0; i < playerCharacters.Count; i++)
            {
                playerCharacters[i].Connection.IsMovementDirty = false;
            }
        }

        public static GameServer GetGameServerInstance()
        {
            if (gameServerInstance == null)
            {
                gameServerInstance = new GameServer();
            }
            return gameServerInstance;
        }

        public void AddPlayerCharacter(PlayerCharacter character)
        {
            if (!playerCharacters.Contains(character)) playerCharacters.Add(character);
        }


        /*
        public void SetPositionForPlayerCharacter(PlayerConnection playerConnection, MovementPacketFromClient movementPacket)
        {
            //Console.WriteLine("packet proccesed: " + movementPacket.PacketId + " = " + movementPacket.Horizontal + " = " + movementPacket.Vertical);

            float speedKoeff = MathF.Sqrt(
                movementPacket.Horizontal * movementPacket.Horizontal + movementPacket.Vertical * movementPacket.Vertical);
                        
            //speedKoeff = speedKoeff <= 3 ? speedKoeff : 3;

            float brutto_angle = MathF.Atan2(movementPacket.Horizontal, movementPacket.Vertical) * Functions.is180_pi;

            float new_position_x = playerConnection.CurrentPlayerCharacter.Position.X 
                + MathF.Sin(brutto_angle * Functions.Deg2Rad) / 10f * playerConnection.CurrentPlayerCharacter.Speed * speedKoeff * 0.75f;
            
            float new_position_z = playerConnection.CurrentPlayerCharacter.Position.Z 
                + MathF.Cos(brutto_angle * Functions.Deg2Rad) / 10f * playerConnection.CurrentPlayerCharacter.Speed * speedKoeff * 0.75f;
            
            if (lands.isColliding(new Vector3(new_position_x, 0, new_position_z), 0.3f)) { return; }

            playerConnection.CurrentPlayerCharacter.SetNewOrientation(
                new_position_x,
                lands.GetWalkableYCoord(new_position_x, playerConnection.CurrentPlayerCharacter.Position.Y, new_position_z),
                new_position_z,
                0, brutto_angle, 0);

            //Console.WriteLine("current: " + playerConnection.CurrentPlayerCharacter.Position);
        }
        */

    }
}
