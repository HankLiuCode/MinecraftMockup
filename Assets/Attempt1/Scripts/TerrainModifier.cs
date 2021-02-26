using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attempt1
{
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
                    Vector3 pointInTargetBlock;
                    pointInTargetBlock = hitInfo.point + transform.forward * .01f; //move a little inside the block


                    int cubePosX = Mathf.FloorToInt(hitInfo.point.x);
                    int cubePosY = Mathf.FloorToInt(hitInfo.point.y);
                    int cubePosZ = Mathf.FloorToInt(hitInfo.point.z);



                }
            }
        }
    }

}