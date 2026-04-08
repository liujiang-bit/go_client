// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatShare_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ChatShare_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatShare";

        public UI_Item_ChatShare_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChatShare;
		[HideInInspector] public PolygonImage m_img_title_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_choose_PolygonImage;
		[HideInInspector] public GameButton m_btn_choose_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_ChatShare = gameObject.GetComponent<RectTransform>();
			m_img_title_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_title");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_title");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec");
			m_lbl_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_btn_choose_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_choose");
			m_btn_choose_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_choose");


			BindEvent();
        }

        #endregion
    }
}