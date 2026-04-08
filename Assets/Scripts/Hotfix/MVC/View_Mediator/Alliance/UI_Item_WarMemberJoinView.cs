// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, April 27, 2020
// Update Time         :    Monday, April 27, 2020
// Class Description   :    UI_Item_WarMemberJoinView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WarMemberJoinView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WarMemberJoin";

        public UI_Item_WarMemberJoinView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_Join_PolygonImage;
		[HideInInspector] public GameButton m_btn_Join_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_Join_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_Join");
			m_btn_Join_GameButton = FindUI<GameButton>(vb.transform ,"btn_Join");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}