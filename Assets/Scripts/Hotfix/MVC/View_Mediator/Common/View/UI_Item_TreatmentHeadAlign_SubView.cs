// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TreatmentHeadAlign_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_TreatmentHeadAlign_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TreatmentHeadAlign";

        public UI_Item_TreatmentHeadAlign_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Item_TreatmentHeadAlign_PolygonImage;

		[HideInInspector] public UI_Item_TreatmentHead_SubView m_UI_Item_TreatmentHead;


        private void UIFinder()
        {       
			m_UI_Item_TreatmentHeadAlign_PolygonImage = gameObject.GetComponent<PolygonImage>();

			m_UI_Item_TreatmentHead = new UI_Item_TreatmentHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_TreatmentHead"));

			BindEvent();
        }

        #endregion
    }
}