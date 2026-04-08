// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月15日
// Update Time         :    2020年4月15日
// Class Description   :    UI_Item_TavernBoxDescView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_TavernBoxDescView : GameView
    {
		public const string VIEW_NAME = "UI_Item_TavernBoxDesc";

        public UI_Item_TavernBoxDescView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_lbl2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lbl2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lbl1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lbl1_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_lbl2_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_lbl2");
			m_lbl_lbl2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_lbl2");

			m_lbl_lbl1_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_lbl1");
			m_lbl_lbl1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_lbl1");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}