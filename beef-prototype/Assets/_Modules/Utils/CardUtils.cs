using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Utils
{
    public static class CardUtils
    {
        private static System.Random rng = new System.Random();

        //From: https://stackoverflow.com/questions/273313/randomize-a-listt
        //Based on Fisher–Yates shuffle
        //NOTE(BEN): Likely not sufficiently random, should use something like xoshiro256++ instead 2024-01-28
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
