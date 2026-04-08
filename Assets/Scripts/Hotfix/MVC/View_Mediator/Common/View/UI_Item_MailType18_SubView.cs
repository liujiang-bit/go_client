// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType18_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_MailType18_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType18";

        public UI_Item_MailType18_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailType18_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public VerticalLayoutGroup m_pl_mes_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_mes_ContentSizeFitter;

		[HideInInspector] public VerticalLayoutGroup m_pl_view_VerticalLayoutGroup;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public UI_Pop_MailType18_SubView m_UI_Pop_MailType18;


        private void UIFinder()
        {       
			m_UI_Item_MailType18_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_pl_mes_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_mes");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_mes");
			m_lbl_mes_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/lbl_mes");

			m_pl_view_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_mes/pl_view");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_mes/pl_view/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_view/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(gameObject.transform ,"pl_mes/pl_view/sv_list_view");

			m_UI_Pop_MailType18 = new UI_Pop_MailType18_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Pop_MailType18"));

			BindEvent();
        }

        #endregion
    }
}