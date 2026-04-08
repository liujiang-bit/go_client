// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, April 10, 2020
// Update Time         :    Friday, April 10, 2020
// Class Description   :    UI_Item_GuildPurviewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildPurviewView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildPurview";

        public UI_Item_GuildPurviewView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_titleName_LanguageText;

		[HideInInspector] public PolygonImage m_img_r1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_r2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_r3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_r4_PolygonImage;

		[HideInInspector] public PolygonImage m_img_r5_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_titleName_LanguageText = FindUI<LanguageText>(vb.transform ,"frame/lbl_titleName");

			m_img_r1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"frame/img_r1");

			m_img_r2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"frame/img_r2");

			m_img_r3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"frame/img_r3");

			m_img_r4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"frame/img_r4");

			m_img_r5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"frame/img_r5");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}