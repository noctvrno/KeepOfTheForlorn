using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KOTF.Utils.Extensions
{
    public static class FloatExtensions
    {
        public static float AlterWithinRange(this float value, float lowerThreshold, float upperThreshold, float factor, bool increase)
        {
            if (increase)
            {
                if (!value.IsEqualTol(upperThreshold))
                    return value;

                return value + value * Time.deltaTime * factor;
            }

            if (!value.IsEqualTol(lowerThreshold))
                return value;

            return value - value * Time.deltaTime * factor;
        }

        public static bool IsEqualTol(this float x, float y, float tolerance = 1e-3f)
        {
            return Mathf.Abs(x - y) <= tolerance;
        }
    }
}
