// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, May 18, 2020
// Update Time         :    Monday, May 18, 2020
// Class Description   :    UI_Win_GuildMemberView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildMemberView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildMember";

        public UI_Win_GuildMemberView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public RectTransform m_pl_rect1;
		[HideInInspector] public RectTransform m_pl_blet;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public GameButton m_btn_PlayerHead_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_PlayerHead_ArabLayoutCompment;
		[HideInInspector] public PolygonImage m_btn_PlayerHead_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_member_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_member_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_member_info_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kill_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kill_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power1_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_rect1 = FindUI<RectTransform>(vb.transform ,"pl_rect1");
			m_pl_blet = FindUI<RectTransform>(vb.transform ,"pl_rect1/top/pl_blet");
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect1/top/UI_PlayerHead"));
			m_btn_PlayerHead_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect1/top/btn_PlayerHead");
			m_btn_PlayerHead_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/btn_PlayerHead");
			m_btn_PlayerHead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/top/btn_PlayerHead");

			m_btn_member_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/top/btn_member_info");
			m_btn_member_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect1/top/btn_member_info");
			m_btn_member_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/btn_member_info");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/top/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/top/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/lbl_name");

			m_lbl_kill_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/top/lbl_kill");
			m_lbl_kill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/lbl_kill");

			m_lbl_power1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/top/lbl_power1");
			m_lbl_power1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/lbl_power1");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_rect1/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_rect1/sv_list");


            UI_Win_GuildMemberMediator mt = new UI_Win_GuildMemberMediator(vb.gameObject);
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
