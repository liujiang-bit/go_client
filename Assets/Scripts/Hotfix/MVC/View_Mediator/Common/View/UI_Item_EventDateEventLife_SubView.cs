// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventDateEventLife_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EventDateEventLife_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventDateEventLife";

        public UI_Item_EventDateEventLife_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventDateEventLife;
		[HideInInspector] public PolygonImage m_btn_mes_PolygonImage;
		[HideInInspector] public GameButton m_btn_mes_GameButton;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public HorizontalLayoutGroup m_img_bg_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_name_ContentSizeFitter;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;


        private void UIFinder()
        {       
			m_UI_Item_EventDateEventLife = gameObject.GetComponent<RectTransform>();
			m_btn_mes_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_mes");
			m_btn_mes_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_mes");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_mes/img_bg");
			m_img_bg_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"btn_mes/img_bg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_mes/img_bg/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_mes/img_bg/lbl_name");
			m_lbl_name_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"btn_mes/img_bg/lbl_name");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_mes/UI_Common_Redpoint"));

			BindEvent();
        }

        #endregion
    }
}