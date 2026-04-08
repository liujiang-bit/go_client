// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月28日
// Update Time         :    2020年9月28日
// Class Description   :    UI_Model_GuideAnim_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.IO;

namespace Game {
    public partial class UI_Model_GuideAnim_SubView : UI_SubView
    {
        private bool m_isPlay = false;

        public void Play()
        {
            m_img_movie_VideoPlayer.gameObject.SetActive(false);
            if (m_isPlay)
            {
                return;
            }
            m_isPlay = true;
            m_img_movie_VideoPlayer.url = Path.Combine(Application.streamingAssetsPath, "guildBuildGuide.mp4");
            m_img_movie_VideoPlayer.gameObject.SetActive(true);
            m_img_movie_VideoPlayer.Play();
        }
    }
}