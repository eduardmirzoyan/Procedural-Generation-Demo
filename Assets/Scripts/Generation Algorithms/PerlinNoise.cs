using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise
{
    public int[,] Generate(int seed, int mapWidth, int mapHeight, float threshold, float scale)
    {
        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[mapWidth, mapHeight];
        System.Random rng = new(seed);
        float offset = rng.Next(100000);

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {

                float noise = GetNoiseAtLocation(i, j, scale, offset);
                if (noise > threshold)
                {
                    // Set floor here
                    tiles[i, j] = 1;
                }
            }
        }

        return tiles;
    }

    private float GetNoiseAtLocation(int x, int y, float scale, float offset)
    {

        return Mathf.PerlinNoise(x / scale + offset, y / scale + offset);
    }
}
