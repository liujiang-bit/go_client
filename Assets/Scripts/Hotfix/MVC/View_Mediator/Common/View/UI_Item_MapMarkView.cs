// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月8日
// Update Time         :    2020年4月8日
// Class Description   :    UI_Item_MapMarkView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_MapMarkView : GameView
    {
		public const string VIEW_NAME = "UI_Item_MapMark";

        public UI_Item_MapMarkView () 
        {
        }

        #region gen ui code 


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}