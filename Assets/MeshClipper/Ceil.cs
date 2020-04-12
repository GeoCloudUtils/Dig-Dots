/*
 Author: Unknown
 Edited: Ghercioglo Roman (Romeon0)
 
 */

using System.Collections.Generic;
using UnityEngine;

namespace Assets.MeshClipper
{
    [ExecuteInEditMode]
    public class Ceil : MonoBehaviour
    {
        [Header("Settings")]
        public const float scale = 100f;
        public GameObject bevelObject;
        public float width = 100f;
        public float height = 100f;
        public static float depth = 7f;
        //Private attributes
        [HideInInspector] public List<IntPoint> path;
        [HideInInspector] public int px;
        [HideInInspector] public int py;
        [HideInInspector] public float area;
        [HideInInspector] public Mesh mesh;
        [HideInInspector] public Mesh bevelMesh;
        private MeshCollider _bevelMeshCollider;
        private MeshClipperController _meshWorker;
        private float size;
        private bool _IsInitialized = false;

        public void Awake()
        {
            if (_IsInitialized) return;
            _IsInitialized = true;

            GetComponent<MeshFilter>().mesh = (mesh = new Mesh());
            bevelObject.GetComponent<MeshFilter>().mesh = (bevelMesh = new Mesh());
            _bevelMeshCollider = bevelObject.GetComponent<MeshCollider>();
            _bevelMeshCollider.sharedMesh = bevelMesh;
        }

        public void SetClipperController(MeshClipperController controller)
        {
            _meshWorker = controller;
        }
        
        public void CreateMeshAt(int vx, int vy, Texture2D texture, float meshWidth, float meshHeight)
        {
            Vector3 meshPos = _meshWorker.gameObject.transform.position;

            px = vx;
            py = vy;
            mesh.name = "Mesh";
            float widthPerCeil = meshWidth / (float)_meshWorker.soilColumns / (float)_meshWorker.soilMultiple;
            float heightPerCeil = meshHeight / (float)_meshWorker.soilRows / (float)_meshWorker.soilMultiple;
            float positionX = (meshPos.x * 100f) + width * (float)px * widthPerCeil;
            float positionY = (meshPos.y * 100f) + height * (float)py * heightPerCeil;

            //Debug.LogFormat("Data: {0} {1}",
            //    positionX, positionY);

            //Create mesh points
            List<IntPoint> list = new List<IntPoint>(4);
            list.Add(new IntPoint(positionX, positionY));
            list.Add(new IntPoint(positionX + width * widthPerCeil, positionY));
            list.Add(new IntPoint(positionX + width * widthPerCeil, positionY + height * heightPerCeil));
            list.Add(new IntPoint(positionX, positionY + height * heightPerCeil));
            CreateMesh(list);

            //Set texture for Mesh & BevelMesh
            MeshRenderer[] components = GetComponents<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in components)
            {
                meshRenderer.sharedMaterial.SetTexture("_MainTex", texture);
            }
            MeshRenderer[] components2 = bevelObject.GetComponents<MeshRenderer>();
            foreach (MeshRenderer meshRenderer2 in components2)
            {
                meshRenderer2.sharedMaterial.SetTexture("_MainTex", texture);
            }
        }

        public bool UpdateMesh(List<IntPoint> pathIn)
        {
            mesh.Clear();
            bevelMesh.Clear();
            return CreateMesh(pathIn);
        }

        public void CleanUp()
        {
            mesh.Clear();
            bevelMesh.Clear();
            UnityEngine.Object.Destroy(mesh);
            UnityEngine.Object.Destroy(bevelMesh);
            UnityEngine.Object.Destroy(bevelObject);
        }

