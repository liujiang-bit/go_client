// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月29日
// Update Time         :    2020年9月29日
// Class Description   :    UI_Win_GuildTerrtroyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_GuildTerrtroyView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildTerrtroy";

        public UI_Win_GuildTerrtroyView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_get;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_food;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_wood;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_stone;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_gold;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Item_AliGuide_SubView m_UI_Item_AliGuide;
		[HideInInspector] public RectTransform m_pl_guideNode;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/res/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"rect/res/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/res/btn_info");

			m_btn_get = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/res/btn_get"));
			m_UI_food = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/res/resRect/UI_food"));
			m_UI_wood = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/res/resRect/UI_wood"));
			m_UI_stone = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/res/resRect/UI_stone"));
			m_UI_gold = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/res/resRect/UI_gold"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_UI_Item_AliGuide = new UI_Item_AliGuide_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_AliGuide"));
			m_pl_guideNode = FindUI<RectTransform>(vb.transform ,"pl_guideNode");

            UI_Win_GuildTerrtroyMediator mt = new UI_Win_GuildTerrtroyMediator(vb.gameObject);
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
