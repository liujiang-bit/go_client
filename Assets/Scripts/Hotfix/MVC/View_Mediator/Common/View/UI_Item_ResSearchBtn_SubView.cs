// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ResSearchBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ResSearchBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ResSearchBtn";

        public UI_Item_ResSearchBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ResSearchBtn_ViewBinder;
		[HideInInspector] public GrayChildrens m_UI_Item_ResSearchBtn_MakeChildrenGray;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public LanguageText m_Txt_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ResSearchBtn_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Item_ResSearchBtn_MakeChildrenGray = gameObject.GetComponent<GrayChildrens>();

			m_img_frame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl/img_frame");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl/img_select");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl/img_icon");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl/btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"pl/btn_btn");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl/UI_Tag_ClickAnimeMsg_btn"));
			m_Txt_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl/Txt_name");


			BindEvent();
        }

        #endregion
    }
}