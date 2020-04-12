/*
    Author: Ghercioglo Roman (Romeon0)
 */

using Assets.MeshClipper;
using UnityEngine;

namespace ScriptUtils.Editor
{
    public class GameView : MonoBehaviour
    {
        //References
        public MeshClipperController meshClipperController;

        public Transform leftWall;
        public Transform rightWall;

        private void Start()
        {
            Regenerate();
           
        }
        private void Awake()
        {
            if (meshClipperController == null) { Debug.LogError("meshClipperController is not assigned!"); }
            if (leftWall == null) { Debug.LogError("leftWall is not assigned!"); }
            if (rightWall == null) { Debug.LogError("rightWall is not assigned!"); }
        }

        private void SetWalls()
        {
            Debug.Log("GameView: SetWalls!");
            Vector3 leftMargin = Vector3.zero, rightMargin = Vector3.zero;

            int raycastMask = LayerMask.GetMask("RaycastPanel");
            Ray left = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.1f, 0));
            RaycastHit hitInfo;
            if(Physics.Raycast(left, out hitInfo, 400, raycastMask))
            {
                Debug.Log("Hit left: " + hitInfo.collider.name);
                leftMargin = hitInfo.point;
            }

            Ray right = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.9f, 0));
            if (Physics.Raycast(right, out hitInfo, 400, raycastMask))
            {
                Debug.Log("Hit right: " + hitInfo.collider.name);
                rightMargin = hitInfo.point;
            }

            leftWall.position = new Vector3(leftMargin.x - leftWall.localScale.x / 2, leftWall.position.y, leftWall.position.z);
            rightWall.position = new Vector3(rightMargin.x + rightWall.localScale.x / 2, rightWall.position.y, rightWall.position.z);
        }

        private void SetTerrain()
        {
            Vector3 v1 = leftWall.position + leftWall.localScale/2;
            Vector3 v2 = rightWall.position - rightWall.localScale/2;

            Transform terrain = meshClipperController.gameObject.transform;
            terrain.position = new Vector3(v1.x, terrain.position.y, terrain.position.y);

            meshClipperController.width = (v2 - v1).x;
        }

        public void Regenerate()
        {
            SetWalls();
            SetTerrain();

            meshClipperController.Regenerate();
        }
    }
}
