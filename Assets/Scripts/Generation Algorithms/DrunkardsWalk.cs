using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkardsWalk
{
    public int[,] Generate(int seed, int width, int height, int cullPercentage, int numSteps)
    {
        System.Random rng = new(seed);

        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[width, height];

        int amountToCull = (int)(width * height * (cullPercentage / 100f));

        while (amountToCull > 0)
        {
            // Get random start position
            Vector2Int currentPosition = new Vector2Int(rng.Next(width), rng.Next(height));
            int stepCount = numSteps;
            while (stepCount > 0)
            {
                // Stop if out of bounds
                if (currentPosition.x < 0 || currentPosition.y < 0 || currentPosition.x >= width || currentPosition.y >= height)
                {
                    break;
                }

                // Remove wall at current position if needed
                if (tiles[currentPosition.x, currentPosition.y] == 0)
                {
                    tiles[currentPosition.x, currentPosition.y] = 1;
                    amountToCull--;
                }

                // Move in random direction (including backwards)
                Vector2Int randomDirection = MapGenerator.DIRECTIONS[rng.Next(4)];
                currentPosition += randomDirection;

                // Decrement steps
                stepCount--;
            }
        }

        return tiles;
    }
}

