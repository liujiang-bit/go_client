// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    UI_Item_MaterialElement_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_MaterialElement_SubView : UI_SubView
    {
        private List<UI_Model_Item_SubView> m_itemSubViews = new List<UI_Model_Item_SubView>();

        protected override void BindEvent()
        {
            base.BindEvent();
            m_itemSubViews.Add(m_UI_Item1);
            m_itemSubViews.Add(m_UI_Item2);
            m_itemSubViews.Add(m_UI_Item3);
            m_itemSubViews.Add(m_UI_Item4);

        }
        
        

        public void SetInfo(int index, List<MaterialItem> infos,UnityAction<int,MaterialItem> callback,MaterialPageType type,int selectID=0)
        {
            for (int i = 0; i < m_itemSubViews.Count; i++)
            {
                if (infos.Count > i)
                {
                    var info = infos[i];
                    var itemID = info.ItemID;
                    var itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
                    bool showwRedDot = false;
                    switch (type)
                    {
                        case MaterialPageType.Mix:
                            if (info.MaterialDefine!=null && info.MaterialDefine.mix != 0 && info.MaterialType == EquipItemType.DrawingMaterial)
                            {
                                showwRedDot = info.MaterialDefine.mixCostNum <= info.ItemNum;
                            }
                            break;
                    }

                    m_itemSubViews[i].gameObject.SetActive(true);
                    m_itemSubViews[i].Refresh(itemDefine,info.ItemNum,itemID== selectID,false,showwRedDot);
                    m_itemSubViews[i].RemoveBtnAllListener();
                    m_itemSubViews[i].AddBtnListener(() =>
                    {
                        callback?.Invoke(index, info);
                    });
                }
                else
                {
                    m_itemSubViews[i].gameObject.SetActive(false);
                }
            }
        }
    }
}