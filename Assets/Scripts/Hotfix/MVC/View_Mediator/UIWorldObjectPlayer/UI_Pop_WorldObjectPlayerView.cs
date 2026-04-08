// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月9日
// Update Time         :    2020年9月9日
// Class Description   :    UI_Pop_WorldObjectPlayerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_WorldObjectPlayerView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_WorldObjectPlayer";

        public UI_Pop_WorldObjectPlayerView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public Animator m_pl_content_Animator;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_position_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_position_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_UI_Item_WorldObjectPopBtns;
		[HideInInspector] public RectTransform m_pl_1g;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_Model_1gbtn1;
		[HideInInspector] public GridLayoutGroup m_pl_2g_GridLayoutGroup;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_2gbtn1;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_2gbtn2;
		[HideInInspector] public GridLayoutGroup m_pl_3g_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_3g_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_UI_Model_3gbtn3;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_UI_Model_3gbtn2;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_3gbtn1;
		[HideInInspector] public RectTransform m_pl_inSitu;
		[HideInInspector] public GameToggle m_ck_Situ_GameToggle;

		[HideInInspector] public UI_Common_PopFun_SubView m_UI_Common_PopFun;
		[HideInInspector] public PolygonImage m_btn_descBack_PolygonImage;
		[HideInInspector] public GameButton m_btn_descBack_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descBack_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment;

		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line1;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line2;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line3;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_head_PolygonImage;
		[HideInInspector] public GameButton m_btn_head_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_head_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_descinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_descinfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descinfo_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_recommend_LanguageText;

		[HideInInspector] public UI_Tag_PopAnime_SkillTip_SubView m_UI_Tag_PopAnime_SkillTip;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_content_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideL");

			m_lbl_position_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/lbl_position");
			m_lbl_position_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/lbl_position");

			m_UI_Item_WorldObjectPopBtns = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns");
			m_pl_1g = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_1g");
			m_UI_Model_1gbtn1 = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_1g/UI_Model_1gbtn1"));
			m_pl_2g_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_2g");

			m_UI_Model_2gbtn1 = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_2g/UI_Model_2gbtn1"));
			m_UI_Model_2gbtn2 = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_2g/UI_Model_2gbtn2"));
			m_pl_3g_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_3g");
			m_pl_3g_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_3g");

			m_UI_Model_3gbtn3 = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_3g/UI_Model_3gbtn3"));
			m_UI_Model_3gbtn2 = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_3g/UI_Model_3gbtn2"));
			m_UI_Model_3gbtn1 = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns/pl_3g/UI_Model_3gbtn1"));
			m_pl_inSitu = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/pl_inSitu");
			m_ck_Situ_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/pl_inSitu/LabelSitu/ck_Situ");

			m_UI_Common_PopFun = new UI_Common_PopFun_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/UI_Common_PopFun"));
			m_btn_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/btn_descBack");
			m_btn_descBack_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/btn_descBack");
			m_btn_descBack_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/btn_descBack");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/sv_desc");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/description/sv_desc/v/c/lbl_desc");

			m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer");

			m_UI_Item_line1 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/textLayer/UI_Item_line1"));
			m_UI_Item_line2 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/textLayer/UI_Item_line2"));
			m_UI_Item_line3 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/textLayer/UI_Item_line3"));
			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/UI_Model_PlayerHead"));
			m_btn_head_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/btn_head");
			m_btn_head_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/btn_head");
			m_btn_head_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/SpecInfo/UI_Item_WorldObjInfoTPlayer/btn_head");

			m_btn_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/btn_descinfo");
			m_btn_descinfo_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/btn_descinfo");
			m_btn_descinfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/btn_descinfo");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/lbl_name");

			m_lbl_recommend_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/normalInfo/lbl_recommend");

			m_UI_Tag_PopAnime_SkillTip = new UI_Tag_PopAnime_SkillTip_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/UI_Tag_PopAnime_SkillTip"));

            UI_Pop_WorldObjectPlayerMediator mt = new UI_Pop_WorldObjectPlayerMediator(vb.gameObject);
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
