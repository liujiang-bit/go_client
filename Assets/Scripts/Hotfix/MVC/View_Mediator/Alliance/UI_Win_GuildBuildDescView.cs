// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, July 13, 2020
// Update Time         :    Monday, July 13, 2020
// Class Description   :    UI_Win_GuildBuildDescView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildBuildDescView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildBuildDesc";

        public UI_Win_GuildBuildDescView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public PolygonImage m_pl_build_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public BtnAnimation m_btn_info_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_remove_PolygonImage;
		[HideInInspector] public GameButton m_btn_remove_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_remove_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_hp_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_hp_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_infoBar_PolygonImage;
		[HideInInspector] public GameButton m_btn_infoBar_GameButton;
		[HideInInspector] public BtnAnimation m_btn_infoBar_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_BuildLevelInfo_PolygonImage;

		[HideInInspector] public PolygonImage m_img_fill_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_state_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_fireSpeed_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_fireSpeed_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_fire;
		[HideInInspector] public LanguageText m_lbl_fireTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_fireTime_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_fireFighting_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_fireFighting_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_btn_fireFightingAlliance;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_fireFightingDaner;
		[HideInInspector] public LanguageText m_lbl_limit_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_limit_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_cdTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_cdTime_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_build_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/right/pl_build");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/right/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"rect/right/btn_info");
			m_btn_info_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/right/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/right/btn_info");

			m_btn_remove_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/right/btn_remove");
			m_btn_remove_GameButton = FindUI<GameButton>(vb.transform ,"rect/right/btn_remove");
			m_btn_remove_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/right/btn_remove");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/lbl_mes");

			m_lbl_hp_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_hp");
			m_lbl_hp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/lbl_hp");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/left/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/pb_rogressBar");

			m_btn_infoBar_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/pb_rogressBar/btn_infoBar");
			m_btn_infoBar_GameButton = FindUI<GameButton>(vb.transform ,"rect/left/pb_rogressBar/btn_infoBar");
			m_btn_infoBar_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/left/pb_rogressBar/btn_infoBar");

			m_img_BuildLevelInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/pb_rogressBar/btn_infoBar/img_BuildLevelInfo");

			m_img_fill_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/pb_rogressBar/Fill Area/img_fill");

			m_lbl_state_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_state");
			m_lbl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/lbl_state");

			m_lbl_fireSpeed_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_fireSpeed");
			m_lbl_fireSpeed_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/lbl_fireSpeed");

			m_pl_fire = FindUI<RectTransform>(vb.transform ,"rect/left/pl_fire");
			m_lbl_fireTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/pl_fire/lbl_fireTime");
			m_lbl_fireTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/pl_fire/lbl_fireTime");

			m_lbl_fireFighting_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/pl_fire/lbl_fireFighting");
			m_lbl_fireFighting_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/pl_fire/lbl_fireFighting");

			m_btn_fireFightingAlliance = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/pl_fire/btn_fireFightingAlliance"));
			m_btn_fireFightingDaner = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/pl_fire/btn_fireFightingDaner"));
			m_lbl_limit_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/pl_fire/lbl_limit");
			m_lbl_limit_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/pl_fire/lbl_limit");

			m_lbl_cdTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/pl_fire/lbl_cdTime");
			m_lbl_cdTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/pl_fire/lbl_cdTime");


            UI_Win_GuildBuildDescMediator mt = new UI_Win_GuildBuildDescMediator(vb.gameObject);
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
