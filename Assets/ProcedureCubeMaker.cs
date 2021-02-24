using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProcedureCubeMaker : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;
    float adjScale = 0.5f;

    public float scale = 1f;


    private void Awake()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        adjScale = scale / 2;
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void MakeCube(Vector3 pos)
    {
        MakeFace(Direction.North, pos, TilePos.tiles[Tile.GrassSide].GetUVs());
        MakeFace(Direction.East, pos, TilePos.tiles[Tile.GrassSide].GetUVs());
        MakeFace(Direction.South, pos, TilePos.tiles[Tile.GrassSide].GetUVs());
        MakeFace(Direction.West, pos, TilePos.tiles[Tile.GrassSide].GetUVs());
        MakeFace(Direction.Up, pos, TilePos.tiles[Tile.Grass].GetUVs());
        MakeFace(Direction.Down, pos, TilePos.tiles[Tile.Dirt].GetUVs());
    }

    public void MakeFace(Direction dir, Vector3 pos, Vector2[] textureUv)
    {
        vertices.AddRange(CubeMeshData.GetFaceVertices(dir, pos, adjScale));
        int vCount = vertices.Count;
        triangles.Add(vCount - 4 + 0);
        triangles.Add(vCount - 4 + 1);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 0);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 3);

        uvs.AddRange(textureUv);
    }

    public void Clear()
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
    }

    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }
}
