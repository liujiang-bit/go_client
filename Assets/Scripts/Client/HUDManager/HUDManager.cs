using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using System.Linq;
using System;
using System.Reflection;
using UnityEngine.UI;

namespace Client
{
    public enum HUDLayer
    {
        city,
        citymenu,
        world,
        fight
    }

    public class HUDManager : MonoSingleton<HUDManager>
    {
        private List<HUDUI> m_HUDInfos = new List<HUDUI>();
        private List<HUDUI> m_sortingOrders = new List<HUDUI>();

        private Transform[] m_layers = new Transform[4];

        private bool isInitedCamera;
        private Camera uiCam;
        private RectTransform canvasRect;

        private Vector2 tmpScreenPos;
        private Vector2 tmpAnchoredPos;
        private float tmpCameraDxf;
        private float lastCameraDxf;
        
        private Dictionary<long,HUDUI> m_hasHud = new Dictionary<long,HUDUI>();


        public bool HasHud(long id)
        {
            return m_hasHud.ContainsKey(id);
        }

        public void AddHud(long id,HUDUI hud)
        {
            if (!m_hasHud.ContainsKey(id))
            {
                m_hasHud.Add(id,hud);
            }
        }

        public void RemoveHud(long id)
        {
            if (m_hasHud.ContainsKey(id))
            {
                HUDUI hud;
                if (m_hasHud.TryGetValue(id, out hud))
                {
                    m_hasHud.Remove(id);
                    hud.Close();
                }
            }
        }

        /// <summary>
        /// 打开一个HUD
        /// </summary>
        public void ShowHud(HUDUI hud,object data = null)
        {
            Debug.LogFormat("ShowHud:{0}", hud.assetName);
            InitCamera();
            if (!hud.targetObj&&!hud.isFollowWorldPos)
            {
                CoreUtils.logService.Warn("该HUD跟随的Obj不存在或已经销毁");
                return;
            }
            m_HUDInfos.Add(hud);
            LoadUIAsync(hud, data);
        }

        /// <summary>
        /// 关闭单个HUD
        /// </summary>
        /// <param name="hashCode"></param>
        public void CloseSingleHud(ref HUDUI hud)
        {
            if (hud != null)
            {
                hud.Close();
            }
            m_HUDInfos.Remove(hud);
        }

