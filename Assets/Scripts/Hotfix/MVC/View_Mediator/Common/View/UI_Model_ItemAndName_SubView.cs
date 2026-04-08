// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_ItemAndName_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_ItemAndName_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_ItemAndName";

        public UI_Model_ItemAndName_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_ItemAndName_ViewBinder;
		[HideInInspector] public UIDefaultValue m_UI_Model_ItemAndName_UIDefaultValue;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public LanguageText m_lbl_itemName_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_ItemAndName_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Model_ItemAndName_UIDefaultValue = gameObject.GetComponent<UIDefaultValue>();

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_lbl_itemName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_itemName");


			BindEvent();
        }

        #endregion
    }
}