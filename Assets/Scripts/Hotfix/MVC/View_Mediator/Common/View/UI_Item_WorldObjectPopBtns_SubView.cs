// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjectPopBtns_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjectPopBtns_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjectPopBtns";

        public UI_Item_WorldObjectPopBtns_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_WorldObjectPopBtns_ViewBinder;

		[HideInInspector] public RectTransform m_pl_1g;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_Model_1gbtn1;
		[HideInInspector] public GridLayoutGroup m_pl_2g_GridLayoutGroup;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_Model_2gbtn1;
		[HideInInspector] public UI_Model_DoubleLineButton_Red2_SubView m_UI_Model_2gbtn2;
		[HideInInspector] public GridLayoutGroup m_pl_3g_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_3g_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_UI_Model_3gbtn3;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_UI_Model_3gbtn2;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_3gbtn1;


        private void UIFinder()
        {       
			m_UI_Item_WorldObjectPopBtns_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_1g = FindUI<RectTransform>(gameObject.transform ,"pl_1g");
			m_UI_Model_1gbtn1 = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_1g/UI_Model_1gbtn1"));
			m_pl_2g_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_2g");

			m_UI_Model_2gbtn1 = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_2g/UI_Model_2gbtn1"));
			m_UI_Model_2gbtn2 = new UI_Model_DoubleLineButton_Red2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_2g/UI_Model_2gbtn2"));
			m_pl_3g_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_3g");
			m_pl_3g_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_3g");

			m_UI_Model_3gbtn3 = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_3g/UI_Model_3gbtn3"));
			m_UI_Model_3gbtn2 = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_3g/UI_Model_3gbtn2"));
			m_UI_Model_3gbtn1 = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_3g/UI_Model_3gbtn1"));

			BindEvent();
        }

        #endregion
    }
}