        /// <summary>
        /// 关闭一个UIInfo下的HUD
        /// </summary>
        /// <param name="layer"></param>
        public void CloseAllFromSingleUIInfo(HUDLayer layer)
        {
            for(int i = m_HUDInfos.Count-1;i>=0;i--)
            {
                if (m_HUDInfos[i].layer == layer)
                {
                    m_HUDInfos[i].Close();
                    if (m_HUDInfos[i] != null)
                    {
                        m_HUDInfos[i] = null;
                        m_HUDInfos.RemoveAt(i);
                    }
                }
            }
        }
        /// <summary>
        /// 关闭一个UIInfo下的HUD
        /// </summary>
        /// <param name="layer"></param>
        public bool ExistSingleUIInfo(HUDLayer layer)
        {
            for (int i = m_HUDInfos.Count - 1; i >= 0; i--)
            {
                if (m_HUDInfos[i].layer == layer)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 关闭所有HUD
        /// </summary>
        public void CloseAll()
        {
            for (int i = m_HUDInfos.Count - 1; i >= 0; i--)
            {
                if (m_HUDInfos[i] != null)
                {
                    m_HUDInfos[i].Close();
                }
                m_HUDInfos[i] = null;
                m_HUDInfos.RemoveAt(i);
            }
        }

        public void Update()
        {
            if (m_HUDInfos.Count==0)
            {
                return;
            }
            tmpCameraDxf = WorldCamera.Instance().getCurrentCameraDxf();
            bool needSetScale = tmpCameraDxf != lastCameraDxf;
            lastCameraDxf = tmpCameraDxf;
            for(int i = m_HUDInfos.Count-1;i>=0;i--)
            {
                if(m_HUDInfos[i]==null)
                {
                    m_HUDInfos.RemoveAt(i);
                    continue;
                }
                if(m_HUDInfos[i].bDispose)
                {
                    m_HUDInfos[i].Close();
                    m_HUDInfos.RemoveAt(i);
                    continue;
                }
                if(m_HUDInfos[i].uiObj)
                {
                    if(!m_HUDInfos[i].targetObj&&!m_HUDInfos[i].isFollowWorldPos)
                    {
                        m_HUDInfos[i].Close();
                        m_HUDInfos.RemoveAt(i);
                        continue;
                    }
                    AutoAnchorUI(m_HUDInfos[i]);
                    if(needSetScale)
                    {
                        SetScale(m_HUDInfos[i]);
                    }
                    m_HUDInfos[i].CallOnUpdate();
                    if (i < m_HUDInfos.Count)
                    {
                        AutoSetActiveByLodChange(m_HUDInfos[i]);
                    }
                }
            }
        }

        #region 私有方法

        //初始化摄像机等引用
        private void InitCamera()
        {
            if (isInitedCamera)
                return;

            isInitedCamera = true;
            uiCam = CoreUtils.uiManager.GetUICamera();
            canvasRect = CoreUtils.uiManager.GetCanvas().GetComponent<RectTransform>();
            RectTransform parent = CoreUtils.uiManager.GetUILayer((int)UILayer.HUDLayer);
            m_layers[0] = parent.Find("pl_city");
            m_layers[1] = parent.Find("pl_citymenu");
            m_layers[2] = parent.Find("pl_world");
            m_layers[3] = parent.Find("pl_fight");
        }

        //固定UI锚点
        private void AutoAnchorUI(HUDUI ui)
        {
            if (!ui.autoAnchorPos || !ui.assetLoadFinish|| !isInitedCamera)
            {
                return;
            }
            if(!ui.isFollowWorldPos&& !ui.targetObj)
            {
                return;
            }
            if (!ui.IsAllowUpdatePos)
            {
                return;
            }
            Vector3 pos = ui.isFollowWorldPos ? ui.targetPos : ui.targetObj.transform.position;
            tmpScreenPos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), pos);         
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, tmpScreenPos, uiCam, out tmpAnchoredPos))
            {
                if(ui.rectTran!=null)
                {
                    if(ui.autoAnchorScale)
                    {
                        float curScale = ui.InitCameraDxf / tmpCameraDxf;
                        ui.rectTran.anchoredPosition = tmpAnchoredPos + ui.posOffset* curScale;
                    }
                    else
                    {
                        ui.rectTran.anchoredPosition = tmpAnchoredPos + ui.posOffset;
                    }

                }

            }

