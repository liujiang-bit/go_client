// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月3日
// Update Time         :    2020年2月3日
// Class Description   :    EmailEnclosureMediator
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

namespace Game {
    public class EmailEnclosureMediator : GameMediator {
        #region Member
        public static string NameMediator = "EmailEnclosureMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private RewardInfo m_reward;
        #endregion

        //IMediatorPlug needs
        public EmailEnclosureMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public EmailEnclosureView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
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

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {

        }

        protected override void BindUIEvent()
        {
            view.m_pl_btn.m_btn_languageButton_GameButton.onClick.AddListener(OnClose);
            view.m_UI_Model_Window_TypeSmall.m_btn_close_GameButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_EmailEnclosure);
        }

        protected override void BindUIData()
        {
            m_reward = view.data as RewardInfo;
            List<string> prefabs = new List<string>();
            prefabs.Add("UI_LC_MailReward");//物品图标
            ClientUtils.PreLoadRes(view.gameObject, prefabs, OnLoadFinish);
        }

        #endregion

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assetDic = asset;
            InitView();
        }

        private void InitView()
        {
            Transform parent = view.m_sv_list_view_ScrollRect.content;
            if (m_reward.food>0)
            {
                SetCurrencyGift(InstantiateGift(parent), (int)EnumCurrencyType.food, m_reward.food);
            }
            if (m_reward.wood > 0)
            {
                SetCurrencyGift(InstantiateGift(parent), (int)EnumCurrencyType.wood, m_reward.wood);
            }
            if (m_reward.stone > 0)
            {
                SetCurrencyGift(InstantiateGift(parent), (int)EnumCurrencyType.stone, m_reward.stone);
            }
            if (m_reward.gold > 0)
            {
                SetCurrencyGift(InstantiateGift(parent), (int)EnumCurrencyType.gold, m_reward.gold);
            }
            if (m_reward.denar > 0)
            {
                SetCurrencyGift(InstantiateGift(parent), (int)EnumCurrencyType.denar, m_reward.denar);
            }
            if (m_reward.soldiers != null)
            {
                for (int i = 0; i < m_reward.soldiers.Count; i++)
                {
                    UI_LC_MailRewardView itemView = InstantiateGift(parent);
                    itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = m_reward.soldiers[i].num.ToString("N0");
                    itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
                    itemView.m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(false);

                    ArmsDefine armsDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)m_reward.soldiers[i].id);
                    itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(armsDefine.l_armsID);
                    //设置icon
                    ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_icon_PolygonImage, armsDefine.icon);
                }
            }
            if (m_reward.items != null)
            {
                for (int i = 0; i < m_reward.items.Count; i++)
                {
                    UI_LC_MailRewardView itemView = InstantiateGift(parent);
                    itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = m_reward.items[i].itemNum.ToString("N0");
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_reward.items[i].itemId);
                    itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
                    itemView.m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(true);
                    //设置icon
                    ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_icon_PolygonImage, itemDefine.itemIcon);
                    ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_quality_PolygonImage, RS.ItemQualityBg[itemDefine.quality - 1]);
                    if (itemDefine.l_topID >= 1)
                    {
                        itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(true);
                        itemView.m_UI_Model_Item.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), itemDefine.topData);
                    }
                }
            }
        }

        private void SetCurrencyGift(UI_LC_MailRewardView itemView, int id, long num)
        {
            itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
            itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = num.ToString("N0");
            itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
            itemView.m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(false);
            CurrencyDefine currencyDefine = CoreUtils.dataService.QueryRecord<CurrencyDefine>(id);
            itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(currencyDefine.l_desID);
            //设置icon
            ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_icon_PolygonImage, currencyDefine.iconID);
        }

        private UI_LC_MailRewardView InstantiateGift(Transform parent)
        {
            GameObject go = CoreUtils.assetService.Instantiate(m_assetDic["UI_LC_MailReward"]);
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            UI_LC_MailRewardView giftView = MonoHelper.AddHotFixViewComponent<UI_LC_MailRewardView>(go);
            return giftView;
        }
    }
}