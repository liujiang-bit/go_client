// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_GuideAnim_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;
using UnityEngine.Video;

namespace Game {
    public partial class UI_Model_GuideAnim_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_GuideAnim";

        public UI_Model_GuideAnim_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_GuideAnim;
		[HideInInspector] public RawImage m_img_movie_RawImage;
		[HideInInspector] public VideoPlayer m_img_movie_VideoPlayer;



        private void UIFinder()
        {       
			m_UI_Model_GuideAnim = gameObject.GetComponent<RectTransform>();
			m_img_movie_RawImage = FindUI<RawImage>(gameObject.transform ,"img_movie");
			m_img_movie_VideoPlayer = FindUI<VideoPlayer>(gameObject.transform ,"img_movie");


			BindEvent();
        }

        #endregion
    }
}