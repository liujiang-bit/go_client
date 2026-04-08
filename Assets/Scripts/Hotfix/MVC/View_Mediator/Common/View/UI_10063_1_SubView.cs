// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_10063_1_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_10063_1_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_10063_1";

        public UI_10063_1_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_10063_1_Animator;

		[HideInInspector] public PolygonImage m_img_star003_1_PolygonImage;



        private void UIFinder()
        {       
			m_UI_10063_1_Animator = gameObject.GetComponent<Animator>();

			m_img_star003_1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_star003_1");


			BindEvent();
        }

        #endregion
    }
}