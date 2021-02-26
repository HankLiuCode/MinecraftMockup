using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public float maxDistance = 10f;
    public LayerMask modifyLayer;

    public bool hasSth;


    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        hasSth = Physics.Raycast(ray, maxDistance, modifyLayer);

        bool leftClick = Input.GetMouseButtonDown(0);
        if (leftClick)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDistance, modifyLayer))
            {
                Vector3 pointInTargetBlock = hitInfo.point + transform.forward * .01f; //move a little inside the block


                int cubePosX = Mathf.FloorToInt(pointInTargetBlock.x);
                int cubePosY = Mathf.FloorToInt(pointInTargetBlock.y);
                int cubePosZ = Mathf.FloorToInt(pointInTargetBlock.z);
                Debug.LogFormat("cubePosX:{0},PosY:{1},PosZ:{2}", cubePosX, cubePosY, cubePosZ);

                int chunkPosX = Mathf.FloorToInt (pointInTargetBlock.x / TerrainChunk.chunkWidth) * TerrainChunk.chunkWidth;
                int chunkPosZ = Mathf.FloorToInt(pointInTargetBlock.z / TerrainChunk.chunkWidth) * TerrainChunk.chunkWidth;
                Debug.LogFormat("ChunkPosX:{0},ChunkPosZ:{1}", cubePosX, cubePosZ);

                TerrainChunk chunk = TerrainGenerator.chunks[new ChunkPos(chunkPosX, chunkPosZ)];
                chunk.blocks[cubePosX - chunkPosX, cubePosY, cubePosZ - chunkPosZ] = BlockType.Air;
                chunk.BuildMesh();
            }
        }
    }
}
