// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventDateListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventDateListItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventDateListItem";

        public UI_Item_EventDateListItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_EventDateListItem_ViewBinder;

		[HideInInspector] public GameButton m_btn_click_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_click_Empty4Raycast;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bgActive_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bgActive_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redpot_PolygonImage;

		[HideInInspector] public LanguageText m_img_redpot_LanguageText;

		[HideInInspector] public PolygonImage m_img_new_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_EventDateListItem_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_click_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_click");
			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"btn_click");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_click/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_click/img_bg");

			m_img_bgActive_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_click/img_bgActive");
			m_img_bgActive_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_click/img_bgActive");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_click/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_click/img_icon");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_click/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_click/lbl_title");

			m_img_redpot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_click/img_redpot");

			m_img_redpot_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_click/img_redpot/img_redpot");

			m_img_new_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_click/img_new");


			BindEvent();
        }

        #endregion
    }
}