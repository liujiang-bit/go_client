// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_PlayerDataBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_PlayerDataBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_PlayerDataBtn";

        public UI_Item_PlayerDataBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_PlayerDataBtn_ViewBinder;
		[HideInInspector] public Animator m_UI_Item_PlayerDataBtn_Animator;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_wait_PolygonImage;
		[HideInInspector] public Animation m_img_wait_Animation;

		[HideInInspector] public LanguageText m_lbl_Text_LanguageText;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;


        private void UIFinder()
        {       
			m_UI_Item_PlayerDataBtn_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Item_PlayerDataBtn_Animator = gameObject.GetComponent<Animator>();

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_wait_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_wait");
			m_img_wait_Animation = FindUI<Animation>(gameObject.transform ,"img_wait");

			m_lbl_Text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_Text");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Common_Redpoint"));

			BindEvent();
        }

        #endregion
    }
}