using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public class WorldSystem
    {
        private Dictionary<Vector2, Cells> CellsSet = new Dictionary<Vector2, Cells>();
        public const int CELL_SIZE = 2;
        private Vector2 startPoint;
        private int worldWidth;
        private int worldHeight;
        
        public WorldSystem(Vector2 startPointLeftDown, int worldWidth, int worldHeight, Terrain terrain)
        {
            startPoint = startPointLeftDown;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;

            float width = startPointLeftDown.X;
            float height = startPointLeftDown.Y;

            for (int x = 0; x < worldWidth / CELL_SIZE; x++)
            {
                for (int y = 0; y < worldHeight / CELL_SIZE; y++)
                {
                    CellsSet.Add(new Vector2(width, height), new Cells(width, height, width + CELL_SIZE, height + CELL_SIZE));                    
                    height += CELL_SIZE;
                }
                height = 0;
                width += CELL_SIZE;
            }

            //Enter terrain data
            Vector3[] arr = terrain.GetTerrainArray();
            for (int i = 0; i < arr.Length; i++)
            {
                GetCell(arr[i]).TerrainVectors.Add(arr[i]);
            }

            List<ICollider> colliders = new List<ICollider>();
            colliders.AddRange(terrain.GetRectCollidersArray());
            colliders.AddRange(terrain.GetRoundCollidersArray());
            for (int i = 0; i < colliders.Count; i++)
            {
                List<Vector3> coverage = colliders[i].GetCoverageCells();
                for (int j = 0; j < coverage.Count; j++)
                {
                    GetCell(coverage[j]).AllColliders.Add(colliders[i]);
                }
            }
        }

        public Cells GetCell(Vector3 position)
        {
            int x = (int)position.X;
            if (x % 2 != 0) x--;

            int y = (int)position.Z;
            if (y % 2 != 0) y--;

            Vector2 result = new Vector2(x, y);

            if (CellsSet.ContainsKey(result))
            {
                return CellsSet[result];
            }
            else
            {
                return new Cells(0, 0, 0, 0);
            }
        }

        public Vector3[] GetTerrainClosestVectors(Vector3 position)
        {
            int x = (int)position.X;
            if (x % 2 != 0) x--;

            int y = (int)position.Z;
            if (y % 2 != 0) y--;
                        
            Vector3[] result = CellsSet[new Vector2(x, y)].TerrainVectors.ToArray();

            if (position.X < (x + (float)CELL_SIZE / 3))
            {                                   
                result = result.Concat(CellsSet[new Vector2(x - CELL_SIZE, y)].TerrainVectors.ToArray()).ToArray();                                 
            }
            else if(position.X > (x + (float)CELL_SIZE * 2 / 3))
            {                   
                result = result.Concat(CellsSet[new Vector2(x + CELL_SIZE, y)].TerrainVectors.ToArray()).ToArray();                                 
            }

            if (position.Z < (y + (float)CELL_SIZE / 3))
            {                
                result = result.Concat(CellsSet[new Vector2(x, y - CELL_SIZE)].TerrainVectors.ToArray()).ToArray();                                 
            }
            else if(position.Z > (y + (float)CELL_SIZE * 2 / 3))
            {   
                result = result.Concat(CellsSet[new Vector2(x, y + CELL_SIZE)].TerrainVectors.ToArray()).ToArray();                                 
            }

            return result;
        }
        
        public ICollider[] GetAllCollidersClosestVector(Vector3 position)
        {
            int x = (int)position.X;
            if (x % 2 != 0) x--;

            int y = (int)position.Z;
            if (y % 2 != 0) y--;

            return CellsSet[new Vector2(x, y)].AllColliders.ToArray();
        }



    }
}
