// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月11日
// Update Time         :    2020年2月11日
// Class Description   :    UI_Item_PlayerPowerInfo_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using Data;
using DG.Tweening;
using UnityEngine.Rendering;
using SprotoType;

namespace Game {
    public partial class UI_Item_PlayerPowerInfo_SubView : UI_SubView
    {
        private int m_vitalityLimit;

        private long m_lastPower;

        private GameObject m_powerUpEffect;

        private bool isEffectLoading = false;

        private Sequence m_sequence;

        private float firstDestoryTime;

        private PlayerProxy m_playProxy;

        public bool imageLoadFinish = false;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_powerShow_GameButton.onClick.AddListener(OnPowerWindow);

            m_btn_playerHeadIcon_GameButton.onClick.AddListener(ShowPlayerInfo);

            m_playProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            PlayerAttributeProxy playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            SubViewManager.Instance.AddListener(new string[] {
                CmdConstant.UpdatePlayerPower,
                CmdConstant.ForceUpdatePlayerPower,
                CmdConstant.UpdatePlayerActionPower,
                CmdConstant.VipPointChange,
                CmdConstant.VipFreeBoxFlagChange,
                CmdConstant.VipDailyPointFlagChange,
                Role_SetRoleHead.TagName,
        },this.gameObject, OnNotification);
            m_img_head.LoadPlayerIcon(()=>
            {
                imageLoadFinish = true;
                AppFacade.GetInstance().SendNotification(CmdConstant.ShowPlayerResInfo);
            });
            m_lbl_powerVal_LanguageText.text = ClientUtils.FormatComma(m_playProxy.CurrentRoleInfo.combatPower);
            m_lastPower = m_playProxy.CurrentRoleInfo.combatPower;
            m_vitalityLimit = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).vitalityLimit;
            long crrPower = m_playProxy.CurrentRoleInfo.actionForce;
            long maxPower = m_vitalityLimit + (int)playerAttributeProxy.GetCityAttribute(attrType.maxVitality).value;
            m_pb_ap_GameSlider.value = (float)crrPower / maxPower;
            SetVIPLevelText();
            RefreshVIPRedDot();
        }

        private void OnNotification(INotification notification)
        {
            PlayerProxy m_playProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            PlayerAttributeProxy playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            switch (notification.Name)
            {
                case CmdConstant.UpdatePlayerPower:
                    if(GlobalEffectMediator.IsPlayingPowerUpEffect>0)
                    {
                        break;
                    }
                    GlobalEffectMediator.IsPlayingPowerUpEffect = 0;
                    PlayPowerUpEffect(m_playProxy.CurrentRoleInfo.combatPower);
                    m_lastPower = m_playProxy.CurrentRoleInfo.combatPower;
                    break;
                case CmdConstant.ForceUpdatePlayerPower:
                    {
                        PlayPowerUpEffect(m_playProxy.CurrentRoleInfo.combatPower);
                        m_lastPower = m_playProxy.CurrentRoleInfo.combatPower;
                    }
                    break;
                case CmdConstant.UpdatePlayerActionPower:
                    long crrPower = m_playProxy.CurrentRoleInfo.actionForce;
                    long maxPower = m_vitalityLimit + (int)playerAttributeProxy.GetCityAttribute(attrType.maxVitality).value;
                    m_pb_ap_GameSlider.value = (float)crrPower / maxPower;
                    break;
                case CmdConstant.VipPointChange:
                    SetVIPLevelText();
                    break;
                case Role_SetRoleHead.TagName:
                    m_img_head.LoadPlayerIcon();
                    break;
                case CmdConstant.VipFreeBoxFlagChange:
                case CmdConstant.VipDailyPointFlagChange:
                    RefreshVIPRedDot();
                    break;
                default:
                    break;
            }
        }

        
        private void PlayPowerUpEffect(long currentPower)
        {
            if(currentPower>m_lastPower&&!isEffectLoading&&NotInvisible())//战力升高播放特效
            {
                if (m_powerUpEffect == null)
                {
                    isEffectLoading = true;
                    CoreUtils.assetService.Instantiate("UI_10010", (go) =>
                    {
                        if (m_btn_powerShow_PolygonImage == null)
                        {
                            CoreUtils.assetService.Destroy(go);
                            return;
                        }
                        isEffectLoading = false;
                        m_powerUpEffect = go;
                        go.transform.SetParent(m_btn_powerShow_PolygonImage.rectTransform);
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = Vector3.one;


                        SortingGroup sortingGroup = go.GetComponent<SortingGroup>();
                        if(sortingGroup)
                        {
                            Canvas canvas = sortingGroup.GetComponentInParent<Canvas>();
                            if (canvas)
                            {
                                sortingGroup.sortingOrder = canvas.sortingOrder + 51;
                            }
                        }
                    });
                }
                else
                {
                    AutoDestory autoDestory = m_powerUpEffect.GetComponent<AutoDestory>();
                    if(autoDestory)
                    {
                        if(firstDestoryTime==0)
                        {
                            firstDestoryTime = autoDestory.time;
                        }
                        autoDestory.time += firstDestoryTime;

                    }
                    Animator ani = m_powerUpEffect.GetComponentInChildren<Animator>();
                    if(ani)
                    {
                        ani.Play("UI_10010",-1,0);
                    }
                    var effects = m_powerUpEffect.GetComponentsInChildren<ParticleSystem>();
                    if(effects != null)
                    {
                        for(int i = 0;i<effects.Length;i++)
                        {
                            effects[i].time = 0;
                            effects[i].Play();
                        }
                    }
                }
            }

            long diff = currentPower - m_lastPower;
            if(diff==0)
            {
                return;
            }

            m_sequence = DOTween.Sequence();
            SetSequence(m_lastPower,diff);
        }

        private void SetSequence(long lastPower,long diff)
        {
            for (int i = 0; i < 20; i++)
            {
                long floatNum = lastPower + i * diff / 20;
                m_sequence.AppendInterval(0.06f);
                m_sequence.AppendCallback(() =>
                {
                    ChangeText(floatNum);
                });
            }
            PlayerProxy m_playProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_sequence.AppendCallback(() =>
            {
                ChangeText(lastPower + diff);
            });
            m_sequence.Play().SetEase(Ease.OutCirc);
        }

        private void ChangeText(long num)
        {
            if (m_lbl_powerVal_LanguageText == null || m_lbl_powerVal_LanguageText.gameObject == null)
            {
                return;
            }
            string tmpNum = ClientUtils.FormatComma(num);
            if(tmpNum!= m_lbl_powerVal_LanguageText.text)
            {
                m_lbl_powerVal_LanguageText.text = tmpNum;
            }
        }

        private bool NotInvisible()
        {
            return gameObject.activeSelf && m_UI_Item_PlayerPowerInfo_CanvasGroup.alpha > 0;
        }

        private void OnPowerWindow()
        {
            CoreUtils.uiManager.ShowUI(UI.s_PlayerPower);
        }

        private void ShowPlayerInfo()
        {
            CoreUtils.uiManager.ShowUI(UI.s_PlayerInfo);
        }

        private void SetVIPLevelText()
        {
            var vipDefine = m_playProxy.GetCurVipInfo();
            if (vipDefine != null)
            {
                m_lbl_vip_LanguageText.text = LanguageUtils.getText(vipDefine.l_nameID);
            }
        }

        private void RefreshVIPRedDot()
        {
            m_img_redpoint_PolygonImage.gameObject.SetActive(!m_playProxy.CurrentRoleInfo.vipFreeBox||!m_playProxy.CurrentRoleInfo.vipExpFlag);
        }
    }
}