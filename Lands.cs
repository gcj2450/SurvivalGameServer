using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SurvivalGameServer
{
    internal class Lands
    {
        private const float DIST_FOR_Y_AXIS = 2f;
        private WorldSystem ws;

        public Lands()
        {
            ws = new WorldSystem(new Vector2(-50, -50), 50, 50, new Terrain());
        }

        public float GetWalkableYCoord(float X, float Y, float Z)
        {            
            Vector3 vector = new Vector3(X,Y,Z);
            SortedDictionary<float, Vector3> vectors = new SortedDictionary<float, Vector3>();            
            Vector3[] terrainArray = ws.GetTerrainClosestVectors(vector);
            if (terrainArray.Length == 0) return 0;

            //if equal height
            float testY = 0;
            int result = 0;
            for (int i = 0; i < terrainArray.Length; i++)
            {
                if (i == 0)
                {
                    testY = terrainArray[i].Y;
                }
                else if (terrainArray[i].Y == testY)
                {
                    result++;
                }
            }
            if (result == terrainArray.Length - 1) return testY;

            //if different height
            for (int i = 0; i < terrainArray.Length; i++)
            {
                float distance = Functions.Vector3Distance(terrainArray[i], vector);
                if (distance < DIST_FOR_Y_AXIS)
                {
                    if (!vectors.ContainsKey(distance)) 
                        vectors.Add(distance, terrainArray[i]);                    
                }
            }
            
            float maxY = float.MinValue;
            float minY = float.MaxValue;

            foreach (Vector3 coords in vectors.Values)
            {
                if (maxY < coords.Y) maxY = coords.Y;
                if (minY > coords.Y) minY = coords.Y;
            }
                        
            return (maxY + minY) / 2f;
        }

        public bool isColliding(Vector3 position, float radius)
        {            
            if (ws.GetAllCollidersClosestVector(position).Length==0) return false;

            ICollider[] colliders = ws.GetAllCollidersClosestVector(position);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].isColliding(position, radius)) return true;
            }

            return false;
        }
    }


}
