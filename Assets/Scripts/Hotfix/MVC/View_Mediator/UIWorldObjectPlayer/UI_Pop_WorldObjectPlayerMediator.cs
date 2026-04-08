// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月25日
// Update Time         :    2020年2月25日
// Class Description   :    UI_Pop_WorldObjectPlayerMediator
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
using Data;
using System;

namespace Game {
    public class UI_Pop_WorldObjectPlayerMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_WorldObjectPlayerMediator";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private AllianceProxy m_allianceProxy;
        private TroopProxy m_troopProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;

        private MapObjectInfoEntity data;
        #endregion

        //IMediatorPlug needs
        public UI_Pop_WorldObjectPlayerMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_WorldObjectPlayerView view;

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

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_RallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_1gbtn1.m_btn_languageButton_GameButton.onClick.AddListener(On1_1BtnClick);
            view.m_UI_Model_2gbtn1.m_btn_languageButton_GameButton.onClick.AddListener(On2_1BtnClick);
            view.m_UI_Model_2gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(On2_2BtnClick);
            view.m_UI_Model_3gbtn1.m_btn_languageButton_GameButton.onClick.AddListener(On3_1BtnClick);
            view.m_UI_Model_3gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(On3_2BtnClick);
            view.m_UI_Model_3gbtn3.m_btn_languageButton_GameButton.onClick.AddListener(On3_3BtnClick);
        }

        protected override void BindUIData()
        {
            if (view.data is RoleInfoEntity)
            {
                RoleInfoEntity roleInfoEntity = view.data as RoleInfoEntity;
                data = m_cityBuildingProxy.MyCityObjData.mapObjectExtEntity;
                RefreshView(roleInfoEntity);
            }
            else if (view.data is MapObjectInfoEntity)
            {
                 data = view.data as MapObjectInfoEntity;
                if (data != null)
                {
                    RefreshView(data);
                }
            }
            else
            {
                Debug.LogError("not find type ");

            }
            view.m_UI_Common_PopFun.InitSubView(data);
            ChangeWinPos();
            InitView();
        }

        #endregion


        private void InitView()
        {
            view.m_UI_Item_line1.m_lbl_title_LanguageText.text = LanguageUtils.getText(180314);
            view.m_UI_Item_line2.m_lbl_title_LanguageText.text = LanguageUtils.getText(145012);
            view.m_UI_Item_line3.m_lbl_title_LanguageText.text = LanguageUtils.getText(570002);

            view.m_UI_Model_1gbtn1.m_lbl_Text_LanguageText.text = LanguageUtils.getText(300105);//进入
            view.m_UI_Model_2gbtn1.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730123);//资源援助
            view.m_UI_Model_2gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730122);//增援
            view.m_UI_Model_3gbtn1.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500011);//侦察
            view.m_UI_Model_3gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500014);//集结
            view.m_UI_Model_3gbtn3.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500012);//攻击
        }
        private void RefreshView(RoleInfoEntity roleInfoEntity)
        {
            view.m_btn_descinfo_GameButton.gameObject.SetActive(false);
            view.m_pl_inSitu.gameObject.SetActive(false);
            view.m_lbl_recommend_LanguageText.gameObject.SetActive(false);

            view.m_pl_1g.gameObject.SetActive(true);
            view.m_pl_2g_GridLayoutGroup.gameObject.SetActive(false);
            view.m_pl_3g_GridLayoutGroup.gameObject.SetActive(false);


            view.m_UI_Model_PlayerHead.LoadPlayerIcon(roleInfoEntity.headId, roleInfoEntity.headFrameID);

            view.m_UI_Item_line1.m_lbl_content_LanguageText.text =  roleInfoEntity.combatPower.ToString("N0");
            view.m_UI_Item_line2.m_lbl_content_LanguageText.text = GetKillCount(roleInfoEntity.killCount).ToString("N0");
            if (roleInfoEntity.guildId != 0)
            {
                var info = m_allianceProxy.GetAlliance();
                view.m_UI_Item_line3.m_lbl_content_LanguageText.text = LanguageUtils.getTextFormat(300030, info.abbreviationName, info.name);
                view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, info.abbreviationName, roleInfoEntity.name);

            }
            else
            {
                view.m_UI_Item_line3.m_lbl_content_LanguageText.text = LanguageUtils.getTextFormat(570029);
                view.m_lbl_name_LanguageText.text = roleInfoEntity.name;
            }

            view.m_btn_head_GameButton.onClick.AddListener(() => {
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);
                CoreUtils.uiManager.ShowUI(UI.s_PlayerInfo);
            });
            view.m_lbl_position_LanguageText.text = PosHelper.FormatServerPos(roleInfoEntity.pos);
        }
        private long GetKillCount(Dictionary<long, KillCount> kills)
        {
            long killCount = 0;
            if (kills != null)
            {
                foreach (var Kill in kills.Values)
                {
                    killCount += Kill.count;
                }
            }
            return killCount;
        }

        private void RefreshView(MapObjectInfoEntity mapObjectInfoEntity) 
        {
            view.m_btn_descinfo_GameButton.gameObject.SetActive(false);
            view.m_pl_inSitu.gameObject.SetActive(false);
            view.m_lbl_recommend_LanguageText.gameObject.SetActive(false);
            if (m_playerProxy.CurrentRoleInfo.guildId != 0 && mapObjectInfoEntity.guildId == m_playerProxy.CurrentRoleInfo.guildId)
            {
                view.m_pl_1g .gameObject.SetActive(false);
                view.m_pl_2g_GridLayoutGroup .gameObject.SetActive(true);
                view.m_pl_3g_ArabLayoutCompment.gameObject.SetActive(false);
            }
            else
            {
                view.m_pl_1g.gameObject.SetActive(false);
                view.m_pl_2g_GridLayoutGroup.gameObject.SetActive(false);
                view.m_pl_3g_ArabLayoutCompment .gameObject.SetActive(true);
            }

                view.m_UI_Model_PlayerHead.LoadPlayerIcon(mapObjectInfoEntity.headId, mapObjectInfoEntity.headFrameID);

                view.m_UI_Item_line1.m_lbl_content_LanguageText.text = mapObjectInfoEntity.objectPower.ToString("N0");
                view.m_UI_Item_line2.m_lbl_content_LanguageText.text = GetKillCount(mapObjectInfoEntity.killCount).ToString("N0");
            if (mapObjectInfoEntity.guildId != 0)
            {
                view.m_UI_Item_line3.m_lbl_content_LanguageText.text = LanguageUtils.getTextFormat(300030, mapObjectInfoEntity.guildAbbName, mapObjectInfoEntity.guildFullName);
                view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, mapObjectInfoEntity.guildAbbName, mapObjectInfoEntity.cityName);

            }
            else
            {
                view.m_UI_Item_line3.m_lbl_content_LanguageText.text = LanguageUtils.getText(570029);
                view.m_lbl_name_LanguageText.text = mapObjectInfoEntity.cityName;
            }

            view.m_btn_head_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_OtherPlayerInfo, null, mapObjectInfoEntity);
                    CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);
                });
                view.m_lbl_position_LanguageText.text = PosHelper.FormatServerPos(mapObjectInfoEntity.objectPos);

            //功能介绍引导
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.OtherPlayerCity);
        }

        #region 点击事件
        private void On1_1BtnClick()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.EnterCity);
            CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);
        }
        /// <summary>
        /// 资源援助
        /// </summary>
        private void On2_1BtnClick()
        {
            if (!m_troopProxy.GetIsCityCreateTroop())
            {
                Tip.CreateTip(184030, Tip.TipStyle.Middle).Show();
                return;
            }
            BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.TradingPost);

            if (buildingInfoEntity == null)
            {
                Tip.CreateTip(184031, Tip.TipStyle.Middle).Show();
                return;
            }
           
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);
                Transport_GetTransport.request req = new Transport_GetTransport.request();
                req.targetRid = data.cityRid;
                AppFacade.GetInstance().SendSproto(req);

        }
        /// <summary>
        /// 增援
        /// </summary>
        private void On2_2BtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);
            AppFacade.GetInstance().SendNotification(CmdConstant.GetCityReinforceInfo, data, null);
        }
        /// <summary>
        /// 侦察
        /// </summary>
        private void On3_1BtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);          
            FightHelper.Instance.Scout(data.objectPos.x ,data.objectPos.y,  (int)data.objectId);
        }
        /// <summary>
        /// 集结
        /// </summary>
        private void On3_2BtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);

