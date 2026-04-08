// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Pop_ScoutSelectMediator
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
using UnityEngine.UI;
using Data;
using Hotfix;

namespace Game
{
    public class UI_Pop_ScoutSelectMediator : GameMediator
    {
        public class Param
        {
            public Vector2 pos;
            public long targetIndex;
        }

        public class ScoutTimeSort
        {
            public int Index;
            public int Times;
        }

        #region Member

        public static string NameMediator = "UI_Pop_ScoutSelectMediator";
        private ScoutProxy m_scoutProxy;
        private TroopProxy m_troopProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private RssProxy m_RssProxy;

        private Param m_Param;
        private Timer m_timer = null;
        private Int64 m_endTime;
        private ScoutProxy.ScoutInfo scoutInfo;

        private List<UI_Item_ScoutQueueHead_SubView> m_queueHeadList = new List<UI_Item_ScoutQueueHead_SubView>();
        private Dictionary<int, int> m_mapKey = new Dictionary<int, int>();
        private int m_selectIndex = 0;

        private int m_marchSpeed;  //斥候行军速度

        private ScoutProxy.ScoutState m_lastStatus;
        private FogSystemMediator m_fogMedia;

        #endregion

        //IMediatorPlug needs
        public UI_Pop_ScoutSelectMediator(object viewComponent) : base(NameMediator, viewComponent)
        {

        }


        public UI_Pop_ScoutSelectView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.ScoutQueueUpdate,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ScoutQueueUpdate: //队列数据更新
                    Int64 scoutIndex = (Int64)notification.Body;
                    ScoutProxy.ScoutInfo info = m_scoutProxy.GetSoutInfoById(scoutIndex);
                    RefreshHeadStatus(info);
                    if (scoutInfo != null && scoutInfo.id == info.id)
                    {
                        RefreshTipStatus();
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

        public override void OnRemove()
        {
            base.OnRemove();
            OnRemoveDrawLine();
            if (m_Param.targetIndex == 0)
            {
                WarFogMgr.RemoveFadeGroupByType(FogSystemMediator.FADE_TYPE_CLICK);
            }
        }

        public override void WinClose()
        {
            //CoreUtils.uiManager.ShowUI(UI.s_mainInterface);
            CloseSelectScoutTargetLine();
            long index = -1;
            if (m_scoutProxy != null)
            {
                m_scoutProxy.SetCurrSelectIndex(index);
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.SetSelectScoutHead, index);
            OnRemoveDrawLine();
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            m_Param = (Param)view.data;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_RssProxy=AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;

            m_marchSpeed = m_scoutProxy.GetScoutSpeed();

            m_fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Item_ScoutQueueHead1.AddClickEvent(() => { OnScoutSelected(0); });
            view.m_UI_Item_ScoutQueueHead2.AddClickEvent(() => { OnScoutSelected(1); });
            view.m_UI_Item_ScoutQueueHead3.AddClickEvent(() => { OnScoutSelected(2); });

            view.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(OnSendClick);
        }

        protected override void BindUIData()
        {
            int num = m_scoutProxy.GetScoutNum();
            view.m_UI_Item_ScoutQueueHead1.gameObject.SetActive(num > 0);
            view.m_UI_Item_ScoutQueueHead2.gameObject.SetActive(num > 1);
            view.m_UI_Item_ScoutQueueHead3.gameObject.SetActive(num > 2);

            view.m_UI_Item_ScoutQueueHead1.m_img_select_PolygonImage.gameObject.SetActive(false);
            view.m_UI_Item_ScoutQueueHead2.m_img_select_PolygonImage.gameObject.SetActive(false);
            view.m_UI_Item_ScoutQueueHead3.m_img_select_PolygonImage.gameObject.SetActive(false);

            m_queueHeadList.Add(view.m_UI_Item_ScoutQueueHead1);
            m_queueHeadList.Add(view.m_UI_Item_ScoutQueueHead2);
            m_queueHeadList.Add(view.m_UI_Item_ScoutQueueHead3);

            if (num > 0)
            {
                view.m_UI_Item_ScoutQueueHead1.SetScoutInfo(m_scoutProxy.GetScoutInfo(0));
                m_mapKey[m_scoutProxy.GetScoutInfo(0).id] = 0;
            }

            if (num > 1)
            {
                view.m_UI_Item_ScoutQueueHead2.SetScoutInfo(m_scoutProxy.GetScoutInfo(1));
                m_mapKey[m_scoutProxy.GetScoutInfo(1).id] = 1;
            }

            if (num > 2)
            {
                view.m_UI_Item_ScoutQueueHead3.SetScoutInfo(m_scoutProxy.GetScoutInfo(2));
                m_mapKey[m_scoutProxy.GetScoutInfo(2).id] = 2;
            }

            view.m_pl_Tip_Animator.gameObject.SetActive(false);

            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_ScoutQueueHead3.m_root_RectTransform.parent as RectTransform);

            view.m_UI_Model_StandardButton_Yellow.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500514);

