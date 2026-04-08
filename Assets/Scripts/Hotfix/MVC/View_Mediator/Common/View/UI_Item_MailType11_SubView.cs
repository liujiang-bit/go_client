// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType11_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType11_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType11";

        public UI_Item_MailType11_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public VerticalLayoutGroup m_UI_Item_MailType11_VerticalLayoutGroup;
		[HideInInspector] public ViewBinder m_UI_Item_MailType11_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;



        private void UIFinder()
        {       
			m_UI_Item_MailType11_VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
			m_UI_Item_MailType11_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(gameObject.transform ,"sv_list_view");


			BindEvent();
        }

        #endregion
    }
}