// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_IF_EquipTalentChooseMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public enum EquipTalent
    {
        Integration,
        Archer,
        Cavalry,
        Infantry,
        LeaderShip,
    }
    public class UI_IF_EquipTalentChooseMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_EquipTalentChooseMediator";

        private BagProxy m_bagProxy;

        private List<UI_Model_EquipAtt_SubView> m_equipAttLst = new List<UI_Model_EquipAtt_SubView>();
        private List<UI_Model_CaptainHead_SubView> m_captainHeadLst = new List<UI_Model_CaptainHead_SubView>();
        
        private ForgeEquipItemInfo m_curForgeItem;
        private int m_selectTalent;
        #endregion

        //IMediatorPlug needs
        public UI_IF_EquipTalentChooseMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_EquipTalentChooseView view;

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
            m_curForgeItem = view.data as ForgeEquipItemInfo;
            if (m_curForgeItem == null)
            {
                CoreUtils.logService.Warn("铁匠铺====天赋选择错误,当前锻造装备为空！！！");
                OnClose();
                return;
            }
            
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            
            m_equipAttLst.Add(view.m_UI_Model_EquipAtt);
            m_captainHeadLst.Add(view.m_UI_Model_CaptainHead);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_sure.AddClickEvent(OnConfirm);
            view.m_btn_back.AddClickEvent(OnCancel);

        }

        protected override void BindUIData()
        {
            InitTalentBtn();
            RefreshItemInfo();
        }
       
        #endregion

        private void InitTalentBtn()
        {
            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            var talents = config.equipTalent;
            List<UI_Item_EquipTalent_SubView> talentViews = new List<UI_Item_EquipTalent_SubView>()
            {
                view.m_UI_Item_EquipTalent1,
                view.m_UI_Item_EquipTalent2,
                view.m_UI_Item_EquipTalent3,
                view.m_UI_Item_EquipTalent4,
                view.m_UI_Item_EquipTalent5,
            };

            foreach (var talentView in talentViews)
            {
                talentView.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < talents.Count; i++)
            {
                if (talentViews.Count > i)
                {
                    talentViews[i].gameObject.SetActive(true);
                    talentViews[i].SetInfo(talents[i],true,ChooseTalent);
                }
                else
                {
                    break;
                }
            }
        }

        private void RefreshItemInfo()
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(m_curForgeItem.EquipID);
            var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(m_curForgeItem.EquipID);
            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            var talentPlus = config.equipTalentPromote;

            ClientUtils.LoadSprite(view.m_img_equip_PolygonImage,itemCfg.itemIcon);
            
            DisableEquipAttItem();
            for (int i = 0; i < equipCfg.att.Count && i <equipCfg.attAdd.Count; i++)
            {
                if (m_equipAttLst.Count <= i)
                {
                    var equipAttItemObj = GameObject.Instantiate(view.m_UI_Model_EquipAtt.gameObject,view.m_pl_equipatt_GridLayoutGroup.gameObject.transform);
                    var equipAttItem = new UI_Model_EquipAtt_SubView(equipAttItemObj.GetComponent<RectTransform>());
                    m_equipAttLst.Add(equipAttItem);
                }
                m_equipAttLst[i].gameObject.SetActive(true);
                m_equipAttLst[i].SetAttInfo(equipCfg.att[i], equipCfg.attAdd[i],talentPlus);
            }
        }

        private void ChooseTalent(int talentID)
        {
            view.m_pl_talent.gameObject.SetActive(false);
            view.m_pl_talentPreview.gameObject.SetActive(true);
            view.m_pl_preview.SetInfo(talentID,false);
            
            var talentCfg = CoreUtils.dataService.QueryRecord<HeroTalentTypeDefine>(talentID);
            var captainCfgs = CoreUtils.dataService.QueryRecords<HeroTalentDefine>().FindAll(x => x.type == talentID);

            m_selectTalent = talentID;

            DisableCaptainHeadItem();
            int j = 0;
            for (int i = 0; i < captainCfgs.Count; i++)
            {
                var heroCfg = CoreUtils.dataService.QueryRecord<HeroDefine>(captainCfgs[i].ID / 100);
                if (heroCfg == null || heroCfg.listDisplay==1)
                {
                    continue;
                }

                if (m_captainHeadLst.Count <= j)
                {
                    var captainHeadItemObj = GameObject.Instantiate(view.m_UI_Model_CaptainHead.gameObject,view.m_pl_heroHead_ArabLayoutCompment.gameObject.transform);
                    var captainHeadItem = new UI_Model_CaptainHead_SubView(captainHeadItemObj.GetComponent<RectTransform>());
                    m_captainHeadLst.Add(captainHeadItem);
                }
                m_captainHeadLst[j].gameObject.SetActive(true);
                m_captainHeadLst[j].SetHero(captainCfgs[i].ID/100,0);
                j++;
            }

            
            view.m_lbl_talentTip_LanguageText.text =
                LanguageUtils.getTextFormat(182080, LanguageUtils.getText(talentCfg.l_talentID));
            
            
        }
        
        private void DisableEquipAttItem()
        {
            foreach (var attSubView in m_equipAttLst)
            {
                attSubView.gameObject.SetActive(false);
            }
        }

        private void DisableCaptainHeadItem()
        {
            foreach (var captainHead in m_captainHeadLst)
            {
                captainHead.gameObject.SetActive(false);
            }
        }

        private void OnConfirm()
        {
            m_bagProxy.MakeEquipment(m_curForgeItem.EquipID,m_selectTalent);
            CoreUtils.uiManager.CloseUI(UI.s_EquipTalentChoose);
        }

        private void OnCancel()
        {
            view.m_pl_talentPreview.gameObject.SetActive(false);
            view.m_pl_talent.gameObject.SetActive(true);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_EquipTalentChoose);
        }
    }
}