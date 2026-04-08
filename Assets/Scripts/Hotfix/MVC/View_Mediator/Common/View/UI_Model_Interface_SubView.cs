// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Interface_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_Interface_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Interface";

        public UI_Model_Interface_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_Interface;
		[HideInInspector] public PolygonImage m_img_mask_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public BtnAnimation m_btn_back_BtnAnimation;



        private void UIFinder()
        {       
			m_UI_Model_Interface = gameObject.GetComponent<RectTransform>();
			m_img_mask_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_mask");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_back");
			m_btn_back_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_back");


			BindEvent();
        }

        #endregion
    }
}