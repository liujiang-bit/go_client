// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Win_TheWallMediator
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
using Hotfix;
using System;
using Data;
using DG.Tweening;
using UnityEngine.UI;

namespace Game
{
    public class UI_Win_TheWallMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_TheWallMediator";
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private HeroProxy m_heroProxy;
        private BuldingObjData m_wallData;
        private CurrencyProxy m_currencyProxy;
        private List<HeroProxy.Hero> m_ownHeros;
        private List<HeroProxy.Hero> m_summonHeros;
        private List<HeroProxy.Hero> m_noSummomHeros;
        private Timer m_repairTimer;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<string> m_preLoadRes = new List<string>();
        HeroProxy.Hero mainHero;
        HeroProxy.Hero deputyHero;
        private Color m_originDenarTextColor; //代币的默认字体颜色
        private int pagetype = 1; //1 主界面，2详情界面
        #endregion

        //IMediatorPlug needs
        public UI_Win_TheWallMediator(object viewComponent) : base(NameMediator, viewComponent) { }

        public UI_Win_TheWallView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.MainHeroIdChange,
               CmdConstant.DeputyHeroIdChange,
               CmdConstant.CityBuildinginfoChange,
               CmdConstant.UpdateCurrency,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.MainHeroIdChange:
                    mainHero = m_heroProxy.GetHeroByID(m_playerProxy.CurrentRoleInfo.mainHeroId);
                    RefreshMainHeroView();
                    RefreashPowerView();
                    break;
                case CmdConstant.DeputyHeroIdChange:
                    deputyHero = m_heroProxy.GetHeroByID(m_playerProxy.CurrentRoleInfo.deputyHeroId);
                    RefreashDeputyHeroView();
                    RefreashPowerView();
                    break;
                case CmdConstant.CityBuildinginfoChange:
                    {
                        RefreashStateView();
                    }
                    break;
                case CmdConstant.UpdateCurrency:
                    {
                        RefreshFireBtnView();
                    }
                    break;
                default:
                    break;
            }
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
            ClearRepairTimer();
        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_wallData = m_cityBuildingProxy.GetBuldingObjDataByType(EnumCityBuildingType.CityWall);
            mainHero = m_heroProxy.GetHeroByID(m_playerProxy.CurrentRoleInfo.mainHeroId);
            deputyHero = m_heroProxy.GetHeroByID(m_playerProxy.CurrentRoleInfo.deputyHeroId);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_info_GameButton.onClick.AddListener(BtnInfoClick);
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.onClick.AddListener(BtnBackClick);
            view.m_btn_fire.AddClickEvent(BtnFireClick);
            view.m_btn_repair.AddClickEvent(BtnRepairClick);
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_theWall);
            });
            view.m_btn_hero_add1_GameButton.onClick.AddListener(BtnMainHeroClick);
            view.m_btn_hero_add2_GameButton.onClick.AddListener(BtnDeputyHeroClick);

        }

        protected override void BindUIData()
        {
            m_originDenarTextColor = view.m_btn_fire.m_lbl_line2_LanguageText.color;
            m_preLoadRes.AddRange(view.m_UI_Item_CaptainList.m_sv_captainHead_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
            });

            RefreshMainHeroView();
            RefreashDeputyHeroView();
            RefreashPowerView();
            RefreashStateView();
            RefreshFireBtnView();
            string str = m_cityBuildingProxy.GetImgIdByType((long)m_wallData.type);
            CoreUtils.assetService.Instantiate(str, (go) =>
            {
                go.transform.SetParent(view.m_pl_wallPoint_ArabLayoutCompment.transform);
                go.transform.localPosition = new Vector3(0, 0, 0);
                go.transform.localScale = new Vector3(1, 1, 1);
            });
            view.m_lbl_walldesct_LanguageText.text = LanguageUtils.getText(181177);

            MoveUIPos(true);
        }



        #endregion

        private void InitView()
        {

        }

        private void RefreshFireBtnView()
        {
            int needDenar = m_playerProxy.ConfigDefine.cityWallOutfire;
            view.m_btn_fire.m_lbl_line2_LanguageText.color = m_currencyProxy.Gem < needDenar ? Color.red : m_originDenarTextColor;
            view.m_btn_fire.SetNum(needDenar.ToString("N0"));
            float width1 = view.m_btn_fire.m_lbl_line2_LanguageText.preferredWidth;
            view.m_btn_fire.m_lbl_line2_LanguageText.GetComponent<RectTransform>().sizeDelta = new Vector2(width1, 27.7f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_btn_fire.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
        }
        private void RefreashStateView()
        {
            int wallDurableMax = 0;
            int wallDurable = 0;
            BuildingCityWallDefine buildingCityWallDefine = CoreUtils.dataService.QueryRecord<BuildingCityWallDefine>((int)m_wallData.buildingInfoEntity.level);
            if (buildingCityWallDefine != null)
            {
                wallDurableMax = buildingCityWallDefine.wallDurableMax;
            }
            view.m_pb_wallhpbar_GameSlider.minValue = 0;
            if (LanguageUtils.IsArabic())
            {
                view.m_img_Fill_PolygonImage.rectTransform.pivot = new Vector2(0, 0.5f);
            }
            else
            {
                view.m_img_Fill_PolygonImage.rectTransform.pivot = new Vector2(1, 0.5f);
            }
            wallDurable = wallDurableMax - (int)m_wallData.buildingInfoEntity.lostHp;
            view.m_pb_wallhpbar_GameSlider.maxValue = wallDurableMax;
            view.m_pb_wallhpbar_GameSlider.value = wallDurable; ;
            RefreshRepairTime();
            if ((m_wallData.buildingInfoEntity.beginBurnTime != 0 && ServerTimeModule.Instance.GetServerTime() - m_wallData.buildingInfoEntity.beginBurnTime < 30 * 60) && m_wallData.buildingInfoEntity.lostHp != 0)
            {
                view.m_lbl_walldesc_LanguageText.text = LanguageUtils.getText(181174);
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_red);
                view.m_lbl_wallstate_LanguageText.text = LanguageUtils.getTextFormat(181170, LanguageUtils.getText(181172));
                view.m_btn_fire.gameObject.SetActive(true);
                if (pagetype == 1)
                {
                    if (view.m_img_Fill_PolygonImage.transform.Find("UI_10020") == null)
                    {
                        ClientUtils.UIAddEffect("UI_10020", view.m_img_Fill_PolygonImage.transform, (go) =>
                        {
                            if (LanguageUtils.IsArabic())
                            {

                            }
                            else
                            {
                                go.transform.Rotate(0, 180, 0);
                            }
                        });
                    }
                }
            }
            else if ((m_wallData.buildingInfoEntity.beginBurnTime == 0 || ServerTimeModule.Instance.GetServerTime() - m_wallData.buildingInfoEntity.beginBurnTime >= 30 * 60) && m_wallData.buildingInfoEntity.lostHp != 0)
            {
                view.m_lbl_walldesc_LanguageText.text = LanguageUtils.getText(181174);
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_yellow);
                view.m_lbl_wallstate_LanguageText.text = LanguageUtils.getTextFormat(181170, LanguageUtils.getText(181173));
                view.m_btn_fire.gameObject.SetActive(false);
                if (pagetype == 1)
                {
                    if (view.m_img_Fill_PolygonImage.transform.Find("UI_10020") != null)
                    {
                        view.m_img_Fill_PolygonImage.transform.DestroyAllChild();
                    }
                }
            }
            else if (m_wallData.buildingInfoEntity.beginBurnTime == 0 && m_wallData.buildingInfoEntity.lostHp == 0)
            {
                view.m_lbl_walldesc_LanguageText.text = LanguageUtils.getText(181169);
                ClientUtils.LoadSprite(view.m_img_Fill_PolygonImage, RS.GameSlider_green);
                view.m_lbl_wallstate_LanguageText.text = LanguageUtils.getTextFormat(181170, LanguageUtils.getText(181171));
                view.m_btn_fire.gameObject.SetActive(false);
                view.m_btn_repair.gameObject.SetActive(false);
                if (view.m_img_Fill_PolygonImage.transform.Find("UI_10020") != null)
                {
                    view.m_img_Fill_PolygonImage.transform.DestroyAllChild();
                }
            }
            else
            {
                Debug.LogErrorFormat("error 不存在的状态{0},{1},{2}", m_wallData.buildingInfoEntity.beginBurnTime, m_wallData.buildingInfoEntity.lostHp, 1);
            }
            view.m_lbl_wallhp_LanguageText.text = LanguageUtils.getTextFormat(181104, wallDurable.ToString("N0"), wallDurableMax.ToString("N0"));
        }
        private void RefreshMainHeroView()
        {
            if (mainHero != null)
            {
                view.m_UI_CaptainHead0.SetIcon(mainHero.config.heroIcon);
                view.m_UI_CaptainHead0.SetLevel(mainHero.level);
                view.m_UI_CaptainHead0.SetRare(mainHero.config.rare);
                view.m_lbl_mainName_LanguageText.text = LanguageUtils.getText(mainHero.config.l_nameID);
                view.m_img_skill1_1.SetSkillInfo(mainHero, 0, 2);
                view.m_img_skill1_2.SetSkillInfo(mainHero, 1, 2);
                view.m_img_skill1_3.SetSkillInfo(mainHero, 2, 2);
                view.m_img_skill1_4.SetSkillInfo(mainHero, 3, 2);
                if (mainHero.config.skill.Count == 4)
                {
                    view.m_img_skill1_5.gameObject.SetActive(false);
                }
                else
                {
                    view.m_img_skill1_5.SetSkillInfo(mainHero, 4, 2);

                }
            }
            else
            {
                view.m_UI_CaptainHead0.gameObject.SetActive(false);
                view.m_lbl_mainName_LanguageText.text = "";
                view.m_pl_sklii1_GridLayoutGroup.gameObject.SetActive(false);
                view.m_pl_skill1bg_GridLayoutGroup.gameObject.SetActive(true);
            }
        }
        private void RefreashPowerView()
        {
            int power = 0;
            if (mainHero != null)
            {
                power += mainHero.power;
            }
            if (deputyHero != null)
            {
                power += deputyHero.power;
            }
            view.m_lbl_power_LanguageText.text = LanguageUtils.getTextFormat(181167, power.ToString("N0"));
            view.m_lbl_power_LanguageText.gameObject.SetActive(power != 0);
        }
        private void RefreashDeputyHeroView()
        {
            if (deputyHero != null)
            {
                view.m_UI_CaptainHead1.SetIcon(deputyHero.config.heroIcon);
                view.m_UI_CaptainHead1.SetLevel(deputyHero.level);
                view.m_UI_CaptainHead1.SetRare(deputyHero.config.rare);
                view.m_lbl_subName_LanguageText.text = LanguageUtils.getText(deputyHero.config.l_nameID);
                view.m_img_skill2_1.SetSkillInfo(deputyHero, 0, 2);
                view.m_img_skill2_2.SetSkillInfo(deputyHero, 1, 2);
                view.m_img_skill2_3.SetSkillInfo(deputyHero, 2, 2);
                view.m_img_skill2_4.SetSkillInfo(deputyHero, 3, 2);

                if (deputyHero.config.skill.Count == 4)
                {
                    view.m_img_skill2_5.gameObject.SetActive(false);
                }
                else
                {
                    view.m_img_skill2_5.SetSkillInfo(deputyHero, 4, 2);
                }
            }
            else
            {
                view.m_UI_CaptainHead1.gameObject.SetActive(false);
                view.m_lbl_subName_LanguageText.text = "";
                view.m_pl_sklii2_GridLayoutGroup.gameObject.SetActive(false);
                view.m_pl_skill2bg_GridLayoutGroup.gameObject.SetActive(true);
            }
        }

        private void RefreshRepairTime()
        {
            long costTime = m_playerProxy.ConfigDefine.cityWallMaintainCoolingTime;
            view.m_btn_repair.gameObject.SetActive(true);
            ClearRepairTimer();
            if (ServerTimeModule.Instance.GetServerTime() < (costTime + m_wallData.buildingInfoEntity.serviceTime))
            {
                view.m_btn_repair.SetInteractable(false);
                view.m_btn_repair.ShowLineTwo(true);
                long refreshTime = m_wallData.buildingInfoEntity.serviceTime + costTime;
                UpdateRepairTime(refreshTime);
                m_repairTimer = Timer.Register(1, () =>
                    {
                        UpdateRepairTime(refreshTime);
                    }
                    , null, true, true, view.vb);
            }
            else
            {
                view.m_btn_repair.SetInteractable(true);
                view.m_btn_repair.ShowLineTwo(false);
            }
        }

        private void UpdateRepairTime(long refreshTime)
        {
            long leftTime = refreshTime - ServerTimeModule.Instance.GetServerTime();
            if (leftTime <= 0)
            {
                leftTime = 0;
                ClearRepairTimer();
                RefreashStateView();
            }
            view.m_btn_repair.SetNum(ClientUtils.FormatCountDown((int)leftTime));

        }

        private void ClearRepairTimer()
        {
            if (m_repairTimer != null)
            {
                m_repairTimer.Cancel();
                m_repairTimer = null;
            }
        }

        #region 点击事件
        private void BtnInfoClick()
        {
            view.m_pl_rect_Animator.Play("ShowDesc");
            pagetype = 2;
            if (view.m_img_Fill_PolygonImage.transform.Find("UI_10020") != null)
            {
                view.m_img_Fill_PolygonImage.transform.DestroyAllChild();
            }
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(true);
        }
        private void BtnBackClick()
        {
            view.m_pl_rect_Animator.Play("Show0");
            pagetype = 1;
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(false);
            RefreashStateView();
        }
        private void BtnRepairClick()
        {
            if (ServerTimeModule.Instance.GetServerTime() >= (m_playerProxy.ConfigDefine.cityWallMaintainCoolingTime + m_wallData.buildingInfoEntity.serviceTime))
            {
                Build_Service.request req = new Build_Service.request();
                AppFacade.GetInstance().SendSproto(req);
            }
            else
            {
            }

        }

        private void BtnFireClick()
        {
            if (m_cityBuildingProxy.MyCityObjData.fireState == FireState.FIRED)
            {
                Alert.CreateAlert(LanguageUtils.getTextFormat(300072, m_playerProxy.ConfigDefine.cityWallOutfire.ToString("N0"))).SetRightButton(() =>
                {
                    if (!m_currencyProxy.ShortOfDenar(m_playerProxy.ConfigDefine.cityWallOutfire))
                    {
                        Build_Extinguishing.request req = new Build_Extinguishing.request();
                        AppFacade.GetInstance().SendSproto(req);
                    }
                }).SetLeftButton().Show();
            }

        }
        private void BtnMainHeroClick()
        {
            MoveUIPos(false);
            CaptainListData captainListData = new CaptainListData();
            captainListData.selectHero = mainHero;
            captainListData.title = LanguageUtils.getText(200015);
            captainListData.CloseCallback = SelectMainHero;
            captainListData.m_assetDic = m_assetDic;
            captainListData.type = HeroProxy.SortType.Level;
            captainListData.ignoreHero = deputyHero;
            captainListData.ignoreOut = true;
            view.m_UI_Item_CaptainList.SetData(captainListData);
        }
        private void SelectMainHero(HeroProxy.Hero hero)
        {
            Build_DefendHero.request req = new Build_DefendHero.request();
            req.mainHeroId = hero.config.ID;
            AppFacade.GetInstance().SendSproto(req);
            MoveUIPos(true);
        }



        private void SelectDeputyHero(HeroProxy.Hero hero)
        {
            Build_DefendHero.request req = new Build_DefendHero.request();
            req.deputyHeroId = hero.config.ID;
            AppFacade.GetInstance().SendSproto(req);
            MoveUIPos(true);

        }

        private void BtnDeputyHeroClick()
        {
            MoveUIPos(false);
            if (m_playerProxy.CurrentRoleInfo.mainHeroId != 0)
            {
                CaptainListData captainListData = new CaptainListData();
                captainListData.selectHero = deputyHero;
                captainListData.title = LanguageUtils.getText(200016); ;
                captainListData.m_assetDic = m_assetDic;
                captainListData.CloseCallback = SelectDeputyHero;
                captainListData.type = HeroProxy.SortType.Level;
                captainListData.ignoreOut = true;
                captainListData.ignoreHero = mainHero;
                view.m_UI_Item_CaptainList.SetData(captainListData);
            }
            else
            {
                CaptainListData captainListData = new CaptainListData();
                captainListData.selectHero = m_heroProxy.GetHeroByID(m_playerProxy.CurrentRoleInfo.mainHeroId);
                captainListData.title = LanguageUtils.getText(200015);
                captainListData.CloseCallback = SelectMainHero;
                captainListData.m_assetDic = m_assetDic;
                captainListData.ignoreOut = true;
                captainListData.type = HeroProxy.SortType.Level;
                view.m_UI_Item_CaptainList.SetData(captainListData);
            }
        }
        #endregion


        private void MoveUIPos(bool moveOut = false)
        {
            if (moveOut)
            {
                if (LanguageUtils.IsArabic())
                {
                    view.m_UI_Model_Window_Type2.gameObject.transform.DOLocalMoveX(0, 0.2f);
                    view.m_pl_rect_Animator.gameObject.transform.DOLocalMoveX(0, 0.2f);
                    view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(870, 0.2f).OnComplete(() =>
                    {
                        view.m_UI_Item_CaptainList.gameObject.SetActive(false);
                    });
                }
                else
                {
                    view.m_UI_Model_Window_Type2.gameObject.transform.DOLocalMoveX(0, 0.2f);
                    view.m_pl_rect_Animator.gameObject.transform.DOLocalMoveX(0, 0.2f);
                    view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(-870, 0.2f).OnComplete(() =>
                    {
                        view.m_UI_Item_CaptainList.gameObject.SetActive(false);
                    });
                }
            }
            else
            {
                if (LanguageUtils.IsArabic())
                {
                    view.m_UI_Item_CaptainList.gameObject.SetActive(true);
                    view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(580, 0.2f);
                    view.m_UI_Model_Window_Type2.gameObject.transform.DOLocalMoveX(-138, 0.2f);
                    view.m_pl_rect_Animator.gameObject.transform.DOLocalMoveX(-130, 0.2f);
                }
                else
                {
                    view.m_UI_Item_CaptainList.gameObject.SetActive(true);
                    view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(-580, 0.2f);
                    view.m_UI_Model_Window_Type2.gameObject.transform.DOLocalMoveX(138, 0.2f);
                    view.m_pl_rect_Animator.gameObject.transform.DOLocalMoveX(130, 0.2f);
                }
            }
        }
    }
}