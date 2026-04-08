// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 16, 2020
// Update Time         :    Thursday, April 16, 2020
// Class Description   :    UI_Win_GuildDepotView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildDepotView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildDepot";

        public UI_Win_GuildDepotView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public GridLayoutGroup m_pl_list_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_list_ArabLayoutCompment;

		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes1;
		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes2;
		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes3;
		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes4;
		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes5;
		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_list_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_list");
			m_pl_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_list");

			m_UI_Item_GuildDepotRes1 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_list/UI_Item_GuildDepotRes1"));
			m_UI_Item_GuildDepotRes2 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_list/UI_Item_GuildDepotRes2"));
			m_UI_Item_GuildDepotRes3 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_list/UI_Item_GuildDepotRes3"));
			m_UI_Item_GuildDepotRes4 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_list/UI_Item_GuildDepotRes4"));
			m_UI_Item_GuildDepotRes5 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_list/UI_Item_GuildDepotRes5"));
			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_info");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");


            UI_Win_GuildDepotMediator mt = new UI_Win_GuildDepotMediator(vb.gameObject);
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
