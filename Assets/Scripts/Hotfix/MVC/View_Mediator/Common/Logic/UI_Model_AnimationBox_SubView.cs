// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Model_AnimationBox_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine;
using System;

namespace Game {
    public partial class UI_Model_AnimationBox_SubView : UI_SubView
    {
        private bool m_isOpen = false;
        private bool m_isOpenAni = false;
        public void SetBox(bool isOpened,bool isCanOpen = false)
        {
            m_isOpen = isOpened;
            m_UI_Model_AnimationBox_SkeletonGraphic.startingLoop = true;
            if (isOpened)
            {
                m_UI_Model_AnimationBox_SkeletonGraphic.startingAnimation = "UE_TreasureBox_opened";
            }
            else
            {
                if (isCanOpen)
                {
                    m_UI_Model_AnimationBox_SkeletonGraphic.startingAnimation = "UE_TreasureBox_stay";
                }
                else
                    m_UI_Model_AnimationBox_SkeletonGraphic.startingAnimation = "UE_TreasureBox_close";
            }
            
            m_UI_Model_AnimationBox_SkeletonGraphic.Initialize(true);
        }

        public void OpenBox()
        {
            if (m_isOpen)
            {
                return;
            }
            else
            {
                m_UI_Model_AnimationBox_SkeletonGraphic.startingAnimation = "UE_TreasureBox_open";
                m_UI_Model_AnimationBox_SkeletonGraphic.startingLoop = false;
                m_UI_Model_AnimationBox_SkeletonGraphic.Initialize(true);
            }
        }

        public float OpenBoxAni(Action callback = null)
        {
            m_isOpenAni = true;
            m_UI_Model_AnimationBox_SkeletonGraphic.startingAnimation = "UE_TreasureBox_open";
            m_UI_Model_AnimationBox_SkeletonGraphic.startingLoop = false;
            m_UI_Model_AnimationBox_SkeletonGraphic.Initialize(true);

            float playTime = 1f;
            TrackEntry track = m_UI_Model_AnimationBox_SkeletonGraphic.AnimationState.GetCurrent(0);
            if (track != null)
            {
                playTime = track.Animation.Duration;
            } 
            Timer.Register(playTime, ()=> {
                if (gameObject == null)
                {
                    return;
                }
                m_isOpenAni = false;
                if (callback != null)
                {
                    callback();
                }
            });
            return playTime;
        }

        public bool IsOpened()
        {
            return m_isOpen;
        }

        public bool IsOpenAni()
        {
            return m_isOpenAni;
        }
    }
}