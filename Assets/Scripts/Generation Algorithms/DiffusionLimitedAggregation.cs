using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionLimitedAggregation
{
    public int[,] Generate(int seed, int width, int height, int steps)
    {
        System.Random rng = new(seed);
        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[width, height];

        // set a initial seed, a cluster of tiles at the center of the room 
        Vector2Int mapCenter = new(
            width / 2,
            height / 2
        );

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2Int temp = mapCenter + new Vector2Int(i, j);
                tiles[temp.x, temp.y] = 1;
            }
        }

        // The diffusion process consists in firing from a random point in a random direction until we hit a previously allocated point
        for (int i = 0; i < steps;)
        {
            // choose a random point 
            Vector2Int originPoint = new Vector2Int(rng.Next(width), rng.Next(height));

            // move in a random direction until we hit something, or get out of bounds
            while (true)
            {
                // choose a direction
                Vector2Int randomDirection = MapGenerator.DIRECTIONS[rng.Next(4)];
                Vector2Int newPoint = originPoint + randomDirection;

                // Check out of bounds
                if (newPoint.x < 0 || newPoint.y < 0 || newPoint.x >= width || newPoint.y >= height)
                {
                    break;
                }

                // check if this is not a previously visited point
                // if it is a visited point, we can add the "origin_point" to our grid 
                if (tiles[newPoint.x, newPoint.y] == 1)
                {
                    tiles[originPoint.x, originPoint.y] = 1;
                    i++;
                    break;
                }

                // else we update origin point and continue moving our particle  
                originPoint = newPoint;
            }
        }

        return tiles;
    }
}
