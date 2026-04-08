// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_SideToggleGroupBtns_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_SideToggleGroupBtns_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_SideToggleGroupBtns";

        public UI_Model_SideToggleGroupBtns_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public VerticalLayoutGroup m_UI_Model_SideToggleGroupBtns_VerticalLayoutGroup;

		[HideInInspector] public ToggleGroup m_pl_group_ToggleGroup;
		[HideInInspector] public LayoutElement m_pl_group_LayoutElement;

		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side1;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side2;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side3;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side4;


        private void UIFinder()
        {       
			m_UI_Model_SideToggleGroupBtns_VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();

			m_pl_group_ToggleGroup = FindUI<ToggleGroup>(gameObject.transform ,"pl_group");
			m_pl_group_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_group");

			m_pl_side1 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_side1"));
			m_pl_side2 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_side2"));
			m_pl_side3 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_side3"));
			m_pl_side4 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_side4"));

			BindEvent();
        }

        #endregion
    }
}