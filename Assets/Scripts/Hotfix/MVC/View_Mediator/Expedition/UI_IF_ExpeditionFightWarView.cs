// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月28日
// Update Time         :    2020年8月28日
// Class Description   :    UI_IF_ExpeditionFightWarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_ExpeditionFightWarView : GameView
    {
        public const string VIEW_NAME = "UI_IF_ExpeditionFightWar";

        public UI_IF_ExpeditionFightWarView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_pl_target_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_target_Animator;
		[HideInInspector] public VerticalLayoutGroup m_pl_target_VerticalLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_target_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_target_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_target_bg_ArabLayoutCompment;
		[HideInInspector] public LayoutElement m_img_target_bg_LayoutElement;

		[HideInInspector] public LayoutElement m_pl_view_LayoutElement;

		[HideInInspector] public PolygonImage m_btn_icon_PolygonImage;
		[HideInInspector] public GameButton m_btn_icon_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_atk_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_atk_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_def_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_def_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_arrow_PolygonImage;
		[HideInInspector] public GameButton m_btn_arrow_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_arrow_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_target_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_target_PolygonImage;
		[HideInInspector] public ListView m_sv_target_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_target_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_target_PolygonImage;
		[HideInInspector] public Mask m_v_target_Mask;

		[HideInInspector] public RectTransform m_c_target;
		[HideInInspector] public CanvasGroup m_pl_myself_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_myself_ArabLayoutCompment;
		[HideInInspector] public PolygonImage m_pl_myself_PolygonImage;
		[HideInInspector] public ContentSizeFitter m_pl_myself_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_pl_myself_VerticalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_myself_bg_PolygonImage;
		[HideInInspector] public LayoutElement m_img_myself_bg_LayoutElement;
		[HideInInspector] public ContentSizeFitter m_img_myself_bg_ContentSizeFitter;

		[HideInInspector] public UI_Item_MainIFArm_SubView m_UI_Item_MainIFArm1;
		[HideInInspector] public UI_Item_MainIFArm_SubView m_UI_Item_MainIFArm2;
		[HideInInspector] public UI_Item_MainIFArm_SubView m_UI_Item_MainIFArm3;
		[HideInInspector] public UI_Item_MainIFArm_SubView m_UI_Item_MainIFArm4;
		[HideInInspector] public UI_Item_MainIFArm_SubView m_UI_Item_MainIFArm5;
		[HideInInspector] public RectTransform m_pl_timeAndTurn;
		[HideInInspector] public PolygonImage m_img_timeIcon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public Shadow m_lbl_time_Shadow;

		[HideInInspector] public LanguageText m_lbl_turn_LanguageText;
		[HideInInspector] public Shadow m_lbl_turn_Shadow;

		[HideInInspector] public RectTransform m_pl_hp;
		[HideInInspector] public GameSlider m_pb_Hp_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_Hp_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;
		[HideInInspector] public ArabLayoutCompment m_pl_buff_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_buff_GridLayoutGroup;
		[HideInInspector] public RectMask2D m_pl_buff_RectMask2D;

		[HideInInspector] public UI_Item_MainIFBuff_SubView m_UI_Item_MainIFBuff;


        private void UIFinder()
        {
			m_pl_target_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target");
			m_pl_target_Animator = FindUI<Animator>(vb.transform ,"pl_target");
			m_pl_target_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_target");
			m_pl_target_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_target");

			m_img_target_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_target/img_target_bg");
			m_img_target_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target/img_target_bg");
			m_img_target_bg_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_target/img_target_bg");

			m_pl_view_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_target/pl_view");

			m_btn_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_target/pl_view/btn_icon");
			m_btn_icon_GameButton = FindUI<GameButton>(vb.transform ,"pl_target/pl_view/btn_icon");
			m_btn_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target/pl_view/btn_icon");

			m_img_atk_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_target/pl_view/btn_icon/img_atk");
			m_img_atk_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target/pl_view/btn_icon/img_atk");

			m_img_def_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_target/pl_view/btn_icon/img_def");
			m_img_def_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target/pl_view/btn_icon/img_def");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_target/pl_view/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target/pl_view/lbl_num");

			m_btn_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_target/pl_view/btn_arrow");
			m_btn_arrow_GameButton = FindUI<GameButton>(vb.transform ,"pl_target/pl_view/btn_arrow");
			m_btn_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target/pl_view/btn_arrow");

			m_sv_target_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_target/sv_target");
			m_sv_target_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_target/sv_target");
			m_sv_target_ListView = FindUI<ListView>(vb.transform ,"pl_target/sv_target");
			m_sv_target_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_target/sv_target");

			m_v_target_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_target/sv_target/v_target");
			m_v_target_Mask = FindUI<Mask>(vb.transform ,"pl_target/sv_target/v_target");

			m_c_target = FindUI<RectTransform>(vb.transform ,"pl_target/sv_target/v_target/c_target");
			m_pl_myself_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_myself");
			m_pl_myself_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_myself");
			m_pl_myself_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_myself");
			m_pl_myself_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_myself");
			m_pl_myself_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_myself");

			m_img_myself_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_myself/img_myself_bg");
			m_img_myself_bg_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_myself/img_myself_bg");
			m_img_myself_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_myself/img_myself_bg");

			m_UI_Item_MainIFArm1 = new UI_Item_MainIFArm_SubView(FindUI<RectTransform>(vb.transform ,"pl_myself/UI_Item_MainIFArm1"));
			m_UI_Item_MainIFArm2 = new UI_Item_MainIFArm_SubView(FindUI<RectTransform>(vb.transform ,"pl_myself/UI_Item_MainIFArm2"));
			m_UI_Item_MainIFArm3 = new UI_Item_MainIFArm_SubView(FindUI<RectTransform>(vb.transform ,"pl_myself/UI_Item_MainIFArm3"));
			m_UI_Item_MainIFArm4 = new UI_Item_MainIFArm_SubView(FindUI<RectTransform>(vb.transform ,"pl_myself/UI_Item_MainIFArm4"));
			m_UI_Item_MainIFArm5 = new UI_Item_MainIFArm_SubView(FindUI<RectTransform>(vb.transform ,"pl_myself/UI_Item_MainIFArm5"));
			m_pl_timeAndTurn = FindUI<RectTransform>(vb.transform ,"pl_timeAndTurn");
			m_img_timeIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_timeAndTurn/img_timeIcon");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_timeAndTurn/lbl_time");
			m_lbl_time_Shadow = FindUI<Shadow>(vb.transform ,"pl_timeAndTurn/lbl_time");

			m_lbl_turn_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_timeAndTurn/lbl_turn");
			m_lbl_turn_Shadow = FindUI<Shadow>(vb.transform ,"pl_timeAndTurn/lbl_turn");

			m_pl_hp = FindUI<RectTransform>(vb.transform ,"pl_hp");
			m_pb_Hp_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_hp/pb_Hp");
			m_pb_Hp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_hp/pb_Hp");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_hp/UI_Model_CaptainHead"));
			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));
			m_pl_buff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_buff");
			m_pl_buff_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_buff");
			m_pl_buff_RectMask2D = FindUI<RectMask2D>(vb.transform ,"pl_buff");

			m_UI_Item_MainIFBuff = new UI_Item_MainIFBuff_SubView(FindUI<RectTransform>(vb.transform ,"pl_buff/UI_Item_MainIFBuff"));

            UI_IF_ExpeditionFightWarMediator mt = new UI_IF_ExpeditionFightWarMediator(vb.gameObject);
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
