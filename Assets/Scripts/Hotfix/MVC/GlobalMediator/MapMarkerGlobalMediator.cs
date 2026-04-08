// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    MapMarkerGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using SprotoType;
using PureMVC.Interfaces;
using Skyunion;
using Data;
using Client;

namespace Game {
    public class MapMarkerGlobalMediator : GameMediator {
        #region Member

        public static string NameMediator = "MapMarkerGlobalMediator";

        private MapMarkerProxy mapMarkerProxy;
        private FogSystemMediator fogSystemMediator;

        private Transform m_land_root;
        private Transform m_hud_root;

        private bool updateFlag = true;
        private float curCamHeight;
        private float uiIconMinDxf;
        private float uiIconMaxDxf;
        private float uiPopMinDxf;
        private float uiPopMaxDxf;

        List<long> loadingEffectIdList = new List<long>();
        Dictionary<long, GameObject> loadedEffectDic = new Dictionary<long, GameObject>();
        List<long> wantDestroyEffectIdList = new List<long>();

        List<long> loadingUIIconIdList = new List<long>();
        Dictionary<long, GameObject> loadedUIIconDic = new Dictionary<long, GameObject>();
        Dictionary<long, MapElementUI> loadedUIIconElementDic = new Dictionary<long, MapElementUI>();
        List<long> wantDestroyUIIconIdList = new List<long>();

        List<long> loadingUIPopIdList = new List<long>();
        Dictionary<long, GameObject> loadedUIPopDic = new Dictionary<long, GameObject>();
        Dictionary<long, UI_Pop_Book_SubView> loadedUIPopSubViewDic = new Dictionary<long, UI_Pop_Book_SubView>();
        Dictionary<long, MapElementUI> loadedUIPopElementDic = new Dictionary<long, MapElementUI>();
        List<long> wantDestroyUIPopIdList = new List<long>();

        #endregion

        //IMediatorPlug needs
        public MapMarkerGlobalMediator():base(NameMediator, null ) {}

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.GuildMapMarkerInfoDelete,
                CmdConstant.GuildMapMarkerInfoAdd,
                CmdConstant.GuildMapMarkerInfoEdit
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.GuildMapMarkerInfoDelete:
                    if (notification.Body is List<long>)
                    {
                        List<long> deleteMarkerIdList = notification.Body as List<long>;
                        DestroyMarker(deleteMarkerIdList);
                    }
                    break;
                case CmdConstant.GuildMapMarkerInfoAdd:
                    if (notification.Body is List<long>)
                    {
                        List<long> addMarkerIdList = notification.Body as List<long>;
                        CreateMarker(addMarkerIdList);
                    }
                    break;
                case CmdConstant.GuildMapMarkerInfoEdit:
                    if (notification.Body is List<long>)
                    {
                        List<long> editMarkerIdList = notification.Body as List<long>;
                        EditMarker(editMarkerIdList);
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;

            uiIconMinDxf = WorldCamera.Instance().getCameraDxf("city_bound");
            uiIconMaxDxf = WorldCamera.Instance().getCameraDxf("limit_max");
            uiPopMinDxf = WorldCamera.Instance().getCameraDxf("city_bound");
            uiPopMaxDxf = WorldCamera.Instance().getCameraDxf("limit_max");
        }

        protected override void BindUIEvent()
        {
            WorldCamera.Instance().AddViewChange(OnWorldViewChange);
            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }

        protected override void BindUIData()
        {

        }

        public override void Update()
        {

        }

