// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Pop_WorldObjectInfoTypeRunesMediator
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

namespace Game {

    public class UI_Pop_WorldObjectInfoTypeRunesMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_WorldObjectInfoTypeRunesMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Pop_WorldObjectInfoTypeRunesMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_WorldObjectInfoTypeRunesView view;

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
            if(m_showTimeTimer != null)
            {
                m_showTimeTimer.Cancel();
                m_showTimeTimer = null;
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            if (m_playerProxy == null || m_troopProxy == null || m_worldProxy == null) return;

            long objId = (long)view.data;
            var objInfo = m_worldProxy.GetWorldMapObjectByobjectId(objId);
            if (objInfo == null) return;
            UpdatePopPos(objInfo.gameobject);
            m_runeInfo = objInfo;
            m_runeItemTypeCfg = CoreUtils.dataService.QueryRecord<Data.MapItemTypeDefine>((int)m_runeInfo.runeId);
            if (m_runeItemTypeCfg == null) return;
            m_buffItemList.Add(view.m_pl_buff1);
            m_buffItemList.Add(view.m_pl_buff2);
            RefreshUI();

            //功能介绍引导
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.Rune);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_collect.AddClickEvent(OnClickGather);
            view.m_btn_descBack_GameButton.onClick.AddListener(OnClickDescriptionBackButton);
            view.m_btn_descinfo_GameButton.onClick.AddListener(OnClickDescriptionButton);
        }

        protected override void BindUIData()
        {
            view.m_UI_Common_PopFun.InitSubView(m_runeInfo);
        }

        #endregion

        void UpdatePopPos(GameObject gameObject)
        {
            UIHelper.SelfAdaptPopViewPos(gameObject, view.gameObject.GetComponent<RectTransform>(),
                view.m_pl_pos.GetComponent<RectTransform>(),
                view.m_pl_content_Animator.GetComponent<RectTransform>(),
                view.m_img_arrowSideL_PolygonImage.gameObject,
                view.m_img_arrowSideR_PolygonImage.gameObject,
                view.m_img_arrowSideTop_PolygonImage.gameObject,
                view.m_img_arrowSideButtom_PolygonImage.gameObject
            );
        }

        private void OnClickDescriptionButton()
        {
            view.m_pl_normalInfo_CanvasGroup.gameObject.SetActive(false);
            view.m_pl_description_CanvasGroup.gameObject.SetActive(true);
        }

        private void OnClickDescriptionBackButton()
        {
            view.m_pl_normalInfo_CanvasGroup.gameObject.SetActive(true);
            view.m_pl_description_CanvasGroup.gameObject.SetActive(false);
        }

        private void OnClickGather()
        {
            MapObjectInfoEntity runeInfo = m_worldProxy.GetWorldMapObjectByobjectId(m_runeInfo.objectId);
            if (runeInfo == null)
            {
                Tip.CreateTip(200001).Show();
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldTunes);
                return;
            }
            m_troopProxy.SituStation = view.m_ck_Situ_GameToggle.isOn;
            CoreUtils.uiManager.CloseUI(UI.s_pop_WorldTunes);
            FightHelper.Instance.Gather((int)m_runeInfo.objectId);
        }

        private void OnClickFace()
        {
            Tip.CreateTip(100045).Show();
        }
        private void OnClickShare()
        {
            Tip.CreateTip(100045).Show();
        }

        private void OnClickCollect()
        {
            Tip.CreateTip(100045).Show();
        }

        private void RefreshUI()
        {
            var gamePos = WorldMapObjectProxy.ServerPosToGamePos(m_runeInfo.objectPos);
            view.m_lbl_position_LanguageText.text = LanguageUtils.getTextFormat(200505, gamePos.x, gamePos.y);
            OnTimerUpdateShowTime(0);
            m_showTimeTimer = Timer.Register(1, null, OnTimerUpdateShowTime, true);
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(m_runeItemTypeCfg.l_nameId);
            for (int i = 0; i < m_buffItemList.Count; ++i)
            {
                if (m_runeItemTypeCfg.buffData.Count > i)
                {
                    m_buffItemList[i].gameObject.SetActive(true);
                    var cityBuffCfg = CoreUtils.dataService.QueryRecord<Data.CityBuffDefine>(m_runeItemTypeCfg.buffData[i]);
                    m_buffItemList[i].RefreshBuff(cityBuffCfg);
                }
                else
                {
                    m_buffItemList[i].gameObject.SetActive(false);
                }
            }         
            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage, m_runeItemTypeCfg.headIcon);
            //if(RS.TuneFrameImage.Length >= m_runeItemTypeCfg.level)
            //{
            //    ClientUtils.LoadSprite(view.m_img_frame_PolygonImage, RS.TuneFrameImage[m_runeItemTypeCfg.level - 1]);
            //}
            view.m_ck_Situ_GameToggle.isOn = m_playerProxy.CurrentRoleInfo.situStation;
            RefreshDescriptionUI();
        }

        private void OnTimerUpdateShowTime(float time)
        {
            long serverTime = ServerTimeModule.Instance.GetServerTime();
            int leftTime = Mathf.Max(0, (int)(m_runeItemTypeCfg.showTime + m_runeInfo.runeRefreshTime - serverTime));
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(leftTime);
            if(leftTime == 0)
            {
                m_showTimeTimer.Cancel();
                m_showTimeTimer = null;
            }
        }

        private void RefreshDescriptionUI()
        {
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_runeItemTypeCfg.l_desc);
        }

        private PlayerProxy m_playerProxy;
        private TroopProxy m_troopProxy;
        private WorldMapObjectProxy m_worldProxy;
        private MapObjectInfoEntity m_runeInfo = null;
        private Timer m_showTimeTimer = null;
        private Data.MapItemTypeDefine m_runeItemTypeCfg = null;
        private List<UI_Item_WorldObjectInfoBuffItem_SubView> m_buffItemList = new List<UI_Item_WorldObjectInfoBuffItem_SubView>();
    }
}