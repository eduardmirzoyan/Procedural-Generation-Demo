using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar
{
    private static Vector2Int[] DIRECTIONS = new Vector2Int[4] { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };

    private class Node
    {
        public Vector2Int location;

        public int G;
        public int H;
        public int F { get { return G + H; } }
        public Node previous;

        public Node(Vector2Int location)
        {
            this.location = location;
            previous = null;
        }
    }

    public List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int end, int[,] tiles)
    {
        // TODO

        return new List<Vector2Int>();
    }

    private List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        // Find path using A*
        var open = new List<Node>(); // Makes sure no copies are kept, idealy should be prio queue
        var closed = new List<Node>();

        var startNode = new Node(start);

        // Start from start
        open.Add(startNode);

        while (open.Count > 0)
        {
            // Sort by F value then get first item
            var currentNode = open.OrderBy(node => node.F).First();

            open.Remove(currentNode);
            closed.Add(currentNode);

            if (currentNode.location == end)
            {
                // Debug
                // Debug.Log(open.Count);
                // string debug = "Open: ";
                // foreach (var location in open)
                // {
                //     debug += location.location + " ";
                // }
                // Debug.Log(debug);

                // Return finalized path
                return GetFinalPath(startNode, currentNode);
            }


            // Search neighbors
            foreach (var direction in DIRECTIONS)
            {
                var neighbor = currentNode.location + direction;

                // Make node
                var neighborNode = new Node(neighbor);

                // Skip if closed contains node
                if (closed.Any(node => node.location == neighbor))
                {
                    continue;
                }

                // Update values
                neighborNode.G = ManhattanDistance(start, neighbor); // G
                neighborNode.H = ManhattanDistance(end, neighbor); // H

                // Update previous
                neighborNode.previous = currentNode;

                // Make sure no copies exist
                if (!open.Any(node => node.location == neighbor))
                    open.Add(neighborNode);
            }
        }

        // Return empty list
        return new List<Vector2Int>();
    }

    private List<Vector2Int> GetFinalPath(Node start, Node end)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        var current = end;

        while (current != null)
        {
            // Add location
            result.Add(current.location);
            // Increment
            current = current.previous;
        }

        // Reverse list
        result.Reverse();

        return result;
    }

    private int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
