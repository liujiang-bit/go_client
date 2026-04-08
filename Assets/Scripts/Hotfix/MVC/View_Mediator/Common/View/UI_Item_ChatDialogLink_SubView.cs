// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatDialogLink_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ChatDialogLink_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatDialogLink";

        public UI_Item_ChatDialogLink_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ChatDialogLink_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_ali_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_ali_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_ali_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_name_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public HrefText m_lbl_linkmes_HrefText;
		[HideInInspector] public ArabLayoutCompment m_lbl_linkmes_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ChatDialogLink_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_ali_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_ali");
			m_lbl_ali_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_ali");
			m_lbl_ali_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_ali");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_ali/lbl_name");
			m_lbl_name_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_ali/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_ali/lbl_name");

			m_lbl_linkmes_HrefText = FindUI<HrefText>(gameObject.transform ,"lbl_ali/lbl_name/lbl_linkmes");
			m_lbl_linkmes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_ali/lbl_name/lbl_linkmes");


			BindEvent();
        }

        #endregion
    }
}