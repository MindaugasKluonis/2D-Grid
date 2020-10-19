using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTile : IHeapItem<GridTile>
{
    public Vector3Int LocalLocation { set; get; }
    public Vector3 WorldLocation { set; get; }
    public TileScriptable Tile { set; get; }
    public Tilemap Tilemap { set; get; }
    public int Index { set; get; }

    public int GCost { set; get; }
    public int HCost { set; get; }
    public int FCost { get { return GCost + HCost; } }
    public bool Walkable { get { return Tile.isWall != true; } }
    public GridTile Parent { get; set; }
    public bool IsLocked { get; set; }
    public bool IsUsed { get; set; }

    public int CompareTo(GridTile other)
    {
        int compare = FCost.CompareTo(other.FCost);

        if (compare == 0)
        {
            compare = HCost.CompareTo(other.HCost);
        }

        return -compare;
    }

    public void LockTile()
    {
        IsLocked = true;
        Tilemap.SetTileFlags(LocalLocation, TileFlags.None);
        Tilemap.SetColor(LocalLocation, Color.yellow);
    }

    public void UnlockTile()
    {
        IsLocked = false;
        Tilemap.SetTileFlags(LocalLocation, TileFlags.None);
        Tilemap.SetColor(LocalLocation, Color.white);
    }

    public void LockTile(Color color)
    {
        IsLocked = true;
        Tilemap.SetTileFlags(LocalLocation, TileFlags.None);
        Tilemap.SetColor(LocalLocation, color);
    }

    public void SetUsed(bool value)
    {
        IsUsed = value;
    }
}