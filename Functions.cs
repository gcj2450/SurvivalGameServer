using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class Functions
    {
        public const float Deg2Rad = 0.017453292f; //(float)Math.PI / 180f;
        public const float is180_pi = 57.295776f; //(float)Math.PI / 180f;

        public static void normalize_to_vector(ref float[] vector_axis)
        {
            float find_max = vector3_magnitude_unity(vector_axis[0], vector_axis[1], vector_axis[2]);
            if (find_max > 0.00001f)
            {
                vector_axis = new float[] { (vector_axis[0] / find_max), (vector_axis[1] / find_max), (vector_axis[2] / find_max) };
            }
            else
            {
                vector_axis = new float[] { 0, 0, 0 };
            }
        }

        public static float vector3_magnitude_unity(float x_axis_1, float y_axis_1, float z_axis_1)
        {
            return MathF.Sqrt(x_axis_1 * x_axis_1 + y_axis_1 * y_axis_1 + z_axis_1 * z_axis_1);
        }

        public static float RadianToDegree(float angle)
        {
            return angle * (180 / (float)Math.PI);
        }

        public static float[] Lerp(float start_x, float start_y, float start_z, float end_x, float end_y, float end_z, float koef)
        {

            float DeltaX = (end_x - start_x) * koef;

            float DeltaY = (end_y - start_y) * koef;

            float DeltaZ = (end_z - start_z) * koef;

            return new float[3] { start_x + DeltaX, start_y + DeltaY, start_z + DeltaZ };
        }

        public static Vector3 Lerp(Vector3 start, Vector3 end, float koef)
        {

            float DeltaX = (end.X - start.X) * koef;

            float DeltaY = (end.Y - start.Y) * koef;

            float DeltaZ = (end.Z - start.Z) * koef;

            return new Vector3 (start.X + DeltaX, start.Y + DeltaY, start.Z + DeltaZ);
        }

        public static float Vector3Distance(Vector3 one, Vector3 two)
        {
            float xxxxx = MathF.Abs(one.X - two.X);
            float yyyyy = MathF.Abs(one.Y - two.Y);
            float zzzzz = MathF.Abs(one.Z - two.Z);
            return MathF.Sqrt(xxxxx * xxxxx + yyyyy * yyyyy + zzzzz * zzzzz);
        }
    }
}
