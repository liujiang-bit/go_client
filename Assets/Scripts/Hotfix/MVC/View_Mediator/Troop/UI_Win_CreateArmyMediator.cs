// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Win_CreateArmyMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Skyunion;
using Client;
using DG.Tweening;
using Hotfix;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;
using Data;
using System.Text;

namespace Game
{
    public enum OpenPanelType
    {
        None=0,
        Common, //普通创建部队       
        CreateRally, //集结
        JoinRally, //加入集结
        Reinfore  // 增援
    }

    public class OpenPanelData
    {
        public int id;
        public OpenPanelType type= OpenPanelType.Common;
        public long jonRid;
        public long armyIndex;
        public long rallyRid;
        public int timesType;
        public long reinforceObjectIndex;
        public Vector2 pos;
        public bool isAllianceBulid = false;
        public long rallyTroopNum = 0;
        public int ExpeditionTroopIndex { get; set; }
        public long joinRallyTimes;
        /// <summary>
        /// 默认为true，增援情况下，目标在视野外则摄像机移动锁定目标。为false，不移动摄像机。
        /// </summary>
        public bool viewFlag = true;
        public long targetMonsterId = 0;

        public OpenPanelData(int id, OpenPanelType type)
        {
            this.id = id;
            this.type = type;
        }

        public void SetData(long rid, long armyIndex)
        {
            this.jonRid = rid;
            this.armyIndex = armyIndex;
        }
    }

    public class SortHeroData
    {
        public HeroProxy.Hero hero;
        public float speedMulti;
        public int power;
        public int star;
        public int level;
        public int rare;
    }

    public class UI_Win_CreateArmyMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Win_CreateArmyMediator";

        #endregion