        public override void LateUpdate()
        {
            if (updateFlag)
            {
                curCamHeight = Common.GetLodDistance();

                Dictionary<long, MapMarkerInfo> guildMapMarkerInfoDic = mapMarkerProxy.GetGuildMapMarkerInfoDic();
                foreach (var mapMarkerInfo in guildMapMarkerInfoDic.Values)
                {
                    var x = PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.x);
                    var y = PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.y);

                    if (IsLodVisable(x, y))
                    {
                        CreateEffect(mapMarkerInfo.markerId);
                        CreateUIIcon(mapMarkerInfo.markerId);
                        CreateUIPop(mapMarkerInfo.markerId);

                        if (loadedUIIconElementDic.ContainsKey(mapMarkerInfo.markerId))
                        {
                            MapElementUI ui = loadedUIIconElementDic[mapMarkerInfo.markerId];
                            if (ui != null)
                            {
                                if (curCamHeight >= uiIconMinDxf && curCamHeight <= uiIconMaxDxf)
                                {
                                    ui.SetUIFadeShow((int)MapElementUI.FadeType.AllFadeIn);
                                }
                                else
                                {
                                    ui.SetUIFadeShow((int)MapElementUI.FadeType.AllFadeOut);
                                }
                            }
                        }

                        if (loadedUIPopElementDic.ContainsKey(mapMarkerInfo.markerId))
                        {
                            MapElementUI ui = loadedUIPopElementDic[mapMarkerInfo.markerId];
                            if (ui != null)
                            {
                                if (curCamHeight >= uiPopMinDxf && curCamHeight <= uiPopMaxDxf)
                                {
                                    ui.SetUIFadeShow((int)MapElementUI.FadeType.AllFadeIn);
                                }
                                else
                                {
                                    ui.SetUIFadeShow((int)MapElementUI.FadeType.AllFadeOut);
                                }
                            }
                        }
                    }
                    else
                    {
                        DestroyEffect(mapMarkerInfo.markerId);
                        DestroyUIIcon(mapMarkerInfo.markerId);
                        DestroyUIPop(mapMarkerInfo.markerId);
                    }
                }

