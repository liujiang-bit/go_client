// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeGrowingItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeGrowingItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeGrowingItem";

        public UI_Item_ChargeGrowingItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeGrowingItem;
		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_count_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_get_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_get_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_btn_buy;


        private void UIFinder()
        {       
			m_UI_Item_ChargeGrowingItem = gameObject.GetComponent<RectTransform>();
			m_img_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/bg/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/bg/img_arrow");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_icon");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_count");
			m_lbl_count_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"rect/lbl_count");

			m_lbl_get_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_count/lbl_get");
			m_lbl_get_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"rect/lbl_count/lbl_get");

			m_img_cur_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/lbl_count/img_cur");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_desc");

			m_btn_buy = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_buy"));

			BindEvent();
        }

        #endregion
    }
}