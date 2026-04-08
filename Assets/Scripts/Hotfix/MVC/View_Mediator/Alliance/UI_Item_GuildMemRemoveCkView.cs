// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Item_GuildMemRemoveCkView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildMemRemoveCkView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildMemRemoveCk";

        public UI_Item_GuildMemRemoveCkView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GameToggle m_ck_ck_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_ck_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_ck_ck_GameToggle = FindUI<GameToggle>(vb.transform ,"ck_ck");
			m_ck_ck_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"ck_ck");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}