// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ShowResShort_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ShowResShort_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ShowResShort";

        public UI_Item_ShowResShort_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ShowResShort;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ShowResShort = gameObject.GetComponent<RectTransform>();
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_count");


			BindEvent();
        }

        #endregion
    }
}