            if(ui.canvas)
            {
                ui.yAxis = ui.isFollowWorldPos ? ui.targetPos.z : ui.targetObj.transform.position.z;
                ui.canvas.sortingOrder = GetSortingOrder(ui);
            }
        }

        //设置缩放
        private void SetScale(HUDUI ui)
        {
            if (!ui.autoAnchorScale || !ui.assetLoadFinish || !isInitedCamera)
            {
                return;
            }
            float curScale = ui.InitCameraDxf/ tmpCameraDxf;
            if(ui.rectTran!=null)
                ui.rectTran.localScale = new Vector3(curScale, curScale, 1f);
        }

        private void AutoSetActiveByLodChange(HUDUI ui)
        {
            if(ui.uiObj==null)
            {
                return;
            }
            if(ui.minLodCameraDxf> tmpCameraDxf || ui.maxLodCameraDxf< tmpCameraDxf)
            {
                if(!ui.isOutOfLodRange)
                {
                    ui.isOutOfLodRange = true;
                    ui.onOutLodRange?.Invoke(true,ui);
                }
                if (ui.uiObj!=null&&ui.uiObj.activeSelf)
                    ui.uiObj.SetActive(false);
            }
            else
            {
                if (ui.isOutOfLodRange)
                {
                    ui.isOutOfLodRange = false;
                    ui.onOutLodRange?.Invoke(false,ui);
                }
                if (ui.uiObj != null && !ui.uiObj.activeSelf)
                    ui.uiObj.SetActive(true);
            }
        }


        //异步加载UI
        private void LoadUIAsync(HUDUI uiInfo, object data)
        {
            if(uiInfo.viewClass==null)
            {
                CreateHud(uiInfo);
            }
            else
            {
                CreateHudByViewBinder(uiInfo, data);
            }
        }

        private void CreateHudByViewBinder(HUDUI uiInfo, object data)
        {
            GameView tmp = CoreUtils.hotService.Instantiate<GameView>(uiInfo.viewClass);
            ViewBinder.Create(uiInfo.assetName, tmp, () =>
            {
                uiInfo.assetLoadFinish = true;
                GameObject uiObj = tmp.vb.gameObject;
                uiInfo.uiObj = uiObj;
                if (uiInfo.bDispose)
                {
                    uiInfo.Close();
                    return;
                }
                if (uiInfo.InitCameraDxf == -1f)
                {
                    uiInfo.InitCameraDxf = WorldCamera.Instance().getCurrentCameraDxf();
                }
                tmp.data = data;

                if (uiObj == null)
                {
                    Debug.LogError("生成失败：" + uiInfo.assetName);
                    return;
                }
                if (uiInfo.rectTran == null)
                {
                    uiInfo.rectTran = uiInfo.uiObj.GetComponent<RectTransform>();
                }
                if (!uiInfo.canvasGroup)
                {
                    uiInfo.canvasGroup = uiInfo.uiObj.GetComponent<CanvasGroup>();
                    if (!uiInfo.canvasGroup)
                        uiInfo.canvasGroup = uiInfo.uiObj.AddComponent<CanvasGroup>();
                }
                uiInfo.uiObj.transform.SetParent(m_layers[(int)uiInfo.layer]);
                uiInfo.uiObj.transform.localScale = Vector3.one;
                Canvas canvas = uiInfo.uiObj.GetComponent<Canvas>();
                if (!canvas)
                {
                    canvas = uiInfo.uiObj.AddComponent<Canvas>();
                }
                GraphicRaycaster gr = uiInfo.uiObj.GetComponent<GraphicRaycaster>();
                if (!gr)
                {
                    gr = uiInfo.uiObj.AddComponent<GraphicRaycaster>();
                }
                if (uiInfo.autoSorting || uiInfo.layer == HUDLayer.city)
                {
                    if (uiInfo.isFollowWorldPos)
                    {
                        uiInfo.yAxis = uiInfo.targetPos.z;
                    }
                    else
                    {
                        if (uiInfo.targetObj != null)
                        {
                            uiInfo.yAxis = uiInfo.targetObj.transform.position.z;
                        }
                    }
                    //uiInfo.yAxis = uiInfo.isFollowWorldPos ? uiInfo.targetPos.z : uiInfo.targetObj.transform.position.z;
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = GetSortingOrder(uiInfo);
                    uiInfo.canvas = canvas;
                }
                else if (uiInfo.sortingOrder != 0)
                {
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = uiInfo.sortingOrder;
                    uiInfo.canvas = canvas;
                }

                uiInfo.gameView = tmp;
                uiInfo.gameView.BindSingleUI(uiInfo.uiObj);
                AutoAnchorUI(uiInfo);
                SetScale(uiInfo);
                uiInfo.isOutOfLodRange = uiInfo.minLodCameraDxf > tmpCameraDxf || uiInfo.maxLodCameraDxf < tmpCameraDxf;
                uiInfo.initCallBack?.Invoke(uiInfo);
                uiInfo.onOutLodRange?.Invoke(uiInfo.isOutOfLodRange, uiInfo);
            });
        }

        private void CreateHud(HUDUI uiInfo)
        {
            CoreUtils.assetService.Instantiate(uiInfo.assetName,(GameObject uiObj)=>
            {
                uiInfo.uiObj = uiObj;
                if (uiInfo.bDispose)
                {
                    uiInfo.Close();
                    return;
                }
                uiInfo.assetLoadFinish = true;
                if (uiInfo.InitCameraDxf == -1f)
                {
                    uiInfo.InitCameraDxf = WorldCamera.Instance().getCurrentCameraDxf();
                }
                if (uiObj == null)
                {
                    Debug.LogError("生成失败：" + uiInfo.assetName);
                    return;
                }
                if (uiInfo.rectTran == null)
                {
                    uiInfo.rectTran = uiInfo.uiObj.GetComponent<RectTransform>();
                }
                if (!uiInfo.canvasGroup)
                {
                    uiInfo.canvasGroup = uiInfo.uiObj.GetComponent<CanvasGroup>();
                    if (!uiInfo.canvasGroup)
                        uiInfo.canvasGroup = uiInfo.uiObj.AddComponent<CanvasGroup>();
                }
                uiInfo.uiObj.transform.SetParent(m_layers[(int)uiInfo.layer]);
                uiInfo.uiObj.transform.localScale = Vector3.one;
                Canvas canvas = uiInfo.uiObj.GetComponent<Canvas>();
                if (!canvas)
                {
                    canvas = uiInfo.uiObj.AddComponent<Canvas>();
                }
                GraphicRaycaster gr = uiInfo.uiObj.GetComponent<GraphicRaycaster>();
                if (!gr)
                {
                    gr = uiInfo.uiObj.AddComponent<GraphicRaycaster>();
                }
                if (uiInfo.autoSorting||uiInfo.layer==HUDLayer.city)
                {
                    uiInfo.yAxis = uiInfo.isFollowWorldPos ? uiInfo.targetPos.z : uiInfo.targetObj.transform.position.z;
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = GetSortingOrder(uiInfo);
                    uiInfo.canvas = canvas;
                }
                else if (uiInfo.sortingOrder != 0)
                {
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = uiInfo.sortingOrder;
                    uiInfo.canvas = canvas;
                }

                uiInfo.isOutOfLodRange = uiInfo.minLodCameraDxf > tmpCameraDxf || uiInfo.maxLodCameraDxf < tmpCameraDxf;
                AutoAnchorUI(uiInfo);
                SetScale(uiInfo);
                uiInfo.initCallBack?.Invoke(uiInfo);
                uiInfo.onOutLodRange?.Invoke(uiInfo.isOutOfLodRange, uiInfo);
            });
        }

        private float Remap(float input1, float input2, float output1, float output2, float value)
        {
            return output1 + (value - input1) * (output2 - output1) / (input2 - input1);
        }

        private int GetSortingOrder(HUDUI ui)
        {
            if (m_sortingOrders.Count == 0)
            {
                m_sortingOrders.Add(ui);
                return 0;
            }
            if(!m_sortingOrders.Contains(ui))
            {
                m_sortingOrders.Add(ui);
            }

            int index =  m_sortingOrders.IndexOf(ui);

            bool dirty = false;
            if(m_sortingOrders.Count>index+1&& m_sortingOrders[index+1].yAxis>ui.yAxis)
            {
                dirty = true;
            }
            if(index-1>=0&& m_sortingOrders[index - 1].yAxis < ui.yAxis)
            {
                dirty = true;
            }

            if(dirty)
            {
                m_sortingOrders.Sort(SortByyAxis);
                index = m_sortingOrders.IndexOf(ui);
            }

            return index;
        }

        private int SortByyAxis(HUDUI role1,HUDUI role2)
        {
            return role2.yAxis.CompareTo(role1.yAxis);
        }

        public void RemoveFromSortingOrder(HUDUI ui)
        {
            m_sortingOrders.Remove(ui);
        }

        #endregion
    }
}

