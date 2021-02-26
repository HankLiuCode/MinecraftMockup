using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject terrainChunkPrefab;
    public GameObject waterChunkPrefab;

    public Transform player;
    public List<Vector3> debugList;
    public FastNoise noise = new FastNoise();
    public int chunkDist = 2;

    public static Dictionary<ChunkPos, TerrainChunk> chunks = new Dictionary<ChunkPos, TerrainChunk>(new ChunkPosEqualityComparer());
    public ChunkPos curChunk = new ChunkPos(Mathf.RoundToInt(-Mathf.Infinity), Mathf.RoundToInt(-Mathf.Infinity));


    private void Start()
    {
        LoadChunks();
    }

    private void Update()
    {
        LoadChunks();
    }

    public void LoadChunks()
    {
        int curChunkPosX = Mathf.FloorToInt(player.position.x / TerrainChunk.chunkWidth) * TerrainChunk.chunkWidth;
        int curChunkPosZ = Mathf.FloorToInt(player.position.z / TerrainChunk.chunkWidth) * TerrainChunk.chunkWidth;
        int curChunkPosY = Mathf.FloorToInt(player.position.y / TerrainChunk.chunkHeight) * TerrainChunk.chunkHeight;

        // Debug.LogFormat("{0},{1}", curChunkPosX, curChunkPosZ);

        if(curChunk.xPos != curChunkPosX || curChunk.zPos != curChunkPosZ)
        {
            curChunk.xPos = curChunkPosX;
            curChunk.zPos = curChunkPosZ;

            for (int i = curChunkPosX - TerrainChunk.chunkWidth * chunkDist; i < curChunkPosX + TerrainChunk.chunkWidth * chunkDist; i += TerrainChunk.chunkWidth)
            {
                for (int j = curChunkPosZ - TerrainChunk.chunkWidth * chunkDist; j < curChunkPosZ + TerrainChunk.chunkWidth * chunkDist; j += TerrainChunk.chunkWidth)
                {
                    ChunkPos cpos = new ChunkPos(i, j);
                    if (!chunks.ContainsKey(cpos))
                    {
                        BuildChunk(i, 0, j);
                    }
                }
            }
        }

        List<ChunkPos> toDestroy = new List<ChunkPos>();
        foreach(KeyValuePair<ChunkPos, TerrainChunk> chunk in chunks)
        {
            if(Mathf.Abs(chunk.Key.xPos - curChunk.xPos) > TerrainChunk.chunkWidth * (chunkDist + 1) || Mathf.Abs(chunk.Key.zPos - curChunk.zPos) > TerrainChunk.chunkWidth * (chunkDist + 1)) {
                toDestroy.Add(chunk.Key);
            }
        }

        foreach(ChunkPos cp in toDestroy)
        {
            Destroy(chunks[cp].gameObject);
            chunks.Remove(cp);
        }
    }

    public void BuildChunk(int xPos, int yPos, int zPos)
    {
        GameObject chunkGO = Instantiate(terrainChunkPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        TerrainChunk chunk = chunkGO.GetComponent<TerrainChunk>();

        for (int x = 0; x < TerrainChunk.chunkWidth; x++)
            for (int z = 0; z < TerrainChunk.chunkWidth; z++)
                for (int y = 0; y < TerrainChunk.chunkHeight; y++) 
                {
                    chunk.blocks[x, y, z] = GetBlockType(xPos + x, yPos + y, zPos + z);
                }

        chunk.BuildMesh();
        chunks.Add(new ChunkPos(xPos, zPos), chunk);
        debugList.Add(new Vector3(xPos, 0, zPos));
    }

    BlockType GetBlockType(int x, int y, int z)
    {

        //print(noise.GetSimplex(x, z));
        float simplex1 = noise.GetSimplex(x * .8f, z * .8f) * 10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = TerrainChunk.chunkHeight * .5f + heightMap;

        //3d noise for caves and overhangs and such
        float caveNoise1 = noise.GetPerlinFractal(x * 5f, y * 10f, z * 5f);
        float caveMask = noise.GetSimplex(x * .3f, z * .3f) + .3f;

        //stone layer heightmap
        float simplexStone1 = noise.GetSimplex(x * 1f, z * 1f) * 10;
        float simplexStone2 = (noise.GetSimplex(x * 5f, z * 5f) + .5f) * 20 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);

        float stoneHeightMap = simplexStone1 + simplexStone2;
        float baseStoneHeight = TerrainChunk.chunkHeight * .25f + stoneHeightMap;


        //float cliffThing = noise.GetSimplex(x * 1f, z * 1f, y) * 10;
        //float cliffThingMask = noise.GetSimplex(x * .4f, z * .4f) + .3f;



        BlockType blockType = BlockType.Air;

        //under the surface, dirt block
        if (y <= baseLandHeight)
        {
            blockType = BlockType.Dirt;

            //just on the surface, use a grass type
            if (y > baseLandHeight - 1)
                blockType = BlockType.Grass;

            if (y <= baseStoneHeight)
                blockType = BlockType.Stone;
        }


        if (caveNoise1 > Mathf.Max(caveMask, .2f))
            blockType = BlockType.Air;

        /*if(blockType != BlockType.Air)
            blockType = BlockType.Stone;*/

        //if(blockType == BlockType.Air && noise.GetSimplex(x * 4f, y * 4f, z*4f) < 0)
        //  blockType = BlockType.Dirt;

        //if(Mathf.PerlinNoise(x * .1f, z * .1f) * 10 + y < TerrainChunk.chunkHeight * .5f)
        //    return BlockType.Grass;

        return blockType;
    }
}

public class ChunkPos
{
    public int xPos, zPos;
    public ChunkPos(int xPos, int zPos)
    {
        this.xPos = xPos;
        this.zPos = zPos;
    }
}

public class ChunkPosEqualityComparer : IEqualityComparer<ChunkPos>
{
    public bool Equals(ChunkPos c1, ChunkPos c2)
    {
        return GetHashCode(c1) == GetHashCode(c2);

    }

    public int GetHashCode(ChunkPos obj)
    {
        int x = obj.xPos;
        int y = obj.zPos;
        int xx = x >= 0 ? x * 2 : x * -2 - 1;
        int yy = y >= 0 ? y * 2 : y * -2 - 1;
        return (xx >= yy) ? (xx * xx + xx + yy) : (yy * yy + xx);
    }
}
