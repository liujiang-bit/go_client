// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType16MessageList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType16MessageList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType16MessageList";

        public UI_Item_MailType16MessageList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailType16MessageList;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_info_Empty4Raycast;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailType16MessageList = gameObject.GetComponent<RectTransform>();
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_title");

			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_info");
			m_btn_info_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_info");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_num");


			BindEvent();
        }

        #endregion
    }
}