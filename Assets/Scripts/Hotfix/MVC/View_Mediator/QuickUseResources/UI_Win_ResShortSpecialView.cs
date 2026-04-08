// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月24日
// Update Time         :    2020年7月24日
// Class Description   :    UI_Win_ResShortSpecialView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ResShortSpecialView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ResShortSpecial";

        public UI_Win_ResShortSpecialView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public LanguageText m_lbl_Tip_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_Tip_ArabLayoutCompment;

		[HideInInspector] public HorizontalLayoutGroup m_pl_mes_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_pl_res_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_pl_res_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_res_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Gold;
		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Wood;
		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Stone;
		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Food;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_Model_btn;
		[HideInInspector] public PolygonImage m_pl_item_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_pl_item_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_item_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_item_count_LanguageText;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_bug;
		[HideInInspector] public LanguageText m_lbl_item_Stockedup_LanguageText;



        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_lbl_Tip_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_Tip");
			m_lbl_Tip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_Tip");

			m_pl_mes_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"rect/pl_mes");

			m_pl_res_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_mes/pl_res");
			m_pl_res_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_mes/pl_res");
			m_pl_res_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes/pl_res");

			m_UI_Item_Gold = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_res/UI_Item_Gold"));
			m_UI_Item_Wood = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_res/UI_Item_Wood"));
			m_UI_Item_Stone = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_res/UI_Item_Stone"));
			m_UI_Item_Food = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_res/UI_Item_Food"));
			m_UI_Model_btn = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_res/UI_Model_btn"));
			m_pl_item_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_mes/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_mes/pl_item");

			m_lbl_item_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_mes/pl_item/lbl_item_name");

			m_lbl_item_count_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_mes/pl_item/lbl_item_count");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_item/UI_Model_Item"));
			m_btn_bug = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_mes/pl_item/btn_bug"));
			m_lbl_item_Stockedup_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_mes/pl_item/lbl_item_Stockedup");


            UI_Win_ResShortSpecialMediator mt = new UI_Win_ResShortSpecialMediator(vb.gameObject);
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
