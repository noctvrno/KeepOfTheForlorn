using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOTF.Utils.Extensions
{
    public static class ComparableExtensions
    {
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable
        {
            if (value.CompareTo(min) < 0)
                return min;

            return value.CompareTo(max) > 0 ? max : value;
        }
    }
}
