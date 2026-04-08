// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailContact_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailContact_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailContact";

        public UI_Item_MailContact_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailContact;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_select_PolygonImage;
		[HideInInspector] public GameButton m_btn_select_GameButton;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_MailContact = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_name");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/UI_Model_PlayerHead"));
			m_btn_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/btn_select");
			m_btn_select_GameButton = FindUI<GameButton>(gameObject.transform ,"rect/btn_select");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/btn_select/img_select");


			BindEvent();
        }

        #endregion
    }
}