using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MapDrawer mapDrawer;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GeneratorSelectionUI selectionUI;
    [SerializeField] private LegendUI legendUI;
    [SerializeField] private PathfindingUI pathfindingUI;

    [Header("Data")]
    [SerializeField] private int seed;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int steps;
    [SerializeField] private int cullChance;
    [SerializeField] private int roomWidth;
    [SerializeField] private int roomHeight;
    [SerializeField] private float threshold;
    [SerializeField] private float scale;

    private int[,] currentMap;
    private int[,] currentDijstra;
    private List<Vector2Int> currentAStar;
    [SerializeField] private Vector3Int selectedLocation;
    [SerializeField] private Vector3Int entranceLocation;
    [SerializeField] private Vector3Int exitLocation;

    [SerializeField] private GeneratorAlgorithm currentAlgorithm;
    private bool dijstraIsDrawn;
    private bool astarIsDrawn;

    private void Awake()
    {
        currentMap = null;
        selectedLocation = entranceLocation = exitLocation = Vector3Int.back;
        currentAlgorithm = Enum.GetValues(typeof(GeneratorAlgorithm)).Cast<GeneratorAlgorithm>().Min();
        seed = -1;

        mapGenerator = ScriptableObject.CreateInstance<MapGenerator>();
        mapGenerator.Initialize();

        CheckPathfindingState();
    }

    private void Start()
    {
        selectionUI.SetAlgorithm(currentAlgorithm);
        selectionUI.UpdateWidthText(width);
        selectionUI.UpdateHeightText(height);
        selectionUI.UpdateCullChanceText(cullChance);
        selectionUI.UpdateNumStepsText(steps);
        selectionUI.UpdateRoomWidth(roomWidth);
        selectionUI.UpdateRoomHeight(roomHeight);
        selectionUI.UpdateThreshold(threshold);
        selectionUI.UpdateScale(scale);
    }

    private void Update()
    {
        // Highlight any floor tile
        HighlightTile();

        if (Input.GetMouseButtonDown(0)) // Left Click places entrance
        {
            if (selectedLocation != Vector3Int.back)
            {
                // If exit is already here
                if (exitLocation == selectedLocation)
                {
                    // Do nothing
                }
                // De-select entrance if same location chosen
                else if (entranceLocation == selectedLocation)
                {
                    mapDrawer.SetEntrance(entranceLocation, false);
                    entranceLocation = Vector3Int.back;
                    UpateLegend(selectedLocation);
                }
                else
                {
                    // If we had a previous entrance, remove it first
                    if (entranceLocation != Vector3Int.back)
                    {
                        mapDrawer.SetEntrance(entranceLocation, false);
                    }

                    entranceLocation = selectedLocation;
                    mapDrawer.SetEntrance(entranceLocation, true);
                    UpateLegend(selectedLocation);
                }
            }

            CheckPathfindingState();
        }
        else if (Input.GetMouseButtonDown(1)) // Right click places exit
        {
            if (selectedLocation != Vector3Int.back)
            {
                // If entrance is already here
                if (entranceLocation == selectedLocation)
                {
                    // Do nothing
                }
                // De-select exit if same location chosen
                else if (exitLocation == selectedLocation)
                {
                    mapDrawer.SetExit(exitLocation, false);
                    exitLocation = Vector3Int.back;
                    UpateLegend(selectedLocation);
                }
                else
                {
                    // If we had a previous entrance, remove it first
                    if (exitLocation != Vector3Int.back)
                    {
                        mapDrawer.SetExit(exitLocation, false);
                    }

                    exitLocation = selectedLocation;
                    mapDrawer.SetExit(exitLocation, true);
                    UpateLegend(selectedLocation);
                }
            }

            CheckPathfindingState();
        }
    }

    private void CheckPathfindingState()
    {
        if (entranceLocation != Vector3Int.back)
        {
            pathfindingUI.SetDijstra(true);
        }
        else
        {
            pathfindingUI.SetDijstra(false);
        }

        if (entranceLocation != Vector3Int.back && exitLocation != Vector3Int.back)
        {
            pathfindingUI.SetAStar(true);
        }
        else
        {
            pathfindingUI.SetAStar(false);
        }
    }

    private void HighlightTile()
    {
        // Make sure map exits
        if (currentMap == null)
            return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int selectedLocation = new Vector3Int(Mathf.FloorToInt(mouseWorldPosition.x), Mathf.FloorToInt(mouseWorldPosition.y));

        // If same location, then dip
        if (this.selectedLocation == selectedLocation)
            return;

        // Check bounds
        if (selectedLocation.x < 0 || selectedLocation.y < 0 || selectedLocation.x >= width || selectedLocation.y >= height)
        {
            Unhighlight();

            return;
        }

        // Do nothing if we did not select a floor
        if (currentMap[selectedLocation.x, selectedLocation.y] != 1)
        {
            Unhighlight();

            return;
        }

        // Unselect previous location
        Unhighlight();

        // Select new location
        mapDrawer.SelectTile(selectedLocation);

        UpateLegend(selectedLocation);

        this.selectedLocation = selectedLocation;
    }

    private void UpateLegend(Vector3Int location)
    {
        // Show legend mesasge based on entrance/exit state

        string entranceText;
        // If you are hovering exit
        if (location == exitLocation)
        {
            entranceText = "-";
        }
        // Entrance is set
        else if (entranceLocation != Vector3Int.back)
        {
            // If you are hoving over entrance
            if (location == entranceLocation)
            {
                entranceText = "[Left Click] to Remove Entrance";
            }
            else
            {
                entranceText = "[Left Click] to Move Entrance";
            }

        }
        else
        {
            entranceText = "[Left Click] to Set Entrance";
        }

        string exitText;

        // If you are hovering exit
        if (location == entranceLocation)
        {
            exitText = "-";
        }
        // Exit is set
        else if (exitLocation != Vector3Int.back)
        {
            // If you are hoving over exit
            if (location == exitLocation)
            {
                exitText = "[Left Click] to Remove Exit";
            }
            else
            {
                exitText = "[Right Click] to Move Exit";
            }
        }
        else
        {
            exitText = "[Right Click] to Set Exit";
        }

        legendUI.Show(entranceText, exitText);
    }

    private void Unhighlight()
    {
        if (this.selectedLocation != Vector3Int.back)
        {
            mapDrawer.UnselectTile(this.selectedLocation);

            // Hide
            legendUI.Hide();

            this.selectedLocation = Vector3Int.back;
        }
    }

    public void SetSeed(string seed)
    {
        if (seed == "")
            this.seed = -1;
        else
            this.seed = int.Parse(seed, 0);
    }

    public void SetWidth(float width)
    {
        this.width = (int)width;
        selectionUI.UpdateWidthText(width);
    }

    public void SetHeight(float height)
    {
        this.height = (int)height;
        selectionUI.UpdateHeightText(height);
    }

    public void SetCullChance(float cullChance)
    {
        this.cullChance = (int)cullChance;
        selectionUI.UpdateCullChanceText(cullChance);
    }

    public void SetNumSteps(float steps)
    {
        this.steps = (int)steps;
        selectionUI.UpdateNumStepsText(steps);
    }

    public void SetRoomWidth(float roomWidth)
    {
        this.roomWidth = (int)roomWidth;
        selectionUI.UpdateRoomWidth(roomWidth);
    }

    public void SetRoomHeight(float roomHeight)
    {
        this.roomHeight = (int)roomHeight;
        selectionUI.UpdateRoomHeight(roomHeight);
    }

    public void SetThreshold(float threshold)
    {
        this.threshold = threshold;
        selectionUI.UpdateThreshold(threshold);
    }

    public void SetScale(float scale)
    {
        this.scale = scale;
        selectionUI.UpdateScale(scale);
    }

    public void NextAlgorithm()
    {
        // If we are at the top algorthim
        var max = Enum.GetValues(typeof(GeneratorAlgorithm)).Cast<GeneratorAlgorithm>().Max();
        if (currentAlgorithm == max)
        {
            // Reset back down to min
            var min = Enum.GetValues(typeof(GeneratorAlgorithm)).Cast<GeneratorAlgorithm>().Min();
            currentAlgorithm = min;
        }
        else
        {
            currentAlgorithm++;
        }

        // Update algo
        selectionUI.SetAlgorithm(currentAlgorithm);
    }

    public void PreviousAlgorithm()
    {
        // If we are at the bottom
        var min = Enum.GetValues(typeof(GeneratorAlgorithm)).Cast<GeneratorAlgorithm>().Min();
        if (currentAlgorithm == min)
        {
            // Reset back up to max
            var max = Enum.GetValues(typeof(GeneratorAlgorithm)).Cast<GeneratorAlgorithm>().Max();
            currentAlgorithm = max;
        }
        else
        {
            currentAlgorithm--;
        }

        // Update algo
        selectionUI.SetAlgorithm(currentAlgorithm);
    }

    public void GenerateMapWithCurrentAlgorithm()
    {
        CleanMap();

        // Use random seed if input is 0
        int seedToUse = seed;
        if (seed == -1)
            seedToUse = UnityEngine.Random.Range(0, int.MaxValue);

        switch (currentAlgorithm)
        {
            case GeneratorAlgorithm.UniformRandom:
                currentMap = mapGenerator.uniformRandom.Generate(seedToUse, width, height, cullChance);
                break;
            case GeneratorAlgorithm.DiffusionLimitedAggregation:
                currentMap = mapGenerator.diffusionLimitedAggregation.Generate(seedToUse, width, height, steps);
                break;
            case GeneratorAlgorithm.DrunkardsWalk:
                currentMap = mapGenerator.drunkardsWalk.Generate(seedToUse, width, height, cullChance, steps);
                break;
            case GeneratorAlgorithm.IssacRoomGeneration:
                currentMap = mapGenerator.issacRoomGeneration.Generate(seedToUse, width, height, roomWidth, roomHeight, steps);
                break;
            case GeneratorAlgorithm.PerlinNoise:
                currentMap = mapGenerator.perlinNoise.Generate(seedToUse, width, height, threshold, scale);
                break;
        }

        mapDrawer.DrawTiles(currentMap);
        cameraController.CenterCamera(width, height);
    }

    public void ToggleDijstraMap()
    {
        // If we don't have an active map or entrance was not placed
        if (currentMap == null || entranceLocation == Vector3Int.back)
        {
            return;
        }

        // If we already have astar
        if (astarIsDrawn)
        {
            currentAStar = null;
            mapDrawer.ClearAStar();
            astarIsDrawn = false;
        }

        if (!dijstraIsDrawn)
        {
            currentDijstra = mapGenerator.dijkstraMap.Generate(currentMap, (Vector2Int)entranceLocation);
            mapDrawer.DrawDijkstraMap(currentDijstra);
        }
        else
        {
            currentDijstra = null;
            mapDrawer.ClearDijkstraMap();
        }

        dijstraIsDrawn = !dijstraIsDrawn;
    }

    public void ToggleAStarPath()
    {
        // If we don't have an active map or entrance was not placed
        if (currentMap == null || entranceLocation == Vector3Int.back)
        {
            return;
        }

        // If we already have astar
        if (dijstraIsDrawn)
        {
            currentDijstra = null;
            mapDrawer.ClearDijkstraMap();
            dijstraIsDrawn = false;
        }

        if (!astarIsDrawn)
        {
            currentAStar = mapGenerator.aStar.FindShortestPath((Vector2Int)entranceLocation, (Vector2Int)exitLocation, currentMap);
            mapDrawer.DrawAStar(currentAStar);
        }
        else
        {
            currentAStar = null;
            mapDrawer.ClearAStar();
        }

        astarIsDrawn = !astarIsDrawn;
    }

    private void CleanMap()
    {
        // Clear Dijstra
        if (dijstraIsDrawn)
        {
            ToggleDijstraMap();
        }

        if (astarIsDrawn)
        {
            ToggleAStarPath();
        }

        // Clear highlight
        if (selectedLocation != Vector3Int.back)
        {
            mapDrawer.UnselectTile(selectedLocation);
        }

        // Clear entrance
        if (entranceLocation != Vector3Int.back)
        {
            mapDrawer.SetEntrance(entranceLocation, false);
            entranceLocation = Vector3Int.back;
        }

        // Clear exit
        if (exitLocation != Vector3Int.back)
        {
            mapDrawer.SetExit(exitLocation, false);
            exitLocation = Vector3Int.back;
        }

        CheckPathfindingState();
    }
}
