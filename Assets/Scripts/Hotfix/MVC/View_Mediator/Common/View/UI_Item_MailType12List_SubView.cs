// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType12List_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType12List_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType12List";

        public UI_Item_MailType12List_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailType12List_ViewBinder;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_message_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_message_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_look_PolygonImage;
		[HideInInspector] public GameButton m_btn_look_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_look_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_reddot_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_reddot_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailType12List_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"count/UI_Model_PlayerHead"));
			m_lbl_message_LanguageText = FindUI<LanguageText>(gameObject.transform ,"count/lbl_message");
			m_lbl_message_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"count/lbl_message");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"count/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"count/lbl_title");

			m_btn_look_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"count/btn_look");
			m_btn_look_GameButton = FindUI<GameButton>(gameObject.transform ,"count/btn_look");
			m_btn_look_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"count/btn_look");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/lbl_time");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/lbl_name");

			m_img_reddot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_reddot");
			m_img_reddot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_reddot");


			BindEvent();
        }

        #endregion
    }
}