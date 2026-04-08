// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月10日
// Update Time         :    2020年1月10日
// Class Description   :    ArmyBuffMediator
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

namespace Game {

    public class ArmyBuffMediator : GameMediator {
        #region Member
        public static string NameMediator = "ArmyBuffMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        //private BattleReport2.BattleData m_battleData;

        #endregion

        //IMediatorPlug needs
        public ArmyBuffMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ArmyBuffView view;

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
            //SetBuffData();
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        //private void SetBuffData()
        //{
        //    UI_Item_ArmyBuff_SubView itemView = view.m_UI_Item_ArmyBuff;
        //    itemView.m_lbl_nameL_LanguageText.text = m_battleData.personalData[0].name;
        //    ClientUtils.LoadSprite(itemView.m_UI_Model_PlayerHeadL.m_img_circle_PolygonImage, m_battleData.personalData[0].icon);
        //    if(m_battleData.personalData[0].hero1!=null)
        //    {
        //        HeroDefine hero1 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_battleData.personalData[0].hero1.heroId);
        //        if (hero1 != null)
        //        {
        //            itemView.m_pl_captainL.gameObject.SetActive(true);
        //            itemView.m_UI_Item_ArmyBuffCaptainL1.m_lbl_name_LanguageText.text = LanguageUtils.getText(hero1.l_nameID);
        //            ClientUtils.LoadSprite(itemView.m_UI_Item_ArmyBuffCaptainL1.m_UI_Model_CaptainHead.m_img_char_PolygonImage, hero1.heroIcon);
        //            itemView.m_UI_Item_ArmyBuffCaptainL1.m_UI_Model_HeadStar1.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero1.star >= 1);
        //            itemView.m_UI_Item_ArmyBuffCaptainL1.m_UI_Model_HeadStar2.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero1.star >= 2);
        //            itemView.m_UI_Item_ArmyBuffCaptainL1.m_UI_Model_HeadStar3.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero1.star >= 3);
        //            itemView.m_UI_Item_ArmyBuffCaptainL1.m_UI_Model_HeadStar4.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero1.star >= 4);
        //            itemView.m_UI_Item_ArmyBuffCaptainL1.m_UI_Model_HeadStar5.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero1.star >= 5);
        //            itemView.m_UI_Item_ArmyBuffCaptainL1.m_UI_Model_HeadStar6.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero1.star >= 6);
        //        }
        //        else
        //        {
        //            itemView.m_pl_captainL.gameObject.SetActive(false);
        //        }
        //    }
        //    else
        //    {
        //        itemView.m_pl_captainL.gameObject.SetActive(false);
        //    }

        //    if (m_battleData.personalData[0].hero2 != null)
        //    {
        //        HeroDefine hero2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_battleData.personalData[0].hero2.heroId);
        //        if (hero2 != null)
        //        {
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.gameObject.SetActive(true);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_lbl_name_LanguageText.text = LanguageUtils.getText(hero2.l_nameID);
        //            ClientUtils.LoadSprite(itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_CaptainHead.m_img_char_PolygonImage, hero2.heroIcon);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar1.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero2.star >= 1);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar2.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero2.star >= 2);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar3.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero2.star >= 3);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar4.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero2.star >= 4);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar5.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero2.star >= 5);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar6.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[0].hero2.star >= 6);
        //        }
        //        else
        //        {
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.gameObject.SetActive(false);
        //        }
        //    }
        //    else
        //    {
        //        itemView.m_UI_Item_ArmyBuffCaptainL2.gameObject.SetActive(false);
        //    }


        //    if (m_battleData.personalData[1].hero1 != null)
        //    {
        //        HeroDefine hero1 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_battleData.personalData[1].hero1.heroId);
        //        if (hero1 != null)
        //        {
        //            itemView.m_pl_captainR.gameObject.SetActive(true);
        //            itemView.m_UI_Item_ArmyBuffCaptainR1.m_lbl_name_LanguageText.text = LanguageUtils.getText(hero1.l_nameID);
        //            ClientUtils.LoadSprite(itemView.m_UI_Item_ArmyBuffCaptainR1.m_UI_Model_CaptainHead.m_img_char_PolygonImage, hero1.heroIcon);
        //            itemView.m_UI_Item_ArmyBuffCaptainR1.m_UI_Model_HeadStar1.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero1.star >= 1);
        //            itemView.m_UI_Item_ArmyBuffCaptainR1.m_UI_Model_HeadStar2.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero1.star >= 2);
        //            itemView.m_UI_Item_ArmyBuffCaptainR1.m_UI_Model_HeadStar3.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero1.star >= 3);
        //            itemView.m_UI_Item_ArmyBuffCaptainR1.m_UI_Model_HeadStar4.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero1.star >= 4);
        //            itemView.m_UI_Item_ArmyBuffCaptainR1.m_UI_Model_HeadStar5.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero1.star >= 5);
        //            itemView.m_UI_Item_ArmyBuffCaptainR1.m_UI_Model_HeadStar6.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero1.star >= 6);
        //        }
        //        else
        //        {
        //            itemView.m_pl_captainR.gameObject.SetActive(false);
        //        }
        //    }
        //    else
        //    {
        //        itemView.m_pl_captainR.gameObject.SetActive(false);
        //    }

        //    if (m_battleData.personalData[1].hero2 != null)
        //    {
        //        HeroDefine hero2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_battleData.personalData[0].hero2.heroId);
        //        if (hero2 != null)
        //        {
        //            itemView.m_pl_captainR.gameObject.SetActive(true);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_lbl_name_LanguageText.text = LanguageUtils.getText(hero2.l_nameID);
        //            ClientUtils.LoadSprite(itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_CaptainHead.m_img_char_PolygonImage, hero2.heroIcon);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar1.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero2.star >= 1);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar2.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero2.star >= 2);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar3.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero2.star >= 3);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar4.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero2.star >= 4);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar5.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero2.star >= 5);
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.m_UI_Model_HeadStar6.m_img_star_PolygonImage.gameObject.SetActive(m_battleData.personalData[1].hero2.star >= 6);
        //        }
        //        else
        //        {
        //            itemView.m_UI_Item_ArmyBuffCaptainL2.gameObject.SetActive(false);
        //        }
        //    }
        //    else
        //    {
        //        itemView.m_UI_Item_ArmyBuffCaptainL2.gameObject.SetActive(false);
        //    }



        //}

    }
}