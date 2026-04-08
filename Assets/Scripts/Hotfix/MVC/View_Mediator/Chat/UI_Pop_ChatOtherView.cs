// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月10日
// Update Time         :    2020年7月10日
// Class Description   :    UI_Pop_ChatOtherView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_ChatOtherView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_ChatOther";

        public UI_Pop_ChatOtherView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_img_bg_GridLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_img_bg_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_chatpoint_PolygonImage;
		[HideInInspector] public GameButton m_btn_chatpoint_GameButton;

		[HideInInspector] public PolygonImage m_btn_copy_PolygonImage;
		[HideInInspector] public GameButton m_btn_copy_GameButton;

		[HideInInspector] public PolygonImage m_btn_report_PolygonImage;
		[HideInInspector] public GameButton m_btn_report_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"img_bg");
			m_img_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"img_bg");

			m_btn_chatpoint_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_chatpoint");
			m_btn_chatpoint_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/btn_chatpoint");

			m_btn_copy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_copy");
			m_btn_copy_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/btn_copy");

			m_btn_report_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_report");
			m_btn_report_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/btn_report");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}