// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_CaptainStar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_CaptainStar_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_CaptainStar";

        public UI_Model_CaptainStar_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_CaptainStar;
		[HideInInspector] public PolygonImage m_img_starNormal_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starPreview_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_starPreview_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_starHighlight_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_CaptainStar = gameObject.GetComponent<RectTransform>();
			m_img_starNormal_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_starNormal");

			m_img_starPreview_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_starPreview");
			m_img_starPreview_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_starPreview");

			m_img_starHighlight_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_starHighlight");


			BindEvent();
        }

        #endregion
    }
}