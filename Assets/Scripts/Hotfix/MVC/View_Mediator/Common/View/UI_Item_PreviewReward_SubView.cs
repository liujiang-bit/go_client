// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_PreviewReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_PreviewReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_PreviewReward";

        public UI_Item_PreviewReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_PreviewReward_ViewBinder;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public UI_Model_CaptainHeadBtn_SubView m_UI_Model_CaptainHeadBtn;


        private void UIFinder()
        {       
			m_UI_Item_PreviewReward_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_UI_Model_CaptainHeadBtn = new UI_Model_CaptainHeadBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHeadBtn"));

			BindEvent();
        }

        #endregion
    }
}