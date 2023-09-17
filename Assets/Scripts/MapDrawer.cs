using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MapDrawer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap roomTilemap;
    [SerializeField] private Tilemap decorTilemap;
    [SerializeField] private Tilemap selectTilemap;
    [SerializeField] private Tilemap dijkstraTilemap;
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile wallTile;
    [SerializeField] private Tile entranceTile;
    [SerializeField] private Tile exitTile;
    [SerializeField] private GameObject numberPrefab;
    [SerializeField] private Tile selectTile;

    [Header("Data")]
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;

    private List<NumberTile> numberTiles;

    private void Awake()
    {
        numberTiles = new List<NumberTile>();
    }

    public void SelectTile(Vector3Int location)
    {
        selectTilemap.SetTile(location, selectTile);
    }

    public void UnselectTile(Vector3Int location)
    {
        selectTilemap.SetTile(location, null);
    }

    public void SetEntrance(Vector3Int location, bool active)
    {
        if (active)
            decorTilemap.SetTile(location, entranceTile);
        else
            decorTilemap.SetTile(location, null);
    }

    public void SetExit(Vector3Int location, bool active)
    {
        if (active)
            decorTilemap.SetTile(location, exitTile);
        else
            decorTilemap.SetTile(location, null);
    }

    public void DrawTiles(int[,] tiles)
    {
        roomTilemap.ClearAllTiles();

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                switch (tiles[i, j])
                {
                    case 0:
                        roomTilemap.SetTile(new Vector3Int(i, j), wallTile);
                        break;
                    case 1:
                        roomTilemap.SetTile(new Vector3Int(i, j), floorTile);
                        break;
                    case 2:
                        roomTilemap.SetTile(new Vector3Int(i, j), entranceTile);
                        break;
                    case 3:
                        roomTilemap.SetTile(new Vector3Int(i, j), exitTile);
                        break;
                }
            }
        }
    }

    public void DrawDijkstraMap(int[,] map)
    {
        float maxValue = 0;

        // Label map
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                var location = new Vector3Int(i, j);
                var value = map[i, j];
                var position = roomTilemap.GetCellCenterWorld(location);
                var numberTile = Instantiate(numberPrefab, position, Quaternion.identity, roomTilemap.transform).GetComponent<NumberTile>();
                numberTile.SetValue(value);
                numberTiles.Add(numberTile);

                maxValue = Mathf.Max(maxValue, value);
            }
        }

        // Color map
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                var location = new Vector3Int(i, j);
                var value = map[i, j];

                if (value >= 0)
                {
                    Color color = Color.Lerp(minColor, maxColor, value / maxValue);
                    dijkstraTilemap.SetTile(location, selectTile);
                    dijkstraTilemap.SetColor(location, color);
                }
            }
        }
    }

    public void ClearDijkstraMap()
    {
        dijkstraTilemap.ClearAllTiles();

        foreach (var numberTile in numberTiles)
        {
            Destroy(numberTile.gameObject);
        }
        numberTiles.Clear();
    }

    public void DrawAStar(List<Vector2Int> path)
    {
        int count = 0;
        foreach (var point in path)
        {
            var position = roomTilemap.GetCellCenterWorld((Vector3Int)point);

            dijkstraTilemap.SetTile((Vector3Int)point, selectTile);
            dijkstraTilemap.SetColor((Vector3Int)point, Color.white);

            var numberTile = Instantiate(numberPrefab, position, Quaternion.identity, roomTilemap.transform).GetComponent<NumberTile>();
            numberTile.SetValue(count);

            numberTiles.Add(numberTile);
            count++;
        }
    }

    public void ClearAStar()
    {
        dijkstraTilemap.ClearAllTiles();

        foreach (var numberTile in numberTiles)
        {
            Destroy(numberTile.gameObject);
        }
        numberTiles.Clear();
    }
}
