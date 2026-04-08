// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, December 31, 2019
// Update Time         :    Tuesday, December 31, 2019
// Class Description   :    ResearchUpateMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game
{
    public class ResearchUpateMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "ResearchUpateMediator";

        private ResearchProxy m_resProxy;

        private CityBuildingProxy m_buildProxy;

        private CurrencyProxy m_crrProxy;

        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();


        private List<StudyDefine> m_crrStudys = null;

        private StudyDefine m_nextStudy;


        //前置条件
        private List<StudyDefine> preStudyIDs = null;

        private bool isCostEnough = false;

        private long m_needDenar;       //立即完成所需代币
        private Color m_originDenarTextColor; //代币的字体颜色

        #endregion

        //IMediatorPlug needs
        public ResearchUpateMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public ResearchUpateView view;

        private QueueInfo m_qinfo;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.UpdateCurrency,
                CmdConstant.technologyChange,
                CmdConstant.technologyQueueChange,
                Technology_ResearchTechnology.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:

                    BindUIData();
                    break;
                case  CmdConstant.technologyQueueChange:
                    BindUIData();
                    break;
                case  Technology_ResearchTechnology.TagName:
                    BindUIData();
                    break;
                default:
                    break;
            }
        }

        public StudyDefine GetStudyDefine()
        {
            return this.view.data as StudyDefine;
        }


        #region UI template method

        public override void OpenAniEnd()
        {
        }

        public override void WinFocus()
        {
        }

        public override void WinClose()
        {
            if (m_qinfo != null)
            {
   
            }
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            m_resProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;
            m_buildProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;

            m_crrProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.setCloseHandle(CloseRes);
            view.m_btn_cancel_GameButton.onClick.AddListener(onCancel);
        }
        
        private void onCancel()
        {
            Alert.CreateAlert(402016, LanguageUtils.getText(300099)).SetRightButton(onCancelOK,LanguageUtils.getText(300014)).SetLeftButton(null,LanguageUtils.getText(300013)).Show();
        }

        private void onCancelOK()
        {
            m_resProxy.StopTechnology();
            Tip.CreateTip(402022).Show();
            CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
        }


        private void CloseRes()
        {
            CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
        }

        protected override void BindUIData()
        {
            var study = GetStudyDefine();
            var isSoldierStudy = m_resProxy.IsSoldierRes(study.ID);

            var spImg = isSoldierStudy != null ? isSoldierStudy.icon : study.icon;
            var spLanID = isSoldierStudy != null ? isSoldierStudy.l_studyNameID : study.l_nameID;
            var spDes = isSoldierStudy != null ? isSoldierStudy.l_desID : study.l_des;

            view.m_lbl_des_LanguageText.text = LanguageUtils.getText(spDes);
            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage, spImg);

            int maxLv = m_resProxy.GetTechnologyMaxLv(study.studyType);
            int crrLv = m_resProxy.GetCrrTechnologyLv(study.studyType);

            bool isMax = crrLv == maxLv;

            m_crrStudys = m_resProxy.GetTechnologyList(GetStudyDefine().studyType);

            StudyDefine crrStudy = null;
            if (crrLv > 0)
            {
                crrStudy = m_crrStudys[crrLv - 1];
            }
            else
            {
                crrStudy = new StudyDefine();
                crrStudy.desData = "0.0";
                crrStudy.power = 0;
                crrStudy.l_des = m_crrStudys[0].l_des;
                crrStudy.l_buffDesID = m_crrStudys[0].l_buffDesID;
            }


            m_nextStudy = m_crrStudys[crrLv < m_crrStudys.Count - 1 ? crrLv : m_crrStudys.Count - 1];

