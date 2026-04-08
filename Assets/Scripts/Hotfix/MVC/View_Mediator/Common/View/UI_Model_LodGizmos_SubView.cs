// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_LodGizmos_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_LodGizmos_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_LodGizmos";

        public UI_Model_LodGizmos_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Model_LodGizmos_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_LodGizmos_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_text");


			BindEvent();
        }

        #endregion
    }
}