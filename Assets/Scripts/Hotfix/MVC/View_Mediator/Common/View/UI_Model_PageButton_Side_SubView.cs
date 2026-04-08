// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_PageButton_Side_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_PageButton_Side_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_PageButton_Side";

        public UI_Model_PageButton_Side_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_PageButton_Side;
		[HideInInspector] public PolygonImage m_img_dark_PolygonImage;

		[HideInInspector] public PolygonImage m_img_iconD_PolygonImage;

		[HideInInspector] public PolygonImage m_img_highLight_PolygonImage;

		[HideInInspector] public PolygonImage m_img_iconH_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_redpot_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_reddotcount_LanguageText;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;


        private void UIFinder()
        {       
			m_UI_Model_PageButton_Side = gameObject.GetComponent<RectTransform>();
			m_img_dark_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_dark");

			m_img_iconD_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_dark/img_iconD");

			m_img_highLight_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_highLight");

			m_img_iconH_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_highLight/img_iconH");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_redpot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_redpot");

			m_lbl_reddotcount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_redpot/lbl_reddotcount");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Common_Redpoint"));

			BindEvent();
        }

        #endregion
    }
}