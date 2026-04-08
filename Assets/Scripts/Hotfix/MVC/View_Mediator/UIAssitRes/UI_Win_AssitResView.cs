// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Win_AssitResView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_AssitResView : GameView
    {
        public const string VIEW_NAME = "UI_Win_AssitRes";

        public UI_Win_AssitResView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public RectTransform m_pl_headEffect;
		[HideInInspector] public LanguageText m_lbl_tax_LanguageText;

		[HideInInspector] public LanguageText m_lbl_taxRate_LanguageText;

		[HideInInspector] public LanguageText m_lbl_capatiy_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_capatiy_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue_long_SubView m_UI_transport;
		[HideInInspector] public GridLayoutGroup m_pl_res_GridLayoutGroup;

		[HideInInspector] public UI_Item_AssitResItem_SubView m_UI_Item_AssitResItem1;
		[HideInInspector] public UI_Item_AssitResItem_SubView m_UI_Item_AssitResItem2;
		[HideInInspector] public UI_Item_AssitResItem_SubView m_UI_Item_AssitResItem3;
		[HideInInspector] public UI_Item_AssitResItem_SubView m_UI_Item_AssitResItem4;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/right/UI_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/lbl_name");

			m_pl_headEffect = FindUI<RectTransform>(vb.transform ,"rect/right/pl_headEffect");
			m_lbl_tax_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/lbl_tax");

			m_lbl_taxRate_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/lbl_taxRate");

			m_lbl_capatiy_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/right/lbl_capatiy");
			m_lbl_capatiy_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/right/lbl_capatiy");

			m_UI_transport = new UI_Model_DoubleLineButton_Blue_long_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/UI_transport"));
			m_pl_res_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/left/pl_res");

			m_UI_Item_AssitResItem1 = new UI_Item_AssitResItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/pl_res/UI_Item_AssitResItem1"));
			m_UI_Item_AssitResItem2 = new UI_Item_AssitResItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/pl_res/UI_Item_AssitResItem2"));
			m_UI_Item_AssitResItem3 = new UI_Item_AssitResItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/pl_res/UI_Item_AssitResItem3"));
			m_UI_Item_AssitResItem4 = new UI_Item_AssitResItem_SubView(FindUI<RectTransform>(vb.transform ,"rect/left/pl_res/UI_Item_AssitResItem4"));

            UI_Win_AssitResMediator mt = new UI_Win_AssitResMediator(vb.gameObject);
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
