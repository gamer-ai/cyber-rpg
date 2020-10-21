using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    public const int MAX_WIDTH_OF_ROOM = 100;
    public const int MAX_HEIGHT_OF_ROOM = 100;

    // Start is called before the first frame update
    void Start()
    {
        Tilemaps = GetComponentsInChildren<Tilemap>();
        Grid[] grids = GetComponentsInChildren<Grid>();
        GridsDict = new Dictionary<string, Grid>();
        foreach (Grid grid in grids)
        {
            GridsDict.Add(grid.name, grid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Tilemap tilemap in Tilemaps)
        {
            if (tilemap.name == "Ground")
            {
                Vector3 worldCenter =
                    tilemap.CellToWorld(
                        new Vector3Int(
                            Mathf.RoundToInt(tilemap.cellBounds.center.x),
                            Mathf.RoundToInt(tilemap.cellBounds.center.y), 0));
                Bounds bounds = new Bounds(worldCenter,
                                    tilemap.cellBounds.size);
                Bounds worldBounds = bounds;
                if (worldBounds.Contains(target.position))
                {

                    string roomName = tilemap.transform.parent.name;
                    if (CurrentGrid.Key != roomName)
                    {
                        if (!GridsDict.ContainsKey(roomName))
                        {
                            Debug.LogErrorFormat(
                                "Didn't find room with name: {0}", roomName);
                        }
                        else
                        {
                            CurrentGrid =
                                new KeyValuePair<string, PathFind.Grid>(
                                    roomName, BuildGrid(GridsDict[roomName]));
                        }
                    }
                }
            }
        }
    }

    private PathFind.Grid BuildGrid(Grid grid)
    {
        Tilemap ground = grid.transform.Find("Ground").GetComponent<Tilemap>();
        Tilemap collision =
            grid.transform.Find("Collision").GetComponent<Tilemap>();
        bool[,] movableTiles = new bool[MAX_WIDTH_OF_ROOM, MAX_HEIGHT_OF_ROOM];
        for (int x = ground.cellBounds.min.x; x < ground.cellBounds.max.x; ++x)
        {
            for (int y = ground.cellBounds.min.y; y < ground.cellBounds.max.y;
                ++y)
            {
                int r = x - ground.cellBounds.min.x;
                int c = y - ground.cellBounds.min.y;
                // All cell should be unreachable by default.
                movableTiles[r, c] = false;
                if (ground.HasTile(new Vector3Int(x, y, 0)))
                {
                    movableTiles[r, c] = true;
                }
                if (collision.HasTile(new Vector3Int(x, y, 0)))
                {
                    movableTiles[r, c] = false;
                }
            }
        }
        int width = ground.cellBounds.size.x;
        int height = ground.cellBounds.size.y;
        PathFind.Grid pathGrid = new PathFind.Grid(width, height, movableTiles);
        return pathGrid;
    }

    public Vector2Int? WorldPosToGridPos(Vector3 worldPos)
    {
        string roomName = CurrentGrid.Key;
        if (!GridsDict.ContainsKey(roomName))
        {
            Debug.LogErrorFormat(
                "Didn't find room with name: {0}", roomName);
            return null;
        }
        Tilemap ground = GridsDict[roomName].transform.Find("Ground")
            .GetComponent<Tilemap>();
        Vector3Int cellPosInt = ground.WorldToCell(worldPos);
        if (!ground.cellBounds.Contains(cellPosInt))
        {
            Debug.LogErrorFormat("World position {0} is not in current grid " +
                "cell bounds {1}.", cellPosInt, ground.cellBounds);
            return null;
        }
        // Add xy offset to align with 2D arrary start point.
        return new Vector2Int(cellPosInt.x - ground.cellBounds.min.x,
            cellPosInt.y - ground.cellBounds.min.y);
    }

    public Vector3? GridPosToWorldPos(Vector2Int gridPos)
    {
        string roomName = CurrentGrid.Key;
        if (!GridsDict.ContainsKey(roomName))
        {
            Debug.LogErrorFormat(
                "Didn't find room with name: {0}", roomName);
            return null;
        }
        Tilemap ground = GridsDict[roomName].transform.Find("Ground")
            .GetComponent<Tilemap>();
        Vector3 worldPos = ground.CellToWorld(
            new Vector3Int(gridPos.x + ground.cellBounds.min.x,
            gridPos.y + ground.cellBounds.min.y, 0));
        // Move position to cell center.
        worldPos += new Vector3(ground.cellSize.x / 2.0f, ground.cellSize.y / 2.0f, 0.0f);
        return worldPos;
    }

    public Tilemap[] Tilemaps { get; private set; }
    public Dictionary<string, Grid> GridsDict { get; private set; }
    public KeyValuePair<string, PathFind.Grid> CurrentGrid { get; private set; }
    public Transform target;
}