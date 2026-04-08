// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeCitySuppyItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeCitySuppyItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeCitySuppyItem";

        public UI_Item_ChargeCitySuppyItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeCitySuppyItem;
		[HideInInspector] public PolygonImage m_img_card_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_tips_PolygonImage;
		[HideInInspector] public GameButton m_btn_tips_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_get_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_get_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniYellow_SubView m_btn_buy;
		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_btn_get;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ChargeCitySuppyItem = gameObject.GetComponent<RectTransform>();
			m_img_card_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_card");

			m_btn_tips_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/btn_tips");
			m_btn_tips_GameButton = FindUI<GameButton>(gameObject.transform ,"rect/btn_tips");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name");

			m_lbl_get_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_get");
			m_lbl_get_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"rect/lbl_get");

			m_img_cur_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/lbl_get/img_cur");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_dec");

			m_btn_buy = new UI_Model_StandardButton_MiniYellow_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_buy"));
			m_btn_get = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_get"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_time");


			BindEvent();
        }

        #endregion
    }
}