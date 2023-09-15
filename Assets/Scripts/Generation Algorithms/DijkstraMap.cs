using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraMap
{
    private int[,] tiles;
    private int width;
    private int height;

    public int[,] Generate(int[,] tiles, Vector2Int startLocation)
    {
        this.tiles = tiles;
        width = tiles.GetLength(0);
        height = tiles.GetLength(1);

        // Create a grid that stores the depth of each tile from start point
        // Initialize to -1
        int[,] map = new int[width, height];
        for (int i = 0; i < width * height; i++) map[i % width, i / width] = -1;

        BFS(map, startLocation);

        return map;
    }

    private void BFS(int[,] map, Vector2Int startLocation)
    {
        // Hold a list of visited tiles
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];

        // Queue of tiles to explore
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        // Add the start location
        queue.Enqueue(startLocation);
        map[startLocation.x, startLocation.y] = 0;
        visited[startLocation.x, startLocation.y] = true;

        BFSSearch(queue, visited, map);
    }

    private void BFSSearch(Queue<Vector2Int> queue, bool[,] visited, int[,] map)
    {
        if (queue.Count == 0)
        {
            return;
        }

        var location = queue.Dequeue();
        var depth = map[location.x, location.y];

        foreach (var direction in MapGenerator.DIRECTIONS)
        {
            var newLocation = location + direction;

            // location.x < 0 || location.x >= width || location.y < 0 || location.y >= height
            if (!OutOfBounds(newLocation) && !visited[newLocation.x, newLocation.y])
            {
                // If location is a wall
                if (tiles[newLocation.x, newLocation.y] == 0)
                {
                    // Do not explore
                    map[newLocation.x, newLocation.y] = -1;
                }
                else
                {
                    // Explroe the tile
                    queue.Enqueue(newLocation);
                    map[newLocation.x, newLocation.y] = depth + 1;
                }

                // Mark as visited
                visited[newLocation.x, newLocation.y] = true;
            }
        }

        BFSSearch(queue, visited, map);
    }

    private bool OutOfBounds(Vector2Int location)
    {
        return location.x < 0 || location.x >= width || location.y < 0 || location.y >= height;
    }
}
