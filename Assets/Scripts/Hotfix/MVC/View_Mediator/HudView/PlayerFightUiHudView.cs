// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年1月14日
// Update Time         :    2020年1月14日
// Class Description   :    玩家战斗HudView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using System;
using DG.Tweening;
using Game;
using Hotfix;
using UnityEngine.UI;

namespace Client
{
    public class PlayerFightUiHudView : FightUiHudView
    {
        private UI_Pop_WorldArmyCmdView m_armyCmdView;
        private GameObject m_targetObject;
        private ArmyData m_armyData;
        private Vector3 m_vec3 = Vector3.zero;
        private Vector3 m_normalized = Vector3.zero;
        private RallyTroopsProxy m_RallyTroopsProxy;        

        private bool m_isSelectStatus;
        private float m_statusRefreshTime = 0;

        public void Create(int troopId, GameObject go)
        {
            HUDUI hud = HUDUI
                .Register(UI_Pop_WorldArmyCmdView.VIEW_NAME, typeof(UI_Pop_WorldArmyCmdView),
                    HUDLayer.world, go.gameObject).SetPositionAutoAnchor(true)
                .SetInitCallback(OnTroopHudFightCallBack)
                .SetTargetGameObject(go.gameObject).SetScaleAutoAnchor(true)
                .SetAllowUpdatePos(false)
                .SetUpdateCallback(UpdateTroopFightHUDUI, -1).SetCameraLodDist(500f, 3000f, (isOn, view) =>
                {
                    if (isOn)
                    {
                        WorldMapLogicMgr.Instance.BattleUIEffectHandler.Remove(troopId);
                        if(m_armyCmdView != null)
                        {
                            m_armyCmdView.m_pl_beTarget_Animator.Play("Empty");
                            m_armyCmdView.m_UI_Model_CaptainHead.Play("Empty");
                        }                        
                    }
                });
            ClientUtils.hudManager.ShowHud(hud);

            m_targetObject = go;
            TroopId = troopId;
            HuduiView = hud;
        }

