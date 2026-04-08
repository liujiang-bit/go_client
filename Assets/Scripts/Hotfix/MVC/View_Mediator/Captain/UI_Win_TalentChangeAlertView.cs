// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_Win_TalentChangeAlertView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_TalentChangeAlertView : GameView
    {
        public const string VIEW_NAME = "UI_Win_TalentChangeAlert";

        public UI_Win_TalentChangeAlertView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public RectTransform m_btn_sure;
		[HideInInspector] public PolygonImage m_btn_languageButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_languageButton_GameButton;
		[HideInInspector] public VerticalLayoutGroup m_btn_languageButton_VerticalLayoutGroup;
		[HideInInspector] public BtnAnimation m_btn_languageButton_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;
		[HideInInspector] public LayoutElement m_img_img_LayoutElement;

		[HideInInspector] public PolygonImage m_img_forbid_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_Text_LanguageText;
		[HideInInspector] public Shadow m_lbl_Text_Shadow;

		[HideInInspector] public HorizontalLayoutGroup m_pl_line2_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon2_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_line2_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_line2_ContentSizeFitter;
		[HideInInspector] public Shadow m_lbl_line2_Shadow;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;

		[HideInInspector] public UI_Model_DoubleLineButton_Red2_SubView m_btn_cancel;
		[HideInInspector] public LanguageText m_lbl_have_LanguageText;



        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_text");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"img_bg/lbl_title");

			m_btn_sure = FindUI<RectTransform>(vb.transform ,"img_bg/btn_sure");
			m_btn_languageButton_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_sure/btn_languageButton");
			m_btn_languageButton_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/btn_sure/btn_languageButton");
			m_btn_languageButton_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"img_bg/btn_sure/btn_languageButton");
			m_btn_languageButton_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"img_bg/btn_sure/btn_languageButton");

			m_img_img_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_sure/btn_languageButton/img_img");
			m_img_img_LayoutElement = FindUI<LayoutElement>(vb.transform ,"img_bg/btn_sure/btn_languageButton/img_img");

			m_img_forbid_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_sure/btn_languageButton/img_img/img_forbid");

			m_lbl_Text_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/btn_sure/btn_languageButton/lbl_Text");
			m_lbl_Text_Shadow = FindUI<Shadow>(vb.transform ,"img_bg/btn_sure/btn_languageButton/lbl_Text");

			m_pl_line2_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"img_bg/btn_sure/btn_languageButton/pl_line2");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_sure/btn_languageButton/pl_line2/img_frame");

			m_img_icon2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_sure/btn_languageButton/pl_line2/img_frame/img_icon2");

			m_lbl_line2_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/btn_sure/btn_languageButton/pl_line2/lbl_line2");
			m_lbl_line2_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"img_bg/btn_sure/btn_languageButton/pl_line2/lbl_line2");
			m_lbl_line2_Shadow = FindUI<Shadow>(vb.transform ,"img_bg/btn_sure/btn_languageButton/pl_line2/lbl_line2");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_sure/btn_languageButton/img_redpoint");

			m_btn_cancel = new UI_Model_DoubleLineButton_Red2_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/btn_cancel"));
			m_lbl_have_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_have");


            UI_Win_TalentChangeAlertMediator mt = new UI_Win_TalentChangeAlertMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}
