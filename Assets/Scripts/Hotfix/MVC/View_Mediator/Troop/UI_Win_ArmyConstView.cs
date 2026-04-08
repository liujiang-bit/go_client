// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月4日
// Update Time         :    2020年9月4日
// Class Description   :    UI_Win_ArmyConstView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ArmyConstView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ArmyConst";

        public UI_Win_ArmyConstView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public Empty4Raycast m_pl_top_Empty4Raycast;

		[HideInInspector] public LanguageText m_lbl_total_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_total_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_total_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_total_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_total_num_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_troopsline_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_troopsline_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_troopsline_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_troopsline_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_troopsline_num_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_powar_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_powar_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_powar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_powar_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_powar_num_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_hospital_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_hospital_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_hospital_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_hospital_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_hospital_num_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_ck_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_ck_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;
		[HideInInspector] public LayoutElement m_img_line_LayoutElement;

		[HideInInspector] public GameToggle m_ck_total_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_total_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_total_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redpoint1_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num1_LanguageText;

		[HideInInspector] public GameToggle m_ck_incity_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_incity_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_incity_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redpoint2_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num2_LanguageText;

		[HideInInspector] public GameToggle m_ck_outcity_GameToggle;
		[HideInInspector] public BtnAnimation m_ck_outcity_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_ck_outcity_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redpoint3_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num3_LanguageText;

		[HideInInspector] public ToggleGroup m_pl_group_ToggleGroup;
		[HideInInspector] public LayoutElement m_pl_group_LayoutElement;

		[HideInInspector] public LanguageText m_lbl_notroops_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_PolygonImage;
		[HideInInspector] public Mask m_v_list_Mask;

		[HideInInspector] public RectTransform m_c_list;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_pl_top_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"rect/pl_top");

			m_lbl_total_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_total");
			m_lbl_total_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/pl_top/lbl_total");
			m_lbl_total_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_total");

			m_lbl_total_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_total/lbl_total_num");
			m_lbl_total_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_total/lbl_total_num");

			m_lbl_troopsline_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_troopsline");
			m_lbl_troopsline_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/pl_top/lbl_troopsline");
			m_lbl_troopsline_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_troopsline");

			m_lbl_troopsline_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_troopsline/lbl_troopsline_num");
			m_lbl_troopsline_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_troopsline/lbl_troopsline_num");

			m_lbl_powar_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_powar");
			m_lbl_powar_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/pl_top/lbl_powar");
			m_lbl_powar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_powar");

			m_lbl_powar_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_powar/lbl_powar_num");
			m_lbl_powar_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_powar/lbl_powar_num");

			m_lbl_hospital_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_hospital");
			m_lbl_hospital_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/pl_top/lbl_hospital");
			m_lbl_hospital_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_hospital");

			m_lbl_hospital_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_top/lbl_hospital/lbl_hospital_num");
			m_lbl_hospital_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_top/lbl_hospital/lbl_hospital_num");

			m_pl_ck_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_ck");
			m_pl_ck_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_ck");

			m_img_line_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_ck/img_line");
			m_img_line_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_ck/img_line");

			m_ck_total_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_ck/ck_total");
			m_ck_total_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/pl_ck/ck_total");
			m_ck_total_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_ck/ck_total");

			m_img_redpoint1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_ck/ck_total/img_redpoint1");

			m_lbl_num1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_ck/ck_total/img_redpoint1/lbl_num1");

			m_ck_incity_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_ck/ck_incity");
			m_ck_incity_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/pl_ck/ck_incity");
			m_ck_incity_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_ck/ck_incity");

			m_img_redpoint2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_ck/ck_incity/img_redpoint2");

			m_lbl_num2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_ck/ck_incity/img_redpoint2/lbl_num2");

			m_ck_outcity_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_ck/ck_outcity");
			m_ck_outcity_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/pl_ck/ck_outcity");
			m_ck_outcity_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_ck/ck_outcity");

			m_img_redpoint3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_ck/ck_outcity/img_redpoint3");

			m_lbl_num3_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_ck/ck_outcity/img_redpoint3/lbl_num3");

			m_pl_group_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"rect/pl_ck/pl_group");
			m_pl_group_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_ck/pl_group");

			m_lbl_notroops_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_notroops");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_v_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list/v_list");
			m_v_list_Mask = FindUI<Mask>(vb.transform ,"rect/sv_list/v_list");

			m_c_list = FindUI<RectTransform>(vb.transform ,"rect/sv_list/v_list/c_list");

            UI_Win_ArmyConstMediator mt = new UI_Win_ArmyConstMediator(vb.gameObject);
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
            vb.onMenuBackCallback = mt.onMenuBackCallback;
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
