// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, 05 November 2020
// Update Time         :    Thursday, 05 November 2020
// Class Description   :    UI_Item_ArmyQueueHeadView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_ArmyQueueHeadView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ArmyQueueHead";

        public UI_Item_ArmyQueueHeadView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_noTextButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_noTextButton_GameButton;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_Info_PolygonImage;
		[HideInInspector] public GameButton m_btn_Info_GameButton;

		[HideInInspector] public PolygonImage m_img_state_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_state_ArabLayoutCompment;

		[HideInInspector] public UI_Common_TroopsState_SubView m_UI_Common_TroopsState;
		[HideInInspector] public PolygonImage m_pl_time_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public PolygonImage m_img_checkEffect_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_checkEffect_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_checkBg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_check_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_noTextButton_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton");
			m_btn_noTextButton_GameButton = FindUI<GameButton>(vb.transform ,"btn_noTextButton");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_noTextButton/UI_Model_CaptainHead"));
			m_img_select_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton/img_select");

			m_btn_Info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton/btn_Info");
			m_btn_Info_GameButton = FindUI<GameButton>(vb.transform ,"btn_noTextButton/btn_Info");

			m_img_state_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton/img_state");
			m_img_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_noTextButton/img_state");

			m_UI_Common_TroopsState = new UI_Common_TroopsState_SubView(FindUI<RectTransform>(vb.transform ,"btn_noTextButton/UI_Common_TroopsState"));
			m_pl_time_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton/pl_time");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_noTextButton/pl_time/lbl_time");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_noTextButton/bg/lbl_count");

			m_img_checkEffect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton/img_checkEffect");
			m_img_checkEffect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_noTextButton/img_checkEffect");

			m_img_checkBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton/img_checkEffect/img_checkBg");

			m_img_check_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_noTextButton/img_checkEffect/img_check");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}