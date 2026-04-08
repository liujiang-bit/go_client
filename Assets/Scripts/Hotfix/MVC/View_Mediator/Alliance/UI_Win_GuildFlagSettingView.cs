// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, May 12, 2020
// Update Time         :    Tuesday, May 12, 2020
// Class Description   :    UI_Win_GuildFlagSettingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildFlagSettingView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildFlagSetting";

        public UI_Win_GuildFlagSettingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public ScrollRect m_sv_bg_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_bg_PolygonImage;
		[HideInInspector] public ListView m_sv_bg_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_bg_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_sd_bgcolor_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_bgcolor_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bgC0_PolygonImage;
		[HideInInspector] public LayoutElement m_img_bgC0_LayoutElement;

		[HideInInspector] public PolygonImage m_img_bgC1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC4_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC5_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC6_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC7_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC8_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC9_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC10_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC11_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC12_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC13_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC14_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgC15_PolygonImage;
		[HideInInspector] public LayoutElement m_img_bgC15_LayoutElement;

		[HideInInspector] public ScrollRect m_sv_img_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_img_PolygonImage;
		[HideInInspector] public ListView m_sv_img_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_img_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_sd_imgColor_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_imgColor_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_imgC0_PolygonImage;
		[HideInInspector] public LayoutElement m_img_imgC0_LayoutElement;

		[HideInInspector] public PolygonImage m_img_imgC1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_imgC2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_imgC3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_imgC4_PolygonImage;

		[HideInInspector] public PolygonImage m_img_imgC5_PolygonImage;

		[HideInInspector] public PolygonImage m_img_imgC6_PolygonImage;
		[HideInInspector] public LayoutElement m_img_imgC6_LayoutElement;

		[HideInInspector] public RectTransform m_pl_building;
		[HideInInspector] public PolygonImage m_img_flagRange_PolygonImage;

		[HideInInspector] public UI_alliance_1002_SubView m_UI_alliance_1002;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_Model_GuildFlag;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_ok;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_btn_change;
		[HideInInspector] public PolygonImage m_btn_random_PolygonImage;
		[HideInInspector] public GameButton m_btn_random_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_random_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_sv_bg_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/left/top/sv_bg");
			m_sv_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sv_bg");
			m_sv_bg_ListView = FindUI<ListView>(vb.transform ,"rect/left/top/sv_bg");
			m_sv_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/top/sv_bg");

			m_sd_bgcolor_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/left/top/sd_bgcolor");
			m_sd_bgcolor_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/top/sd_bgcolor");

			m_img_bgC0_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC0");
			m_img_bgC0_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC0");

			m_img_bgC1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC1");

			m_img_bgC2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC2");

			m_img_bgC3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC3");

			m_img_bgC4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC4");

			m_img_bgC5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC5");

			m_img_bgC6_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC6");

			m_img_bgC7_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC7");

			m_img_bgC8_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC8");

			m_img_bgC9_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC9");

			m_img_bgC10_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC10");

			m_img_bgC11_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC11");

			m_img_bgC12_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC12");

			m_img_bgC13_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC13");

			m_img_bgC14_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC14");

			m_img_bgC15_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC15");
			m_img_bgC15_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/left/top/sd_bgcolor/Background/img_bgC15");

			m_sv_img_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/left/buttom/sv_img");
			m_sv_img_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sv_img");
			m_sv_img_ListView = FindUI<ListView>(vb.transform ,"rect/left/buttom/sv_img");
			m_sv_img_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/buttom/sv_img");

			m_sd_imgColor_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/left/buttom/sd_imgColor");
			m_sd_imgColor_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/buttom/sd_imgColor");

			m_img_imgC0_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC0");
			m_img_imgC0_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC0");

			m_img_imgC1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC1");

			m_img_imgC2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC2");

			m_img_imgC3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC3");

			m_img_imgC4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC4");

			m_img_imgC5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC5");

			m_img_imgC6_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC6");
			m_img_imgC6_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/left/buttom/sd_imgColor/Background/img_imgC6");

			m_pl_building = FindUI<RectTransform>(vb.transform ,"rect/right/pl_building");
			m_img_flagRange_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/right/pl_building/img_flagRange");

			m_UI_alliance_1002 = new UI_alliance_1002_SubView(FindUI<RectTransform>(vb.transform ,"rect/right/pl_building/UI_alliance_1002"));
			m_UI_Model_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"rect/right/flag/UI_Model_GuildFlag"));
			m_btn_ok = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/right/btn/btn_ok"));
			m_btn_change = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/right/btn/btn_change"));
			m_btn_random_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/right/btn/btn_random");
			m_btn_random_GameButton = FindUI<GameButton>(vb.transform ,"rect/right/btn/btn_random");
			m_btn_random_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/right/btn/btn_random");


            UI_Win_GuildFlagSettingMediator mt = new UI_Win_GuildFlagSettingMediator(vb.gameObject);
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
