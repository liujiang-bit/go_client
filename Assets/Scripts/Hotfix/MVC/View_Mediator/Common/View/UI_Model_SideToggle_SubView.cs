// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_SideToggle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_SideToggle_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_SideToggle";

        public UI_Model_SideToggle_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_SideToggle;
		[HideInInspector] public GameToggle m_ck_ck_GameToggle;

		[HideInInspector] public PolygonImage m_img_dark_PolygonImage;

		[HideInInspector] public PolygonImage m_img_iconD_PolygonImage;

		[HideInInspector] public PolygonImage m_img_highLight_PolygonImage;

		[HideInInspector] public PolygonImage m_img_iconH_PolygonImage;

		[HideInInspector] public PolygonImage m_img_redpot_PolygonImage;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;


        private void UIFinder()
        {       
			m_UI_Model_SideToggle = gameObject.GetComponent<RectTransform>();
			m_ck_ck_GameToggle = FindUI<GameToggle>(gameObject.transform ,"ck_ck");

			m_img_dark_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ck_ck/img_dark");

			m_img_iconD_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ck_ck/img_dark/img_iconD");

			m_img_highLight_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ck_ck/img_dark/img_highLight");

			m_img_iconH_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ck_ck/img_dark/img_highLight/img_iconH");

			m_img_redpot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_redpot");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Common_Redpoint"));

			BindEvent();
        }

        #endregion
    }
}