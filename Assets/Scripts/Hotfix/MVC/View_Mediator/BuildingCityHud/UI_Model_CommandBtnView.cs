// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    UI_Model_CommandBtnView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Model_CommandBtnView : GameView
    {
		public const string VIEW_NAME = "UI_Model_CommandBtn";

        public UI_Model_CommandBtnView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_noTextButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_noTextButton_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_noTextButton_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton");
			m_btn_noTextButton_GameButton = FindUI<GameButton>(vb.transform ,"btn_noTextButton");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}