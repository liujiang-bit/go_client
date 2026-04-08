// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月2日
// Update Time         :    2020年7月2日
// Class Description   :    UI_Win_WriteAMailView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_WriteAMailView : GameView
    {
        public const string VIEW_NAME = "UI_Win_WriteAMail";

        public UI_Win_WriteAMailView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type4_SubView m_UI_Model_Window_Type4;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public PolygonImage m_ipt_mailTitle_PolygonImage;
		[HideInInspector] public GameInput m_ipt_mailTitle_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_mailTitle_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_mailtarget_PolygonImage;
		[HideInInspector] public GameInput m_ipt_mailtarget_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_mailtarget_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_targetList_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_targetList_PolygonImage;
		[HideInInspector] public ListView m_sv_targetList_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_targetList_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_checkError_PolygonImage;
		[HideInInspector] public GameButton m_btn_checkError_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_checkError_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_checkGood_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_pl_checkGood_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_ipt_mailContent_PolygonImage;
		[HideInInspector] public GameInput m_ipt_mailContent_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_mailContent_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_send_PolygonImage;
		[HideInInspector] public GameButton m_btn_send_GameButton;
		[HideInInspector] public GrayChildrens m_btn_send_MakeChildrenGray;

		[HideInInspector] public LanguageText m_lbl_wordsCount_LanguageText;



        private void UIFinder()
        {
			m_UI_Model_Window_Type4 = new UI_Model_Window_Type4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type4"));
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_close");

			m_ipt_mailTitle_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/ipt_mailTitle");
			m_ipt_mailTitle_GameInput = FindUI<GameInput>(vb.transform ,"rect/ipt_mailTitle");
			m_ipt_mailTitle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/ipt_mailTitle");

			m_ipt_mailtarget_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/target/ipt_mailtarget");
			m_ipt_mailtarget_GameInput = FindUI<GameInput>(vb.transform ,"rect/target/ipt_mailtarget");
			m_ipt_mailtarget_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/target/ipt_mailtarget");

			m_sv_targetList_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/target/sv_targetList");
			m_sv_targetList_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/target/sv_targetList");
			m_sv_targetList_ListView = FindUI<ListView>(vb.transform ,"rect/target/sv_targetList");
			m_sv_targetList_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/target/sv_targetList");

			m_btn_checkError_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/target/btn_checkError");
			m_btn_checkError_GameButton = FindUI<GameButton>(vb.transform ,"rect/target/btn_checkError");
			m_btn_checkError_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/target/btn_checkError");

			m_pl_checkGood_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/target/pl_checkGood");
			m_pl_checkGood_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/target/pl_checkGood");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/target/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(vb.transform ,"rect/target/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/target/btn_add");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view");
			m_ipt_mailContent_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_mailContent");
			m_ipt_mailContent_GameInput = FindUI<GameInput>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_mailContent");
			m_ipt_mailContent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_mailContent");

			m_btn_send_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_send");
			m_btn_send_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_send");
			m_btn_send_MakeChildrenGray = FindUI<GrayChildrens>(vb.transform ,"rect/btn_send");

			m_lbl_wordsCount_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_wordsCount");


            UI_Win_WriteAMailMediator mt = new UI_Win_WriteAMailMediator(vb.gameObject);
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
