using System;
using Client;
using Game;
using Skyunion;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public static class HUDHelp
    {        
        private static Color colorGreen;
        private static Color colorWhite;
        private static Color colorBlue;
        private static Color colorRed;

       
        public static void Init()
        {
            colorGreen = RS.green;
            colorBlue = RS.blue;
            colorWhite = RS.white;
            colorRed = RS.red;
        }

        public static void SetAllianceIcon(UI_Pop_WorldArmyCmdView view,ArmyData armyData)
        {
            if (armyData.guildFlagSigns != null && armyData.guildFlagSigns.Count > 0)
            {     
                view.m_UI_GuildFlag.gameObject.SetActive(true);
                view.m_UI_GuildFlag1.gameObject.SetActive(true);
                view.m_UI_GuildFlag.setData(armyData.guildFlagSigns);
                view.m_UI_GuildFlag1.setData(armyData.guildFlagSigns);
            }
        }

        public static Color GetOtherTroopColor(ArmyData data)
        {
            Color color = RS.white;
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy == null)
                return color;

            RoleInfoEntity role = playerProxy.CurrentRoleInfo;
            if (role == null)
                return color;

            if (role.guildId != 0 && role.guildId == data.guild && !data.isPlayerHave)
            {
                return RS.blue;
            }
            // 无目标就是默认白色
            if (data.targetId == 0)
                return color;

            var wormapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            if (wormapProxy == null)
                return color;

            // 打击目标是盟友或者我需要红色
            MapObjectInfoEntity infoEntity = wormapProxy.GetWorldMapObjectByobjectId(data.targetId);

            if (infoEntity == null)
            {
                return color;
            }

            if ((role.guildId != 0 && infoEntity.guildId != 0 && role.guildId == infoEntity.guildId))
            {
                color = RS.red;
            }
            else if (infoEntity.collectRid == role.rid)
            {
                color = RS.red;
            }
            else if (infoEntity.cityRid == role.rid)
            {
                color = RS.red;
            }
            else if (infoEntity.armyRid == role.rid)
            {
                color = RS.red;
            }

            return color;
        }
        public static Color GetOtherTroopColor(MapObjectInfoEntity objectEntity)
        {
            Color color = RS.white;
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy == null)
                return color;

            RoleInfoEntity role = playerProxy.CurrentRoleInfo;
            if (role == null)
                return color;
            // 无目标就是默认白色
            if (objectEntity.atkId == 0)
                return color;

            var wormapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            if (wormapProxy == null)
                return color;

            // 打击目标是盟友或者我需要红色
            MapObjectInfoEntity infoEntity = wormapProxy.GetWorldMapObjectByobjectId(objectEntity.atkId);
            if (infoEntity == null)
            {
                return color;
            }

            if ((role.guildId != 0 && infoEntity.guildId != 0 && role.guildId == infoEntity.guildId))
            {
                color = RS.red;
            }
            else if (infoEntity.collectRid == role.rid)
            {
                color = RS.red;
            }
            else if (infoEntity.cityRid == role.rid)
            {
                color = RS.red;
            }
            else if (infoEntity.armyRid == role.rid)
            {
                color = RS.red;
            }

            return color;
        }

        public static Color GetCityColor(MapObjectInfoEntity mapObjectInfoEntity)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy == null)
                return RS.white;

            if (playerProxy.CurrentRoleInfo == null)
                return RS.white;

            if (mapObjectInfoEntity.cityRid == playerProxy.CurrentRoleInfo.rid)
                return RS.green;

            AllianceProxy allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            if (allianceProxy == null)
                return RS.white;

            if (playerProxy.CurrentRoleInfo.guildId != 0 && playerProxy.CurrentRoleInfo.guildId == mapObjectInfoEntity.guildId)
                return RS.blue;

            return RS.white;
        }

        public static void OnRallyUpdateTroopHud(UI_Pop_WorldArmyCmdView view, ArmyData armyData,RallyTroopsProxy m_RallyTroopsProxy)
        {
            if (view == null || armyData == null)
            {
                return;
            }
            if (m_RallyTroopsProxy.IsCaptainByarmIndex(armyData.troopId))
            {     
                view.m_lbl_count_LanguageText.text =TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH)? "0": ClientUtils.FormatComma(armyData.troopNums);
            } else if (m_RallyTroopsProxy.isRallyTroopHaveGuid(armyData.armyRid))
            {
                view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
            }
            else
            {
                view.m_lbl_count_LanguageText.text = "????";
            }
        }


        public static void OnRallyTroopHud(UI_Pop_WorldArmyCmdView view, ArmyData armyData, RallyTroopsProxy m_RallyTroopsProxy, Action callback = null, bool isHeadHud = false)
        {
           #region 操作盘处理          
         
            if (m_RallyTroopsProxy.IsCaptainByarmIndex(armyData.troopId))
            {
                view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                view.m_UI_Item_CMDBtns.SetRallTroopActive(false);
                view.m_UI_Item_CMDBtns.SetBtn0Active(false);
                view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
                view.m_lbl_count_LanguageText.color = colorGreen;
                view.m_lbl_playerName_LanguageText.color = colorGreen;
              
                view.m_lbl_count_LanguageText.text =TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH)? "0": ClientUtils.FormatComma(armyData.troopNums);
                
            }else  if (m_RallyTroopsProxy.isRallyTroopHaveGuid(armyData.armyRid))
            {
                view.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(armyData.troopNums);
                view.m_lbl_count_LanguageText.color = colorBlue;
                view.m_lbl_playerName_LanguageText.color = colorBlue;
                ClientUtils.LoadSprite(view.m_UI_Item_CMDBtns.m_UI_Model_CommandBtn0.m_btn_noTextButton_PolygonImage,
                    RS.HudRally);
                view.m_UI_Item_CMDBtns.SetTroopActive(false);
                                    
                if (m_RallyTroopsProxy.IsRallyTroopAttackMe(armyData.armyRid))
                {
                    view.m_UI_Item_CMDBtns.SetBtn0Active(false);
                    view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                }
                else
                {
                    view.m_UI_Item_CMDBtns.SetBtn0Active(true);
                    view.m_UI_Item_CMDBtns.gameObject.SetActive(true);
                }
                
                view.m_lbl_count_LanguageText.text = TroopHelp.IsHaveState((long)armyData.armyStatus, ArmyStatus.FAILED_MARCH)? "0":  ClientUtils.FormatComma(armyData.troopNums);
                view.m_pl_namebg_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                view.m_UI_Item_CMDBtns.gameObject.SetActive(true);
                view.m_UI_Item_CMDBtns.SetRallTroopActive(true);
                view.m_lbl_count_LanguageText.text = "????";
                if (m_RallyTroopsProxy.HasRallyed(armyData.targetId))
                {
                    view.m_lbl_count_LanguageText.color = colorRed;
                    view.m_lbl_playerName_LanguageText.color = colorRed;
                }
                else
                {
                    view.m_lbl_count_LanguageText.color = colorWhite;
                    view.m_lbl_playerName_LanguageText.color = colorWhite;
                }
            }
            view.m_UI_Model_CaptainHead.SetArmyRare(armyData);

            
            string guildAddName = string.Empty;
            if (!string.IsNullOrEmpty(armyData.guildAbbName))
            {
                guildAddName = string.Format("[{0}]", armyData.guildAbbName);
            }

            view.m_lbl_playerName_LanguageText.text = string.Format("{0}{1}",guildAddName,armyData.armyName);
            view.m_UI_Item_CMDBtns.m_UI_Model_CommandBtn3.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                var m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
                m_ScoutProxy.CheckScoutCondition(armyData.objectId, () =>
                {
                    Vector2 v2 = new Vector2(armyData.go.transform.position.x, armyData.go.transform.position.z);
                    UI_Pop_ScoutSelectMediator.Param param = new UI_Pop_ScoutSelectMediator.Param();
                    param.pos = v2;
                    param.targetIndex = armyData.objectId;
                    CoreUtils.uiManager.ShowUI(UI.s_scoutSelectMenu, null, param);
                    callback?.Invoke();
                });
            });
            
            view.m_UI_Item_CMDBtns.m_UI_Model_CommandBtn4.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                FightHelper.Instance.Attack(armyData.objectId);
                callback?.Invoke();
            });
            
            view.m_UI_Item_CMDBtns.m_UI_Model_CommandBtn5.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                FightHelper.Instance.Concentrate(armyData.objectId);
                callback?.Invoke();
            });
            
            view.m_UI_Item_CMDBtns.m_UI_Model_CommandBtn0.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                FightHelper.Instance.Reinfore(armyData.objectId, (int)armyData.armyRid, armyData.objectId, armyData.go.transform.position.x * 100f, armyData.go.transform.position.z * 100f);
                callback?.Invoke();
            });

            if (isHeadHud)
            {
                view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
            }
            
            #endregion
        }

        public static void SetBloodProgress(GameSlider slider, Image img, float maxValue, float value)
        {
            slider.maxValue = maxValue;
            slider.value = value;

            float ratio = value * 1.0f / maxValue;
            if (ratio <= 0.3f)
            {
                img.color = new Color(234 / 255f, 65 / 255f, 40 / 255f, 1f);
            }
            else if (ratio <= 0.7f)
            {
                img.color = new Color(222 / 255f, 144 / 255f, 45 / 255f, 1f);
            }
            else
            {
                img.color = Color.white;
            }
        }
    }
}