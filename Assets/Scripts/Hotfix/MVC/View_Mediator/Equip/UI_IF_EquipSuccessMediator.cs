// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_IF_EquipSuccessMediator
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
    public class UI_IF_EquipSuccessMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_EquipSuccessMediator";
        
        private EquipItemInfo m_item;
        #endregion

        //IMediatorPlug needs
        public UI_IF_EquipSuccessMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_EquipSuccessView view;

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
            if (m_item != null)
            {
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                mt.FlyItemEffect(m_item.ItemID,0,view.m_img_euqip_PolygonImage.rectTransform);
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_item = view.data as EquipItemInfo;

            if (m_item == null)
            {
                OnClose();
            }
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            RefreshItemInfo();
        }
       
        #endregion

        private void RefreshItemInfo()
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(m_item.ItemID);

            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);
            ClientUtils.LoadSprite(view.m_img_euqip_PolygonImage,itemCfg.itemIcon);
            view.m_img_euqip_Animation.Play("UA_Equip_Scale");
            Timer.Register(2, () =>
            {
             //   view.m_img_euqip_Animation.Play("UA_Equip_Scale");
            }, null);
            if (m_item.Exclusive != 0)
            {
                var talentCfg = CoreUtils.dataService.QueryRecord<HeroTalentTypeDefine>(m_item.Exclusive);
                view.m_lbl_talentName_LanguageText.gameObject.SetActive(true);

                view.m_lbl_talentName_LanguageText.text =LanguageUtils.getTextFormat(182079,LanguageUtils.getText(talentCfg.l_talentID));
                ClientUtils.LoadSprite(view.m_img_talent_PolygonImage,talentCfg.equipIcon);
            }
            else
            {
                view.m_lbl_talentName_LanguageText.gameObject.SetActive(false);
            }
        }
        
        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_EquipSuccess);
        }
    }
}