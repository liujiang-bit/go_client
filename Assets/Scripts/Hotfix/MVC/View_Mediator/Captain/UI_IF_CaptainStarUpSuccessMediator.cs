// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_IF_CaptainStarUpSuccessMediator
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

namespace Game {

    public class CaptainStarUpSuccessViewData
    {
        public HeroProxy.Hero Hero { get; set; }
        public int OldHeroStar { get; set; }
    }

    public class UI_IF_CaptainStarUpSuccessMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_CaptainStarUpSuccessMediator";


        #endregion

        //IMediatorPlug needs
        public UI_IF_CaptainStarUpSuccessMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_CaptainStarUpSuccessView view;

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
            CaptainStarUpSuccessViewData data = view.data as CaptainStarUpSuccessViewData;
            if (data == null || data.Hero == null) return;
            m_data = data;
            m_captainStarList.Add(view.m_UI_Model_CaptainStar1);
            m_captainStarList.Add(view.m_UI_Model_CaptainStar2);
            m_captainStarList.Add(view.m_UI_Model_CaptainStar3);
            m_captainStarList.Add(view.m_UI_Model_CaptainStar4);
            m_captainStarList.Add(view.m_UI_Model_CaptainStar5);
            m_captainStarList.Add(view.m_UI_Model_CaptainStar6);
            RefreshUI();
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void addStarEffect(GameObject gameObject)
        {
            Transform eff = gameObject.transform.Find(RS.HeroStarLevelUpStarEffectName + "(Clone)");
            if (eff != null)
            {
                eff.transform.gameObject.SetActive(false);
                eff.transform.gameObject.SetActive(true);
                return;
            }

            CoreUtils.assetService.Instantiate(RS.HeroStarLevelUpStarEffectName, (go) =>
            {
                go.transform.SetParent(gameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            });
        }

        private void RefreshUI()
        {
            view.m_UI_CaptainHead.SetHero(m_data.Hero);
            for(int i = 0; i < m_captainStarList.Count; ++i)
            {
                m_captainStarList[i].m_img_starHighlight_PolygonImage.gameObject.SetActive(m_data.Hero.star > i);
                if (i == (m_data.Hero.star-1))
                {
                    addStarEffect(m_captainStarList[i].gameObject);
                }
            }

            var heroBeforeStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(m_data.OldHeroStar);
            if (heroBeforeStarCfg == null) return;
            view.m_lbl_levelBefore_LanguageText.text = $"{heroBeforeStarCfg.starLimit}";

            var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(m_data.Hero.star);
            if (heroStarCfg == null) return;
            view.m_lbl_levelAfter_LanguageText.text = $"{heroStarCfg.starLimit}";

            if (heroStarCfg.starEffect == 1)
            {
                List<Data.HeroStarDefine> newStarCfgs = new List<Data.HeroStarDefine>();
                for(int i = m_data.OldHeroStar + 1; i <= m_data.Hero.star; ++i)
                {
                    newStarCfgs.Add(CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(i));
                }
                RefreshSkillUnlock(newStarCfgs);
            }
            else if(heroStarCfg.starEffect == 2)
            {
                RefreshMoreTalentPoint(heroStarCfg);
            }
        }

        private void RefreshSkillUnlock(List<Data.HeroStarDefine> heroStarCfgs)
        {
            view.m_pl_point.gameObject.SetActive(false);
            view.m_pl_newskill.gameObject.SetActive(true);
            view.m_UI_Item_CaptainSkill.gameObject.SetActive(false);
            foreach (var cfg in heroStarCfgs)
            {
                var skillSubViewObj = CoreUtils.assetService.Instantiate(view.m_UI_Item_CaptainSkill.gameObject);
                skillSubViewObj.gameObject.SetActive(true);
                skillSubViewObj.transform.SetParent(view.m_pl_skill_GridLayoutGroup.transform);
                skillSubViewObj.transform.localPosition = Vector3.zero;
                skillSubViewObj.transform.localScale = Vector3.one;
                var skillSubView = new UI_Item_CaptainSkill_SubView(skillSubViewObj.transform as RectTransform);
                skillSubView.SetPreviewSkillInfo(m_data.Hero, cfg.ID - 1);
            }
        }

        private void RefreshMoreTalentPoint(Data.HeroStarDefine heroStarCfg)
        {
            view.m_pl_point.gameObject.SetActive(true);
            view.m_pl_newskill.gameObject.SetActive(false);
            view.m_lbl_giftPoint_LanguageText.text = $"{heroStarCfg.starEffectData}";
        }

        private CaptainStarUpSuccessViewData m_data = null;
        private List<UI_Model_CaptainStar_SubView> m_captainStarList = new List<UI_Model_CaptainStar_SubView>();
    }
}