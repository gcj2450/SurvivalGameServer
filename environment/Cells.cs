using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public struct Cells
    {
        public Vector2 Location { get; private set; }
        public Vector3 MinCoord { get; private set; }
        public Vector3 MaxCoord { get; private set; }
        public HashSet<Vector3> TerrainVectors;
        public HashSet<Characters> Characters;
        public HashSet<RectCollider> RectColliders;
        public Cells(float minX, float minZ, float maxX, float maxZ) 
        {
            Location = new Vector2(minX, minZ);
            MinCoord = new Vector3(minX, 0, minZ);
            MaxCoord = new Vector3(maxX, 0, maxZ);
            TerrainVectors = new HashSet<Vector3>();
            Characters = new HashSet<Characters>();
            RectColliders = new HashSet<RectCollider>();
        }

        public bool isInsideThisCell(Vector3 position)
        {
            return position.X >= MinCoord.X && position.Z >= MinCoord.Z && position.X < MaxCoord.X && position.Z < MaxCoord.Y;
        }                
    }
}
