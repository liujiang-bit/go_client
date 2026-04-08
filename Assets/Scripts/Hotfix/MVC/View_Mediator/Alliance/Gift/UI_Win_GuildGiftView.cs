// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, July 31, 2020
// Update Time         :    Friday, July 31, 2020
// Class Description   :    UI_Win_GuildGiftView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildGiftView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildGift";

        public UI_Win_GuildGiftView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public GameSlider m_pb_giftBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_giftBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_giftbarText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_giftbarText_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_question_PolygonImage;
		[HideInInspector] public GameButton m_btn_question_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_question_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_topPoint_PolygonImage;
		[HideInInspector] public GameButton m_btn_topPoint_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_topPoint_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_boxLevel_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_boxLevel_ArabLayoutCompment;

		[HideInInspector] public UI_Effect_BuildShow_SubView m_UI_Effect_BuildShow;
		[HideInInspector] public LanguageText m_lbl_boxName_LanguageText;

		[HideInInspector] public PolygonImage m_btn_Box_PolygonImage;
		[HideInInspector] public GameButton m_btn_Box_GameButton;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_pl_box;
		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Redpoint;
		[HideInInspector] public GameSlider m_pb_boxBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_boxBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_boxbarText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_boxbarText_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_key_PolygonImage;
		[HideInInspector] public GameButton m_btn_key_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_key_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_passTime_LanguageText;

		[HideInInspector] public GameToggle m_ck_normal_GameToggle;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_redpoint_normal;
		[HideInInspector] public GameToggle m_ck_rara_GameToggle;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_redpoint_rara;
		[HideInInspector] public ScrollRect m_sv_list_common_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_common_PolygonImage;
		[HideInInspector] public ListView m_sv_list_common_ListView;

		[HideInInspector] public ScrollRect m_sv_list_uncommon_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_uncommon_PolygonImage;
		[HideInInspector] public ListView m_sv_list_uncommon_ListView;

		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_delete_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_getAll_PolygonImage;
		[HideInInspector] public GameButton m_btn_getAll_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_getAll_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_nogift_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_nogift_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pb_giftBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/top/pb_giftBar");
			m_pb_giftBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/pb_giftBar");

			m_lbl_giftbarText_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/pb_giftBar/lbl_giftbarText");
			m_lbl_giftbarText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/pb_giftBar/lbl_giftbarText");

			m_btn_question_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_question");
			m_btn_question_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_question");
			m_btn_question_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_question");

			m_btn_topPoint_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_topPoint");
			m_btn_topPoint_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_topPoint");
			m_btn_topPoint_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_topPoint");

			m_lbl_boxLevel_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/lbl_boxLevel");
			m_lbl_boxLevel_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/lbl_boxLevel");

			m_UI_Effect_BuildShow = new UI_Effect_BuildShow_SubView(FindUI<RectTransform>(vb.transform ,"rect/box/UI_Effect_BuildShow"));
			m_lbl_boxName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/box/blet/lbl_boxName");

			m_btn_Box_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/box/btn_Box");
			m_btn_Box_GameButton = FindUI<GameButton>(vb.transform ,"rect/box/btn_Box");

			m_pl_box = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/box/pl_box"));
			m_UI_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"rect/box/UI_Redpoint"));
			m_pb_boxBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/box/pb_boxBar");
			m_pb_boxBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/box/pb_boxBar");

			m_lbl_boxbarText_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/box/pb_boxBar/lbl_boxbarText");
			m_lbl_boxbarText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/box/pb_boxBar/lbl_boxbarText");

			m_btn_key_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/box/btn_key");
			m_btn_key_GameButton = FindUI<GameButton>(vb.transform ,"rect/box/btn_key");
			m_btn_key_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/box/btn_key");

			m_lbl_passTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/box/lbl_passTime");

			m_ck_normal_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/list/toggle/ck_normal");

			m_UI_redpoint_normal = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"rect/list/toggle/ck_normal/UI_redpoint_normal"));
			m_ck_rara_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/list/toggle/ck_rara");

			m_UI_redpoint_rara = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"rect/list/toggle/ck_rara/UI_redpoint_rara"));
			m_sv_list_common_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/list/bg/sv_list_common");
			m_sv_list_common_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/list/bg/sv_list_common");
			m_sv_list_common_ListView = FindUI<ListView>(vb.transform ,"rect/list/bg/sv_list_common");

			m_sv_list_uncommon_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/list/bg/sv_list_uncommon");
			m_sv_list_uncommon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/list/bg/sv_list_uncommon");
			m_sv_list_uncommon_ListView = FindUI<ListView>(vb.transform ,"rect/list/bg/sv_list_uncommon");

			m_btn_delete_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/list/btn_delete");
			m_btn_delete_GameButton = FindUI<GameButton>(vb.transform ,"rect/list/btn_delete");
			m_btn_delete_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/list/btn_delete");

			m_btn_getAll_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/list/btn_getAll");
			m_btn_getAll_GameButton = FindUI<GameButton>(vb.transform ,"rect/list/btn_getAll");
			m_btn_getAll_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/list/btn_getAll");

			m_lbl_nogift_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/list/lbl_nogift");
			m_lbl_nogift_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/list/lbl_nogift");


            UI_Win_GuildGiftMediator mt = new UI_Win_GuildGiftMediator(vb.gameObject);
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
