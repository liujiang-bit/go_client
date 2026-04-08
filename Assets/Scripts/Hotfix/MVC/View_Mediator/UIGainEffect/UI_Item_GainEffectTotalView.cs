// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Item_GainEffectTotalView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GainEffectTotalView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GainEffectTotal";

        public UI_Item_GainEffectTotalView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_val_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");

			m_lbl_val_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_val");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}