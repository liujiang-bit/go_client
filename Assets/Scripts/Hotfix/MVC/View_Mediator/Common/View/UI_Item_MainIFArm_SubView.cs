// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MainIFArm_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_MainIFArm_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MainIFArm";

        public UI_Item_MainIFArm_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MainIFArm_ViewBinder;
		[HideInInspector] public GrayChildrens m_UI_Item_MainIFArm_GrayChildrens;

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



        private void UIFinder()
        {       
			m_UI_Item_MainIFArm_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Item_MainIFArm_GrayChildrens = gameObject.GetComponent<GrayChildrens>();

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");
			m_btn_btn_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_btn");
			m_btn_btn_UIPressBtn = FindUI<UIPressBtn>(gameObject.transform ,"btn_btn");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_btn/UI_Model_CaptainHead"));
			m_img_choose_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_choose");

			m_pl_state = FindUI<RectTransform>(gameObject.transform ,"btn_btn/pl_state");
			m_img_collect_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_state/img_collect");
			m_img_collect_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"btn_btn/pl_state/img_collect");

			m_img_stay_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_state/img_stay");

			m_img_fight_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_state/img_fight");

			m_UI_Common_TroopsState = new UI_Common_TroopsState_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_btn/pl_state/UI_Common_TroopsState"));
			m_img_line_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_line");


			BindEvent();
        }

        #endregion
    }
}