using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public struct RectCollider
    {
        public readonly Vector3 StartPoint;
        public readonly Vector3 EndPoint;
        private List<Vector3> coverageCells;

        public RectCollider(Vector3 startPoint, Vector3 endPoint)
        {            
            StartPoint = startPoint;
            EndPoint = endPoint;

            //Coverage
            coverageCells = new List<Vector3>();
            for (int xx = (int)startPoint.X; xx <= (int)endPoint.X; xx++)
            {
                for (int zz = (int)startPoint.Z; zz <= (int)endPoint.Z; zz++)
                {
                    coverageCells.Add(new Vector3(xx, 0, zz));
                }
            }
        }

        public List<Vector3> GetCoverageCells()
        {
            return coverageCells;
        }
    }
}
