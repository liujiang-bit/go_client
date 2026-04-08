using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    [ExecuteInEditMode]
    public class LodCastle : LodBase
    {
        private struct RoadInfo
        {
            public int id;

            public Vector3 pos;

            public Vector2 w_h;

            public Vector2 uv1;

            public Vector2 uv2;

            public Vector2 uv3;

            public Vector2 uv4;

            public Color color;

            public string idName;

            public Transform colliderTrans;

            public RoadInfo(int id, Vector3 pos, Vector2 wh, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, string idName, Color c, Transform t)
            {
                this.id = id;
                this.pos = pos;
                this.w_h = wh;
                this.uv1 = uv1;
                this.uv2 = uv2;
                this.uv3 = uv3;
                this.uv4 = uv4;
                this.idName = idName;
                this.color = c;
                this.colliderTrans = t;
            }

            public void Set(int id, Vector3 pos, Vector2 wh, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, string idName, Color c)
            {
                this.id = id;
                this.pos = pos;
                this.w_h = wh;
                this.uv1 = uv1;
                this.uv2 = uv2;
                this.uv3 = uv3;
                this.uv4 = uv4;
                this.idName = idName;
                this.color = c;
                this.colliderTrans.localPosition = pos;
            }

            public void SetCollider(Transform t)
            {
                this.colliderTrans = t;
            }
        }

        protected TileCollideManager m_tile_collide;

        public AnimationCurve m_scale_curve;

        public float m_max_scale = 7f;

        private Transform roadTrans;

        private Dictionary<int, LodCastle.RoadInfo> roadInfoDic = new Dictionary<int, LodCastle.RoadInfo>();

        private List<int> allRoadIds = new List<int>();

        private void Awake()
        {
            this.m_tile_collide = base.GetComponent<TileCollideManager>();
        }

        public override void UpdateLod()
        {
            int currentLodLevel = base.GetCurrentLodLevel();
            int previousLodLevel = base.GetPreviousLodLevel();
            if (currentLodLevel <= 2)
            {
                this.m_tile_collide.SetScale(Mathf.Min(this.m_scale_curve.Evaluate(Common.GetLodDistance()), this.m_max_scale), false);
            }
            else if (currentLodLevel >= 3)
            {
            }
            base.UpdateLod();
        }

        public static void SetMaxScaleS(LodCastle self, float max_scale)
        {
            self.SetMaxScale(max_scale);
        }

        private void SetMaxScale(float max_scale)
        {
            this.m_max_scale *= max_scale;
        }

        public static float GetCurrentCityScaleS(LodCastle self)
        {
            return self.GetCurrentCityScale();
        }

        private float GetCurrentCityScale()
        {
            return this.m_scale_curve.Evaluate(Common.GetLodDistance());
        }

        public static void ForceUpdateScaleS(LodCastle self)
        {
            self.m_tile_collide.SetScale(Mathf.Min(self.m_scale_curve.Evaluate(Common.GetLodDistance()), self.m_max_scale), true);
        }

        public static void CheckRoadInOneS(LodCastle self, int[] allIds, Vector3[] allPos, Vector2[] allUV, string[] allIdNames, Action<GameObject> action, GameObject obj = null)
        {
            self.CheckRoadInOne(allIds, allPos, allUV, allIdNames, action, obj);
        }

        private void CheckRoadInOne(int[] allIds, Vector3[] allPos, Vector2[] allUV, string[] allIdNames, Action<GameObject> action, GameObject obj)
        {
            if (obj == null)
            {
                CoreUtils.assetService.Instantiate("RoadInOne", (GameObject gameobject) =>
                {
                    gameobject.name = "RoadInOne";
                    this.roadTrans = gameobject.transform;
                    CheckRoadInOne(allIds, allPos, allUV, allIdNames, action, gameobject);
                });
                return;
            }
            MeshFilter component = obj.GetComponent<MeshFilter>();
            MeshRenderer component2 = obj.GetComponent<MeshRenderer>();
            obj.transform.parent = null;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            List<int> list = new List<int>();
            this.allRoadIds = new List<int>(allIds);
            Dictionary<int, int> dictionary = new Dictionary<int, int>(allIds.Length);
            for (int i = 0; i < allIds.Length; i++)
            {
                dictionary[allIds[i]] = i;
            }
            foreach (int current in this.roadInfoDic.Keys)
            {
                if (!dictionary.ContainsKey(current))
                {
                    list.Add(current);
                }
            }
            this.RemoveRoadCollider(list.ToArray());
            int num = allIds.Length;
            for (int j = 0; j < num; j++)
            {
                if (this.roadInfoDic.ContainsKey(allIds[j]))
                {
                    LodCastle.RoadInfo value = this.roadInfoDic[allIds[j]];
                    value.Set(allIds[j], allPos[j], allUV[5 * j], allUV[5 * j + 1], allUV[5 * j + 2], allUV[5 * j + 3], allUV[5 * j + 4], allIdNames[j], Color.white);
                    this.roadInfoDic[allIds[j]] = value;
                }
                else
                {
                    this.CreateRoadCollider(allIdNames[j], allPos[j], (Transform trans) =>
                    {
                        LodCastle.RoadInfo value2 = new LodCastle.RoadInfo(allIds[j], allPos[j], allUV[5 * j], allUV[5 * j + 1], allUV[5 * j + 2], allUV[5 * j + 3], allUV[5 * j + 4], allIdNames[j], Color.white, trans);
                        this.roadInfoDic[allIds[j]] = value2;
                    });
                }
            }
            component.mesh = this.CreateRoadMeshInOne();
            obj.transform.parent = base.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            action?.Invoke(obj);
        }

        private Mesh CreateRoadMeshInOne()
        {
            Mesh mesh = new Mesh();
            mesh.name = "road";
            int count = this.allRoadIds.Count;
            Vector3[] array = new Vector3[count * 4];
            Vector2[] array2 = new Vector2[count * 4];
            int[] array3 = new int[count * 6];
            Color[] array4 = new Color[count * 4];
            for (int i = 0; i < count; i++)
            {
                LodCastle.RoadInfo roadInfo = this.roadInfoDic[this.allRoadIds[i]];
                float num = roadInfo.w_h.x / 2f;
                float num2 = roadInfo.w_h.y / 2f;
                array[4 * i] = new Vector3(roadInfo.pos.x - num, roadInfo.pos.y, roadInfo.pos.z - num2);
                array[4 * i + 1] = new Vector3(roadInfo.pos.x + num, roadInfo.pos.y, roadInfo.pos.z - num2);
                array[4 * i + 2] = new Vector3(roadInfo.pos.x + num, roadInfo.pos.y, roadInfo.pos.z + num2);
                array[4 * i + 3] = new Vector3(roadInfo.pos.x - num, roadInfo.pos.y, roadInfo.pos.z + num2);
                array2[4 * i] = roadInfo.uv1;
                array2[4 * i + 1] = roadInfo.uv2;
                array2[4 * i + 2] = roadInfo.uv3;
                array2[4 * i + 3] = roadInfo.uv4;
                array3[6 * i] = 4 * i;
                array3[6 * i + 1] = 4 * i + 2;
                array3[6 * i + 2] = 4 * i + 1;
                array3[6 * i + 3] = 4 * i;
                array3[6 * i + 4] = 4 * i + 3;
                array3[6 * i + 5] = 4 * i + 2;
                array4[4 * i] = roadInfo.color;
                array4[4 * i + 1] = roadInfo.color;
                array4[4 * i + 2] = roadInfo.color;
                array4[4 * i + 3] = roadInfo.color;
            }
            mesh.vertices = array;
            mesh.uv = array2;
            mesh.triangles = array3;
            mesh.colors = array4;
            return mesh;
        }

        private void CreateRoadCollider(string name, Vector3 pos, Action<Transform> action)
        {
            CoreUtils.assetService.Instantiate("RoadCollider", (GameObject gameObject) =>
            {
                gameObject.name = name;
                Transform transform = gameObject.transform;
                transform.parent = this.roadTrans;
                transform.localEulerAngles = Vector3.zero;
                transform.localPosition = pos;
                action?.Invoke(transform);
            });
        }

        private void RemoveRoadCollider(int[] toRemove)
        {
            for (int i = 0; i < toRemove.Length; i++)
            {
                Transform colliderTrans = this.roadInfoDic[toRemove[i]].colliderTrans;
                if (colliderTrans != null)
                {
                    CoreUtils.assetService.Destroy(colliderTrans.gameObject);
                }
                this.roadInfoDic.Remove(toRemove[i]);
            }
        }

        public static void SetRoadShowS(LodCastle self, int[] roadList, bool isShow)
        {
            self.SetRoadShow(roadList, isShow);
        }

        private void SetRoadShow(int[] roadList, bool isShow)
        {
            MeshFilter component = this.roadTrans.GetComponent<MeshFilter>();
            Mesh mesh = component.mesh;
            Color[] colors = mesh.colors;
            List<int> list = new List<int>(roadList);
            for (int i = 0; i < colors.Length; i += 4)
            {
                LodCastle.RoadInfo roadInfo = this.roadInfoDic[this.allRoadIds[i / 4]];
                if (list.Contains(roadInfo.id))
                {
                    Color color = new Color(1f, 1f, 1f, (!isShow) ? 0f : 1f);
                    colors[i] = color;
                    colors[i + 1] = color;
                    colors[i + 2] = color;
                    colors[i + 3] = color;
                    roadInfo.color = color;
                }
            }
            mesh.colors = colors;
            component.mesh = mesh;
        }
    }
}