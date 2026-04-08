// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_Toggle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Common_Toggle_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_Toggle";

        public UI_Common_Toggle_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Common_Toggle_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_rect_VerticalLayoutGroup;
		[HideInInspector] public ToggleGroup m_pl_rect_ToggleGroup;

		[HideInInspector] public UI_Common_ToggleItem_SubView m_UI_Common_Toggle;


        private void UIFinder()
        {       
			m_UI_Common_Toggle_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_pl_rect_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_rect");
			m_pl_rect_ToggleGroup = FindUI<ToggleGroup>(gameObject.transform ,"pl_rect");

			m_UI_Common_Toggle = new UI_Common_ToggleItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rect/UI_Common_Toggle"));

			BindEvent();
        }

        #endregion
    }
}