// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    UI_Item_ArmySaveDataView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ArmySaveDataView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ArmySaveData";

        public UI_Item_ArmySaveDataView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_click_PolygonImage;
		[HideInInspector] public GameButton m_btn_click_GameButton;

		[HideInInspector] public LanguageText m_txt_hint_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_click_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_click");
			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"btn_click");

			m_txt_hint_LanguageText = FindUI<LanguageText>(vb.transform ,"txt_hint");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}