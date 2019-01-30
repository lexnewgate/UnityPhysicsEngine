using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixedMath
{
    internal static class HashCodeHelper
    {
       internal static int CombineHashCodes(int h1,int h2)
       {
            return (((h1 << 5) + h1) ^ h2);
       } 
    }
}