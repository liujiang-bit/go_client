// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月27日
// Update Time         :    2020年8月27日
// Class Description   :    UI_IF_ChargeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_ChargeView : GameView
    {
        public const string VIEW_NAME = "UI_IF_Charge";

        public UI_IF_ChargeView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;
		[HideInInspector] public UI_Model_Resources_SubView m_img_gem;
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public UI_Item_ChargeFirst_SubView m_UI_Item_ChargeFirst;
		[HideInInspector] public UI_Item_ChargeRiseRoad_SubView m_UI_Item_ChargeRiseRoad;
		[HideInInspector] public UI_Item_ChargeSuperGift_SubView m_UI_Item_ChargeSuperGift;
		[HideInInspector] public UI_Item_ChargeDayCheap_SubView m_UI_Item_ChargeDayCheap;
		[HideInInspector] public UI_Item_ChargeCitySupply_SubView m_UI_Item_ChargeCitySupply;
		[HideInInspector] public UI_Item_ChargeGrowing_SubView m_UI_Item_ChargeGrowing;
		[HideInInspector] public UI_Item_ChargeGemShop_SubView m_UI_Item_ChargeGemShop;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public ToggleGroup m_c_list_view_SideBtns_ToggleGroup;

		[HideInInspector] public PolygonImage m_btn_service_PolygonImage;
		[HideInInspector] public GameButton m_btn_service_GameButton;



        private void UIFinder()
        {
			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));
			m_img_gem = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"img_gem"));
			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_content");
			m_UI_Item_ChargeFirst = new UI_Item_ChargeFirst_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Item_ChargeFirst"));
			m_UI_Item_ChargeRiseRoad = new UI_Item_ChargeRiseRoad_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Item_ChargeRiseRoad"));
			m_UI_Item_ChargeSuperGift = new UI_Item_ChargeSuperGift_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Item_ChargeSuperGift"));
			m_UI_Item_ChargeDayCheap = new UI_Item_ChargeDayCheap_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Item_ChargeDayCheap"));
			m_UI_Item_ChargeCitySupply = new UI_Item_ChargeCitySupply_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Item_ChargeCitySupply"));
			m_UI_Item_ChargeGrowing = new UI_Item_ChargeGrowing_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Item_ChargeGrowing"));
			m_UI_Item_ChargeGemShop = new UI_Item_ChargeGemShop_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Item_ChargeGemShop"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"list/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"list/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"list/sv_list");

			m_c_list_view_SideBtns_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"list/sv_list/v/c_list_view_SideBtns");

			m_btn_service_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_service");
			m_btn_service_GameButton = FindUI<GameButton>(vb.transform ,"btn_service");


            UI_IF_ChargeMediator mt = new UI_IF_ChargeMediator(vb.gameObject);
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
