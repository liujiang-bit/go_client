// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_CaptainHeadBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_CaptainHeadBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_CaptainHeadBtn";

        public UI_Model_CaptainHeadBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_CaptainHeadBtn;
		[HideInInspector] public PolygonImage m_btn_languageButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_languageButton_GameButton;
		[HideInInspector] public BtnAnimation m_btn_languageButton_ButtonAnimation;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;


        private void UIFinder()
        {       
			m_UI_Model_CaptainHeadBtn = gameObject.GetComponent<RectTransform>();
			m_btn_languageButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_languageButton");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_languageButton/UI_Model_CaptainHead"));

			BindEvent();
        }

        #endregion
    }
}