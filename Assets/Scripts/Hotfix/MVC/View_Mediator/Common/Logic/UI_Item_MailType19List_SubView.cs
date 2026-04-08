// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月4日
// Update Time         :    2020年9月4日
// Class Description   :    UI_Item_MailType19List_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_MailType19List_SubView : UI_SubView
    {
        private bool m_isInit;
        private List<UI_Model_Item_SubView> m_itemViewList;
        private RewardGroupProxy m_rewardGroupProxy;

        public void Refresh(RoleList roleData)
        {

            m_UI_Model_PlayerHead.LoadPlayerIcon(roleData.headId, roleData.headFrameID);
            m_lbl_name_LanguageText.text = roleData.name;
            if (!m_isInit)
            {
                m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
                m_itemViewList = new List<UI_Model_Item_SubView>();
                m_itemViewList.Add(m_UI_Model_Item1);
                m_itemViewList.Add(m_UI_Model_Item2);
                m_itemViewList.Add(m_UI_Model_Item3);
                m_itemViewList.Add(m_UI_Model_Item4);
                m_itemViewList.Add(m_UI_Model_Item5);
                m_isInit = true;
            }
            if (roleData.rewardInfo == null)
            {
                Debug.LogError("RoleList.rewardInfo is null");
                return;
            }
            List<RewardGroupData> rewardList = m_rewardGroupProxy.GetRewardDataByRewardInfo(roleData.rewardInfo);
            int count = rewardList.Count;
            for (int i = 0; i < m_itemViewList.Count; i++)
            {
                if (i < count)
                {
                    m_itemViewList[i].gameObject.SetActive(true);
                    m_itemViewList[i].RefreshByGroup(rewardList[i],3);
                }
                else
                {
                    m_itemViewList[i].gameObject.SetActive(false);
                }
            }
        }
    }
}