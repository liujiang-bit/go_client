// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年1月14日
// Update Time         :    2020年1月14日
// Class Description   :    怪物战斗HudView
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
    public class MonsterFightUiHudView : FightUiHudView
    {
        private UI_Pop_WorldArmyCmdView m_armyCmdView;
        private GameObject m_targetObject;
        private MapObjectInfoEntity m_monsterData;
        private Vector3 m_vec3 = Vector3.zero;
        private Vector3 m_normalized = Vector3.zero;

        public void Create(int troopId, GameObject go, MapObjectInfoEntity monsterData)
        {
            HUDUI hud = HUDUI
                           .Register(UI_Pop_WorldArmyCmdView.VIEW_NAME, typeof(UI_Pop_WorldArmyCmdView),HUDLayer.world, go)
                           .SetTargetGameObject(go)
                           .SetPositionAutoAnchor(true).SetInitCallback(OnMonsterHudUI).SetScaleAutoAnchor(true)
                           .SetAllowUpdatePos(false)
                           .SetUpdateCallback(UpdateMonsterHUDUI, -1).SetCameraLodDist(500f, 3000f, (isOn, view) =>
                           {
                               if (isOn)
                               {
                                   WorldMapLogicMgr.Instance.BattleUIEffectHandler.Remove(troopId);

                                   m_armyCmdView.m_pl_beTarget_Animator.Play("Empty");
                                   m_armyCmdView.m_UI_Model_CaptainHead.Play("Empty");
                               }
                           });
            ClientUtils.hudManager.ShowHud(hud);

            m_targetObject = go;
            TroopId = troopId;
            HuduiView = hud;
            m_monsterData = monsterData;
        }

        private void OnMonsterHudUI(HUDUI info)
        {
            UI_Pop_WorldArmyCmdView view = info.gameView as UI_Pop_WorldArmyCmdView;
            if (view != null)
            {
                m_armyCmdView = view;
                MapElementUi = info.uiObj.GetComponent<MapElementUI>();

                view.m_UI_Item_CMDBtns.gameObject.SetActive(false);
                view.m_pl_namebg_PolygonImage.gameObject.SetActive(false);
                view.m_pl_time.gameObject.SetActive(false);
                if (m_monsterData != null)
                {
                    Color color = HUDHelp.GetOtherTroopColor(m_monsterData);
                    if (color.Equals(RS.red))
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
                    view.m_lbl_FightPlayerName_LanguageText.color = color; ;

                    view.gameObject.name = string.Format("{0}_{1}_{2}", UI_Pop_WorldArmyCmdView.VIEW_NAME,"FightMonster", m_monsterData.objectId);
                    view.m_UI_Model_CaptainHead.SetIcon(m_monsterData.heroDefine.heroIcon);
                    view.m_UI_Model_CaptainHead.SetFightArmyRare(RS.MonsterFightArmyFrame);

                    HUDHelp.SetBloodProgress(view.m_pl_sd_GameSlider_GameSlider, view.m_img_Fill_PolygonImage, m_monsterData.HPMax, m_monsterData.HP);

                    var pos = view.m_pl_head.anchoredPosition;
                    pos.x = 100;
                    pos.y = -7.5f;
                    view.m_pl_head.anchoredPosition = pos;
                    info.uiObj.GetComponent<MapElementUI>().SetUIType((int)MapElementUI.ElementUIType.Troop);
                    info.uiObj.GetComponent<MapElementUI>().SetUIStatus(true, 0);
                    info.uiObj.GetComponent<MapElementUI>().SetUIFadeShow((int)MapElementUI.FadeType.AllFadeIn);
                    
                    view.m_btn_captainHead_GameButton.onClick.AddListener(() =>
                    {
                        m_RssProxy.OpenMonsterUI((int)m_monsterData.objectId);
                    });
                }
            }
        }

        private void UpdateMonsterHUDUI(HUDUI hudui)
        {
            if (m_targetObject == null)
            {
                return;
            }
            if (m_armyCmdView == null)
            {
                return;
            }

            HUDHelp.SetBloodProgress(m_armyCmdView.m_pl_sd_GameSlider_GameSlider, m_armyCmdView.m_img_Fill_PolygonImage, m_monsterData.HPMax, m_monsterData.HP);

            m_vec3.Set(m_targetObject.transform.forward.x, 0, m_targetObject.transform.forward.z);
            m_normalized = m_vec3.normalized;
            if (IsFighting)
            {
                MapElementUi.SetBattlePos(m_normalized.x, 0, m_normalized.z,
                            m_targetObject.transform.position.x,
                            m_targetObject.transform.position.y,
                            m_targetObject.transform.position.z);
            }
            else
            {
                MapElementUi.SetPosition(m_targetObject.transform.position.x, m_targetObject.transform.position.y, m_targetObject.transform.position.z);
                FadeOutTime = FadeOutTime + Time.deltaTime;
                if (FadeOutTime > 1.1f)
                {
                    Close();
                }
            }  
            m_armyCmdView.m_UI_Item_WorldArmyCmdAp.SetImgActive(m_monsterData.maxSp, m_monsterData.sp);
        }

        public override void ShowHeadStatus()
        {
            base.ShowHeadStatus();
            if (battleUiData == null)
            {
                return;
            }

            if (m_armyCmdView == null)
            {
                return;
            }

            bool isShow =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().IsShowEffect((int) m_monsterData.objectId);
            if (!isShow)
            {
                return;
            }

            switch (battleUiData.type)
            {
                case BattleUIType.BattleUI_HeadChangeScale:
                    m_armyCmdView.m_pl_captainhead_Animator.Play("UseSkill");
                    //Debug.LogError("野蛮人播放头像变大");
                    break;
                case BattleUIType.BattleUI_ShowHeadEffect:
                   // Debug.LogError("野蛮人播放ui特效");
                    WorldMapLogicMgr.Instance.BattleUIEffectHandler.Play(battleUiData.id,RS.BattleHudHeadEffect,m_armyCmdView.m_UI_Model_CaptainHead.gameObject.transform);
                    break;
                case BattleUIType.BattleUI_ShowViceHead:
                    m_armyCmdView.m_pl_subCaptain_Animator.Play("UseSkill");
                    m_armyCmdView.m_UI_SubCaptain.gameObject.SetActive(true);
                    break;
                case BattleUIType.BattleUI_ShowBeAttack:       
                   // Debug.LogError("野蛮人播放被锁定特效");
                    m_armyCmdView.m_pl_beTarget_Animator.Play("Flash");
                    break;
                case BattleUIType.BattleUI_ShowBeAttackIcon:
                   // Debug.LogError("野蛮人表现被技能伤害效果");
                    m_armyCmdView.m_UI_Model_CaptainHead.Play("Flash");
                    m_armyCmdView.m_pl_captainhead_Animator.transform.DOShakePosition(5, new Vector3(0, 3, 0));
                    break;
            }

        }
    }
}