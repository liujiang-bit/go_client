// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Item_ResSearchBtnView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ResSearchBtnView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ResSearchBtn";

        public UI_Item_ResSearchBtnView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public LanguageText m_Txt_name_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_select_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl/img_select");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl/img_icon");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl/btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"pl/btn_btn");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(vb.transform ,"pl/UI_Tag_ClickAnimeMsg_btn"));
			m_Txt_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl/Txt_name");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}