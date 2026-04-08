// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    QuickUseResourcesView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class QuickUseResourcesView : GameView
    {
        public const string VIEW_NAME = "UI_Win_QuickUseResources";

        public QuickUseResourcesView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public VerticalLayoutGroup m_pl_pageBtn_VerticalLayoutGroup;

		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_food;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_wood;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_stone;
		[HideInInspector] public UI_Model_PageButton_Side_SubView m_UI_Model_PageButton_gold;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;



        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_pl_pageBtn_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"rect/pl_pageBtn");

			m_UI_Model_PageButton_food = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_pageBtn/UI_Model_PageButton_food"));
			m_UI_Model_PageButton_wood = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_pageBtn/UI_Model_PageButton_wood"));
			m_UI_Model_PageButton_stone = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_pageBtn/UI_Model_PageButton_stone"));
			m_UI_Model_PageButton_gold = new UI_Model_PageButton_Side_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_pageBtn/UI_Model_PageButton_gold"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/content/sv_list_view");


            QuickUseResourcesMediator mt = new QuickUseResourcesMediator(vb.gameObject);
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