                updateFlag = false;
            }
        }

        public override void FixedUpdate()
        {

        }

        public override void OnRemove()
        {
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
            CoreUtils.inputManager.RemoveTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);

            foreach (var loadedEffect in loadedEffectDic.Values)
            {
                if (loadedEffect != null)
                {
                    CoreUtils.assetService.Destroy(loadedEffect);
                }
            }

            loadingEffectIdList.Clear();
            loadedEffectDic.Clear();
            wantDestroyEffectIdList.Clear();

            foreach (var loadedModel in loadedUIIconDic.Values)
            {
                if (loadedModel != null)
                {
                    CoreUtils.assetService.Destroy(loadedModel);
                }
            }

            loadingUIIconIdList.Clear();
            loadedUIIconDic.Clear();
            loadedUIIconElementDic.Clear();
            wantDestroyUIIconIdList.Clear();

            foreach (var loadedUI in loadedUIPopDic.Values)
            {
                if (loadedUI != null)
                {
                    CoreUtils.assetService.Destroy(loadedUI);
                }
            }

            loadingUIPopIdList.Clear();
            loadedUIPopDic.Clear();
            loadedUIPopSubViewDic.Clear();
            loadedUIPopElementDic.Clear();
            wantDestroyUIPopIdList.Clear();

            mapMarkerProxy = null;
            fogSystemMediator = null;
        }

        #endregion

        private Transform GetLandRoot()
        {
            if (this.m_land_root == null)
            {
                this.m_land_root = GameObject.Find("SceneObject/land_root").transform;
            }

            return this.m_land_root;
        }

        private Transform GetHudRoot()
        {
            if (this.m_hud_root == null)
            {
                this.m_hud_root = GameObject.Find("UIRoot/Container/HUDLayer/pl_world").transform;
            }

            return this.m_hud_root;
        }

        private void CreateMarker(List<long> markerIdList)
        {
            foreach (var markerId in markerIdList)
            {
                CreateEffect(markerId);
                CreateUIIcon(markerId);
                CreateUIPop(markerId);
            }            
        }

        private void DestroyMarker(List<long> markerIdList)
        {
            foreach (var markerId in markerIdList)
            {
                DestroyEffect(markerId);
                DestroyUIIcon(markerId);
                DestroyUIPop(markerId);
            }            
        }

        private void EditMarker(List<long> markerIdList)
        {
            foreach (var markerId in markerIdList)
            {
                EditEffect(markerId);
                EditUIIcon(markerId);
                EditUIPop(markerId);
            }
        }

        #region Effect

        private void CreateEffect(long markerId)
        {
            MapMarkerInfo markerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
            if (markerInfo == null)
            {
                return;
            }

            if (!IsLodVisable(PosHelper.ServerUnitToClientUnit(markerInfo.pos.x), PosHelper.ServerUnitToClientUnit(markerInfo.pos.y)))
            {
                return;
            }

            if (wantDestroyEffectIdList.Contains(markerId))
            {
                wantDestroyEffectIdList.Remove(markerId);
            }

            if (loadedEffectDic.ContainsKey(markerId) || loadingEffectIdList.Contains(markerId))
            {
                return;
            }

            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)markerId);
            if (mapMarkerTypeDefine != null)
            {
                loadingEffectIdList.Add(markerId);

                CoreUtils.assetService.Instantiate(mapMarkerTypeDefine.effectShow, (obj) =>
                {
                    if (obj == null)
                    {
                        Debug.LogErrorFormat("MapMarker Effect Error. MarkerId:{0} EffectId:{1}", markerId, mapMarkerTypeDefine.effectShow);
                        return;
                    }
                    obj = CoreUtils.assetService.Instantiate(obj);

                    loadingEffectIdList.Remove(markerId);

                    if (wantDestroyEffectIdList.Contains(markerId))
                    {
                        CoreUtils.assetService.Destroy(obj);

                        wantDestroyEffectIdList.Remove(markerId);
                    }
                    else
                    {
                        MapMarkerInfo mapMarkerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
                        if (mapMarkerInfo == null)
                        {
                            CoreUtils.assetService.Destroy(obj);
                        }
                        else
                        {
                            obj.name = string.Format("MapMarker_Effect_{0}", mapMarkerInfo.markerId);

                            obj.transform.SetParent(GetLandRoot());
                            obj.transform.localScale = Vector3.one;
                            obj.transform.position = new Vector3(PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.x), 0, PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.y));
                            
                            ChangeSpriteColor[] componentArray = obj.GetComponentsInChildren<ChangeSpriteColor>();
                            foreach (var component in componentArray)
                            {
                                Color color = Color.white;
                                ColorUtility.TryParseHtmlString(mapMarkerTypeDefine.effectColor, out color);
                                ChangeSpriteColor.SetColor(component, color);
                            }

                            var lod = obj.GetComponent<LevelDetailScale>();
                            if (lod)
                            {
                                lod.UpdateLod();
                            }

                            loadedEffectDic.Add(markerId, obj);
                        }                        
                    }
                });
            }
        }

        private void DestroyEffect(long markerId)
        {
            if (loadedEffectDic.ContainsKey(markerId))
            {
                GameObject obj = loadedEffectDic[markerId];
                if (obj != null)
                {
                    CoreUtils.assetService.Destroy(obj);
                }

                loadedEffectDic.Remove(markerId);
            }
            else
            {
                if (!wantDestroyEffectIdList.Contains(markerId))
                {
                    wantDestroyEffectIdList.Add(markerId);
                }                
            }
        }

        private void EditEffect(long markerId)
        {
            MapMarkerInfo mapMarkerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
            if (mapMarkerInfo == null)
            {
                return;
            }

            if (loadedEffectDic.ContainsKey(markerId))
            {
                GameObject obj = loadedEffectDic[markerId];
                if (obj != null)
                {
                    obj.transform.position = new Vector3(PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.x), 0, PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.y));
                }
            }

            if (!loadedEffectDic.ContainsKey(markerId))
            {
                CreateEffect(markerId);
            }
        }

        #endregion

        #region UI Icon

        private void CreateUIIcon(long markerId)
        {
            MapMarkerInfo markerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
            if (markerInfo == null)
            {
                return;
            }

            if (!IsLodVisable(PosHelper.ServerUnitToClientUnit(markerInfo.pos.x), PosHelper.ServerUnitToClientUnit(markerInfo.pos.y)))
            {
                return;
            }

            if (wantDestroyUIIconIdList.Contains(markerId))
            {
                wantDestroyUIIconIdList.Remove(markerId);
            }

            if (loadedUIIconDic.ContainsKey(markerId) || loadingUIIconIdList.Contains(markerId))
            {
                return;
            }

            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)markerId);
            if (mapMarkerTypeDefine != null)
            {
                loadingUIIconIdList.Add(markerId);

                CoreUtils.assetService.LoadAssetAsync<GameObject>("UI_Pop_BookIcon", (asset) =>
                {
                    GameObject obj = asset.asset() as GameObject;
                    if (obj == null)
                    {
                        Debug.LogErrorFormat("MapMarker UI Icon Error. MarkerId:{0}", markerId);
                        return;
                    }
                    obj = CoreUtils.assetService.Instantiate(obj);
                    asset.Attack(obj);
                    loadingUIIconIdList.Remove(markerId);

                    if (wantDestroyUIIconIdList.Contains(markerId))
                    {
                        CoreUtils.assetService.Destroy(obj);

                        wantDestroyUIIconIdList.Remove(markerId);
                    }
                    else
                    {
                        MapMarkerInfo mapMarkerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
                        if (mapMarkerInfo == null)
                        {
                            CoreUtils.assetService.Destroy(obj);
                        }
                        else
                        {
                            obj.name = string.Format("MapMarker_UI_Icon_{0}", mapMarkerInfo.markerId);

                            obj.transform.SetParent(GetHudRoot());

                            UI_Pop_BookIcon_SubView subView = new UI_Pop_BookIcon_SubView(obj.GetComponent<RectTransform>());
                            subView.SetIcon(mapMarkerTypeDefine.iconImg);

                            MapElementUI ui = obj.GetComponent<MapElementUI>();
                            if (ui != null)
                            {
                                ui.InitData();
                                ui.SetUIType((int)MapElementUI.ElementUIType.GuildMarker);
                                ui.SetPosition(PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.x), 0, PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.y));
                            }

                            loadedUIIconDic.Add(markerId, obj);
                            loadedUIIconElementDic.Add(markerId, ui);
                        }
                    }
                }, null);
            }
        }

        private void DestroyUIIcon(long markerId)
        {
            if (loadedUIIconDic.ContainsKey(markerId))
            {
                GameObject obj = loadedUIIconDic[markerId];
                if (obj != null)
                {
                    CoreUtils.assetService.Destroy(obj);
                }

                loadedUIIconDic.Remove(markerId);
                loadedUIIconElementDic.Remove(markerId);
            }
            else
            {
                if (!wantDestroyUIIconIdList.Contains(markerId))
                {
                    wantDestroyUIIconIdList.Add(markerId);
                }
            }
        }

        private void EditUIIcon(long markerId)
        {
            MapMarkerInfo mapMarkerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
            if (mapMarkerInfo == null)
            {
                return;
            }

            if (loadedUIIconElementDic.ContainsKey(markerId))
            {
                MapElementUI ui = loadedUIIconElementDic[markerId];
                if (ui != null)
                {
                    ui.SetPosition(PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.x), 0, PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.y));
                }
            }

            if (!loadedUIIconDic.ContainsKey(markerId))
            {
                CreateUIIcon(markerId);
            }
        }

        #endregion

        #region UI Pop

        private void CreateUIPop(long markerId)
        {
            MapMarkerInfo markerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
            if (markerInfo == null)
            {
                return;
            }

            if (!IsLodVisable(PosHelper.ServerUnitToClientUnit(markerInfo.pos.x), PosHelper.ServerUnitToClientUnit(markerInfo.pos.y)))
            {
                return;
            }

            if (wantDestroyUIPopIdList.Contains(markerId))
            {
                wantDestroyUIPopIdList.Remove(markerId);
            }

            if (loadedUIPopDic.ContainsKey(markerId) || loadingUIPopIdList.Contains(markerId))
            {
                return;
            }

            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)markerId);
            if (mapMarkerTypeDefine != null)
            {
                loadingUIPopIdList.Add(markerId);

                CoreUtils.assetService.LoadAssetAsync<GameObject>("UI_Pop_Book", (asset) =>
                {
                    GameObject obj = asset.asset() as GameObject;
                    if (obj == null)
                    {
                        Debug.LogErrorFormat("MapMarker UI Pop Error. MarkerId:{0}", markerId);
                        return;
                    }
                    obj = CoreUtils.assetService.Instantiate(obj);
                    asset.Attack(obj);

                    loadingUIPopIdList.Remove(markerId);

                    if (wantDestroyUIPopIdList.Contains(markerId))
                    {
                        CoreUtils.assetService.Destroy(obj);

                        wantDestroyUIPopIdList.Remove(markerId);
                    }
                    else
                    {
                        MapMarkerInfo mapMarkerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
                        if (mapMarkerInfo == null)
                        {
                            CoreUtils.assetService.Destroy(obj);
                        }
                        else
                        {
                            obj.name = string.Format("MapMarker_UI_Pop_{0}", mapMarkerInfo.markerId);

                            obj.transform.SetParent(GetHudRoot());

                            obj.SetActive(!mapMarkerInfo.description.Equals(string.Empty));

                            UI_Pop_Book_SubView subView = new UI_Pop_Book_SubView(obj.GetComponent<RectTransform>());
                            subView.SetDescription(mapMarkerInfo.description);

                            MapElementUI ui = obj.GetComponent<MapElementUI>();
                            if (ui != null)
                            {
                                ui.InitData();
                                ui.SetUIType((int)MapElementUI.ElementUIType.GuildMarker);
                                ui.SetPosition(PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.x), 0, PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.y));
                            }                            

                            loadedUIPopDic.Add(markerId, obj);
                            loadedUIPopSubViewDic.Add(markerId, subView);
                            loadedUIPopElementDic.Add(markerId, ui);
                        }
                    }
                }, null);
            }
        }

        private void DestroyUIPop(long markerId)
        {
            if (loadedUIPopDic.ContainsKey(markerId))
            {
                GameObject obj = loadedUIPopDic[markerId];
                if (obj != null)
                {
                    CoreUtils.assetService.Destroy(obj);
                }

                loadedUIPopDic.Remove(markerId);
                loadedUIPopSubViewDic.Remove(markerId);
                loadedUIPopElementDic.Remove(markerId);
            }
            else
            {
                if (!wantDestroyUIPopIdList.Contains(markerId))
                {
                    wantDestroyUIPopIdList.Add(markerId);
                }
            }
        }

        private void EditUIPop(long markerId)
        {
            MapMarkerInfo mapMarkerInfo = mapMarkerProxy.GetGuildMapMarkerInfo(markerId);
            if (mapMarkerInfo == null)
            {
                return;
            }
                
            if (loadedUIPopDic.ContainsKey(markerId))
            {
                GameObject obj = loadedUIPopDic[markerId];
                if (obj != null)
                {
                    obj.SetActive(!mapMarkerInfo.description.Equals(string.Empty));
                }
            }

            if (loadedUIPopSubViewDic.ContainsKey(markerId))
            {
                UI_Pop_Book_SubView subView = loadedUIPopSubViewDic[markerId];
                if (subView != null)
                {
                    subView.SetDescription(mapMarkerInfo.description);
                }
            }

            if (loadedUIPopElementDic.ContainsKey(markerId))
            {
                MapElementUI ui = loadedUIPopElementDic[markerId];
                if (ui != null)
                {
                    ui.SetPosition(PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.x), 0, PosHelper.ServerUnitToClientUnit(mapMarkerInfo.pos.y));
                }
            }

            if (!loadedUIPopDic.ContainsKey(markerId))
            {
                CreateUIPop(markerId);
            }
        }

        #endregion

        private void OnWorldViewChange(float x, float y, float dxf)
        {
            updateFlag = true;
        }

        private void OnTouchBegan(int x, int y)
        {

        }

        private void OnTouchMoved(int x, int y)
        {
            updateFlag = true;
        }

        private void OnTouchEnded(int x, int y)
        {
            
        }

        private bool IsLodVisable(float x, float y)
        {
            fogSystemMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;

            if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y))
            {
                if (!fogSystemMediator.HasFogAtWorldPos(x, y))
                {
                    return true;
                }
            }

            return false;
        }
    }
}