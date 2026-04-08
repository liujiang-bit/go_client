// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_Bag_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_Bag_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_Bag";

        public UI_Item_Bag_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_Bag_ViewBinder;
		[HideInInspector] public UIDefaultValue m_UI_Item_Bag_UIDefaultValue;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public RectTransform m_pl_new;


        private void UIFinder()
        {       
			m_UI_Item_Bag_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Item_Bag_UIDefaultValue = gameObject.GetComponent<UIDefaultValue>();

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_pl_new = FindUI<RectTransform>(gameObject.transform ,"pl_new");

			BindEvent();
        }

        #endregion
    }
}