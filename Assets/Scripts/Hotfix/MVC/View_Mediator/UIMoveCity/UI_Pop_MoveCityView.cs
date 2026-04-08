// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年10月27日
// Update Time         :    2020年10月27日
// Class Description   :    UI_Pop_MoveCityView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Pop_MoveCityView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_MoveCity";

        public UI_Pop_MoveCityView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_buy;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_move;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_cancel;
		[HideInInspector] public PolygonImage m_img_Animator_PolygonImage;
		[HideInInspector] public Animator m_img_Animator_Animator;

		[HideInInspector] public CanvasGroup m_pl_before_CanvasGroup;

		[HideInInspector] public UI_Model_Item_SubView m_UI_ItemBefotr;
		[HideInInspector] public LanguageText m_lbl_itemNameBefore_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemNameBefore_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemDescBefore_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemDescBefore_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemTipBefore_LanguageText;

		[HideInInspector] public CanvasGroup m_pl_after_CanvasGroup;
		[HideInInspector] public UIDefaultValue m_pl_after_UIDefaultValue;

		[HideInInspector] public UI_Model_Item_SubView m_UI_ItemAfter;
		[HideInInspector] public LanguageText m_lbl_itemNameAfter_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemNameAfter_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemDescAfter_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemDescAfter_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemTipAfter_LanguageText;



        private void UIFinder()
        {
			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_btn_buy = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/rect/btns/btn_buy"));
			m_btn_move = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/rect/btns/btn_move"));
			m_btn_cancel = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/rect/btns/btn_cancel"));
			m_img_Animator_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/rect/img_Animator");
			m_img_Animator_Animator = FindUI<Animator>(vb.transform ,"pl_pos/rect/img_Animator");

			m_pl_before_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/rect/img_Animator/pl_before");

			m_UI_ItemBefotr = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/rect/img_Animator/pl_before/UI_ItemBefotr"));
			m_lbl_itemNameBefore_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/rect/img_Animator/pl_before/lbl_itemNameBefore");
			m_lbl_itemNameBefore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/rect/img_Animator/pl_before/lbl_itemNameBefore");

			m_lbl_itemDescBefore_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/rect/img_Animator/pl_before/lbl_itemDescBefore");
			m_lbl_itemDescBefore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/rect/img_Animator/pl_before/lbl_itemDescBefore");

			m_lbl_itemTipBefore_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/rect/img_Animator/pl_before/lbl_itemTipBefore");

			m_pl_after_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/rect/img_Animator/pl_after");
			m_pl_after_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos/rect/img_Animator/pl_after");

			m_UI_ItemAfter = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/rect/img_Animator/pl_after/UI_ItemAfter"));
			m_lbl_itemNameAfter_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/rect/img_Animator/pl_after/lbl_itemNameAfter");
			m_lbl_itemNameAfter_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/rect/img_Animator/pl_after/lbl_itemNameAfter");

			m_lbl_itemDescAfter_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/rect/img_Animator/pl_after/lbl_itemDescAfter");
			m_lbl_itemDescAfter_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/rect/img_Animator/pl_after/lbl_itemDescAfter");

			m_lbl_itemTipAfter_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/rect/img_Animator/pl_after/lbl_itemTipAfter");


            UI_Pop_MoveCityMediator mt = new UI_Pop_MoveCityMediator(vb.gameObject);
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