//            if (!m_RallyTroopsProxy.isRally(m_mapObjectInfoEntity.cityRid,(int)m_mapObjectInfoEntity.objectId))
//            {
//                return;
//            }
//
//            OpenPanelData openPanelData= new OpenPanelData((int)m_mapObjectInfoEntity.objectId,OpenPanelType.Play);
//            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
            FightHelper.Instance.Concentrate((int)data.objectId);
        }
        /// <summary>
        /// 攻击
        /// </summary>
        private void On3_3BtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);
//            if (m_troopProxy.CheckAttackOtherCity((int)m_mapObjectInfoEntity.objectId))
//            {                       
//                return;
//            }
//
//            if (!m_RallyTroopsProxy.IsWasFever((int) m_mapObjectInfoEntity.objectId, OpenPanelType.Common))
//            {
//               return; 
//            }
//
//            OpenPanelData openPanelData= new OpenPanelData((int)m_mapObjectInfoEntity.objectId,OpenPanelType.Common);
//            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
            FightHelper.Instance.Attack((int)data.objectId);
        }


        #endregion
        //改变窗口位置
        private void ChangeWinPos()
        {
            if (data == null)
            {
                return;
            }
            if (data.gameobject == null)
            {
                return;
            }

            //屏幕坐标转界面局部坐标
            Vector2 localPos;
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), data.gameobject.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.m_pl_pos.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform viewRect = view.gameObject.GetComponent<RectTransform>();
            var rect = view.m_pl_content_Animator.GetComponent<RectTransform>().rect;

            float diffNum = 50f;

            // 左
            if (localPos.x < viewRect.rect.width / 2)
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    localPos.y = localPos.y - (rect.height / 2) - diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideTop_PolygonImage.transform.localPosition = new Vector2(offset,
                                                                           view.m_img_arrowSideTop_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                                                   view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x + (rect.width / 2) + diffNum;
                    view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                }
            }
            // 右
            else
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.y - (viewRect.rect.height - rect.height / 2);
                        view.m_img_arrowSideR_PolygonImage.transform.localPosition = new Vector2(view.m_img_arrowSideR_PolygonImage.transform.localPosition.x,
                                                                                                 offset);
                        view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);

                        localPos.x = localPos.x - (rect.width / 2) - diffNum;
                        localPos.y = viewRect.rect.height - rect.height / 2;
                    }
                    else
                    {
                        localPos.y = localPos.y - (rect.height / 2) - diffNum;
                        view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                    }
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.x - (viewRect.rect.width - rect.width / 2);
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                                                                                                       view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = (viewRect.rect.width - rect.width / 2);
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x - (rect.width / 2) - diffNum;
                    view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                }
            }
            view.m_pl_content_Animator.transform.localPosition = localPos;
        }
    }
}