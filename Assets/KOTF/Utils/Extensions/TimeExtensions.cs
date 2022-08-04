using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KOTF.Utils.Extensions
{
    public static class TimeExtensions
    {
        public static Vector3 ToDeltaTime(this Vector3 value)
        {
            return value * Time.deltaTime;
        }
    }
}
