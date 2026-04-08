// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptionArrType_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptionArrType_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptionArrType";

        public UI_Item_CaptionArrType_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptionArrType;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_CaptionArrType = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_img");


			BindEvent();
        }

        #endregion
    }
}