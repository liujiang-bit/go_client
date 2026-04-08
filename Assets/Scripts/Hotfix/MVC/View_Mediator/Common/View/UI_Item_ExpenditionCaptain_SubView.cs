// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ExpenditionCaptain_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ExpenditionCaptain_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ExpenditionCaptain";

        public UI_Item_ExpenditionCaptain_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public VerticalLayoutGroup m_UI_Item_ExpenditionCaptain_VerticalLayoutGroup;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_guard_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_ExpenditionCaptain_VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_btn/UI_Model_CaptainHead"));
			m_img_guard_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_guard");


			BindEvent();
        }

        #endregion
    }
}