// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Win_TheTowerMediator
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
    public class UI_Win_TheTowerMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_TheTowerMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;

        private BuldingObjData m_guardTowerData;

        private string img_build;//建筑物图片
        private float m_warningTowerHpMax;//警戒塔最大生命值
        #endregion

        //IMediatorPlug needs
        public UI_Win_TheTowerMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_TheTowerView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                 CmdConstant.GuardTowerHpChange,
                 CmdConstant.CityBuildingLevelUP,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.GuardTowerHpChange:
                    {
                        RefreshStateView();
                    }
                    break;
                case CmdConstant.CityBuildingLevelUP:
                    {
                        BuildingGuardTowerDefine buildingGuardTowerDefine = CoreUtils.dataService.QueryRecord<BuildingGuardTowerDefine>((int)m_guardTowerData.buildingInfoEntity. level);
                        if (buildingGuardTowerDefine != null)
                        {
                            m_warningTowerHpMax = buildingGuardTowerDefine.warningTowerHpMax;
                        }
                        RefreshStateView();
                        RefreshBuildImgView();
                    }
                    break;
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
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_guardTowerData = m_cityBuildingProxy.GetBuldingObjDataByType(EnumCityBuildingType.GuardTower);

            BuildingGuardTowerDefine  buildingGuardTowerDefine = CoreUtils.dataService.QueryRecord<BuildingGuardTowerDefine>((int)m_guardTowerData.buildingInfoEntity.level);
            if (buildingGuardTowerDefine != null)
            {
                m_warningTowerHpMax = buildingGuardTowerDefine.warningTowerHpMax;
            }
            RefreshBuildImgView();
            RefreshStateView();

        }

        protected override void BindUIEvent()
        {
            view.m_btn_BuildLevelInfo_GameButton.onClick.AddListener(OnInfoBtnClick);
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.onClick.AddListener(OnBackBtnClick);
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_theTower);
            });
        }

        protected override void BindUIData()
        {
             img_build = m_cityBuildingProxy.GetImgIdByType((long)m_guardTowerData.type);
            CoreUtils.assetService.Instantiate(img_build, (go) =>
            {
                go.transform.SetParent(view.m_img_buildImg_PolygonImage.transform);
                go.transform.localPosition = new Vector3(0, 0, 0);
                go.transform.localScale = new Vector3(1, 1, 1);
            });
        }

        #endregion

        private void RefreshStateView()
        {
            int lanid = 0;
            int protectPer = 100;
            int attackPer = 100;
            long guardTowerHp = m_playerProxy.CurrentRoleInfo.guardTowerHp;
         //    guardTowerHp = 500;
            if (guardTowerHp > m_warningTowerHpMax * 0.8f)
            {
                lanid = 180709;
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_green);
                protectPer = 30;
                attackPer = 100;
            }
            else if (guardTowerHp > m_warningTowerHpMax * 0.6f)
            {
                lanid = 180710;
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_green);
                protectPer = 25;
                attackPer =  90;
            }
            else if (guardTowerHp > m_warningTowerHpMax * 0.4f)
            {
                lanid = 180711;
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_yellow);
                protectPer = 20;
                attackPer = 80;
            }
            else if (guardTowerHp > m_warningTowerHpMax * 0.2f)
            {
                lanid = 180712;
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_red);
                protectPer = 10;
                attackPer = 70;
            }
            else if (guardTowerHp > 0)
            {
                lanid = 180712;
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_gray);
                protectPer = 10;
                attackPer = 70;
            }
            else
            {
                lanid = 180713;
                protectPer = 0;
                attackPer = 0;
            }
            view.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(180708, LanguageUtils.getText(lanid));
            view.m_pb_bar_GameSlider.maxValue = m_warningTowerHpMax;
            view.m_pb_bar_GameSlider.minValue = 0;
            view.m_pb_bar_GameSlider.value = guardTowerHp;
            view.m_lbl_hp_LanguageText.text = LanguageUtils.getTextFormat(180714, guardTowerHp.ToString("N0"), m_warningTowerHpMax.ToString("N0"));
            view.m_lbl_tiptext_LanguageText.text = string.Format("{0}\n{1}" ,LanguageUtils.getTextFormat(180715, protectPer), LanguageUtils.getTextFormat(180716, attackPer));
            if (LanguageUtils.IsArabic())
            {
                view.m_img_Fill_PolygonImage.rectTransform.pivot = new Vector2(0, 0.5f);
            }
            else
            {
                view.m_img_Fill_PolygonImage.rectTransform.pivot = new Vector2(1, 0.5f);
            }
            view.m_img_tips_PolygonImage.transform.position = new Vector2(view.m_img_Fill_PolygonImage.transform.position.x, view.m_img_tips_PolygonImage.transform.position.y);
        }
        private void RefreshBuildImgView()
        {
            string img =  m_cityBuildingProxy.GetImgIdByType((long)m_guardTowerData.type);
            if (!string.Equals(img,img_build))
            {
                img_build = img;
                view.m_img_buildImg_PolygonImage.transform.DestroyAllChild();
                CoreUtils.assetService.Instantiate(img_build, (go) =>
                {
                    go.transform.SetParent(view.m_img_buildImg_PolygonImage.transform);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    go.transform.localScale = new Vector3(1, 1, 1);
                });
            }
        }

        private void OnInfoBtnClick()
        {
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_pl_Right_Animator.Play("ShowDesc");
        }

        public void OnBackBtnClick()
        {
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(false);
         view.m_pl_Right_Animator.Play("ShowInfo");

        }

    }
}