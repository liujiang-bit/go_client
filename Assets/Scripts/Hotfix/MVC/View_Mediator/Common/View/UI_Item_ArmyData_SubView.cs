// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyData_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyData_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyData";

        public UI_Item_ArmyData_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_ArmyData_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_white_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_white_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;


        private void UIFinder()
        {       
			m_UI_Item_ArmyData_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_white_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_white");
			m_img_white_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/img_white");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/lbl_text");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/img_icon");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_ClickAnimeMsg_btn"));

			BindEvent();
        }

        #endregion
    }
}