// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 30, 2020
// Update Time         :    Thursday, April 30, 2020
// Class Description   :    UI_Item_GuildApplicationView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildApplicationView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildApplication";

        public UI_Item_GuildApplicationView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kill_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kill_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_yes_PolygonImage;
		[HideInInspector] public GameButton m_btn_yes_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_yes_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_no_PolygonImage;
		[HideInInspector] public GameButton m_btn_no_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_no_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_name");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_power");

			m_lbl_kill_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_kill");
			m_lbl_kill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_kill");

			m_btn_yes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_yes");
			m_btn_yes_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_yes");
			m_btn_yes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/btn_yes");

			m_btn_no_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_no");
			m_btn_no_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_no");
			m_btn_no_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/btn_no");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}