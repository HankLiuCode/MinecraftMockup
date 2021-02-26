using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainChunk : MonoBehaviour
{

    public const int chunkWidth = 10;
    public const int chunkHeight = 20;
    public BlockType[,,] blocks = new BlockType[chunkWidth, chunkHeight, chunkWidth];


    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    public void BuildMesh()
    {
        Mesh mesh = new Mesh();
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();


        for (int x=0; x<chunkWidth; x++)
            for(int z=0; z<chunkWidth; z++)
                for(int y=0; y<chunkHeight; y++)
                {
                    if (blocks[x, y, z] == BlockType.Air) continue;

                    // top
                    Vector3 blockPos = new Vector3(x, y, z);

                    bool makeBack = IsOutOfRange(x, y, z + 1) ? false : (blocks[x, y, z + 1] == BlockType.Air);
                    bool makeRight = IsOutOfRange(x + 1, y, z) ? false : (blocks[x + 1, y, z] == BlockType.Air);
                    bool makeFront = IsOutOfRange(x, y, z - 1) ? false : (blocks[x, y, z - 1] == BlockType.Air);
                    bool makeLeft = IsOutOfRange(x - 1, y, z) ? false : (blocks[x - 1, y, z] == BlockType.Air);
                    bool makeTop = (IsOutOfRange(x, y + 1, z) ? false : (blocks[x, y + 1, z] == BlockType.Air)) || (y == chunkHeight - 1);
                    bool makeBottom = IsOutOfRange(x, y - 1, z) ? false : (blocks[x, y - 1, z] == BlockType.Air);

                    if (makeBack)
                    {
                        int vCount = vertices.Count;
                        vertices.Add(blockPos + new Vector3(1, 0, 1));
                        vertices.Add(blockPos + new Vector3(1, 1, 1));
                        vertices.Add(blockPos + new Vector3(0, 1, 1));
                        vertices.Add(blockPos + new Vector3(0, 0, 1));

                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 1);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 3);
                        uvs.AddRange(Block.blocks[blocks[x, y, z]].sidePos.GetUvs());
                    }

                    if (makeRight)
                    {
                        int vCount = vertices.Count;
                        vertices.Add(blockPos + new Vector3(1, 0, 0));
                        vertices.Add(blockPos + new Vector3(1, 1, 0));
                        vertices.Add(blockPos + new Vector3(1, 1, 1));
                        vertices.Add(blockPos + new Vector3(1, 0, 1));
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 1);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 3);
                        uvs.AddRange(Block.blocks[blocks[x, y, z]].sidePos.GetUvs());
                    }

                    if (makeFront)
                    {
                        int vCount = vertices.Count;
                        vertices.Add(blockPos + new Vector3(0, 0, 0));
                        vertices.Add(blockPos + new Vector3(0, 1, 0));
                        vertices.Add(blockPos + new Vector3(1, 1, 0));
                        vertices.Add(blockPos + new Vector3(1, 0, 0));

                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 1);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 3);
                        uvs.AddRange(Block.blocks[blocks[x, y, z]].sidePos.GetUvs());
                    }

                    if (makeLeft)
                    {
                        int vCount = vertices.Count;
                        vertices.Add(blockPos + new Vector3(0, 0, 1));
                        vertices.Add(blockPos + new Vector3(0, 1, 1));
                        vertices.Add(blockPos + new Vector3(0, 1, 0));
                        vertices.Add(blockPos + new Vector3(0, 0, 0));
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 1);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 3);
                        uvs.AddRange(Block.blocks[blocks[x, y, z]].sidePos.GetUvs());
                    }

                    if (makeTop)
                    {
                        int vCount = vertices.Count;
                        vertices.Add(blockPos + new Vector3(0, 1, 0));
                        vertices.Add(blockPos + new Vector3(0, 1, 1));
                        vertices.Add(blockPos + new Vector3(1, 1, 1));
                        vertices.Add(blockPos + new Vector3(1, 1, 0));

                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 1);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 3);

                        uvs.AddRange(Block.blocks[blocks[x, y, z]].topPos.GetUvs());
                    }

                    if (makeBottom)
                    {
                        int vCount = vertices.Count;
                        vertices.Add(blockPos + new Vector3(1, 0, 0));
                        vertices.Add(blockPos + new Vector3(1, 0, 1));
                        vertices.Add(blockPos + new Vector3(0, 0, 1));
                        vertices.Add(blockPos + new Vector3(0, 0, 0));

                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 1);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 0);
                        triangles.Add(vCount + 2);
                        triangles.Add(vCount + 3);
                        uvs.AddRange(Block.blocks[blocks[x, y, z]].bottomPos.GetUvs());
                    }
                }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public bool IsOutOfRange(int x, int y, int z)
    {
        if (x < 0 || x > chunkWidth - 1) return true;
        if (z < 0 || z > chunkWidth - 1) return true;
        if (y < 0 || y > chunkHeight - 1) return true;
        return false;
    }

    public void PopulateTestTerrain()
    {
        for (int x = 0; x < chunkWidth; x++)
            for (int z = 0; z < chunkWidth; z++)
                for (int y = 0; y < chunkHeight; y++)
                {
                    blocks[x, y, z] = BlockType.Grass;
                }
    }
}
