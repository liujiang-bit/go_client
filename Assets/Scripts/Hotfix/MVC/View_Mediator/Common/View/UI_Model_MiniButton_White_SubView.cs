// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_MiniButton_White_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_MiniButton_White_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_MiniButton_White";

        public UI_Model_MiniButton_White_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_MiniButton_White;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;


        private void UIFinder()
        {       
			m_UI_Model_MiniButton_White = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_ClickAnimeMsg_btn"));

			BindEvent();
        }

        #endregion
    }
}