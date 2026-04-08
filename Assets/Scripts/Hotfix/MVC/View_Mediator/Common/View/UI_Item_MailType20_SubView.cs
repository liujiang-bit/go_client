// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType20_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType20_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType20";

        public UI_Item_MailType20_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailType20_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_go;


        private void UIFinder()
        {       
			m_UI_Item_MailType20_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_mes");

			m_btn_go = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_go"));

			BindEvent();
        }

        #endregion
    }
}