// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TalentSpecialText_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_TalentSpecialText_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TalentSpecialText";

        public UI_Item_TalentSpecialText_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_TalentSpecialText;
		[HideInInspector] public LanguageText m_lbl_lv_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lv_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_message_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_message_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_message_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_TalentSpecialText = gameObject.GetComponent<RectTransform>();
			m_lbl_lv_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_lv");
			m_lbl_lv_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_lv");

			m_lbl_message_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_message");
			m_lbl_message_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_message");
			m_lbl_message_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_message");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_num");


			BindEvent();
        }

        #endregion
    }
}