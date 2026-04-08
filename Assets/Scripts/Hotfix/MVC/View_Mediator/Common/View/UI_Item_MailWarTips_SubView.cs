// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailWarTips_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailWarTips_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailWarTips";

        public UI_Item_MailWarTips_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailWarTips;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_playername_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playername_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_troopsnum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_troopsnum_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailWarTips = gameObject.GetComponent<RectTransform>();
			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_lbl_playername_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_playername");
			m_lbl_playername_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_playername");

			m_lbl_troopsnum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_troopsnum");
			m_lbl_troopsnum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_troopsnum");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_title");


			BindEvent();
        }

        #endregion
    }
}