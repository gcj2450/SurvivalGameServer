using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGameServer
{
    internal class Functions
    {
        public const float Deg2Rad = (float)Math.PI / 180f;

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
    }
}
