// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MaterialElement_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MaterialElement_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MaterialElement";

        public UI_Item_MaterialElement_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_MaterialElement_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_MaterialElement_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Item1;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Item2;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Item3;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Item4;


        private void UIFinder()
        {       
			m_UI_Item_MaterialElement_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_MaterialElement_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item1"));
			m_UI_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item2"));
			m_UI_Item3 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item3"));
			m_UI_Item4 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item4"));

			BindEvent();
        }

        #endregion
    }
}