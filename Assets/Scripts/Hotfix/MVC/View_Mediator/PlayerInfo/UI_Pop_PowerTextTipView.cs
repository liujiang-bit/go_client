// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, March 10, 2020
// Update Time         :    Tuesday, March 10, 2020
// Class Description   :    UI_Pop_PowerTextTipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_PowerTextTipView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_PowerTextTip";

        public UI_Pop_PowerTextTipView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public LanguageText m_lbl_des_LanguageText;

		[HideInInspector] public LanguageText m_lbl_tip_LanguageText;

		[HideInInspector] public LanguageText m_lbl_des2_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_content");
			m_lbl_des_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/lbl_des");

			m_lbl_tip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/lbl_tip");

			m_lbl_des2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/lbl_des2");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}