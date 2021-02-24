using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProcedureCubeMaker))]
public class TerrainChunk : MonoBehaviour
{
    ProcedureCubeMaker cubeMeshMaker;
    public int length = 5;
    public int width = 5;
    public int height = 5;
    int[,,] terrain;
    private Vector3 offset;


    void Start()
    {
        cubeMeshMaker = GetComponent<ProcedureCubeMaker>();
        terrain = new int[length, height, width];
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
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    terrain[x, y, z] = 1; //Random.Range(0, 2);
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
                    if(terrain[x,y,z] == 1)
                    {
                        if (!CheckCubeExist(x, y, z, Direction.North))
                        {
                            cubeMeshMaker.MakeFace(Direction.North, new Vector3(x, y, z) + offset, TilePos.tiles[Tile.GrassSide].GetUVs());
                        }
                        if (!CheckCubeExist(x, y, z, Direction.East))
                        {
                            cubeMeshMaker.MakeFace(Direction.East, new Vector3(x, y, z) + offset, TilePos.tiles[Tile.GrassSide].GetUVs());
                        }
                        if (!CheckCubeExist(x, y, z, Direction.South))
                        {
                            cubeMeshMaker.MakeFace(Direction.South, new Vector3(x, y, z) + offset, TilePos.tiles[Tile.GrassSide].GetUVs());
                        }
                        if (!CheckCubeExist(x, y, z, Direction.West))
                        {
                            cubeMeshMaker.MakeFace(Direction.West, new Vector3(x, y, z) + offset, TilePos.tiles[Tile.GrassSide].GetUVs());
                        }
                        if (!CheckCubeExist(x, y, z, Direction.Up))
                        {
                            cubeMeshMaker.MakeFace(Direction.Up, new Vector3(x, y, z) + offset, TilePos.tiles[Tile.Grass].GetUVs());
                        }
                        if (!CheckCubeExist(x, y, z, Direction.Down))
                        {
                            cubeMeshMaker.MakeFace(Direction.Down, new Vector3(x, y, z) + offset, TilePos.tiles[Tile.GrassSide].GetUVs());
                        }
                    }
                }
            }
        }
    }

    public void RemoveCube(int x, int y, int z)
    {
        Debug.LogFormat("X:{0},Y:{1},Z:{2}",x,y,z);
        terrain[x, y, z] = 0;
        MakeTerrain();
        UpdateTerrain();
    }

    public bool CheckCubeExist(int x, int y, int z, Direction dir)
    {
        if (dir == Direction.North)
        {
            return IsOutOfRange(x, y, z + 1) ? false : (terrain[x, y, z + 1] == 1 ? true : false);
        }
        else if (dir == Direction.East)
        {
            return IsOutOfRange(x + 1, y, z) ? false :
                        terrain[x + 1, y, z] == 1 ? true : false;
        }
        else if (dir == Direction.South)
        {
            return IsOutOfRange(x, y, z - 1) ? false :
                        terrain[x, y, z - 1] == 1 ? true : false;
        }
        else if (dir == Direction.West)
        {
            return IsOutOfRange(x - 1, y, z) ? false :
                        terrain[x - 1, y, z] == 1 ? true : false;
        }
        else if (dir == Direction.Up)
        {
            return IsOutOfRange(x, y + 1, z) ? false :
                        terrain[x, y + 1, z] == 1 ? true : false;
        }
        else if (dir == Direction.Down)
        {
            return IsOutOfRange(x, y - 1, z) ? false :
                        terrain[x, y - 1, z] == 1 ? true : false;
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
