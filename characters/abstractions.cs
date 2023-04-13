using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public abstract class Characters
    {
        private const float MAX_DISTANCE_FOR_POINT_TO_MOVE = 20;

        public long ObjectId { get; private set; }
        public ushort AppearanceId { get; private set; }
        public string Name { get; private set; }
        public ushort CurrentHealth { get; private set; }
        public ushort MaxHealth { get; private set; }
        public ushort Level { get; private set; }
        public ushort Armor { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 PointToMove { get; private set; }

        private float speed = 1f;
        public float Speed
        {
            get => speed;
            private set
            {
                if (value < 0)
                {
                    speed = 0;
                }
                else
                {
                    speed = value;
                }
            }
        }

        public byte AnimationId { get; private set; }

        //public Characters(){}

        protected Characters(
            long objectId, ushort appearanceId, string name, ushort currentHealth, ushort maxHealth, 
            ushort level, ushort armor, Vector3 position, Vector3 rotation, float speed)
        {
            ObjectId = objectId;
            AppearanceId = appearanceId;
            Name = name;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Level = level;
            Armor = armor;
            Position = position;
            Rotation = rotation;
            Speed = speed;
        }

        public void SetNewOrientation(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
        public void SetNewOrientation(float positionX, float positionY, float positionZ, 
            float rotationX, float rotationY, float rotationZ)
        {
            Position = new Vector3(positionX, positionY, positionZ);
            Rotation = new Vector3(rotationX, rotationY, rotationZ);
        }
        public void SetNewPosition(Vector3 position)
        {
            Position = position;
        }
        public void SetNewRotation(Vector3 rotation)
        {
            Rotation = rotation;
        }

        public void SetNewPointToMove(Vector3 pointToMove)
        {
            if (Functions.Vector3Distance(Position, pointToMove) > MAX_DISTANCE_FOR_POINT_TO_MOVE) return;

            PointToMove = pointToMove;

            Task.Run(() => moveToPoint(PointToMove));
        }

        private async void moveToPoint(Vector3 point)
        {
            float distance = Functions.Vector3Distance(point, Position);
            Vector3 startPoint = Position;

            for (float i = 0.001f; i < distance; i+=0.2f)
            {
                if (point != PointToMove)
                {                    
                    break;
                }
                    

                Vector3 normalVector = Functions.Normalize(point - Position);

                Rotation = new Vector3(
                    0,
                    MathF.Atan2(normalVector.X, normalVector.Z) * Functions.is180_pi, 
                    0);
                    //MathF.Atan2(movementPacket.Horizontal, movementPacket.Vertical) * Functions.is180_pi;

                Position = Functions.Lerp(startPoint, point, i / distance);
                //Console.WriteLine(normalVector + " = " + Rotation);
                await Task.Delay(50);
            }
        }
    }
}
