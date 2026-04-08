// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月18日
// Update Time         :    2020年3月18日
// Class Description   :    UI_Win_BuildingUpdateView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_BuildingUpdateView : GameView
    {
        public const string VIEW_NAME = "UI_Win_BuildingUpdate";

        public UI_Win_BuildingUpdateView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public CanvasGroup m_pl_content_CanvasGroup;
		[HideInInspector] public Animator m_pl_content_Animator;

		[HideInInspector] public PolygonImage m_img_titleflag_PolygonImage;

		[HideInInspector] public RectTransform m_pl_buildImg;
		[HideInInspector] public PolygonImage m_btn_BuildLevelInfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_BuildLevelInfo_GameButton;
		[HideInInspector] public BtnAnimation m_btn_BuildLevelInfo_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_left_LanguageText;

		[HideInInspector] public VerticalLayoutGroup m_pl_Right_VerticalLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_Right_ArabLayoutCompment;

		[HideInInspector] public UI_Item_Buildinglevelup_SubView m_UI_Item_Buildinglevelup;
		[HideInInspector] public PolygonImage m_pl_attr_PolygonImage;
		[HideInInspector] public VerticalLayoutGroup m_pl_attr_VerticalLayoutGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_upgrade_ArabLayoutCompment;

		[HideInInspector] public UI_Item_BuildingUpgrade_SubView m_UI_Item_BuildingUpgrade;
		[HideInInspector] public Animator m_pl_sv_level_Animator;
		[HideInInspector] public CanvasGroup m_pl_sv_level_CanvasGroup;

		[HideInInspector] public RectTransform m_pl_tilte;
		[HideInInspector] public ScrollRect m_sv_levelData_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_levelData_PolygonImage;
		[HideInInspector] public ListView m_sv_levelData_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_pl_content_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"Rect/pl_content");
			m_pl_content_Animator = FindUI<Animator>(vb.transform ,"Rect/pl_content");

			m_img_titleflag_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/Left/img_titleflag");

			m_pl_buildImg = FindUI<RectTransform>(vb.transform ,"Rect/pl_content/Left/pl_buildImg");
			m_btn_BuildLevelInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo");
			m_btn_BuildLevelInfo_GameButton = FindUI<GameButton>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo");
			m_btn_BuildLevelInfo_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo");

			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/Left/btn_BuildLevelInfo/img_polygonImage");

			m_lbl_languageText_left_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/Left/lbl_languageText_left");

			m_pl_Right_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"Rect/pl_content/pl_Right");
			m_pl_Right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right");

			m_UI_Item_Buildinglevelup = new UI_Item_Buildinglevelup_SubView(FindUI<RectTransform>(vb.transform ,"Rect/pl_content/pl_Right/UI_Item_Buildinglevelup"));
			m_pl_attr_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_attr");
			m_pl_attr_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"Rect/pl_content/pl_Right/pl_attr");

			m_pl_upgrade_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_upgrade");

			m_UI_Item_BuildingUpgrade = new UI_Item_BuildingUpgrade_SubView(FindUI<RectTransform>(vb.transform ,"Rect/pl_content/pl_upgrade/UI_Item_BuildingUpgrade"));
			m_pl_sv_level_Animator = FindUI<Animator>(vb.transform ,"Rect/pl_sv_level");
			m_pl_sv_level_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"Rect/pl_sv_level");

			m_pl_tilte = FindUI<RectTransform>(vb.transform ,"Rect/pl_sv_level/pl_tilte");
			m_sv_levelData_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/pl_sv_level/sv_levelData");
			m_sv_levelData_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_sv_level/sv_levelData");
			m_sv_levelData_ListView = FindUI<ListView>(vb.transform ,"Rect/pl_sv_level/sv_levelData");


            UI_Win_BuildingUpdateMediator mt = new UI_Win_BuildingUpdateMediator(vb.gameObject);
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
