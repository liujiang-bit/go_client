// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, May 13, 2020
// Update Time         :    Wednesday, May 13, 2020
// Class Description   :    UI_Win_GuildResearchView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildResearchView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildResearch";

        public UI_Win_GuildResearchView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_pl_win;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_develop_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_develop_PolygonImage;
		[HideInInspector] public ListView m_sv_develop_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_develop_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public ScrollRect m_sv_terrtroy_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_terrtroy_PolygonImage;
		[HideInInspector] public ListView m_sv_terrtroy_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_terrtroy_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_war_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_war_PolygonImage;
		[HideInInspector] public ListView m_sv_war_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_war_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_skill;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_UI_GuildSkill0;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_UI_GuildSkill1;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_UI_GuildSkill2;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_UI_GuildSkill3;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_UI_GuildSkill4;
		[HideInInspector] public UI_Item_GuildResearchSubItem_SubView m_UI_GuildSkill5;
		[HideInInspector] public RectTransform m_pl_corner;
		[HideInInspector] public RectTransform m_pl_donate;
		[HideInInspector] public LanguageText m_lbl_donate_LanguageText;

		[HideInInspector] public RectTransform m_pl_time;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public RectTransform m_pl_speedUp;
		[HideInInspector] public GameSlider m_pb_spBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_spBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_spTimeLeft_LanguageText;

		[HideInInspector] public PolygonImage m_btn_cancelRes_PolygonImage;
		[HideInInspector] public GameButton m_btn_cancelRes_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_cancelRes_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_spbg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_spbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_spIcon_PolygonImage;



        private void UIFinder()
        {
			m_pl_win = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"pl_win"));
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_sv_develop_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_develop");
			m_sv_develop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_develop");
			m_sv_develop_ListView = FindUI<ListView>(vb.transform ,"rect/sv_develop");
			m_sv_develop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_develop");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_develop/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_develop/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/sv_develop/v_list_view/c_list_view");
			m_sv_terrtroy_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_terrtroy");
			m_sv_terrtroy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_terrtroy");
			m_sv_terrtroy_ListView = FindUI<ListView>(vb.transform ,"rect/sv_terrtroy");
			m_sv_terrtroy_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_terrtroy");

			m_sv_war_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_war");
			m_sv_war_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_war");
			m_sv_war_ListView = FindUI<ListView>(vb.transform ,"rect/sv_war");
			m_sv_war_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_war");

			m_pl_skill = FindUI<RectTransform>(vb.transform ,"rect/pl_skill");
			m_UI_GuildSkill0 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_skill/skills/UI_GuildSkill0"));
			m_UI_GuildSkill1 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_skill/skills/UI_GuildSkill1"));
			m_UI_GuildSkill2 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_skill/skills/UI_GuildSkill2"));
			m_UI_GuildSkill3 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_skill/skills/UI_GuildSkill3"));
			m_UI_GuildSkill4 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_skill/skills/UI_GuildSkill4"));
			m_UI_GuildSkill5 = new UI_Item_GuildResearchSubItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_skill/skills/UI_GuildSkill5"));
			m_pl_corner = FindUI<RectTransform>(vb.transform ,"rect/pl_corner");
			m_pl_donate = FindUI<RectTransform>(vb.transform ,"rect/pl_corner/pl_donate");
			m_lbl_donate_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_corner/pl_donate/lbl_donate");

			m_pl_time = FindUI<RectTransform>(vb.transform ,"rect/pl_corner/pl_time");
			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_corner/pl_time/lbl_time");

			m_pl_speedUp = FindUI<RectTransform>(vb.transform ,"pl_speedUp");
			m_pb_spBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_speedUp/pb_spBar");
			m_pb_spBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_speedUp/pb_spBar");

			m_lbl_spTimeLeft_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_speedUp/pb_spBar/lbl_spTimeLeft");

			m_btn_cancelRes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_speedUp/btn_cancelRes");
			m_btn_cancelRes_GameButton = FindUI<GameButton>(vb.transform ,"pl_speedUp/btn_cancelRes");
			m_btn_cancelRes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_speedUp/btn_cancelRes");

			m_img_spbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_speedUp/img_spbg");
			m_img_spbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_speedUp/img_spbg");

			m_img_spIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_speedUp/img_spbg/img_spIcon");


            UI_Win_GuildResearchMediator mt = new UI_Win_GuildResearchMediator(vb.gameObject);
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
