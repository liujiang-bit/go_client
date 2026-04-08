// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月4日
// Update Time         :    2020年8月4日
// Class Description   :    UI_Tip_GuardianLevelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_GuardianLevelView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_GuardianLevel";

        public UI_Tip_GuardianLevelView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}