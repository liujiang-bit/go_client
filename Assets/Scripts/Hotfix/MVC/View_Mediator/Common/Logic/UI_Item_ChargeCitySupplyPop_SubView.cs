// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_ChargeCitySupplyPop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using ExcelDataReader.Log;

namespace Game {
    public partial class UI_Item_ChargeCitySupplyPop_SubView : UI_SubView
    {
        private bool m_isShow= false;

        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_closeButton_GameButton.onClick.AddListener(OnClickClose);
        }

        public void SetInfo(Data.RechargeSupplyDefine supply)
        {
            gameObject.SetActive(true);
            m_isShow = true;

            m_lbl_curnum_LanguageText.text = ClientUtils.FormatComma(supply.giveGem) + "";
            m_lbl_getdec_LanguageText.text = LanguageUtils.getTextFormat(800139,supply.continueDays);
            RewardGroupProxy rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> groupDataList = rewardGroupProxy.GetRewardDataByGroup(supply.itemPackage);
            if (groupDataList.Count > 0 && groupDataList[0].ItemData != null && groupDataList[0].ItemData.itemDefine != null)
            {
                m_UI_Model_Item.gameObject.SetActive(true);
                m_UI_Model_Item.RefreshByGroup(groupDataList[0],3);
                m_lbl_name_LanguageText.text = LanguageUtils.getText(groupDataList[0].ItemData.itemDefine.l_nameID);
            }
            else
            {
                m_UI_Model_Item.gameObject.SetActive(false);
                m_lbl_name_LanguageText.text = "";
            }
        }
        
        private void OnClickClose()
        {
            if (!m_isShow)
            {
                return;
            }

            m_isShow = false;
            gameObject.SetActive(false);
        }
    }
}