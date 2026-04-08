// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月14日
// Update Time         :    2020年3月14日
// Class Description   :    UI_Item_WarMailBarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WarMailBarView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WarMailBar";

        public UI_Item_WarMailBarView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_bar_PolygonImage;
		[HideInInspector] public GameButton m_btn_bar_GameButton;

		[HideInInspector] public PolygonImage m_img_fight_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_bar_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_bar");
			m_btn_bar_GameButton = FindUI<GameButton>(vb.transform ,"btn_bar");

			m_img_fight_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_fight");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"img_fight/UI_Model_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"img_fight/lbl_name");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}