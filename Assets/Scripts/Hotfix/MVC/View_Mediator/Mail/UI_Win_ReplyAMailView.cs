// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月2日
// Update Time         :    2020年7月2日
// Class Description   :    UI_Win_ReplyAMailView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ReplyAMailView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ReplyAMail";

        public UI_Win_ReplyAMailView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type4_SubView m_UI_Model_Window_Type4;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_ipt_mailContent_PolygonImage;
		[HideInInspector] public GameInput m_ipt_mailContent_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_mailContent_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_wordsCount_LanguageText;

		[HideInInspector] public LanguageText m_lbl_addressee_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_addressee_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_addressee_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playername_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_playername_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_playername_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_reply_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_reply_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_reply_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mailtitle_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_mailtitle_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_mailtitle_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_send_PolygonImage;
		[HideInInspector] public GameButton m_btn_send_GameButton;
		[HideInInspector] public GrayChildrens m_btn_send_MakeChildrenGray;

		[HideInInspector] public ScrollRect m_sv_mes_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_mes_PolygonImage;
		[HideInInspector] public ListView m_sv_mes_ListView;

		[HideInInspector] public PolygonImage m_v_mes_PolygonImage;
		[HideInInspector] public Mask m_v_mes_Mask;

		[HideInInspector] public ContentSizeFitter m_c_mes_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_c_mes_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_mes_ContentSizeFitter;



        private void UIFinder()
        {
			m_UI_Model_Window_Type4 = new UI_Model_Window_Type4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type4"));
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_close");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_PlayerHead"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view");
			m_ipt_mailContent_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_mailContent");
			m_ipt_mailContent_GameInput = FindUI<GameInput>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_mailContent");
			m_ipt_mailContent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_mailContent");

			m_lbl_wordsCount_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_wordsCount");

			m_lbl_addressee_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_addressee");
			m_lbl_addressee_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/lbl_addressee");
			m_lbl_addressee_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_addressee");

			m_lbl_playername_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_addressee/lbl_playername");
			m_lbl_playername_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/lbl_addressee/lbl_playername");
			m_lbl_playername_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_addressee/lbl_playername");

			m_lbl_reply_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_reply");
			m_lbl_reply_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/lbl_reply");
			m_lbl_reply_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_reply");

			m_lbl_mailtitle_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_reply/lbl_mailtitle");
			m_lbl_mailtitle_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/lbl_reply/lbl_mailtitle");
			m_lbl_mailtitle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_reply/lbl_mailtitle");

			m_btn_send_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_send");
			m_btn_send_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_send");
			m_btn_send_MakeChildrenGray = FindUI<GrayChildrens>(vb.transform ,"rect/btn_send");

			m_sv_mes_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_mes");
			m_sv_mes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_mes");
			m_sv_mes_ListView = FindUI<ListView>(vb.transform ,"rect/sv_mes");

			m_v_mes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_mes/v_mes");
			m_v_mes_Mask = FindUI<Mask>(vb.transform ,"rect/sv_mes/v_mes");

			m_c_mes_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/sv_mes/v_mes/c_mes");
			m_c_mes_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"rect/sv_mes/v_mes/c_mes");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/sv_mes/v_mes/c_mes/lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_mes/v_mes/c_mes/lbl_mes");
			m_lbl_mes_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/sv_mes/v_mes/c_mes/lbl_mes");


            UI_Win_ReplyAMailMediator mt = new UI_Win_ReplyAMailMediator(vb.gameObject);
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