        private void OnTroopHudFightCallBack(HUDUI info)
        {
            var m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            var m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            var m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;

            UI_Pop_WorldArmyCmdView view = info.gameView as UI_Pop_WorldArmyCmdView;
            if (view != null)
            {
                view.gameObject.name = string.Format("{0}_{1}_{2}", UI_Pop_WorldArmyCmdView.VIEW_NAME, "FightTroop", TroopId);

                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(TroopId);
                if (armyData != null)
                {
                    m_armyCmdView = view;
                    m_armyData = armyData;
                    MapElementUi = info.uiObj.GetComponent<MapElementUI>();
                    HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(armyData.heroId);

                    view.m_pl_namebg_PolygonImage.gameObject.SetActive(false);
                    view.m_pl_time.gameObject.SetActive(false);
                    view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                    view.m_pl_state.gameObject.SetActive(false);
                    var pos = view.m_pl_head.anchoredPosition;
                    pos.x = 100;
                    pos.y = -7.5f;
                    view.m_pl_head.anchoredPosition = pos;

                    HUDHelp.SetBloodProgress(view.m_pl_sd_GameSlider_GameSlider, view.m_img_Fill_PolygonImage, armyData.troopNumMax, armyData.troopNums);

                    if (armyData.isPlayerHave)
                    {
                        ClientUtils.LoadSprite(view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_PolygonImage,
                            RS.HudHeroGreen);
                        ClientUtils.LoadSprite(view.m_UI_SubCaptain.m_UI_Model_CaptainHead_PolygonImage,
                            RS.HudHeroGreen);

                        ClientUtils.LoadSprite(view.m_img_line_PolygonImage, RS.FightHudUILineGreen);
                        view.m_img_line_PolygonImage.color = RS.green; //绿色
                        view.m_lbl_FightPlayerName_LanguageText.color = RS.green;
                    }
                    else
                    {
                        if (GuideProxy.IsGuideing)
                        {
                            ClientUtils.LoadSprite(view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_PolygonImage,
                                RS.HudHeroBlue);
                            ClientUtils.LoadSprite(view.m_UI_SubCaptain.m_UI_Model_CaptainHead_PolygonImage,
                                RS.HudHeroBlue);
                            ClientUtils.LoadSprite(view.m_img_line_PolygonImage, RS.FightHudUILineBlue);
                            view.m_img_line_PolygonImage.color = RS.blue; //蓝色
                            view.m_lbl_FightPlayerName_LanguageText.color = RS.blue;
                        }
                        else
                        {
                            Color color = HUDHelp.GetOtherTroopColor(armyData);
                            if(color.Equals(RS.red))
                            {
                                ClientUtils.LoadSprite(view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_PolygonImage,
                                    RS.HudHeroRed);
                                ClientUtils.LoadSprite(view.m_UI_SubCaptain.m_UI_Model_CaptainHead_PolygonImage,
                                    RS.HudHeroRed);
                                ClientUtils.LoadSprite(view.m_img_line_PolygonImage, RS.FightHudUILineRed);
                            }
                            else if (color.Equals(RS.blue))
                            {
                                ClientUtils.LoadSprite(view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_PolygonImage,
                                    RS.HudHeroBlue);
                                ClientUtils.LoadSprite(view.m_UI_SubCaptain.m_UI_Model_CaptainHead_PolygonImage,
                                    RS.HudHeroBlue);
                                ClientUtils.LoadSprite(view.m_img_line_PolygonImage, RS.FightHudUILineBlue);
                            }
                            else
                            {
                                ClientUtils.LoadSprite(view.m_UI_Model_CaptainHead.m_UI_Model_CaptainHead_PolygonImage,
                                    RS.HudHeroWhite);
                                ClientUtils.LoadSprite(view.m_UI_SubCaptain.m_UI_Model_CaptainHead_PolygonImage,
                                    RS.HudHeroWhite);
                                ClientUtils.LoadSprite(view.m_img_line_PolygonImage, RS.FightHudUILineWhite);
                            }
                            view.m_img_line_PolygonImage.color = color;
                            view.m_lbl_FightPlayerName_LanguageText.color = color; 
                        }
                    }
                    
                    string guildAddName = string.Empty;
                    if (!string.IsNullOrEmpty(armyData.guildAbbName))
                    {
                        guildAddName = string.Format("[{0}]", armyData.guildAbbName);
                    }

                    HUDHelp.SetAllianceIcon(view, armyData);

                    switch (GameModeManager.Instance.CurGameMode)
                    {
                        case GameModeType.World:
                            view.m_lbl_FightPlayerName_LanguageText.text = string.Format("{0}{1}", guildAddName, armyData.armyName);
                            break;
                        case GameModeType.Expedition:
                            {
                                if (hero != null)
                                {
                                    view.m_lbl_FightPlayerName_LanguageText.text = LanguageUtils.getText(hero.config.l_nameID);
                                }
                            }
                            break;
                    }
                    
                    if (hero != null)
                    {
                        view.m_UI_Model_CaptainHead.SetIcon(hero.config.heroIcon);
                       // view.m_UI_Model_CaptainHead.SetFightArmyRare(armyData);
                    }

                    HeroProxy.Hero viceHero = m_HeroProxy.GetHeroByID(armyData.viceId);
                    if (viceHero != null)
                    {
                        view.m_UI_SubCaptain.SetIcon(viceHero.config.heroIcon);
                    }

                    view.m_btn_captainHead_GameButton.onClick.AddListener(() =>
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.TouchTroopSelectByFightHudIcon, armyData.objectId);
                    });

                    info.uiObj.GetComponent<MapElementUI>().SetUIType((int) MapElementUI.ElementUIType.Troop);
                    info.uiObj.GetComponent<MapElementUI>().SetUIStatus(true, 0);
                    info.uiObj.GetComponent<MapElementUI>().SetUIFadeShow((int) MapElementUI.FadeType.AllFadeIn);
                }
            }
        }

        private void UpdateTroopFightHUDUI(HUDUI hudui)
        {
            if (m_armyCmdView == null)
            {
                return;
            }

            if (m_targetObject == null)
            {
                return;
            }

            HUDHelp.SetBloodProgress(m_armyCmdView.m_pl_sd_GameSlider_GameSlider, m_armyCmdView.m_img_Fill_PolygonImage, m_armyData.troopNumMax, m_armyData.troopNums);

            m_vec3.Set(m_targetObject.transform.forward.x, 0, m_targetObject.transform.forward.z);
            m_normalized = m_vec3.normalized;
            if (IsFighting)
            {
                int attackerId;
                Vector3 attackerPos;
                if(WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetAttackerPos(TroopId, out attackerId, out attackerPos))
                {
                    m_normalized = (attackerPos - m_targetObject.transform.position).normalized;
                    targetId = attackerId;
                    stanceAngle = Common.GetAngle360(Vector3.forward, (m_targetObject.transform.position - attackerPos).normalized);
                }

                int layer = stanceLayer;
                if (stanceMaxLayer != 0 && stanceLayer > stanceMaxLayer)
                {
                    layer = stanceMaxLayer;
                }
                float offset = layer * stanceOffset;

                //调试时使用
                //m_armyCmdView.m_lbl_FightPlayerName_LanguageText.text = layer + "|" + TroopId;

                MapElementUi.SetBattlePos(m_normalized.x, 0, m_normalized.z, m_targetObject.transform.position.x,
                    m_targetObject.transform.position.y, m_targetObject.transform.position.z, offset);
            }
            else
            {
                MapElementUi.SetPosition(m_targetObject.transform.position.x, m_targetObject.transform.position.y,
                    m_targetObject.transform.position.z);
                FadeOutTime = FadeOutTime + Time.deltaTime;
                if (FadeOutTime > 1.1f)
                {
                    Close();
                }
            }

            if (m_isSelectStatus)
            {
                m_statusRefreshTime = m_statusRefreshTime + Time.deltaTime;
                if (m_statusRefreshTime > 0.5f)
                {
                    m_statusRefreshTime = 0f;
                    RefreshFightNum();
                }
            }

            m_armyCmdView.m_UI_Item_WorldArmyCmdAp.SetImgActive(m_armyData.angerMax, m_armyData.anger);
        }

        public void OpenRallyTroopHUDView()
        {
            if (m_RallyTroopsProxy == null)
            {
                m_RallyTroopsProxy =
                    AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            }

            if (m_armyData != null)
            {
                HUDHelp.OnRallyTroopHud(m_armyCmdView, m_armyData, m_RallyTroopsProxy);
                SetBtnActive(false);
            }
        }

        public void SetBtnActive(bool isShow)
        {
            if (m_armyCmdView == null)
            {
                return;
            }
            m_armyCmdView.m_UI_Item_CMDBtns.gameObject.SetActive(isShow);
        }


        public void CloseOtherTroopHudView()
        {
            if (m_armyCmdView == null)
            {
                return;
            }

            m_isSelectStatus = false;
            m_armyCmdView.m_pl_namebg_PolygonImage.gameObject.SetActive(false);
            m_armyCmdView.m_UI_Item_CMDBtns.gameObject.SetActive(false);
            m_armyCmdView.m_lbl_FightPlayerName_LanguageText.gameObject.SetActive(true);
        }

        public override void ShowFightStatus()
        {
            if (m_armyCmdView == null)
            {
                return;
            }

            m_isSelectStatus = true;
            m_statusRefreshTime = 0f;
            m_armyCmdView.m_lbl_FightPlayerName_LanguageText.gameObject.SetActive(false);
            m_armyCmdView.m_pl_namebg_PolygonImage.gameObject.SetActive(true);
            string guildAddName = string.Empty;
            if (!string.IsNullOrEmpty(m_armyData.guildAbbName))
            {
                guildAddName = string.Format("[{0}]", m_armyData.guildAbbName);
            }

            HUDHelp.SetAllianceIcon(m_armyCmdView, m_armyData);

            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    {
                        m_armyCmdView.m_lbl_playerName_LanguageText.text = string.Format("{0}{1}", guildAddName, m_armyData.armyName);
                    }
                    break;
                case GameModeType.Expedition:
                    {
                        var m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                        HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(m_armyData.heroId);
                        if (hero != null)
                        {
                            m_armyCmdView.m_lbl_playerName_LanguageText.text = LanguageUtils.getText(hero.config.l_nameID);
                        }
                    }
                    break;
            }

            RefreshFightNum();
        }

        public override void ShowHeadStatus()
        {
            base.ShowHeadStatus();
            if (battleUiData == null||m_armyCmdView==null)
            {
                return;
            }

            switch (battleUiData.type)
            {
                case BattleUIType.BattleUI_HeadChangeScale:
                    if(!m_armyCmdView.m_pl_namebg_PolygonImage.gameObject.activeSelf)
                    {
                        m_armyCmdView.m_pl_captainhead_Animator.Play("UseSkill");
                    }
                    //Debug.LogError("播放头像变大");
                    break;
                case BattleUIType.BattleUI_ShowHeadEffect:
                    //Debug.LogError("播放ui特效");                 
                    WorldMapLogicMgr.Instance.BattleUIEffectHandler.Play(battleUiData.id, RS.BattleHudHeadEffect,
                        m_armyCmdView.m_UI_Model_CaptainHead.gameObject.transform);
                    break;
                case BattleUIType.BattleUI_ShowViceHeadEffect:
                    //Debug.LogError("播放ui特效");                 
                    WorldMapLogicMgr.Instance.BattleUIEffectHandler.Play(battleUiData.id, RS.BattleHudHeadEffect,
                        m_armyCmdView.m_UI_SubCaptain.gameObject.transform);
                    break;
                case BattleUIType.BattleUI_ShowViceHead:
                    m_armyCmdView.m_UI_SubCaptain.gameObject.SetActive(true);
                    m_armyCmdView.m_pl_subCaptain_Animator.Play("UseSkill");
                    WorldMapLogicMgr.Instance.BattleUIEffectHandler.Play(battleUiData.id, RS.BattleHudHeadEffect,
                        m_armyCmdView.m_UI_SubCaptain.gameObject.transform);
                    break;
                case BattleUIType.BattleUI_ShowBeAttack:
                    // Debug.LogError("播放被锁定特效");
                    m_armyCmdView.m_pl_beTarget_Animator.Play("Flash");
                    break;
                case BattleUIType.BattleUI_ShowBeAttackIcon:
                    m_armyCmdView.m_UI_Model_CaptainHead.Play("Flash");
                    m_armyCmdView.m_pl_captainhead_Animator.transform.DOShakePosition(5, new Vector3(0, 3, 0));
                    break;
            }
        }

        private void RefreshFightNum()
        {
            if (m_armyCmdView == null)
            {
                return;
            }

            m_armyCmdView.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(m_armyData.troopNums);
        }


        public override void FadeOut()
        {
            base.FadeOut();
            if (m_isSelectStatus)
            {
                RefreshFightNum();
            }
        }
    }
}