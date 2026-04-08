// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月8日
// Update Time         :    2020年1月8日
// Class Description   :    UI_Model_AttrItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Model_AttrItemView : GameView
    {
		public const string VIEW_NAME = "UI_Model_AttrItem";

        public UI_Model_AttrItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_crrVaule_LanguageText;

		[HideInInspector] public LanguageText m_lbl_addVaule_LanguageText;

		[HideInInspector] public PolygonImage m_pl_line_PolygonImage;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");

			m_lbl_crrVaule_LanguageText = FindUI<LanguageText>(vb.transform ,"layout/lbl_crrVaule");

			m_lbl_addVaule_LanguageText = FindUI<LanguageText>(vb.transform ,"layout/lbl_addVaule");


			m_pl_line_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_line");
        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}