// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 30, 2020
// Update Time         :    Thursday, April 30, 2020
// Class Description   :    UI_Win_GuildViewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildViewView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildView";

        public UI_Win_GuildViewView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public RectTransform m_pl_rect0;
		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_trans_PolygonImage;
		[HideInInspector] public GameButton m_btn_trans_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_trans_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_btn_comment;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_btn_mail;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_btn_menber;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_flag;
		[HideInInspector] public LanguageText m_lbl_guildName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_guildName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_master_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_master_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_terrtroy_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_terrtroy_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_gift_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_gift_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_member_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_member_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_rect0 = FindUI<RectTransform>(vb.transform ,"pl_rect0");
			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/left/bg/desc/sv/v/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/left/bg/desc/sv/v/lbl_desc");

			m_btn_trans_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect0/left/bg/btn_trans");
			m_btn_trans_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect0/left/bg/btn_trans");
			m_btn_trans_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/left/bg/btn_trans");

			m_UI_btn_comment = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/bg/features/UI_btn_comment"));
			m_UI_btn_mail = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/bg/features/UI_btn_mail"));
			m_UI_btn_menber = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/bg/features/UI_btn_menber"));
			m_UI_flag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/right/flagbg/UI_flag"));
			m_lbl_guildName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_guildName");
			m_lbl_guildName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_guildName");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_power");

			m_lbl_master_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_master");
			m_lbl_master_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_master");

			m_lbl_terrtroy_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_terrtroy");
			m_lbl_terrtroy_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_terrtroy");

			m_lbl_gift_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_gift");
			m_lbl_gift_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_gift");

			m_lbl_member_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_member");
			m_lbl_member_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_member");


            UI_Win_GuildViewMediator mt = new UI_Win_GuildViewMediator(vb.gameObject);
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
