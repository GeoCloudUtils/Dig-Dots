/*
 * Author: Unknown
 *
 *
 * Edited: Ghercioglo Roman (Romeon0)
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MeshClipper
{
    public class MeshClipperController : MonoBehaviour
    {
        public static MeshClipperController Instance;

        [Header("References")]
        public GameObject ceilTemplate;
        public Texture2D ceilTexture;
        [Header("Settings")]
        public int soilColumns = 5;
        public int soilRows = 10;
        public int soilMultiple = 2;
        public float cutRadius = 3f;
        public float width = 40f;
        public float height = 80f;
        //Private attributes
        private Vector3 _deltaPos=Vector3.zero;
        private Vector3 _clickPos = Vector3.zero;
        private Camera _mainCamera;
        [SerializeField]private bool _enabled = false;

        private void Awake()
        {
            if(ceilTemplate==null) { Debug.LogError("ceilTemplate is not assigned!");}
            if(ceilTexture == null) { Debug.LogError("ceilTexture is not assigned!");}

            _mainCamera = Camera.main;
            Instance = this;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (!_enabled) return;


            if (Input.GetMouseButtonDown(0))
            {
                _clickPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _deltaPos = Input.mousePosition - _clickPos;
                _clickPos = Input.mousePosition;

                Vector3 pos = _clickPos;
                pos.z = 0f - _mainCamera.transform.position.z;
                Vector2 fromPos = _mainCamera.ScreenToWorldPoint(pos - _deltaPos);
                Vector2 toPos = _mainCamera.ScreenToWorldPoint(pos);

                CutRoundedLine(fromPos, toPos, cutRadius, false);
            }
        }


        #region Public API

        public void Enable() => _enabled = true;
        public void Disable() => _enabled = false;

        #endregion

        #region Callbacks

        public void Regenerate()
        {
            Clear();
            GenerateMesh();
            Enable();
        }

        #endregion


        #region Private Methods

        private void Clear()
        {
            for (int a = 0; a < transform.childCount; ++a)
            {
                Destroy(transform.GetChild(a).gameObject);
            }
        }


        private void GenerateMesh()
        {
            int counter = 0;
            float num = (float)Screen.width * 1f / (float)Screen.height;
            for (int k = 0; k < soilColumns * soilMultiple; k++)
            {
                for (int l = 0; l < soilRows * soilMultiple; l++)
                {

                    GameObject gameObject = UnityEngine.Object.Instantiate(ceilTemplate, new Vector3(0f, 0f, 0f), Quaternion.identity);
                    gameObject.transform.SetParent(transform);
                    gameObject.name = "Ceil " + counter++;

                    Vector3 pos = transform.position;
                    Ceil component = gameObject.GetComponent<Ceil>();
                    component.SetClipperController(this);
                    component.Awake();
                    component.CreateMeshAt(k, l, ceilTexture, width, height);
                    //component.CreateMeshAt((int)pos.x + k, (int)pos.y + l, ceilTexture, width, height);


                    if (l >= soilRows * soilMultiple - 2 && num <= 0.5f)
                    {
                        gameObject = UnityEngine.Object.Instantiate(ceilTemplate, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        gameObject.transform.SetParent(transform);
                        gameObject.name = "Ceil " + counter++;

                        pos = transform.position;
                        component = gameObject.GetComponent<Ceil>();
                        component.SetClipperController(this);
                        component.Awake();
                        component.CreateMeshAt( k, l + 2, ceilTexture, width, height);
                        //component.CreateMeshAt((int)pos.x + k, (int)pos.y + l + 2, ceilTexture, width, height);
                    }
                }
            }

        }


        private void CutRoundedLine(Vector2 fromPos, Vector2 toPos, float radius, bool animated)
        {
            Vector2 vector = toPos - fromPos;
            RaycastHit2D[] array = Physics2D.CircleCastAll(fromPos, radius, vector.normalized, vector.magnitude);
            if (array.Length > 0)
            {
                DateTime now = DateTime.Now;
                List<IntPoint> cutterTouchPath = GetCutterTouchPath(toPos, fromPos, radius);
                RaycastHit2D[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    RaycastHit2D raycastHit2D = array2[i];
                    Ceil component = raycastHit2D.collider.gameObject.GetComponent<Ceil>();
                    cutHole(toPos, component, cutterTouchPath, animated, radius);
                }
            }
        }


        private List<IntPoint> GetCutterTouchPath(Vector2 toPos, Vector2 fromPos, float radius)
        {
            int num = 8 + 3 * (int)radius;
            float scale = 100f;
            radius *= scale;
            float num2 = Mathf.Atan2(toPos.y - fromPos.y, toPos.x - fromPos.x);
            List<IntPoint> list = new List<IntPoint>(num);
            for (int i = 0; i < num; i++)
            {
                float f = (float)Math.PI * (float)i / (float)num + num2 + (float)Math.PI / 2f;
                list.Add(new IntPoint(scale * fromPos.x + radius * Mathf.Cos(f), scale * fromPos.y + radius * Mathf.Sin(f)));
            }
            for (int j = 0; j < num; j++)
            {
                float f2 = (float)Math.PI * (float)j / (float)num + num2 + 4.712389f;
                list.Add(new IntPoint(scale * toPos.x + radius * Mathf.Cos(f2), scale * toPos.y + radius * Mathf.Sin(f2)));
            }
            return list;
        }


        private void cutHole(Vector2 cutPosition, Ceil ceil, List<IntPoint> cutterPath, bool animated, float radius)
        {
            Clipper clipper = new Clipper();
            List<List<IntPoint>> list = new List<List<IntPoint>>(1);
            list.Add(ceil.path);
            clipper.AddPaths(list, PolyType.ptSubject, closed: true);
            List<List<IntPoint>> list2 = new List<List<IntPoint>>(1);
            list2.Add(cutterPath);
            clipper.AddPaths(list2, PolyType.ptClip, closed: true);
            List<List<IntPoint>> list3 = new List<List<IntPoint>>();
            clipper.Execute(ClipType.ctDifference, list3, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
            list3 = Clipper.SimplifyPolygons(list3, PolyFillType.pftPositive);
            if (list3.Count == 0)
            {
                ceil.gameObject.SetActive(value: false);
                return;
            }
            float num = ceil.area;
            int num2 = 0;
            foreach (List<IntPoint> item in list3)
            {
                if (num2 == 0)
                {
                    bool flag = ceil.UpdateMesh(list3[0]);
                    num -= ceil.area;
                    if (!flag)
                    {
                        ceil.gameObject.SetActive(value: false);
                    }
                }
                else
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate(ceil.gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
                    gameObject.transform.SetParent(transform);
                    gameObject.name = "Ceil";
                    Ceil component = gameObject.GetComponent<Ceil>();
                    bool flag = component.UpdateMesh(item);
                    num -= component.area;
                }
                num2++;
            }
            if (animated)
            {
                int num3 = Mathf.CeilToInt(num * 0.25f);
                if (num3 > 0)
                {
                    //do something in end line 
                    //eg : play  effect
                }
            }
        }

        #endregion


        private void OnDrawGizmos()
        {
            Vector3 border = new Vector3(1, 1, 0);
            Vector3 center = new Vector3(
                transform.position.x + width / 2, 
                transform.position.y + height / 2, 
                transform.position.z);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, new Vector3(width+border.x, height+border.y, 2f));
        }

    }
}
