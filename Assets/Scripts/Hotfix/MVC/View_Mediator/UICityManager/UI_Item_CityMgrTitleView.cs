// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月22日
// Update Time         :    2020年4月22日
// Class Description   :    UI_Item_CityMgrTitleView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_CityMgrTitleView : GameView
    {
		public const string VIEW_NAME = "UI_Item_CityMgrTitle";

        public UI_Item_CityMgrTitleView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_title");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}