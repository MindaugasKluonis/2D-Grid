using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public Tilemap baseTilemap;
    private int posZ;

    private Dictionary<Vector3, GridTile> levelTiles;
    private GridTile _currentTile;
    private Pathfinding _pathfinding;
    private LineController _lineController;

    private void Awake()
    {
        posZ = (int)transform.position.z;
        Init();
        _pathfinding = new Pathfinding(levelTiles);
        _lineController = new LineController();

    }

    private void Init()
    {
        levelTiles = new Dictionary<Vector3, GridTile>();

        foreach (Vector3Int pos in baseTilemap.cellBounds.allPositionsWithin)
        {
            var localLocation = new Vector3Int(pos.x, pos.y, pos.z);

            if (!baseTilemap.HasTile(localLocation)) continue; //to check any holes in the tilemap

            var tile = new GridTile
            {
                LocalLocation = localLocation,
                WorldLocation = baseTilemap.CellToWorld(localLocation),
                Tilemap = baseTilemap,
                Tile = baseTilemap.GetTile<TileScriptable>(localLocation)
            };

            levelTiles.Add(tile.WorldLocation, tile);
        }

    }

    public List<GridTile> FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        GridTile startTile = GetGridTile(startPosition);
        GridTile targetTile = GetGridTile(targetPosition);

        return _pathfinding.FindPath(startTile, targetTile);
    }


    public GridTile GetGridTile(Vector3 position)
    {
        var positionInt = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), posZ);

        if (levelTiles.TryGetValue(positionInt, out GridTile tile))
        {
            return tile;
        }

        return null;
    }
}

//if (Input.GetMouseButtonDown(0))
//{
//    Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//    var locInt = new Vector3Int(Mathf.FloorToInt(loc.x), Mathf.FloorToInt(loc.y), (int)posZ);

//    if (levelTiles.TryGetValue(locInt, out _currentTile))
//    {
//        _currentTile.Tilemap.SetTileFlags(_currentTile.LocalLocation, TileFlags.None);
//        _currentTile.Tilemap.SetColor(_currentTile.LocalLocation, Color.red);
//    }
//}

//--------------------------------------------------------------------------------------------
//if (Input.GetMouseButtonDown(0))
//{
//    Stopwatch stopwatch = new Stopwatch();
//    stopwatch.Start();

//    Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//    var locInt = new Vector3Int(Mathf.FloorToInt(loc.x), Mathf.FloorToInt(loc.y), 0);

//    if (levelTiles.TryGetValue(locInt, out _currentTile))
//    {
//        List<GridTile> path = FindPath(new Vector3(0, 0, 0), locInt);

//        foreach (GridTile tile in path)
//        {
//            UnityEngine.Debug.Log(8);
//            tile.Tilemap.SetTileFlags(tile.LocalLocation, TileFlags.None);
//            tile.Tilemap.SetColor(tile.LocalLocation, Color.red);
//        }
//    }

//    stopwatch.Stop();

//    UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
//}

//public void RemoveGridTileWithGameObject(GameObject targetObject)
//{
//    Vector3 targetPos = targetObject.transform.position;

//    var pos = new Vector3Int(Mathf.FloorToInt(targetPos.x), Mathf.FloorToInt(targetPos.y), (int)posZ);
//    levelTiles.Remove(pos);
//}