// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainAddStarItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_CaptainAddStarItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainAddStarItem";

        public UI_Item_CaptainAddStarItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainAddStarItem;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public LanguageText m_lbl_starExpNum_LanguageText;

		[HideInInspector] public LanguageText m_lbl_luckyNum_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_get;


        private void UIFinder()
        {       
			m_UI_Item_CaptainAddStarItem = gameObject.GetComponent<RectTransform>();
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/UI_Model_Item"));
			m_lbl_starExpNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_starExpNum");

			m_lbl_luckyNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_luckyNum");

			m_btn_get = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_get"));

			BindEvent();
        }

        #endregion
    }
}