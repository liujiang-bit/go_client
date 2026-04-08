// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_Item_VipStoreList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_VipStoreList_SubView : UI_SubView
    {
        private int m_itemIDZero = 0;
        private int m_itemIDOne = 0;

        public void SetInfo(VipStoreDefine vipStoreDefine1,VipStoreDefine vipStoreDefine2,Action<VipStoreDefine,int,Transform> OnClickCallback)
        {
            m_itemIDZero = vipStoreDefine1.ID;
            m_itemIDOne = vipStoreDefine2==null?0: vipStoreDefine2.ID;
            m_UI_Item_VipStoreItem0.SetInfo(vipStoreDefine1,OnClickCallback);
            m_UI_Item_VipStoreItem1.SetInfo(vipStoreDefine2,OnClickCallback);
        }

        public void SetBtnBuyPosition(Transform tran,int ID)
        {
            var btnTransform = ID== m_itemIDZero? m_UI_Item_VipStoreItem0.m_btn_buy.gameObject.transform: m_UI_Item_VipStoreItem1.m_btn_buy.gameObject.transform;
            tran.position = btnTransform.position;
        }
    }
}