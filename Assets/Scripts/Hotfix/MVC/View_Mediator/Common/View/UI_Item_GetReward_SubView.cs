// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GetReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GetReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GetReward";

        public UI_Item_GetReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_GetReward_ViewBinder;

		[HideInInspector] public Animator m_pl_rect_Animator;
		[HideInInspector] public UIDefaultValue m_pl_rect_UIDefaultValue;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet;


        private void UIFinder()
        {       
			m_UI_Item_GetReward_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_rect_Animator = FindUI<Animator>(gameObject.transform ,"pl_rect");
			m_pl_rect_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_rect");

			m_UI_Model_RewardGet = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rect/UI_Model_RewardGet"));

			BindEvent();
        }

        #endregion
    }
}