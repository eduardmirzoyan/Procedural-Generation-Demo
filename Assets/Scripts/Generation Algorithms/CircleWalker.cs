using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleWalker
{
    private Vector2 radiusRange;
    private Vector2 lifetimeRange;
    private Vector2 angleRange;
    private int splitLimit;

    private class Walker
    {
        public Vector2 position;
        public Vector2 direction;
        public float maxRadius;
        public float radius;
        public float maxLifetime;
        public float lifetime;

        public List<Walker> Split(float rradius, float lifetime, float angle)
        {
            List<Walker> walkers = new List<Walker>();

            // Convert the angle offset from degrees to radians
            // float angle = 30f;
            float angleOffsetRadians = angle * Mathf.Deg2Rad;

            // Calculate the cosine and sine of the angle offset
            float cosAngle = Mathf.Cos(angleOffsetRadians);
            float sinAngle = Mathf.Sin(angleOffsetRadians);

            // Calculate the rotated directions
            Vector2 directionA = new Vector2(
                direction.x * cosAngle - direction.y * sinAngle,
                direction.x * sinAngle + direction.y * cosAngle
            );

            Vector2 directionB = new Vector2(
                direction.x * cosAngle + direction.y * sinAngle,
                -direction.x * sinAngle + direction.y * cosAngle
            );

            for (int i = 0; i < 2; i++)
            {
                Walker walker = new Walker();
                walker.position = position;

                walker.maxRadius = rradius;
                walker.radius = rradius;

                walker.maxLifetime = lifetime;
                walker.lifetime = lifetime;

                // Split direction
                if (i == 0)
                {
                    walker.direction = directionA;
                }
                else
                {
                    walker.direction = directionB;
                }

                walkers.Add(walker);
            }

            return walkers;
        }
    }

    public CircleWalker(Vector2 radiusRange, Vector2 lifetimeRange, Vector2 angleRange, int splitLimit)
    {
        this.radiusRange = radiusRange;
        this.lifetimeRange = lifetimeRange;
        this.angleRange = angleRange;
        this.splitLimit = splitLimit;
    }

    public int[,] Generate(int radius, int walkerCount, float walkerSize, float walkerDistance, float walkerSplitAngle)
    {
        // 0 - Empty | 1 - Contains Path
        int[,] grid = new int[radius * 2 + 1, radius * 2 + 1];

        Vector2 centerPosition = new Vector2(radius, radius);

        List<Walker> walkers = new List<Walker>();

        // Create multiple walkers in the center of map and project them outward
        float angleIncrement = 360f / walkerCount;
        for (int i = 0; i < walkerCount; i++)
        {
            Walker walker = new Walker();
            walker.position = centerPosition;

            float angle = i * angleIncrement;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
            walker.direction = direction;

            walker.maxRadius = walkerSize;
            walker.radius = walker.maxRadius;

            walker.maxLifetime = walkerDistance;
            walker.lifetime = walker.maxLifetime;

            walkers.Add(walker);
        }

        while (walkers.Count > 0)
        {
            // Debug.Log(walkers.Count);

            int index = 0;
            while (index < walkers.Count)
            {
                var walker = walkers[index];

                // Clear our space
                for (float i = -walker.radius; i <= walker.radius; i++)
                {
                    for (float j = -walker.radius; j <= walker.radius; j++)
                    {
                        var worldPosition = walker.position + new Vector2(i, j);
                        Vector2Int gridPosition = Vector2Int.RoundToInt(worldPosition);
                        if (IsWithinBounds(gridPosition, grid) && IsInsideCircle(worldPosition, walker.position, walker.radius))
                        {
                            grid[gridPosition.x, gridPosition.y] = 1;
                        }
                    }
                }

                // Increment position
                walker.position += walker.direction;

                // Check if valid
                if (Vector3.Distance(walker.position, centerPosition) > radius)
                {
                    // Remove walker
                    walkers.RemoveAt(index);

                    // Skip
                    continue;
                }

                // Lerp radius over lifetime
                float ratio = walker.lifetime / walker.maxLifetime; // 1 -> 0
                if (ratio > 0.5f)
                {
                    // Decrease size
                    walker.radius = Mathf.Lerp(0, walker.maxRadius, ratio);
                }
                else
                {
                    // Increase size
                    walker.radius = Mathf.Lerp(walker.maxRadius, 0, ratio);
                }

                // Decrease lifetime
                walker.lifetime -= 1f;
                if (walker.lifetime <= 0)
                {
                    // Make sure we don't have too many branches
                    if (walkers.Count > splitLimit)
                        return grid;

                    // Generate random new values
                    float randomRadius = Random.Range(radiusRange.x, radiusRange.y);
                    float randomLifetime = Random.Range(lifetimeRange.x, lifetimeRange.y);
                    float randomAngle = Random.Range(angleRange.x, angleRange.y);

                    // Split walker
                    walkers.AddRange(walker.Split(randomRadius, randomLifetime, randomAngle));

                    // Remove walker
                    walkers.RemoveAt(index);

                    // Skip
                    continue;
                }

                // Increment index
                index++;
            }
        }

        return grid;
    }

    private bool IsWithinBounds(Vector2Int position, int[,] grid)
    {
        return position.x >= 0 && position.x < grid.GetLength(0) && position.y >= 0 && position.y < grid.GetLength(1);
    }

    private bool IsInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        return Mathf.Pow(point.x - center.x, 2) + Mathf.Pow(point.y - center.y, 2) < Mathf.Pow(radius, 2);
    }
}
