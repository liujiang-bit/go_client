// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月28日
// Update Time         :    2020年7月28日
// Class Description   :    PlayerPowerViewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class PlayerPowerViewView : GameView
    {
        public const string VIEW_NAME = "UI_PlayerPowerView";

        public PlayerPowerViewView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public VerticalLayoutGroup m_c_list_view_VerticalLayoutGroup;

		[HideInInspector] public VerticalLayoutGroup m_pl_bg_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_powerTitle_LanguageText;

		[HideInInspector] public UI_Model_PowerItem_SubView m_buildLine;
		[HideInInspector] public UI_Model_PowerItem_SubView m_technologyLine;
		[HideInInspector] public UI_Model_PowerItem_SubView m_armyLine;
		[HideInInspector] public UI_Model_PowerItem_SubView m_horeLine;
		[HideInInspector] public VerticalLayoutGroup m_pl_bg1_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_powerTotalTilte_LanguageText;

		[HideInInspector] public UI_Model_PowerItem_SubView m_historyHightPower;
		[HideInInspector] public UI_Model_PowerItem_SubView m_wincount;
		[HideInInspector] public UI_Model_PowerItem_SubView m_losscount;
		[HideInInspector] public UI_Model_PowerItem_SubView m_deadcount;
		[HideInInspector] public UI_Model_PowerItem_SubView m_spycount;
		[HideInInspector] public VerticalLayoutGroup m_pl_bg2_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_resTotalTitle_LanguageText;

		[HideInInspector] public UI_Model_PowerItem_SubView m_resLine;
		[HideInInspector] public UI_Model_PowerItem_SubView m_resHelpLine;
		[HideInInspector] public UI_Model_PowerItem_SubView m_guildHelp;
		[HideInInspector] public PolygonImage m_img_topHead_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_changename_PolygonImage;
		[HideInInspector] public GameButton m_btn_changename_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_changename_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kill_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kill_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_ackTip_PolygonImage;
		[HideInInspector] public GameButton m_btn_ackTip_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_ackTip_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/sv_list_view/v_list_view");

			m_c_list_view_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view");

			m_pl_bg_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg");

			m_lbl_powerTitle_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg/titlebg/lbl_powerTitle");

			m_buildLine = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg/buildLine"));
			m_technologyLine = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg/technologyLine"));
			m_armyLine = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg/armyLine"));
			m_horeLine = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg/horeLine"));
			m_pl_bg1_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg1");

			m_lbl_powerTotalTilte_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg1/titlebg/lbl_powerTotalTilte");

			m_historyHightPower = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg1/historyHightPower"));
			m_wincount = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg1/wincount"));
			m_losscount = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg1/losscount"));
			m_deadcount = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg1/deadcount"));
			m_spycount = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg1/spycount"));
			m_pl_bg2_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg2");

			m_lbl_resTotalTitle_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg2/titlebg/lbl_resTotalTitle");

			m_resLine = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg2/resLine"));
			m_resHelpLine = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg2/resHelpLine"));
			m_guildHelp = new UI_Model_PowerItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/sv_list_view/v_list_view/c_list_view/pl_bg2/guildHelp"));
			m_img_topHead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_topHead");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/img_topHead/UI_Model_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_topHead/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_topHead/lbl_name");

			m_btn_changename_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_topHead/btn_changename");
			m_btn_changename_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_topHead/btn_changename");
			m_btn_changename_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_topHead/btn_changename");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_topHead/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_topHead/lbl_power");

			m_lbl_kill_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_topHead/lbl_kill");
			m_lbl_kill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_topHead/lbl_kill");

			m_btn_ackTip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_topHead/btn_ackTip");
			m_btn_ackTip_GameButton = FindUI<GameButton>(vb.transform ,"rect/img_topHead/btn_ackTip");
			m_btn_ackTip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_topHead/btn_ackTip");


            PlayerPowerViewMediator mt = new PlayerPowerViewMediator(vb.gameObject);
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
