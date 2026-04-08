using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapCloudMgr : LodBase
    {
        [Serializable]
        public class CloudLayerConfig
        {
            public float LodDistance;
        }

        private static MapCloudMgr s_instance;

        public float m_highDistance;

        public MapCloudMgr.CloudLayerConfig[] m_layerConfigs;

        public float m_minSpawnTime;

        public Vector2 m_spawnTimeRange;

        public Vector2 m_scaleValueRange;

        public float m_sizeValue;

        public Vector2 m_startFadeTimeRange;

        public int m_midMaxCount;

        public float m_midScaleRateBase;

        public Vector2 m_midSpeedRangeBase;

        public Vector2 m_midStartScaleRange;

        public Vector2 m_midScaleSpeedRange;

        public Vector2 m_midIdleTimeRange;

        public Vector2 m_midGenDxfRange;

        public string[] m_midPrefabNames;

        public int m_highMaxCount;

        public float m_highScaleRateBase;

        public Vector2 m_highSpeedRangeBase;

        public Vector2 m_highStartScaleRange;

        public Vector2 m_highScaleSpeedRange;

        public Vector2 m_highIdleTimeRange;

        public Vector2 m_highGenDxfRange;

        public string[] m_highPrefabNames;

        private int m_curLayer = -1;

        private float m_curDxf;

        private List<MapCloud>[] m_layerObjs;

        private float m_spawnCountdown = -1f;

        private bool m_isLoadingCloud;

        private Vector3 m_lastCameraPos = Vector3.zero;

        public static MapCloudMgr GetInstance()
        {
            return MapCloudMgr.s_instance;
        }

        private void Awake()
        {
            if (MapCloudMgr.s_instance != null)
            {
                base.gameObject.SetActive(false);
                UnityEngine.Object.Destroy(base.gameObject);
            }
            else
            {
                MapCloudMgr.s_instance = this;
            }
        }

        private new void Start()
        {
            this.m_layerObjs = new List<MapCloud>[this.m_layerConfigs.Length];
            for (int i = 0; i < this.m_layerObjs.Length; i++)
            {
                this.m_layerObjs[i] = new List<MapCloud>();
            }
            base.Start();
        }

        private new void OnDestroy()
        {
            if (MapCloudMgr.s_instance == this)
            {
                MapCloudMgr.s_instance = null;
            }
            base.OnDestroy();
        }

        private void Update()
        {
            try
            {
                if (Camera.main)
                {
                    for (int i = 0; i < this.m_layerObjs.Length; i++)
                    {
                        List<MapCloud> list = this.m_layerObjs[i];
                        int j = 0;
                        while (j < list.Count)
                        {
                            MapCloud mapCloud = list[j];
                            if (mapCloud == null)
                            {
                                list.RemoveAt(j);
                            }
                            else
                            {
                                mapCloud.UpdateCloud();
                                if (mapCloud.NeedDestroy())
                                {
                                    CoreUtils.assetService.Destroy(mapCloud.gameObject);
                                    list.RemoveAt(j);
                                }
                                else
                                {
                                    j++;
                                }
                            }
                        }
                    }
                    Vector3 position = Camera.main.transform.position;
                    if (this.m_curLayer != -1)
                    {
                        int curLayer = this.m_curLayer;
                        List<MapCloud> list2 = this.m_layerObjs[curLayer];
                        if (list2.Count < 3 && this.m_spawnCountdown > this.m_minSpawnTime)
                        {
                            this.m_spawnCountdown = this.m_minSpawnTime;
                        }
                        this.m_spawnCountdown -= Time.deltaTime;
                        if (this.m_spawnCountdown < 0f && !this.m_isLoadingCloud)
                        {
                            float dxf = this.m_curDxf;
                            float num;
                            if (dxf >= this.m_highDistance)
                            {
                                num = (float)this.m_highMaxCount;
                            }
                            else
                            {
                                num = (float)this.m_midMaxCount;
                            }
                            if (this.m_lastCameraPos != position)
                            {
                                this.m_spawnCountdown = this.m_minSpawnTime;
                            }
                            else if ((float)list2.Count <= num / 2f)
                            {
                                this.m_spawnCountdown = this.m_minSpawnTime;
                            }
                            else
                            {
                                this.m_spawnCountdown = UnityEngine.Random.Range(this.m_spawnTimeRange.x, this.m_spawnTimeRange.y);
                            }
                            if ((float)list2.Count < num)
                            {
                                string cloudPrefabName = this.GetCloudPrefabName(this.m_curDxf);
                                this.m_isLoadingCloud = true;
                                CoreUtils.assetService.Instantiate(cloudPrefabName, (GameObject obj) =>
                                {
                                    if (this == null)
                                    {
                                        UnityEngine.Object.Destroy(obj);
                                        return;
                                    }
                                    this.m_isLoadingCloud = false;
                                    if (curLayer != this.m_curLayer)
                                    {
                                        UnityEngine.Object.Destroy(obj);
                                        return;
                                    }
                                    GameObject gameObject = obj as GameObject;
                                    MapCloud component = gameObject.GetComponent<MapCloud>();
                                    Camera main = Camera.main;
                                    float num2;
                                    float speed;
                                    float num4;
                                    float scaleSpeed;
                                    bool needAdjustScalePos;
                                    float idleTime;
                                    float num5;
                                    if (dxf >= this.m_highDistance)
                                    {
                                        num2 = this.m_highScaleRateBase * dxf / this.m_layerConfigs[0].LodDistance;
                                        float num3 = num2;
                                        speed = UnityEngine.Random.Range(num3 * this.m_highSpeedRangeBase.x, num3 * this.m_highSpeedRangeBase.y);
                                        num4 = UnityEngine.Random.Range(this.m_highStartScaleRange.x, this.m_highStartScaleRange.y);
                                        scaleSpeed = (1f - num4) / UnityEngine.Random.Range(this.m_highScaleSpeedRange.x, this.m_highScaleSpeedRange.y);
                                        needAdjustScalePos = false;
                                        idleTime = UnityEngine.Random.Range(this.m_highIdleTimeRange.x, this.m_highIdleTimeRange.y);
                                        num5 = dxf * UnityEngine.Random.Range(this.m_highGenDxfRange.x, this.m_highGenDxfRange.y);
                                    }
                                    else
                                    {
                                        num2 = this.m_midScaleRateBase * dxf / this.m_layerConfigs[0].LodDistance;
                                        float num6 = num2;
                                        speed = UnityEngine.Random.Range(num6 * this.m_midSpeedRangeBase.x, num6 * this.m_midSpeedRangeBase.y);
                                        num4 = UnityEngine.Random.Range(this.m_midStartScaleRange.x, this.m_midStartScaleRange.y);
                                        scaleSpeed = (1f - num4) / UnityEngine.Random.Range(this.m_midScaleSpeedRange.x, this.m_midScaleSpeedRange.y);
                                        needAdjustScalePos = true;
                                        idleTime = UnityEngine.Random.Range(this.m_midIdleTimeRange.x, this.m_midIdleTimeRange.y);
                                        num5 = dxf * UnityEngine.Random.Range(this.m_midGenDxfRange.x, this.m_midGenDxfRange.y);
                                    }
                                    float num7 = num2 * UnityEngine.Random.Range(this.m_scaleValueRange.x, this.m_scaleValueRange.y);
                                    component.transform.localScale = new Vector3(num7, num7, num7);
                                    component.m_scaleRate = num7;
                                    component.m_size = new Vector2(num7 * this.m_sizeValue, num7 * this.m_sizeValue);
                                    component.m_speed = speed;
                                    component.m_idleTime = idleTime;
                                    component.m_scale = num4;
                                    component.m_scaleSpeed = scaleSpeed;
                                    component.m_needAdjustScalePos = needAdjustScalePos;
                                    float y = num5 / main.fieldOfView / 1.41f;
                                    Plane plane = new Plane(Vector3.up, new Vector3(0f, y, 0f));
                                    Vector3[] cameraCornors = Common.GetCameraCornors(main, plane);
                                    Vector3 position2 = new Vector3(0f, y, 0f);
                                    if (dxf >= this.m_highDistance)
                                    {
                                        float num8 = (cameraCornors[2].x - cameraCornors[3].x) * 0.3f;
                                        float num9 = (cameraCornors[3].z - cameraCornors[0].z) * 0.2f;
                                        position2.x = UnityEngine.Random.Range(cameraCornors[3].x - num8, cameraCornors[2].x);
                                        position2.z = UnityEngine.Random.Range(cameraCornors[0].z - num9, cameraCornors[3].z + num9);
                                    }
                                    else
                                    {
                                        position2.x = UnityEngine.Random.Range(cameraCornors[3].x, cameraCornors[2].x);
                                        position2.z = UnityEngine.Random.Range(cameraCornors[0].z, cameraCornors[3].z);
                                    }
                                    component.transform.position = position2;
                                    component.SetStart(UnityEngine.Random.Range(this.m_startFadeTimeRange.x, this.m_startFadeTimeRange.y));
                                    component.UpdateCloud();
                                    this.m_layerObjs[curLayer].Add(component);
                                });
                            }
                        }
                    }
                    this.m_lastCameraPos = position;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void UpdateLod()
        {
            float lodDistance = Common.GetLodDistance();
            if (lodDistance != this.m_curDxf)
            {
                this.m_curDxf = lodDistance;
                int num = -1;
                for (int i = 0; i < this.m_layerConfigs.Length - 1; i++)
                {
                    if (this.m_layerConfigs[i].LodDistance <= lodDistance && this.m_layerConfigs[i + 1].LodDistance >= lodDistance)
                    {
                        num = i;
                        break;
                    }
                }
                if (this.m_curLayer != num)
                {
                    for (int j = 0; j < this.m_layerObjs.Length; j++)
                    {
                        if (num != j)
                        {
                            List<MapCloud> list = this.m_layerObjs[j];
                            for (int k = 0; k < list.Count; k++)
                            {
                                MapCloud mapCloud = list[k];
                                if (mapCloud != null)
                                {
                                    mapCloud.SetFadeOut(1f);
                                }
                            }
                        }
                    }
                    this.m_curLayer = num;
                    this.m_spawnCountdown = 0.1f;
                }
            }
            base.UpdateLod();
        }

        private string GetCloudPrefabName(float dxf)
        {
            if (dxf >= this.m_highDistance)
            {
                return this.m_highPrefabNames[UnityEngine.Random.Range(0, this.m_highPrefabNames.Length)];
            }
            return this.m_midPrefabNames[UnityEngine.Random.Range(0, this.m_midPrefabNames.Length)];
        }
    }
}