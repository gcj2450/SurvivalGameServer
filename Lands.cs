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

        private Vector3[] grounds =  new Vector3[] {
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

        public Lands()
        {
            
        }

        public float GetWalkableY(float X, float Z)
        {
            Vector3 vec = new Vector3(X,0,Z);
            SortedDictionary<float, Vector3> vecs = new SortedDictionary<float, Vector3>();

            for (int i = 0; i < grounds.Length; i++)
            {
                if (Functions.Vector3Distance(grounds[i], vec) < 4)
                {
                    if (!vecs.ContainsKey(Functions.Vector3Distance(grounds[i], vec))) vecs.Add(Functions.Vector3Distance(grounds[i], vec), grounds[i]);
                    
                }
            }

            //float ave = vecs.Select(k=>k.Value.Y).Take(4).Average();
            float ave = vecs.Select(k => k.Value.Y).Take(4).Average();
            float max = vecs.Select(k => k.Value.Y).Take(4).Max();
            float min = vecs.Select(k => k.Value.Y).Take(4).Min();
            float closect = vecs[vecs.Select(k => k.Key).First()].Y;

            //float result = ave / max < 0.3f ? ave : (ave + max) / 2;
            float result = (max + min) / 2f;


            Console.WriteLine("===============================");            
            Console.WriteLine($"averag {ave} and max {max} and sootn ave k max{ave/max} and closect Y{closect}");
            //Console.WriteLine($"x:{X} and z:{Z} min is {minimum} and {maximum}");
            return result;
        }
    }


}
