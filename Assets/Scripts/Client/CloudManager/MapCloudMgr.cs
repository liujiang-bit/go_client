using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Client
{
    /// <summary>
    /// 云管理器
    /// </summary>
    public class MapCloudMgr : LevelDetailBase
    {
        [Serializable]
        public class CloudLayerConfig
        {
            public float LodDistance;
        }

        private static MapCloudMgr s_instance;

        public float m_highDistance;

        public CloudLayerConfig[] m_layerConfigs;

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
            return s_instance;
        }

        private void Awake()
        {
            if (s_instance != null)
            {
                base.gameObject.SetActive(value: false);
                UnityEngine.Object.Destroy(base.gameObject);
            }
            else
            {
                s_instance = this;
            }
        }

        private new void Start()
        {
            m_layerObjs = new List<MapCloud>[m_layerConfigs.Length];
            for (int i = 0; i < m_layerObjs.Length; i++)
            {
                m_layerObjs[i] = new List<MapCloud>();
            }
            base.Start();
        }

        private new void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
            }
            base.OnDestroy();
        }

        private void Update()
        {
            try
            {
                if ((bool)Camera.main)
                {
                    for (int i = 0; i < m_layerObjs.Length; i++)
                    {
                        List<MapCloud> list = m_layerObjs[i];
                        int num = 0;
                        while (num < list.Count)
                        {
                            MapCloud mapCloud = list[num];
                            if (mapCloud == null)
                            {
                                list.RemoveAt(num);
                            }
                            else
                            {
                                mapCloud.UpdateCloud();
                                if (mapCloud.NeedDestroy())
                                {
                                    CoreUtils.assetService.Destroy(mapCloud.gameObject);
                                    list.RemoveAt(num);
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                    }
                    Vector3 position = Camera.main.transform.position;
                    if (m_curLayer != -1)
                    {
                        int curLayer = m_curLayer;
                        List<MapCloud> list2 = m_layerObjs[curLayer];
                        if (list2.Count < 3 && m_spawnCountdown > m_minSpawnTime)
                        {
                            m_spawnCountdown = m_minSpawnTime;
                        }
                        m_spawnCountdown -= Time.deltaTime;
                        if (m_spawnCountdown < 0f && !m_isLoadingCloud)
                        {
                            float dxf = m_curDxf;
                            float num2 = (!(dxf >= m_highDistance)) ? ((float)m_midMaxCount) : ((float)m_highMaxCount);
                            if (m_lastCameraPos != position)
                            {
                                m_spawnCountdown = m_minSpawnTime;
                            }
                            else if ((float)list2.Count <= num2 / 2f)
                            {
                                m_spawnCountdown = m_minSpawnTime;
                            }
                            else
                            {
                                m_spawnCountdown = UnityEngine.Random.Range(m_spawnTimeRange.x, m_spawnTimeRange.y);
                            }
                            if ((float)list2.Count < num2)
                            {
                                string cloudPrefabName = GetCloudPrefabName(m_curDxf);
                                m_isLoadingCloud = true;
                                CoreUtils.assetService.Instantiate(cloudPrefabName, (obj)=>
                                {
                                    if (this == null)
                                    {
                                        UnityEngine.Object.Destroy(obj);
                                    }
                                    else
                                    {
                                        m_isLoadingCloud = false;
                                        if (curLayer != m_curLayer)
                                        {
                                            UnityEngine.Object.Destroy(obj);
                                        }
                                        else
                                        {
                                            GameObject gameObject = obj as GameObject;
                                            MapCloud component = gameObject.GetComponent<MapCloud>();
                                            Camera main = Camera.main;
                                            float num3;
                                            float speed;
                                            float num5;
                                            float scaleSpeed;
                                            bool needAdjustScalePos;
                                            float idleTime;
                                            float num6;
                                            if (dxf >= m_highDistance)
                                            {
                                                num3 = m_highScaleRateBase * dxf / m_layerConfigs[0].LodDistance;
                                                float num4 = num3;
                                                speed = UnityEngine.Random.Range(num4 * m_highSpeedRangeBase.x, num4 * m_highSpeedRangeBase.y);
                                                num5 = UnityEngine.Random.Range(m_highStartScaleRange.x, m_highStartScaleRange.y);
                                                scaleSpeed = (1f - num5) / UnityEngine.Random.Range(m_highScaleSpeedRange.x, m_highScaleSpeedRange.y);
                                                needAdjustScalePos = false;
                                                idleTime = UnityEngine.Random.Range(m_highIdleTimeRange.x, m_highIdleTimeRange.y);
                                                num6 = dxf * UnityEngine.Random.Range(m_highGenDxfRange.x, m_highGenDxfRange.y);
                                            }
                                            else
                                            {
                                                num3 = m_midScaleRateBase * dxf / m_layerConfigs[0].LodDistance;
                                                float num7 = num3;
                                                speed = UnityEngine.Random.Range(num7 * m_midSpeedRangeBase.x, num7 * m_midSpeedRangeBase.y);
                                                num5 = UnityEngine.Random.Range(m_midStartScaleRange.x, m_midStartScaleRange.y);
                                                scaleSpeed = (1f - num5) / UnityEngine.Random.Range(m_midScaleSpeedRange.x, m_midScaleSpeedRange.y);
                                                needAdjustScalePos = true;
                                                idleTime = UnityEngine.Random.Range(m_midIdleTimeRange.x, m_midIdleTimeRange.y);
                                                num6 = dxf * UnityEngine.Random.Range(m_midGenDxfRange.x, m_midGenDxfRange.y);
                                            }
                                            float num8 = num3 * UnityEngine.Random.Range(m_scaleValueRange.x, m_scaleValueRange.y);
                                            component.transform.localScale = new Vector3(num8, num8, num8);
                                            component.m_scaleRate = num8;
                                            component.m_size = new Vector2(num8 * m_sizeValue, num8 * m_sizeValue);
                                            component.m_speed = speed;
                                            component.m_idleTime = idleTime;
                                            component.m_scale = num5;
                                            component.m_scaleSpeed = scaleSpeed;
                                            component.m_needAdjustScalePos = needAdjustScalePos;
                                            float y = num6 / main.fieldOfView / 1.41f;
                                            Plane plane = new Plane(Vector3.up, new Vector3(0f, y, 0f));
                                            Vector3[] cameraCornors = Common.GetCameraCornors(main, plane);
                                            Vector3 position2 = new Vector3(0f, y, 0f);
                                            if (dxf >= m_highDistance)
                                            {
                                                float num9 = (cameraCornors[2].x - cameraCornors[3].x) * 0.3f;
                                                float num10 = (cameraCornors[3].z - cameraCornors[0].z) * 0.2f;
                                                position2.x = UnityEngine.Random.Range(cameraCornors[3].x - num9, cameraCornors[2].x);
                                                position2.z = UnityEngine.Random.Range(cameraCornors[0].z - num10, cameraCornors[3].z + num10);
                                            }
                                            else
                                            {
                                                position2.x = UnityEngine.Random.Range(cameraCornors[3].x, cameraCornors[2].x);
                                                position2.z = UnityEngine.Random.Range(cameraCornors[0].z, cameraCornors[3].z);
                                            }
                                            component.transform.position = position2;
                                            component.SetStart(UnityEngine.Random.Range(m_startFadeTimeRange.x, m_startFadeTimeRange.y));
                                            component.UpdateCloud();
                                            m_layerObjs[curLayer].Add(component);
                                        }
                                    }
                                });
                            }
                        }
                    }
                    m_lastCameraPos = position;
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
            if (lodDistance != m_curDxf)
            {
                m_curDxf = lodDistance;
                int num = -1;
                for (int i = 0; i < m_layerConfigs.Length - 1; i++)
                {
                    if (m_layerConfigs[i].LodDistance <= lodDistance && m_layerConfigs[i + 1].LodDistance >= lodDistance)
                    {
                        num = i;
                        break;
                    }
                }
                if (m_curLayer != num)
                {
                    for (int j = 0; j < m_layerObjs.Length; j++)
                    {
                        if (num == j)
                        {
                            continue;
                        }
                        List<MapCloud> list = m_layerObjs[j];
                        for (int k = 0; k < list.Count; k++)
                        {
                            MapCloud mapCloud = list[k];
                            if (mapCloud != null)
                            {
                                mapCloud.SetFadeOut(1f);
                            }
                        }
                    }
                    m_curLayer = num;
                    m_spawnCountdown = 0.1f;
                }
            }
            base.UpdateLod();
        }

        private string GetCloudPrefabName(float dxf)
        {
            if (dxf >= m_highDistance)
            {
                return m_highPrefabNames[UnityEngine.Random.Range(0, m_highPrefabNames.Length)];
            }
            return m_midPrefabNames[UnityEngine.Random.Range(0, m_midPrefabNames.Length)];
        }
    }
}