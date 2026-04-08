// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType18List_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_MailType18List_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType18List";

        public UI_Item_MailType18List_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailType18List;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_bg_PolygonImage;
		[HideInInspector] public GameButton m_btn_bg_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_MailType18List = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_time");

			m_btn_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bg");
			m_btn_bg_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_bg");


			BindEvent();
        }

        #endregion
    }
}