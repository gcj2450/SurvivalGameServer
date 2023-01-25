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
        private Dictionary<int,List<Vector3>> terrs = new Dictionary<int, List<Vector3>>();

        private Vector3[] grounds =  new Vector3[] {
            new Vector3(-3f,0,1.5f),
            new Vector3(-4,0,0.5f),
            new Vector3(-4,0,1.5f),
            new Vector3(-3,0,0.5f),
            new Vector3(-2,0,1.5f),
            new Vector3(-4,0,-0.5f),
            new Vector3(-2,0,0.5f),
            new Vector3(-1,0,1.5f),
            new Vector3(-3,0,-0.5f),
            new Vector3(-4,1,-1.5f),
            new Vector3(-1,0,0.5f),
            new Vector3(0,0,1.5f),
            new Vector3(-2,0,-0.5f),
            new Vector3(-3,1,-1.5f),
            new Vector3(-4,1,-2.5f),
            new Vector3(0,0,0.5f),
            new Vector3(1,0,1.5f),
            new Vector3(1,0,0.5f),
            new Vector3(-1,0,-0.5f),
            new Vector3(0,0,-0.5f),
            new Vector3(1,0,-0.5f),
            new Vector3(-2,1,-1.5f),
            new Vector3(-3,1,-2.5f),
            new Vector3(-4,1,-3.5f),
            new Vector3(-3,1,-3.5f),
            new Vector3(-2,1,-2.5f),
            new Vector3(-2,1,-3.5f),
            new Vector3(-1,0,-1.5f),
            new Vector3(0,0,-1.5f),
            new Vector3(1,0,-1.5f),
            new Vector3(-1,0,-2.5f),
            new Vector3(-1,0,-3.5f),
            new Vector3(0,0,-2.5f),
            new Vector3(1,0,-2.5f),
            new Vector3(0,0,-3.5f),
            new Vector3(1,0,-3.5f)
        };

        public Lands()
        {
            for (int i = 0; i < grounds.Length; i++)
            {
                int key = (int)(grounds[i].X + grounds[i].Z);
                if (!terrs.ContainsKey(key))
                {
                    terrs.Add(key, new List<Vector3> { grounds[i] });
                } 
                else
                {
                    terrs[key].Add(grounds[i]);
                }
            }
        }

        public float GetWalkableY(float X, float Z)
        {
            int key = (int)(X + Z);

            List<Vector3> arr = terrs[key];

            
            float Xmin = float.MinValue;
            float Xmax = float.MaxValue;
            float Zmin = float.MinValue;
            float Zmax = float.MaxValue;
            Vector3 Xminv = Vector3.Zero;
            Vector3 Xmaxv = Vector3.Zero;
            Vector3 Zminv = Vector3.Zero;
            Vector3 Zmaxv = Vector3.Zero;

            for (int i = 0; i < arr.Count; i++)
            {
                if (X <= arr[i].X && Xmax > arr[i].X)
                {
                    Xmax = arr[i].X;
                    Xmaxv = arr[i];
                } 
                else if (X >= arr[i].X && Xmin < arr[i].X)
                {
                    Xmin = arr[i].X;
                    Xminv = arr[i];
                }

                if (Z <= arr[i].Z && Zmax > arr[i].Z)
                {
                    Zmax = arr[i].Z;
                    Zmaxv = arr[i];
                }
                else if (Z >= arr[i].Z && Zmin < arr[i].Z)
                {
                    Zmin = arr[i].Z;
                    Zminv = arr[i];
                }
            }

            Console.WriteLine("===========================");
            Console.WriteLine(string.Join('=', arr));
            Console.WriteLine($"X={X} from {Xmin} to {Xmax}  and Z={Z} from {Zmin} to {Zmax}");
            Console.WriteLine(Xminv + " = " + Xmaxv + " = " + Zminv + " = " + Zmaxv);

            Console.WriteLine($"between {Xminv} and {Xmaxv} with {X} is x:{((X - (Xmax < Xmin ? Xmax : Xmin)) / MathF.Abs(Xmin - Xmax))}  multi {MathF.Abs(Xmaxv.Y - Xminv.Y)} plus {(Xmaxv.Y < Xminv.Y ? Xmaxv.Y : Xminv.Y)}");
            Console.WriteLine($"between {Zminv} and {Zmaxv} with {Z} is x:{((Z - (Zmax < Zmin ? Zmax : Zmin)) / MathF.Abs(Zmin - Zmax))}  multi {MathF.Abs(Zmaxv.Y - Zminv.Y)} plus {(Zmaxv.Y < Zminv.Y ? Zmaxv.Y : Zminv.Y)}");

            Console.WriteLine($"between {Xminv} and {Xmaxv} lerp is {Functions.Lerp(Xminv, Xmaxv, ((X - (Xmax < Xmin ? Xmax : Xmin)) / MathF.Abs(Xmin - Xmax)))}");
            Console.WriteLine($"between {Zminv} and {Zmaxv} lerp is {Functions.Lerp(Zminv, Zmaxv, ((Z - (Zmax < Zmin ? Zmax : Zmin)) / MathF.Abs(Zmin - Zmax)))}");

            float percentFromX = ((X - (Xmax<Xmin?Xmax:Xmin)) / MathF.Abs(Xmin - Xmax)) * MathF.Abs(Xmaxv.Y - Xminv.Y) + (Xmaxv.Y < Xminv.Y ? Xmaxv.Y : Xminv.Y);
            float percentFromZ = ((Z - (Zmax<Zmin?Zmax:Zmin)) / MathF.Abs(Zmin - Zmax)) * MathF.Abs(Zmaxv.Y - Zminv.Y) + (Zmaxv.Y < Zminv.Y ? Zmaxv.Y : Zminv.Y);

            Console.WriteLine("===========================");
            return Functions.Lerp(Xminv, Xmaxv, ((X - (Xmax < Xmin ? Xmax : Xmin)) / MathF.Abs(Xmin - Xmax))).Y;
        }
    }


}
