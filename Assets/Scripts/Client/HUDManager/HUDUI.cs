using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using System;

namespace Client
{
    public class HUDUI
    {
        public string assetName;

        //受UIInfo层级管理
        public HUDLayer layer;

        public bool bDispose;

        //跟随的目标物体
        public GameObject targetObj;

        //跟随的世界坐标位置
        public Vector3 targetPos;

        //当前hud的obj实例
        public GameObject uiObj;

        //跟随的是目标物体还是世界坐标
        public bool isFollowWorldPos;

        //当前hud的rectTran实例
        public RectTransform rectTran;

        public Type viewClass;

        public GameView gameView;

        //初始化回调
        public Action<HUDUI> initCallBack = null;

        //间隔时间内调用
        public Action<HUDUI> updateCallBack = null;

        //超过Lod范围后调用
        public Action<bool,HUDUI> onOutLodRange = null;

        public bool isOutOfLodRange;

        //间隔时间
        public float updateInterval = -1;

        //开启自动对齐
        public bool autoAnchorPos = true;

        //开启自动缩放
        public bool autoAnchorScale = false;

        //Asset已经加载完成
        public bool assetLoadFinish;

        //加载层级
        public int sortingOrder;

        public float yAxis;

        //自动根据层级排列
        public bool autoSorting = false;

        //绑定的位置偏移量
        public Vector2 posOffset;

        //最小的可见相机距离
        public float minLodCameraDxf = 0;

        //最大的可见相机距离
        public float maxLodCameraDxf = float.MaxValue;

        //超过LOD范围自动隐藏
        public bool AutoSetActiveByLodChange = false;

        public CanvasGroup canvasGroup;

        public Canvas canvas;

        public float InitCameraDxf = 172f;

        public object data;

        public object viewData;

        public bool IsAllowUpdatePos = true;

        public static HUDUI Register(string assetName, Type viewClass, HUDLayer layer, GameObject target)
        {
            if(ClientUtils.hudManager!=null)
            {
                HUDUI hud = new HUDUI(assetName, viewClass, layer, target);
                return hud;
            }
            return null;
        }

        public static HUDUI Register(string assetName,Vector3 targetPos,HUDLayer layer)
        {
            if (ClientUtils.hudManager != null)
            {
                HUDUI hud = new HUDUI(assetName, targetPos, layer);
                return hud;
            }
            return null;
        }

        /// <summary>
        /// 构造一个HUDUI
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="viewClass">view类</param>
        /// <param name="layer">挂载的父级UIInfo</param>
        /// <param name="target">目标物体</param>
        public HUDUI(string assetName, Type viewClass, HUDLayer layer,GameObject target)
        {
            this.layer = layer;
            targetObj = target;
            this.viewClass = viewClass;
            this.assetName = assetName;     
        }

        public HUDUI(string assetName, Vector3 targetPos,HUDLayer layer)
        {
            this.layer = layer;
            this.targetPos = targetPos;
            this.assetName = assetName;
            this.isFollowWorldPos = true;
        }

        /// <summary>
        /// 只拷贝构造函数的前四个参数
        /// </summary>
        /// <param name="ui"></param>
        public HUDUI(HUDUI ui)
        {
            layer = ui.layer;
            targetObj = ui.targetObj;
            viewClass = ui.viewClass;
            assetName = ui.assetName;
        }

        /// <summary>
        /// 初始化完成的回调
        /// </summary>
        /// <param name="initCallBack">初始化完成的回调</param>
        /// <param name="updateCallBack">每一帧的回调</param>
        /// <returns></returns>
        public HUDUI SetInitCallback(Action<HUDUI> initCallBack)
        {
            this.initCallBack = initCallBack;
            return this;
        }

         /// <summary>
         /// 更新位置
         /// </summary>
         /// <param name="pos"></param>
         /// <returns></returns>
        public HUDUI UpdateTargetPos(Vector3 pos)
        {
            this.targetPos = pos;
            return this;
        }

        /// <summary>
        /// 是否允许更新坐标
        /// </summary>
        /// <returns></returns>
        public HUDUI SetAllowUpdatePos(bool isBool)
        {
            this.IsAllowUpdatePos = isBool;
            return this;
        }

        /// <summary>
        /// 间隔时间内调用
        /// </summary>
        /// <param name="updateCallBack"></param>
        /// <param name="interval">-1时为每一帧调用一次</param>
        /// <returns></returns>
        public HUDUI SetUpdateCallback(Action<HUDUI> updateCallBack,float interval = -1f)
        {
            this.updateCallBack = updateCallBack;
            this.updateInterval = interval;
            return this;
        }

