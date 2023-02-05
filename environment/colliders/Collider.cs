using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    abstract public class Collider
    {
        public ColliderType Type { get; set; }
        protected Dictionary<Vector3,Vector3> coverageCells;
        protected Collider(ColliderType Type)
        {
            this.Type = Type;
            coverageCells = new Dictionary<Vector3, Vector3>();
        }

        public virtual Dictionary<Vector3, Vector3> GetCoverageCells()
        {
            return coverageCells;
        }
    }

    public enum ColliderType
    {
        Poly = 0,
        Rect = 1,
        Round = 2
    }
}
