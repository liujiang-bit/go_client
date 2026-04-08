// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月8日
// Update Time         :    2020年6月8日
// Class Description   :    UI_IF_BattleMainView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_BattleMainView : GameView
    {
        public const string VIEW_NAME = "UI_IF_BattleMain";

        public UI_IF_BattleMainView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_UI_Item_BattleMainList_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_BattleMainList_ArabLayoutCompment;

		[HideInInspector] public UI_Item_BattleMainMenu_SubView m_UI_Item_BattleMainMenu1;
		[HideInInspector] public UI_Item_BattleMainMenu_SubView m_UI_Item_BattleMainMenu2;
		[HideInInspector] public UI_Item_BattleMainMenu_SubView m_UI_Item_BattleMainMenu3;
		[HideInInspector] public UI_Item_BattleMainMenu_SubView m_UI_Item_BattleMainMenu4;
		[HideInInspector] public PolygonImage m_btn_left_PolygonImage;
		[HideInInspector] public GameButton m_btn_left_GameButton;

		[HideInInspector] public PolygonImage m_img_redpoint_left_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_right_PolygonImage;
		[HideInInspector] public GameButton m_btn_right_GameButton;

		[HideInInspector] public PolygonImage m_img_redpoint_right_PolygonImage;



        private void UIFinder()
        {
			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"sv_list");
			m_sv_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"sv_list");

			m_UI_Item_BattleMainList_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"sv_list/v/c/UI_Item_BattleMainList");
			m_UI_Item_BattleMainList_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"sv_list/v/c/UI_Item_BattleMainList");

			m_UI_Item_BattleMainMenu1 = new UI_Item_BattleMainMenu_SubView(FindUI<RectTransform>(vb.transform ,"sv_list/v/c/UI_Item_BattleMainList/UI_Item_BattleMainMenu1"));
			m_UI_Item_BattleMainMenu2 = new UI_Item_BattleMainMenu_SubView(FindUI<RectTransform>(vb.transform ,"sv_list/v/c/UI_Item_BattleMainList/UI_Item_BattleMainMenu2"));
			m_UI_Item_BattleMainMenu3 = new UI_Item_BattleMainMenu_SubView(FindUI<RectTransform>(vb.transform ,"sv_list/v/c/UI_Item_BattleMainList/UI_Item_BattleMainMenu3"));
			m_UI_Item_BattleMainMenu4 = new UI_Item_BattleMainMenu_SubView(FindUI<RectTransform>(vb.transform ,"sv_list/v/c/UI_Item_BattleMainList/UI_Item_BattleMainMenu4"));
			m_btn_left_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_left");
			m_btn_left_GameButton = FindUI<GameButton>(vb.transform ,"btn_left");

			m_img_redpoint_left_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_left/img_redpoint_left");

			m_btn_right_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_right");
			m_btn_right_GameButton = FindUI<GameButton>(vb.transform ,"btn_right");

			m_img_redpoint_right_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_right/img_redpoint_right");


            UI_IF_BattleMainMediator mt = new UI_IF_BattleMainMediator(vb.gameObject);
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
