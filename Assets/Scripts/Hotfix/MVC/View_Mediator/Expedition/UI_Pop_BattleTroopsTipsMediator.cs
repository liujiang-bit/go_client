// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月6日
// Update Time         :    2020年7月6日
// Class Description   :    UI_Pop_BattleTroopsTipsMediator
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

    public class BattleTroopsTipsData
    {
        public Vector3 ScreenPosition { get; set; }
        public int EnemyTroopIndex { get; set; }

        public Vector2 Offset { get; set; }
    }


    public class UI_Pop_BattleTroopsTipsMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_BattleTroopsTipsMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Pop_BattleTroopsTipsMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_BattleTroopsTipsView view;

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
            m_data = view.data as BattleTroopsTipsData;
            Refresh();
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void Refresh()
        {
            int enemyIndex = m_data.EnemyTroopIndex;           
            ExpeditionProxy epxedition = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            var monsterData = epxedition.GetMonsterTroopData(enemyIndex);

            BattleTroopsTipHeroData mainHero = new BattleTroopsTipHeroData()
            {
                HeroId = monsterData.TroopsCfg.heroID1,
                HeroLevel = monsterData.TroopsCfg.heroLevel1,
                HeroStar = monsterData.TroopsCfg.hero1Star,
                HeroAwake = monsterData.TroopsCfg.hero1AwakenSkill > 0,
                HeroSkillLevel = new System.Collections.Generic.List<int>()
                {
                    monsterData.TroopsCfg.hero1SkillLevel1,
                    monsterData.TroopsCfg.hero1SkillLevel2,
                    monsterData.TroopsCfg.hero1SkillLevel3,
                    monsterData.TroopsCfg.hero1SkillLevel4,
                }
            };
            view.m_UI_Item_BattleTroopsTipsCaptain1.Show(mainHero);

            BattleTroopsTipHeroData deputyHero = null;
            if (monsterData.TroopsCfg.heroID2 != 0)
            {
                deputyHero = new BattleTroopsTipHeroData()
                {
                    HeroId = monsterData.TroopsCfg.heroID2,
                    HeroLevel = monsterData.TroopsCfg.heroLevel2,
                    HeroStar = monsterData.TroopsCfg.hero2Star,
                    HeroAwake = monsterData.TroopsCfg.hero2AwakenSkill > 0,
                    HeroSkillLevel = new System.Collections.Generic.List<int>()
                {
                    monsterData.TroopsCfg.hero2SkillLevel1,
                    monsterData.TroopsCfg.hero2SkillLevel2,
                    monsterData.TroopsCfg.hero2SkillLevel3,
                    monsterData.TroopsCfg.hero2SkillLevel4,
                }
                };
            }
            view.m_UI_Item_BattleTroopsTipsCaptain2.gameObject.SetActive(deputyHero != null);
            if (deputyHero != null)
            {
                view.m_UI_Item_BattleTroopsTipsCaptain2.Show(deputyHero);
            }
            int soldierCount = 0;
            foreach (var soldier in monsterData.Soldiers)
            {
                soldierCount += (int)soldier.Value.num;
                var obj = CoreUtils.assetService.Instantiate(view.m_UI_Item_SoldierHead.gameObject);
                obj.transform.SetParent(view.m_pl_allarmys_GridLayoutGroup.transform);
                obj.transform.position = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                var subView = new UI_Item_SoldierHead_SubView(obj.transform as RectTransform);
                subView.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int)soldier.Value.id), (int)soldier.Value.num);
            }
            view.m_UI_Item_SoldierHead.gameObject.SetActive(false);
            view.m_lbl_armyNum_LanguageText.text = ClientUtils.FormatComma(soldierCount);
            AdjustUIPos();
        }

        private void AdjustUIPos()
        {          
            var screenPoint = m_data.ScreenPosition;
            var size = view.m_img_bg_PolygonImage.rectTransform.sizeDelta;
            bool isLeft = false;
            if (screenPoint.x < Screen.width / 2)
            {
                isLeft = true;
            }
            Vector2 position = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.transform as RectTransform, screenPoint, CoreUtils.uiManager.GetUICamera(), out position);
            float halfSize = size.x / 2;
            position.x = isLeft ? position.x + halfSize : position.x - halfSize;
            position.x += isLeft ? m_data.Offset.x : -m_data.Offset.x;
            view.m_img_bg_PolygonImage.rectTransform.anchoredPosition = position;
            view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(isLeft);
            view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(!isLeft);
        }

        private BattleTroopsTipsData m_data = null;
    }
}