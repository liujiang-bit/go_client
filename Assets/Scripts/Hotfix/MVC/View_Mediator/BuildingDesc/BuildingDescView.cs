// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月7日
// Update Time         :    2020年3月7日
// Class Description   :    BuildingDescView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class BuildingDescView : GameView
    {
        public const string VIEW_NAME = "UI_Win_BuildingDesc";

        public BuildingDescView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public Animator m_pl_content_Animator;
		[HideInInspector] public CanvasGroup m_pl_content_CanvasGroup;

		[HideInInspector] public PolygonImage m_img_buildImg_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_BuildLevelInfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_BuildLevelInfo_GameButton;
		[HideInInspector] public BtnAnimation m_btn_BuildLevelInfo_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_BuildLevelInfo_PolygonImage;

		[HideInInspector] public VerticalLayoutGroup m_pl_Right_VerticalLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_Right_ArabLayoutCompment;

		[HideInInspector] public LayoutElement m_pl_attr_pos1_LayoutElement;

		[HideInInspector] public LayoutElement m_pl_attr_pos2_LayoutElement;

		[HideInInspector] public PolygonImage m_pl_solider_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_attr_PolygonImage;
		[HideInInspector] public VerticalLayoutGroup m_pl_attr_VerticalLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_attr_ContentSizeFitter;

		[HideInInspector] public ScrollRect m_sv_center_text_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_center_text_PolygonImage;
		[HideInInspector] public ListView m_sv_center_text_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_center_text_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_c_center_text;
		[HideInInspector] public LanguageText m_lbl_center_buildingDesc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_center_buildingDesc_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_bottom_text_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_bottom_text_PolygonImage;
		[HideInInspector] public ListView m_sv_bottom_text_ListView;

		[HideInInspector] public RectTransform m_c_bottom_text;
		[HideInInspector] public LanguageText m_lbl_bottom_buildingDesc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_bottom_buildingDesc_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_bottom_text2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_bottom_text2_PolygonImage;
		[HideInInspector] public ListView m_sv_bottom_text2_ListView;

		[HideInInspector] public RectTransform m_c_bottom_text2;
		[HideInInspector] public LanguageText m_lbl_bottom_buildingDesc2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_bottom_buildingDesc2_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_sv_level_CanvasGroup;
		[HideInInspector] public Animator m_pl_sv_level_Animator;

		[HideInInspector] public RectTransform m_pl_tilte;
		[HideInInspector] public ScrollRect m_sv_levelData_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_levelData_PolygonImage;
		[HideInInspector] public ListView m_sv_levelData_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_pl_content_Animator = FindUI<Animator>(vb.transform ,"Rect/pl_content");
			m_pl_content_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"Rect/pl_content");

			m_img_buildImg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/Left/img_buildImg");

			m_btn_BuildLevelInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo");
			m_btn_BuildLevelInfo_GameButton = FindUI<GameButton>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo");
			m_btn_BuildLevelInfo_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo");

			m_img_BuildLevelInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo/img_BuildLevelInfo");

			m_pl_Right_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"Rect/pl_content/pl_Right");
			m_pl_Right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right");

			m_pl_attr_pos1_LayoutElement = FindUI<LayoutElement>(vb.transform ,"Rect/pl_content/pl_Right/pl_attr_pos1");

			m_pl_attr_pos2_LayoutElement = FindUI<LayoutElement>(vb.transform ,"Rect/pl_content/pl_Right/pl_attr_pos2");

			m_pl_solider_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_solider");

			m_pl_attr_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_attr");
			m_pl_attr_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"Rect/pl_content/pl_Right/pl_attr");
			m_pl_attr_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"Rect/pl_content/pl_Right/pl_attr");

			m_sv_center_text_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/pl_content/pl_Right/sv_center_text");
			m_sv_center_text_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/sv_center_text");
			m_sv_center_text_ListView = FindUI<ListView>(vb.transform ,"Rect/pl_content/pl_Right/sv_center_text");
			m_sv_center_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/sv_center_text");

			m_c_center_text = FindUI<RectTransform>(vb.transform ,"Rect/pl_content/pl_Right/sv_center_text/v/c_center_text");
			m_lbl_center_buildingDesc_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/pl_Right/sv_center_text/v/c_center_text/lbl_center_buildingDesc");
			m_lbl_center_buildingDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/sv_center_text/v/c_center_text/lbl_center_buildingDesc");

			m_sv_bottom_text_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text");
			m_sv_bottom_text_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text");
			m_sv_bottom_text_ListView = FindUI<ListView>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text");

			m_c_bottom_text = FindUI<RectTransform>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text/v/c_bottom_text");
			m_lbl_bottom_buildingDesc_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text/v/c_bottom_text/lbl_bottom_buildingDesc");
			m_lbl_bottom_buildingDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text/v/c_bottom_text/lbl_bottom_buildingDesc");

			m_sv_bottom_text2_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text2");
			m_sv_bottom_text2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text2");
			m_sv_bottom_text2_ListView = FindUI<ListView>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text2");

			m_c_bottom_text2 = FindUI<RectTransform>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text2/v/c_bottom_text2");
			m_lbl_bottom_buildingDesc2_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text2/v/c_bottom_text2/lbl_bottom_buildingDesc2");
			m_lbl_bottom_buildingDesc2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/sv_bottom_text2/v/c_bottom_text2/lbl_bottom_buildingDesc2");

			m_pl_sv_level_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"Rect/pl_sv_level");
			m_pl_sv_level_Animator = FindUI<Animator>(vb.transform ,"Rect/pl_sv_level");

			m_pl_tilte = FindUI<RectTransform>(vb.transform ,"Rect/pl_sv_level/pl_tilte");
			m_sv_levelData_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/pl_sv_level/sv_levelData");
			m_sv_levelData_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_sv_level/sv_levelData");
			m_sv_levelData_ListView = FindUI<ListView>(vb.transform ,"Rect/pl_sv_level/sv_levelData");


            BuildingDescMediator mt = new BuildingDescMediator(vb.gameObject);
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
