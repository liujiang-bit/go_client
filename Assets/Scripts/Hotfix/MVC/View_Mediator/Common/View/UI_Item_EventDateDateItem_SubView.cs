// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventDateDateItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EventDateDateItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventDateDateItem";

        public UI_Item_EventDateDateItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventDateDateItem;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_selectBg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_week_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_week_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_day_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_day_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventDateDateItem = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_img_selectBg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_selectBg");

			m_lbl_week_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_week");
			m_lbl_week_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_week");

			m_lbl_day_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_day");
			m_lbl_day_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_day");


			BindEvent();
        }

        #endregion
    }
}