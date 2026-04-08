// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月11日
// Update Time         :    2020年9月11日
// Class Description   :    UI_Item_EventTribeKingBox_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Item_EventTribeKingBox_SubView : UI_SubView
    {
        public int Index;
        public Action<int> BtnClickEvent;
        public bool m_isInit;

        private int m_boxStatus; 

        public void Init(int index)
        {
            Index = index;
            if (!m_isInit)
            {
                m_btn_box_GameButton.onClick.AddListener(ClickEvent);
                m_isInit = true;
            }
        }

        public void ClickEvent()
        {
            BtnClickEvent(Index);
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

        //变更宝箱样式
        public void ChangeSkinName(string name)
        {
            m_UI_Model_AnimationBox.m_UI_Model_AnimationBox_SkeletonGraphic.initialSkinName = name;
        }
    }
}