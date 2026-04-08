// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, May 12, 2020
// Update Time         :    Tuesday, May 12, 2020
// Class Description   :    UI_Pop_GuildFortressTopView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_GuildFortressTopView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_GuildFortressTop";

        public UI_Pop_GuildFortressTopView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Pop_TextTip_SubView m_UI_Pop_TextTip;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_Flag;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Pop_TextTip = new UI_Pop_TextTip_SubView(FindUI<RectTransform>(vb.transform ,"UI_Pop_TextTip"));
			m_UI_Flag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"UI_Flag"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}