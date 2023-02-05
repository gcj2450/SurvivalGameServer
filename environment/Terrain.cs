using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    public class Terrain
    {
        public Terrain() { }

        public Vector3[] GetTerrainArray() => grounds;
        public RectCollider[] GetRectCollidersArray() => rectColliders;

        private RectCollider[] rectColliders = new RectCollider[]
        {
            new RectCollider(new Vector3(1, 0, 3.5f), new Vector3(3, 0, 4.5f))
        };

        private Vector3[] grounds = new Vector3[] {
            new Vector3(10.00f,0.02f,1.43f),
            new Vector3(10.00f,0.00f,0.00f),
            new Vector3(8.57f,0.00f,0.00f),
            new Vector3(8.57f,0.46f,1.43f),
            new Vector3(10.00f,0.11f,2.86f),
            new Vector3(7.14f,0.13f,0.00f),
            new Vector3(8.57f,0.76f,2.86f),
            new Vector3(10.00f,0.08f,4.29f),
            new Vector3(7.14f,0.91f,1.43f),
            new Vector3(5.71f,0.03f,0.00f),
            new Vector3(8.57f,0.46f,4.29f),
            new Vector3(10.00f,0.06f,5.71f),
            new Vector3(5.71f,0.56f,1.43f),
            new Vector3(4.29f,0.20f,0.00f),
            new Vector3(7.14f,0.62f,2.86f),
            new Vector3(8.57f,0.73f,5.71f),
            new Vector3(10.00f,0.05f,7.14f),
            new Vector3(4.29f,0.48f,1.43f),
            new Vector3(2.86f,0.00f,0.00f),
            new Vector3(5.71f,1.99f,2.86f),
            new Vector3(7.14f,0.67f,4.29f),
            new Vector3(8.57f,0.58f,7.14f),
            new Vector3(10.00f,0.02f,8.57f),
            new Vector3(2.86f,0.77f,1.43f),
            new Vector3(1.43f,0.00f,0.00f),
            new Vector3(7.14f,0.96f,5.71f),
            new Vector3(4.29f,0.76f,2.86f),
            new Vector3(5.71f,1.39f,4.29f),
            new Vector3(8.57f,0.35f,8.57f),
            new Vector3(10.00f,0.00f,10.00f),
            new Vector3(8.57f,0.03f,10.00f),
            new Vector3(1.43f,0.00f,1.43f),
            new Vector3(0.00f,0.00f,0.00f),
            new Vector3(0.00f,0.00f,1.43f),
            new Vector3(7.14f,0.64f,7.14f),
            new Vector3(7.14f,0.46f,8.57f),
            new Vector3(7.14f,0.07f,10.00f),
            new Vector3(2.86f,0.17f,2.86f),
            new Vector3(1.43f,0.00f,2.86f),
            new Vector3(0.00f,0.00f,2.86f),
            new Vector3(5.71f,0.84f,5.71f),
            new Vector3(4.29f,1.01f,4.29f),
            new Vector3(5.71f,1.08f,7.14f),
            new Vector3(5.71f,0.74f,8.57f),
            new Vector3(5.71f,0.02f,10.00f),
            new Vector3(1.43f,0.00f,4.29f),
            new Vector3(0.00f,0.00f,4.29f),
            new Vector3(2.86f,0.34f,4.29f),
            new Vector3(4.29f,0.68f,5.71f),
            new Vector3(4.29f,0.31f,7.14f),
            new Vector3(4.29f,0.24f,8.57f),
            new Vector3(4.29f,0.00f,10.00f),
            new Vector3(2.86f,0.35f,5.71f),
            new Vector3(1.43f,0.00f,5.71f),
            new Vector3(0.00f,0.00f,5.71f),
            new Vector3(2.86f,0.37f,7.14f),
            new Vector3(2.86f,0.19f,8.57f),
            new Vector3(2.86f,0.00f,10.00f),
            new Vector3(1.43f,0.00f,7.14f),
            new Vector3(0.00f,0.00f,7.14f),
            new Vector3(1.43f,0.00f,8.57f),
            new Vector3(1.43f,0.00f,10.00f),
            new Vector3(0.00f,0.00f,8.57f),
            new Vector3(0.00f,0.00f,10.00f)
        };
    }
}
