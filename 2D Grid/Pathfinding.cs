using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private List<GridTile> _waypoints;
    private Heap<GridTile> _openSet;
    private Heap<GridTile> _closedSet;
    private bool _pathFound = false;
    private Dictionary<Vector3, GridTile> _levelTiles;

    public Pathfinding(Dictionary<Vector3, GridTile> levelTiles)
    {
        _levelTiles = levelTiles;
    }

    public List<GridTile> FindPath(GridTile startPosition, GridTile targetPosition)
    {
        _pathFound = false;

        GridTile startTile = startPosition;
        GridTile targetTile = targetPosition;

        if (startTile != null && targetTile != null)
        {
            _openSet = new Heap<GridTile>(_levelTiles.Count);
            _closedSet = new Heap<GridTile>(_levelTiles.Count);

            _openSet.AddItem(startTile);

            while (_openSet.Count > 0)
            {
                GridTile currentTile = _openSet.RemoveFirstItem();
                _closedSet.AddItem(currentTile);

                if (currentTile == targetTile)
                {
                    _pathFound = true;
                    break;
                }

                foreach (GridTile neighbour in GetNeighbourTiles(currentTile))
                {
                    if (_closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    if (neighbour.Walkable == false || (neighbour.IsLocked == true && neighbour != targetTile))
                    {
                        _closedSet.AddItem(neighbour);
                        continue;
                    }

                    int newMovementCostToNeighbour = currentTile.GCost + GetDistance(currentTile, neighbour);

                    if (newMovementCostToNeighbour < neighbour.GCost || !_openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetTile);
                        neighbour.Parent = currentTile;

                        if (!_openSet.Contains(neighbour))
                        {
                            _openSet.AddItem(neighbour);
                        }

                        else
                        {
                            _openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        if (_pathFound)
        {
            _waypoints = RetracePath(startTile, targetTile);
            return _waypoints;
        }

        return new List<GridTile>();
    }

    private List<GridTile> RetracePath(GridTile startTile, GridTile targetTile)
    {
        List<GridTile> path = new List<GridTile>();
        GridTile current = targetTile;

        while (current != startTile)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(GridTile tileA, GridTile tileB)
    {
        int distanceX = Mathf.Abs(tileA.LocalLocation.x - tileB.LocalLocation.x);
        int distanceY = Mathf.Abs(tileA.LocalLocation.y - tileB.LocalLocation.y);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    private List<GridTile> GetNeighbourTiles(GridTile currentTile)
    {
        List<GridTile> tiles = new List<GridTile>();
        Vector3 tileLocation = currentTile.WorldLocation;

        if (_levelTiles.TryGetValue(tileLocation + Vector3.down, out GridTile tileDown))
        {
            tiles.Add(tileDown);
        }

        if (_levelTiles.TryGetValue(tileLocation + Vector3.up, out GridTile tileUp))
        {
            tiles.Add(tileUp);
        }

        if (_levelTiles.TryGetValue(tileLocation + Vector3.left, out GridTile tileLeft))
        {
            tiles.Add(tileLeft);
        }

        if (_levelTiles.TryGetValue(tileLocation + Vector3.right, out GridTile tileRight))
        {
            tiles.Add(tileRight);
        }

        //if (_levelTiles.TryGetValue(tileLocation + Vector3.down + Vector3.left, out GridTile tileDownLeft))
        //{
        //    tiles.Add(tileDownLeft);
        //}

        //if (_levelTiles.TryGetValue(tileLocation + Vector3.down + Vector3.right, out GridTile tileDownRight))
        //{
        //    tiles.Add(tileDownRight);
        //}

        //if (_levelTiles.TryGetValue(tileLocation + Vector3.up + Vector3.left, out GridTile tileUpLeft))
        //{
        //    tiles.Add(tileUpLeft);
        //}

        //if (_levelTiles.TryGetValue(tileLocation + Vector3.up + Vector3.right, out GridTile tileUpRight))
        //{
        //    tiles.Add(tileUpRight);
        //}

        return tiles;
    }
}
