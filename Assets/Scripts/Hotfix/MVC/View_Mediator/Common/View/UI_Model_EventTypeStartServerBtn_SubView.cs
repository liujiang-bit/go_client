// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_EventTypeStartServerBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_EventTypeStartServerBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_EventTypeStartServerBtn";

        public UI_Model_EventTypeStartServerBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Model_EventTypeStartServerBtn_PolygonImage;
		[HideInInspector] public GameButton m_UI_Model_EventTypeStartServerBtn_GameButton;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_select_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_img_lock_PolygonImage;

		[HideInInspector] public PolygonImage m_img_red_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_EventTypeStartServerBtn_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Model_EventTypeStartServerBtn_GameButton = gameObject.GetComponent<GameButton>();

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");
			m_img_select_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_select");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_img_lock_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_lock");

			m_img_red_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_red");


			BindEvent();
        }

        #endregion
    }
}