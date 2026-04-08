// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月20日
// Update Time         :    2020年5月20日
// Class Description   :    UI_Win_OpenFogShowMediator 王国地图 道具开雾
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using System;
using Data;

namespace Game {
    public class UI_Win_OpenFogShowMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_OpenFogShowMediator";

        private GameObject m_fogShowObj;

        private Int64 m_itemIndex;

        private bool m_isDispose;

        #endregion

        //IMediatorPlug needs
        public UI_Win_OpenFogShowMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_OpenFogShowView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void OnRemove()
        { 
            m_isDispose = true;
            //if (m_fogShowObj != null)
            //{
            //    CoreUtils.assetService.Destroy(m_fogShowObj);
            //    m_fogShowObj = null;
            //}
            CoreUtils.uiManager.SetGuideStatus(false);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            CoreUtils.uiManager.SetGuideStatus(true); //禁用返回键关闭界面
            m_itemIndex = (Int64)view.data;

            float delayTime = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).kindomMapEffectDelay;
            int groupSize =CoreUtils.dataService.QueryRecord<UnitViewDefine>(8).viewRange*2/PosHelper.TIlE_BASE_SIZE;
            Debug.LogFormat("groupSize:{0}", groupSize);

            //有迷雾 开启迷雾表现
            FogSystemMediator fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            var cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            var tilePos = fogMedia.Pos2Tile(cityBuildingProxy.RolePos.x, cityBuildingProxy.RolePos.y);
            Vector2 tile = WarFogMgr.FindGroupForUseItem(groupSize, 0.5f, tilePos.x, tilePos.y);
            //Debug.LogErrorFormat("坐标：{0}", tile);
            if (tile.x < 0)
            {
                tile = WarFogMgr.FindFogClosestAt(tilePos.x, tilePos.y);
                if (tile.x < 0)
                {
                    return;
                }
            }

            Vector2 worldPos = fogMedia.Tile2Pos((int)tile.x, (int)tile.y);
            Vector3 reqPos = new Vector3(worldPos.x, 0, worldPos.y); //Vector3 effectPos = fogMedia.GetGroupFogCenterPos(worldPos.x, worldPos.y, groupSize);
            Vector3 effectPos = fogMedia.GetGroupFogCenterPos(worldPos.x, worldPos.y, groupSize);

            //Debug.LogErrorFormat("发送前:{0} {1}", tile.x, tile.y);
            //Debug.LogErrorFormat("world pos:{0}", worldPos);
            //Debug.LogErrorFormat("centerPos pos:{0}", centerPos);
            //Debug.LogErrorFormat("effectPos pos:{0}", effectPos);

            float dxf = WorldCamera.Instance().getCameraDxf("TacticsToStrategy2");
            WorldCamera.Instance().SetCameraDxf(dxf, 0f, () => { });
            WorldCamera.Instance().ViewTerrainPos(effectPos.x, effectPos.z, 500, () =>
            {
                if (m_isDispose)
                {
                    return;
                }
                //创建特效
                CoreUtils.assetService.Instantiate("operation_2010", (fotObj) =>
                {
                    if (m_isDispose)
                    {
                        return;
                    }
                    m_fogShowObj = fotObj;
                    fotObj.transform.localScale = Vector3.one;
                    fotObj.transform.position = effectPos;
                    Timer.Register(1f, () =>
                    {
                        if (m_isDispose)
                        {
                            return;
                        }

                        PosInfo posInfo = new PosInfo();
                        posInfo.x = (int)reqPos.x * 100;
                        posInfo.y = (int)reqPos.z * 100;
                        var sp = new Item_ItemUse.request();
                        sp.itemIndex = m_itemIndex;
                        sp.itemNum = 1;
                        sp.pos = posInfo;
                        AppFacade.GetInstance().SendSproto(sp);

                        //Debug.LogFormat("发送迷雾开启坐标:{0} {1}", posInfo.x, posInfo.y);

                        //发送迷雾开启坐标
                        Timer.Register(delayTime, () =>
                        {
                            if (m_isDispose)
                            {
                                return;
                            }
                            Close();
                            if (!WarFogMgr.IsAllFogOpen())
                            {
                                var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                                int itemId = 502100001;
                                if (bagProxy.GetItemNum(itemId) > 0)
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_quickUseItem, null, itemId);
                                }
                            }
                        });
                    });
                });
            });
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_openFogShow);
        }
    }
}