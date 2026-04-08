// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月27日
// Update Time         :    2020年8月27日
// Class Description   :    UI_IF_ExpeditionFightTaskView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_ExpeditionFightTaskView : GameView
    {
        public const string VIEW_NAME = "UI_IF_ExpeditionFightTask";

        public UI_IF_ExpeditionFightTaskView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_mask_PolygonImage;

		[HideInInspector] public Animator m_pl_center_Animator;
		[HideInInspector] public CanvasGroup m_pl_center_CanvasGroup;

		[HideInInspector] public CanvasGroup m_pl_flagMes_CanvasGroup;

		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_btn_start1;
		[HideInInspector] public LanguageText m_lbl_tip_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_tip_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_img_captains_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_img_captains_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ExpenditionCaptain_SubView m_UI_Item_ExpenditionCaptain1;
		[HideInInspector] public UI_Item_ExpenditionCaptain_SubView m_UI_Item_ExpenditionCaptain2;
		[HideInInspector] public UI_Item_ExpenditionCaptain_SubView m_UI_Item_ExpenditionCaptain3;
		[HideInInspector] public UI_Item_ExpenditionCaptain_SubView m_UI_Item_ExpenditionCaptain4;
		[HideInInspector] public UI_Item_ExpenditionCaptain_SubView m_UI_Item_ExpenditionCaptain5;
		[HideInInspector] public UI_Item_ExpeditionFightTask_SubView m_UI_Item_ExpeditionFightTask;
		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_side_PolygonImage;
		[HideInInspector] public Animator m_pl_side_Animator;
		[HideInInspector] public CanvasGroup m_pl_side_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_tropNum_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_preview;
		[HideInInspector] public GridLayoutGroup m_pl_layout_GridLayoutGroup;

		[HideInInspector] public UI_Item_ExpeditionFightList_SubView m_UI_Item_ExpeditionFightList1;
		[HideInInspector] public UI_Item_ExpeditionFightList_SubView m_UI_Item_ExpeditionFightList2;
		[HideInInspector] public UI_Item_ExpeditionFightList_SubView m_UI_Item_ExpeditionFightList3;
		[HideInInspector] public UI_Item_ExpeditionFightList_SubView m_UI_Item_ExpeditionFightList4;
		[HideInInspector] public UI_Item_ExpeditionFightList_SubView m_UI_Item_ExpeditionFightList5;
		[HideInInspector] public Animator m_pl_preview_Animator;
		[HideInInspector] public CanvasGroup m_pl_preview_CanvasGroup;

		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_btn_start2;
		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;
		[HideInInspector] public ArabLayoutCompment m_pl_buff_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_buff_GridLayoutGroup;
		[HideInInspector] public RectMask2D m_pl_buff_RectMask2D;

		[HideInInspector] public UI_Item_MainIFBuff_SubView m_UI_Item_MainIFBuff;
		[HideInInspector] public RectTransform m_pl_effect;


        private void UIFinder()
        {
			m_img_mask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_mask");

			m_pl_center_Animator = FindUI<Animator>(vb.transform ,"pl_center");
			m_pl_center_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_center");

			m_pl_flagMes_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_center/pl_flagMes");

			m_btn_start1 = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_center/pl_flagMes/btn_start1"));
			m_lbl_tip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_center/pl_flagMes/lbl_tip");
			m_lbl_tip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_center/pl_flagMes/lbl_tip");

			m_img_captains_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_center/pl_flagMes/img_captains");
			m_img_captains_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_center/pl_flagMes/img_captains");

			m_UI_Item_ExpenditionCaptain1 = new UI_Item_ExpenditionCaptain_SubView(FindUI<RectTransform>(vb.transform ,"pl_center/pl_flagMes/img_captains/UI_Item_ExpenditionCaptain1"));
			m_UI_Item_ExpenditionCaptain2 = new UI_Item_ExpenditionCaptain_SubView(FindUI<RectTransform>(vb.transform ,"pl_center/pl_flagMes/img_captains/UI_Item_ExpenditionCaptain2"));
			m_UI_Item_ExpenditionCaptain3 = new UI_Item_ExpenditionCaptain_SubView(FindUI<RectTransform>(vb.transform ,"pl_center/pl_flagMes/img_captains/UI_Item_ExpenditionCaptain3"));
			m_UI_Item_ExpenditionCaptain4 = new UI_Item_ExpenditionCaptain_SubView(FindUI<RectTransform>(vb.transform ,"pl_center/pl_flagMes/img_captains/UI_Item_ExpenditionCaptain4"));
			m_UI_Item_ExpenditionCaptain5 = new UI_Item_ExpenditionCaptain_SubView(FindUI<RectTransform>(vb.transform ,"pl_center/pl_flagMes/img_captains/UI_Item_ExpenditionCaptain5"));
			m_UI_Item_ExpeditionFightTask = new UI_Item_ExpeditionFightTask_SubView(FindUI<RectTransform>(vb.transform ,"pl_center/pl_flagMes/UI_Item_ExpeditionFightTask"));
			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_center/pl_flagMes/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_center/pl_flagMes/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_center/pl_flagMes/btn_info");

			m_pl_side_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_side");
			m_pl_side_Animator = FindUI<Animator>(vb.transform ,"pl_side");
			m_pl_side_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_side");

			m_lbl_tropNum_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_side/lbl_tropNum");

			m_btn_preview = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_side/btn_preview"));
			m_pl_layout_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_side/pl_layout");

			m_UI_Item_ExpeditionFightList1 = new UI_Item_ExpeditionFightList_SubView(FindUI<RectTransform>(vb.transform ,"pl_side/pl_layout/UI_Item_ExpeditionFightList1"));
			m_UI_Item_ExpeditionFightList2 = new UI_Item_ExpeditionFightList_SubView(FindUI<RectTransform>(vb.transform ,"pl_side/pl_layout/UI_Item_ExpeditionFightList2"));
			m_UI_Item_ExpeditionFightList3 = new UI_Item_ExpeditionFightList_SubView(FindUI<RectTransform>(vb.transform ,"pl_side/pl_layout/UI_Item_ExpeditionFightList3"));
			m_UI_Item_ExpeditionFightList4 = new UI_Item_ExpeditionFightList_SubView(FindUI<RectTransform>(vb.transform ,"pl_side/pl_layout/UI_Item_ExpeditionFightList4"));
			m_UI_Item_ExpeditionFightList5 = new UI_Item_ExpeditionFightList_SubView(FindUI<RectTransform>(vb.transform ,"pl_side/pl_layout/UI_Item_ExpeditionFightList5"));
			m_pl_preview_Animator = FindUI<Animator>(vb.transform ,"pl_preview");
			m_pl_preview_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_preview");

			m_btn_start2 = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_preview/btn_start2"));
			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));
			m_pl_buff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_buff");
			m_pl_buff_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_buff");
			m_pl_buff_RectMask2D = FindUI<RectMask2D>(vb.transform ,"pl_buff");

			m_UI_Item_MainIFBuff = new UI_Item_MainIFBuff_SubView(FindUI<RectTransform>(vb.transform ,"pl_buff/UI_Item_MainIFBuff"));
			m_pl_effect = FindUI<RectTransform>(vb.transform ,"pl_effect");

            UI_IF_ExpeditionFightTaskMediator mt = new UI_IF_ExpeditionFightTaskMediator(vb.gameObject);
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
