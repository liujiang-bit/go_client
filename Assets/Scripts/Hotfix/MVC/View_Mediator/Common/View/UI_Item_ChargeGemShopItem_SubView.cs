// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeGemShopItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeGemShopItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeGemShopItem";

        public UI_Item_ChargeGemShopItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeGemShopItem;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_count_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_gem_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_price_LanguageText;

		[HideInInspector] public PolygonImage m_img_extra_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_addName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_add_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ChargeGemShopItem = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_icon");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_count");
			m_lbl_count_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"btn_btn/lbl_count");

			m_img_gem_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/lbl_count/img_gem");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_name");

			m_lbl_price_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_price");

			m_img_extra_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_extra");

			m_lbl_addName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_addName");

			m_lbl_add_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_add");


			BindEvent();
        }

        #endregion
    }
}