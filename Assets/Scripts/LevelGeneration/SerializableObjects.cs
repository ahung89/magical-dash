using UnityEngine;

namespace SerializableObjects
{
    public struct TerrainBlock
    {
        public Vector2 Position;
        public int Width;
        public int Height;

        public TerrainBlock (Vector2 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
        }
    }
}