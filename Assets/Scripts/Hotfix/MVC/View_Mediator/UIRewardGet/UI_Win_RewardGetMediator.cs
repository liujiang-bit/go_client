// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Win_RewardGetMediator
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
using UnityEngine.UI;
using System;

namespace Game {
    public class RewardGetData
    {
        //标题
        public string  title;
        //奖励数据
        public List<RewardGroupData> rewardGroupDataList;
        public bool playSound = false;
        public bool playTitleEffect = false;
        public int nameType = 1;//itemtipname，itemname
        //点击关闭的回调
        public Action CloseCallback;
    }
    public class UI_Win_RewardGetMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_RewardGetMediator";

        private RewardGetData m_rewardData;
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private const float intervalTime = 0.3f;

        #endregion

        //IMediatorPlug needs
        public UI_Win_RewardGetMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_RewardGetView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
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
            if (m_rewardData.playSound)
            {
                CoreUtils.audioService.PlayOneShot(RS.Sound_Ui_WinGetReward);
            }
            if (m_rewardData.playTitleEffect)
            {
                ClientUtils.UIAddEffect(RS.TitleEffect, view.m_lbl_title_LanguageText.transform,null);
            }
        }

        public override void WinClose()
        {
            m_rewardData.CloseCallback?.Invoke();
        }

        public override void OnRemove()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideCheck, (int)EnumFuncGuide.ReceiveVipBoxIntro);
        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_rewardData = view.data as RewardGetData;
            view.m_btn_close_1_GameButton.gameObject.SetActive(true);
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
            m_preLoadRes.Add("UI_Item_GetReward");
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                OnLoadFinnish();
            });


        }
        private void OnLoadFinnish()
        {
            Transform rewards = view.gameObject.transform.Find("reward").transform;
            rewards.DestroyAllChild();
            for (int i = 0; i < m_rewardData.rewardGroupDataList.Count; i++)
            {
                RewardGroupData rewardGroupData = m_rewardData.rewardGroupDataList[i];
                GameObject gameObject = CoreUtils.assetService.Instantiate(m_assetDic["UI_Item_GetReward"]);
                UI_Item_GetReward_SubView getRewardView = new UI_Item_GetReward_SubView(gameObject.GetComponent<RectTransform>());
                gameObject.transform.SetParent(rewards);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                switch ((EnumRewardType)rewardGroupData.RewardType)
                {
                    case EnumRewardType.Currency:
                        getRewardView.m_UI_Model_RewardGet. m_UI_Model_Item.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_soldier_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_package_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_cur_ViewBinder.gameObject.transform.localScale = Vector3.one;
                        getRewardView.m_UI_Model_RewardGet.RefreshReward(rewardGroupData.CurrencyData.currencyDefine, rewardGroupData.number);
                       gameObject.name = rewardGroupData.CurrencyData.ID.ToString();
                        break;
                    case EnumRewardType.Item:
                        getRewardView.m_UI_Model_RewardGet.m_UI_Model_Item.gameObject.transform.localScale = Vector3.one;
                        getRewardView.m_UI_Model_RewardGet.m_pl_soldier_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_package_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_cur_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.RefreshReward(rewardGroupData.ItemData.itemDefine, rewardGroupData.number, m_rewardData.nameType);
                        gameObject.name = rewardGroupData.ItemData.ID.ToString();
                        break;
                    case EnumRewardType.Soldier:
                        getRewardView.m_UI_Model_RewardGet.m_UI_Model_Item.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_soldier_ViewBinder.gameObject.transform.localScale = Vector3.one;
                        getRewardView.m_UI_Model_RewardGet.m_pl_package_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_cur_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.RefreshReward(rewardGroupData.SoldierData.armsDefine, rewardGroupData.number);
                        gameObject.name = rewardGroupData.SoldierData.ID.ToString();
                        break;
                    case EnumRewardType.Hero:
                        getRewardView.m_UI_Model_RewardGet.m_UI_Model_Item.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_soldier_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.m_pl_package_ViewBinder.gameObject.transform.localScale = Vector3.one;
                        getRewardView.m_UI_Model_RewardGet.m_pl_cur_ViewBinder.gameObject.transform.localScale = Vector3.zero;
                        getRewardView.m_UI_Model_RewardGet.RefreshReward(rewardGroupData.HeroData.HeroDefine, rewardGroupData.number);
                        gameObject.name = rewardGroupData.HeroData.ID.ToString();
                        break;
                }

                if (i == m_rewardData.rewardGroupDataList.Count - 1)
                {
                    ShowAnim();
                }
            }
         
        }

        #endregion
        public void ShowAnim()
        {
            int num = 1;
         
                Timer timer = null;
                Transform rewards = view.gameObject.transform.Find("reward").transform;

                LayoutRebuilder.ForceRebuildLayoutImmediate(rewards.GetComponent<RectTransform>());
                {
                    UI_Item_GetReward_SubView getRewardView = new UI_Item_GetReward_SubView(rewards.GetChild(num).GetComponent<RectTransform>());
                    getRewardView.gameObject.SetActive(true);
                    getRewardView.m_pl_rect_Animator.Play("Open");
                    CoreUtils.audioService.PlayOneShot(RS.sfx_collect_reward);
            }
            if (m_rewardData.rewardGroupDataList.Count > 1)
            {
                timer = Timer.Register(intervalTime, () =>
                {
                    UI_Item_GetReward_SubView getRewardView = new UI_Item_GetReward_SubView(rewards.GetChild(num).GetComponent<RectTransform>());
                    getRewardView.gameObject.SetActive(true);
                    getRewardView.m_pl_rect_Animator.Play("Open");
                    CoreUtils.audioService.PlayOneShot(RS.sfx_collect_reward);
                    num++;
                    if (num == m_rewardData.rewardGroupDataList.Count)
                    {
                        if (timer != null)
                        {
                            timer.Cancel();
                            timer = null;
                        }
                        view.m_btn_close_1_GameButton.onClick.RemoveAllListeners();
                        view.m_btn_close_1_GameButton.onClick.AddListener(() => { FlyReward(); view.m_btn_close_1_GameButton.onClick.RemoveAllListeners(); });
                    }
                }, null, true, false, view.vb);
            }
            else
            {
                view.m_btn_close_1_GameButton.onClick.RemoveAllListeners();
                view.m_btn_close_1_GameButton.onClick.AddListener(() => { FlyReward(); view.m_btn_close_1_GameButton.onClick.RemoveAllListeners(); });
            }
        }

        private void FlyReward()
        {
            Transform rewards = view.gameObject.transform.Find("reward").transform;
            for (int i = 0; i < m_rewardData.rewardGroupDataList.Count; i++)
            {
                RewardGroupData rewardGroupData = m_rewardData.rewardGroupDataList[i];
                switch ((EnumRewardType)rewardGroupData.RewardType)
                {
                    case EnumRewardType.Currency:
                        {
                            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                            mt.FlyUICurrency(rewardGroupData.CurrencyData.ID, (int)rewardGroupData.number, rewards.GetChild(i).transform.position, () => { if(CoreUtils.uiManager.ExistUI(UI.s_rewardGet)) CoreUtils.uiManager.CloseUI(UI.s_rewardGet); });

                        }
                        break;
                    case EnumRewardType.Soldier:
                        {
                            //飘飞特效
                            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                            mt.FlyPowerUpEffect(rewards.GetChild(i).gameObject, rewards.GetChild(i).transform.GetComponent<RectTransform>(), Vector3.one, () => { if (CoreUtils.uiManager.ExistUI(UI.s_rewardGet)) CoreUtils.uiManager.CloseUI(UI.s_rewardGet); });
                        }
                        break;
                    case EnumRewardType.Item:
                        {
                            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                            mt.FlyItemEffect(rewardGroupData.ItemData.ID, (int)rewardGroupData.number, rewards.GetChild(i).GetComponent<RectTransform>(),()=> { if (CoreUtils.uiManager.ExistUI(UI.s_rewardGet)) CoreUtils.uiManager.CloseUI(UI.s_rewardGet); });
                        }
                        break;
                }
            }
        }
    }
    
}