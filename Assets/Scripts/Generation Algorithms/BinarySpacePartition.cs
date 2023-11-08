using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BinarySpacePartition
{

    // NON-FUNCTIONAL

    private struct Room
    {
        public Vector2Int position;
        public Vector2Int size;

        public Room(Vector2Int position, Vector2Int size)
        {
            this.position = position;
            this.size = size;
        }

        public Room(int x, int y, int width, int height)
        {
            position = new Vector2Int(x, y);
            size = new Vector2Int(width, height);
        }
    };

    public int[,] Generate(int seed, int width, int height, int roomWidth, int roomHeight)
    {
        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[width, height];

        // Get list of rooms from map dimensions
        List<Room> rooms = Partition(width, height, roomWidth, roomHeight, seed);
        Debug.Log(rooms.Count);

        // Convert rooms into tiles
        foreach (var room in rooms)
        {
            for (int i = 0; i < room.size.x - 1; i++)
            {
                for (int j = 0; j < room.size.y - 1; j++)
                {
                    int x = room.position.x + i;
                    int y = room.position.y + j;

                    tiles[x, y] = 1;
                }
            }
        }

        return tiles;
    }

    private List<Room> Partition(int startWidth, int startHeight, int minWidth, int minHieght, int seed)
    {
        System.Random rng = new(seed);

        Queue<Room> roomQueue = new Queue<Room>();
        List<Room> roomList = new List<Room>();
        Room startRoom = new Room(0, 0, startWidth, startHeight);
        roomQueue.Enqueue(startRoom);

        while (roomQueue.Count > 0)
        {
            var room = roomQueue.Dequeue();

            // If room big enough to split
            if (room.size.x > minWidth || room.size.y > minHieght)
            {
                // If either
                if (room.size.x > minWidth && room.size.y > minHieght)
                {
                    // 50/50 split
                    if (rng.NextDouble() < 0.5f)
                    {
                        PartitionHorizontally(room, roomQueue);
                    }
                    else
                    {
                        PartitionVertically(room, roomQueue);
                    }
                }
                else if (room.size.x > minWidth)
                {
                    if (rng.NextDouble() < 0.5f)
                        PartitionVertically(room, roomQueue);
                    else
                        roomList.Add(room);
                }
                else if (room.size.y > minHieght)
                {
                    if (rng.NextDouble() < 0.5f)
                        PartitionHorizontally(room, roomQueue);
                    else
                        roomList.Add(room);
                }
            }
            else
            {
                roomList.Add(room);
            }
        }

        return roomList;
    }

    private void PartitionHorizontally(Room roomToSplit, Queue<Room> roomQueue)
    {
        Vector2Int position = roomToSplit.position;
        Vector2Int size = roomToSplit.size;
        size.y /= 2;
        Room leftRoom = new Room(position, size);

        position.y += size.y;
        Room rightRoom = new Room(position, size);

        roomQueue.Enqueue(leftRoom);
        roomQueue.Enqueue(rightRoom);
    }

    private void PartitionVertically(Room roomToSplit, Queue<Room> roomQueue)
    {
        Vector2Int position = roomToSplit.position;
        Vector2Int size = roomToSplit.size;
        size.x /= 2;
        Room leftRoom = new Room(position, size);

        position.x += size.x;
        Room rightRoom = new Room(position, size);

        roomQueue.Enqueue(leftRoom);
        roomQueue.Enqueue(rightRoom);
    }
}
