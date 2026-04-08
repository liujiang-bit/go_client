// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    UI_Item_ReinforcementsHistroyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ReinforcementsHistroyView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ReinforcementsHistroy";

        public UI_Item_ReinforcementsHistroyView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_PlayerHead"));
			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_playerName");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_count");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_time");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}