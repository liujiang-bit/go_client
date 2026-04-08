// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月13日
// Update Time         :    2020年1月13日
// Class Description   :    UI_Item_QuestBox_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using DG.Tweening;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_QuestBox_SubView : UI_SubView
    {
        bool opened = false;//已打开
        public void AddClickEvent(UnityAction callback)
        {
            m_btn_box_GameButton.onClick.AddListener(callback);
        }
        public void SetOpen()
        {
            m_pl_box.OpenBox();
        }
        public void SetClose()
        {
            m_pl_bgeffect.gameObject.SetActive(false);
            m_pl_box.SetBox(false, false);

        }
        public void SetStay()
        {
            if (!opened)
            {
                m_pl_bgeffect.gameObject.SetActive(true);
                if (m_pl_bgeffect.Find(RS.TaskBoxBgEffect) == null)
                {
                    ClientUtils.UIAddEffect(RS.TaskBoxBgEffect, m_pl_bgeffect, null);
                }
                m_pl_box.SetBox(false, true);
            }
        }
        public void SetOpened()
        {
            m_pl_bgeffect.gameObject.SetActive(false);
            m_pl_box.SetBox(true, false);
            opened = true;
        }
    }
}