// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月5日
// Update Time         :    2020年3月5日
// Class Description   :    HospitalView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class HospitalView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Hospital";

        public HospitalView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window;
		[HideInInspector] public RectTransform m_pl_bg_effect;
		[HideInInspector] public PolygonImage m_img_manFull_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_manFull_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_manNone_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_manNone_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_count;
		[HideInInspector] public PolygonImage m_btn_countInfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_countInfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_countInfo_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_img_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_countTotal_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_countTotal_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_right_content_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_hostipalEmpty_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_notlEmpty_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title2_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_armyList_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_armyList_PolygonImage;
		[HideInInspector] public ListView m_sv_armyList_ListView;

		[HideInInspector] public PolygonImage m_pl_res_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_pl_res_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_res_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesConsume1;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesConsume2;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesConsume3;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesConsume4;
		[HideInInspector] public PolygonImage m_pl_timebar_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_pl_timebar_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_sd_count_GameSlider;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public LanguageText m_lbl_countdown_LanguageText;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue_long_SubView m_UI_Model_blue;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_long_SubView m_UI_Model_yellow;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue_long_SubView m_UI_Model_max;
		[HideInInspector] public RectTransform m_pl_item_effect;
		[HideInInspector] public RectTransform m_pl_explain;
		[HideInInspector] public ScrollRect m_sv_explain_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_explain_view_PolygonImage;
		[HideInInspector] public ListView m_sv_explain_view_ListView;

		[HideInInspector] public PolygonImage m_v_explain_view_PolygonImage;
		[HideInInspector] public Mask m_v_explain_view_Mask;

		[HideInInspector] public RectTransform m_c_explain_view;
		[HideInInspector] public LanguageText m_lbl_explain_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_explain_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_explain_ContentSizeFitter;



        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_Window = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window"));
			m_pl_bg_effect = FindUI<RectTransform>(vb.transform ,"Rect/Left/pl_bg_effect");
			m_img_manFull_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Left/img_manFull");
			m_img_manFull_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Left/img_manFull");

			m_img_manNone_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Left/img_manNone");
			m_img_manNone_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Left/img_manNone");

			m_pl_count = FindUI<RectTransform>(vb.transform ,"Rect/Left/pl_count");
			m_btn_countInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Left/pl_count/btn_countInfo");
			m_btn_countInfo_GameButton = FindUI<GameButton>(vb.transform ,"Rect/Left/pl_count/btn_countInfo");
			m_btn_countInfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Left/pl_count/btn_countInfo");

			m_img_img_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Left/pl_count/img_img");
			m_img_img_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Left/pl_count/img_img");

			m_lbl_countTotal_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/Left/pl_count/lbl_countTotal");
			m_lbl_countTotal_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Left/pl_count/lbl_countTotal");

			m_pl_right_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Right/pl_right_content");

			m_pl_hostipalEmpty_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Right/pl_right_content/pl_hostipalEmpty");

			m_pl_notlEmpty_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty");

			m_lbl_title2_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/lbl_title2");
			m_lbl_title2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/lbl_title2");

			m_sv_armyList_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/armys/sv_armyList");
			m_sv_armyList_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/armys/sv_armyList");
			m_sv_armyList_ListView = FindUI<ListView>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/armys/sv_armyList");

			m_pl_res_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_res");
			m_pl_res_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_res");
			m_pl_res_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_res");

			m_UI_Model_ResourcesConsume1 = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_res/UI_Model_ResourcesConsume1"));
			m_UI_Model_ResourcesConsume2 = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_res/UI_Model_ResourcesConsume2"));
			m_UI_Model_ResourcesConsume3 = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_res/UI_Model_ResourcesConsume3"));
			m_UI_Model_ResourcesConsume4 = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_res/UI_Model_ResourcesConsume4"));
			m_pl_timebar_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_timebar");
			m_pl_timebar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_timebar");

			m_sd_count_GameSlider = FindUI<GameSlider>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_timebar/sd_count");

			m_pl_effect = FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_timebar/sd_count/Handle Slide Area/Handle/pl_effect");
			m_lbl_countdown_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_timebar/lbl_countdown");

			m_UI_Model_blue = new UI_Model_DoubleLineButton_Blue_long_SubView(FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/btns/UI_Model_blue"));
			m_UI_Model_yellow = new UI_Model_DoubleLineButton_Yellow_long_SubView(FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/btns/UI_Model_yellow"));
			m_UI_Model_max = new UI_Model_DoubleLineButton_Blue_long_SubView(FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/btns/UI_Model_max"));
			m_pl_item_effect = FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_right_content/pl_notlEmpty/pl_item_effect");
			m_pl_explain = FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_explain");
			m_sv_explain_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view");
			m_sv_explain_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view");
			m_sv_explain_view_ListView = FindUI<ListView>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view");

			m_v_explain_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view/v_explain_view");
			m_v_explain_view_Mask = FindUI<Mask>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view/v_explain_view");

			m_c_explain_view = FindUI<RectTransform>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view/v_explain_view/c_explain_view");
			m_lbl_explain_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view/v_explain_view/c_explain_view/lbl_explain");
			m_lbl_explain_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view/v_explain_view/c_explain_view/lbl_explain");
			m_lbl_explain_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"Rect/Right/pl_explain/sv_explain_view/v_explain_view/c_explain_view/lbl_explain");


            HospitalMediator mt = new HospitalMediator(vb.gameObject);
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
