using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attempt1
{
    public class TerrainGenerator : MonoBehaviour
    {
        public Transform player;
        public GameObject TerrainChunkPrefab;
        public Dictionary<Vector3, TerrainChunk> chunks = new Dictionary<Vector3, TerrainChunk>();

        private void Start()
        {
            TerrainChunk newChunk = Instantiate(TerrainChunkPrefab, Vector3.zero, Quaternion.identity).GetComponent<TerrainChunk>();
        }

        private void Update()
        {
            int curChunkPosX = Mathf.FloorToInt(player.position.x / TerrainChunk.length);
            int curChunkPosY = Mathf.FloorToInt(player.position.y / TerrainChunk.width);


        }

        public void SpawnChunk(Direction dir)
        {
            if (dir == Direction.East)
            {

            }
        }

    }
}
