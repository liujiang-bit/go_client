// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_Item_VipListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using PureMVC.Core;

namespace Game {
    public partial class UI_Item_VipListItem_SubView : UI_SubView
    {
        public Action<int,string, Vector3,float> OnShowBoxTip;
        private VipDefine m_vipDefine;
        private Timer m_freeBoxTimer = null;
        private bool m_isCurrentLevel = false;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<VipAttDefine> m_vipAttDefineList = new List<VipAttDefine>();
        private RechargeProxy m_rechargeProxy;

        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_freebox_GameButton.onClick.AddListener(OnClickShowFreeBox);
            m_btn_speicalbox_GameButton.onClick.AddListener(OnClickShowSpecialBox);
            m_btn_get.AddClickEvent(OnClickClaim);
            m_btn_buy.AddClickEvent(OnClickBuy);
            ClientUtils.PreLoadRes(gameObject, m_sv_listBuff_ListView.ItemPrefabDataList, LoadBuffItemFinish);

        }

        public void SetInfo(VipDefine vipDefine,int currentLevel,bool freeBoxOpened,bool specialBoxOpened)
        {
            m_rechargeProxy = AppFacade.GetInstance().RetrieveProxy(RechargeProxy.ProxyNAME) as RechargeProxy;
            m_vipDefine = vipDefine;
            m_lbl_titleBox_LanguageText.text = LanguageUtils.getTextFormat(800021, vipDefine.level);
            m_lbl_titleBuff_LanguageText.text = LanguageUtils.getTextFormat(800022, vipDefine.level);
            RefreshDailyBox(vipDefine.level, currentLevel,freeBoxOpened,ServerTimeModule.Instance.GetServerTime()+ServerTimeModule.Instance.GetDistanceZeroTime());
            RefreshSpecialBox(vipDefine.level,currentLevel,specialBoxOpened,vipDefine.price);
            RefreshBuffInfo(m_vipDefine.level);

        }

        public void ClearTimer()
        {
            if (m_freeBoxTimer != null)
            {
                m_freeBoxTimer.Cancel();
                m_freeBoxTimer = null;
            }
        }
        
        private void RefreshDailyBox(int vipLevel,int currentLevel,bool isOpened,long refreshTime)
        {
            ClearTimer();
            if (currentLevel == vipLevel)
            {
                m_btn_get.gameObject.SetActive(!isOpened);
                m_lbl_time_LanguageText.gameObject.SetActive(isOpened);
                m_pl_freebox.SetBox(isOpened);
                if (isOpened)
                {
                    UpdateTime(refreshTime);
                    m_freeBoxTimer = Timer.Register(1, () =>
                    {
                        UpdateTime(refreshTime);
                    }, null, true);
                }
            }
            else
            {
                m_btn_get.gameObject.SetActive(false);
                m_lbl_time_LanguageText.gameObject.SetActive(true);
                m_pl_freebox.SetBox(false);
                m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(800023, vipLevel);
            }
        }

        private void UpdateTime(long endTime)
        {
            var remainTimeStr = UIHelper.GetHMSCounterDown(endTime);
            if (remainTimeStr ==null)
            {
                ClearTimer();
                return;
            }
            m_lbl_time_LanguageText.text = remainTimeStr;
        }

        private void RefreshSpecialBox(int vipLevel,int currentLevel,bool isOpened,int price)
        {

            if (currentLevel>=vipLevel)
            {
                m_btn_buy.gameObject.SetActive(!isOpened);
                m_lbl_soldOut_LanguageText.gameObject.SetActive(isOpened);
                m_pl_speicalbox.SetBox(isOpened);
                if (!isOpened)
                {
                    m_btn_buy.m_lbl_Text_LanguageText.text = m_rechargeProxy.GetPriceString(price);
                }
                else
                {
                    m_lbl_soldOut_LanguageText.text = LanguageUtils.getText(787009);
                }
            }
            else
            {
                m_btn_buy.gameObject.SetActive(false);
                m_lbl_soldOut_LanguageText.gameObject.SetActive(true);
                m_pl_speicalbox.SetBox(false);
                m_lbl_soldOut_LanguageText.text = LanguageUtils.getTextFormat(800143, vipLevel);
            }
        }

        private void RefreshBuffInfo(int vipLevel)
        {
            if (m_assetDic.Count <= 0)
            {
                return;
            }
            m_vipAttDefineList.Clear();
            var vipAttDefineList = CoreUtils.dataService.QueryRecords<Data.VipAttDefine>();
            foreach (var vipAttDefine in vipAttDefineList)
            {
                if (vipAttDefine.levelGroup == vipLevel && vipAttDefine.l_attID!=0)
                {
                    m_vipAttDefineList.Add(vipAttDefine);
                }
            }
            m_sv_listBuff_ListView.FillContent(m_vipAttDefineList.Count);
            m_sv_listBuff_ListView.MovePanelToItemIndex(0);
        }

        private void InitBuffListItem(ListView.ListItem item)
        {
            UI_Item_VipBuffListItem_SubView buffListItem = null;
            if (item.data != null)
            {
                buffListItem = item.data as UI_Item_VipBuffListItem_SubView;
            }
            else
            {
                buffListItem = new UI_Item_VipBuffListItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = buffListItem;
            }

            var vipAttDefine = m_vipAttDefineList[item.index];
            
            buffListItem.m_img_new_PolygonImage.gameObject.SetActive(vipAttDefine.newSign == 1);
            if (vipAttDefine.l_attID != 0)
            {
                string param = "";
                if (vipAttDefine.integer == 1)
                {
                    param = vipAttDefine.add.ToString();
                }
                else
                {
                    param = vipAttDefine.add / 10 + "%";
                }
                
                if (vipAttDefine.add > 0)
                {
                    param = "+" + param;
                }

                buffListItem.m_lbl_buff_LanguageText.text = LanguageUtils.getTextFormat(vipAttDefine.l_attID,param);
            }
            else
            {
                buffListItem.m_lbl_buff_LanguageText.text = "";
            }
        }


        private void LoadBuffItemFinish(Dictionary<string, IAsset> dic)
        {

            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitBuffListItem;
            m_sv_listBuff_ListView.SetInitData(m_assetDic, functab);
            RefreshBuffInfo(m_vipDefine.level);

        }
        

        private void OnClickClaim()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.VipClaimFreeBox, m_vipDefine.level);

        }

        private void OnClickBuy()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.VipBuySpecialBox,  m_vipDefine.price);
        }

        private void OnClickShowFreeBox()
        {
            OnShowBoxTip?.Invoke(m_vipDefine.freeBox,LanguageUtils.getText(800028),m_btn_freebox_PolygonImage.transform.position,m_btn_freebox_PolygonImage.rectTransform.rect.width/2);
        }

        private void OnClickShowSpecialBox()
        {
            OnShowBoxTip?.Invoke(m_vipDefine.specialBox,LanguageUtils.getText(800029),m_btn_speicalbox_PolygonImage.transform.position,m_btn_freebox_PolygonImage.rectTransform.rect.width/2);
        }
    }
}