// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_ArmyCountView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ArmyCountView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ArmyCount";

        public UI_Item_ArmyCountView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_ArmyName_LanguageText;

		[HideInInspector] public PolygonImage m_ipt_ArmyInput_PolygonImage;
		[HideInInspector] public GameInput m_ipt_ArmyInput_GameInput;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public GameSlider m_sd_count_GameSlider;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_ArmyName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_ArmyName");

			m_ipt_ArmyInput_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_ArmyInput");
			m_ipt_ArmyInput_GameInput = FindUI<GameInput>(vb.transform ,"ipt_ArmyInput");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");

			m_sd_count_GameSlider = FindUI<GameSlider>(vb.transform ,"sd_count");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}