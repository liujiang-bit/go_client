// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Saturday, 17 October 2020
// Update Time         :    Saturday, 17 October 2020
// Class Description   :    UI_Model_HallBox_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using Spine;

namespace Game {
    public partial class UI_Model_HallBox_SubView : UI_SubView
    {
        public string AniClose = "UE_HallBox_close";
        public string AniJump = "UE_HallBox_jump";
        public string AniOpen = "UE_HallBox_open";

        public float PlayAni(string aniName, Action callback = null)
        {
            m_UI_Model_HallBox_SkeletonGraphic.startingAnimation = aniName;
            m_UI_Model_HallBox_SkeletonGraphic.startingLoop = false;
            m_UI_Model_HallBox_SkeletonGraphic.Initialize(true);

            float playTime = 1f;
            TrackEntry track = m_UI_Model_HallBox_SkeletonGraphic.AnimationState.GetCurrent(0);
            if (track != null)
            {
                playTime = track.Animation.Duration;
            }

            Timer.Register(playTime, () => {
                if (gameObject == null)
                {
                    return;
                }
                if (callback != null)
                {
                    callback();
                }
            });
            return playTime;
        }
    }
}