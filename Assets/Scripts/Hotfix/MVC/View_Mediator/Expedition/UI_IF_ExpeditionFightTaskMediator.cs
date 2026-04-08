// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月11日
// Update Time         :    2020年6月11日
// Class Description   :    UI_IF_ExpeditionFightTaskMediator
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
using Hotfix;

namespace Game {

    public class ExpeditionFightTaskViewData
    {
        public ExpeditionDefine ExpeditionCfg { get; set; }
        public ExpeditionBattleDefine ExpeditionBattleCfg { get; set; }
    }

    public enum ExpeditionFightTaskViewType
    {
        None,
        Normal,
        Preview,
        Hide,
    }

    public class UI_IF_ExpeditionFightTaskMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_ExpeditionFightTaskMediator";


        #endregion

        //IMediatorPlug needs
        public UI_IF_ExpeditionFightTaskMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_ExpeditionFightTaskView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.CreateExpeditionTroop,
                Expedition_ExpeditionChallenge.TagName,
                CmdConstant.ExpeditionReadyAniEnd,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.CreateExpeditionTroop:
                    int index = (int)notification.Body;
                    CreateTroop(index);
                    break;
                case Expedition_ExpeditionChallenge.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.ExitExpeditionMap);
                            CoreUtils.uiManager.CloseUI(UI.s_expeditionFightTask);

                            ErrorMessage error = (ErrorMessage)notification.Body;
                            Tip.CreateTip(LanguageUtils.getTextFormat(100125, error.errorCode), Tip.TipStyle.Middle).Show();
                        }
                        else
                        {
                            var response = notification.Body as Expedition_ExpeditionChallenge.response;
                            if (response == null) return;
                            OnOpenFightResponse(response);
                        }                            
                    }
                    break;
                case CmdConstant.ExpeditionReadyAniEnd:
                    StartExpedition();
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
            CoreUtils.uiManager.CloseUI(UI.s_battleTroopsTips);
            CoreUtils.uiManager.CloseUI(UI.s_buffList);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_viewData = view.data as ExpeditionFightTaskViewData;
            if (m_viewData == null) return;
            m_expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            m_enemycaptainViewList.Add(view.m_UI_Item_ExpenditionCaptain1);
            m_enemycaptainViewList.Add(view.m_UI_Item_ExpenditionCaptain2);
            m_enemycaptainViewList.Add(view.m_UI_Item_ExpenditionCaptain3);
            m_enemycaptainViewList.Add(view.m_UI_Item_ExpenditionCaptain4);
            m_enemycaptainViewList.Add(view.m_UI_Item_ExpenditionCaptain5);

            m_troopViewList.Add(view.m_UI_Item_ExpeditionFightList1);
            m_troopViewList.Add(view.m_UI_Item_ExpeditionFightList2);
            m_troopViewList.Add(view.m_UI_Item_ExpeditionFightList3);
            m_troopViewList.Add(view.m_UI_Item_ExpeditionFightList4);
            m_troopViewList.Add(view.m_UI_Item_ExpeditionFightList5);
            SwitchViewType(ExpeditionFightTaskViewType.Normal);
            RefreshUI();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Interface.AddClickEvent(OnClickedBack);
            view.m_btn_start1.AddClickEvent(OnClickedStart);
            view.m_btn_preview.AddClickEvent(OnClickedPreview);
            view.m_btn_info_GameButton.onClick.AddListener(OnClickedRule);
            view.m_btn_start2.AddClickEvent(OnClickedStart);
        }

        protected override void BindUIData()
        {

        }

        public override bool onMenuBackCallback()
        {
            OnClickedBack();
            return true;
        }

        #endregion

        private void OnClickedStart()
        {
            if (m_expeditionProxy.ExpeditionStatus == ExpeditionFightStatus.WatingForStart) return;
            m_expeditionProxy.ExpeditionStatus = ExpeditionFightStatus.WatingForStart;
            if (m_expeditionProxy.GetPlayerTroopCount() == 0) return;
            CoreUtils.uiManager.CloseUI(UI.s_createAnmy);
            SwitchViewType(ExpeditionFightTaskViewType.Hide);
            CoreUtils.uiManager.ShowUI(UI.s_expeditionFightReady);

            SummonerTroopMgr.Instance.ExpeditionTroop.Clear(); 
        }

        private void StartExpedition()
        {            
            var allPlayerTroop = m_expeditionProxy.GetAllPlayerTroopData();
            if (allPlayerTroop.Count == 0) return;

            Expedition_ExpeditionChallenge.request request = new Expedition_ExpeditionChallenge.request();
            request.troops = new List<Expedition_ExpeditionChallenge.request.Troops>();
            foreach(var data in allPlayerTroop)
            {
                request.troops.Add(new Expedition_ExpeditionChallenge.request.Troops()
                {
                    mainHeroId = data.MainHeroId,
                    deputyHeroId = data.DeputyHeroId,
                    soldiers = data.Soldiers,
                    armyIndex = data.Index
                });
            }
            request.id = m_viewData.ExpeditionCfg.ID;
            AppFacade.GetInstance().SendSproto(request);
            m_expeditionProxy.CachePlayerTroopData(allPlayerTroop);
        }

        private void OnClickedBack()
        {
            if (m_expeditionProxy.ExpeditionStatus == ExpeditionFightStatus.WatingForStart) return;
            switch (m_expeditionProxy.ExpeditionStatus)
            {
                case ExpeditionFightStatus.Fightting:
                    CoreUtils.uiManager.CloseUI(UI.s_expeditionFightTask);
                    AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionTaskUICloseWhenFightting);
                    break;
                case ExpeditionFightStatus.PrepareNormal:                   
                    Alert.CreateAlert(805010).SetLeftButton().SetRightButton(() =>
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ExitExpeditionMap);
                        CoreUtils.uiManager.CloseUI(UI.s_expeditionFightTask);
                        
                    }).Show();
                    break;
                case ExpeditionFightStatus.PreparePreview:
                    m_expeditionProxy.ExpeditionStatus = ExpeditionFightStatus.PrepareNormal;
                    SwitchViewType(ExpeditionFightTaskViewType.Normal);
                    break;
            }            
        }

        private void OnClickedRule()
        {
            HelpTip.CreateTip(6000, view.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }

        private void OnClickedPreview()
        {
            m_expeditionProxy.ExpeditionStatus = ExpeditionFightStatus.PreparePreview;
            SwitchViewType(ExpeditionFightTaskViewType.Preview);
        }

        private void RefreshUI()
        {
            RefreshStarRequire();
            RefreshEnemyCaptain();
            InitTroopListView();
            RefreshBuff();
            if(m_expeditionProxy.ExpeditionStatus == ExpeditionFightStatus.Fightting)
            {
                view.m_btn_preview.gameObject.SetActive(false);
                view.m_btn_start1.m_btn_languageButton_GameButton.interactable = false;
                view.m_btn_start2.m_btn_languageButton_GameButton.interactable = false;
            }
        }

        private void RefreshEnemyCaptain()
        {
            var allMonsterTroopData = m_expeditionProxy.GetAllMonsterTroopData();
            for(int i= 0;i < allMonsterTroopData.Count && i < m_enemycaptainViewList.Count; ++i)
            {
                int index = allMonsterTroopData[i].Index;
                int uiIndex = i;
                m_enemycaptainViewList[i].Show(allMonsterTroopData[i].TroopsCfg.heroID1, allMonsterTroopData[i].TroopsCfg.heroLevel1, ()=>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_battleTroopsTips, null, new BattleTroopsTipsData()
                    {
                        ScreenPosition = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(m_enemycaptainViewList[uiIndex].m_root_RectTransform.position),
                        EnemyTroopIndex = index,
                        Offset = new Vector2(m_enemycaptainViewList[uiIndex].m_UI_Model_CaptainHead.m_root_RectTransform.sizeDelta.x / 2 *
                        m_enemycaptainViewList[uiIndex].m_UI_Model_CaptainHead.m_root_RectTransform.localScale.x, 0),
                    });
                });
            }
            for(int i = allMonsterTroopData.Count; i < m_enemycaptainViewList.Count; ++i)
            {
                m_enemycaptainViewList[i].gameObject.SetActive(false);
            }
        }

        private void RefreshStarRequire()
        {
            view.m_UI_Item_ExpeditionFightTask.Show(m_viewData.ExpeditionCfg);
        }

        private void RefreshBuff()
        {
            var objCityBuff = view.m_UI_Item_MainIFBuff.gameObject;
            view.m_UI_Item_MainIFBuff.gameObject.SetActive(false);
            Dictionary<int, CityBuff> cityBuffDic = new Dictionary<int, CityBuff>();
            List<CityBuff> cityBuffList = new List<CityBuff>();

            if (m_playerProxy.CurrentRoleInfo.cityBuff != null)
            {
                foreach(var cityBuff in m_playerProxy.CurrentRoleInfo.cityBuff)
                {
                    if (cityBuff.Value.expiredTime != -2)
                    {
                        CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.Value.id);
                        if (cityBuffDefine != null)
                        {
                            if (!cityBuffDic.ContainsKey(cityBuffDefine.group))
                            {
                                cityBuffDic.Add(cityBuffDefine.group, cityBuff.Value);
                            }
                        }
                    }
                }
                cityBuffList.AddRange(cityBuffDic.Values);
                cityBuffList.Sort((x, y) => (int)(x.id - y.id));
            }

            foreach(var cityBuff in cityBuffList)
            {
                if (cityBuff.expiredTime != -2)
                {
                    CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.id);
                    CityBuffGroupDefine cityBuffGroupDefine = CoreUtils.dataService.QueryRecord<CityBuffGroupDefine>((int)cityBuffDefine.group);
                    if (cityBuffDefine != null && cityBuffGroupDefine != null)
                    {
                        GameObject obj = CoreUtils.assetService.Instantiate(objCityBuff);
                        obj.transform.SetParent(view.m_pl_buff_GridLayoutGroup.transform);
                        obj.transform.localPosition = Vector3.zero;
                        obj.transform.localScale = Vector3.one;
                        obj.gameObject.SetActive(true);
                        obj.name = cityBuffDefine.ID.ToString();
                        UI_Item_MainIFBuff_SubView SubView = new UI_Item_MainIFBuff_SubView(obj.GetComponent<RectTransform>());
                        SubView.SetCityBuffId((int)cityBuff.id);
                        SubView.SetIcon(cityBuffDefine.icon);
                        SubView.AddBtnListener(() =>
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_buffList, null, obj.transform.position);
                        });
                    }
                }
            }
        }

        private void InitTroopListView()
        {
            if(m_expeditionProxy.ExpeditionStatus == ExpeditionFightStatus.PrepareNormal)
            {
                var playerTroopDataCache = m_expeditionProxy.GetPlayeyTroopDataCache();
                if (playerTroopDataCache.Count > 0)
                {
                    for (int i = 0; i < m_viewData.ExpeditionCfg.troopsNumber && i < playerTroopDataCache.Count; ++i)
                    {
                        if(playerTroopDataCache[i].Index <= m_viewData.ExpeditionCfg.troopsNumber)
                        {
                            m_expeditionProxy.AddPlayerTroop(playerTroopDataCache[i].Index, playerTroopDataCache[i].MainHeroId,
                                playerTroopDataCache[i].DeputyHeroId, playerTroopDataCache[i].Soldiers, true);
                        }
                    }
                }
            }

            RefreshCreatedTroopNum();
            ShowTroopViewSelected(0);

            for (int i = 0; i < m_viewData.ExpeditionCfg.troopsNumber && i < m_troopViewList.Count; ++i)
            {
                int index = i;
                m_troopViewList[i].SetViewType(FightTroopViewType.Normal);

                switch (m_expeditionProxy.ExpeditionStatus)
                {
                    case ExpeditionFightStatus.Fightting:
                        {
                            var troopData = m_expeditionProxy.GetPlayerTroopData(i + 1);
                            if (troopData != null)
                            {
                                m_troopViewList[i].SetTroop(m_heroProxy.GetHeroByID(troopData.MainHeroId));
                            }
                            m_troopViewList[i].m_img_normal_GameButton.interactable = false;
                            m_troopViewList[i].m_btn_reduce_GameButton.gameObject.SetActive(false);
                            m_troopViewList[i].m_img_select_PolygonImage.gameObject.SetActive(false);
                        }
                        break;
                    case ExpeditionFightStatus.PrepareNormal:
                        {
                            CreateTroop(i + 1);
                            m_troopViewList[i].AddAddListener(() =>
                            {
                                if (m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PrepareNormal) return;
                                FightHelper.Instance.OpenCreateArmyPanel(new OpenPanelData(0, OpenPanelType.Common)
                                {
                                    ExpeditionTroopIndex = index + 1,
                                });
                            });
                            m_troopViewList[i].AddRemoveListener(() =>
                            {
                                if (m_expeditionProxy.ExpeditionStatus != ExpeditionFightStatus.PrepareNormal) return;
                                m_expeditionProxy.RemovePlayerTroop(index + 1);
                                SummonerTroopMgr.Instance.ExpeditionTroop.DestroyPreviewPlayerFormation(index + 1);
                                m_troopViewList[index].RemoveTroop();
                                ShowTroopViewSelected(GetCurSelectTroopViewIndex());
                                RefreshCreatedTroopNum();
                                AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionPrepareTroopChanage, index + 1);
                            });
                            break;
                        }                        
                }              
            }

            for(int i = m_viewData.ExpeditionCfg.troopsNumber; i < m_troopViewList.Count; ++i)
            {
                m_troopViewList[i].SetViewType(FightTroopViewType.Lock); 
            }
        }

        private void CreateTroop(int index)
        {
            var troopData = m_expeditionProxy.GetPlayerTroopData(index);
            if (troopData == null) return;
            m_troopViewList[index - 1].SetTroop(m_heroProxy.GetHeroByID(troopData.MainHeroId));
            SummonerTroopMgr.Instance.ExpeditionTroop.CreatePreviewPlayerFormation(troopData);
            ShowTroopViewSelected(GetCurSelectTroopViewIndex());
            RefreshCreatedTroopNum();
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionPrepareTroopChanage, index );
        }

        private int GetCurSelectTroopViewIndex()
        {
            int nextTroopViewIndex = 0;
            while(nextTroopViewIndex < m_viewData.ExpeditionCfg.troopsNumber)
            {
                if (m_expeditionProxy.GetPlayerTroopData(nextTroopViewIndex + 1) == null)
                {
                    break;
                }
                ++nextTroopViewIndex;
            }          
            return nextTroopViewIndex;
        }

        private void RefreshCreatedTroopNum()
        {
            view.m_lbl_tropNum_LanguageText.text = LanguageUtils.getTextFormat(181104, m_expeditionProxy.GetPlayerTroopCount(), m_viewData.ExpeditionCfg.troopsNumber);
            view.m_btn_start1.m_img_forbid_PolygonImage.gameObject.SetActive(m_expeditionProxy.GetPlayerTroopCount() == 0);
            view.m_btn_start1.m_btn_languageButton_GameButton.interactable = m_expeditionProxy.GetPlayerTroopCount() > 0;

            view.m_btn_start2.m_img_forbid_PolygonImage.gameObject.SetActive(m_expeditionProxy.GetPlayerTroopCount() == 0);
            view.m_btn_start2.m_btn_languageButton_GameButton.interactable = m_expeditionProxy.GetPlayerTroopCount() > 0;
            UpdatePowerTip();
        }

        private void ShowTroopViewSelected(int index)
        {
            m_troopViewList[m_curTroopViewIndex].SetSelected(false);
            m_curTroopViewIndex = 0;
            if (index >= m_troopViewList.Count || index >= m_viewData.ExpeditionCfg.troopsNumber) return;
            m_troopViewList[index].SetSelected(true);
            m_curTroopViewIndex = index;
        }


        private void OnOpenFightResponse(Expedition_ExpeditionChallenge.response response)
        {            
            m_expeditionProxy.SetFightEndTime(response.endTime);
            var allEnemyTroops = m_expeditionProxy.GetAllMonsterTroopData();
            foreach (var data in allEnemyTroops)
            {
                SummonerTroopMgr.Instance.ExpeditionTroop.CreateEnemyArmyData(data);
            }

            var allPlayerTroops = m_expeditionProxy.GetAllPlayerTroopData();
            foreach (var data in allPlayerTroops)
            {
                SummonerTroopMgr.Instance.ExpeditionTroop.CreatePlayerArmyData(data);
            }
            
            CoreUtils.uiManager.ShowUI(UI.s_expeditionFightWar, null, new ExpeditionFightWarViewData()
            {
                ExpeditionCfg = m_viewData.ExpeditionCfg,
                ExpeditionBattleCfg = m_viewData.ExpeditionBattleCfg,
            });
            CoreUtils.uiManager.CloseUI(UI.s_expeditionFightTask);
            m_expeditionProxy.ExpeditionStatus = ExpeditionFightStatus.Fightting;
            AppFacade.GetInstance().SendNotification(CmdConstant.ExpeditionFightStart);
        }

        private void SwitchViewType(ExpeditionFightTaskViewType targetType)
        {
            if (m_curViewType == targetType) return;
            m_curViewType = targetType;
            switch (m_curViewType)
            {
                case ExpeditionFightTaskViewType.Normal:
                    ClientUtils.PlayUIAnimation(view.m_pl_preview_Animator, "Hide");
                    view.m_pl_buff_GridLayoutGroup.gameObject.SetActive(false);
                    ClientUtils.PlayUIAnimation(view.m_pl_side_Animator, "Show");
                    ClientUtils.PlayUIAnimation(view.m_pl_center_Animator, "Show");
                    view.m_img_mask_PolygonImage.gameObject.SetActive(true);
                    break;
                case ExpeditionFightTaskViewType.Preview:
                    view.m_pl_buff_GridLayoutGroup.gameObject.SetActive(true);
                    ClientUtils.PlayUIAnimation(view.m_pl_preview_Animator, "Show");
                    ClientUtils.PlayUIAnimation(view.m_pl_side_Animator, "Hide");
                    ClientUtils.PlayUIAnimation(view.m_pl_center_Animator, "Hide");
                    view.m_img_mask_PolygonImage.gameObject.SetActive(false);
                    break;
                case ExpeditionFightTaskViewType.Hide:
                    view.m_pl_buff_GridLayoutGroup.gameObject.SetActive(false);
                    ClientUtils.PlayUIAnimation(view.m_pl_preview_Animator, "Hide");
                    ClientUtils.PlayUIAnimation(view.m_pl_side_Animator, "Hide");
                    ClientUtils.PlayUIAnimation(view.m_pl_center_Animator, "Hide");
                    view.m_img_mask_PolygonImage.gameObject.SetActive(false);
                    break;
            }            
        }

        private void UpdatePowerTip()
        {
            var readySoldiers = m_expeditionProxy.GetReadySoldiers();
            long soliderPower = TroopProxy.GetFightingCount(readySoldiers);
            view.m_lbl_tip_LanguageText.text = LanguageUtils.getText(FightHelper.Instance.GetFightingDifficultTip((int)soliderPower, GetMonsterTroopPower()));
        }

        private int GetMonsterTroopPower()
        {
            int power = 0;
            var allMonsterTroopData = m_expeditionProxy.GetAllMonsterTroopData();
            foreach(var data in allMonsterTroopData)
            {
                power += data.MonsterCfg.powerAdvise;
            }
            return power;
        }

        private int m_curTroopViewIndex = 0;
        private ExpeditionFightTaskViewData m_viewData = null;
        private ExpeditionProxy m_expeditionProxy = null;
        private HeroProxy m_heroProxy = null;
        private PlayerProxy m_playerProxy = null;
        private List<UI_Item_ExpenditionCaptain_SubView> m_enemycaptainViewList = new List<UI_Item_ExpenditionCaptain_SubView>();
        private List<UI_Item_ExpeditionFightList_SubView> m_troopViewList = new List<UI_Item_ExpeditionFightList_SubView>();
        private ExpeditionFightTaskViewType m_curViewType = ExpeditionFightTaskViewType.None;
    }
}