//            ClientUtils.Print(m_nextStudy);


            view.m_lbl_leveLow_LanguageText.text = string.Format(LanguageUtils.getText(180306), crrLv);

            view.m_lbl_leveHight_LanguageText.text = (crrLv + 1).ToString();
            view.m_pl_upPlane_ArabLayoutCompment.gameObject.SetActive(maxLv > crrLv);

            view.m_pb_bar_GameSlider.value = (float) crrLv / maxLv;

            view.m_lbl_barText_LanguageText.text = string.Format("{0}/{1}", crrLv, maxLv);

            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(spLanID));

            view.m_btn_more_GameButton.onClick.RemoveAllListeners();
            view.m_btn_more_GameButton.onClick.AddListener(onMore);

            
            view.m_UI_Model_AttrList.ClearAllData();
            
            //属性加成
            if (!string.IsNullOrEmpty(m_nextStudy.desData))
            {
                view.m_UI_Model_AttrList.AddAttr(crrStudy.l_buffDesID, crrStudy.desData.Replace("%",""),
                    !isMax
                        ? ClientUtils.FormatDecimal(ClientUtils.PerctangleToDecimal(m_nextStudy.desData) -
                                                    ClientUtils.PerctangleToDecimal(crrStudy.desData))
                        : "",m_nextStudy.desData.Contains("%"));
            }

            view.m_UI_Model_AttrList.AddAttr(300005, ClientUtils.FormatComma(crrStudy.power) ,
                !isMax ? ClientUtils.FormatComma((m_nextStudy.power - crrStudy.power)):"");
            view.m_UI_Model_AttrList.RefreshUI();


            preStudyIDs = m_resProxy.CheckPreAllTechnology(m_nextStudy);

            QueueInfo queueInfo = m_resProxy.GetCrrTechnologying();
            
            view.m_img_up_PolygonImage.gameObject.SetActive(crrLv < maxLv);
            view.m_lbl_leveHight_LanguageText.gameObject.SetActive(crrLv < maxLv);

            if (crrLv == maxLv)
            {
                //最大等级
                view.m_lbl_unlock_LanguageText.text = LanguageUtils.getText(402020);
                view.m_lbl_unlock_LanguageText.gameObject.SetActive(true);
                view.m_pl_condition.gameObject.SetActive(false);
                
                
            }
            else if (preStudyIDs.Count > 0 && crrLv == 0 && m_resProxy.GetCrrTechnologyLv(preStudyIDs[0].studyType)==0)
            {
                //解锁前置等级
                view.m_lbl_unlock_LanguageText.gameObject.SetActive(true);
                view.m_lbl_unlock_LanguageText.text = LanguageUtils.getText(402019);
                view.m_pl_condition.gameObject.SetActive(false);
            }
            else if (queueInfo.finishTime > 0 && queueInfo.technologyType == study.studyType)
            {
                //研究中
                m_qinfo = queueInfo;
                view.m_pl_speedUp_PolygonImage.gameObject.SetActive(true);
                view.m_lbl_unlock_LanguageText.gameObject.SetActive(false);
                view.m_pl_condition.gameObject.SetActive(false);


                ClientUtils.LoadSprite(view.m_img_spIcon_PolygonImage, spImg);
                CityHudCountDownManager.Instance.AddUiQueue(null, view.m_lbl_spTimeLeft_LanguageText,
                    view.m_pb_spBar_GameSlider, queueInfo, null, HidePro, 402021);
            }
            else
            {
                view.m_lbl_unlock_LanguageText.gameObject.SetActive(false);
                //建筑等级判断
                var buildinfo = m_buildProxy.GetBuildingInfoByType((long) EnumCityBuildingType.Academy);
                
                //消耗资源组件
                isCostEnough = view.m_UI_Model_ResCost.UpdateResCost(m_nextStudy.needFood, m_nextStudy.needWood,
                    m_nextStudy.needStone, m_nextStudy.needGold);

                
                if (m_nextStudy.campusLv > buildinfo.level)
                {
                    //学院等级不足
                    view.m_pl_BuildingUpLimit_PolygonImage.gameObject.SetActive(true);
                    view.m_pl_condition.gameObject.SetActive(true);
                    view.m_pl_needPreResearch_PolygonImage.gameObject.SetActive(false);
                    view.m_pl_btns.gameObject.SetActive(false);

                    var buildConfig = m_buildProxy.GetBuildConfig(EnumCityBuildingType.Academy);

                    view.m_UI_BuildingUpLimit.m_lbl_languageText_LanguageText.text =
                        string.Format("{0}\n{1}", LanguageUtils.getText(buildConfig.l_nameId),
                            LanguageUtils.getTextFormat(180306, m_nextStudy.campusLv));
                    view.m_UI_BuildingUpLimit.SetIcon(m_buildProxy.GetImgIdByType(buildConfig.type));

                    view.m_UI_BuildingUpLimit.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick
                        .RemoveAllListeners();
                    view.m_UI_BuildingUpLimit.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick
                        .AddListener(GoResBuild);
                }
                else
                {
                    view.m_pl_condition.gameObject.SetActive(true);

                   

                    if (preStudyIDs.Count > 0)
                    {
                        //可以前置条件
                        m_preLoadRes.AddRange(view.m_sv_preResearchList_ListView.ItemPrefabDataList);
                        ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
                        {
                            Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                            ListView.FuncTab funcTab = new ListView.FuncTab();
                            funcTab.ItemEnter = PreItemByIndex;

                            view.m_sv_preResearchList_ListView.SetInitData(assetDic, funcTab);
                            view.m_sv_preResearchList_ListView.FillContent(preStudyIDs.Count);
                        });

                        view.m_pl_needPreResearch_PolygonImage.gameObject.SetActive(true);
                        view.m_pl_BuildingUpLimit_PolygonImage.gameObject.SetActive(false);
                        view.m_pl_btns.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.m_pl_needPreResearch_PolygonImage.gameObject.SetActive(false);
                        view.m_pl_BuildingUpLimit_PolygonImage.gameObject.SetActive(false);
                        view.m_pl_btns.gameObject.SetActive(true);

                        //研究按钮
                        view.m_btn_research.m_lbl_line1_LanguageText.text = LanguageUtils.getText(402015);
                        
                        PlayerAttributeProxy playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
                        var attr = playerAttributeProxy.GetCityAttribute(attrType.researchSpeedMulti);
                        float speed = attr.origvalue ;

                        float needTime = (float)m_nextStudy.costTime/(1+ speed/1000f);
                        view.m_btn_research.m_lbl_line2_LanguageText.text =   ClientUtils.FormatCountDown((int)needTime);
                        
                        ClientUtils.UIReLayout(view.m_btn_research.m_btn_languageButton_GameButton);
                        
                        m_needDenar = m_crrProxy.CaculateImmediatelyFinishPrice(needTime,m_nextStudy.needWood,m_nextStudy.needFood,m_nextStudy.needStone,m_nextStudy.needGold);
                        view.m_btn_researchNow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(300048);
                        view.m_btn_researchNow.m_lbl_line2_LanguageText.text = m_needDenar.ToString("N0");
                        if(m_originDenarTextColor!=Color.red)
                        {
                            m_originDenarTextColor = view.m_btn_researchNow.m_lbl_line2_LanguageText.color;
                        }
                        view.m_btn_researchNow.m_lbl_line2_LanguageText.color = m_crrProxy.Gem < m_needDenar ? Color.red : m_originDenarTextColor;

                        LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_btn_researchNow
                            .m_btn_languageButton_GameButton.GetComponent<RectTransform>());
                        
                        LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_btn_research
                            .m_btn_languageButton_GameButton.GetComponent<RectTransform>());

                        view.m_btn_research.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                        view.m_btn_research.m_btn_languageButton_GameButton.onClick.AddListener(OnResearch);
                        view.m_btn_researchNow.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                        view.m_btn_researchNow.m_btn_languageButton_GameButton.onClick.AddListener(OnResearchNow);
                    }
                }
            }
        }

        private void HidePro(long time)
        {
            view.m_pl_speedUp_PolygonImage.gameObject.SetActive(false);
//            CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
//            CoreUtils.uiManager.CloseUI(UI.s_ResearchMain);
            
            QueueInfo queueInfo = m_resProxy.GetCrrTechnologying();
            queueInfo.finishTime = -1;
        }

        private void GoResBuild()
        {
            CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
            CoreUtils.uiManager.CloseUI(UI.s_ResearchMain);

            var buildM =
                AppFacade.GetInstance().RetrieveMediator(CityGlobalMediator.NameMediator) as CityGlobalMediator;
            var buildInfo = m_buildProxy.GetBuildingInfoByType((int) EnumCityBuildingType.Academy);

            AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenu, buildInfo);
        }

        private void OnResearch()
        {
            Debug.Log("OnResearch " + isCostEnough);
            
            QueueInfo queueInfo = m_resProxy.GetCrrTechnologying();

            if (queueInfo != null && queueInfo.finishTime > 0)
            {
                Tip.CreateTip(402023).Show();
                return;
            }
            
            if (isCostEnough)
            {
                m_resProxy.StudyTechnology(GetStudyDefine().studyType, false);
                CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);

                CoreUtils.audioService.PlayOneShot(RS.SoundResStart);
            }
            else
            {
                m_crrProxy.LackOfResources(m_nextStudy.needFood, m_nextStudy.needWood, m_nextStudy.needStone,
                    m_nextStudy.needGold);
            }
        }

        private void OnResearchNow()
        {
            if (!m_crrProxy.ShortOfDenar(m_needDenar))
            {
                UIHelper.DenarCostRemain(m_needDenar, () =>
                {
                    CoreUtils.audioService.PlayOneShot(RS.SoundResStart);
                    m_resProxy.StudyTechnology(GetStudyDefine().studyType, true);
                    //                CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
                    //                CoreUtils.uiManager.CloseUI(UI.s_ResearchMain);

                    AddEffect("UI_10015", view.m_img_icon_PolygonImage.transform, (gameobject) =>
                    {
                        Timer.Register(3, () =>
                        {
                            if (gameobject)
                            {
                                CoreUtils.assetService.Destroy(gameobject);
                            }

                        });


                    });

                    CoreUtils.assetService.Instantiate("UE_ResFly_research", (effectGO) =>
                    {
                        UE_ResFly_researchView itemView = MonoHelper.GetOrAddHotFixViewComponent<UE_ResFly_researchView>(effectGO);
                        //飘飞特效
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, GetStudyDefine().icon, false, () =>
                        {
                            mt.FlyPowerUpEffect(effectGO, view.m_btn_researchNow.m_root_RectTransform, Vector3.one);
                            GameObject.DestroyImmediate(effectGO);
                        });
                    });

                });


            }
            else
            {
                CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
            }
        }
        
        public void AddEffect(string effectName, Transform targetPart, Action<GameObject> callBack)
        {
            CoreUtils.assetService.Instantiate(effectName, (effectGO) =>
            {
                effectGO.name = effectName;
                effectGO.SetActive(true);
                effectGO.transform.SetParent(targetPart);
                effectGO.transform.localScale = Vector3.one;
                effectGO.transform.localPosition = Vector3.zero;

                callBack?.Invoke(effectGO);
            });
        }


        //前置条件
        void PreItemByIndex(ListView.ListItem scrollItem)
        {
            ResearchPreItemView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<ResearchPreItemView>(scrollItem.go);
            var a = preStudyIDs[scrollItem.index];
            if (a != null)
            {
                itemView.m_lbl_lv_LanguageText.text = string.Format(LanguageUtils.getText(180306), a.studyLv);
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(a.l_nameID);
                ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, a.icon);
                itemView.m_btn_jump.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_jump.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    
                    ResearchMainMediator md =
                        AppFacade.GetInstance().RetrieveMediator(ResearchMainMediator.NameMediator) as
                            ResearchMainMediator;
                    md.SetJump(a.studyType);
                    
                    CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
                });
            }
        }


        private void onMore()
        {
            view.m_img_update_Animator.gameObject.SetActive(false);
            view.m_pl_power_Animator.gameObject.SetActive(true);
            
            ClientUtils.PlayUIAnimation(view.m_pl_power_Animator,"Show");
    
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.onClick.RemoveAllListeners();
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.onClick.AddListener(OnBack);


            view.m_lbl_subDes_LanguageText.text = LanguageUtils.getText(m_crrStudys[0].l_buffDesID);

            m_preLoadRes.AddRange(view.m_sv_power_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;


                Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;


                view.m_sv_power_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_power_ListView.FillContent(m_crrStudys.Count);
            });
        }

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            ResearchPowerItemView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<ResearchPowerItemView>(scrollItem.go);
            var a = m_crrStudys[scrollItem.index];
            if (a != null)
            {
                itemView.m_lbl_lv_LanguageText.text = a.studyLv.ToString();

                if (!string.IsNullOrEmpty(a.desData))
                {
                    itemView.m_lbl_ack_LanguageText.text = a.desData;
                }
                else
                {
                    itemView.m_lbl_ack_LanguageText.text = LanguageUtils.getText(a.l_function);
                }

                itemView.m_lbl_power_LanguageText.text = string.Format("+{0}", ClientUtils.FormatComma(a.power) );
            }
        }

        private void OnBack()
        {
            view.m_img_update_Animator.gameObject.SetActive(true);
            view.m_pl_power_Animator.gameObject.SetActive(false);
            
            ClientUtils.PlayUIAnimation(view.m_img_update_Animator,"Show");
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(false);
        }

        #endregion
    }
}