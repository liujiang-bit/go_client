// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MainIFEventBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_MainIFEventBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MainIFEventBtn";

        public UI_Item_MainIFEventBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_MainIFEventBtn_Animator;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_eventIcon_PolygonImage;
		[HideInInspector] public GameButton m_btn_eventIcon_GameButton;

		[HideInInspector] public PolygonImage m_img_eventFlash_PolygonImage;

		[HideInInspector] public RectTransform m_img_redpoint;
		[HideInInspector] public LanguageText m_lbl_redpoint_LanguageText;

		[HideInInspector] public PolygonImage m_img_timebg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_MainIFEventBtn_Animator = gameObject.GetComponent<Animator>();

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");
			m_img_frame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_frame");

			m_btn_eventIcon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_eventIcon");
			m_btn_eventIcon_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_eventIcon");

			m_img_eventFlash_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_eventFlash");

			m_img_redpoint = FindUI<RectTransform>(gameObject.transform ,"img_redpoint");
			m_lbl_redpoint_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_redpoint/lbl_redpoint");

			m_img_timebg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_timebg");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_timebg/lbl_time");


			BindEvent();
        }

        #endregion
    }
}