using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attempt1
{
    public class Block
    {
        public BlockType type;
        public TilePos top, side, bottom;
        public Block(BlockType type, TilePos top, TilePos side, TilePos bottom)
        {
            this.type = type;
            this.top = top;
            this.side = side;
            this.bottom = bottom;
        }

        public static Block GetBlock(BlockType type)
        {
            return blocks[type];
        }

        private static Dictionary<BlockType, Block> blocks = new Dictionary<BlockType, Block>()
    {
        { BlockType.Grass, new Block(BlockType.Grass, TilePos.tiles[Tile.Grass], TilePos.tiles[Tile.GrassSide], TilePos.tiles[Tile.Dirt])},
        { BlockType.Dirt, new Block(BlockType.Dirt, TilePos.tiles[Tile.Dirt], TilePos.tiles[Tile.Dirt], TilePos.tiles[Tile.Dirt])},
        { BlockType.Trunk, new Block(BlockType.Trunk, TilePos.tiles[Tile.TreeCX], TilePos.tiles[Tile.TreeSide],TilePos.tiles[Tile.TreeCX])},
        { BlockType.Leaves, new Block(BlockType.Leaves, TilePos.tiles[Tile.Leaves], TilePos.tiles[Tile.Leaves],TilePos.tiles[Tile.Leaves])},
        { BlockType.Stone, new Block(BlockType.Stone, TilePos.tiles[Tile.Stone], TilePos.tiles[Tile.Stone],TilePos.tiles[Tile.Stone])}
    };
    }

    public enum BlockType
    {
        None,
        Grass,
        Dirt,
        Trunk,
        Leaves,
        Stone
    }
}
