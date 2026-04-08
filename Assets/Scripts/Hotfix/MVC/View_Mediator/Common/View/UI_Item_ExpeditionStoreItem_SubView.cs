// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ExpeditionStoreItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ExpeditionStoreItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ExpeditionStoreItem";

        public UI_Item_ExpeditionStoreItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ExpeditionStoreItem;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public LanguageText m_lbl_soldOut_LanguageText;

		[HideInInspector] public UI_Model_HorizontalButton_MiniBlue_SubView m_btn_buy;
		[HideInInspector] public LanguageText m_lbl_lastNum_LanguageText;
		[HideInInspector] public Outline m_lbl_lastNum_Outline;



        private void UIFinder()
        {       
			m_UI_Item_ExpeditionStoreItem = gameObject.GetComponent<RectTransform>();
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_lbl_soldOut_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_soldOut");

			m_btn_buy = new UI_Model_HorizontalButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_buy"));
			m_lbl_lastNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_lastNum");
			m_lbl_lastNum_Outline = FindUI<Outline>(gameObject.transform ,"lbl_lastNum");


			BindEvent();
        }

        #endregion
    }
}