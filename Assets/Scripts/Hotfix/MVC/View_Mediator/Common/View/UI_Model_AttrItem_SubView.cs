// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_AttrItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_AttrItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_AttrItem";

        public UI_Model_AttrItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_AttrItem_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_crrVaule_LanguageText;

		[HideInInspector] public LanguageText m_lbl_addVaule_LanguageText;

		[HideInInspector] public PolygonImage m_pl_line_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_AttrItem_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_lbl_crrVaule_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/lbl_crrVaule");

			m_lbl_addVaule_LanguageText = FindUI<LanguageText>(gameObject.transform ,"layout/lbl_addVaule");

			m_pl_line_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_line");


			BindEvent();
        }

        #endregion
    }
}