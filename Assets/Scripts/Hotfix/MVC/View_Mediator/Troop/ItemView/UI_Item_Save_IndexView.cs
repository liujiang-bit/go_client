// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月10日
// Update Time         :    2020年4月10日
// Class Description   :    UI_Item_Save_IndexView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_Save_IndexView : GameView
    {
		public const string VIEW_NAME = "UI_Item_Save_Index";

        public UI_Item_Save_IndexView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_Background_PolygonImage;

		[HideInInspector] public PolygonImage m_img_Checkmark_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_saveid_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_Background_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_Background");

			m_img_Checkmark_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_Background/img_Checkmark");

			m_lbl_saveid_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_saveid");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}