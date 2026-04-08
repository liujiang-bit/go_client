// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_Redpoint_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Common_Redpoint_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_Redpoint";

        public UI_Common_Redpoint_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Common_Redpoint;
		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;

		[HideInInspector] public PolygonImage m_img_redpointNum_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;

		[HideInInspector] public PolygonImage m_img_new_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Common_Redpoint = gameObject.GetComponent<RectTransform>();
			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_redpoint");

			m_img_redpointNum_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_redpointNum");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_redpointNum/lbl_num");

			m_img_new_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_new");


			BindEvent();
        }

        #endregion
    }
}