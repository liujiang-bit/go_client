// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, 05 November 2020
// Update Time         :    Thursday, 05 November 2020
// Class Description   :    UI_Item_MainIFArmView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_MainIFArmView : GameView
    {
		public const string VIEW_NAME = "UI_Item_MainIFArm";

        public UI_Item_MainIFArmView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;
		[HideInInspector] public BtnAnimation m_btn_btn_BtnAnimation;
		[HideInInspector] public UIPressBtn m_btn_btn_UIPressBtn;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_choose_PolygonImage;

		[HideInInspector] public RectTransform m_pl_state;
		[HideInInspector] public PolygonImage m_img_collect_PolygonImage;
		[HideInInspector] public UIDefaultValue m_img_collect_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_stay_PolygonImage;

		[HideInInspector] public PolygonImage m_img_fight_PolygonImage;

		[HideInInspector] public UI_Common_TroopsState_SubView m_UI_Common_TroopsState;
		[HideInInspector] public PolygonImage m_img_line_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");
			m_btn_btn_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_btn");
			m_btn_btn_UIPressBtn = FindUI<UIPressBtn>(vb.transform ,"btn_btn");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_btn/UI_Model_CaptainHead"));
			m_img_choose_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_choose");

			m_pl_state = FindUI<RectTransform>(vb.transform ,"btn_btn/pl_state");
			m_img_collect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/pl_state/img_collect");
			m_img_collect_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"btn_btn/pl_state/img_collect");

			m_img_stay_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/pl_state/img_stay");

			m_img_fight_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/pl_state/img_fight");

			m_UI_Common_TroopsState = new UI_Common_TroopsState_SubView(FindUI<RectTransform>(vb.transform ,"btn_btn/pl_state/UI_Common_TroopsState"));
			m_img_line_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_line");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}