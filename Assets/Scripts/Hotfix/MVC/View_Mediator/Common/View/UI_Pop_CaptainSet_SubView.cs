// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_CaptainSet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Pop_CaptainSet_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_CaptainSet";

        public UI_Pop_CaptainSet_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Pop_CaptainSet;
		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_btn_ck_PolygonImage;
		[HideInInspector] public GameButton m_btn_ck_GameButton;



        private void UIFinder()
        {       
			m_UI_Pop_CaptainSet = gameObject.GetComponent<RectTransform>();
			m_img_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_arrow");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));
			m_btn_ck_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_ck");
			m_btn_ck_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_ck");


			BindEvent();
        }

        #endregion
    }
}