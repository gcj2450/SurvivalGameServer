using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public interface ICollider
    {
        List<Vector3> GetCoverageCells();
        bool isColliding(Vector3 point, float radius);
    }
}
