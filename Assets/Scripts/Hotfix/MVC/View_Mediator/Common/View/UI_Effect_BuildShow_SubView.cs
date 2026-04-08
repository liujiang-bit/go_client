// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Effect_BuildShow_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Effect_BuildShow_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Effect_BuildShow";

        public UI_Effect_BuildShow_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Effect_BuildShow_Animator;

		[HideInInspector] public PolygonImage m_img_star003_2_PolygonImage;
		[HideInInspector] public DeemoRotate m_img_star003_2_DeemoRotate;

		[HideInInspector] public PolygonImage m_img_star003_1_PolygonImage;
		[HideInInspector] public DeemoRotate m_img_star003_1_DeemoRotate;



        private void UIFinder()
        {       
			m_UI_Effect_BuildShow_Animator = gameObject.GetComponent<Animator>();

			m_img_star003_2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_star003_2");
			m_img_star003_2_DeemoRotate = FindUI<DeemoRotate>(gameObject.transform ,"img_star003_2");

			m_img_star003_1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_star003_1");
			m_img_star003_1_DeemoRotate = FindUI<DeemoRotate>(gameObject.transform ,"img_star003_1");


			BindEvent();
        }

        #endregion
    }
}