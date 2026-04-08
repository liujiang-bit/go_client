// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月24日
// Update Time         :    2020年7月24日
// Class Description   :    CaptainSummonView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class CaptainSummonView : GameView
    {
        public const string VIEW_NAME = "UI_IF_CaptainSummon";

        public CaptainSummonView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Model_Interface_SubView m_pl_modelIF;
		[HideInInspector] public PolygonImage m_btn_char_PolygonImage;
		[HideInInspector] public GameButton m_btn_char_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_char_ArabLayoutCompment;
		[HideInInspector] public CanvasGroup m_btn_char_CanvasGroup;
		[HideInInspector] public Animator m_btn_char_Animator;

		[HideInInspector] public SkeletonGraphic m_spin_hero_SkeletonGraphic;

		[HideInInspector] public ArabLayoutCompment m_pl_rect_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_rect_Animator;
		[HideInInspector] public CanvasGroup m_pl_rect_CanvasGroup;
		[HideInInspector] public UIDefaultValue m_pl_rect_UIDefaultValue;

		[HideInInspector] public UI_Item_CaptainHead_SubView m_UI_Item_CaptainHead;
		[HideInInspector] public ArabLayoutCompment m_pl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_img_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_title_PolygonImage;
		[HideInInspector] public GameButton m_btn_title_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_talent_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_talent_GridLayoutGroup;

		[HideInInspector] public UI_Model_CaptainTalent_SubView m_UI_Model_CaptainTalent_3;
		[HideInInspector] public UI_Model_CaptainTalent_SubView m_UI_Model_CaptainTalent_2;
		[HideInInspector] public UI_Model_CaptainTalent_SubView m_UI_Model_CaptainTalent_1;
		[HideInInspector] public PolygonImage m_pl_qualityBg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_pl_qualityBg_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_quality_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_quality_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_stroy_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_stroy_PolygonImage;
		[HideInInspector] public ListView m_sv_list_stroy_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_tipOnBtn_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_UI_Model_StandardButton_Blue_sure;
		[HideInInspector] public Image m_UI_3D_Scene_Image;



        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_pl_modelIF = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"pl_modelIF"));
			m_btn_char_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_char");
			m_btn_char_GameButton = FindUI<GameButton>(vb.transform ,"btn_char");
			m_btn_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_char");
			m_btn_char_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"btn_char");
			m_btn_char_Animator = FindUI<Animator>(vb.transform ,"btn_char");

			m_spin_hero_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"btn_char/spin_hero");

			m_pl_rect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect");
			m_pl_rect_Animator = FindUI<Animator>(vb.transform ,"pl_rect");
			m_pl_rect_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_rect");
			m_pl_rect_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_rect");

			m_UI_Item_CaptainHead = new UI_Item_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/headgroup/UI_Item_CaptainHead"));
			m_pl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/pl_title");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/headgroup/pl_title/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/pl_title/lbl_title");

			m_img_img_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/headgroup/pl_title/img_img");
			m_img_img_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/pl_title/img_img");

			m_btn_title_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/headgroup/pl_title/btn_title");
			m_btn_title_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/headgroup/pl_title/btn_title");
			m_btn_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/pl_title/btn_title");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/headgroup/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/lbl_name");

			m_pl_talent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/pl_talent");
			m_pl_talent_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/headgroup/pl_talent");

			m_UI_Model_CaptainTalent_3 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/headgroup/pl_talent/UI_Model_CaptainTalent_3"));
			m_UI_Model_CaptainTalent_2 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/headgroup/pl_talent/UI_Model_CaptainTalent_2"));
			m_UI_Model_CaptainTalent_1 = new UI_Model_CaptainTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/headgroup/pl_talent/UI_Model_CaptainTalent_1"));
			m_pl_qualityBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/headgroup/pl_qualityBg");
			m_pl_qualityBg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/pl_qualityBg");

			m_lbl_quality_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/headgroup/lbl_quality");
			m_lbl_quality_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/headgroup/lbl_quality");

			m_sv_list_stroy_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_rect/sv_list_stroy");
			m_sv_list_stroy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/sv_list_stroy");
			m_sv_list_stroy_ListView = FindUI<ListView>(vb.transform ,"pl_rect/sv_list_stroy");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/sv_list_stroy/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"pl_rect/sv_list_stroy/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"pl_rect/sv_list_stroy/v_list_view/c_list_view");
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/sv_list_stroy/v_list_view/c_list_view/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/sv_list_stroy/v_list_view/c_list_view/lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_rect/sv_list_stroy/v_list_view/c_list_view/lbl_text");

			m_lbl_tipOnBtn_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/lbl_tipOnBtn");

			m_UI_Model_StandardButton_Blue_sure = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/UI_Model_StandardButton_Blue_sure"));
			m_UI_3D_Scene_Image = FindUI<Image>(vb.transform ,"UI_3D_Scene");


            CaptainSummonMediator mt = new CaptainSummonMediator(vb.gameObject);
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
