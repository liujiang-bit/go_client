// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildCityBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_BuildCityBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildCityBtn";

        public UI_Item_BuildCityBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_BuildCityBtn_ViewBinder;

		[HideInInspector] public PolygonImage m_img_iconOff_PolygonImage;

		[HideInInspector] public PolygonImage m_img_iconOn_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;


        private void UIFinder()
        {       
			m_UI_Item_BuildCityBtn_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_iconOff_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_iconOff");

			m_img_iconOn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_iconOn");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Common_Redpoint"));

			BindEvent();
        }

        #endregion
    }
}