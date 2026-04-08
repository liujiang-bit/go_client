// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MainIFBuff_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_MainIFBuff_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MainIFBuff";

        public UI_Item_MainIFBuff_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MainIFBuff;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_noTextButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_noTextButton_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_MainIFBuff = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");

			m_btn_noTextButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_noTextButton");
			m_btn_noTextButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_noTextButton");


			BindEvent();
        }

        #endregion
    }
}