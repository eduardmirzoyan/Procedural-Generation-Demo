using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformRandom
{
    public int[,] Generate(int seed, int width, int height, int cullPercentage)
    {
        System.Random rng = new(seed);
        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[width, height];

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (rng.Next(100) < cullPercentage)
                {
                    tiles[i, j] = 1;
                }
            }
        }

        return tiles;
    }
}