            List<int> findList = new List<int>();
            for (int i = 0; i < num; i++)
            {
                ScoutProxy.ScoutInfo info = m_scoutProxy.GetScoutInfo(i);
                if (info != null)
                {
                    if (info.state == ScoutProxy.ScoutState.None || 
                        info.state == ScoutProxy.ScoutState.Back_City ||
                        info.state == ScoutProxy.ScoutState.Return)
                    {
                        findList.Add(i);
                    }
                }
            }
            int findIndex = 0;
            if (findList.Count > 0)
            {
                findIndex = GetNearestIndex(findList);
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    findList.Add(i);
                }
                findIndex = GetNearestIndex(findList);
            }
            OnScoutSelected(findIndex);

            if (m_Param.targetIndex == 0)
            {
                var groupId = m_fogMedia.GetFadeGroupId(m_Param.pos.x, m_Param.pos.y, WarFogMgr.GROUP_SIZE);
                WarFogMgr.CreateFadeGroup(groupId, FogSystemMediator.FADE_TYPE_CLICK, WarFogMgr.GROUP_SIZE);
            }
        }

        private int GetNearestIndex(List<int> findList)
        {
            int count = findList.Count;
            if (count == 1)
            {
                return findList[0];
            }
            List<ScoutTimeSort> sortList = new List<ScoutTimeSort>();
            for (int i = 0; i < findList.Count; i++)
            {
                ScoutTimeSort timeSort = new ScoutTimeSort();
                timeSort.Index = findList[i];
                timeSort.Times = GetMarchTime(findList[i]);
                sortList.Add(timeSort);
            }
            sortList.Sort(SortBytime);
            return sortList[0].Index;
        }

        private int SortBytime(ScoutTimeSort obj1, ScoutTimeSort obj2)
        {
            int re = obj1.Times.CompareTo(obj2.Times);
            return re;
        }

        private int GetMarchTime(int index)
        {
            int times = 0;
            ScoutProxy.ScoutInfo scoutInfo = m_scoutProxy.GetScoutInfo(index);
            if (scoutInfo == null)
            {
                return 0;
            }
            if (scoutInfo.state == ScoutProxy.ScoutState.None)
            {
                Vector2 startPos = new Vector2(m_cityBuildingProxy.RolePos.x * 100, m_cityBuildingProxy.RolePos.y * 100);
                Vector2 targetPos = new Vector2(m_Param.pos.x * 100, m_Param.pos.y * 100);
                //城堡坐标矫正
                startPos = TroopHelp.CalOutCityPos(startPos, targetPos);
                times = (int)(TroopHelp.GetDistance(startPos, targetPos) / m_marchSpeed);
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Back_City || scoutInfo.state == ScoutProxy.ScoutState.Return ||
                     scoutInfo.state == ScoutProxy.ScoutState.Scouting || scoutInfo.state == ScoutProxy.ScoutState.Surveing ||
                     scoutInfo.state == ScoutProxy.ScoutState.Fog)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId((int)scoutInfo.id);
                if (armyData != null)
                {
                    float distance = 0;
                    Vector2 targetPos = new Vector2(m_Param.pos.x * 100, m_Param.pos.y * 100);
                    Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                    if (formation != null)
                    {
                        Vector2 startPos = new Vector2((int)Mathf.Floor(formation.gameObject.transform.position.x) * 100,
                                                  (int)Mathf.Floor(formation.gameObject.transform.position.z) * 100);

                        distance = TroopHelp.GetDistance(startPos, targetPos);
                        times = (int)(distance / m_marchSpeed);
                    }
                    else
                    {
                        Vector2 startPos = armyData.GetMovePos();
                        startPos.Set(startPos.x * 100, startPos.y * 100);
                        distance = TroopHelp.GetDistance(startPos, targetPos);
                        times = (int)(distance / m_marchSpeed);
                    }
                }
            }

            return times;
        }

        private void OnScoutSelected(int index)
        {
            m_queueHeadList[m_selectIndex].m_img_select_PolygonImage.gameObject.SetActive(false);
            m_queueHeadList[index].m_img_select_PolygonImage.gameObject.SetActive(true);
            m_selectIndex = index;
            view.m_pl_Tip_Animator.gameObject.SetActive(true);
            scoutInfo = m_scoutProxy.GetScoutInfo(index);
            var pos = view.m_pl_Tip_Animator.transform.position;
            CancelTimer();

            float headPosY = 0;
            if (index == 0)
            {
                headPosY = view.m_UI_Item_ScoutQueueHead1.m_root_RectTransform.position.y;
            }
            else if (index == 1)
            {
                headPosY = view.m_UI_Item_ScoutQueueHead2.m_root_RectTransform.position.y;
            }
            else if (index == 2)
            {
                headPosY = view.m_UI_Item_ScoutQueueHead3.m_root_RectTransform.position.y;
            }

            //Debug.Log("Y:" + headPosY.ToString());
            pos.y = headPosY+0.15f;

            view.m_pl_Tip_Animator.transform.position = pos;

            RefreshTipStatus();

            m_scoutProxy.SetCurrSelectIndex(scoutInfo.id);

            AppFacade.GetInstance().SendNotification(CmdConstant.SetSelectScoutHead, m_scoutProxy.GetCurrSelectIndex());

        }

        private void RefreshHeadStatus(ScoutProxy.ScoutInfo info)
        {
            m_queueHeadList[m_mapKey[info.id]].SetScoutInfo(info);
        }

        private void RefreshTipStatus()
        {
            if (scoutInfo == null)
            {
                return;
            }

            bool isOpenTimer = false;
            bool isNeedCost = true;
            m_lastStatus = scoutInfo.state;
            if (scoutInfo.state == ScoutProxy.ScoutState.None)
            {
                view.m_lbl_state_LanguageText.text = LanguageUtils.getText(181151);
                view.m_lbl_state_val_LanguageText.gameObject.SetActive(false);
                view.m_lbl_time_LanguageText.gameObject.SetActive(true);

                Vector2 startPos = new Vector2(m_cityBuildingProxy.RolePos.x * 100, m_cityBuildingProxy.RolePos.y * 100);
                Vector2 targetPos = new Vector2(m_Param.pos.x * 100, m_Param.pos.y * 100);

                //城堡坐标矫正
                startPos = TroopHelp.CalOutCityPos(startPos, targetPos);

                int offset = (int)(TroopHelp.GetDistance(startPos, targetPos) / m_marchSpeed);
                view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(offset);
                OnCreateDrawLine();
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Fog)
            {
                view.m_lbl_state_LanguageText.text = LanguageUtils.getText(181150);
                view.m_lbl_state_val_LanguageText.gameObject.SetActive(true);
                view.m_lbl_time_LanguageText.gameObject.SetActive(true);

                isNeedCost = false;
                m_endTime = scoutInfo.endTime;
                isOpenTimer = true;
                UpdateTime();
                OnCreateDrawLine();
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Scouting)//侦察
            {
                view.m_lbl_state_LanguageText.text = LanguageUtils.getText(181153);
                ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId((int)scoutInfo.id);
                MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(armyData.targetId);
                if (mapObjectInfoEntity != null)
                {
                    if (mapObjectInfoEntity.rssType == RssType.Cave)
                    {
                        view.m_lbl_state_LanguageText.text = LanguageUtils.getText(181162);
                    }
                }
                view.m_lbl_state_val_LanguageText.gameObject.SetActive(true);
                view.m_lbl_time_LanguageText.gameObject.SetActive(true);
                
                m_endTime = scoutInfo.endTime;
                isOpenTimer = true;
                UpdateTime();
                OnCreateDrawLine();
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Surveing)//调查
            {
                view.m_lbl_state_LanguageText.text = LanguageUtils.getText(181153);
                view.m_lbl_state_val_LanguageText.gameObject.SetActive(false);
                view.m_lbl_time_LanguageText.gameObject.SetActive(true);

                m_endTime = scoutInfo.endTime;
                isOpenTimer = true;
                UpdateTime();
                OnCreateDrawLine();
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Return)//探索完成返回
            {
                view.m_lbl_state_LanguageText.text = LanguageUtils.getText(181152);
                view.m_lbl_state_val_LanguageText.gameObject.SetActive(true);
                view.m_lbl_time_LanguageText.gameObject.SetActive(true);

                m_endTime = scoutInfo.endTime;
                isOpenTimer = true;
                UpdateTime();
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Back_City)//中途返回
            {
                view.m_lbl_state_LanguageText.text = LanguageUtils.getText(181152);
                view.m_lbl_state_val_LanguageText.gameObject.SetActive(true);
                view.m_lbl_time_LanguageText.gameObject.SetActive(true);

                m_endTime = scoutInfo.endTime;
                isOpenTimer = true;
                UpdateTime();
            }

            if (scoutInfo.state == ScoutProxy.ScoutState.None)
            {
                CloseSelectScoutTargetLine();
            }
            else
            {
                ShowSelectedScoutTargetLine(scoutInfo.id,m_Param.pos);
            }
            
            if (isNeedCost)
            {
                ScoutProxy.ScoutCost scoutCostInfo = m_scoutProxy.GetScoutCostDefine((int) m_Param.targetIndex);
                if (scoutCostInfo != null)
                {
                    view.m_UI_Model_StandardButton_Yellow.SetResVisible(true);
                    var currencyDefine = CoreUtils.dataService.QueryRecord<CurrencyDefine>(scoutCostInfo.currencyId);
                    view.m_UI_Model_StandardButton_Yellow.SetResInfo(currencyDefine.iconID,scoutCostInfo.number);
                    
                    var playProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                    int ownCount = (int)playProxy.GetResNumByType(scoutCostInfo.currencyId);
                    if (ownCount >= scoutCostInfo.number)
                    {
                        view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.color = RS.white;
                    }
                    else
                    {
                        view.m_UI_Model_StandardButton_Yellow.m_lbl_line2_LanguageText.color = RS.red;
                    }
                }
                else
                    view.m_UI_Model_StandardButton_Yellow.SetResVisible(false);
            }
            else
            {
                view.m_UI_Model_StandardButton_Yellow.SetResVisible(false); 
            }

            if (isOpenTimer)
            {
                if (m_timer == null)
                {
                    m_timer = Timer.Register(1.0f, UpdateTime, null, true, false, view.m_lbl_time_LanguageText);
                }
            }
            else
            {
                CancelTimer();
            }
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void UpdateTime()
        {
            if (m_lastStatus != scoutInfo.state) //状态变更刷新
            {
                //Debug.LogError("状态变更");
                RefreshTipStatus();
                RefreshHeadStatus(scoutInfo);
                return;
            }
            int offset = 0;
            float distance = 0;
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId((int)scoutInfo.id);
            if (armyData != null)
            {
                Vector2 targetPos = new Vector2(m_Param.pos.x * 100, m_Param.pos.y * 100);
                Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                if (formation != null)
                {
                    Vector2 startPos = new Vector2((int)Mathf.Floor(formation.gameObject.transform.position.x) * 100,
                                              (int)Mathf.Floor(formation.gameObject.transform.position.z) * 100);

                    distance = TroopHelp.GetDistance(startPos, targetPos);
                    offset = (int)(distance / m_marchSpeed);
                }
                else
                {
                    Vector2 startPos = armyData.GetMovePos();
                    startPos.Set(startPos.x*100, startPos.y * 100);
                    distance = TroopHelp.GetDistance(startPos, targetPos);
                    offset = (int)(distance / m_marchSpeed);
                }
            }
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)offset);


            if (scoutInfo.state == ScoutProxy.ScoutState.Return || scoutInfo.state == ScoutProxy.ScoutState.Back_City)
            {
                //返回
                Int64 diffTime = m_endTime - ServerTimeModule.Instance.GetServerTime();
                if (diffTime >= 0)
                {
                    view.m_lbl_state_val_LanguageText.text = ClientUtils.FormatCountDown((int)diffTime);
                }
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Fog) 
            {
                ///探险中 刷新探险进度
                if (scoutInfo.posInfos.Count > 1)
                {
                    List<Vector2Int> tlist = m_fogMedia.GetGroupFog(scoutInfo.posInfos[1].x/100, scoutInfo.posInfos[1].y/100);
                    int val = 25 - tlist.Count;
                    val = val < 0 ? 0 : val;
                    int pro = Mathf.FloorToInt((float)val / 25 * 100);
                    view.m_lbl_state_val_LanguageText.text = LanguageUtils.getTextFormat(180357, pro);
                }
            }
            else if (scoutInfo.state == ScoutProxy.ScoutState.Scouting) 
            {
                //侦察
                Int64 diffTime = m_endTime - ServerTimeModule.Instance.GetServerTime();
                if (diffTime >= 0)
                {
                    view.m_lbl_state_val_LanguageText.text = ClientUtils.FormatCountDown((int)diffTime);
                }
            }
        }

        #region 画白线
        private Vector2 lastV2;
        private int lastScoutId=10;
        private Timer drawLineTime;

        private void OnRemoveDrawLine()
        {
            if (drawLineTime != null)
            {
                drawLineTime.Cancel();
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveDrawLineCity);
        }

        private void OnCreateDrawLine()
        {
            OnRemoveDrawLine();
            OnDrawLine();
            drawLineTime = Timer.Register(1, () =>
            {
                OnDrawLine();
            }, null, true);
        }

        private void OnDrawLine()
        {
            Vector2 pos = m_RssProxy.GetV2((int)m_Param.targetIndex);
            if (pos == Vector2.zero)
            {
                pos = m_Param.pos;
            }

            if (pos != lastV2|| m_selectIndex!=lastScoutId)
            {
                DrawLineInfo drawLineInfo1 = new DrawLineInfo();
                drawLineInfo1.targetPos = pos;
                AppFacade.GetInstance().SendNotification(CmdConstant.MapDrawLineCity, drawLineInfo1);
                lastV2 = pos;
                lastScoutId = m_selectIndex;
            }
        }
        #endregion
        private void OnSendClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_scoutSelectMenu);
            
            if (m_Param.targetIndex > 0)
            {
                ScoutProxy.ScoutCost scoutCostInfo = m_scoutProxy.GetScoutCostDefine((int) m_Param.targetIndex);
                if (scoutCostInfo != null)
                {
                    if (scoutCostInfo.currencyId > 0)
                    {
                        var playProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                        var currencyDefine = CoreUtils.dataService.QueryRecord<CurrencyDefine>(scoutCostInfo.currencyId);
                        int ownCount = (int)playProxy.GetResNumByType(scoutCostInfo.currencyId);
                        if (ownCount >= scoutCostInfo.number)
                        {
                            WarFogMgr.RemoveFadeGroupByType(FogSystemMediator.FADE_TYPE_CLICK);
                            m_scoutProxy.SearPos(scoutInfo.id, (int) m_Param.pos.x, (int) m_Param.pos.y, m_Param.targetIndex);
                        }
                        else
                        {
                            long food = scoutCostInfo.currencyId == 100 ? scoutCostInfo.number : 0;
                            long wood = scoutCostInfo.currencyId == 101 ? scoutCostInfo.number : 0;
                            long stone = scoutCostInfo.currencyId == 102 ? scoutCostInfo.number : 0;
                            long gold = scoutCostInfo.currencyId == 103 ? scoutCostInfo.number : 0;
                            var currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                            currencyProxy.LackOfResources(food,wood,stone,gold);
                        }
                    }

                    return;
                }
            }

            WarFogMgr.RemoveFadeGroupByType(FogSystemMediator.FADE_TYPE_CLICK);
            m_scoutProxy.SearPos(scoutInfo.id, (int) m_Param.pos.x, (int) m_Param.pos.y, m_Param.targetIndex);
        }

        #endregion
        
          #region 选中斥候目标线
        private Timer m_scoutLineRefreshTimer = null;
        private TroopLineInfo m_selectedScoutTargetLine = null;

        private void CloseSelectScoutTargetLine()
        {
            CloseScoutTargetLineRefreshTimer();
            RemoveSelectedScoutTargetLine();
        }
        private void ShowSelectedScoutTargetLine(int scoutID, Vector2 endPos)
        {
            CloseScoutTargetLineRefreshTimer();
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId(scoutID);
            if (armyData == null)
            {
                return;
            }

            DrawSelectedScoutTargetLine(new Vector2[] {armyData.GetMovePos(), endPos});
            m_scoutLineRefreshTimer = Timer.Register(1, () =>
            {
                if (armyData == null)
                {
                    CloseSelectScoutTargetLine();
                    return;
                }
                DrawSelectedScoutTargetLine(new Vector2[] {armyData.GetMovePos(), endPos});
            },null,true);
        }
        private void DrawSelectedScoutTargetLine(Vector2[] paths)
        {
            if (m_selectedScoutTargetLine == null)
            {
                m_selectedScoutTargetLine = new TroopLineInfo();
                m_selectedScoutTargetLine.LoadTroopLine(false,"troop_line_mine");
                m_selectedScoutTargetLine.ChangeLineColor(Color.white);
            }
            m_selectedScoutTargetLine.SetTroopLinePath(paths);
        }

        private void RemoveSelectedScoutTargetLine()
        {
            if (m_selectedScoutTargetLine != null)
            {
                m_selectedScoutTargetLine.Destroy();
                m_selectedScoutTargetLine = null;
            }
        }

        private void CloseScoutTargetLineRefreshTimer()
        {
            if (m_scoutLineRefreshTimer != null)
            {
                m_scoutLineRefreshTimer.Cancel();
                m_scoutLineRefreshTimer = null;
            }
        }
        #endregion
    }
}