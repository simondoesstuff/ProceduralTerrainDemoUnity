using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInitializer : MonoBehaviour
{
    public int length;
    public int lengthPerChunk;
    public GameObject chunkPrefab;

    private TerrainSettings _terrainSettings;

    private void Start()
    {
        _terrainSettings = GetComponent<TerrainSettings>();
        _terrainSettings.meshes = new ChunkMesh[length * length];
        
        var chunks = new ChunkMesh[length * length];
        var position = transform.position;
        int index = 0;

        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < length; z++)
            {
                var mesh = Instantiate(chunkPrefab).GetComponent<ChunkMesh>();

                mesh.xSize = lengthPerChunk;
                mesh.zSize = lengthPerChunk;
                
                var chunkPos = position;
                chunkPos.x += x * (lengthPerChunk - 1);
                chunkPos.z += z * (lengthPerChunk - 1);
                mesh.gameObject.transform.position = chunkPos;

                _terrainSettings.meshes[index] = mesh;
                index++;
            }
        }
    }
}
