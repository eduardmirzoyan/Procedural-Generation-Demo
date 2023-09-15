using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GeneratorAlgorithm { UniformRandom, DiffusionLimitedAggregation, DrunkardsWalk };
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MapDrawer mapDrawer;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GeneratorSelectionUI selectionUI;

    [Header("Data")]
    [SerializeField] private int seed;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int steps;
    [SerializeField] private int cullChance;

    private int[,] currentMap;
    private int[,] currentDijstra;
    private Vector3Int selectedLocation;

    [SerializeField] private GeneratorAlgorithm currentAlgorithm;
    private bool dijstraIsDrawn;

    private void Awake()
    {
        currentMap = null;
        selectedLocation = Vector3Int.back;
        currentAlgorithm = Enum.GetValues(typeof(GeneratorAlgorithm)).Cast<GeneratorAlgorithm>().Min();
        seed = -1;
    }

    private void Start()
    {
        selectionUI.SetAlgorithm(currentAlgorithm);
        selectionUI.UpdateWidthText(width);
        selectionUI.UpdateHeightText(height);
        selectionUI.UpdateCullChanceText(cullChance);
        selectionUI.UpdateNumStepsText(steps);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTile();
        }
    }

    private void SelectTile()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int selectedLocation = new Vector3Int(Mathf.FloorToInt(mouseWorldPosition.x), Mathf.FloorToInt(mouseWorldPosition.y));

        // Check bounds
        if (selectedLocation.x < 0 || selectedLocation.y < 0 || selectedLocation.x >= width || selectedLocation.y >= height)
            return;

        // Do nothing if we did not select a floor
        if (currentMap[selectedLocation.x, selectedLocation.y] != 1)
            return;

        if (this.selectedLocation == selectedLocation)
        {
            // Deselect
            mapDrawer.UnselectTile(selectedLocation);
            this.selectedLocation = Vector3Int.back;
        }
        else
        {
            // Unselect previous location
            if (this.selectedLocation != Vector3Int.back)
            {
                mapDrawer.UnselectTile(this.selectedLocation);
            }

            // Select new location
            mapDrawer.SelectTile(selectedLocation);
            this.selectedLocation = selectedLocation;
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
        mapDrawer.UnselectTile(selectedLocation);

        // Use random seed if input is 0
        int seedToUse = seed;
        if (seed == -1)
            seedToUse = UnityEngine.Random.Range(0, int.MaxValue);

        switch (currentAlgorithm)
        {
            case GeneratorAlgorithm.UniformRandom:
                currentMap = mapGenerator.GenerateViaUniformRandom(seedToUse, width, height, cullChance);
                break;
            case GeneratorAlgorithm.DiffusionLimitedAggregation:
                currentMap = mapGenerator.GenerateViaAggregation(seedToUse, width, height, steps);
                break;
            case GeneratorAlgorithm.DrunkardsWalk:
                currentMap = mapGenerator.GenerateViaDrunkardsWalk(seedToUse, width, height, cullChance, steps);
                break;
        }

        mapDrawer.DrawTiles(currentMap);
        cameraController.CenterCamera(width, height);
    }

    public void ToggleDijstraMap()
    {
        if (currentMap == null || selectedLocation == Vector3Int.back)
        {
            return;
        }

        if (!dijstraIsDrawn)
        {
            currentDijstra = mapGenerator.GenerateDijkstraMap(currentMap, (Vector2Int)selectedLocation);
            mapDrawer.DrawDijkstraMap(currentDijstra);
        }
        else
        {
            mapDrawer.ClearDijkstraMap();
        }

        dijstraIsDrawn = !dijstraIsDrawn;
    }
}
