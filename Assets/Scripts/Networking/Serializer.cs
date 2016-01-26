using UnityEngine;
using ExitGames.Client.Photon;
using SerializableObjects;

public class Serializer : MonoBehaviour {

	void Awake()
    {
        PhotonPeer.RegisterType(typeof(TerrainBlock), (byte)'T', SerializeTerrainBlock, DeserializeTerrainBlock);
    }

    private static byte[] SerializeTerrainBlock(object obj)
    {
        TerrainBlock block = (TerrainBlock)obj;

        byte[] bytes = new byte[4 * 4]; // floats/ints are 4 bytes each.
        int index = 0;
        Protocol.Serialize(block.Position.x, bytes, ref index);
        Protocol.Serialize(block.Position.y, bytes, ref index);
        Protocol.Serialize(block.Width, bytes, ref index);
        Protocol.Serialize(block.Height, bytes, ref index);

        return bytes;
    }

    private static object DeserializeTerrainBlock(byte[] bytes)
    {
        TerrainBlock block = new TerrainBlock();
        Vector2 pos = new Vector2();

        int index = 0;
        Protocol.Deserialize(out pos.x, bytes, ref index);
        Protocol.Deserialize(out pos.y, bytes, ref index);
        Protocol.Deserialize(out block.Width, bytes, ref index);
        Protocol.Deserialize(out block.Height, bytes, ref index);

        block.Position = pos;
        return block;
    }
}
