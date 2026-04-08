// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月1日
// Update Time         :    2020年6月1日
// Class Description   :    UI_Win_GuildSettingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildSettingView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildSetting";

        public UI_Win_GuildSettingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public PolygonImage m_btn_edit_PolygonImage;
		[HideInInspector] public GameButton m_btn_edit_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_edit_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_sample_PolygonImage;
		[HideInInspector] public GameButton m_btn_sample_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_sample_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_sample_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_sample_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_rename_PolygonImage;
		[HideInInspector] public GameButton m_btn_rename_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_rename_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_desc_PolygonImage;
		[HideInInspector] public GameInput m_ipt_desc_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_desc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_join_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_join_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_left_PolygonImage;
		[HideInInspector] public GameButton m_btn_left_GameButton;

		[HideInInspector] public PolygonImage m_btn_right_PolygonImage;
		[HideInInspector] public GameButton m_btn_right_GameButton;

		[HideInInspector] public LanguageText m_lbl_mid_LanguageText;

		[HideInInspector] public PolygonImage m_img_language_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_language_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_language_PolygonImage;
		[HideInInspector] public GameButton m_btn_language_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_language_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_curLanguage_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_btn_change;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_long_SubView m_btn_create;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_BlueSelect;


        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/frame/lbl_title");

			m_btn_edit_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_edit");
			m_btn_edit_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_edit");
			m_btn_edit_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_edit");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_edit/img_redpoint");

			m_btn_sample_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/btn_sample");
			m_btn_sample_GameButton = FindUI<GameButton>(vb.transform ,"rect/left/btn_sample");
			m_btn_sample_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/btn_sample");

			m_lbl_sample_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/btn_sample/lbl_sample");
			m_lbl_sample_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/btn_sample/lbl_sample");

			m_btn_rename_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/btn_rename");
			m_btn_rename_GameButton = FindUI<GameButton>(vb.transform ,"rect/left/btn_rename");
			m_btn_rename_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/btn_rename");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/btn_rename/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/btn_rename/lbl_name");

			m_ipt_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/ipt_desc");
			m_ipt_desc_GameInput = FindUI<GameInput>(vb.transform ,"rect/left/ipt_desc");
			m_ipt_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/ipt_desc");

			m_img_join_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/img_join");
			m_img_join_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/img_join");

			m_btn_left_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/img_join/btn_left");
			m_btn_left_GameButton = FindUI<GameButton>(vb.transform ,"rect/left/img_join/btn_left");

			m_btn_right_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/img_join/btn_right");
			m_btn_right_GameButton = FindUI<GameButton>(vb.transform ,"rect/left/img_join/btn_right");

			m_lbl_mid_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/img_join/lbl_mid");

			m_img_language_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/img_language");
			m_img_language_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/img_language");

			m_btn_language_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/img_language/btn_language");
			m_btn_language_GameButton = FindUI<GameButton>(vb.transform ,"rect/left/img_language/btn_language");
			m_btn_language_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/img_language/btn_language");

			m_lbl_curLanguage_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/img_language/lbl_curLanguage");

			m_btn_change = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/btn_change"));
			m_btn_create = new UI_Model_DoubleLineButton_Yellow_long_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/btn_create"));
			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"rect/right/bg/UI_GuildFlag"));
			m_UI_BlueSelect = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/right/bg/UI_BlueSelect"));

            UI_Win_GuildSettingMediator mt = new UI_Win_GuildSettingMediator(vb.gameObject);
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
