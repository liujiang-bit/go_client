// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月2日
// Update Time         :    2020年1月2日
// Class Description   :    UI_Item_CaptainSkillView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_CaptainSkillView : GameView
    {
		public const string VIEW_NAME = "UI_Item_CaptainSkill";

        public UI_Item_CaptainSkillView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_Button_PolygonImage;
		[HideInInspector] public GameButton m_btn_Button_GameButton;

		[HideInInspector] public PolygonImage m_img_icon_gray_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;
		[HideInInspector] public Outline m_lbl_level_Outline;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");

			m_btn_Button_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_Button");
			m_btn_Button_GameButton = FindUI<GameButton>(vb.transform ,"btn_Button");

			m_img_icon_gray_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon_gray");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_level");
			m_lbl_level_Outline = FindUI<Outline>(vb.transform ,"lbl_level");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}