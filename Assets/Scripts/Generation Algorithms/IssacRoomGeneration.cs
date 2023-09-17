using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IssacRoomGeneration
{
    public int[,] Generate(int seed, int mapWidth, int mapHeight, int roomWidth, int roomHeight, int numRooms)
    {
        System.Random rng = new(seed);

        // Calc max grid of the map
        int rows = mapWidth / roomWidth;
        int cols = mapHeight / roomHeight;

        int[,] grid = GenerateGrid(rows, cols, numRooms);

        int[,] tiles = GenerateTilesFromGrid(grid, mapWidth, mapHeight, roomWidth, roomHeight);

        return tiles;
    }

    private int[,] GenerateGrid(int rows, int cols, int numRooms)
    {
        // Start location is center of grid
        Vector2Int startLocation = new Vector2Int(rows / 2, cols / 2);

        // Queue of rooms to expand from
        Queue<Vector2Int> roomQueue;

        // 0 == Empty | 1 == Room
        int[,] grid;

        // Number of rooms to carve out
        int numRoomsLeft;
        do
        {
            // Init values
            grid = new int[rows, cols];
            roomQueue = new Queue<Vector2Int>();

            // Set up start
            grid[startLocation.x, startLocation.y] = 1;
            roomQueue.Enqueue(startLocation);
            numRoomsLeft = numRooms - 1;

            // Recurisvely generate grid
            BFS(roomQueue, grid, ref numRoomsLeft);

        } while (numRoomsLeft != 0);

        return grid;
    }

    private int[,] GenerateTilesFromGrid(int[,] grid, int mapWidth, int mapHeight, int roomWidth, int roomHeight)
    {
        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[mapWidth, mapHeight];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                // If room in grid
                if (grid[i, j] == 1)
                {
                    // Carve out room in tiles
                    for (int x = 0; x < roomWidth; x++)
                    {
                        for (int y = 0; y < roomHeight; y++)
                        {
                            // Set to floor
                            tiles[i * roomWidth + x, j * roomHeight + y] = 1;
                        }
                    }
                }
            }
        }

        return tiles;
    }

    private void BFS(Queue<Vector2Int> roomQueue, int[,] grid, ref int numRoomsLeft)
    {
        if (roomQueue.Count == 0 || numRoomsLeft <= 0)
        {
            return;
        }

        var location = roomQueue.Dequeue();

        // Check each valid neighbor
        foreach (var neighbor in GetValidNeighbors(location, grid))
        {
            // 50% chance of exploring room
            if (Random.Range(0, 100) > 50)
            {
                // Add to queue
                numRoomsLeft--;
                grid[neighbor.x, neighbor.y] = 1;
                roomQueue.Enqueue(neighbor);
            }
        }

        // Recursively call
        BFS(roomQueue, grid, ref numRoomsLeft);
    }

    private Vector2Int[] GetValidNeighbors(Vector2Int location, int[,] grid)
    {
        List<Vector2Int> validNeighbors = new List<Vector2Int>();

        foreach (var direction in MapGenerator.DIRECTIONS)
        {
            var newLocation = location + direction;

            // Skip if out of bounds
            if (OutOfBounds(newLocation, grid))
            {
                continue;
            }

            bool validNeighbor = true;
            foreach (var neighborDirection in MapGenerator.DIRECTIONS)
            {
                var newNewLocation = newLocation + neighborDirection;

                // Skip if out of bounds
                if (OutOfBounds(newNewLocation, grid))
                {
                    continue;
                }

                // If neighbor has another neighbor that isn't original
                if (grid[newNewLocation.x, newNewLocation.y] == 1 && newNewLocation != location)
                {
                    // Then invalid location
                    validNeighbor = false;
                }
            }

            // Add to valid neighbors
            if (validNeighbor)
            {
                validNeighbors.Add(newLocation);
            }
        }

        return validNeighbors.ToArray();
    }

    private bool OutOfBounds(Vector2Int location, int[,] grid)
    {
        return location.x < 0 || location.y < 0 || location.x >= grid.GetLength(0) || location.y >= grid.GetLength(1);
    }
}
