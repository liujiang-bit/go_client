// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_AttrList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_AttrList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_AttrList";

        public UI_Model_AttrList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Model_AttrList_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;


        private void UIFinder()
        {       
			m_UI_Model_AttrList_PolygonImage = gameObject.GetComponent<PolygonImage>();

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(gameObject.transform ,"sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(gameObject.transform ,"sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(gameObject.transform ,"sv_list_view/v_list_view/c_list_view");

			BindEvent();
        }

        #endregion
    }
}