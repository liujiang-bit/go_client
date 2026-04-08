// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月11日
// Update Time         :    2020年2月11日
// Class Description   :    UI_Item_PlayerResources_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using Data;
using DG.Tweening;
using System;

namespace Game {
    public class CurrencyPopAniParam
    {
        public EnumCurrencyType type;
        public bool play;
    }
    public partial class UI_Item_PlayerResources_SubView : UI_SubView
    {

        private Sequence m_foodSequence;
        private long m_lastFood;
        private Sequence m_woodSequence;
        private long m_lastWood;
        private Sequence m_stoneSequence;
        private long m_lastStone;
        private Sequence m_goldSequence;
        private long m_lastGold;
        private Sequence m_denarSequence;
        private long m_lastDenar;

        private CurrencyProxy m_currencyProxy;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            OnCurrencyViewBind();
            ShowCurrencyModules();
            ShowGemModules();
            SubViewManager.Instance.AddListener(
                new string[] { 
                    CmdConstant.UpdateFloatCurrency,
                    CmdConstant.UpdateCurrency,
                    CmdConstant.UpdateMainRoleLevel,
                    CmdConstant.OnGuideMainInterfaceModule,
                    CmdConstant.GuideFinished,
                    CmdConstant.ForceCloseGuide,
                    CmdConstant.FlyUpdatePlayerCurrency,
                    CmdConstant.PlayCurrencyPopAni,
                },this.gameObject, HandleNotification);
        }

