// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType19_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType19_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType19";

        public UI_Item_MailType19_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailType19_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public VerticalLayoutGroup m_pl_mes_VerticalLayoutGroup;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_PolygonImage;
		[HideInInspector] public Mask m_v_list_Mask;

		[HideInInspector] public RectTransform m_c_list;


        private void UIFinder()
        {       
			m_UI_Item_MailType19_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_pl_mes_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_mes");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_mes/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"pl_mes/sv_list");

			m_v_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/sv_list/v_list");
			m_v_list_Mask = FindUI<Mask>(gameObject.transform ,"pl_mes/sv_list/v_list");

			m_c_list = FindUI<RectTransform>(gameObject.transform ,"pl_mes/sv_list/v_list/c_list");

			BindEvent();
        }

        #endregion
    }
}