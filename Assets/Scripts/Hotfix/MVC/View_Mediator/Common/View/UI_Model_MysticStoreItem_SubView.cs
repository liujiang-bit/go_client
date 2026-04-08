// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_MysticStoreItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_MysticStoreItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_MysticStoreItem";

        public UI_Model_MysticStoreItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_MysticStoreItem;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public PolygonImage m_img_cutBg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_cutOff_LanguageText;

		[HideInInspector] public LanguageText m_lbl_soldOut_LanguageText;

		[HideInInspector] public UI_Model_HorizontalButton_MiniYellow_SubView m_btn_buy;


        private void UIFinder()
        {       
			m_UI_Model_MysticStoreItem = gameObject.GetComponent<RectTransform>();
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_img_cutBg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_cutBg");

			m_lbl_cutOff_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_cutBg/lbl_cutOff");

			m_lbl_soldOut_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_soldOut");

			m_btn_buy = new UI_Model_HorizontalButton_MiniYellow_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_buy"));

			BindEvent();
        }

        #endregion
    }
}