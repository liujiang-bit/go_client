// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_SideBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_SideBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_SideBtn";

        public UI_Model_SideBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Model_SideBtn_GridLayoutGroup;

		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_1;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_2;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_3;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_4;


        private void UIFinder()
        {       
			m_UI_Model_SideBtn_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();

			m_UI_Model_PageButton_1 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PageButton_1"));
			m_UI_Model_PageButton_2 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PageButton_2"));
			m_UI_Model_PageButton_3 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PageButton_3"));
			m_UI_Model_PageButton_4 = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PageButton_4"));

			BindEvent();
        }

        #endregion
    }
}