using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public class RoundCollider: ICollider
    {
        public readonly Vector3 CenterPoint;
        public readonly float Radius;
        private List<Vector3> coverageCells;

        public RoundCollider(Vector3 centerPoint, float radius)
        {
            CenterPoint = centerPoint;
            Radius = radius;

            //Coverage
            coverageCells = new List<Vector3>();
            for (int xx = (int)(centerPoint.X - radius); xx <= (int)(centerPoint.X + radius); xx++)
            {
                for (int zz = (int)(centerPoint.Z - radius); zz <= (int)(centerPoint.Z + radius); zz++)
                {
                    coverageCells.Add(new Vector3(xx, 0, zz));
                }
            }
        }
                
        public List<Vector3> GetCoverageCells()
        {
            return coverageCells;
        }
        
        public bool isColliding(Vector3 point, float radius)
        {
            return Functions.Vector3Distance(CenterPoint, point) < (radius + Radius);
        }
    }
}
