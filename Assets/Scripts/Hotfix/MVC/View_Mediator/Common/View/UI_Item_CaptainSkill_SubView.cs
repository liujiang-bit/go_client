// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainSkill_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainSkill_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainSkill";

        public UI_Item_CaptainSkill_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainSkill;
		[HideInInspector] public PolygonImage m_img_skillBg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_gray_PolygonImage;

		[HideInInspector] public PolygonImage m_img_lvevlBg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;

		[HideInInspector] public PolygonImage m_btn_Button_PolygonImage;
		[HideInInspector] public GameButton m_btn_Button_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_CaptainSkill = gameObject.GetComponent<RectTransform>();
			m_img_skillBg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_skillBg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");

			m_img_icon_gray_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon_gray");

			m_img_lvevlBg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_lvevlBg");

			m_lbl_level_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_level");

			m_btn_Button_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_Button");
			m_btn_Button_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_Button");


			BindEvent();
        }

        #endregion
    }
}