        /// <summary>
        /// 设置绑定的位置偏移量
        /// </summary>
        /// <param name="posOffset"></param>
        /// <returns></returns>
        public HUDUI SetPosOffset(Vector2 posOffset)
        {
            this.posOffset = posOffset;
            return this;
        }

        /// <summary>
        /// 每一帧自动跟随
        /// </summary>
        /// <param name="needAutoAnchor"></param>
        /// <returns></returns>
        public HUDUI SetPositionAutoAnchor(bool autoAnchorPos)
        {
            this.autoAnchorPos = autoAnchorPos;
            return this;
        }

        /// <summary>
        /// 是否跟随相机远近自动缩放
        /// </summary>
        /// <returns></returns>
        public HUDUI SetScaleAutoAnchor(bool autoAnchorScale)
        {
            this.autoAnchorScale = autoAnchorScale;
            return this;
        }

        /// <summary>
        /// 设置可见lod范围
        /// </summary>
        /// <param name="minLevel"></param>
        /// <param name="maxLevel"></param>
        /// <param name="AutoSetActiveByLodChange"></param>
        /// <returns></returns>
        public HUDUI SetLodLevel(int minLevel,int maxLevel,Action<bool, HUDUI> onOutLodRange = null, bool AutoSetActiveByLodChange = true)
        {
            this.minLodCameraDxf = LevelDetailCamera.instance.m_lod_array[minLevel];
            this.maxLodCameraDxf = LevelDetailCamera.instance.m_lod_array[maxLevel];
            this.AutoSetActiveByLodChange = AutoSetActiveByLodChange;
            this.onOutLodRange = onOutLodRange;
            return this;
        }

        /// <summary>
        /// 设置默认的相机距离，以便适配合适的缩放
        /// </summary>
        /// <param name="cameraDxf"></param>
        /// <returns></returns>
        public HUDUI SetDefaultCameraDxf(float cameraDxf)
        {
            this.InitCameraDxf = cameraDxf;
            return this;
        }

        /// <summary>
        /// 设置该hud在lod范围内才可见
        /// </summary>
        /// <param name="minDxf"></param>
        /// <param name="maxDxf"></param>
        /// <param name="AutoSetActiveByLodChange"></param>
        /// <returns></returns>
        public HUDUI SetCameraLodDist(float minDxf, float maxDxf, Action<bool, HUDUI> onOutLodRange = null, bool AutoSetActiveByLodChange = true)
        {
            this.minLodCameraDxf = minDxf;
            this.maxLodCameraDxf = maxDxf;
            this.AutoSetActiveByLodChange = AutoSetActiveByLodChange;
            this.onOutLodRange = onOutLodRange;
            return this;
        }

        /// <summary>
        /// 设置跟随的物体
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public HUDUI SetTargetGameObject(GameObject go)
        {
            if(go!=null)
            {
                targetObj = go;
                this.isFollowWorldPos = false;
            }
            return this;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HUDUI SetData(object data)
        {
            this.data = data;
            return this;
        }

        public HUDUI SetSortingOrder(int order)
        {
            this.sortingOrder = order;
            return this;
        }

        public HUDUI Show()
        {
            ClientUtils.hudManager.ShowHud(this);
            return this;
        }

        public HUDUI Close()
        {
            Debug.LogFormat("close:{0}", assetName);
            HUDManager.Instance().RemoveFromSortingOrder(this);
            if (uiObj)
            {
                var animator = uiObj.GetComponent<Animator>();
                float fDelayTime = 0.0f;
                if (animator != null)
                {
                    animator.Play("Close");
                    fDelayTime = 0.15f;

                    if (gameView != null)
                    {
                        if (!gameView.vb.IsAniCloseing)
                        {
                            gameView.vb.IsAniCloseing = true;
                            gameView.vb.onCloseAnimatorCallback = () =>
                            {
                                CoreUtils.assetService.Destroy(uiObj);
                            };
                        }
                    }
                    else
                    {
                        CoreUtils.assetService.Destroy(uiObj);
                    }
                }
                else
                {
                    CoreUtils.assetService.Destroy(uiObj, fDelayTime);
                }
            }
            bDispose = true;
            return this;
        }

        /// <summary>
        /// 是否在可见lod范围内
        /// </summary>
        /// <returns></returns>
        public bool IsThisInLodLevel(float cameraDxf)
        {
            return cameraDxf >= minLodCameraDxf && cameraDxf <= maxLodCameraDxf;
        }

        private float lastCallbackTime;
        public void CallOnUpdate()
        {
            if(updateInterval<0)
            {
                updateCallBack?.Invoke(this);
                return;
            }
            if(Time.realtimeSinceStartup-lastCallbackTime >= updateInterval)
            {
                lastCallbackTime = Time.realtimeSinceStartup;
                updateCallBack?.Invoke(this);
            }
        }
    }
}

