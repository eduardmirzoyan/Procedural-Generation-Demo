using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// References Here:
// https://www.youtube.com/watch?v=-fYI_5hQcOI

public class VoronoiDiagram
{

    // NON-FUNCTIONAL

    public int[,] Generate(int seed, int mapWidth, int mapHeight, int gridSize)
    {
        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[mapWidth, mapHeight];
        int pixelsPerCell = mapWidth / gridSize;

        Vector2Int[,] positionGrid = new Vector2Int[gridSize, gridSize];
        Color[,] colorGrid = new Color[mapWidth, mapHeight];

        Color[,] map = new Color[mapWidth, mapHeight];

        // Generate points in grid
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                // Generate random offset
                int randX = Random.Range(0, pixelsPerCell);
                int randY = Random.Range(0, pixelsPerCell);

                // Random position within square and color
                Vector2Int position = new Vector2Int(i * pixelsPerCell + randX, j * pixelsPerCell + randY);
                positionGrid[i, j] = position;

                // Color randomColor = Random.ColorHSV();

                // Testing
                tiles[position.x, position.y] = 1;
            }
        }

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                int gridX = i / pixelsPerCell;
                int gridY = j / pixelsPerCell;

                float nearestDistance = float.MaxValue;
                Vector2Int nearestPoint = new Vector2Int();

                for (int a = -1; a <= 1; a++)
                {
                    for (int b = -1; b <= 1; b++)
                    {
                        int x = gridX + a;
                        int y = gridY + b;

                        if (x < 0 || y < 0 || x >= gridSize || y >= gridSize)
                            continue;

                        float distance = Vector2Int.Distance(new Vector2Int(i, j), positionGrid[x, y]);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestPoint = new Vector2Int(x, y);
                        }
                    }
                }

                // Get color of nearest point
                Color color = colorGrid[nearestPoint.x, nearestPoint.y];
                map[i, j] = color;
            }
        }

        // Return map?

        return tiles;
    }
}
