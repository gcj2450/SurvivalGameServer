using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public abstract class BaseCharacter
    {
        public readonly int Id;
        public uint CurrentHealth { get; private set; }
        public uint MaxHealth { get; private set; }
        public uint Level { get; private set; }
        public uint Armor { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public uint ZoneOfLocationId { get; private set; }
        private float speed;
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

        public BaseCharacter(){}
        public void SetNewOrientation(Vector3 position, Vector3 rotation, uint zone)
        {
            Position = position;
            Rotation = rotation;
            ZoneOfLocationId = zone;
        }
        public void SetNewOrientation(float positionX, float positionY, float positionZ, 
            float rotationX, float rotationY, float rotationZ, uint zone)
        {
            Position = new Vector3(positionX, positionY, positionZ);
            Rotation = new Vector3(rotationX, rotationY, rotationZ);
            ZoneOfLocationId = zone;
        }
        public void SetNewPosition(Vector3 position)
        {
            Position = position;
        }
        public void SetNewRotation(Vector3 rotation)
        {
            Rotation = rotation;
        }
    }
}
