// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月11日
// Update Time         :    2020年8月11日
// Class Description   :    UI_Win_VipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_VipView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Vip";

        public UI_Win_VipView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public PolygonImage m_btn_arrowNext_PolygonImage;
		[HideInInspector] public GameButton m_btn_arrowNext_GameButton;

		[HideInInspector] public PolygonImage m_btn_arrowBack_PolygonImage;
		[HideInInspector] public GameButton m_btn_arrowBack_GameButton;

		[HideInInspector] public GameSlider m_pb_vipBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_vipBar_ArabLayoutCompment;
		[HideInInspector] public SmoothProgressBar m_pb_vipBar_SmoothBar;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_barText_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_pointBox_PolygonImage;
		[HideInInspector] public GameButton m_btn_pointBox_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_pointBox_ArabLayoutCompment;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_pl_pointbox;
		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_redpoint_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_boxTime_LanguageText;

		[HideInInspector] public PolygonImage m_btn_question_PolygonImage;
		[HideInInspector] public GameButton m_btn_question_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_question_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_vipPoint_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_vipPoint_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_vipDay_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_vipDay_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_vip_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_vip_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_vipLevel_LanguageText;
		[HideInInspector] public Outline m_lbl_vipLevel_Outline;

		[HideInInspector] public ScrollRect m_sv_listVip_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_listVip_PolygonImage;
		[HideInInspector] public ListView m_sv_listVip_ListView;

		[HideInInspector] public UI_Tip_BoxReward_SubView m_UI_Tip_BoxReward;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_btn_arrowNext_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_arrowNext");
			m_btn_arrowNext_GameButton = FindUI<GameButton>(vb.transform ,"btn_arrowNext");

			m_btn_arrowBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_arrowBack");
			m_btn_arrowBack_GameButton = FindUI<GameButton>(vb.transform ,"btn_arrowBack");

			m_pb_vipBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/top/pb_vipBar");
			m_pb_vipBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/pb_vipBar");
			m_pb_vipBar_SmoothBar = FindUI<SmoothProgressBar>(vb.transform ,"rect/top/pb_vipBar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/pb_vipBar/lbl_barText");
			m_lbl_barText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/pb_vipBar/lbl_barText");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_add");

			m_btn_pointBox_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_pointBox");
			m_btn_pointBox_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_pointBox");
			m_btn_pointBox_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_pointBox");

			m_pl_pointbox = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/top/btn_pointBox/pl_pointbox"));
			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_pointBox/img_redpoint");
			m_img_redpoint_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_pointBox/img_redpoint");

			m_lbl_boxTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/btn_pointBox/lbl_boxTime");

			m_btn_question_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_question");
			m_btn_question_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_question");
			m_btn_question_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_question");

			m_lbl_vipPoint_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/lbl_vipPoint");
			m_lbl_vipPoint_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/lbl_vipPoint");

			m_lbl_vipDay_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/lbl_vipDay");
			m_lbl_vipDay_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/lbl_vipDay");

			m_img_vip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/img_vip");
			m_img_vip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/img_vip");

			m_lbl_vipLevel_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/img_vip/lbl_vipLevel");
			m_lbl_vipLevel_Outline = FindUI<Outline>(vb.transform ,"rect/top/img_vip/lbl_vipLevel");

			m_sv_listVip_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/list/sv_listVip");
			m_sv_listVip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/list/sv_listVip");
			m_sv_listVip_ListView = FindUI<ListView>(vb.transform ,"rect/list/sv_listVip");

			m_UI_Tip_BoxReward = new UI_Tip_BoxReward_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tip_BoxReward"));

            UI_Win_VipMediator mt = new UI_Win_VipMediator(vb.gameObject);
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
