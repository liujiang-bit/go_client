// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月23日
// Update Time         :    2020年9月23日
// Class Description   :    UI_Win_BookView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_BookView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Book";

        public UI_Win_BookView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public Empty4Raycast m_pl_top_Empty4Raycast;

		[HideInInspector] public GridLayoutGroup m_pl_group_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_group_ArabLayoutCompment;

		[HideInInspector] public UI_Item_BookGroup_SubView m_img_all;
		[HideInInspector] public UI_Item_BookGroup_SubView m_img_special;
		[HideInInspector] public UI_Item_BookGroup_SubView m_img_friends;
		[HideInInspector] public UI_Item_BookGroup_SubView m_img_enemy;
		[HideInInspector] public UI_Item_BookGroup_SubView m_img_alliance;
		[HideInInspector] public ToggleGroup m_pl_toggle_group_ToggleGroup;
		[HideInInspector] public LayoutElement m_pl_toggle_group_LayoutElement;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;
		[HideInInspector] public LayoutElement m_img_line_LayoutElement;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_PolygonImage;
		[HideInInspector] public Mask m_v_list_Mask;

		[HideInInspector] public RectTransform m_c_list;
		[HideInInspector] public LanguageText m_lbl_none_LanguageText;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_top_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_top");

			m_pl_group_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_top/pl_group");
			m_pl_group_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_top/pl_group");

			m_img_all = new UI_Item_BookGroup_SubView(FindUI<RectTransform>(vb.transform ,"pl_top/pl_group/img_all"));
			m_img_special = new UI_Item_BookGroup_SubView(FindUI<RectTransform>(vb.transform ,"pl_top/pl_group/img_special"));
			m_img_friends = new UI_Item_BookGroup_SubView(FindUI<RectTransform>(vb.transform ,"pl_top/pl_group/img_friends"));
			m_img_enemy = new UI_Item_BookGroup_SubView(FindUI<RectTransform>(vb.transform ,"pl_top/pl_group/img_enemy"));
			m_img_alliance = new UI_Item_BookGroup_SubView(FindUI<RectTransform>(vb.transform ,"pl_top/pl_group/img_alliance"));
			m_pl_toggle_group_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"pl_top/pl_group/pl_toggle_group");
			m_pl_toggle_group_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_top/pl_group/pl_toggle_group");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_top/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_top/lbl_num");

			m_img_line_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_line");
			m_img_line_LayoutElement = FindUI<LayoutElement>(vb.transform ,"img_line");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"sv_list");

			m_v_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list/v_list");
			m_v_list_Mask = FindUI<Mask>(vb.transform ,"sv_list/v_list");

			m_c_list = FindUI<RectTransform>(vb.transform ,"sv_list/v_list/c_list");
			m_lbl_none_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_none");


            UI_Win_BookMediator mt = new UI_Win_BookMediator(vb.gameObject);
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
