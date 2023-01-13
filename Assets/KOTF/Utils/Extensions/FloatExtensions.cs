using UnityEngine;

namespace KOTF.Utils.Extensions
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Alters a <paramref name="value"/> by increasing or decreasing it based on <paramref name="increase"/> and
        /// clamps it between <paramref name="lowerThreshold"/> and <paramref name="upperThreshold"/> by a <paramref name="factor"/>.
        /// Mathf.Clamp was not used as it does not provide a tolerance parameter.
        /// </summary>
        /// <returns>The altered value.</returns>
        public static float AlterWithinRangeTol(this float value, float lowerThreshold, float upperThreshold, float factor, bool increase, float clampTolerance = 0.1f)
        {
            if (increase)
            {
                if (value.IsEqualTol(upperThreshold, clampTolerance))
                    return upperThreshold;

                return value + value * Time.deltaTime * factor;
            }

            if (value.IsEqualTol(lowerThreshold, clampTolerance))
                return lowerThreshold;

            return value - value * Time.deltaTime * factor;
        }

        public static bool IsEqualTol(this float x, float y, float tolerance = 1e-3f)
        {
            return Mathf.Abs(x - y) <= tolerance;
        }
    }
}
