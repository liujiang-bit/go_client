// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_EventHell_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {

    public enum EventHellBoxStatus
    {
        /// <summary>
        /// 已领取
        /// </summary>
        AlreadyReceive = 1,
        /// <summary>
        /// 可领取
        /// </summary>
        CanReceive = 2,
        /// <summary>
        /// 不可领取
        /// </summary>
        NotCanReceive = 3
    }
    public partial class UI_Item_EventHell_SubView : UI_SubView
    {
        private int m_index;
        public Action<UI_Item_EventHell_SubView> BtnAction;
        private bool m_isInit;
        private int m_boxStatus; //1 

        public void Refresh(int index)
        {
            if (!m_isInit)
            {
                m_btn_box_GameButton.onClick.AddListener(OnClick);
                m_isInit = true;
            }
            m_index = index;
        }

        public void RefreshBoxStatus(int status)
        {
            if (m_UI_Model_AnimationBox.IsOpenAni()) //宝箱正在开启动画 不允许更新状态
            {
                return;
            }
            m_boxStatus = status;
            if (status == (int)EventHellBoxStatus.AlreadyReceive) //已领取
            {
                m_UI_Model_AnimationBox.SetBox(true);
            }
            else if (status == (int)EventHellBoxStatus.CanReceive) //可领取
            {
                m_UI_Model_AnimationBox.SetBox(false, true);
            }
            else //不可领取
            {
                m_UI_Model_AnimationBox.SetBox(false, false);
            }
        }

        public void ChangeBoxStatus(int status)
        {
            m_boxStatus = status;
        }

        public float OpenBoxAni(Action callback)
        {
            return m_UI_Model_AnimationBox.OpenBoxAni(callback);
        }

        public int GetBoxStatus()
        {
            return m_boxStatus;
        }

        public int GetIndex()
        {
            return m_index;
        }

        private void OnClick()
        {
            if (BtnAction != null)
            {
                BtnAction(this);
            }
        }
    }
}