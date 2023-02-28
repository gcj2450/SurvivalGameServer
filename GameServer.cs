using System.Net;
using System.Numerics;

namespace SurvivalGameServer
{
    internal class GameServer
    {
        private Lands lands;
        private List<PlayerCharacter> playerCharacter;
        private static GameServer gameServerInstance;
        
        //timer for movement update control
        private System.Timers.Timer updateMovementTimer;

        private GameServer()
        {
            lands = new Lands();
            playerCharacter = new List<PlayerCharacter>();

            updateMovementTimer = new System.Timers.Timer(Globals.TICKi);
            updateMovementTimer.Elapsed += delegate {
                updateMovementForPlayers();
            };
            updateMovementTimer.AutoReset = true;
            updateMovementTimer.Enabled = true;
        }

        private void updateMovementForPlayers()
        {
            for (int i = 0; i < playerCharacter.Count; i++)
            {
                playerCharacter[i].Connection.HandleMovementPacketsQueue(SetPositionForPlayerCharacter);
                playerCharacter[i].Connection.SendUpdatedMovementToPlayer();
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
            playerCharacter.Add(character);
            //character.Connection.HandleMovement = SetPositionForPlayerCharacter;
        }

        public void SetPositionForPlayerCharacter(PlayerConnection playerConnection, MovementPacketFromClient movementPacket)
        {            
            float brutto_angle = MathF.Atan2(movementPacket.Horizontal, movementPacket.Vertical) * 180 / MathF.PI;

            float new_position_x = playerConnection.PlayerCharacter.Position.X 
                + MathF.Sin(brutto_angle * Functions.Deg2Rad) / 10f * playerConnection.PlayerCharacter.Speed;
            
            float new_position_z = playerConnection.PlayerCharacter.Position.Z 
                + MathF.Cos(brutto_angle * Functions.Deg2Rad) / 10f * playerConnection.PlayerCharacter.Speed;
            
                if (lands.isColliding(new Vector3(new_position_x, 0, new_position_z), 0.3f)) { return; }

            playerConnection.PlayerCharacter.SetNewOrientation(
                new_position_x,
                lands.GetWalkableYCoord(new_position_x, playerConnection.PlayerCharacter.Position.Y, new_position_z),
                new_position_z,
                0, brutto_angle, 0);
            
        }

    }
}