        private  void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateFloatCurrency:
                    OnCurrencyFloat((EnumCurrencyType)notification.Body, false);
                    break;
                case CmdConstant.FlyUpdatePlayerCurrency:
                    OnCurrencyFloat((EnumCurrencyType)notification.Body, true);
                    break;
                case CmdConstant.PlayCurrencyPopAni:
                    OnCurrencyPopAni(notification.Body);
                    break;
                case CmdConstant.UpdateCurrency:
                    if ((bool)notification.Body)
                    {
                        OnCurrencyChange();
                    }
                    break;
                case CmdConstant.UpdateMainRoleLevel:
                    ShowCurrencyModules();
                    break;
                case CmdConstant.OnGuideMainInterfaceModule:
                case CmdConstant.ForceCloseGuide:
                case CmdConstant.GuideFinished:
                    ShowGemModules();
                    break;
                default:break;
            }
        }
        #region 货币相关


        private void OnCurrencyViewBind()
        {
            //第一次赋值
            OnCurrencyChange();
            m_UI_Model_food.m_btn_btn_GameButton.onClick.AddListener(() => { OnBtnClick(0); });
            m_UI_Model_wood.m_btn_btn_GameButton.onClick.AddListener(() => { OnBtnClick(1); });
            m_UI_Model_stone.m_btn_btn_GameButton.onClick.AddListener(() => { OnBtnClick(2); });
            m_UI_Model_gold.m_btn_btn_GameButton.onClick.AddListener(() => { OnBtnClick(3); });
            m_UI_Model_gem.m_btn_btn_GameButton.onClick.AddListener(()=>
            {
                CoreUtils.audioService.PlayOneShot("Sound_Ui_GetGem");
            });
        }

        private void OnCurrencyChange()
        {
            CurrencyProxy m_CurrencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_UI_Model_food.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_CurrencyProxy.Food);
            m_UI_Model_wood.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_CurrencyProxy.Wood);
            m_UI_Model_stone.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_CurrencyProxy.Stone);
            m_UI_Model_gold.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_CurrencyProxy.Gold);
            m_UI_Model_gem.m_lbl_val_LanguageText.text = ClientUtils.CurrencyFormat(m_CurrencyProxy.Gem);

            m_lastFood = m_CurrencyProxy.Food;
            m_lastWood = m_CurrencyProxy.Wood;
            m_lastStone = m_CurrencyProxy.Stone;
            m_lastGold = m_CurrencyProxy.Gold;
            m_lastDenar = m_CurrencyProxy.Gem;
        }

        private void OnBtnClick(int i)
        {
            CoreUtils.uiManager.ShowUI(UI.s_quickUseRss, null, i);
            switch(i)
            {
                case 0:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_GetFood");
                    break;
                case 1:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_GetWood");
                    break;
                case 2:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_GetStone");
                    break;
                case 3:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_GetGlod");
                    break;
                default:break;
            }
        }

        //货币数值跳动
        private void OnCurrencyFloat(EnumCurrencyType type, bool forceUpdate)
        {
            switch (type)
            {
                case EnumCurrencyType.food:
                    if (!forceUpdate)
                    {
                        if(GlobalEffectMediator.IsPlayingFoodEffect>0)
                        {
                            return;
                        }
                        GlobalEffectMediator.IsPlayingFoodEffect = 0;
                    }
                    SetSequence(m_foodSequence, m_UI_Model_food.m_lbl_val_LanguageText, m_currencyProxy.Food,m_lastFood, forceUpdate);
                    m_lastFood = m_currencyProxy.Food;
                    break;
                case EnumCurrencyType.wood:
                    if (!forceUpdate)
                    {
                        if (GlobalEffectMediator.IsPlayingWoodEffect > 0)
                        {
                            return;
                        }
                        GlobalEffectMediator.IsPlayingWoodEffect = 0;
                    }
                    SetSequence(m_woodSequence, m_UI_Model_wood.m_lbl_val_LanguageText, m_currencyProxy.Wood, m_lastWood, forceUpdate);
                    m_lastWood = m_currencyProxy.Wood;
                    break;
                case EnumCurrencyType.stone:
                    if (!forceUpdate)
                    {
                        if (GlobalEffectMediator.IsPlayingStoneEffect > 0)
                        {
                            return;
                        }
                        GlobalEffectMediator.IsPlayingStoneEffect = 0;
                    }
                    SetSequence(m_stoneSequence, m_UI_Model_stone.m_lbl_val_LanguageText, m_currencyProxy.Stone, m_lastStone, forceUpdate);
                    m_lastStone = m_currencyProxy.Stone;
                    break;
                case EnumCurrencyType.gold:
                    if (!forceUpdate)
                    {
                        if (GlobalEffectMediator.IsPlayingGoldEffect > 0)
                        {
                            return;
                        }
                        GlobalEffectMediator.IsPlayingGoldEffect = 0;
                    }
                    SetSequence(m_goldSequence, m_UI_Model_gold.m_lbl_val_LanguageText, m_currencyProxy.Gold, m_lastGold, forceUpdate);
                    m_lastGold = m_currencyProxy.Gold;
                    break;
                case EnumCurrencyType.denar:
                    //Debug.LogError("last denar:" + m_currencyProxy.Gem);
                    if (!forceUpdate)
                    {
                        if (GlobalEffectMediator.IsPlayingDenarEffect > 0)
                        {
                            //Debug.LogError("return denar:" + m_currencyProxy.Gem);
                            return;
                        }
                        GlobalEffectMediator.IsPlayingDenarEffect = 0;
                    }
                    //Debug.LogError("start dear:" + m_currencyProxy.Gem);
                    SetSequence(m_denarSequence,m_UI_Model_gem.m_lbl_val_LanguageText, m_currencyProxy.Gem, m_lastDenar, forceUpdate, 99);
                    m_lastDenar = m_currencyProxy.Gem;
                    break;
                default:
                    break;
            }
        }

        private void SetSequence(Sequence sequence,LanguageText text, long currentCurrency , long lastCurrency, bool forceUpdate, int currType = 1)
        {
            if (sequence != null)
            {
                sequence.Kill();
                sequence = null;
            }
            int cType = currType;
            long diff = currentCurrency - lastCurrency;
            bool changeColor = diff < 0 && !forceUpdate;
            sequence = DOTween.Sequence();
            for (int i = 0; i < 10; i++)
            {
                long floatNum = lastCurrency + i * diff / 20;
                sequence.AppendInterval(0.06f);
                sequence.AppendCallback(() =>
                {
                    ChangeTextAndColor(text, floatNum, changeColor, forceUpdate, cType);
                });
            }
            sequence.AppendCallback(() =>
            {
                ChangeTextAndColor(text, lastCurrency + diff, false, forceUpdate, cType);
            });
            sequence.Play().SetEase(Ease.OutCirc);
        }

        private void ChangeTextAndColor(LanguageText text, long num, bool changeColor,bool playPopAni = false, int currType = 1)
        {
            if (text == null || text.gameObject == null)
            {
                return;
            }
            if (currType == 99)
            {
                //Debug.LogError("update denar:" + num);
            }
            text.text = ClientUtils.CurrencyFormat(num);
            Color color = changeColor ? Color.red : Color.white;
            if(text.color!=color)
            {
                text.color = color;
            }
        }

        private void OnCurrencyPopAni(object body)
        {
            if(body is CurrencyPopAniParam)
            {
                CurrencyPopAniParam param = body as CurrencyPopAniParam;

                Animator ani;
                switch(param.type)
                {
                    case EnumCurrencyType.food: ani = m_UI_Model_food.m_img_icon_Animator; break;
                    case EnumCurrencyType.wood: ani = m_UI_Model_wood.m_img_icon_Animator; break;
                    case EnumCurrencyType.stone: ani = m_UI_Model_stone.m_img_icon_Animator; break;
                    case EnumCurrencyType.gold: ani = m_UI_Model_gold.m_img_icon_Animator; break;
                    case EnumCurrencyType.denar: ani = m_UI_Model_gem.m_img_icon_Animator; break;
                    default: return;
                }
                if(param.play)
                {
                    ani.Play("Start",-1,0);
                }
                else
                {
                    ani.Play("Empty");
                }
            }
        }

        private void ShowCurrencyModules()
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            ConfigDefine define = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);

            bool showFood = playerProxy.CurrentRoleInfo.level >= define.foodShowLev;
            if(m_UI_Model_food.gameObject.activeSelf!=showFood)
            {
                m_UI_Model_food.gameObject.SetActive(showFood);
            }
            bool showWood = playerProxy.CurrentRoleInfo.level >= define.woodShowLev;
            if (m_UI_Model_wood.gameObject.activeSelf != showWood)
            {
                m_UI_Model_wood.gameObject.SetActive(showWood);
            }
            bool showStone = playerProxy.CurrentRoleInfo.level >= define.stoneShowLev;
            if (m_UI_Model_stone.gameObject.activeSelf != showStone)
            {
                m_UI_Model_stone.gameObject.SetActive(showStone);
            }
            bool showGold = playerProxy.CurrentRoleInfo.level >= define.goldShowLev;
            if (m_UI_Model_gold.gameObject.activeSelf != showGold)
            {
                m_UI_Model_gold.gameObject.SetActive(showGold);
            }
            OnCaculateSize();
        }

        private void ShowGemModules()
        {
            ConfigDefine define = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            GuideProxy guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
            bool showGem = guideProxy.IsCompletedByStage(define.denarShowStep)||define.denarShowStep==0;
            if (m_UI_Model_gem.gameObject.activeSelf != showGem)
            {
                m_UI_Model_gem.gameObject.SetActive(showGem);
            }
            OnCaculateSize();
        }

        private void OnCaculateSize()
        {
            int count = Convert.ToInt32(m_UI_Model_food.gameObject.activeSelf) + Convert.ToInt32(m_UI_Model_wood.gameObject.activeSelf) + Convert.ToInt32(m_UI_Model_stone.gameObject.activeSelf) + Convert.ToInt32(m_UI_Model_gold.gameObject.activeSelf) + Convert.ToInt32(m_UI_Model_gem.gameObject.activeSelf);

            //float length = m_UI_Item_PlayerResources_Empty4Raycast.cellSize.x;
   float length = 1;
            this.m_root_RectTransform.sizeDelta = new Vector2(count*length, this.m_root_RectTransform.sizeDelta.y);
        }

        #endregion
    }
}