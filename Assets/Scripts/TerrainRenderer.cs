using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainRenderer : MonoBehaviour {
    [SerializeField]
    private int xSizeTest;
    [SerializeField]
    private int ySizeTest;
    [SerializeField]
    private float numColumnsInTilesheet;
    [SerializeField]
    private float numRowsInTilesheet;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> triangles = new List<int>();

    private float tUnitX;
    private float tUnitY;
    private Vector2 testTileOffset;

    private Mesh mesh;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        tUnitX = 1f / numColumnsInTilesheet;
        tUnitY = 1f / numRowsInTilesheet;

        Generate();
    }

    public void Generate()
    {
        // "SampleGenerate" is just throwaway code just to make sure that this script works. Later it will be replaced with
        // actual logic on how to render the terrain block.
        SampleGenerate();        
        DrawMesh();
    }

    void SampleGenerate()
    {
        testTileOffset = new Vector2(1, 11);

        for (int x = 0; x < xSizeTest; x++)
        {
            for (int y = 0; y < ySizeTest; y++)
            {
                GenerateSquare(x, y, testTileOffset);
            }
        }
    }

    void GenerateSquare(int x, int y, Vector2 tileOffset)
    {
        int squareIndex = y * xSizeTest + x;

        vertices.Add(new Vector3(x, y, 0));
        vertices.Add(new Vector3(x + 1, y, 0));
        vertices.Add(new Vector3(x + 1, y - 1, 0));
        vertices.Add(new Vector3(x, y - 1, 0));

        triangles.Add(squareIndex * 4);
        triangles.Add((squareIndex * 4) + 1);
        triangles.Add((squareIndex * 4) + 3);
        triangles.Add((squareIndex * 4) + 1);
        triangles.Add((squareIndex * 4) + 2);
        triangles.Add((squareIndex * 4) + 3);

        uv.Add(new Vector2(tUnitX * tileOffset.x, tUnitY * tileOffset.y + tUnitY));
        uv.Add(new Vector2(tUnitX * tileOffset.x + tUnitX, tUnitY * tileOffset.y + tUnitY));
        uv.Add(new Vector2(tUnitX * tileOffset.x + tUnitX, tUnitY * tileOffset.y));
        uv.Add(new Vector2(tUnitX * tileOffset.x, tUnitY * tileOffset.y));
    }

    void DrawMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
    }
}
