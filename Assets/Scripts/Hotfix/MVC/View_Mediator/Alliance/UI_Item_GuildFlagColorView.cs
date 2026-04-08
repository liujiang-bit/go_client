// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, May 25, 2020
// Update Time         :    Monday, May 25, 2020
// Class Description   :    UI_Item_GuildFlagColorView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildFlagColorView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildFlagColor";

        public UI_Item_GuildFlagColorView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_bg_PolygonImage;
		[HideInInspector] public GameButton m_btn_bg_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_bg");
			m_btn_bg_GameButton = FindUI<GameButton>(vb.transform ,"btn_bg");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}