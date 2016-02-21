using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainRenderer : MonoBehaviour {
    // The length/width of a terrain tile
    public static float TerrainTileUnit = 1;

    [SerializeField]
    private float numColumnsInTilesheet;
    [SerializeField]
    private float numRowsInTilesheet;

    public int WidthInTiles { get; set; }
    public int HeightInTiles { get; set; }
    public bool ColliderConfigured { get; set; }

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> triangles = new List<int>();

    private float tUnitX;
    private float tUnitY;
    private Vector2 testTileOffset;

    private Mesh mesh;
    private BoxCollider2D boxCollider2D;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        boxCollider2D = GetComponent<BoxCollider2D>();

        tUnitX = 1f / numColumnsInTilesheet;
        tUnitY = 1f / numRowsInTilesheet;
    }

    public void Generate(int xSize, int ySize)
    {
        // For detection by destroyer.
        boxCollider2D.size = new Vector2(.1f, .1f);
        ColliderConfigured = false;

        WidthInTiles = xSize;
        HeightInTiles = ySize;

        ClearMesh();
        // "SampleGenerate" is just throwaway code just to make sure that this script works. Later it will be replaced with
        // actual logic on how to render the terrain block.
        SampleGenerate(xSize, ySize);        
        DrawMesh();
    }

    void SampleGenerate(int xSize, int ySize)
    {
        testTileOffset = new Vector2(1, 4);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GenerateSquare(x, y, xSize, testTileOffset);
            }
        }
    }

    void ClearMesh()
    {
        mesh.Clear();
        vertices.Clear();
        uv.Clear();
        triangles.Clear();
    }

    void GenerateSquare(int x, int y, int xSize, Vector2 tileOffset)
    {
        int squareIndex = y * xSize + x;

        vertices.Add(new Vector3(x, y, 0));
        vertices.Add(new Vector3(x + TerrainTileUnit, y, 0));
        vertices.Add(new Vector3(x + TerrainTileUnit, y + TerrainTileUnit, 0));
        vertices.Add(new Vector3(x, y + TerrainTileUnit, 0));

        triangles.Add(squareIndex * 4);
        triangles.Add((squareIndex * 4) + 1);
        triangles.Add((squareIndex * 4) + 3);
        triangles.Add((squareIndex * 4) + 1);
        triangles.Add((squareIndex * 4) + 2);
        triangles.Add((squareIndex * 4) + 3);

        uv.Add(new Vector2(tUnitX * tileOffset.x, tUnitY * tileOffset.y));
        uv.Add(new Vector2(tUnitX * tileOffset.x + tUnitX, tUnitY * tileOffset.y));
        uv.Add(new Vector2(tUnitX * tileOffset.x + tUnitX, tUnitY * tileOffset.y + tUnitY));
        uv.Add(new Vector2(tUnitX * tileOffset.x, tUnitY * tileOffset.y + tUnitY));
    }

    void DrawMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
    }

    public void ConfigureCollider(int xSize, int ySize)
    {
        boxCollider2D.offset = new Vector2((float)xSize / 2f, (float)ySize / 2f);
        boxCollider2D.size = new Vector2(xSize, ySize);
        ColliderConfigured = true;
    }
}
