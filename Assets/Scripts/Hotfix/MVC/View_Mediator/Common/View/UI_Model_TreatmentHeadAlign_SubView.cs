// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_TreatmentHeadAlign_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_TreatmentHeadAlign_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_TreatmentHeadAlign";

        public UI_Model_TreatmentHeadAlign_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public HorizontalLayoutGroup m_UI_Model_TreatmentHeadAlign_HorizontalLayoutGroup;

		[HideInInspector] public UI_Item_TreatmentHeadAlign_SubView m_UI_Item_TreatmentHeadAlign4;
		[HideInInspector] public UI_Item_TreatmentHeadAlign_SubView m_UI_Item_TreatmentHeadAlign3;
		[HideInInspector] public UI_Item_TreatmentHeadAlign_SubView m_UI_Item_TreatmentHeadAlign2;
		[HideInInspector] public UI_Item_TreatmentHeadAlign_SubView m_UI_Item_TreatmentHeadAlign1;


        private void UIFinder()
        {       
			m_UI_Model_TreatmentHeadAlign_HorizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();

			m_UI_Item_TreatmentHeadAlign4 = new UI_Item_TreatmentHeadAlign_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_TreatmentHeadAlign4"));
			m_UI_Item_TreatmentHeadAlign3 = new UI_Item_TreatmentHeadAlign_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_TreatmentHeadAlign3"));
			m_UI_Item_TreatmentHeadAlign2 = new UI_Item_TreatmentHeadAlign_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_TreatmentHeadAlign2"));
			m_UI_Item_TreatmentHeadAlign1 = new UI_Item_TreatmentHeadAlign_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_TreatmentHeadAlign1"));

			BindEvent();
        }

        #endregion
    }
}