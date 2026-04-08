// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月7日
// Update Time         :    2020年7月7日
// Class Description   :    UI_Win_GuildWelcomeEditView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildWelcomeEditView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildWelcomeEdit";

        public UI_Win_GuildWelcomeEditView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type4_SubView m_UI_Model_Window_Type4;
		[HideInInspector] public RectTransform m_pl_winFrame;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public LanguageText m_lbl_wordsCount_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_ipt_inputField_PolygonImage;
		[HideInInspector] public GameInput m_ipt_inputField_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_inputField_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_sure;


        private void UIFinder()
        {
			m_UI_Model_Window_Type4 = new UI_Model_Window_Type4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type4"));
			m_pl_winFrame = FindUI<RectTransform>(vb.transform ,"pl_winFrame");
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_winFrame/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_winFrame/btn_close");

			m_lbl_wordsCount_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_wordsCount");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view");
			m_ipt_inputField_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_inputField");
			m_ipt_inputField_GameInput = FindUI<GameInput>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_inputField");
			m_ipt_inputField_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/ipt_inputField");

			m_btn_sure = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_sure"));

            UI_Win_GuildWelcomeEditMediator mt = new UI_Win_GuildWelcomeEditMediator(vb.gameObject);
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
