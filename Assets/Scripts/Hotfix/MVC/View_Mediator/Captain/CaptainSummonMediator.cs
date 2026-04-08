// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月27日
// Update Time         :    2019年12月27日
// Class Description   :    CaptainSummonMediator
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
    public class CaptainSummonParam
    {
        public int Source; //1 酒馆召唤界面
        public int HeroId;
        public bool IsNew;  //是否是新英雄
        public Action Callback;
    }
    public class CaptainSummonMediator : GameMediator {
        #region Member
        public static string NameMediator = "CaptainSummonMediator";

        private long m_heroId = 0;

        private CaptainSummonParam m_param;

        #endregion

        //IMediatorPlug needs
        public CaptainSummonMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public CaptainSummonView view;
        private Transform m_3dNode;
        private Transform m_camera;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

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

        public override void WinFocus()
        {
            
        }

        public override void WinClose(){
            AppFacade.GetInstance().SendNotification(CmdConstant.HeroSceneVisible,true);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            if(m_param!=null)
            {
                if (m_param.Source == 1)
                {
                    if (m_param.Callback != null)
                    {
                        m_param.Callback();
                    }
                }
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            
            m_3dNode = view.m_UI_3D_Scene_Image.transform.Find("3D/Scene");
            m_camera = view.m_UI_3D_Scene_Image.transform.Find("3D/Camera");
            if (view.data is CaptainSummonParam)
            {
                m_param = view.data as CaptainSummonParam;
                m_heroId = m_param.HeroId;
            }
            else
            {
                if (m_heroId == (long)view.data)
                    return;
                m_heroId = (long)view.data;
            }

            var heroInfo = CoreUtils.dataService.QueryRecord<Data.HeroDefine>((int)m_heroId);
            
            List<string> prefabNames = new List<string>();
            prefabNames.Add(heroInfo.heroScene);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_btn_title_GameButton.onClick.AddListener(OnShowTileClick);

            view.m_UI_Model_StandardButton_Blue_sure.AddClickEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_captainSummon);
            });
            view.m_pl_modelIF.AddClickEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_captainSummon);
            });
        }

        #endregion
        
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            UpdateHero();
        }
        private void UpdateHero()
        {
            
            AppFacade.GetInstance().SendNotification(CmdConstant.HeroSceneVisible,false);
            
            var heroInfo = CoreUtils.dataService.QueryRecord<Data.HeroDefine>((int)m_heroId);

            CoreUtils.audioService.PlayOneShot(heroInfo.voiceOpening);
            ClientUtils.LoadSpine(view.m_spin_hero_SkeletonGraphic, heroInfo.heroModel);


            if (m_assetDic.ContainsKey(heroInfo.heroScene))
            {
                GameObject heroScene = GameObject.Instantiate(m_assetDic[heroInfo.heroScene]);
                if (m_3dNode.transform.childCount > 0)
                {
                    var childNode = m_3dNode.transform.GetChild(0);
                    GameObject.Destroy(childNode.gameObject);
                }
                heroScene.transform.SetParent(m_3dNode.transform);
                heroScene.transform.localPosition = Vector3.zero;
                heroScene.transform.localScale = Vector3.one;
                if (heroInfo.heroScenePoint != null)
                {
                    m_camera.localPosition = new Vector3(heroInfo.heroScenePoint[0],heroInfo.heroScenePoint[1],heroInfo.heroScenePoint[2]);
                }
            }
            else
            {
                CoreUtils.assetService.Instantiate(heroInfo.heroScene, (heroScene) =>
                {
                    if (m_3dNode.transform.childCount > 0)
                    {
                        var childNode = m_3dNode.transform.GetChild(0);
                        CoreUtils.assetService.Destroy(childNode.gameObject);
                    }
                    heroScene.transform.SetParent(m_3dNode.transform);
                    heroScene.transform.localPosition = Vector3.zero;
                    heroScene.transform.localScale = Vector3.one;
                    if (heroInfo.heroScenePoint != null)
                    {
                        m_camera.localPosition = new Vector3(heroInfo.heroScenePoint[0],heroInfo.heroScenePoint[1],heroInfo.heroScenePoint[2]);
                    }
                });
            }
            
            
            view.m_UI_Item_CaptainHead.SetHeroID((int)m_heroId);

            view.m_lbl_text_LanguageText.text = LanguageUtils.getText(heroInfo.l_desID);
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(heroInfo.l_nameID);
            view.m_lbl_title_LanguageText.text = LanguageUtils.getText(heroInfo.l_appellationID);

            var config = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>((int)0);
            view.m_lbl_quality_LanguageText.text = LanguageUtils.getText(heroInfo.rare + config.rareLanguage - 1);
            //Color c;
            //ColorUtility.TryParseHtmlString(RS.HeroQualityColor[heroInfo.rare-1], out c);
            //view.m_lbl_quality_LanguageText.color = c;

            var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)heroInfo.civilization);
            if (civilizationInfo != null)
            {
                ClientUtils.LoadSprite(view.m_img_img_PolygonImage, civilizationInfo.civilizationMark);
                //ColorUtility.TryParseHtmlString(civilizationInfo.markColour, out c);
                //view.m_img_img_PolygonImage.color = c;
                view.m_img_img_PolygonImage.enabled = true;
            }
            else
            {
                view.m_img_img_PolygonImage.enabled = false;
            }

            view.m_UI_Model_CaptainTalent_1.SetTalentId(heroInfo.talent[0]);
            view.m_UI_Model_CaptainTalent_2.SetTalentId(heroInfo.talent[1]);
            view.m_UI_Model_CaptainTalent_3.SetTalentId(heroInfo.talent[2]);

            if (m_param != null && !m_param.IsNew)
            {
                view.m_lbl_tipOnBtn_LanguageText.gameObject.SetActive(true);
                view.m_lbl_tipOnBtn_LanguageText.text = LanguageUtils.getText(760026);
            }
        }
        private void OnShowTileClick()
        {
            var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            var hero = heroProxy.GetHeroByID(m_heroId);
            var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)hero.config.civilization);
            var data1 = LanguageUtils.getText(civilizationInfo.l_civilizationID);
            HelpTip.CreateTip(data1, view.m_btn_title_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }
    }
}