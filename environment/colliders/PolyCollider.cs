using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public class PolyCollider
    {
        public readonly Vector3[] Points;
        public readonly float PointsNumber;

        public PolyCollider(Vector3[] points, float pointsNumber)
        {
            Points = points;
            PointsNumber = pointsNumber;
        }
    }
}
