// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TreatmentHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_TreatmentHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TreatmentHead";

        public UI_Item_TreatmentHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Item_TreatmentHead_PolygonImage;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_TreatmentHead_PolygonImage = gameObject.GetComponent<PolygonImage>();

			m_img_char_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_char");


			BindEvent();
        }

        #endregion
    }
}