using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public abstract class Characters
    {
        public long ObjectId { get; private set; }
        public ushort AppearanceId { get; private set; }
        public string Name { get; private set; }
        public ushort CurrentHealth { get; private set; }
        public ushort MaxHealth { get; private set; }
        public ushort Level { get; private set; }
        public ushort Armor { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }        
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
    }
}
