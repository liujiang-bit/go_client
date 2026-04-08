// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月26日
// Update Time         :    2020年4月26日
// Class Description   :    UI_Win_CityManagerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_CityManagerView : GameView
    {
        public const string VIEW_NAME = "UI_Win_CityManager";

        public UI_Win_CityManagerView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public RectTransform m_pl_rect0;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public RectTransform m_pl_rect1;
		[HideInInspector] public RectTransform m_pl_view1;
		[HideInInspector] public ScrollRect m_sv_list1_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list1_PolygonImage;
		[HideInInspector] public ListView m_sv_list1_ListView;

		[HideInInspector] public PolygonImage m_v_scroll_view_PolygonImage;
		[HideInInspector] public Mask m_v_scroll_view_Mask;

		[HideInInspector] public RectTransform m_c_scroll_view;
		[HideInInspector] public RectTransform m_pl_view2;
		[HideInInspector] public LanguageText m_lbl_warDesc_LanguageText;

		[HideInInspector] public LanguageText m_lbl_line_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list2_PolygonImage;
		[HideInInspector] public ListView m_sv_list2_ListView;

		[HideInInspector] public RectTransform m_pl_c;
		[HideInInspector] public RectTransform m_UI_Item_CityManagerMes1;
		[HideInInspector] public LanguageText m_lbl_att_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_att_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lbl1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lbl1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lbl2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lbl2_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_view_GridLayoutGroup;

		[HideInInspector] public RectTransform m_UI_Item_BuffWarLevel;
		[HideInInspector] public LanguageText m_lbl_state_LanguageText;



        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_rect0 = FindUI<RectTransform>(vb.transform ,"pl_rect0");
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_rect0/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect0/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_rect0/sv_list");

			m_pl_rect1 = FindUI<RectTransform>(vb.transform ,"pl_rect1");
			m_pl_view1 = FindUI<RectTransform>(vb.transform ,"pl_rect1/pl_view1");
			m_sv_list1_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_rect1/pl_view1/sv_list1");
			m_sv_list1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/pl_view1/sv_list1");
			m_sv_list1_ListView = FindUI<ListView>(vb.transform ,"pl_rect1/pl_view1/sv_list1");

			m_v_scroll_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/pl_view1/sv_list1/v_scroll_view");
			m_v_scroll_view_Mask = FindUI<Mask>(vb.transform ,"pl_rect1/pl_view1/sv_list1/v_scroll_view");

			m_c_scroll_view = FindUI<RectTransform>(vb.transform ,"pl_rect1/pl_view1/sv_list1/v_scroll_view/c_scroll_view");
			m_pl_view2 = FindUI<RectTransform>(vb.transform ,"pl_rect1/pl_view2");
			m_lbl_warDesc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/pl_view2/lbl_warDesc");

			m_lbl_line_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/pl_view2/lbl_line");

			m_sv_list2_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_rect1/pl_view2/sv_list2");
			m_sv_list2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/pl_view2/sv_list2");
			m_sv_list2_ListView = FindUI<ListView>(vb.transform ,"pl_rect1/pl_view2/sv_list2");

			m_pl_c = FindUI<RectTransform>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c");
			m_UI_Item_CityManagerMes1 = FindUI<RectTransform>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1");
			m_lbl_att_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/lbl_att");
			m_lbl_att_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/lbl_att");

			m_lbl_lbl1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/lbl_lbl1");
			m_lbl_lbl1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/lbl_lbl1");

			m_lbl_lbl2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/lbl_lbl2");
			m_lbl_lbl2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/lbl_lbl2");

			m_pl_view_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/pl_view");

			m_UI_Item_BuffWarLevel = FindUI<RectTransform>(vb.transform ,"pl_rect1/pl_view2/sv_list2/v/pl_c/UI_Item_CityManagerMes1/pl_view/UI_Item_BuffWarLevel");
			m_lbl_state_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/pl_view2/lbl_state");


            UI_Win_CityManagerMediator mt = new UI_Win_CityManagerMediator(vb.gameObject);
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
