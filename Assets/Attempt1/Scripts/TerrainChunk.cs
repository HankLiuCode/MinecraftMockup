using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attempt1
{
    [RequireComponent(typeof(ProcedureCubeMaker))]
    public class TerrainChunk : MonoBehaviour
    {
        ProcedureCubeMaker cubeMeshMaker;
        public static int length = 12;
        public static int width = 12;
        public static int height = 5;
        BlockType[,,] terrain;
        private Vector3 offset;


        void Start()
        {
            cubeMeshMaker = GetComponent<ProcedureCubeMaker>();
            terrain = new BlockType[length, height, width];
            offset = new Vector3(0.5f, 0, 0.5f);
            PopulateFakeTerrainData();
            MakeTerrain();
            UpdateTerrain();
        }

        public Vector3 WorldToTerrainPos(Vector3 worldPos)
        {
            return worldPos;
        }

        public void PopulateFakeTerrainData()
        {
            for (int x = 0; x < length; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    float xCoord = (float)x / length;
                    float zCoord = (float)z / width;
                    float yCoord = Mathf.PerlinNoise(xCoord, zCoord);
                    int height = Mathf.RoundToInt(yCoord * TerrainChunk.height);
                    // Debug.LogFormat("{0},{1},{2},{3}", xCoord, yCoord, zCoord, height);
                    for (int y = 0; y < height; y++)
                    {
                        terrain[x, y, z] = BlockType.Grass;
                    }
                }
            }
        }

        public void UpdateTerrain()
        {
            cubeMeshMaker.UpdateMesh();
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        }

        public void MakeTerrain()
        {
            cubeMeshMaker.Clear();

            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < width; z++)
                    {
                        if (terrain[x, y, z] != BlockType.None)
                        {
                            BlockType type = terrain[x, y, z];

                            if (!CheckCubeExist(x, y, z, Direction.North))
                            {
                                cubeMeshMaker.MakeFace(Direction.North, new Vector3(x, y, z) + offset, Block.GetBlock(type).side.GetUVs());
                            }
                            if (!CheckCubeExist(x, y, z, Direction.East))
                            {
                                cubeMeshMaker.MakeFace(Direction.East, new Vector3(x, y, z) + offset, Block.GetBlock(type).side.GetUVs());
                            }
                            if (!CheckCubeExist(x, y, z, Direction.South))
                            {
                                cubeMeshMaker.MakeFace(Direction.South, new Vector3(x, y, z) + offset, Block.GetBlock(type).side.GetUVs());
                            }
                            if (!CheckCubeExist(x, y, z, Direction.West))
                            {
                                cubeMeshMaker.MakeFace(Direction.West, new Vector3(x, y, z) + offset, Block.GetBlock(type).side.GetUVs());
                            }
                            if (!CheckCubeExist(x, y, z, Direction.Up))
                            {
                                cubeMeshMaker.MakeFace(Direction.Up, new Vector3(x, y, z) + offset, Block.GetBlock(type).top.GetUVs());
                            }
                            if (!CheckCubeExist(x, y, z, Direction.Down))
                            {
                                cubeMeshMaker.MakeFace(Direction.Down, new Vector3(x, y, z) + offset, Block.GetBlock(type).bottom.GetUVs());
                            }
                        }
                    }
                }
            }
        }

        public void RemoveCube(int x, int y, int z)
        {
            // Debug.LogFormat("X:{0},Y:{1},Z:{2}",x,y,z);
            terrain[x, y, z] = BlockType.None;
            MakeTerrain();
            UpdateTerrain();
        }

        public bool CheckCubeExist(int x, int y, int z, Direction dir)
        {
            if (dir == Direction.North)
            {
                return IsOutOfRange(x, y, z + 1) ? false : (terrain[x, y, z + 1] != BlockType.None ? true : false);
            }
            else if (dir == Direction.East)
            {
                return IsOutOfRange(x + 1, y, z) ? false :
                            (terrain[x + 1, y, z] != BlockType.None ? true : false);
            }
            else if (dir == Direction.South)
            {
                return IsOutOfRange(x, y, z - 1) ? false :
                            (terrain[x, y, z - 1] != BlockType.None ? true : false);
            }
            else if (dir == Direction.West)
            {
                return IsOutOfRange(x - 1, y, z) ? false :
                            (terrain[x - 1, y, z] != BlockType.None ? true : false);
            }
            else if (dir == Direction.Up)
            {
                return IsOutOfRange(x, y + 1, z) ? false :
                            (terrain[x, y + 1, z] != BlockType.None ? true : false);
            }
            else if (dir == Direction.Down)
            {
                return IsOutOfRange(x, y - 1, z) ? false :
                            (terrain[x, y - 1, z] != BlockType.None ? true : false);
            }
            Debug.LogError("Direction" + dir + "Does not exist");
            return false;
        }

        public bool IsOutOfRange(int x, int y, int z)
        {
            if (x >= terrain.GetLength(0) || x < 0) return true;
            if (y >= terrain.GetLength(1) || y < 0) return true;
            if (z >= terrain.GetLength(2) || z < 0) return true;
            return false;
        }
    }

}