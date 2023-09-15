using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDrawer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap roomTilemap;
    [SerializeField] private Tilemap selectTilemap;
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile wallTile;
    [SerializeField] private Tile entranceTile;
    [SerializeField] private Tile exitTile;
    [SerializeField] private GameObject numberPrefab;
    [SerializeField] private Tile selectTile;

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
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                var position = roomTilemap.GetCellCenterWorld(new Vector3Int(i, j));
                var numberTile = Instantiate(numberPrefab, position, Quaternion.identity, roomTilemap.transform).GetComponent<NumberTile>();
                numberTile.SetValue(map[i, j]);
                numberTiles.Add(numberTile);
            }
        }
    }

    public void ClearDijkstraMap()
    {
        foreach (var numberTile in numberTiles)
        {
            Destroy(numberTile.gameObject);
        }
        numberTiles.Clear();
    }
}
