using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartition
{
    public int[,] Generate(int seed, int width, int height, int cullPercentage, int numSteps)
    {
        System.Random rng = new(seed);

        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[width, height];

        // TODO

        return tiles;
    }
}