        //IMediatorPlug needs
        public UI_Win_CreateArmyMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }

        public UI_Win_CreateArmyView view;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private TroopProxy m_TroopProxy;
        private RssProxy m_RssProxy;
        private MonsterProxy m_MonsterProxy;
        private PlayerProxy m_PlayerProxy;
        private WorldMapObjectProxy m_worldProxy;
        private CityBuildingProxy m_CityBuildingProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private HeroProxy m_HeroProxy;
        private ExpeditionProxy m_expeditionProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;

        private IData m_HeroSaveData;
        private ITroopSave _mTroopSave;
        private int rssId = 0;
        private OpenPanelData m_OpenPanelData;


        private LanguageText tx;
        private bool m_btnClearSoldier = true;
        private bool m_isClickedClearSoldier = false;
        private bool m_expeditionInitFlag = false;
        private bool isSelectVice = false;
        private bool isSelectMain = false;
        private bool isOpenDingleChange = false;
        private bool isSelectHeroList = false;

        private HeroProxy.Hero m_selectMainHero;
        private int m_selectMainHeroId { get { return m_selectMainHero != null ? m_selectMainHero.config.ID : 0; } }
        private HeroProxy.Hero m_selectDeputyHero;
        private int m_selecttDeputyHeroId { get { return m_selectDeputyHero != null ? m_selectDeputyHero.config.ID : 0; } }
        private HeroProxy.Hero m_curSelectedHero = null;

        private UI_Item_CaptainHead_SubView m_selectItemCaptainHead;
        private TroopAttackType m_marchType; //行军类型
        private int m_troopsCapacity; //部队容量上限

        private HeroProxy.SortType m_sortType = HeroProxy.SortType.Recomend;

        private long m_resTotalWeight = 0; //资源总负载    
        private MapObjectInfoEntity m_rssData; //资源信息
        private float m_weightMulti; //负载加成

        private List<HeroProxy.Hero> m_availableHeros;
        private List<HeroProxy.Hero> m_availableSelectHeros;
        private List<SoldiersData> m_availableSoldiers= new List<SoldiersData>();
        private Dictionary<int, int> m_selectedSoldiers = new Dictionary<int, int>();
        private int MoveTimes; //行军时间
        private BagProxy m_bagProxy = null;
        private ConfigDefine m_ConfigDefine;
        private SelectMode m_curSelectMode = SelectMode.None;
        
        private List<SortHeroData> lsSortHeroDatas = new List<SortHeroData>();
        private List<SortHeroData> lsSortHeroDatasDeputy =new List<SortHeroData>();
        private List<SortHeroData> lsSortHeroByDeputy =new List<SortHeroData>();

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.OnRefreshTroopSave,
                CmdConstant.OnRefreshSaveIndexViewBule,
                CmdConstant.OnRefreshSaveIndexViewYellow,
                CmdConstant.OnRefreshSaveIndexViewRed,
                CmdConstant.OnRefreshSaveNumView,
                CmdConstant.OnAutoRefreshSaveIndexView,
                CmdConstant.FuncGuideMonsterPowerShow,
                CmdConstant.FuncGuideMonsterPowerHide,
                CmdConstant.UpdatePlayerActionPower,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnRefreshTroopSave:
                    OnRefreshTroopSaveView();
                    break;
                case CmdConstant.OnRefreshSaveIndexViewBule:
                    OnRefreshSaveIndexViewBule();
                    break;
                case CmdConstant.OnRefreshSaveIndexViewYellow:
                    OnRefreshSaveIndexViewYellow();
                    break;
                case CmdConstant.OnRefreshSaveIndexViewRed:
                    OnRefreshSaveIndexViewRed();
                    break;
                case CmdConstant.OnRefreshSaveNumView:
                    OnRefreshSaveNumView();
                    break;
                case CmdConstant.OnAutoRefreshSaveIndexView:
                    TroopSaveNumType type = (TroopSaveNumType)notification.Body;
                    AutoRefreshSaveIndexView(type);
                    break;
                case CmdConstant.FuncGuideMonsterPowerShow:
                    FuncGuideMonsterPowerShow();
                    break;
                case CmdConstant.FuncGuideMonsterPowerHide:
                    CoreUtils.uiManager.CloseUI(UI.s_fingerInfo);
                    if (m_funcGuideEffect != null)
                    {
                        CoreUtils.assetService.Destroy(m_funcGuideEffect);
                        m_funcGuideEffect = null;
                    }
                    break;
                case CmdConstant.UpdatePlayerActionPower:
                    RefreshMobility();
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
            _mTroopSave.RestSave();
            isOpenDingleChange = false;
            ClientUtils.DestroyAllChild(view.m_pl_DoubleSkillsMain_GridLayoutGroup.transform);
            ClientUtils.DestroyAllChild(view.m_pl_DoubleSkillsSub_GridLayoutGroup.transform);
            ClientUtils.DestroyAllChild(view.m_pl_SingleSkills_GridLayoutGroup.transform);
            cache_DoubleSkills.Clear();
            cache_DoubleSkillsSub.Clear();
            cache_SingleSkills.Clear();
        }

        public override void PrewarmComplete()
        {
        }

        private bool m_bCheckArr = false;

        public override void Update()
        {
            if (view.m_UI_Item_CaptainList.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.activeSelf)
            {
                if (Input.GetMouseButtonUp(0) && m_bCheckArr)
                {
                    view.m_UI_Item_CaptainList.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(false);
                }

                m_bCheckArr = true;
            }
        }

        protected override void InitData()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_CityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_RallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;

            IsOpenUpdate = true;
            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_armyList_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_list_View_save_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_UI_Item_CaptainList.m_sv_captainHead_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_list_SaveIndex_View_Blue_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_list_SaveIndex_View_Red_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_list_SaveIndex_View_Yellow_ListView.ItemPrefabDataList);
            view.m_img_NoSingleCaptain_PolygonImage.gameObject.SetActive(false);
            view.m_spin_Schar_SkeletonGraphic.gameObject.SetActive(false);
            view.m_lbl_SingleName_LanguageText.text = "";
            view.m_UI_Item_CaptainList.gameObject.SetActive(false);
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.Expedition:
                    view.m_UI_Model_DoubleLineButton_Yellow.m_pl_line2_HorizontalLayoutGroup.gameObject.SetActive(false);
                    view.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(805047);
                    break;
                case GameModeType.World:
                    ClientUtils.LoadSprite(view.m_UI_Model_DoubleLineButton_Yellow.m_img_icon2_PolygonImage, RS.CountDownIcon);
                    break;
            }


            view.gameObject.SetActive(false);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, (assetDic) =>
            {
                m_assetDic = assetDic;
                AssetLoadFinish();
            });
            tx = view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.transform.Find("lbl_Text")
                .GetComponent<LanguageText>();
            m_ConfigDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_createAnmy);
            });
            view.m_ck_sll_GameToggle.onValueChanged.AddListener(
                (isOn) =>
                {
                    if (isOn)
                    {
                        _mTroopSave.SetSaveType(TroopSaveType.Read);
                        if (!isSaveShowPanel)
                        {
                            MoveUIPos(true, 2);
                            MoveUIPos(true);
                            isSelectVice = false;
                            SetCapainPartLineTxt(true);
                        }

                        OnRefreshSaveData();
                    }
                    else
                    {
                        if (!view.m_pl_Save_ToggleGroup.AnyTogglesOn())
                        {
                            MoveUIPos();
                            _mTroopSave.UpdateAllSave(false);
                        }
                    }
                });
            view.m_ck_sls_GameToggle.onValueChanged.AddListener(
                (isOn) =>
                {
                    if (isOn)
                    {
                        _mTroopSave.SetSaveType(TroopSaveType.Save);

                        if (!isSaveShowPanel)
                        {
                            MoveUIPos(true, 2);
                            MoveUIPos(true);
                        }

                        OnRefreshSaveData();
                    }
                    else
                    {
                        if (!view.m_pl_Save_ToggleGroup.AnyTogglesOn())
                        {
                            MoveUIPos();
                            _mTroopSave.UpdateAllSave(false);
                        }
                    }
                }
            );

            view.m_btn_back_GameButton.onClick.AddListener(() =>
            {
                MoveUIPos();
                _mTroopSave.UpdateAllSave(false);
            });

            view.m_btn_DingleChange1_GameButton.onClick.AddListener(() =>
            {
                RestToggleIsOn();
                if (!isOpenDingleChange)
                {   
                    MoveUIPos();
                    if (isSelectHeroList)
                    {
                        MoveUIPos(true, 2);
                    }
                    else
                    {
                        MoveUIPos(false, 2); 
                    }
                }

                isSelectVice = false;
                isSelectMain = true;
                HeroSelectModeChange(SelectMode.MainHero);
                isOpenDingleChange = true;
                SetCapainPartLineTxt(true);
               
            });

            view.m_btn_DingleChange2_GameButton.onClick.AddListener(() =>
            {
                RestToggleIsOn();
                if (!isOpenDingleChange)
                {
                    MoveUIPos();
                    MoveUIPos(false, 2);
                }

                isSelectVice = true;
                isSelectMain = false;
                HeroSelectModeChange(SelectMode.DeputyHero);
                isOpenDingleChange = true;
                SetCapainPartLineTxt(false);
                view.m_ck_sll_GameToggle.isOn = false;

            });

            view.m_btn_plus2_GameButton.onClick.AddListener(() =>
            {
                RestToggleIsOn();
                if (!isOpenDingleChange)
                {
                    MoveUIPos();
                    MoveUIPos(false, 2);
                }

                isSelectVice = true;
                isSelectMain = false;
                HeroSelectModeChange(SelectMode.DeputyHero);
                isOpenDingleChange = true;
                SetCapainPartLineTxt(false);

            });
            
            view.m_btn_delete2_GameButton.onClick.AddListener(() =>
            {
                RestToggleIsOn();
                isSelectVice = false;
                m_selectDeputyHero = null;
                InitViewData();
                HeroSelectModeChange(SelectMode.None);
                isOpenDingleChange = false;
                SetCapainPartLineTxt(false);
                
            });

            view.m_btn_plus_SingleChar_GameButton.onClick.AddListener(OnChangeHero);
            view.m_btn_SingleChange_GameButton.onClick.AddListener(OnChangeHero);
            view.m_btn_plus1_GameButton.onClick.AddListener(OnChangeHero);
            //view.m_btn_plus2_GameButton.onClick.AddListener(OnChangeHero);
            view.m_btn_Question_GameButton.onClick.AddListener(OnQuestion);

            view.m_UI_Item_CaptainList.m_btn_arr_GameButton.onClick.AddListener(() =>
            {
                view.m_UI_Item_CaptainList.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_star.SetSelected(
                    m_sortType == HeroProxy.SortType.Star);
                view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_power.SetSelected(
                    m_sortType == HeroProxy.SortType.Power);
                view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_level.SetSelected(
                    m_sortType == HeroProxy.SortType.Level);
                view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_Quality.SetSelected(
                    m_sortType == HeroProxy.SortType.Rare);
                view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_Recomend.SetSelected(
                    m_sortType == HeroProxy.SortType.Recomend);
                m_bCheckArr = false;
            });
            view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_star.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Star);
            });
            view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_level.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Level);
            });
            view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_power.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Power);
            });
            view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_Quality.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Rare);
            });
            
            view.m_UI_Item_CaptainList.m_UI_Item_CaptionArrType_Recomend.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Recomend);
            });


            view.m_btn_set_GameButton.onClick.AddListener(() =>
            {
                bool isDelete = _mTroopSave.GetIsDelete();
                if (isDelete)
                {
                    isDelete = false;
                }
                else
                {
                    isDelete = true;
                }

                _mTroopSave.UpdateAllSave(isDelete);
            });

            view.m_btn_refresh_GameButton.onClick.AddListener(() => { PlayAutoRefreshSaveIndexView(); });       
            
            view.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                string strDesc = LanguageUtils.getText(200115);
                HelpTip.CreateTip(strDesc, view.m_btn_noTextButton_GameButton.transform).SetAutoFilter().SetWidth(200).Show();
            });
        }

        private void RestToggleIsOn()
        {
            view.m_ck_sll_GameToggle.isOn = false;
            view.m_ck_sls_GameToggle.isOn = false;
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_DoubleLineButton_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(OnMarch);
        }

        #endregion

        private GameObject m_funcGuideEffect;
        private void FuncGuideMonsterPowerShow()
        {
            CoreUtils.assetService.Instantiate("UI_10009", (obj)=> {
                if (view.gameObject == null)
                {
                    return;
                }
                obj.transform.SetParent(view.m_lbl_compare_LanguageText.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = new Vector2(0, 0);
                obj.gameObject.SetActive(true);
                m_funcGuideEffect = obj;

                FingerTargetParam param = new FingerTargetParam();
                param.AreaTarget = view.m_lbl_compare_LanguageText.gameObject;
                param.ArrowDirection = (int)EnumArrorDirection.Up;
                param.IsAutoClose = false;
                param.IsUseRect = true;
                param.IsClickClose = false;
                param.AreaRect = new Vector2(view.m_lbl_compare_LanguageText.preferredWidth, view.m_lbl_compare_LanguageText.preferredHeight);
                CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
            });
        }

        private void HeroSortRefresh(HeroProxy.SortType type)
        {
            SortHeroByType(type);
            RefreshHeroTitleSortText(type);

            view.m_UI_Item_CaptainList.m_sv_captainHead_ListView.FillContent(Mathf.CeilToInt(m_availableSelectHeros.Count * 1.0f / 2));
        }

        private void RefreshHeroTitleSortText(HeroProxy.SortType type)
        {
            switch (type)
            {
                case HeroProxy.SortType.Rare:
                    view.m_UI_Item_CaptainList.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200026);
                    break;
                case HeroProxy.SortType.Star:
                    view.m_UI_Item_CaptainList.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200027);
                    break;
                case HeroProxy.SortType.Level:
                    view.m_UI_Item_CaptainList.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200028);
                    break;
                case HeroProxy.SortType.Power:
                    view.m_UI_Item_CaptainList.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(300005);
                    break;
                case  HeroProxy.SortType.Recomend:
                    view.m_UI_Item_CaptainList.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(100005);
                    break;
            }
        }

        //部队数量帮助
        private void OnQuestion()
        {
            if(m_OpenPanelData.ExpeditionTroopIndex != 0)
            {
                HelpTip.CreateTip(6000, view.m_btn_Question_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(30).Show();
                return;
            }

            HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(3001);
            if (tipDefine == null)
            {
                return;
            }

            var playerAttributeProxy =
                AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            var troopCapHero = playerAttributeProxy.GetHeroAttribute(m_selectMainHeroId, Data.attrType.troopsCapacity).origvalue;
            var troopCapBuild = playerAttributeProxy.GetCityAttribute(Data.attrType.troopsCapacity).origvalue;

            long baseCap = troopCapBuild + troopCapHero;
            var troopCapHeroSkillSource = playerAttributeProxy.GetHeroSourceAttribute(m_selectMainHeroId, Data.attrType.troopsCapacityMulti, EnumSourceAttr.HeroSkill);
            var troopCapDeputyHeroSkillSource = playerAttributeProxy.GetHeroSourceAttribute(m_selecttDeputyHeroId, Data.attrType.troopsCapacityMulti, EnumSourceAttr.HeroSkill);

            var troopCapHeroTalentSource = playerAttributeProxy.GetHeroSourceAttribute(m_selectMainHeroId, Data.attrType.troopsCapacityMulti, EnumSourceAttr.HeroTalent);
            string troopCapHeroSkillDesc = "0";
            if (troopCapHeroSkillSource.value > 0 || troopCapDeputyHeroSkillSource.value > 0)
            {
                float fValue = (troopCapHeroSkillSource.value + troopCapDeputyHeroSkillSource.value);
                long troopCapHeroSkill = Mathf.FloorToInt(baseCap * fValue);
                troopCapHeroSkillDesc = LanguageUtils.getTextFormat(300251, ClientUtils.FormatComma(troopCapHeroSkill), (fValue * 100).ToString("0.##"));
            }
            string troopCapHeroTalentDesc = "0";
            if (troopCapHeroTalentSource.value > 0)
            {
                long troopCapHeroTalent = Mathf.FloorToInt(baseCap * troopCapHeroTalentSource.value);
                troopCapHeroTalentDesc = LanguageUtils.getTextFormat(300251, ClientUtils.FormatComma(troopCapHeroTalent), troopCapHeroTalentSource.GetShowValue());
            }

            string str = LanguageUtils.getTextFormat(tipDefine.l_data1,
                                                     ClientUtils.FormatComma(m_troopsCapacity),
                                                     m_PlayerProxy.CurrentRoleInfo.level, ClientUtils.FormatComma(troopCapBuild),
                                                     ClientUtils.FormatComma(troopCapHero),
                                                     troopCapHeroSkillDesc,
                                                     troopCapHeroTalentDesc);
            HelpTip.CreateTip(str, view.m_btn_Question_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown)
                .SetOffset(30).Show();
        }

        private void OnChangeHero()
        {
            MoveUIPos();
            if (isSelectHeroList)
            {
                MoveUIPos(true, 2);
            }
            else
            {
                MoveUIPos(false, 2); 
            }
            isSelectVice = false;
            isSelectMain = true;
            HeroSelectModeChange(SelectMode.MainHero);
            SetCapainPartLineTxt(true);
        }

        //计算行军距离
        private float CalDistance()
        {
            //如果是集结部队并且在视野范围外，需要扣除集结部队半径，目前需求还未明确

            float distance = 0f;

            Vector2 startPosV2 = new Vector2(m_CityBuildingProxy.RolePos.x, m_CityBuildingProxy.RolePos.y);
            startPosV2.Set(startPosV2.x * 100.0f, startPosV2.y * 100.0f);

            Vector3 endPosV3;
            if (!TroopHelp.GetRssPos(rssId, out endPosV3))
            {
                if (m_OpenPanelData.type == OpenPanelType.JoinRally)
                {
                    endPosV3 = new Vector3(m_OpenPanelData.pos.x, 0, m_OpenPanelData.pos.y);
                }
                else
                {
                    Vector2 moveSpacePos = m_TroopProxy.GetTroopMoveSpacePos();
                    endPosV3 = new Vector3(moveSpacePos.x, 0, moveSpacePos.y);
                }
            }

            Vector2 endPosV2 = new Vector2(endPosV3.x, endPosV3.z);
            endPosV2.Set(endPosV2.x * 100.0f, endPosV2.y * 100.0f);

            distance = TroopHelp.GetDistance(startPosV2, endPosV2);

            //扣除出发城市半径
            ConfigDefine configDefineToC = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (configDefineToC != null)
            {
                distance = distance - configDefineToC.cityRadius * 100.0f;
            }

            //扣除目标点半径
            distance = distance - TroopHelp.GetRssRadius(rssId) * 100.0f;

            //部队半径
            float armyRadius = TroopHelp.GetArmyRadius(m_selectedSoldiers);

            //扣除部队半径（出发点） 
            distance = distance - armyRadius * 100.0f;

            //扣除部队半径（目标点） 
            if (rssId != 0)
            {
                distance = distance - armyRadius * 100.0f;
            }

            return Math.Max(0, distance);
        }

        //初始化资源数据
        private void InitResData()
        {
            if (m_marchType == TroopAttackType.Collect)
            {

                if (m_rssData != null)
                {
                    if (m_rssData.resourceGatherTypeDefine != null)
                    {
                        m_resTotalWeight = m_rssData.resourceAmount *
                                           m_RssProxy.GetResWeight(m_rssData.resourceGatherTypeDefine.type);
                    }
                }
            }
        }

        private void UpdateResWeightMulti()
        {
            if (m_marchType == TroopAttackType.Collect)
            {
                m_weightMulti = m_TroopProxy.GetTroopsSpaceMulti(m_selectMainHeroId, m_selecttDeputyHeroId);
            }
        }

        private void AssetLoadFinish()
        {
            InitViceSkillGo();
            m_OpenPanelData = view.data as OpenPanelData;
            if (m_OpenPanelData != null) rssId = m_OpenPanelData.id;
            m_marchType =  m_TroopProxy.GetTroopAttackType(rssId);
            m_rssData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
           
            SetInitData();

            #region list

            Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
            prefabNameList["UI_Item_ArmyCount"] = m_assetDic["UI_Item_ArmyCount"];
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListItemByIndex;
            view.m_sv_armyList_ListView.SetInitData(prefabNameList, funcTab);
            int count = m_availableSoldiers?.Count ?? 0;
            view.m_sv_armyList_ListView.FillContent(count);
            view.m_lbl_no_soldier_LanguageText.gameObject.SetActive(count == 0);

            Dictionary<string, GameObject> prefabNameList1 = new Dictionary<string, GameObject>();
            prefabNameList1["UI_Item_TroopsSaveItem"] = m_assetDic["UI_Item_TroopsSaveItem"];
            ListView.FuncTab funcTab1 = new ListView.FuncTab();
            funcTab1.ItemEnter = ListHeroSaveItemByIndex;
            view.m_list_View_save_ListView.SetInitData(prefabNameList1, funcTab1);
            view.m_list_View_save_ListView.FillContent(TroopSave.SaveNum);

            Dictionary<string, GameObject> prefabNameList2 = new Dictionary<string, GameObject>();
            prefabNameList2["UI_LC_Captain"] = m_assetDic["UI_LC_Captain"];
            ListView.FuncTab funcTab2 = new ListView.FuncTab();
            funcTab2.ItemEnter = ListHeroItemByIndex;
            view.m_UI_Item_CaptainList.m_sv_captainHead_ListView.SetInitData(prefabNameList2, funcTab2);

            Dictionary<string, GameObject> prefabNameList3 = new Dictionary<string, GameObject>();
            prefabNameList3["UI_Item_Save_Index"] = m_assetDic["UI_Item_Save_Index"];
            ListView.FuncTab funcTab3 = new ListView.FuncTab();
            funcTab3.ItemEnter = ListSaveItemByIndexByBlue;
            view.m_list_SaveIndex_View_Blue_ListView.SetInitData(prefabNameList3, funcTab3);
            int num = _mTroopSave.GetLsSaveCount(TroopSaveNumType.Blue);
            view.m_list_SaveIndex_View_Blue_ListView.FillContent(num);


            Dictionary<string, GameObject> prefabNameList4 = new Dictionary<string, GameObject>();
            prefabNameList4["UI_Item_Save_Index"] = m_assetDic["UI_Item_Save_Index"];
            ListView.FuncTab funcTab4 = new ListView.FuncTab();
            funcTab4.ItemEnter = ListSaveItemByIndexByYellow;
            view.m_list_SaveIndex_View_Yellow_ListView.SetInitData(prefabNameList4, funcTab4);
            int num4 = _mTroopSave.GetLsSaveCount(TroopSaveNumType.Yellow);
            view.m_list_SaveIndex_View_Yellow_ListView.FillContent(num4);

            Dictionary<string, GameObject> prefabNameList5 = new Dictionary<string, GameObject>();
            prefabNameList5["UI_Item_Save_Index"] = m_assetDic["UI_Item_Save_Index"];
            ListView.FuncTab funcTab5 = new ListView.FuncTab();
            funcTab5.ItemEnter = ListSaveItemByIndexByRed;
            view.m_list_SaveIndex_View_Red_ListView.SetInitData(prefabNameList5, funcTab5);
            int num5 = _mTroopSave.GetLsSaveCount(TroopSaveNumType.Red);
            view.m_list_SaveIndex_View_Red_ListView.FillContent(num5);

            #endregion

            //选择主将
            SetCapainPartLineTxt(true);
            view.gameObject.SetActive(true);
            m_HeroSaveData.Init();
            InitSortIndex();
        }

        private void SetCapainPartLineTxt(bool state)
        {
            if (state)
            {
                view.m_UI_Item_CaptainList.m_UI_Item_CaptainPartline.m_lbl_text_LanguageText.text =
                    LanguageUtils.getText(200015);
                m_curSelectedHero = m_selectMainHero;
                view.m_UI_Item_CaptainList.m_sv_captainHead_ListView.ForceRefresh();
            }
            else
            {
                view.m_UI_Item_CaptainList.m_UI_Item_CaptainPartline.m_lbl_text_LanguageText.text =
                    LanguageUtils.getText(200016);
                m_curSelectedHero = m_selectDeputyHero;
                view.m_UI_Item_CaptainList.m_sv_captainHead_ListView.ForceRefresh();
            }
        }


        private void SetInitData()
        {
            InitResData();
            InitHeroData();
            InitTroopData();
            CalTroopsCapacity(m_selectMainHeroId, m_selecttDeputyHeroId);
            UpdateResWeightMulti();
            //设置默认选中的士兵数据
            SetDefaultSelectData();
            Init(false);
            OnRefreshSaveNumView();
        }

        #region 按目标英雄列表重新排序+默认选择主副英雄
     
        private void SortHero()
        {
            lsSortHeroDatas.Clear();
            MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            float speedMulti = 0;
            bool isPvP = false;
            foreach (var info in m_availableHeros)
            {
                var attributes = m_playerAttributeProxy.GetTroopAttribute(info.data.heroId,m_selecttDeputyHeroId);
                SortHeroData sortHeroData =new SortHeroData();
                sortHeroData.hero = info;

                if (infoEntity != null)
                {
                    if (m_marchType == TroopAttackType.Collect)
                    {
                        switch (infoEntity.rssType)
                        {
                            case RssType.Farmland:
                            case RssType.GuildFoodResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getFoodSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Wood:
                            case RssType.GuildWoodResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getWoodSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Stone:
                            case RssType.GuildGoldResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getStoneSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Gold:
                            case RssType.GuildGemResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getGlodSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Gem:
                                sortHeroData.speedMulti = attributes[(int) attrType.getDiamondSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Rune:
                                sortHeroData.speedMulti = attributes[(int) attrType.troopsCapacityMulti - 1].perfValue;
                                break;
                        }
                    }
                    else if (m_marchType == TroopAttackType.Attack)
                    {
                        switch (infoEntity.rssType)
                        {
                            case RssType.Monster:
                            case RssType.Guardian:
                            case RssType.SummonAttackMonster:
                            case RssType.SummonConcentrateMonster:
                            case RssType.BarbarianCitadel:
                                sortHeroData.speedMulti = attributes[(int) attrType.heroExpMulti - 1].perfValue;
                                break;
                            case RssType.Troop:
                            case RssType.City:
                                sortHeroData.speedMulti = attributes[(int) attrType.troopsCapacityMulti - 1].perfValue;
                                isPvP = true;
                                break;
                            default:
                                sortHeroData.speedMulti = attributes[(int) attrType.troopsCapacityMulti - 1].perfValue;
                                isPvP = true;
                                break;
                        }
                    }
                }
                else if (m_marchType == TroopAttackType.Space)
                {
                    sortHeroData.speedMulti = attributes[(int) attrType.troopsCapacityMulti - 1].perfValue;
                    isPvP = true;
                }

                sortHeroData.power = info.power;
                sortHeroData.level = info.level;
                sortHeroData.star = info.star;
                sortHeroData.rare = info.config.rare;

                lsSortHeroDatas.Add(sortHeroData);
            }

            if (isPvP)
            {
                lsSortHeroDatas.Sort(delegate(SortHeroData data, SortHeroData heroData)
                {
                    if (data.power > heroData.power)
                    {
                        return -1;
                    }

                    if (data.power < heroData.power)
                    {
                        return 1;
                    }
                    
                    if (data.rare > heroData.rare)
                    {
                        return -1;
                    }

                    if (data.rare < heroData.rare)
                    {
                        return 1;
                    }
                    
                    if (data.level > heroData.level)
                    {
                        return -1;
                    }

                    if (data.level < heroData.level)
                    {
                        return 1;
                    }
                    
                    if (data.speedMulti > heroData.speedMulti)
                    {
                        return -1;
                    }

                    if (data.speedMulti < heroData.speedMulti)
                    {
                        return 1;
                    }
                    return 0;
                });
            }
            else
            {
                lsSortHeroDatas.Sort(delegate(SortHeroData data, SortHeroData heroData)
                {
                    if (data.speedMulti > heroData.speedMulti)
                    {
                        return -1;
                    }

                    if (data.speedMulti < heroData.speedMulti)
                    {
                        return 1;
                    }
                
                    if (data.power > heroData.power)
                    {
                        return -1;
                    }

                    if (data.power < heroData.power)
                    {
                        return 1;
                    }

                    return 0;
                });
            }




            m_availableHeros.Clear();
            lsSortHeroDatasDeputy.Clear();
            foreach (var info in lsSortHeroDatas)
            {
                m_availableHeros.Add(info.hero);
                lsSortHeroDatasDeputy.Add(info);
            }

            if (lsSortHeroDatasDeputy.Count > 0)
            {
                lsSortHeroDatasDeputy.RemoveAt(0); 
            }
            SortDeputyHero();
        }


        private void SortDeputyHero()
        {
            lsSortHeroByDeputy.Clear();
            MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            bool isPvP = false;
            foreach (var info in lsSortHeroDatasDeputy)
            {
                var attributes = m_playerAttributeProxy.GetAttributebuteBySkill(info.hero.data.heroId);
                SortHeroData sortHeroData =new SortHeroData();
                sortHeroData.hero = info.hero;
                if (infoEntity != null)
                {
                    if (m_marchType == TroopAttackType.Collect)
                    {
                        switch (infoEntity.rssType)
                        {
                            case RssType.Farmland:
                            case RssType.GuildFoodResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getFoodSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Wood:
                            case RssType.GuildWoodResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getWoodSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Stone:
                            case RssType.GuildGoldResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getStoneSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Gold:
                            case RssType.GuildGemResCenter:
                                sortHeroData.speedMulti = attributes[(int) attrType.getGlodSpeedMulti - 1].perfValue;
                                break;
                            case RssType.Gem:
                                sortHeroData.speedMulti = attributes[(int) attrType.getDiamondSpeedMulti - 1].perfValue;
                                break;
                        }
                                
                    }else if (m_marchType == TroopAttackType.Attack)
                    {                     
                        switch (infoEntity.rssType)
                        {                   
                            case RssType.Monster:
                            case RssType.Guardian:
                            case RssType.SummonAttackMonster:
                            case RssType.SummonConcentrateMonster:
                                sortHeroData.speedMulti=attributes[(int) attrType.heroExpMulti - 1].perfValue;                           
                                break;
                            case RssType.Troop:
                            case RssType.City:
                                sortHeroData.speedMulti=attributes[(int) attrType.troopsCapacityMulti - 1].perfValue;
                                isPvP = true;
                                break;  
                            default:
                                sortHeroData.speedMulti=attributes[(int) attrType.troopsCapacityMulti - 1].perfValue;
                                isPvP = true;
                                break;
                        }
                                
                    }
                }
                else if (m_marchType == TroopAttackType.Space)
                {
                    sortHeroData.speedMulti=attributes[(int) attrType.troopsCapacityMulti - 1].perfValue;
                    isPvP = true;
                }
                sortHeroData.power = info.power;
                sortHeroData.level = info.level;
                sortHeroData.star = info.star;
                sortHeroData.rare = info.rare;
                lsSortHeroByDeputy.Add(sortHeroData);
            }

            if (isPvP)
            {
                lsSortHeroByDeputy.Sort(delegate(SortHeroData data, SortHeroData heroData)
                {
                    if (data.power > heroData.power)
                    {
                        return -1;
                    }

                    if (data.power < heroData.power)
                    {
                        return 1;
                    }
                    
                    if (data.rare > heroData.rare)
                    {
                        return -1;
                    }

                    if (data.rare < heroData.rare)
                    {
                        return 1;
                    }
                    
                    if (data.level > heroData.level)
                    {
                        return -1;
                    }

                    if (data.level < heroData.level)
                    {
                        return 1;
                    }
                    
                    if (data.speedMulti > heroData.speedMulti)
                    {
                        return -1;
                    }

                    if (data.speedMulti < heroData.speedMulti)
                    {
                        return 1;
                    }
                    return 0;
                });
            }
            else
            {
                lsSortHeroByDeputy.Sort(delegate(SortHeroData data, SortHeroData heroData)
                {
                    if (data.speedMulti > heroData.speedMulti)
                    {
                        return -1;
                    }

                    if (data.speedMulti < heroData.speedMulti)
                    {
                        return 1;
                    }
                    return 0;
                });
            }

      

            if (lsSortHeroByDeputy.Count > 0)
            {             
                m_selectDeputyHero = lsSortHeroByDeputy[0].hero;
               // HeroSelectModeChange(SelectMode.DeputyHero);
               isSelectMain = false;
               isSelectVice = true;
            }
        }

        #endregion


        private void InitHeroData()
        {
            m_availableHeros = AvailableHelp.GetAvailableHero(m_OpenPanelData.ExpeditionTroopIndex);

            //新手引导特殊处理
            if (GuideManager.Instance.IsGuideFightBarbarian)
            {
                m_selectMainHero = null;
                m_selectDeputyHero = null;
                m_sortType = HeroProxy.SortType.Level;
            }
            else
            {

                if (m_availableHeros.Count > 0)
                {
                    SortHero();
                    m_selectMainHero = m_availableHeros[0];
                    if (m_selectMainHero.data.star < 3)
                    {
                        m_selectDeputyHero = null;
                    }
                }
                if (m_OpenPanelData.ExpeditionTroopIndex != 0)
                {
                    var troopData = m_expeditionProxy.GetPlayerTroopData(m_OpenPanelData.ExpeditionTroopIndex);
                    if(troopData != null)
                    {
                        m_selectMainHero = m_HeroProxy.GetHeroByID(troopData.MainHeroId);
                        m_selectDeputyHero = m_HeroProxy.GetHeroByID(troopData.DeputyHeroId);
                    }
                }

            }

            m_HeroSaveData = m_TroopProxy.CreateIDataFactory(IDataType.HeroSave);
            _mTroopSave = m_HeroSaveData as ITroopSave;


            if (_mTroopSave!=null)
                SetPanelActive(TroopSaveNumType.Blue);
            int total = m_availableHeros.Count;
            //英雄拥有的数量
            view.m_UI_Item_CaptainList.m_lbl_count_LanguageText.text = total.ToString();
        }

        private void InitTroopData()
        {
            m_availableSoldiers = GetAvailableSoldier();
            SoldierProxy.SortSoldierDataByAttackType(m_availableSoldiers, m_marchType);
        }

        //英雄数据排序
        private void SortHeroByType(HeroProxy.SortType type)
        {
            m_sortType = type;
            if (type == HeroProxy.SortType.Recomend)
            {
                m_availableSelectHeros.Clear();
                foreach (var info in lsSortHeroDatas)
                {
                    m_availableSelectHeros.Add(info.hero);
                }
            }
            else
            {
                m_HeroProxy.SortHeros(m_availableSelectHeros, type);
            }

        }

        // 设置默认选中的士兵数据
        private void SetDefaultSelectData()
        {
            m_selectedSoldiers.Clear();
            int soldierCount = 0;
            if (m_marchType == TroopAttackType.Collect && m_resTotalWeight > 0) //采集
            {
                long leftWeight = m_resTotalWeight;
                foreach (var data in m_availableSoldiers)
                {
                    float multiCapacity = data.ArmysCfg.capacity * m_weightMulti;
                    int num = Mathf.CeilToInt(leftWeight * 1.0f / multiCapacity);
                    num = Mathf.Min(num, data.Number);
                    bool isTroopFull = false;
                    if(soldierCount + num > m_troopsCapacity)
                    {
                        num = m_troopsCapacity - soldierCount;
                        isTroopFull = true;
                    }
                    m_selectedSoldiers[data.Id] = num;
                    if (isTroopFull) break;
                    soldierCount += num;
                    leftWeight -= (long)Mathf.FloorToInt(num * multiCapacity);
                }
            }
            else
            {

                m_availableSoldiers.Sort(SortSoldiersByLevel);
                foreach (var data in m_availableSoldiers)
                {
                    int num = data.Number;
                    bool isTroopFull = false;
                    if (soldierCount + num > m_troopsCapacity)
                    {
                        num = m_troopsCapacity - soldierCount;
                        isTroopFull = true;
                    }
                    m_selectedSoldiers[data.Id] = num;
                    if (isTroopFull) break;
                    soldierCount += num;
                }
                if (m_OpenPanelData.ExpeditionTroopIndex != 0 && !m_isClickedClearSoldier && !m_expeditionInitFlag)
                {
                    var troopData = m_expeditionProxy.GetPlayerTroopData(m_OpenPanelData.ExpeditionTroopIndex);
                    if (troopData != null)
                    {
                        m_selectedSoldiers.Clear();
                        foreach(var kv in troopData.Soldiers)
                        {
                            m_selectedSoldiers[(int)kv.Value.id] = (int)kv.Value.num;
                        }
                    }

                    m_expeditionInitFlag = true;
                }
            }
        }

        private int SortSoldiersByLevel(SoldiersData data1, SoldiersData data2)
        {
            if (data2.ArmysCfg.armsLv > data1.ArmysCfg.armsLv)
            {
                return 1;
            }
            
            if (data2.ArmysCfg.armsLv < data1.ArmysCfg.armsLv)
            {
                return -1;
            }

            if (data2.ArmysCfg.armsType > data1.ArmysCfg.armsType)
            {
                return -1;
            }
            
            if (data2.ArmysCfg.armsType < data1.ArmysCfg.armsType)
            {
                return 1;
            }
            return 0;
        }

        private void Init(bool isRefreshSoldierList = true)
        {
            if (m_selectMainHero != null)
            {
                if (m_marchType == TroopAttackType.Attack) //打怪
                {
                    MapObjectInfoEntity monsterData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);

                    if (monsterData != null &&
                        (monsterData.rssType == RssType.Monster ||
                        monsterData.rssType == RssType.SummonAttackMonster ||
                        monsterData.rssType == RssType.SummonConcentrateMonster ||
                        monsterData.rssType == RssType.BarbarianCitadel))
                    {
                        view.m_lbl_armyWeight_LanguageText.text = "";
                        view.m_icon_mobility_PolygonImage.gameObject.SetActive(true); //行动力显示
                        view.m_lbl_compare_LanguageText.gameObject.SetActive(false);
                        //行动力减免
                        RefreshMobility();
                    }     
                    else
                    {
                        view.m_icon_mobility_PolygonImage.gameObject.SetActive(false);
                        view.m_lbl_armyWeight_LanguageText.text = LanguageUtils.getTextFormat(200047,
                            ClientUtils.FormatComma(m_TroopProxy.GetArmyWeight(m_selectedSoldiers,m_selectMainHero.data.heroId,m_selecttDeputyHeroId,m_rssData.rssType)));
                        view.m_lbl_compare_LanguageText.gameObject.SetActive(false);
                    }
                }
                else //采集
                {
                    view.m_icon_mobility_PolygonImage.gameObject.SetActive(false);
                    if (m_rssData != null)
                    {
                        view.m_lbl_armyWeight_LanguageText.text = LanguageUtils.getTextFormat(200047,
                            ClientUtils.FormatComma(m_TroopProxy.GetArmyWeight(m_selectedSoldiers, m_selectMainHeroId,
                                m_selecttDeputyHeroId, m_rssData.rssType)));                      
                    }
                    view.m_lbl_compare_LanguageText.gameObject.SetActive(false);
                }

                if (isSelectMain)
                {
                    if (m_selectMainHero != null)
                    {                   
                        CoreUtils.audioService.PlayOneShot(m_selectMainHero.config.voiceSelect);                
                    }

                }else if (isSelectVice)
                {
                    if (m_selectDeputyHero != null)
                    {                   
                        CoreUtils.audioService.PlayOneShot(m_selectDeputyHero.config.voiceSelect);
                    }                   
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_DoubleLineButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.GetComponent<RectTransform>());


                m_btnClearSoldier = false;
                UpdateSoldiersData();
                view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    UpdateSoldiersData();
                    
                });

                view.m_UI_Model_MiniButton_White.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                view.m_UI_Model_MiniButton_White.m_btn_btn_GameButton.onClick.AddListener(OnMore);

                view.m_UI_Item_ArmyData1.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                view.m_UI_Item_ArmyData1.m_btn_btn_GameButton.onClick.AddListener(() => { ClickMulti(1); });
                view.m_UI_Item_ArmyData2.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                view.m_UI_Item_ArmyData2.m_btn_btn_GameButton.onClick.AddListener(() => { ClickMulti(2); });
                RefreshHeroMultiAttr();

                if (isRefreshSoldierList)
                {
                    RefreshSoldiersList();
                }

                OnRefreshSoldiersNum();
                InitSkillView();
                InitSpinAnm();
            }
            else //尚未选择主帅
            {
                view.m_lbl_powerTotal_LanguageText.text = LanguageUtils.getTextFormat(200049, 0);
                view.m_img_NoSingleCaptain_PolygonImage.gameObject.SetActive(true);

                if (m_marchType == TroopAttackType.Attack) //打怪
                {
                    view.m_lbl_armyWeight_LanguageText.text = "";
                    view.m_lbl_compare_LanguageText.gameObject.SetActive(false);
                    //行动力减免
                    view.m_icon_mobility_PolygonImage.gameObject.SetActive(true); //行动力显示
                    RefreshMobility();
                }
                else //采集
                {
                    view.m_lbl_compare_LanguageText.gameObject.SetActive(false);
                    view.m_lbl_armyWeight_LanguageText.text = LanguageUtils.getTextFormat(200047,
                        ClientUtils.FormatComma(m_TroopProxy.GetArmyWeight(m_selectedSoldiers,m_selectMainHeroId,m_selecttDeputyHeroId,m_rssData.rssType)));
                }

                RefreshHeroMultiAttr();

                //总战力
                view.m_lbl_powerCap_LanguageText.text = LanguageUtils.getTextFormat(200050,
                    ClientUtils.FormatComma(TroopProxy.GetFightingCount(m_selectedSoldiers)));
                //部队数量
                string str = LanguageUtils.getTextFormat(181104,
                    ClientUtils.FormatComma(GetSelectedSoldierCount()),
                    ClientUtils.FormatComma(m_troopsCapacity));
                view.m_lbl_armyCapa_LanguageText.text = LanguageUtils.getTextFormat(200046, str);
            }
        }

        private int GetSelectedSoldierCount()
        {
            int count = 0;
            foreach(var kv in m_selectedSoldiers)
            {
                count += kv.Value;
            }
            return count;
        }

        //刷新行动力
        private void RefreshMobility()
        {
            if (GuideManager.Instance.IsGuideFightBarbarian)
            {
                view.m_lbl_mobility_LanguageText.text = LanguageUtils.getTextFormat(200048, 50);
                return;
            }

            int ap = GetCostMobility();
            if (m_PlayerProxy.CurrentRoleInfo.actionForce < ap)
            {
                view.m_lbl_mobility_LanguageText.text = LanguageUtils.getTextFormat(200070, ap);
            }
            else
            {
                view.m_lbl_mobility_LanguageText.text = LanguageUtils.getTextFormat(200048, ap);
            }
        }

        private int GetCostMobility()
        {
            int heroAp = 0;
            if (m_selectMainHero != null)
            {
                var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
                heroAp = (int)playerAttributeProxy.GetHeroAttribute(m_selectMainHeroId, attrType.vitalityReduction).value;
            }

            MapObjectInfoEntity monsterData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            if (monsterData != null && monsterData.monsterDefine != null)
            {
                //行动力减免
                int ap = 0;
                if (monsterData.rssType == RssType.Monster ||
                    monsterData.rssType == RssType.SummonAttackMonster)
                {
                    ap = Mathf.Max(0, monsterData.monsterDefine.costAP - heroAp);
                }
                else if (monsterData.rssType == RssType.BarbarianCitadel ||
                        monsterData.rssType == RssType.SummonConcentrateMonster)
                {
                    ap = Mathf.Max(0, monsterData.monsterDefine.rallyAP - heroAp);
                }

                return ap;
            }
            else if (m_OpenPanelData != null && m_OpenPanelData.targetMonsterId != 0)
            {
                MonsterDefine monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)m_OpenPanelData.targetMonsterId);
                if (monsterDefine != null)
                {
                    //行动力减免
                    int ap = 0;
                    //野蛮人
                    if (monsterDefine.type == 1)
                    {
                        ap = Mathf.Max(0, monsterDefine.costAP - heroAp);
                    }
                    //野蛮人城寨
                    else if (monsterDefine.type == 2)
                    {
                        ap = Mathf.Max(0, monsterDefine.rallyAP - heroAp);
                    }
                    //召唤怪
                    else if (monsterDefine.type == 4)
                    {
                        //单人攻击
                        if (monsterDefine.battleType == 1)
                        {
                            ap = Mathf.Max(0, monsterDefine.costAP - heroAp);
                        }
                        //集结挑战
                        else if (monsterDefine.battleType == 2)
                        {
                            ap = Mathf.Max(0, monsterDefine.rallyAP - heroAp);
                        }
                    }

                    return ap;
                }
            }

            return 0; 
        }
        
        //行动力不足
        private bool GetIsCostMobility()
        {
            int ap = GetCostMobility();
            if (m_PlayerProxy.CurrentRoleInfo.actionForce < ap) 
            {
               // TroopHelp.OnClickAddActionPoint();
                 PlayerDataHelp.ShowActionUI(ap);
                return true;
            }
            return false;
        }


        private void OnMarchOnWorldMode()
        {
            if (GuideManager.Instance.IsGuideFightBarbarian)
            {
                CoreUtils.audioService.PlayOneShot("Sound_Ui_CreateTroops");
                CivilizationDefine define =
                    CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_PlayerProxy.CurrentRoleInfo.country);
                if (define != null)
                {
                    HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>(define.initialHero);
                    if (hero != null)
                    {
                        CoreUtils.audioService.PlayOneShot(hero.voiceMove);
                    }
                }

                AppFacade.GetInstance().SendNotification(CmdConstant.GuideFirstMarch);
                CoreUtils.uiManager.CloseUI(UI.s_createAnmy);
                return;
            }

            if (m_marchType == TroopAttackType.Attack) //打怪
            {
                if (GetIsCostMobility()) 
                {
                    return;
                }
            }            

            //判断负载是否足够采集一个资源
            if (m_marchType == TroopAttackType.Collect)
            {
                if (m_rssData.resourceGatherTypeDefine != null)
                {
                    if (m_TroopProxy.GetArmyWeight(m_selectedSoldiers,m_selectMainHeroId,m_selecttDeputyHeroId,m_rssData.rssType) < 1)
                    {
                        Tip.CreateTip(LanguageUtils.getText(500208)).Show();
                        return;
                    }
                }
            }

            CoreUtils.uiManager.CloseUI(UI.s_createAnmy);
            if (m_OpenPanelData.type == OpenPanelType.Common)
            {
                if (rssId != 0 && m_worldProxy.GetWorldMapObjectByobjectId(rssId) == null)
                {
                    Tip.CreateTip(200001).Show();
                    return;
                }
                Vector2 endV2 = m_RssProxy.GetV2(rssId);
                Vector2Int serverV2 = new Vector2Int((int)endV2.x * 100, (int)endV2.y * 100);
                int type = (int)m_TroopProxy.GetTroopAttackType(rssId);
                if (m_selectMainHero != null && m_selectedSoldiers.Count > 0)
                {
                    List<TroopsCell> lsUnitDatas = new List<TroopsCell>();
                    foreach (var item in m_selectedSoldiers)
                    {
                        var solderData = GetSoldiersDataById(item.Key);
                        if (solderData == null) continue;
                        TroopsCell data = new TroopsCell();
                        data.unitId = item.Key;
                        data.unitCount = item.Value;
                        data.unitMaxCount = solderData.Number;
                        data.unitType = (int)solderData.ServerInfo.type;
                        data.unitLevel = (int)solderData.ServerInfo.level;
                        data.unitserverId = (int)solderData.ServerInfo.id;
                        lsUnitDatas.Add(data);
                    }

                    Vector3 curCityPos = new Vector3(m_CityBuildingProxy.RolePos.x, 0, m_CityBuildingProxy.RolePos.y);
                    ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(rssId);
                    if (armyData != null)
                    {
                        
                        bool isGuild = false;
                        if (armyData.guild == 0 && m_PlayerProxy.CurrentRoleInfo.guildId == 0)
                        {
                            isGuild = false;
                        }
                        else
                        {
                            isGuild = armyData.guild == m_PlayerProxy.CurrentRoleInfo.guildId;
                        }
                        
                        if (armyData.isPlayerHave||isGuild)
                        {                     
                            Vector3 v2 = (armyData.go.transform.position - curCityPos).normalized *
                                         armyData.armyRadius;
                            Vector2 v3 = new Vector2(armyData.go.transform.position.x,
                                armyData.go.transform.position.z);
                            Vector2 v1 = new Vector2(v2.x, v2.z);
                            Vector2 v4 = v3 - v1;
                            serverV2 = new Vector2Int((int)v4.x * 100, (int)v4.y * 100);
                            rssId = 0;
                            type = (int)TroopAttackType.Space;
                        }
                    }

                    MapObjectInfoEntity mapObjectInfoEntity = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
                    if (mapObjectInfoEntity != null)
                    {                        
                        if (TroopHelp.IsTouchMoveAllianceBuilding(mapObjectInfoEntity.rssType))
                        {
                            float radius = m_ConfigDefine.cityRadius;
                            Vector3 v2 = (mapObjectInfoEntity.gameobject.transform.position - curCityPos).normalized *
                                         radius;
                            Vector2 v3 = new Vector2(mapObjectInfoEntity.gameobject.transform.position.x,
                                mapObjectInfoEntity.gameobject.transform.position.z);
                            Vector2 v1 = new Vector2(v2.x, v2.z);
                            Vector2 v4 = v3 - v1;
                            serverV2 = new Vector2Int((int)v4.x * 100, (int)v4.y * 100);
                            rssId = 0;
                            type = (int)TroopAttackType.Space;
                        }
                    }

                    m_TroopProxy.SendCreateTroopServer(m_selectMainHeroId, m_selecttDeputyHeroId, lsUnitDatas, type, rssId, serverV2);
                }

            }
            else if (m_OpenPanelData.type == OpenPanelType.CreateRally)
            {
                m_RallyTroopsProxy.SendLaunchRally(InitRallyTroopsData());
            }
            else if (m_OpenPanelData.type == OpenPanelType.JoinRally)
            {
                if (GetIsCostMobility())
                {
                    return;
                }

                ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                if (MoveTimes>m_OpenPanelData.joinRallyTimes*configDefine.rallyJoinTips)
                {
                    string des = ClientUtils.FormatTimeCityBuff(MoveTimes);
                    string[] des1 = des.Split('.');
                    string des2 = string.Empty;
                    if (des1.Length > 0)
                    {
                        des2 = des1[0];
                    }

                    TroopHelp.ShowDistanceCheckPanel(() =>
                    {
                        OnJoinRally();
                    },des2);
                    return;
                }
                OnJoinRally();
            }
            else if (m_OpenPanelData.type == OpenPanelType.Reinfore)
            {
                int soldierNum = 0;
                foreach (var soldier in m_selectedSoldiers)
                {
                    soldierNum += soldier.Value;
                }
                if (soldierNum > m_OpenPanelData.rallyTroopNum&& m_OpenPanelData.rallyTroopNum!=0)
                {
                    Tip.CreateTip(LanguageUtils.getText(200009)).Show();
                    return;
                }
                RallyTroopsData rallyTroopsData = InitRallyTroopsData();
                rallyTroopsData.reinforceObjectIndex = m_OpenPanelData.reinforceObjectIndex;
                rallyTroopsData.armyObjectIndex = m_OpenPanelData.armyIndex;
                m_RallyTroopsProxy.SendReinforeRally(rallyTroopsData);
            }
        }

        private void OnJoinRally()
        {
            RallyTroopsData rallyTroopsData = InitRallyTroopsData();
            rallyTroopsData.joinRid = m_OpenPanelData.jonRid;
            rallyTroopsData.armyObjectIndex = m_OpenPanelData.armyIndex;
            m_RallyTroopsProxy.SendJoinRally(rallyTroopsData);
        }

        private void OnMarchOnExpeditionMode()
        {
            ExpeditionProxy expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            if (expeditionProxy == null) return;
            Dictionary<long, SoldierInfo> soldierInfos = new Dictionary<long, SoldierInfo>();
            foreach(var soldierData in m_selectedSoldiers)
            {
                if (soldierData.Value == 0) continue;
                var data = GetSoldiersDataById(soldierData.Key);
                if (data == null) continue;
                soldierInfos.Add(soldierData.Key, new SoldierInfo()
                {
                    id = soldierData.Key,
                    type = data.ServerInfo.type,
                    level = data.ServerInfo.level,
                    num = soldierData.Value,
                });
            }
            expeditionProxy.AddPlayerTroop(m_OpenPanelData.ExpeditionTroopIndex, m_selectMainHeroId, m_selecttDeputyHeroId, soldierInfos);
            CoreUtils.uiManager.CloseUI(UI.s_createAnmy);
        }

        //行军
        private void OnMarch()
        {
            if (m_selectMainHero == null)
            {
                Tip.CreateTip(LanguageUtils.getText(200072)).Show();
                return;
            }
            //判断一下是否选择了士兵

            float speed = m_TroopProxy.GetArmySpeed(m_selectedSoldiers,m_selectMainHeroId, m_selecttDeputyHeroId); //速度也可以判断是否有部队
            bool isHasCount = speed > 0;
            if (!isHasCount)
            {
                Tip.CreateTip(LanguageUtils.getText(200011)).Show();
                return;
            }
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    OnMarchOnWorldMode();
                    break;
                case GameModeType.Expedition:
                    OnMarchOnExpeditionMode();
                    break;
            }
        }


        private RallyTroopsData InitRallyTroopsData()
        {
            RallyTroopsData data= new RallyTroopsData();
            if (m_selectMainHero != null)
            {
                data.mainHeroId = m_selectMainHeroId;
                data.viceId = m_selecttDeputyHeroId;

                data.targetIndext = rssId;
                data.rallyTimes = m_OpenPanelData.timesType;
                data.soldierInfo= new Dictionary<long, SoldierInfo>();
                foreach (var item in m_selectedSoldiers)
                {
                    SoldierInfo soldierInfo = new SoldierInfo();
                    soldierInfo.id = item.Key;
                    SoldiersData soldiers = GetSoldiersDataById(item.Key);
                    if (soldiers != null)
                    {
                        soldierInfo.type = soldiers.ServerInfo.type;
                        soldierInfo.level = soldiers.ServerInfo.level;
                        soldierInfo.num = item.Value;
                    }
                    data.soldierInfo.Add(soldierInfo.id, soldierInfo);
                }           
            }

            return data;
        }


        private List<UI_Item_CaptainSkill_M1_SubView> cache_SingleSkills = new List<UI_Item_CaptainSkill_M1_SubView>();

        private int m_skillItemLoadStatus =1; //1未加载 2加载中 3已加载

        private void InitSkillViewOne()
        {
            if (m_skillItemLoadStatus == 1)
            {
                m_skillItemLoadStatus = 2;

                for (int i = 0; i < 5; i++)
                {
                    CoreUtils.assetService.Instantiate("UI_Item_CaptainSkill_M1", (go) =>
                    {
                        if (view.gameObject == null)
                        {
                            return;
                        }
                        go.transform.SetParent(view.m_pl_SingleSkills_GridLayoutGroup.transform);
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.localScale = Vector3.one;

                        cache_SingleSkills.Add(new UI_Item_CaptainSkill_M1_SubView(go.GetComponent<RectTransform>()));
                        if (cache_SingleSkills.Count == 5)
                        {
                            m_skillItemLoadStatus = 3;
                            RefreshSingleSkills();
                        }
                    });
                }
            }
            else if (m_skillItemLoadStatus == 3)
            {
                RefreshSingleSkills();
            }          
        }

        private void RefreshSingleSkills()
        {
            if (m_selectMainHero == null)
            {
                return;
            }
            int count = m_selectMainHero.config.skill.Count;

            for (int i = 0; i < count; i++)
            {
                int index = i;
                UI_Item_CaptainSkill_M1_SubView itemView = cache_SingleSkills[i];
                itemView.SetSkillInfo(m_selectMainHero, index);
            }
            var more = cache_SingleSkills.Count - count;
            if (more > 0)
            {
                for (int i = more; i > 0; i--)
                {
                    cache_SingleSkills[cache_SingleSkills.Count - i].gameObject.SetActive(false);
                }
            }
            int bgIndex = 0;
            foreach (Transform skillBg in view.m_pl_SingleSkillsbg_GridLayoutGroup.transform)
            {
                if (bgIndex < 5)
                {
                    if (bgIndex < count)
                    {
                        skillBg.gameObject.SetActive(true);
                    }
                    else
                    {
                        skillBg.gameObject.SetActive(false);
                    }
                }

                bgIndex++;
            }
        }

        private List<UI_Item_CaptainSkill_M1_SubView> cache_DoubleSkills = new List<UI_Item_CaptainSkill_M1_SubView>();
        private void InitSkillView()
        {
            if (m_selectMainHero == null)
            {
                return;
            }
            var callback = new Action<UI_Item_CaptainSkill_M1_SubView, int>(
                (itemview , index) =>
                  {
                      itemview.gameObject.transform.SetParent(view.m_pl_DoubleSkillsMain_GridLayoutGroup.transform);
                      itemview.gameObject.transform.localRotation = Quaternion.identity;
                      itemview.gameObject.transform.localScale = Vector3.one;
                    
                      if (m_selectMainHero != null)
                      {
                          itemview.SetSkillInfo(m_selectMainHero, index);
                      }
                  });
            int count = 0;
            if (m_selectMainHero != null)
            {             
                count = m_selectMainHero.config.skill.Count;
            }

            for (int i = 0; i < count; i++)
            {
                int index = i;
                if (index >= cache_DoubleSkills.Count)
                {
                    if (skillM1 != null)
                    {
                        GameObject go =CoreUtils.assetService.Instantiate(skillM1);
                        var itemView = new UI_Item_CaptainSkill_M1_SubView(go.GetComponent<RectTransform>());
                        cache_DoubleSkills.Add(itemView);
                        callback(itemView, index);
                    }                
                }
                else
                {
                    cache_DoubleSkills[index].gameObject.SetActive(true);
                    callback(cache_DoubleSkills[index], index);
                }
            }

            var more = cache_DoubleSkills.Count - count;
            if (more > 0)
            {
                for (int i = more; i > 0; i--)
                {
                    cache_DoubleSkills[ cache_DoubleSkills.Count - i ].gameObject.SetActive(false);
                }
            }    
            
            int bgIndex = 0;
            foreach (Transform skillBg in view.m_pl_DoubleSkillsMainbg_ArabLayoutCompment.transform)
            {
                if (bgIndex < 5)
                {
                    if (bgIndex < count)
                    {
                        skillBg.gameObject.SetActive(true);
                    }
                    else
                    {
                        skillBg.gameObject.SetActive(false);
                    }
                }

                bgIndex++;
            }
        }


        private void InitViceSkillGo()
        {
            CoreUtils.assetService.Instantiate("UI_Item_CaptainSkill_M1", (go) =>
            {
                skillM1 = go;
                InitSkillView();
                InitViceSkillView();
            });
        }

        private GameObject skillM1;
        private List<GameObject> cache_DoubleSkillsSub = new List<GameObject>();
        private void InitViceSkillView()
        {
            if (m_selectDeputyHero == null) return;
            cache_DoubleSkillsSub.Clear();
            var callback = new Action<GameObject , int>(
                (go , index) =>
                {

                    go.transform.SetParent(view.m_pl_DoubleSkillsSub_GridLayoutGroup.transform);
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;
                    if (m_selectDeputyHero != null)
                    {                   
                        UI_Item_CaptainSkill_M1_SubView itemView1 = new UI_Item_CaptainSkill_M1_SubView(go.transform.GetComponent<RectTransform>());
                        itemView1.SetSkillInfo(m_selectDeputyHero, index);
                    }
                });

            int count = m_selectDeputyHero.config.skill.Count;
            for (int i = 0; i < count; i++)
            {
                int index = i;
                if (index >= cache_DoubleSkillsSub.Count)
                {
                    if (skillM1 != null)
                    {
                        GameObject go = CoreUtils.assetService.Instantiate(skillM1);
                        if (!cache_DoubleSkillsSub.Contains(go))
                        {
                            cache_DoubleSkillsSub.Add(go);
                        }
                        callback(go, index);
                    }
                }
                else
                {
                    cache_DoubleSkillsSub[index].gameObject.SetActive(true);
                    callback(cache_DoubleSkillsSub[index], index);
                }
            }

            var more = cache_DoubleSkillsSub.Count - count;
            if (more > 0)
            {
                for (int i = more; i > 0; i--)
                {
                    cache_DoubleSkillsSub[ cache_DoubleSkillsSub.Count - i ].gameObject.SetActive(false);
                }
            }
            
            int bgIndex = 0;
            foreach (Transform skillBg in view.m_pl_DoubleSkillsSubbg_ArabLayoutCompment.transform)
            {
                if (bgIndex < 5)
                {
                    if (bgIndex < count)
                    {
                        skillBg.gameObject.SetActive(true);
                    }
                    else
                    {
                        skillBg.gameObject.SetActive(false);
                    }
                }

                bgIndex++;
            }
        }

        private void InitSpinAnm()
        {
            if (m_selectMainHero != null)
            {                
                if (m_selectMainHero.star >= 3)
                {
                    view.m_pl_SingleCaptain_PolygonImage.gameObject.SetActive(false);
                    view.m_pl_DoubleCaptain_PolygonImage.gameObject.SetActive(true);
                    ClientUtils.LoadSpine(view.m_spin_Dchar1_SkeletonGraphic, m_selectMainHero.config.heroModel);
                    view.m_img_NoSingleCaptain1_PolygonImage.gameObject.SetActive(false);
                    view.m_lbl_DoubleName1_LanguageText.text = LanguageUtils.getText(m_selectMainHero.config.l_nameID);                 
                    ClientUtils.DestroyAllChild(view.m_pl_DoubleSkillsSub_GridLayoutGroup.transform);
                    InitSkillView();
                    if (m_selectDeputyHero != null)
                    {
                        view.m_spin_Dchar2_SkeletonGraphic.gameObject.SetActive(true);
                        ClientUtils.LoadSpine(view.m_spin_Dchar2_SkeletonGraphic, m_selectDeputyHero.config.heroModel);
                        view.m_img_NoSingleCaptain2_PolygonImage.gameObject.SetActive(false);
                        view.m_lbl_DoubleName2_LanguageText.text = LanguageUtils.getText(m_selectDeputyHero.config.l_nameID);
                        InitViceSkillView();
                        view.m_btn_delete2_GameButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        view.m_spin_Dchar2_SkeletonGraphic.gameObject.SetActive(false);
                        view.m_img_NoSingleCaptain2_PolygonImage.gameObject.SetActive(true);
                        view.m_lbl_DoubleName2_LanguageText.text = string.Empty;
                        view.m_btn_plus2_GameButton.gameObject.SetActive(true);
                        view.m_btn_delete2_GameButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    view.m_pl_SingleCaptain_PolygonImage.gameObject.SetActive(true);
                    view.m_pl_DoubleCaptain_PolygonImage.gameObject.SetActive(false);
                    ClientUtils.LoadSpine(view.m_spin_Schar_SkeletonGraphic, m_selectMainHero.config.heroModel);
                    view.m_spin_Schar_SkeletonGraphic.gameObject.SetActive(true);
                    view.m_img_NoSingleCaptain_PolygonImage.gameObject.SetActive(false);
                    view.m_lbl_SingleName_LanguageText.text = LanguageUtils.getText(m_selectMainHero.config.l_nameID);
                    InitSkillViewOne();
                }               

                //统帅战力
                string name = LanguageUtils.getTextFormat(200049,
                    ClientUtils.FormatComma(m_TroopProxy.GetHeroPower(m_selectMainHeroId, m_selecttDeputyHeroId)));
                view.m_lbl_powerTotal_LanguageText.text = name;
            }
            else
            {
                view.m_img_NoSingleCaptain_PolygonImage.gameObject.SetActive(true);
                view.m_pl_DoubleCaptain_PolygonImage.gameObject.SetActive(false);
                view.m_pl_SingleCaptain_PolygonImage.gameObject.SetActive(true);
                view.m_lbl_SingleName_LanguageText.text = "";
                view.m_lbl_powerTotal_LanguageText.text = LanguageUtils.getTextFormat(200049, 0);
            }
            RefreshDingleChangeBtn();
        }

        private void OnHeroSelected(HeroProxy.Hero hero, UI_Item_CaptainHead_SubView itemView = null)
        {
            if(m_selectMainHero == hero)
            {
                return;
            }
            if (isSelectVice && hero == m_selectDeputyHero)
            {
                return;
            }
            
            if (isSelectMain && hero == m_selectDeputyHero)
            {
                return;
            }
            
            if (m_selectItemCaptainHead != null)
            {
                m_selectItemCaptainHead.Selected(hero);
            }

            itemView.Selected(hero);
            m_curSelectedHero = hero;
            m_selectItemCaptainHead = itemView;
            if (isSelectVice)
            {
                m_selectDeputyHero = hero;
            }
            else if (isSelectMain)
            {
                m_selectMainHero = hero;
                if(m_selectMainHero.star < 3)
                {
                    m_selectDeputyHero = null;
                }
            }

            InitViewData();
        }


        private void InitViewData()
        {
            CalTroopsCapacity(m_selectMainHeroId, m_selecttDeputyHeroId);
            UpdateResWeightMulti();
            SetDefaultSelectData();
            InitSpinAnm();
            Init();
            RefreshDingleChangeBtn();
            MoveUIPos(true, 2);
            isOpenDingleChange = false;
        }

        private void RefreshDingleChangeBtn()
        {
            view.m_btn_DingleChange2_GameButton.gameObject.SetActive(m_selectDeputyHero != null);
        }


        //计算可派遣部队容量上限
        private void CalTroopsCapacity(long heroId, long viceId)
        {

            switch(m_OpenPanelData.type)
            {
                case OpenPanelType.Common:
                    m_troopsCapacity = m_TroopProxy.GetHeroTroopsCapacity(heroId, viceId);                    
                    break;
                case OpenPanelType.Reinfore:
                case OpenPanelType.JoinRally:
                    m_troopsCapacity = m_TroopProxy.GetHeroTroopsCapacity(heroId, viceId);               
                    if (m_OpenPanelData.rallyTroopNum != 0)
                    {
                        m_troopsCapacity = Mathf.Min((int)m_OpenPanelData.rallyTroopNum, (int)m_troopsCapacity);
                    }
                    break;
                case OpenPanelType.CreateRally:
                    //部队容量
                    var heroTroopsCapacity = m_TroopProxy.GetHeroTroopsCapacity(heroId, viceId);
                    //集结部队容量
                    var rallyTroopsCapacity = m_TroopProxy.GetRallyTroopsCapacity(heroId, viceId);
                    //关卡或圣地或联盟建筑容量
                    var otherTroopsCapacity = 0;

                    if (m_rssData != null)
                    {
                        //集结关卡和圣地
                        if (TroopHelp.IsStrongHoldType(m_rssData.rssType))
                        {
                            //关卡或圣地容量
                            otherTroopsCapacity = m_TroopProxy.GetStrongHoldTroopsCapacity(heroId, viceId, (int)m_rssData.strongHoldId);
                        }
                        //集结集结联盟建筑
                        else if (TroopHelp.IsAttackGuildType(m_rssData.rssType))
                        {
                            //联盟建筑容量
                            otherTroopsCapacity = m_TroopProxy.GetGuildTroopsCapacity(heroId, viceId, m_rssData.rssType);
                        }
                    }

                    if (otherTroopsCapacity != 0)
                    {
                        m_troopsCapacity = Mathf.Min(rallyTroopsCapacity, heroTroopsCapacity, otherTroopsCapacity);
                    }
                    else
                    {
                        m_troopsCapacity = Mathf.Min(rallyTroopsCapacity, heroTroopsCapacity);
                    }

                    break;
            }
        }

        private void OnRefreshSoldiersNum()
        {
            //部队数量
            string str = LanguageUtils.getTextFormat(181104, ClientUtils.FormatComma(GetSelectedSoldierCount()),
                ClientUtils.FormatComma(m_troopsCapacity));
            view.m_lbl_armyCapa_LanguageText.text = LanguageUtils.getTextFormat(200046, str);
            //总战力
            long heroPower=  m_TroopProxy.GetHeroPower(m_selectMainHeroId, m_selecttDeputyHeroId);
            long fightPower = TroopProxy.GetFightingCount(m_selectedSoldiers);
            long pwoer= heroPower + fightPower;
            view.m_lbl_powerCap_LanguageText.text = 
                LanguageUtils.getTextFormat(200050, ClientUtils.FormatComma(pwoer));

            //负载
            if (m_marchType != TroopAttackType.Attack|| (m_rssData!=null&& m_rssData.rssType== RssType.City)||
                (m_rssData!=null&& m_rssData.rssType== RssType.Troop))
            {
                if (m_rssData != null)
                {
                    view.m_lbl_armyWeight_LanguageText.text =
                        LanguageUtils.getTextFormat(200047, ClientUtils.FormatComma(m_TroopProxy.GetArmyWeight(m_selectedSoldiers,m_selectMainHeroId,m_selecttDeputyHeroId,m_rssData.rssType)));  
                }
                else
                {
                    view.m_lbl_armyWeight_LanguageText.text =
                        LanguageUtils.getTextFormat(200047, ClientUtils.FormatComma(m_TroopProxy.GetArmyWeight(m_selectedSoldiers,m_selectMainHeroId,m_selecttDeputyHeroId))); 
                }
            }

            //更新行军时间
            float speed = m_TroopProxy.GetArmySpeed(m_selectedSoldiers, m_selectMainHeroId, m_selecttDeputyHeroId);
            MoveTimes = (speed > 0) ? Mathf.CeilToInt(CalDistance() / speed) : 0;

            if (GameModeManager.Instance.CurGameMode == GameModeType.World)
            {
                if (m_TroopProxy.GetArmySum(m_selectedSoldiers) <= 0)
                {
                    view.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.text = LanguageUtils.getText(200052);
                }
                else
                {
                    view.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown(MoveTimes);
                }

                view.m_UI_Model_DoubleLineButton_Yellow.m_img_icon2_PolygonImage.gameObject.SetActive(true);
            }

            RefreshDifficultTip();
        }

        private void RefreshDifficultTip()
        {
            view.m_lbl_compare_LanguageText.gameObject.SetActive(false);
            if (m_rssData == null || m_rssData.monsterDefine == null)
            {
                // 隐藏提示
                return;
            }

            if (m_rssData.monsterDefine.type != 1 && m_rssData.monsterDefine.type != 3)
            {
                return;
            }
            
            // 总战力
            long soliderPower = TroopProxy.GetFightingCount(m_selectedSoldiers);
            
            view.m_lbl_compare_LanguageText.gameObject.SetActive(true);
            view.m_lbl_compare_LanguageText.text = LanguageUtils.getText(
                FightHelper.Instance.GetFightingDifficultTip((int)soliderPower, m_rssData.monsterDefine.powerAdvise));

            if (m_rssData.monsterDefine.type == 1)
            {
                if (!GuideProxy.IsGuideing)
                {
                    //功能引导
                    //部队战力小等于野蛮人战力*50%
                    if (soliderPower <= (m_rssData.monsterDefine.powerAdvise * m_ConfigDefine.battleRecommend4))
                    {
                        if (m_TroopProxy.GetArmySum(m_selectedSoldiers) > 0)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.TroopsLack);
                        }
                    }
                }
            }

        }

        //刷新士兵列表
        private void ListItemByIndex(ListView.ListItem listItem)
        {
            UI_Item_ArmyCount_SubView itemView = null;
            if (listItem.data != null)
            {
                itemView = listItem.data as UI_Item_ArmyCount_SubView;
            }
            else
            {
                itemView = new UI_Item_ArmyCount_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = itemView;

                InputSliderControl control = new InputSliderControl();
                control.Init(itemView.m_ipt_ArmyInput_GameInput, itemView.m_sd_count_GameSlider,
                    itemView.m_lbl_show_LanguageText, NumChangeCallback);
                itemView.InputSdControl = control;
            }

            int index = listItem.index;
            SoldiersData data = m_availableSoldiers[index];
            if (data != null)
            {
                itemView.m_lbl_ArmyName_LanguageText.text = LanguageUtils.getText(data.ArmysCfg.l_armsID);
                ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, data.ArmysCfg.icon);
                itemView.InputSdControl.UpdateMinMax(0, data.Number);
                itemView.InputSdControl.UpdateIndex(listItem.index);
                itemView.InputSdControl.SetInputVal(m_selectedSoldiers.ContainsKey(data.Id)? m_selectedSoldiers[data.Id] : 0);
            }
        }

        private void NumChangeCallback(int num, int index)
        {
            SoldiersData data = m_availableSoldiers[index];
            int oldNum = 0;
            m_selectedSoldiers.TryGetValue(data.Id, out oldNum);
            int total = GetSelectedSoldierCount();
            total -= oldNum;
            int maxNum = m_troopsCapacity - total;
            num = Mathf.Min(num, maxNum);
            m_selectedSoldiers[data.Id] = num;
            ListView.ListItem item = view.m_sv_armyList_ListView.GetItemByIndex(index);
            if (item != null)
            {
                UI_Item_ArmyCount_SubView itemView = item.data as UI_Item_ArmyCount_SubView;
                if (itemView != null)
                {
                    itemView.InputSdControl.SetInputVal(num);
                }
            }
            OnRefreshSoldiersNum();
        }


        #region 部队编队保存

        private void RestInitData(int id, ref bool isShow)
        {
            SaveData saveData = m_HeroSaveData.GetData(id) as SaveData;
            if (saveData != null)
            {
                HeroProxy.Hero hero = m_availableHeros.Find((t)=> { return t.config.ID == saveData.heroId; });
                if (hero == null)
                {
                    isShow = false;
                    Tip.CreateTip(200080).Show();
                    return;
                }

                isShow = true;
                m_selectMainHero = m_availableHeros.Find((t) => { return t.config.ID == saveData.heroId; });
                m_selectDeputyHero = m_availableHeros.Find((t) => { return t.config.ID == saveData.viceId; });
                InitTroopData();
                CalTroopsCapacity(m_selectMainHeroId, m_selecttDeputyHeroId);
                UpdateResWeightMulti();
                SetDefaultSelectData();
                Init(false);
                UpdateSaveSoldiersData(saveData.solds, ref isShow);

                if(m_curSelectMode != SelectMode.None)
                {
                    m_availableSelectHeros = GetAvailableSelectHeros(m_curSelectMode);
                    HeroSortRefresh(m_sortType);
                }
            }
        }

        private void OnRefreshSaveData()
        {
            string des = string.Empty;
            TroopSaveType type = _mTroopSave.GetSaveType();
            if (type == TroopSaveType.Save)
            {
                des = LanguageUtils.getText(200018);
            }
            else if (type == TroopSaveType.Read)
            {
                des = LanguageUtils.getText(200019);
            }

            view.m_lbl_des_LanguageText.text = des;
            OnRefreshSaveNumView();
        }

        private void OnRefreshSaveNumView()
        {
            int count = m_HeroSaveData.GetDataCount();
            view.m_lbl_saveData_LanguageText.text = string.Format("{0}/{1}", count, TroopSave.SaveNum);
            view.m_btn_refresh_GameButton.gameObject.SetActive(count > 0);
            view.m_ck_sll_GameToggle.gameObject.SetActive(count > 0);
        }


        private void OnRefreshTroopSaveView(int index = 0)
        {
            view.m_list_View_save_ListView.ForceRefresh();
            if (index > 0)
            {
                OnMovePanelToItemIndex(index - 1);
            }
        }

        private void OnMovePanelToItemIndex(int index)
        {
            if (index > 0)
            {
                view.m_list_View_save_ListView.MovePanelToItemIndex(index - 1);
            }
        }

        private void ListHeroSaveItemByIndex(ListView.ListItem listItem)
        {
            UITroopsSaveItemView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UITroopsSaveItemView>(listItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UITroopsSaveItemView>(listItem.go);
            }

            int index = listItem.index + 1;
            int idx = GetIndex(index);
            itemView.m_lbl_id_LanguageText.text = idx.ToString();
            SaveData data = m_HeroSaveData.GetData(index) as SaveData;
            if (data != null)
            {
                itemView.m_pl_def.gameObject.SetActive(false);
                itemView.m_btn_save_GameButton.gameObject.SetActive(true);
                itemView.m_btn_delete_GameButton.gameObject.SetActive(true);
                itemView.m_btn_delete_GameButton.gameObject.SetActive(data.isDelete);

                string icondef = GetDefIcon(index);
                string icon = GetIncon(index);
                ClientUtils.LoadSprite(itemView.m_img_bg_PolygonImage, icondef);
                ClientUtils.LoadSprite(itemView.m_img_light_PolygonImage, icon);
                itemView.m_UI_Model_CaptainHeadSub.SetIcon(data.iconPath);
                HeroProxy.Hero heroMain = m_HeroProxy.GetHeroByID(data.heroId);
                if (heroMain != null)
                {
                    itemView.m_UI_Model_CaptainHeadSub.SetRare(heroMain.config.rare);
                }

                if (!string.IsNullOrEmpty(data.iconVicePath))
                {
                    itemView.m_UI_Model_CaptainHeadMain.gameObject.SetActive(true);
                    itemView.m_UI_Model_CaptainHeadMain.SetIcon(data.iconVicePath);
                    HeroProxy.Hero heroVice = m_HeroProxy.GetHeroByID(data.viceId);
                    if (heroVice != null)
                    {
                        itemView.m_UI_Model_CaptainHeadMain.SetRare(heroVice.config.rare);
                    }
                }
                else
                {
                    itemView.m_UI_Model_CaptainHeadMain.gameObject.SetActive(false);
                }

                itemView.m_lbl_num_LanguageText.text =  ClientUtils.FormatComma(data.soldierNum);
                itemView.m_btn_save_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_save_GameButton.onClick.AddListener(() =>
                {
                    string name = GetName(data.type);
                    TroopSaveType type = _mTroopSave.GetSaveType();
                    if (type == TroopSaveType.Save)
                    {
                        string str = LanguageUtils.getTextFormat(200073, name, idx);
                        Alert.CreateAlert(str, LanguageUtils.getText(200074)).SetLeftButton(() => { })
                            .SetRightButton(() =>
                            {
                                UpdateSave(data.id);
                            }).SetReverseButton().Show();
                    }
                    else if (type == TroopSaveType.Read)
                    {
                        ReadSave(data.id, data.type, name, idx);
                    }
                });
                itemView.m_btn_delete_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_delete_GameButton.onClick.AddListener(() =>
                {
                    TroopSaveNumType troopSaveNumType = GetTroopSaveNumType(index);    
                    string name = GetName(troopSaveNumType);
                    string str = LanguageUtils.getTextFormat(200086, name, idx);
                    Alert.CreateAlert(str, LanguageUtils.getText(200074)).SetLeftButton(() => { })
                        .SetRightButton(() =>
                        {
                            _mTroopSave.DeleteSave(index);
                            _mTroopSave.DeletSaveType(troopSaveNumType, data);                           
                        }).SetReverseButton().Show();
                });
            }
            else
            {
                itemView.m_pl_def.gameObject.SetActive(true);
                itemView.m_btn_delete_GameButton.gameObject.SetActive(false);
                itemView.m_btn_save_GameButton.gameObject.SetActive(false);
                string icon = GetDefIcon(index);
                if (!string.IsNullOrEmpty(icon))
                {
                    ClientUtils.LoadSprite(itemView.m_pl_def_img_bg_PolygonImage, icon);
                }

                itemView.m_pl_def_img_bg_GameButton.onClick.RemoveAllListeners();
                itemView.m_pl_def_img_bg_GameButton.onClick.AddListener(() =>
                {
                    TroopSaveType type = _mTroopSave.GetSaveType();
                    if (type == TroopSaveType.Save)
                    {
                        TroopSaveNumType troopSaveNumType = GetTroopSaveNumType(index);
                        bool isShow =InsertSave(index, troopSaveNumType);
                        Sortindex = (int)troopSaveNumType;         
                        string name = GetName(troopSaveNumType);
                        string str = LanguageUtils.getTextFormat(200085, name, idx);
                        if (isShow)
                        {                         
                            Tip.CreateTip(str).Show();  
                        }

                    }
                });
            }
        }

        private void ReadSave(int id,TroopSaveNumType type, string name, int idx)
        {
            string str = LanguageUtils.getTextFormat(200075, name, idx);
            bool isShow = true;
            RestInitData(id, ref isShow);
            _mTroopSave.SetSelect(type, id);
            if (isShow)
            {
                Tip.CreateTip(str).Show();
            }
        }


        private bool InsertSave(int id, TroopSaveNumType type)
        {
            if (m_selectedSoldiers.Count == 0)
            {
                Tip.CreateTip(200076).Show();
                return false;
            }

            _mTroopSave.InsertSave(id, (int) type, m_selectMainHeroId, m_selecttDeputyHeroId, m_selectedSoldiers);
            return true;
        }

        private void UpdateSave(int id)
        {
            if (m_selectedSoldiers.Count == 0)
            {
                Tip.CreateTip(200076).Show();
                return;
            }
            _mTroopSave.UpdateSave(id, m_selectMainHeroId, m_selecttDeputyHeroId, m_selectedSoldiers);
        }

        private int Sortindex = 0;
        private bool isADD = true;
        private bool isRemove = false;
        private TroopSaveNumType curTroopSaveNumType;

        private void InitSortIndex()
        {
            SaveData saveBlueData = _mTroopSave.GetNextTroopSaveNumType(TroopSaveNumType.Blue) as SaveData;
            if (saveBlueData != null)
            {
                Sortindex = GetSortIndex(saveBlueData.type);
                return;
            }

            SaveData saveYellowData = _mTroopSave.GetNextTroopSaveNumType(TroopSaveNumType.Yellow) as SaveData;
            if (saveYellowData != null)
            {
                Sortindex = GetSortIndex(saveYellowData.type);
                return;
            }

            SaveData saveRedData = _mTroopSave.GetNextTroopSaveNumType(TroopSaveNumType.Red) as SaveData;
            if (saveRedData != null)
            {
                Sortindex = GetSortIndex(saveRedData.type);
            }
        }

        private void PlayAutoRefreshSaveIndexView()
        {
            if (Sortindex >= 3)
            {
                isRemove = true;
                isADD = false;
            }

            if (Sortindex <= 1)
            {
                isADD = true;
                isRemove = false;
            }
            if (isRemove)
            {
                Sortindex -= 1;
            }

            if (isADD)
            {
                Sortindex += 1;
            }

            TroopSaveNumType type = (TroopSaveNumType) Sortindex;
            _mTroopSave.RestSave();
            AutoRefreshSaveIndexView(type); 
            
            bool check = _mTroopSave.IsContainsData(type);
            if (!check)
            {
                PlayAutoRefreshSaveIndexView();
                if (type != curTroopSaveNumType)
                {
                    int count = 0;
                    foreach (TroopSaveNumType t in Enum.GetValues(typeof(TroopSaveNumType)))
                    {
                        if (t == TroopSaveNumType.None)
                        {
                            continue;
                        }

                        List<SaveData> saveData = _mTroopSave.GetSaveDataByType(t) as List<SaveData>;
                        if (saveData != null)
                        {
                            count += 1;
                        }
                    }
                    
                    if (count >= 2)
                    {
                        PlayAutoRefreshSaveIndexView();
                    }
                }
            }
        }


        private void AutoRefreshSaveIndexView(TroopSaveNumType type)
        {
            SaveData saveData = _mTroopSave.GetNextTroopSaveNumType(type) as SaveData;
            if (saveData == null)
            {
                return;
            }
            AutoRefreshSaveIndexData(saveData.type, saveData.id);
        }

        private void AutoRefreshSaveIndexData(TroopSaveNumType type, int id)
        {
            switch (type)
            {
                case TroopSaveNumType.Blue:
                    OnRefreshSaveIndexViewBule();
                    break;
                case TroopSaveNumType.Red:
                    OnRefreshSaveIndexViewRed();
                    break;
                case TroopSaveNumType.Yellow:
                    OnRefreshSaveIndexViewYellow();
                    break;
            }

            OnRefreshTroopSaveView(id);
        }

        private void OnRefreshSaveIndexViewBule()
        {
            SetPanelActive(TroopSaveNumType.Blue);
            int count = _mTroopSave.GetLsSaveCount(TroopSaveNumType.Blue);
            view.m_list_SaveIndex_View_Blue_ListView.FillContent(count);
            view.m_list_SaveIndex_View_Blue_ListView.ForceRefresh();
        }

        private void OnRefreshSaveIndexViewYellow()
        {
            SetPanelActive(TroopSaveNumType.Yellow);
            int count = _mTroopSave.GetLsSaveCount(TroopSaveNumType.Yellow);
            view.m_list_SaveIndex_View_Yellow_ListView.FillContent(count);
            view.m_list_SaveIndex_View_Yellow_ListView.ForceRefresh();
        }

        private void OnRefreshSaveIndexViewRed()
        {
            SetPanelActive(TroopSaveNumType.Red);
            int count = _mTroopSave.GetLsSaveCount(TroopSaveNumType.Red);
            view.m_list_SaveIndex_View_Red_ListView.FillContent(count);
            view.m_list_SaveIndex_View_Red_ListView.ForceRefresh();
        }

        private void SetPanelActive(TroopSaveNumType type)
        {
            curTroopSaveNumType = type;
            view.m_list_SaveIndex_View_Blue_ListView.gameObject.SetActive(type == TroopSaveNumType.Blue);
            view.m_list_SaveIndex_View_Yellow_ListView.gameObject.SetActive(type == TroopSaveNumType.Yellow);
            view.m_list_SaveIndex_View_Red_ListView.gameObject.SetActive(type == TroopSaveNumType.Red);
        }

        private void ListSaveItemByIndexByBlue(ListView.ListItem listItem)
        {
            int index = listItem.index;
            UI_Item_Save_IndexView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_Save_IndexView>(listItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_Save_IndexView>(listItem.go);
            }

            if (itemView != null)
            {
                SaveData saveData = _mTroopSave.GetLsSave(TroopSaveNumType.Blue, index) as SaveData;
                if (saveData != null)
                {
                    GameToggle toggle = itemView.gameObject.GetComponent<GameToggle>();
                    toggle.group = view.m_c_list_view_Blue_ToggleGroup;
                    itemView.gameObject.SetActive(true);
                    string icon = GetBtnIcon(TroopSaveNumType.Blue);
                    string icondef = GetBtnDefIcon(TroopSaveNumType.Blue);
                    ClientUtils.LoadSprite(itemView.m_img_Background_PolygonImage, icondef);
                    ClientUtils.LoadSprite(itemView.m_img_Checkmark_PolygonImage, icon);
                    int id = GetIndex(saveData.id);
                    itemView.m_lbl_saveid_LanguageText.text = id.ToString();
                    if (saveData.isSelect)
                    {
                        itemView.m_lbl_saveid_LanguageText.color = new Color((float)(54.0/255),(float)(52.0/255),(float)(50.0/255),1);
                    }
                    else
                    {
                        itemView.m_lbl_saveid_LanguageText.color = new Color((float)(164.0/255),(float)(153.0/255),(float)(117.0/255),1);
                    }

                    toggle.isOn = saveData.isSelect;
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn)
                        {
                           
                        }
                    });
                    toggle.RemoveAllClickListener();
                    toggle.AddListener(() =>
                    {
                        OnMovePanelToItemIndex(saveData.id);  
                        string name = GetName(TroopSaveNumType.Blue);
                        ReadSave(saveData.id,TroopSaveNumType.Blue,name,id);
                        MoveUIPos(true, 2);
                        MoveUIPos();
                    });
                }
                else
                {
                    itemView.gameObject.SetActive(false);
                }
            }
        }


        private void ListSaveItemByIndexByYellow(ListView.ListItem listItem)
        {
            int index = listItem.index;
            UI_Item_Save_IndexView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_Save_IndexView>(listItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_Save_IndexView>(listItem.go);
            }

            if (itemView != null)
            {
                SaveData saveData = _mTroopSave.GetLsSave(TroopSaveNumType.Yellow, index) as SaveData;
                if (saveData != null)
                {
                    GameToggle toggle = itemView.gameObject.GetComponent<GameToggle>();
                    toggle.group = view.m_c_list_view_Yellow_ToggleGroup;
                    itemView.gameObject.SetActive(true);
                    string icon = GetBtnIcon(TroopSaveNumType.Yellow);
                    string icondef = GetBtnDefIcon(TroopSaveNumType.Yellow);
                    ClientUtils.LoadSprite(itemView.m_img_Background_PolygonImage, icondef);
                    ClientUtils.LoadSprite(itemView.m_img_Checkmark_PolygonImage, icon);
                    int id = GetIndex(saveData.id);
                    itemView.m_lbl_saveid_LanguageText.text = id.ToString();
                    toggle.isOn = saveData.isSelect;
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn)
                        {
                          
                        }
                    });
                    toggle.RemoveAllClickListener();
                    toggle.AddListener(() =>
                    {
                        OnMovePanelToItemIndex(saveData.id); 
                        string name = GetName(TroopSaveNumType.Yellow);
                        ReadSave(saveData.id,TroopSaveNumType.Yellow,name,id);
                        MoveUIPos(true, 2);
                        MoveUIPos();
                    });
                }
                else
                {
                    itemView.gameObject.SetActive(false);
                }
            }
        }


        private void ListSaveItemByIndexByRed(ListView.ListItem listItem)
        {
            int index = listItem.index;
            UI_Item_Save_IndexView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_Save_IndexView>(listItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_Save_IndexView>(listItem.go);
            }

            if (itemView != null)
            {
                SaveData saveData = _mTroopSave.GetLsSave(TroopSaveNumType.Red, index) as SaveData;
                if (saveData != null)
                {
                    GameToggle toggle = itemView.gameObject.GetComponent<GameToggle>();
                    toggle.group = view.m_c_list_view_Red_ToggleGroup;
                    itemView.gameObject.SetActive(true);
                    string icon = GetBtnIcon(TroopSaveNumType.Red);
                    string icondef = GetBtnDefIcon(TroopSaveNumType.Red);
                    ClientUtils.LoadSprite(itemView.m_img_Background_PolygonImage, icondef);
                    ClientUtils.LoadSprite(itemView.m_img_Checkmark_PolygonImage, icon);
                    int id = GetIndex(saveData.id);
                    itemView.m_lbl_saveid_LanguageText.text = id.ToString();
                    toggle.isOn = saveData.isSelect;
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn)
                        {
                            
                        }
                    });
                    
                    toggle.RemoveAllClickListener();
                    toggle.AddListener(() =>
                    {
                        OnMovePanelToItemIndex(saveData.id);  
                        string name = GetName(TroopSaveNumType.Red);
                        ReadSave(saveData.id,TroopSaveNumType.Red,name,id);
                        MoveUIPos(true, 2);
                        MoveUIPos();
                    });
                }
                else
                {
                    itemView.gameObject.SetActive(false);
                }
            }
        }

        private int GetSortIndex(TroopSaveNumType type)
        {
            int index = 0;
            switch (type)
            {
                case TroopSaveNumType.Blue:
                    index += 1;
                    break;
                case TroopSaveNumType.Yellow:
                    index += 2;
                    break;
                case TroopSaveNumType.Red:
                    index += 3;
                    break;
            }

            return index;
        }


    
        private int GetIndex(int index)
        {
            int idx = 1;
            if (index >= 1 && index <= 5)
            {
                idx = index;
            }
            else if (index > 5 && index <= 10)
            {
                idx = index - 5;
            }
            else if (index > 10 && index <= 15)
            {
                idx = index - 10;
            }

            return idx;
        }

        private string GetDefIcon(int index)
        {
            string icon = string.Empty;
            if (index >= 1 && index <= 5)
            {
                icon = RS.TroopsSaveDefIcon[0];
            }
            else if (index > 5 && index <= 10)
            {
                icon = RS.TroopsSaveDefIcon[1];
            }
            else if (index > 10 && index <= 15)
            {
                icon = RS.TroopsSaveDefIcon[2];
            }

            return icon;
        }

        private string GetIncon(int index)
        {
            string icon = string.Empty;
            if (index >= 1 && index <= 5)
            {
                icon = RS.TroopsSaveIcon[0];
            }
            else if (index > 5 && index <= 10)
            {
                icon = RS.TroopsSaveIcon[1];
            }
            else if (index > 10 && index <= 15)
            {
                icon = RS.TroopsSaveIcon[2];
            }

            return icon;
        }

        private TroopSaveNumType GetTroopSaveNumType(int index)
        {
            TroopSaveNumType type = TroopSaveNumType.Blue;
            if (index >= 1 && index <= 5)
            {
                type = TroopSaveNumType.Blue;
            }
            else if (index > 5 && index <= 10)
            {
                type = TroopSaveNumType.Yellow;
            }
            else if (index > 10 && index <= 15)
            {
                type = TroopSaveNumType.Red;
            }

            return type;
        }

        private string GetBtnIcon(TroopSaveNumType troopSaveNumType)
        {
            string str = string.Empty;
            switch (troopSaveNumType)
            {
                case TroopSaveNumType.Blue:
                    str = RS.TroopsSaveBtnIcon[0];
                    break;
                case TroopSaveNumType.Yellow:
                    str = RS.TroopsSaveBtnIcon[1];
                    break;
                case TroopSaveNumType.Red:
                    str = RS.TroopsSaveBtnIcon[2];
                    break;
            }

            return str;
        }

        private string GetBtnDefIcon(TroopSaveNumType troopSaveNumType)
        {
            string str = string.Empty;
            switch (troopSaveNumType)
            {
                case TroopSaveNumType.Blue:
                    str = RS.TroopsSaveBtnDefIcon[0];
                    break;
                case TroopSaveNumType.Yellow:
                    str = RS.TroopsSaveBtnDefIcon[1];
                    break;
                case TroopSaveNumType.Red:
                    str = RS.TroopsSaveBtnDefIcon[2];
                    break;
            }

            return str;
        }

        private string GetName(TroopSaveNumType troopSaveNumType)
        {
            string str = string.Empty;
            switch (troopSaveNumType)
            {
                case TroopSaveNumType.Blue:
                    str = LanguageUtils.getText(200077);
                    break;
                case TroopSaveNumType.Yellow:
                    str = LanguageUtils.getText(200078);
                    break;
                case TroopSaveNumType.Red:
                    str = LanguageUtils.getText(200079);
                    break;
            }

            return str;
        }


        private List<int> lsHintName = new List<int>();
        private bool isShowTips = false;

        private void UpdateSaveSoldiersData(List<SaveSoldiersData> solds,ref bool isShow)
        {
            if (solds == null)
            {
                return;
            }

            lsHintName.Clear();
            m_selectedSoldiers.Clear();
            foreach (var info in solds)
            {
                var solderData = m_availableSoldiers.Find((t) => { return t.Id == info.id; });
                if (solderData == null)
                {
                    continue;
                }

                if (solderData.Number<info.num)
                {
                    AddShowHint(info.id);
                    m_selectedSoldiers[info.id] = solderData.Number;
                }
                else
                {
                    m_selectedSoldiers[info.id] = (int)info.num;
                }
            }
            OnRefreshSoldiersNum();
            view.m_sv_armyList_ListView.ForceRefresh();
            string str = string.Empty;
            for (int i = 0; i < lsHintName.Count; i++)
            {
                str = string.Format("{0}{1}{2}", str, i > 0 ? "," : "", LanguageUtils.getText(lsHintName[i]));
            }

            isShowTips = !string.IsNullOrEmpty(str);
            str = LanguageUtils.getTextFormat(200010, str);
            if (isShowTips)
            {
                Tip.CreateTip(str, Tip.TipStyle.Middle).Show();
                isShow = false;
            }
        }

        private void AddShowHint(int id)
        {
            ArmsDefine config = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
            if (config != null)
            {
                lsHintName.Add(config.l_armsID);
            }
        }

        #endregion


        //英雄选择列表
        private void ListHeroItemByIndex(ListView.ListItem listItem)
        {
            UI_LC_Captain_SubView subView = null;

            
            if (listItem.data != null)
            {
                subView = listItem.data as UI_LC_Captain_SubView;
            }
            else
            {
                subView = new UI_LC_Captain_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.AddClickEvent(OnHeroSelected);
            }
            List<HeroProxy.Hero> heros = new List<HeroProxy.Hero>();
            for (int i = listItem.index * 2; i < m_availableSelectHeros.Count && i < (listItem.index + 1) * 2; i++)
            {
                heros.Add(m_availableSelectHeros[i]);
            }
            subView.SetHero(heros);
            subView.SelecteHero(m_curSelectedHero);
            if (subView.m_selectCaptainHead != null)
            {
                m_selectItemCaptainHead = subView.m_selectCaptainHead;
            }
        }

        private void UpdateSoldiersData()
        {
            if(m_btnClearSoldier)
            {
                m_isClickedClearSoldier = true;
                m_selectedSoldiers.Clear();
            }
            else
            {
                UpdateResWeightMulti();
                SetDefaultSelectData();
            }
            if (m_btnClearSoldier)
            {
                tx.text = LanguageUtils.getText(200024);
                m_btnClearSoldier = false;
            }
            else
            {
                tx.text = LanguageUtils.getText(200051);
                m_btnClearSoldier = true;
            }
            OnRefreshSoldiersNum();
            view.m_sv_armyList_ListView.ForceRefresh();
        }

        private void RefreshSoldiersList()
        {
            view.m_sv_armyList_ListView.ForceRefresh();
        }

        private bool isSaveShowPanel = false;

        private void MoveUIPos(bool moveleft = false, int type = 1, bool isPlayUE = true)
        {
            if (type == 1)
            {
                if (moveleft)
                {
                    if (LanguageUtils.IsArabic())
                    {
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(137, 0.2f);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(137, 0.2f);
                        view.m_UI_Item_ArmySaveView_ArabLayoutCompment.gameObject.transform.DOLocalMoveX(-567.5f, 0.2f);
                    }
                    else
                    {
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(-137, 0.2f);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(-137, 0.2f);
                        view.m_UI_Item_ArmySaveView_ArabLayoutCompment.gameObject.transform.DOLocalMoveX(567.5f, 0.2f);
                    }
                }
                else
                {
                    float playUETimes = 0.2f;
                    if (!isPlayUE)
                    {
                        playUETimes = 0;
                    }

                    if (LanguageUtils.IsArabic())
                    {
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(0, 0.2f);
                        view.m_UI_Item_ArmySaveView_ArabLayoutCompment.gameObject.transform.DOLocalMoveX(-1400, playUETimes);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(0, 0.2f); 
                    }
                    else
                    {
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(0, 0.2f);
                        view.m_UI_Item_ArmySaveView_ArabLayoutCompment.gameObject.transform.DOLocalMoveX(1400, playUETimes);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(0, 0.2f);
                    }
                }

                isSaveShowPanel = moveleft;
                isOpenDingleChange = false;
            }
            else if (type == 2)
            {
                if (moveleft)
                {
                    if (LanguageUtils.IsArabic())
                    {
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(0, 0.2f);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(0, 0.2f);
                        view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(870, 0.2f).OnComplete(() =>
                        {
                            view.m_UI_Item_CaptainList.gameObject.SetActive(false);
                        });
                    }
                    else
                    {
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(0, 0.2f);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(0, 0.2f);
                        view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(-870, 0.2f).OnComplete(() =>
                        {
                            view.m_UI_Item_CaptainList.gameObject.SetActive(false);
                        });
                    }
                    isSelectHeroList = false;
                }
                else
                {
                    if (LanguageUtils.IsArabic())
                    {
                        view.m_UI_Item_CaptainList.gameObject.SetActive(true);
                        view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(580, 0.2f);
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(-138, 0.2f);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(-130, 0.2f);
                    }
                    else
                    {
                        view.m_UI_Item_CaptainList.gameObject.SetActive(true);
                        view.m_UI_Item_CaptainList.gameObject.transform.DOLocalMoveX(-580, 0.2f);
                        view.m_UI_Model_Window_Type1.gameObject.transform.DOLocalMoveX(138, 0.2f);
                        view.m_pl_Center_PolygonImage.gameObject.transform.DOLocalMoveX(130, 0.2f);
                    }
                    isSelectHeroList = true;
                }
            }
        }

        #region 加成显示相关

        //更多
        private void OnMore()
        {
            Dictionary<attrType, int> multiDic = new Dictionary<attrType, int>();
            HeroProxy.Hero hreo = m_selectMainHero;
            HeroProxy.Hero deputyHero = m_selectDeputyHero;

            var showType = new List<attrType>();
            showType.Add(attrType.troopsCapacityMulti);
            showType.Add(attrType.infantryAttackMulti);
            showType.Add(attrType.cavalryAttackMulti);
            showType.Add(attrType.bowmenAttackMulti);
            showType.Add(attrType.siegeCarAttackMulti);
            showType.Add(attrType.infantryDefenseMulti);
            showType.Add(attrType.cavalryDefenseMulti);
            showType.Add(attrType.bowmenDefenseMulti);
            showType.Add(attrType.siegeCarDefenseMulti);
            showType.Add(attrType.infantryHpMaxMulti);
            showType.Add(attrType.cavalryHpMaxMulti);
            showType.Add(attrType.bowmenHpMaxMulti);
            showType.Add(attrType.siegeCarHpMaxMulti);
            showType.Add(attrType.marchSpeedMulti);
            showType.Add(attrType.getFoodSpeedMulti);
            showType.Add(attrType.getWoodSpeedMulti);
            showType.Add(attrType.getStoneSpeedMulti);
            showType.Add(attrType.getGlodSpeedMulti);
            showType.Add(attrType.getDiamondSpeedMulti);
            showType.Add(attrType.troopsSpaceMulti);
            showType.Add(attrType.heroExpMulti);
            showType.Add(attrType.barbarianDamageMulti);
            showType.Add(attrType.barbarianVillageDamageMulti);
            showType.Add(attrType.troopsHealthResourcesMulti);

            var playerAttributeProxy =
                AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            long deputyId = 0;
            if (deputyHero != null)
            {
                if (deputyHero.config != null)
                {                 
                    deputyId = deputyHero.config.ID;  
                }
            }

            var attributes = playerAttributeProxy.GetTroopAttribute(hreo.config.ID,deputyId);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < showType.Count; i++)
            {
                var attribute = attributes[(int)showType[i] - 1];
                if (attribute != null && attribute.origvalue > 0)
                {
                    sb.Append($"{LanguageUtils.getTextFormat(182063,LanguageUtils.getText(attribute.define.nameID),attribute.GetShowValue())}");
                    if (i != showType.Count - 1)
                    {
                        sb.Append("\n");
                    }
                }
            }

            HelpTip.CreateTip(sb.ToString(), view.m_UI_Model_MiniButton_White.m_btn_btn_GameButton.transform)
                .SetStyle(HelpTipData.Style.arrowDown).SetOffset(23).Show();
        }

        private void RefreshHeroMultiAttr()
        {
            if (m_selectMainHero == null)
            {
                view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text = "";
                view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = "";
                return;
            }

            attrType attrName1 = attrType.None;
            attrType attrName2 = attrType.None;

            var playerAttributeProxy =
                AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            var attributes = playerAttributeProxy.GetTroopAttribute(m_selectMainHeroId,m_selecttDeputyHeroId);
            MapObjectInfoEntity infoEntity = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            if (m_marchType == TroopAttackType.Collect) //采集
            {
                if (infoEntity != null)
                {
                    if (infoEntity.rssType == RssType.Farmland||
                        infoEntity.rssType== RssType.GuildFoodResCenter)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.getFoodSpeedMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.troopsSpaceMulti - 1].perfValue);

                        attrName1 = attrType.getFoodSpeedMulti;
                        attrName2 = attrType.troopsSpaceMulti;
                    }
                    else if (infoEntity.rssType == RssType.Wood||
                             infoEntity.rssType== RssType.GuildWoodResCenter)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.getWoodSpeedMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.troopsSpaceMulti - 1].perfValue);

                        attrName1 = attrType.getWoodSpeedMulti;
                        attrName2 = attrType.troopsSpaceMulti;
                    }
                    else if (infoEntity.rssType == RssType.Stone||
                             infoEntity.rssType== RssType.GuildGoldResCenter)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.getStoneSpeedMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.troopsSpaceMulti - 1].perfValue);
                        attrName1 = attrType.getStoneSpeedMulti;
                        attrName2 = attrType.troopsSpaceMulti;
                    }
                    else if (infoEntity.rssType == RssType.Gold||
                             infoEntity.rssType== RssType.GuildGemResCenter)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.getGlodSpeedMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.troopsSpaceMulti - 1].perfValue);
                        attrName1 = attrType.getGlodSpeedMulti;
                        attrName2 = attrType.troopsSpaceMulti;
                    }
                    else if (infoEntity.rssType == RssType.Gem)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.getDiamondSpeedMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.troopsSpaceMulti - 1].perfValue);
                        attrName1 = attrType.getDiamondSpeedMulti;
                        attrName2 = attrType.troopsSpaceMulti;
                    }else if (infoEntity.rssType == RssType.Rune)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text =
                            LanguageUtils.getTextFormat(180357, attributes[(int) attrType.troopsCapacityMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.marchSpeedMulti - 1].perfValue);
                        attrName1 = attrType.troopsCapacityMulti;
                        attrName2 = attrType.marchSpeedMulti;
                    }
                }
            }
            else if (m_marchType == TroopAttackType.Attack)
            {
                if (infoEntity != null)
                {
                    if (infoEntity.rssType == RssType.Monster||
                        infoEntity.rssType== RssType.Guardian ||
                        infoEntity.rssType == RssType.SummonAttackMonster)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text =
                            LanguageUtils.getTextFormat(180357, attributes[(int) attrType.heroExpMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.barbarianAttackDamageMulti - 1].perfValue);
                        attrName1 = attrType.heroExpMulti;
                        attrName2 = attrType.barbarianAttackDamageMulti;
                    }
                    else if (infoEntity.rssType == RssType.BarbarianCitadel ||
                        infoEntity.rssType == RssType.SummonConcentrateMonster)
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text =
                            LanguageUtils.getTextFormat(180357, attributes[(int)attrType.heroExpMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int)attrType.barbarianVillageAttackDamageMulti - 1].perfValue);
                        attrName1 = attrType.heroExpMulti;
                        attrName2 = attrType.barbarianVillageAttackDamageMulti;
                    }
                    else
                    {
                        view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text =
                            LanguageUtils.getTextFormat(180357, attributes[(int) attrType.troopsCapacityMulti - 1].perfValue);
                        view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                            attributes[(int) attrType.marchSpeedMulti - 1].perfValue);
                        attrName1 = attrType.troopsCapacityMulti;
                        attrName2 = attrType.marchSpeedMulti;
                    }
                }
            }else 
            {
                view.m_UI_Item_ArmyData1.m_lbl_text_LanguageText.text =
                    LanguageUtils.getTextFormat(180357, attributes[(int) attrType.troopsCapacityMulti - 1].perfValue);
                view.m_UI_Item_ArmyData2.m_lbl_text_LanguageText.text = LanguageUtils.getTextFormat(180357,
                    attributes[(int) attrType.marchSpeedMulti - 1].perfValue);
                attrName1 = attrType.troopsCapacityMulti;
                attrName2 = attrType.marchSpeedMulti;
            }

            AttrInfoDefine define1 = CoreUtils.dataService.QueryRecord<AttrInfoDefine>((int) (attrName1));
            if (define1 != null)
            {
                view.m_UI_Item_ArmyData1.attrInfoDefine = define1;
                ClientUtils.LoadSprite(view.m_UI_Item_ArmyData1.m_img_icon_PolygonImage, define1.icon);
            }

            AttrInfoDefine define2 = CoreUtils.dataService.QueryRecord<AttrInfoDefine>((int) (attrName2));
            if (define1 != null)
            {
                view.m_UI_Item_ArmyData2.attrInfoDefine = define2;
                ClientUtils.LoadSprite(view.m_UI_Item_ArmyData2.m_img_icon_PolygonImage, define2.icon);
            }
        }


        //点击加成
        private void ClickMulti(int num)
        {
            AttrInfoDefine define = null;
            Transform trans = null;
            if (num == 1)
            {
                define = view.m_UI_Item_ArmyData1.attrInfoDefine;
                trans = view.m_UI_Item_ArmyData1.m_img_icon_PolygonImage.transform;
            }
            else
            {
                define = view.m_UI_Item_ArmyData2.attrInfoDefine;
                trans = view.m_UI_Item_ArmyData2.m_img_icon_PolygonImage.transform;
            }

            if (define == null)
            {
                return;
            }

            HelpTip.CreateTip(LanguageUtils.getText(define.nameID), trans).SetStyle(HelpTipData.Style.arrowDown)
                .SetOffset(23).Show();
        }

        #endregion

        private List<SoldiersData> GetAvailableSoldier()
        {
            List<SoldiersData> soldiers = null;
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    soldiers = m_TroopProxy.GetAvailableSoldiers();
                    break;
                case GameModeType.Expedition:
                    var expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
                    if (expeditionProxy != null)
                    {
                        soldiers = expeditionProxy.GetAvailableSoldiers(m_OpenPanelData.ExpeditionTroopIndex);
                    }
                    break;
            }

            return soldiers;
        }   

        private SoldiersData GetSoldiersDataById(int id)
        {
            foreach(var data in m_availableSoldiers)
            {
                if(data.Id == id)
                {
                    return data;
                }
            }
            return null;
        }

        private enum SelectMode
        {
            None,
            MainHero,
            DeputyHero,
        }

        private List<HeroProxy.Hero> GetAvailableSelectHeros(SelectMode selectMode)
        {
            List<HeroProxy.Hero> heros = new List<HeroProxy.Hero>(m_availableHeros);

            switch(selectMode)
            {
                case SelectMode.MainHero:
                    if (m_selectDeputyHero != null)
                    {
                        heros.Remove(m_selectDeputyHero);
                    }
                    break;
                case SelectMode.DeputyHero:
                    if (m_selectMainHero != null)
                    {
                        heros.Remove(m_selectMainHero);
                    }
                    break;
            }
            return heros;
        }

        private void HeroSelectModeChange(SelectMode selectMode)
        {
            if (m_curSelectMode == selectMode) return;
            m_curSelectMode = selectMode;
            m_availableSelectHeros = GetAvailableSelectHeros(m_curSelectMode);
            HeroSortRefresh(m_sortType);
        }
    }
}