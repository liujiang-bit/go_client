// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_BookIcon_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Pop_BookIcon_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_BookIcon";

        public UI_Pop_BookIcon_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Pop_BookIcon_ViewBinder;
		[HideInInspector] public MapElementUI m_UI_Pop_BookIcon_MapElementUI;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ContentSizeFitter m_img_icon_ContentSizeFitter;
		[HideInInspector] public HorizontalLayoutGroup m_img_icon_HorizontalLayoutGroup;



        private void UIFinder()
        {       
			m_UI_Pop_BookIcon_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Pop_BookIcon_MapElementUI = gameObject.GetComponent<MapElementUI>();

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"img_icon");
			m_img_icon_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"img_icon");


			BindEvent();
        }

        #endregion
    }
}