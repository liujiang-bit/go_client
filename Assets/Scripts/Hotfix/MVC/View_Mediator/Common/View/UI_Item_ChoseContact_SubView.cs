// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChoseContact_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ChoseContact_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChoseContact";

        public UI_Item_ChoseContact_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChoseContact;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_img_channel_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_channel_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_languageCheckBox_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_languageCheckBox_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ChoseContact = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_img_channel_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_channel");
			m_img_channel_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_channel");

			m_ck_languageCheckBox_GameToggle = FindUI<GameToggle>(gameObject.transform ,"ck_languageCheckBox");
			m_ck_languageCheckBox_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ck_languageCheckBox");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");


			BindEvent();
        }

        #endregion
    }
}