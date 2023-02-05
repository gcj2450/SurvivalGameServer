using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public class RoundCollider : Collider
    {
        public readonly Vector3 CenterPoint;
        public readonly float Radius;

        public RoundCollider(Vector3 centerPoint, float radius) : base(ColliderType.Round)
        {
            CenterPoint = centerPoint;
            Radius = radius;
        }
    }
}
