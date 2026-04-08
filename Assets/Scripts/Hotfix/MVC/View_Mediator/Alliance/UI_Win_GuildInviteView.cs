// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, August 21, 2020
// Update Time         :    Friday, August 21, 2020
// Class Description   :    UI_Win_GuildInviteView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildInviteView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildInvite";

        public UI_Win_GuildInviteView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public PolygonImage m_ipt_ipt_PolygonImage;
		[HideInInspector] public GameInput m_ipt_ipt_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_ipt_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_txt_Placeholder_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_txt_Placeholder_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_search_PolygonImage;
		[HideInInspector] public GameButton m_btn_search_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_search_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_tip_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_master_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_master_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_civi_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_civi_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_build;
		[HideInInspector] public LanguageText m_lbl_cityLevel_LanguageText;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_Blue;


        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_ipt_ipt_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/ipt_ipt");
			m_ipt_ipt_GameInput = FindUI<GameInput>(vb.transform ,"rect/top/ipt_ipt");
			m_ipt_ipt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/ipt_ipt");

			m_txt_Placeholder_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/ipt_ipt/txt_Placeholder");
			m_txt_Placeholder_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/ipt_ipt/txt_Placeholder");

			m_btn_search_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_search");
			m_btn_search_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_search");
			m_btn_search_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_search");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/left/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/left/sv_list");

			m_lbl_tip_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_tip");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/title/UI_PlayerHead"));
			m_lbl_master_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/title/lbl_master");
			m_lbl_master_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/title/lbl_master");

			m_img_civi_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_right/bg/img_civi");
			m_img_civi_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/bg/img_civi");

			m_pl_build = FindUI<RectTransform>(vb.transform ,"rect/pl_right/bg/pl_build");
			m_lbl_cityLevel_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/bg/lbl_cityLevel");

			m_UI_Blue = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/UI_Blue"));

            UI_Win_GuildInviteMediator mt = new UI_Win_GuildInviteMediator(vb.gameObject);
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
