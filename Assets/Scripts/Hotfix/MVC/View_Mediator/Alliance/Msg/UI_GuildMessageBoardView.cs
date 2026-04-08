// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, July 6, 2020
// Update Time         :    Monday, July 6, 2020
// Class Description   :    UI_GuildMessageBoardView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_GuildMessageBoardView : GameView
    {
        public const string VIEW_NAME = "UI_GuildMessageBoard";

        public UI_GuildMessageBoardView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public RectTransform m_pl_flag;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;
		[HideInInspector] public LanguageText m_lbl_guildName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_guildName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_featureName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_featureName_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_switch_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_switch_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_close_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_close_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_open_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_open_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public GrayChildrens m_pl_publish_MakeChildrenGray;

		[HideInInspector] public PolygonImage m_btn_sendMsg_PolygonImage;
		[HideInInspector] public GameButton m_btn_sendMsg_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_sendMsg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_message_PolygonImage;
		[HideInInspector] public GameInput m_ipt_message_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_message_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_imgUpload_PolygonImage;
		[HideInInspector] public GameButton m_btn_imgUpload_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_imgUpload_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_flag = FindUI<RectTransform>(vb.transform ,"rect/pl_flag");
			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_flag/UI_GuildFlag"));
			m_lbl_guildName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_flag/lbl_guildName");
			m_lbl_guildName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_flag/lbl_guildName");

			m_lbl_featureName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_flag/lbl_featureName");
			m_lbl_featureName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_flag/lbl_featureName");

			m_ck_switch_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_flag/ck_switch");
			m_ck_switch_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_flag/ck_switch");

			m_img_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_flag/ck_switch/img_close");
			m_img_close_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_flag/ck_switch/img_close");

			m_img_open_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_flag/ck_switch/img_open");
			m_img_open_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_flag/ck_switch/img_open");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_pl_publish_MakeChildrenGray = FindUI<GrayChildrens>(vb.transform ,"rect/pl_publish");

			m_btn_sendMsg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_publish/btn_sendMsg");
			m_btn_sendMsg_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_publish/btn_sendMsg");
			m_btn_sendMsg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_publish/btn_sendMsg");

			m_ipt_message_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_publish/ipt_message");
			m_ipt_message_GameInput = FindUI<GameInput>(vb.transform ,"rect/pl_publish/ipt_message");
			m_ipt_message_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_publish/ipt_message");

			m_btn_imgUpload_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_publish/btn_imgUpload");
			m_btn_imgUpload_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_publish/btn_imgUpload");
			m_btn_imgUpload_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_publish/btn_imgUpload");


            UI_GuildMessageBoardMediator mt = new UI_GuildMessageBoardMediator(vb.gameObject);
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
