// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_WarnInfo_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using System;
using Data;
using Hotfix;
using SprotoType;

namespace Game {
    public partial class UI_Item_WarnInfo_SubView : UI_SubView
    {
        private Timer m_arrivalTimer = null;
        private EarlyWarningInfoEntity m_info = null;

        public void Refresh(EarlyWarningInfoEntity info, bool isDetail)
        {
            m_info = info;
            WarWarningType warningType = (WarWarningType)info.earlyWarningType;
            //刷新标头图标
            ClientUtils.PlaySpine(m_UI_Model_Warning.m_UI_Model_Warning_SkeletonGraphic, WarWarningProxy.GetWarningSkinName(warningType, info.isRally),
                "idle", true);
            
            //刷内容
            switch (warningType)
            {
                case WarWarningType.Transport:
                    m_pl_mes_ArabLayoutCompment.gameObject.SetActive(false);
                    m_pl_mes2_ArabLayoutCompment.gameObject.SetActive(true);
                    var txt = "[" + info.guildAbbr + "]" + info.transportName;
                    m_lbl_name2_LanguageText.text = LanguageUtils.getTextFormat(GetNameLangugeId(warningType), txt);
                    long allLoad = 0;
                    if (info.transportResourceInfo != null)
                    {
                        foreach (var v in info.transportResourceInfo)
                        {
                            allLoad += v.load;
                        }
                    }
                    m_lbl_count2_LanguageText.text = LanguageUtils.getTextFormat(184004 , ClientUtils.FormatComma(allLoad));
                    var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                    ClientUtils.LoadSprite(m_img_char_PolygonImage, config.transportIcon);
                    break;
                default:
                    m_pl_mes_ArabLayoutCompment.gameObject.SetActive(true);
                    m_pl_mes2_ArabLayoutCompment.gameObject.SetActive(false);

                    int languageId = GetNameLangugeId(warningType, info.isRally);
                    string text = string.Empty;
                    if (string.IsNullOrEmpty(info.guildAbbr) || info.guildAbbr == "")
                    {
                        text = LanguageUtils.getTextFormat(languageId, info.scoutFromName);
                    }
                    else
                    {
                        text = LanguageUtils.getTextFormat(languageId,
                            string.Format("[{0}]{1}", info.guildAbbr, info.scoutFromName));
                    }
                    m_lbl_name_LanguageText.text = text;
                    m_lbl_count_LanguageText.gameObject.SetActive(true);
                    m_pl_view.gameObject.SetActive(warningType != WarWarningType.Scout);
                    m_img_arrow_PolygonImage.gameObject.SetActive(warningType != WarWarningType.Scout && !isDetail);
                    if(warningType != WarWarningType.Scout)
                    {
                        m_UI_Model_CapHeadMain.gameObject.SetActive(info.mainHeroId != 0);
                        if(info.mainHeroId != 0)
                        {
                            m_UI_Model_CapHeadMain.SetHero(info.mainHeroId, info.mainHeroLevel);
                        }
                        m_UI_Model_CapHeadSub.gameObject.SetActive(info.deputyHeroId != 0);
                        if(info.deputyHeroId != 0)
                        {
                            m_UI_Model_CapHeadSub.SetHero(info.deputyHeroId, info.deputyHeroLevel);
                        }
                        m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(200037, ClientUtils.FormatComma(WarWarningProxy.GetSoldierCount(info)));
                    }
                    break;
            }
            

            //刷新链接
            if (warningType == WarWarningType.Transport)
            {
                m_UI_Model_Link.SetLinkText(LanguageUtils.getText(200034));
            }
            else
            {
                //针对使用scoutObjectType
                m_UI_Model_Link.SetLinkText(GetTargetNanme());
            }
            
            //刷新抵达
            if(m_arrivalTimer != null)
            {
                m_arrivalTimer.Cancel();
                m_arrivalTimer = null;
            }
            m_lbl_time_LanguageText.text = ClientUtils.FormatTimeSplit((int)(info.arrivalTime - ServerTimeModule.Instance.GetServerTime()));
            m_arrivalTimer = Timer.Register(1, null, (time) =>
            {
                m_lbl_time_LanguageText.text = ClientUtils.FormatTimeSplit((int)(info.arrivalTime - ServerTimeModule.Instance.GetServerTime()));
            }, true);
            m_img_ignore_PolygonImage.gameObject.SetActive(info.isShield);
        }

        public void Clear()
        {
            if(m_arrivalTimer != null)
            {
                m_arrivalTimer.Cancel();
                m_arrivalTimer = null;
            }
        }

