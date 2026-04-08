// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, January 3, 2020
// Update Time         :    Friday, January 3, 2020
// Class Description   :    ResearchPowerItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ResearchPowerItemView : GameView
    {
		public const string VIEW_NAME = "UI_ResearchPowerItem";

        public ResearchPowerItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_lv_LanguageText;

		[HideInInspector] public LanguageText m_lbl_ack_LanguageText;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_lv_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_lv");

			m_lbl_ack_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_ack");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_power");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}