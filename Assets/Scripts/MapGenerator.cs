using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeneratorAlgorithm { UniformRandom, DiffusionLimitedAggregation, DrunkardsWalk, IssacRoomGeneration, PerlinNoise, BinarySpacePartition, VoronoiDiagram };

public class MapGenerator : ScriptableObject
{
    public static Vector2Int[] DIRECTIONS = new Vector2Int[4] { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };

    public UniformRandom uniformRandom;
    public DiffusionLimitedAggregation diffusionLimitedAggregation;
    public DrunkardsWalk drunkardsWalk;
    public IssacRoomGeneration issacRoomGeneration;
    public PerlinNoise perlinNoise;
    public BinarySpacePartition binarySpacePartition;
    public VoronoiDiagram voronoiDiagram;

    public DijkstraMap dijkstraMap;
    public AStar aStar;

    public void Initialize()
    {
        uniformRandom = new UniformRandom();
        diffusionLimitedAggregation = new DiffusionLimitedAggregation();
        drunkardsWalk = new DrunkardsWalk();
        issacRoomGeneration = new IssacRoomGeneration();
        perlinNoise = new PerlinNoise();
        binarySpacePartition = new BinarySpacePartition();
        voronoiDiagram = new VoronoiDiagram();

        dijkstraMap = new DijkstraMap();
        aStar = new AStar();
    }
}
