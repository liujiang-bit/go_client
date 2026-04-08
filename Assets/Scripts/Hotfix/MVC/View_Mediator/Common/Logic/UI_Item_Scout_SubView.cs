// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Item_Scout_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using Hotfix;
using Data;
using static Game.ScoutProxy;
using System.Collections.Generic;

namespace Game
{
    public partial class UI_Item_Scout_SubView : UI_SubView
    {
        private ScoutProxy.ScoutInfo m_scout;
        private ScoutProxy.ScoutState m_lastState;
        private TroopProxy m_troopProxy;
        private ScoutProxy m_scoutProxy;
        private Vector2 m_movePos = Vector2.zero;

        protected override void BindEvent()
        {
            m_UI_btn_Blue.AddClickEvent(OnGoClick);
            m_UI_btn_Yellow.AddClickEvent(OnExploerClick);
            m_pl_Link.AddClickEvent(OnPosClick);

            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
        }

        //坐标链接跳转
        private void OnPosClick()
        {
            if (m_scout.state == ScoutProxy.ScoutState.Fog || m_scout.state == ScoutProxy.ScoutState.Scouting)
            {
                CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
                WorldCamera.Instance().ViewTerrainPos(m_scout.x, m_scout.y, 1000, null);
                float dxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
                WorldCamera.Instance().SetCameraDxf(dxf, 1000, null);
            }
        }

        //探索
        private void OnExploerClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
            FogSystemMediator fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;

            var cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;

            //找出正在被探索的迷雾组
            List<ScoutInfo> listInfo = m_scoutProxy.GetAllActiveScounts();
            Dictionary<int, bool> dic = null;
            for (int i = 0; i < listInfo.Count; i++)
            {
                if (listInfo[i].state == ScoutState.Fog)
                {
                    if (listInfo[i].posInfos != null && listInfo[i].posInfos.Count > 0)
                    {
                        int count = listInfo[i].posInfos.Count;
                        ScoutInfo.pos tPos = listInfo[i].posInfos[count - 1];
                        int groupId = fogMedia.GetGroupIdByPos(Mathf.FloorToInt(tPos.x/100), Mathf.FloorToInt(tPos.y/100));
                        if (dic == null)
                        {
                            dic = new Dictionary<int, bool>();
                        }
                        dic[groupId] = true;
                    }
                }
            }

            Vector2 findPos = cityBuildingProxy.RolePos;
            if (m_scout.state == ScoutProxy.ScoutState.Return || m_scout.state == ScoutProxy.ScoutState.Back_City)
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId((int)m_scout.id);
                if (armyData != null)
                {
                    Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                    if (formation != null)
                    {
                        findPos = new Vector2(formation.gameObject.transform.position.x, formation.gameObject.transform.position.z);
                    }
                }
            }

            var worldPos = fogMedia.FindClosestAtWorldPos2(findPos.x, findPos.y, dic);

            Debug.LogFormat("发现组id:{0} ", fogMedia.GetGroupIdByPos(worldPos.x, worldPos.y));

            int fgId = fogMedia.GetFadeGroupId(worldPos.x, worldPos.y, WarFogMgr.GROUP_SIZE);

            WarFogMgr.RemoveFadeGroupByType(FogSystemMediator.FADE_TYPE_CLICK);
            WarFogMgr.CreateFadeGroup(fgId, FogSystemMediator.FADE_TYPE_CLICK, WarFogMgr.GROUP_SIZE);

            CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
            float dxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
            WorldCamera.Instance().SetCameraDxf(dxf, 300, () =>
            {
                Timer.Register(0.1f, () =>
                {
                    WorldCamera.Instance().ViewTerrainPos(worldPos.x, worldPos.y, 200, () =>
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_scoutSearchMenuu, null, worldPos);
                    });
                });
            });
        }

        //前往
        private void OnGoClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
            //WorldCamera.Instance().ViewTerrainPos(m_scout.x, m_scout.y, 1000, null);
            //float dxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
            //WorldCamera.Instance().SetCameraDxf(dxf, 1000, null);

            TouchNotTroopSelect touchSelectScout = new TouchNotTroopSelect();
            touchSelectScout.id = m_scout.id;
            touchSelectScout.isAutoMove = false;
            AppFacade.GetInstance().SendNotification(CmdConstant.TouchScoutSelect, touchSelectScout);
        }

        public void SetScoutInfo(ScoutProxy.ScoutInfo info)
        {
            if (info.id == 1)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(181156);
                ClientUtils.LoadSprite(m_UI_Model_CaptainHead.m_img_char_PolygonImage,
                    CoreUtils.dataService.QueryRecord<ConfigDefine>(0).toScoutsIcon1);
            }
            else if (info.id == 2)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(181157);
                ClientUtils.LoadSprite(m_UI_Model_CaptainHead.m_img_char_PolygonImage,
                    CoreUtils.dataService.QueryRecord<ConfigDefine>(0).toScoutsIcon2);
            }
            else if (info.id == 3)
            {
                m_lbl_name_LanguageText.text = LanguageUtils.getText(181158);
                ClientUtils.LoadSprite(m_UI_Model_CaptainHead.m_img_char_PolygonImage,
                    CoreUtils.dataService.QueryRecord<ConfigDefine>(0).toScoutsIcon3);
            }

            m_lastState = info.state;
            m_scout = info;
            if (m_scout.state == ScoutProxy.ScoutState.None)
            {
                m_UI_btn_Blue.gameObject.SetActive(false);
                SetYellowBtnVisible(true);
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateRest);
                m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                m_lbl_desc_LanguageText.gameObject.SetActive(true);
                m_lbl_desc_LanguageText.text = LanguageUtils.getText(181151);
                m_pl_Link.SetLinkText("");
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_Link.m_UI_Model_Link_ContentSizeFitter
                    .GetComponent<RectTransform>());
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Fog)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateScope);
                m_pb_rogressBar_GameSlider.gameObject.SetActive(true);
                m_lbl_desc_LanguageText.gameObject.SetActive(false);

                m_UI_btn_Blue.gameObject.SetActive(true);
                SetYellowBtnVisible(false);

                var x = m_scout.x;
                var y = m_scout.y;

                FogSystemMediator fogMedia =
                    AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                var fogs = fogMedia.GetGroupFog(x, y);

                var pro = fogs.Count * 100 / (WarFogMgr.GROUP_SIZE * WarFogMgr.GROUP_SIZE);

                m_lbl_barText_LanguageText.languageId = -1;
                m_lbl_barText_LanguageText.text = LanguageUtils.getTextFormat(181145, 100 - pro);

                m_pb_rogressBar_GameSlider.value = 1 - (pro / 100.0f);

                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Return)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateReturn);
                m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                m_lbl_desc_LanguageText.gameObject.SetActive(true);
                m_UI_btn_Blue.gameObject.SetActive(false);
                SetYellowBtnVisible(true);

                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181152),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Scouting)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateEye);
                m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                m_lbl_desc_LanguageText.gameObject.SetActive(true);
                m_UI_btn_Blue.gameObject.SetActive(false);
                SetYellowBtnVisible(false);

                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181162),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Surveing)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateEye);
                m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                m_lbl_desc_LanguageText.gameObject.SetActive(true);
                m_UI_btn_Blue.gameObject.SetActive(false);
                SetYellowBtnVisible(false);

                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181153),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Back_City)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateReturn);
                m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                m_lbl_desc_LanguageText.gameObject.SetActive(true);
                m_UI_btn_Blue.gameObject.SetActive(false);
                SetYellowBtnVisible(true);

                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181152),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
        }

        public void SetYellowBtnVisible(bool isVisible)
        {
            if (isVisible)
            {
                if (WarFogMgr.IsAllFogOpen())
                {
                    m_UI_btn_Yellow.gameObject.SetActive(false);
                }
                else
                {
                    m_UI_btn_Yellow.gameObject.SetActive(true);
                }
            }
            else
            {
                m_UI_btn_Yellow.gameObject.SetActive(false);
            }
        }

        public void RefreshScoutInfo(ScoutProxy.ScoutInfo info)
        {
            if (info.state != m_lastState)
            {
                SetScoutInfo(info);
                return;
            }

            m_scout = info;
            if (m_scout.state == ScoutProxy.ScoutState.Fog)
            {
                FogSystemMediator fogMedia =
                    AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                var fogs = fogMedia.GetGroupFog(m_scout.x, m_scout.y);
                var pro = fogs.Count * 100 / (WarFogMgr.GROUP_SIZE * WarFogMgr.GROUP_SIZE);
                m_lbl_barText_LanguageText.text = LanguageUtils.getTextFormat(181145, 100 - pro);
                m_pb_rogressBar_GameSlider.value = 1 - (pro / 100.0f);

                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Scouting)
            {
                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181162),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Surveing)
            {
                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));
                
                m_pl_Link.RemoveAllClickEvent();
                m_pl_Link.AddClickEvent(() =>
                {
                    TouchNotTroopSelect touchSelectScout = new TouchNotTroopSelect();
                    touchSelectScout.id = info.id;
                    touchSelectScout.isAutoMove = false;
                    AppFacade.GetInstance().SendNotification(CmdConstant.TouchScoutSelect, touchSelectScout);
                    CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
                });

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181153),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Return)
            {
                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181152),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
            else if (m_scout.state == ScoutProxy.ScoutState.Back_City)
            {
                UpdatePos();
                m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, m_movePos.x, m_movePos.y));

                Int64 diffTime = m_scout.endTime - ServerTimeModule.Instance.GetServerTime();
                diffTime = diffTime < 0 ? 0 : diffTime;
                m_lbl_desc_LanguageText.text = string.Format("{0}{1}", LanguageUtils.getText(181152),
                    ClientUtils.FormatCountDown((int) diffTime));
            }
        }

        private void UpdatePos()
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetScoutDataByScoutId((int) m_scout.id);
            if (armyData != null)
            {
                Vector2 fixedPos = PosHelper.ClientUnitToClientPos(armyData.GetMovePos());
                m_movePos.Set(fixedPos.x, fixedPos.y);
                Troops formation =
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
                if (formation != null)
                {
                    m_movePos.Set((int) Mathf.Floor(formation.gameObject.transform.position.x / 6),
                        (int) Mathf.Floor(formation.gameObject.transform.position.z / 6));
                }
            }
        }
    }
}