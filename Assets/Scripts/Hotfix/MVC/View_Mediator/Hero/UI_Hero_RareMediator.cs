// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月27日
// Update Time         :    2019年12月27日
// Class Description   :    UI_Hero_RareMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;

namespace Game {
    public class UI_Hero_RareMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Hero_RareMediator";

        private long m_heroId = 0;


        #endregion

        //IMediatorPlug needs
        public UI_Hero_RareMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Hero_RareView view;

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
            UpdateHero();
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

        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void UpdateHero()
        {
            if(m_heroId == (long)view.data)
                return;
            m_heroId = (long)view.data;

            var heroInfo = CoreUtils.dataService.QueryRecord<Data.HeroDefine>((int)m_heroId);
            Debug.Log(heroInfo.heroModel);
            ClientUtils.LoadSpine(view.m_spine_hero_SkeletonGraphic, heroInfo.heroModel);
            ClientUtils.LoadSprite(view.m_img_head_PolygonImage, heroInfo.heroIcon);

            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(heroInfo.l_desID);
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(heroInfo.l_nameID);
            view.m_lbl_title_LanguageText.text = LanguageUtils.getText(heroInfo.l_appellationID);


            var config = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>((int)0);
            view.m_lbl_rare_LanguageText.text = LanguageUtils.getText(heroInfo.rare+ config.rareLanguage-1);

            var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)heroInfo.civilization);
            Debug.Log(civilizationInfo.civilizationMark);
            ClientUtils.LoadSprite(view.m_img_civilization_PolygonImage, civilizationInfo.civilizationMark);
            Color c;
            ColorUtility.TryParseHtmlString(civilizationInfo.markColour, out c);
            view.m_img_civilization_PolygonImage.color = c;

            var talent1 = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>((int)heroInfo.talent[0]);
            var talent2 = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>((int)heroInfo.talent[1]);
            var talent3 = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>((int)heroInfo.talent[2]);
            ClientUtils.LoadSprite(view.m_img_talent_1_PolygonImage, talent1.icon1);
            ClientUtils.LoadSprite(view.m_img_talent_2_PolygonImage, talent2.icon1);
            ClientUtils.LoadSprite(view.m_img_talent_3_PolygonImage, talent3.icon1);

            view.m_lbl_talent_1_LinkImageText.text = LanguageUtils.getText(talent1.l_talentID);
            view.m_lbl_talent_2_LinkImageText.text = LanguageUtils.getText(talent2.l_talentID);
            view.m_lbl_talent_3_LinkImageText.text = LanguageUtils.getText(talent3.l_talentID);
            ClientUtils.ShowChild<PolygonImage>(view.m_pl_star_HorizontalLayoutGroup.transform, heroInfo.initStar);
        }
    }
}