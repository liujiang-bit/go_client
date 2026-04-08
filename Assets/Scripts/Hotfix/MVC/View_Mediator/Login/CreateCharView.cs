// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, August 19, 2020
// Update Time         :    Wednesday, August 19, 2020
// Class Description   :    CreateCharView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class CreateCharView : GameView
    {
        public const string VIEW_NAME = "CreateCharView";

        public CreateCharView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_pl_charChoose_Animator;

		[HideInInspector] public PolygonImage m_img_charCountryBg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_charCountryFlag_PolygonImage;
		[HideInInspector] public CanvasGroup m_img_charCountryFlag_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_img_charCountryFlag_ArabLayoutCompment;
		[HideInInspector] public UIDefaultValue m_img_charCountryFlag_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_charCountryIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_charCountryIcon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_spDesBg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_charBuildBg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_countryName_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_countryName_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_countryName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_init_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_init_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_horeName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_horeName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_spName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_spName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_spDes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_spDes_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_spbg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_spbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_spUnitIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_spUnitIcon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_spUnit_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_spUnit_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_spUnitName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_spUnitName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_totalDes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_totalDes_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_UI_btn_ok;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_UI_change;
		[HideInInspector] public UI_Common_Spin_SubView m_UI_Common_Spin;
		[HideInInspector] public CanvasGroup m_pl_horeSpine_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_horeSpine_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_pl_heroOffset_SkeletonGraphic;

		[HideInInspector] public RectTransform m_pl_welcome;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public Shadow m_lbl_languageText_Shadow;

		[HideInInspector] public LanguageText m_lbl_welcome_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_welcome_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_btn_proto_LanguageText;
		[HideInInspector] public GameButton m_btn_proto_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_proto_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_proto_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_proto_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_proto_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_proto_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_pl_womanSpine_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_pl_womanSpine_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_country_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_country_PolygonImage;
		[HideInInspector] public ListView m_sv_list_country_ListView;
		[HideInInspector] public CanvasGroup m_sv_list_country_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_sv_list_country_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_img_changeTip_PolygonImage;

		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;


        private void UIFinder()
        {
			m_pl_charChoose_Animator = FindUI<Animator>(vb.transform ,"pl_charChoose");

			m_img_charCountryBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_charChoose/img_charCountryBg");

			m_img_charCountryFlag_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_charChoose/img_charCountryFlag");
			m_img_charCountryFlag_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_charChoose/img_charCountryFlag");
			m_img_charCountryFlag_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag");
			m_img_charCountryFlag_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_charChoose/img_charCountryFlag");

			m_img_charCountryIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_charCountryIcon");
			m_img_charCountryIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_charCountryIcon");

			m_img_spDesBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spDesBg");

			m_img_charBuildBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_charBuildBg");

			m_lbl_countryName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_countryName");
			m_lbl_countryName_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_countryName");
			m_lbl_countryName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_countryName");

			m_lbl_init_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_countryName/lbl_init");
			m_lbl_init_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_countryName/lbl_init");

			m_lbl_horeName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_countryName/lbl_horeName");
			m_lbl_horeName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_countryName/lbl_horeName");

			m_lbl_spName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_spName");
			m_lbl_spName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_spName");

			m_lbl_spDes_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_spDes");
			m_lbl_spDes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_spDes");

			m_img_spbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg");
			m_img_spbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg");

			m_img_spUnitIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg/img_spUnitIcon");
			m_img_spUnitIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg/img_spUnitIcon");

			m_lbl_spUnit_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg/lbl_spUnit");
			m_lbl_spUnit_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg/lbl_spUnit");

			m_lbl_spUnitName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg/lbl_spUnitName");
			m_lbl_spUnitName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/img_spbg/lbl_spUnitName");

			m_lbl_totalDes_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_totalDes");
			m_lbl_totalDes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/img_charCountryFlag/lbl_totalDes");

			m_UI_btn_ok = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_charChoose/img_charCountryFlag/UI_btn_ok"));
			m_UI_change = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_charChoose/img_charCountryFlag/UI_change"));
			m_UI_Common_Spin = new UI_Common_Spin_SubView(FindUI<RectTransform>(vb.transform ,"pl_charChoose/img_charCountryFlag/UI_Common_Spin"));
			m_pl_horeSpine_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_charChoose/pl_horeSpine");
			m_pl_horeSpine_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_charChoose/pl_horeSpine");

			m_pl_heroOffset_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"pl_charChoose/pl_horeSpine/pl_heroOffset");

			m_pl_welcome = FindUI<RectTransform>(vb.transform ,"pl_welcome");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_welcome/img_bg");

			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_welcome/img_polygonImage");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_welcome/img_polygonImage/lbl_languageText");
			m_lbl_languageText_Shadow = FindUI<Shadow>(vb.transform ,"pl_welcome/img_polygonImage/lbl_languageText");

			m_lbl_welcome_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_welcome/bg2/lbl_welcome");
			m_lbl_welcome_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_welcome/bg2/lbl_welcome");

			m_btn_proto_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_welcome/btn_proto");
			m_btn_proto_GameButton = FindUI<GameButton>(vb.transform ,"pl_welcome/btn_proto");
			m_btn_proto_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_welcome/btn_proto");

			m_ck_proto_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_welcome/ck_proto");
			m_ck_proto_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_welcome/ck_proto");

			m_lbl_proto_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_welcome/ck_proto/lbl_proto");
			m_lbl_proto_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_welcome/ck_proto/lbl_proto");

			m_pl_womanSpine_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"pl_welcome/pl_womanSpine");
			m_pl_womanSpine_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_welcome/pl_womanSpine");

			m_sv_list_country_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_list_country");
			m_sv_list_country_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list_country");
			m_sv_list_country_ListView = FindUI<ListView>(vb.transform ,"sv_list_country");
			m_sv_list_country_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"sv_list_country");
			m_sv_list_country_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"sv_list_country");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list_country/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"sv_list_country/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"sv_list_country/v_list_view/c_list_view");
			m_img_changeTip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_changeTip");

			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));

            CreateCharMediator mt = new CreateCharMediator(vb.gameObject);
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
