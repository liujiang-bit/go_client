// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailWarSelf_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailWarSelf_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailWarSelf";

        public UI_Item_MailWarSelf_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailWarSelf;
		[HideInInspector] public LanguageText m_lbl_armyTotal_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyTotal_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyLast_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyLast_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_selfname_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_selfname_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;


        private void UIFinder()
        {       
			m_UI_Item_MailWarSelf = gameObject.GetComponent<RectTransform>();
			m_lbl_armyTotal_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_armyTotal");
			m_lbl_armyTotal_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_armyTotal");

			m_lbl_armyLast_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_armyLast");
			m_lbl_armyLast_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_armyLast");

			m_lbl_selfname_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_selfname");
			m_lbl_selfname_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_selfname");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));

			BindEvent();
        }

        #endregion
    }
}