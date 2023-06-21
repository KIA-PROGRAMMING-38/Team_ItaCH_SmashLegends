using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Method
{
    public static class Method
    {
        public static float CalculateAbsolute(float someFloat)
        {
            if (someFloat > 0)
            {
                return someFloat;
            }
            else
            {
                return -someFloat;
            }
        }

        public static bool IsPositive(float someFloat)
        {
            if (someFloat > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}