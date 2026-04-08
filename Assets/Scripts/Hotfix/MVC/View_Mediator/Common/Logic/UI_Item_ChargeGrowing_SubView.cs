// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_Item_ChargeGrowing_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using SprotoType;

namespace Game {
    public partial class UI_Item_ChargeGrowing_SubView : UI_SubView
    {
        private Vector3 m_gemPos;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_buy.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                if(m_playerProxy.GetVipLevel() < m_confgiCfg.rechargeFundVipLimit)
                {
                    Tip.CreateTip(800090, m_confgiCfg.rechargeFundVipLimit).Show();
                    CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonClickButtonCancel);
                    return;
                }
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonClickButtonSure);

                Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_confgiCfg.rechargeFundPrice);
                if (priceCfg == null) return;
                RechargeProxy rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
                rechargeProxy.CallSdkBuyByPcid(priceCfg, priceCfg.rechargeID.ToString(),priceCfg.price.ToString("N2"));
            });
        }

        private void OnItemBuy(IGGException ex, bool bUserCancel)
        {
            
        }

        public void Show(Vector3 gemPos)
        {
            m_gemPos = gemPos;
            if(!m_isInited)
            {
                Init();                
            }
            if (!m_isInited) return;
            RefreshUI();
        }

        public void OnGetGrowthFunRewardSuccess()
        {
            m_sv_list_ListView.ForceRefresh();
        }

        private void RefreshUI()
        {
            RefreshRechargeFunList();

            Data.PriceDefine priceCfg = CoreUtils.dataService.QueryRecord<Data.PriceDefine>(m_confgiCfg.rechargeFundPrice);
            if (priceCfg == null) return;
            string strPrice = string.Empty;
            var gameItems = IGGPayment.shareInstance().GetIGGGameItems();
            if (gameItems != null)
            {
                IGGGameItem funGameItem = null;
                foreach (var gameItem in gameItems)
                {
                    if(gameItem.getId() == priceCfg.rechargeID.ToString())
                    {
                        funGameItem = gameItem;
                        break;
                    }
                }
                if(funGameItem != null && funGameItem.getPurchase() != null)
                {
                    strPrice = funGameItem.getShopCurrencyPrice();
                }
            }
            if(string.IsNullOrEmpty(strPrice))
            {
                strPrice = $"${priceCfg.price}";
            }
            m_btn_buy.m_lbl_Text_LanguageText.text = strPrice;
            m_btn_buy.m_btn_languageButton_GameButton.gameObject.SetActive(!m_playerRoleInfo.growthFund);
            m_btn_buy.m_img_forbid_PolygonImage.gameObject.SetActive(m_playerProxy.GetVipLevel() < m_confgiCfg.rechargeFundVipLimit);
            m_lbl_tip_LanguageText.text = LanguageUtils.getText(800086);
            m_lbl_tip_LanguageText.gameObject.SetActive(!m_playerRoleInfo.growthFund);
        }

        private void Init()
        {
            var cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            if (cityBuildingProxy == null) return;
            var buildingInfo = cityBuildingProxy.GetBuildingInfoByType((long)(EnumCityBuildingType.TownCenter));
            if (buildingInfo == null) return;
            m_cityCenterLevel = (int)buildingInfo.level;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (m_playerProxy == null) return;
            m_playerRoleInfo = m_playerProxy.CurrentRoleInfo;
            if (m_playerRoleInfo == null) return;
            m_confgiCfg = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0);
            if (m_confgiCfg == null) return;
            m_isInited = true;
        }

        private void RefreshRechargeFunList()
        {
            InitRechargeFundData();
            m_sv_list_ListView.Clear();
            ClientUtils.PreLoadRes(gameObject, m_sv_list_ListView.ItemPrefabDataList, OnItemPrefabLoadFinish);
        }

        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            m_sv_list_ListView.SetInitData(dict, funcTab);
            m_sv_list_ListView.FillContent(m_rechargeFundDataList.Count);
        }

        private void ItemEnter(ListView.ListItem item)
        {
            UI_Item_ChargeGrowingItem_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_ChargeGrowingItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_ChargeGrowingItem_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_rechargeFundDataList.Count) return;
            subView.RefreshUI(m_playerRoleInfo, m_rechargeFundDataList[item.index], m_cityCenterLevel);
            subView.AddClaimClickListener(() =>
            {
                if (m_cityCenterLevel < m_rechargeFundDataList[item.index].needLv)
                {
                    Tip.CreateTip(800141).Show();
                    CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonClickButtonCancel);
                    return;
                }
                if (!m_playerRoleInfo.growthFund)
                {
                    Tip.CreateTip(800142).Show();
                    CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonClickButtonCancel);
                    return;
                }
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonClickButtonSure);
                SprotoType.Recharge_GetGrowthFundReward.request request = new SprotoType.Recharge_GetGrowthFundReward.request()
                {
                    id = m_rechargeFundDataList[item.index].ID
                };
                AppFacade.GetInstance().SendSproto(request);
                subView.DisableClaim();
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                mt.FlyUICurrency((int)EnumCurrencyType.denar, m_rechargeFundDataList[item.index].gem, subView.m_btn_buy.m_img_img_PolygonImage.transform.position,m_gemPos);
            });
        }

        private void InitRechargeFundData()
        {
            m_rechargeFundDataList = CoreUtils.dataService.QueryRecords<Data.RechargeFundDefine>();
            m_rechargeFundDataList.Sort(SortRechargeFund);
        }

        private int SortRechargeFund(Data.RechargeFundDefine data1, Data.RechargeFundDefine data2)
        {
            bool isCanCliam1 = IsRechargeFundCanCliam(data1);
            bool isCanCliam2 = IsRechargeFundCanCliam(data2);
            if (isCanCliam1 != isCanCliam2) return isCanCliam1 ? -1 : 1;
            bool isCliamed1 = IsRechargeFundCliamed(data1);
            bool isCliamed2 = IsRechargeFundCliamed(data2);
            if (isCliamed1 == isCliamed2)
            {
                return data1.ID.CompareTo(data2.ID);
            }
            else
            {
                return isCliamed1 ? 1 : -1;
            }           
        }

        private bool IsRechargeFundCanCliam(Data.RechargeFundDefine data)
        {
            return m_playerRoleInfo.growthFund && !m_playerRoleInfo.growthFundReward.Contains(data.ID) && m_cityCenterLevel >= data.needLv;
        }

        private bool IsRechargeFundCliamed(Data.RechargeFundDefine data)
        {
            return m_playerRoleInfo.growthFund && m_playerRoleInfo.growthFundReward.Contains(data.ID);
        }

        private Data.ConfigDefine m_confgiCfg = null;
        private int m_cityCenterLevel = 0;
        private RoleInfoEntity m_playerRoleInfo = null;
        private PlayerProxy m_playerProxy = null;
        private bool m_isInited = false;
        private List<Data.RechargeFundDefine> m_rechargeFundDataList = new List<Data.RechargeFundDefine>();
    }
}