// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Item_GetRewardView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GetRewardView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GetReward";

        public UI_Item_GetRewardView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_count");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}