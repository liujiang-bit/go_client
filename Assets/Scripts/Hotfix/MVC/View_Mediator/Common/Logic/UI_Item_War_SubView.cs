// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_Item_War_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using System;
using Data;

namespace Game {
    public partial class UI_Item_War_SubView : UI_SubView
    {
        private AllianceProxy m_allianceProxy;
            public void RefreshItemView(ItemWarData itemWarData)
            {
                long leftTimeReady = itemWarData.rallyDetailEntity.rallyReadyTime - ServerTimeModule.Instance.GetServerTime();//准备中
                long leftTimeWait = itemWarData.rallyDetailEntity.rallyWaitTime - ServerTimeModule.Instance.GetServerTime();//等待中
                long leftTimeMarch = itemWarData.rallyDetailEntity.rallyMarchTime - ServerTimeModule.Instance.GetServerTime();//行军中
                TimeSpan m_formatTimeSpan;
     

            if (leftTimeReady >= 0)
            {
                if (itemWarData.detialType == DetialType.rallyDetail)
                {
                    if (!itemWarData.Isme)
                    {
                        if (itemWarData.Involveme)
                        {
                            m_lbl_join_LanguageText.gameObject.SetActive(false);
                            m_btn_add_GameButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            m_lbl_join_LanguageText.gameObject.SetActive(true);
                            m_btn_add_GameButton.gameObject.SetActive(true);
                        }
                    }
                }
                m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeReady);
                if (itemWarData.detialType == DetialType.rallyDetail)
                {
   
                    m_lbl_armystate_LanguageText.text = LanguageUtils.getTextFormat(730288, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                }
                else
                {
                    m_lbl_armystate_LanguageText.text = LanguageUtils.getTextFormat(200091, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                }
                ClientUtils.LoadSprite(m_img_Fill_PolygonImage, RS.GameSlider_green);
                m_pb_ready_GameSlider.minValue = 0;
                m_pb_ready_GameSlider.maxValue = itemWarData.rallyDetailEntity.rallyReadyTime - itemWarData.rallyDetailEntity.rallyStartTime ;
                m_pb_ready_GameSlider.value = ServerTimeModule.Instance.GetServerTime() - itemWarData.rallyDetailEntity.rallyStartTime;
                itemWarData.rallyTroopType = RallyTroopType.ready;
            }
            else if (leftTimeWait >= 0)
            {
                if (itemWarData.detialType == DetialType.rallyDetail)
                {
                    if (!itemWarData.Isme)
                    {
                        m_lbl_join_LanguageText.gameObject.SetActive(false);
                        m_btn_add_GameButton.gameObject.SetActive(false);
                    }
                }
                m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeWait);
                if (itemWarData.detialType == DetialType.rallyDetail)
                {
                    m_lbl_armystate_LanguageText.text = LanguageUtils.getTextFormat(200090, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                }
                else
                {
                    m_lbl_armystate_LanguageText.text = LanguageUtils.getTextFormat(200092, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                }
                m_pb_ready_GameSlider.minValue = 0;
                m_pb_ready_GameSlider.maxValue = itemWarData.rallyDetailEntity.rallyWaitTime - itemWarData.rallyDetailEntity.rallyReadyTime;
                m_pb_ready_GameSlider.value = ServerTimeModule.Instance.GetServerTime() - itemWarData.rallyDetailEntity.rallyReadyTime;

                ClientUtils.LoadSprite(m_img_Fill_PolygonImage, RS.GameSlider_green);
                if (itemWarData.rallyTroopType != RallyTroopType.wait)
                {
                    itemWarData.rallyTroopType = RallyTroopType.wait;
                }
            }
            else
            {
                if (itemWarData.rallyDetailEntity.rallyMarchTime == 0)
                {

                }
                else
                {
                    if (leftTimeMarch >= 0)
                    {
                        if (itemWarData.detialType == DetialType.rallyDetail)
                        { 
                            if (!itemWarData.Isme)
                        {
                            if (itemWarData.Involveme)
                            {
                                m_lbl_join_LanguageText.gameObject.SetActive(false);
                                m_btn_add_GameButton.gameObject.SetActive(false);
                            }
                            else
                            {
                                m_lbl_join_LanguageText.gameObject.SetActive(true);
                                m_btn_add_GameButton.gameObject.SetActive(true);
                            }
                        }
                        }
                        m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeMarch);
                        if (itemWarData.detialType == DetialType.rallyDetail)
                        {
                            m_lbl_armystate_LanguageText.text = LanguageUtils.getTextFormat(730289, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                        }
                        else
                        {
                            m_lbl_armystate_LanguageText.text = LanguageUtils.getTextFormat(200093, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                        }

                        ClientUtils.LoadSprite(m_img_Fill_PolygonImage, RS.GameSlider_yellow);
                        m_pb_ready_GameSlider.minValue = 0; 
                        m_pb_ready_GameSlider.maxValue = itemWarData.rallyDetailEntity.rallyMarchTime - itemWarData.rallyDetailEntity.rallyWaitTime;
                        m_pb_ready_GameSlider.value = ServerTimeModule.Instance.GetServerTime() - itemWarData.rallyDetailEntity.rallyWaitTime;
                        if (itemWarData.rallyTroopType != RallyTroopType.March)
                        {
                            itemWarData.rallyTroopType = RallyTroopType.March;
                        }
                    }
                    else
                    {
                        if (itemWarData.detialType == DetialType.rallyDetail)
                        {
                            if (!itemWarData.Isme)
                            {
                                if (itemWarData.Involveme)
                                {
                                    m_lbl_join_LanguageText.gameObject.SetActive(false);
                                    m_btn_add_GameButton.gameObject.SetActive(false);
                                }
                                else
                                {
                                    m_lbl_join_LanguageText.gameObject.SetActive(true);
                                    m_btn_add_GameButton.gameObject.SetActive(true);
                                }
                            }
                        }
                        m_lbl_armystate_LanguageText.text = LanguageUtils.getText(732063);
                        ClientUtils.LoadSprite(m_img_Fill_PolygonImage, RS.GameSlider_red);
                        m_pb_ready_GameSlider.minValue = 0;
                        m_pb_ready_GameSlider.maxValue = 1;
                        m_pb_ready_GameSlider.value = 1;
                        if (itemWarData.rallyTroopType != RallyTroopType.battle)
                        {
                            itemWarData.rallyTroopType = RallyTroopType.battle;
                        }
                    }
                }
            }

        }
        public void RefreshTargetIcon(ItemWarData itemData )
        {
            if (itemData.detialType == DetialType.rallyDetail)
            {
                long type = itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetType;
                if (type == (long)RssType.City || type == (long)RssType.Troop)
                {
                    if (string.IsNullOrEmpty(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName) || string.Equals("0", itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName))
                    {
                        SetNameTarget(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetName);
                    }
                    else
                    {
                        SetNameTarget(LanguageUtils.getTextFormat(300030, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetName));
                    }
                    m_UI_Item_WarTarget.m_UI_PlayerHead.LoadPlayerIcon(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetHeadId, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetHeadFrameId);
                    m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(true);
                    m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(false);
                }
                else if (type == (long)RssType.BarbarianCitadel || type == (long)RssType.SummonConcentrateMonster)
                {
                    MonsterDefine monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetMonsterId);
                    if (monsterDefine != null)
                    {
                        SetNameTarget(LanguageUtils.getText(monsterDefine.l_nameId));
                        m_UI_Item_WarTarget.SetBuildIcon(monsterDefine.headIcon);
                    }
                    m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(false);
                    m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(true);
                }
                else if (type == (long)RssType.CheckPoint || type == (long)RssType.HolyLand || type == (long)RssType.Sanctuary || type == (long)RssType.Altar || type == (long)RssType.Shrine || type == (long)RssType.LostTemple || type == (long)RssType.Checkpoint_1 || type == (long)RssType.Checkpoint_2 || type == (long)RssType.Checkpoint_3)
                {
                    StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetHolyLandId);
                    if (strongHoldDataDefine != null)
                    {
                        StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>((int)strongHoldDataDefine.type);
                        if (strongHoldTypeDefine != null)
                        {
                            if (string.IsNullOrEmpty(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName) || string.Equals("0", itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName))
                            {
                                SetNameTarget(LanguageUtils.getText(strongHoldTypeDefine.l_nameId));
                            }
                            else
                            {
                                SetNameTarget(LanguageUtils.getTextFormat(300030, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName, LanguageUtils.getText(strongHoldTypeDefine.l_nameId)));
                            }
                            m_UI_Item_WarTarget.SetBuildIcon(strongHoldTypeDefine.iconImg);
                        }
                    }
                    m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(false);
                    m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(true);
                }
                else
                {
                    int alianceBuildingType = m_allianceProxy.GetBuildServerTypeToConfigType(type);
                    var cconfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(alianceBuildingType);
                    m_UI_Item_WarTarget.SetBuildIcon(cconfig.iconImg);
                    if (string.IsNullOrEmpty(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName) || string.Equals("0", itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName))
                    {
                        SetNameTarget(LanguageUtils.getText(cconfig.l_nameId));
                    }
                    else
                    {
                        SetNameTarget(LanguageUtils.getTextFormat(300030, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetGuildName, LanguageUtils.getText(cconfig.l_nameId)));
                    }
                    m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(false);
                    m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(true);
                }
            }
            else if (itemData.detialType == DetialType.rallyedDetail)
            {
                if (string.IsNullOrEmpty(itemData.rallyDetailEntity.rallyGuildName) || string.Equals("0", itemData.rallyDetailEntity.rallyGuildName))
                {
                    SetNameTarget(itemData.rallyDetailEntity.rallyName);
                }
                else
                {
                    SetNameTarget(LanguageUtils.getTextFormat(300030, itemData.rallyDetailEntity.rallyGuildName, itemData.rallyDetailEntity.rallyName));
                }
            }
        }
        public void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
        }
        public void SetNameSelf(string name)
        {
            m_lbl_nameSelf_LanguageText.text = name;
        }

        public void SetArrowRight(bool right = true)
        {
          //  img_build_1003 =   FindUI<Transform>(gameObject.transform, "mid/img_build_1003");
            if (right)
            {
             //   img_build_1003.localRotation = new Quaternion(0, 0, 0, 0);
                m_pl_arrow_GridLayoutGroup.transform.localRotation = new Quaternion(0, 0, 00, 0);
            }
            else
            {
               // img_build_1003.localRotation = new Quaternion(0, 0, 180, 0);
                m_pl_arrow_GridLayoutGroup.transform.localRotation = new Quaternion(0, 0, 180, 0);
            }
       
        }
        public void SetNameTarget(string name)
        {
            m_lbl_nameTarget_LanguageText.text = name;
        }
        public void SetArmyCount(string armyCount)
        {
            m_lbl_armyCount_LanguageText.text = armyCount;
        }
        
        public void AddClickEvent(UnityAction action)
        {
            m_btn_bg_GameButton.onClick.RemoveAllListeners();
            m_btn_bg_GameButton.onClick.AddListener(action);
        }
    }
}