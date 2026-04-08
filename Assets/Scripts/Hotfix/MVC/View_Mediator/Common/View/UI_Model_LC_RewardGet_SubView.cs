// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_LC_RewardGet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_LC_RewardGet_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_LC_RewardGet";

        public UI_Model_LC_RewardGet_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Model_LC_RewardGet_GridLayoutGroup;
		[HideInInspector] public ViewBinder m_UI_Model_LC_RewardGet_ViewBinder;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet3;


        private void UIFinder()
        {       
			m_UI_Model_LC_RewardGet_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Model_LC_RewardGet_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_RewardGet1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_RewardGet1"));
			m_UI_Model_RewardGet2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_RewardGet2"));
			m_UI_Model_RewardGet3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_RewardGet3"));

			BindEvent();
        }

        #endregion
    }
}