        public bool CreateMesh(List<IntPoint> pathIn)
        {
            path = pathIn;
            int pointsCount = path.Count;

            //Set Collider
            PolygonCollider2D component = GetComponent<PolygonCollider2D>();
            Vector2[] points = new Vector2[pointsCount];
            int num = 0;
            foreach (IntPoint item in path)
            {
                IntPoint current = item;
                points[num] = new Vector2(0.01f * (float)current.X, 0.01f * (float)current.Y);
                num++;
            }
            component.SetPath(0, points);

            //Triangulate and get data(vertices, triangles, normals)
            Triangulator triangulator = new Triangulator(points);
            int[] triangles = triangulator.Triangulate();
            area = triangulator.area;
            Vector3[] vertices = new Vector3[pointsCount];
            Vector3[] normals = new Vector3[pointsCount];
            Vector2[] UVs = new Vector2[pointsCount];
            for (int i = 0; i < pointsCount; i++)
            {
                vertices[i].x = points[i].x;
                vertices[i].y = points[i].y;
                vertices[i].z = 0f;
                normals[i] = -Vector3.forward;
            }

            //Calculate mesh size
            if (size == 0f)
            {
                Vector2 minPoint = new Vector3(9999f, 9999f, 9999f);
                Vector2 maxPoint = new Vector3(-9999f, -9999f, -9999f);
                for (int k = 0; k < vertices.Length; k++)
                {
                    minPoint.x = Mathf.Min(minPoint.x, vertices[k].x);
                    maxPoint.x = Mathf.Max(maxPoint.x, vertices[k].x);
                    minPoint.y = Mathf.Min(minPoint.y, vertices[k].y);
                    maxPoint.y = Mathf.Max(maxPoint.y, vertices[k].y);
                }
                size = Mathf.Max(maxPoint.x - minPoint.x, maxPoint.y - minPoint.y);
            }
          


            //Calculate UVs
            for (int i = 0; i < pointsCount; i++)
            {
                UVs[i] = new Vector2((vertices[i].x + size / 2f) / size, (vertices[i].y + size / 2f) / size);
            }

            //Set Data
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = UVs;
            mesh.RecalculateBounds();

            //Debug.LogFormat("Data: {0} {1} {2} {3}",
            //    vertices.Length, triangles.Length,
            //    normals.Length, UVs.Length);

            CreateBevelMesh(points);

            return true;
        }

        public void CreateBevelMesh(Vector2[] points)
        {
            int pointsCount = points.Length;
            int pointsCount2X = pointsCount * 2;

            //Get vertices
            Vector3[] vertices = new Vector3[pointsCount2X];
            Vector3[] normals = new Vector3[pointsCount2X];
            for (int i = 0; i < pointsCount; i++)
            {
                vertices[i].x = points[i].x;
                vertices[i].y = points[i].y;
                vertices[i].z = 0.02f;
                vertices[i + pointsCount].x = points[i].x;
                vertices[i + pointsCount].y = points[i].y;
                vertices[i + pointsCount].z = depth;
            }

            //Get normals
            Vector3 vector = Vector3.right;
            for (int j = 0; j < pointsCount; j++)
            {
                Vector3 normalized = (vertices[(j + 1) % pointsCount] - vertices[j]).normalized;
                vector = 0.9f * vector + 0.1f * Vector3.Cross(normalized, Vector3.forward);
                normals[j] = vector;

                Vector3 normalized2 = (vertices[(j + pointsCount + 1) % pointsCount] - vertices[j + pointsCount]).normalized;
                vector = Vector3.Cross(normalized2, Vector3.forward);
                vector = 0.9f * vector + 0.1f * Vector3.Cross(normalized2, Vector3.forward);
                normals[j + pointsCount] = vector;
            }
            Vector3 a = new Vector3(20f, 70f, 24f);
            for (int m = 0; m < vertices.Length; m++)
            {
                normals[m] = vertices[m].normalized;
                normals[m] = (a - vertices[m]).normalized;
            }

            //Get triangles
            int[] triangles = new int[pointsCount * 6];
            int num2 = 0;
            for (int l = 0; l < pointsCount; l++)
            {
                int num3 = (l + 1) % pointsCount;
                triangles[num2] = l;
                triangles[num2 + 1] = num3;
                triangles[num2 + 2] = l + pointsCount;
                triangles[num2 + 3] = num3;
                triangles[num2 + 4] = num3 + pointsCount;
                triangles[num2 + 5] = l + pointsCount;
                num2 += 6;
            }
         
            //Calculate UVs
            Vector2[] UVs = new Vector2[vertices.Length];
            for (int n = 0; n < UVs.Length; n++)
            {
                Vector3 normalized3 = new Vector3(normals[n].x, normals[n].y).normalized;
                float x = vertices[n].x * Mathf.Abs(normalized3.y) + vertices[n].y * Mathf.Abs(normalized3.x);
                UVs[n] = new Vector2(x, vertices[n].z);
            }

            //Set data
            bevelMesh.vertices = vertices;
            bevelMesh.triangles = triangles;
            bevelMesh.normals = normals;
            bevelMesh.uv = UVs;

            _bevelMeshCollider.sharedMesh = bevelMesh;
        }
    }
}