        public Vector3 GetLocationPosition()
        {
            Vector3 position = Vector3.zero;
            switch ((WarWarningType)m_info.earlyWarningType)
            {
                case WarWarningType.Transport:
                    position = GetCityLocationPos();
                    break;
                default:
                    RssType targetType = (RssType)m_info.scoutObjectType;
                    switch (targetType)
                    {
                        case RssType.City:
                            position = GetCityLocationPos();
                            break;
                        default:
                            
                            position = GetTroopLocationPos((int)m_info.armyIndex);
                            if (position == Vector3.zero)
                            {
                                position = GetRallyTroopPos((int)m_info.objectIndex);
                            }

                            break;
                    }
                    break;
            }
           
            return position;
        }

        private string GetTargetNanme()
        {
            Vector3 position = Vector3.zero;
            RssType targetType = (RssType)m_info.scoutObjectType;
            string name = string.Empty;
            switch (targetType)
            {
                case RssType.Troop:
                    name = LanguageUtils.getText(200036);
                    break;
                case RssType.City:
                    name = LanguageUtils.getText(200034);
                    break;
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Wood:
                case RssType.Gold:
                case RssType.Gem:
                    name = LanguageUtils.getText(200035);
                    break;
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                    {
                        AllianceProxy allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                        int configType = allianceProxy.GetBuildServerTypeToConfigType(m_info.scoutObjectType);
                        var allianceBuildingCfg = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(configType);
                        if(allianceBuildingCfg != null)
                        {
                            name = LanguageUtils.getText(allianceBuildingCfg.l_nameId);
                        }
                    }
                    break;
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.HolyLand:
                case RssType.CheckPoint:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:
                    {
                        var strongHoldDataCfg = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)m_info.holyLandId);
                        if (strongHoldDataCfg != null)
                        {
                            var strongHoldTypeCfg = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataCfg.type);
                            name = LanguageUtils.getText(strongHoldTypeCfg.l_nameId);
                        }
                    }
                    break;
            }            
            return name;
        }

        private Vector3 GetCityLocationPos()
        {
            var cityBuildingProxy = 
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            return new Vector3(cityBuildingProxy.RolePos.x , 0 , cityBuildingProxy.RolePos.y); 
        }


        private Vector3 GetRallyTroopPos(int objectId)
        {
            var  m_RallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;  
            Vector2 v2= m_RallyTroopsProxy.GetRallyTroopPos(objectId);
            return  new Vector3(v2.x/100f,0,v2.y/100f);
        }


        private Vector3 GetTroopLocationPos(int armyIndx)
        {
            var troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            if (troopProxy == null) 
                return Vector3.zero;
            var armyInfo = troopProxy.GetArmyByIndex(armyIndx);
            if (armyInfo == null)
            {
                return Vector3.zero;
            }

            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndx);
            if (armyData == null)
            {
                return  Vector3.zero;
            }

            Vector3 pos = Vector3.zero;
            if (armyData.go != null)
            {
                pos = armyData.go.transform.position;
            }
            else
            {
                var posV2 = armyData.GetMovePos();
                if(posV2 == Vector2.zero)
                {
                    pos.x = armyData.Pos.x;
                    pos.y = armyData.Pos.y;
                }
                else
                {
                    pos.x = posV2.x;
                    pos.z = posV2.y;
                }
            }
            return pos;
        }



        private int GetNameLangugeId(WarWarningType warningType, bool isRally = false)
        {
            int languageId = 0;
            switch (warningType)
            {
                case WarWarningType.Scout:
                    languageId = 200033;
                    break;
                case WarWarningType.War:
                    if(isRally)
                    {
                        languageId = 200112;
                    }
                    else
                    {
                        languageId = 610027;
                    }
                    break;
                case WarWarningType.Reinforce:
                    languageId = 610028;
                    break;
                case WarWarningType.Transport:
                    languageId = 184036;
                    break;
            }
            return languageId;
        }

        public void SetShowDetailButtonInteractable(bool interactable)
        {
            m_pl_rect_GameButton.interactable = interactable;
        }

        public void RemoveAllClickListener()
        {
            m_btn_ignore_GameButton.onClick.RemoveAllListeners();
            m_UI_Model_Link.RemoveAllClickEvent();
            m_pl_rect_GameButton.onClick.RemoveAllListeners();
        }

        public void AddShowDetailButtonListener(UnityAction action)
        {
            m_pl_rect_GameButton.onClick.AddListener(action);
        }

        public void AddIgnoreButtonListener(UnityAction action)
        {
            m_btn_ignore_GameButton.onClick.AddListener(action);
        }

        public void AddLocationTargetButtonListener(UnityAction action)
        {
            m_UI_Model_Link.AddClickEvent(action);
        }
    }
}