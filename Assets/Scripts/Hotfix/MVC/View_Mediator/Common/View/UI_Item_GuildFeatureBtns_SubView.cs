// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GuildFeatureBtns_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GuildFeatureBtns_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GuildFeatureBtns";

        public UI_Item_GuildFeatureBtns_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_GuildFeatureBtns_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;
		[HideInInspector] public PolygonImage m_img_build_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_GuildFeatureBtns_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_bg");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_name");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Common_Redpoint"));
			m_img_build_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_build");


			BindEvent();
        }

        #endregion
    }
}