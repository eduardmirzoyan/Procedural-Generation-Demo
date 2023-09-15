using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static Vector2Int[] DIRECTIONS = new Vector2Int[4] { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };

    UniformRandom uniformRandom;
    DiffusionLimitedAggregation diffusionLimitedAggregation;
    DrunkardsWalk drunkardsWalk;
    DijkstraMap dijkstraMap;

    private void Awake()
    {
        uniformRandom = new UniformRandom();
        diffusionLimitedAggregation = new DiffusionLimitedAggregation();
        drunkardsWalk = new DrunkardsWalk();
        dijkstraMap = new DijkstraMap();
    }

    public int[,] GenerateViaUniformRandom(int seed, int width, int height, int floorChance)
    {
        return uniformRandom.Generate(seed, width, height, floorChance);
    }

    public int[,] GenerateViaAggregation(int seed, int width, int height, int steps)
    {
        return diffusionLimitedAggregation.Generate(seed, width, height, steps);
    }

    public int[,] GenerateViaDrunkardsWalk(int seed, int width, int height, int cullChance, int numSteps)
    {
        return drunkardsWalk.Generate(seed, width, height, cullChance, numSteps);
    }

    public int[,] GenerateDijkstraMap(int[,] tiles, Vector2Int startLocation)
    {
        return dijkstraMap.Generate(tiles, startLocation);
    }


}
