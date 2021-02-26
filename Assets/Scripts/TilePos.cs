using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePos
{
    int x, y;
    Vector2[] uvs;

    public TilePos(int x, int y)
    {
        this.x = x;
        this.y = y;
        uvs = new Vector2[]
        {
            new Vector2(x/16f, y/16f),
            new Vector2(x/16f, (y+1)/16f),
            new Vector2((x+1)/16f, (y+1)/16f),
            new Vector2((x+1)/16f, y/16f)
        };
    }

    public Vector2[] GetUvs()
    {
        return uvs;
    }

    public static Dictionary<Tile, TilePos> tiles = new Dictionary<Tile, TilePos>() 
    {
        {Tile.Dirt, new TilePos(0,0)},
        {Tile.Grass, new TilePos(1,0)},
        {Tile.GrassSide, new TilePos(0,1)},
        {Tile.Stone, new TilePos(0,2)},
        {Tile.TreeSide, new TilePos(0,4)},
        {Tile.TreeCX, new TilePos(0,3)},
        {Tile.Leaves, new TilePos(0,5)},
    };
    
}

public enum Tile { Dirt, Grass, GrassSide, Stone, TreeSide, TreeCX, Leaves }
