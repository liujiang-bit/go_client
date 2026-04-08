// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, 03 November 2020
// Update Time         :    Tuesday, 03 November 2020
// Class Description   :    CaptainView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class CaptainView : GameView
    {
        public const string VIEW_NAME = "UI_IF_Captain";

        public CaptainView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg2_PolygonImage;

		[HideInInspector] public Image m_UI_3D_Scene_Image;

		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;
		[HideInInspector] public PolygonImage m_btn_char_PolygonImage;
		[HideInInspector] public GameButton m_btn_char_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_char_ArabLayoutCompment;

		[HideInInspector] public Image m_pl_talkPos_Image;
		[HideInInspector] public ArabLayoutCompment m_pl_talkPos_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_power_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_powerQ_PolygonImage;
		[HideInInspector] public GameButton m_btn_powerQ_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_powerQ_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_list_set_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_UI_Item_CaptainList_ArabLayoutCompment;
		[HideInInspector] public CanvasGroup m_UI_Item_CaptainList_CanvasGroup;
		[HideInInspector] public Animator m_UI_Item_CaptainList_Animator;
		[HideInInspector] public UIDefaultValue m_UI_Item_CaptainList_UIDefaultValue;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_arr_PolygonImage;
		[HideInInspector] public GameButton m_btn_arr_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_arr_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_arrtext_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_arrtext_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_UI_Pop_arrType_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_Quality;
		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_star;
		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_level;
		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_power;
		[HideInInspector] public ScrollRect m_sv_captainHead_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_captainHead_PolygonImage;
		[HideInInspector] public ListView m_sv_captainHead_ListView;

		[HideInInspector] public UI_Item_CaptainData_SubView m_UI_Item_CaptainData;
		[HideInInspector] public UI_Item_CaptainStroy_SubView m_UI_Item_CaptainStroy;
		[HideInInspector] public Animator m_char_anim_Animator;
		[HideInInspector] public UIFlowPos m_char_anim_UIFlowPos;

		[HideInInspector] public PolygonImage m_btn_changeEquip_PolygonImage;
		[HideInInspector] public GameButton m_btn_changeEquip_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_changeEquip_ArabLayoutCompment;
		[HideInInspector] public Animator m_btn_changeEquip_Animator;
		[HideInInspector] public CanvasGroup m_btn_changeEquip_CanvasGroup;
		[HideInInspector] public UIDefaultValue m_btn_changeEquip_UIDefaultValue;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;
		[HideInInspector] public UI_Item_CaptainEquip_SubView m_UI_Item_CaptainEquip;
		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public UI_Item_CaptainStarUp_SubView m_UI_Item_CaptainStarUp;
		[HideInInspector] public UI_Item_CaptainSkillUp_SubView m_UI_Item_CaptainSkillUp;
		[HideInInspector] public UI_Item_CaptainTalent_SubView m_UI_Item_CaptainTalent;


        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_bg2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_bg2");

			m_UI_3D_Scene_Image = FindUI<Image>(vb.transform ,"UI_3D_Scene");

			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));
			m_btn_char_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_char");
			m_btn_char_GameButton = FindUI<GameButton>(vb.transform ,"btn_char");
			m_btn_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_char");

			m_pl_talkPos_Image = FindUI<Image>(vb.transform ,"btn_char/pl_talkPos");
			m_pl_talkPos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_char/pl_talkPos");

			m_pl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_power");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_power/bg/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_power/bg/lbl_power");

			m_btn_powerQ_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_power/btn_powerQ");
			m_btn_powerQ_GameButton = FindUI<GameButton>(vb.transform ,"pl_power/btn_powerQ");
			m_btn_powerQ_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_power/btn_powerQ");

			m_pl_list_set_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list_set");

			m_UI_Item_CaptainList_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list_set/UI_Item_CaptainList");
			m_UI_Item_CaptainList_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_list_set/UI_Item_CaptainList");
			m_UI_Item_CaptainList_Animator = FindUI<Animator>(vb.transform ,"pl_list_set/UI_Item_CaptainList");
			m_UI_Item_CaptainList_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_list_set/UI_Item_CaptainList");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_list_set/UI_Item_CaptainList/title/lbl_count");
			m_lbl_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list_set/UI_Item_CaptainList/title/lbl_count");

			m_btn_arr_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_list_set/UI_Item_CaptainList/title/btn_arr");
			m_btn_arr_GameButton = FindUI<GameButton>(vb.transform ,"pl_list_set/UI_Item_CaptainList/title/btn_arr");
			m_btn_arr_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list_set/UI_Item_CaptainList/title/btn_arr");

			m_lbl_arrtext_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_list_set/UI_Item_CaptainList/title/btn_arr/lbl_arrtext");
			m_lbl_arrtext_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list_set/UI_Item_CaptainList/title/btn_arr/lbl_arrtext");

			m_UI_Pop_arrType_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list_set/UI_Item_CaptainList/UI_Pop_arrType");

			m_UI_Item_CaptionArrType_Quality = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(vb.transform ,"pl_list_set/UI_Item_CaptainList/UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_Quality"));
			m_UI_Item_CaptionArrType_star = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(vb.transform ,"pl_list_set/UI_Item_CaptainList/UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_star"));
			m_UI_Item_CaptionArrType_level = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(vb.transform ,"pl_list_set/UI_Item_CaptainList/UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_level"));
			m_UI_Item_CaptionArrType_power = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(vb.transform ,"pl_list_set/UI_Item_CaptainList/UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_power"));
			m_sv_captainHead_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_list_set/UI_Item_CaptainList/sv_captainHead");
			m_sv_captainHead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_list_set/UI_Item_CaptainList/sv_captainHead");
			m_sv_captainHead_ListView = FindUI<ListView>(vb.transform ,"pl_list_set/UI_Item_CaptainList/sv_captainHead");

			m_UI_Item_CaptainData = new UI_Item_CaptainData_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_CaptainData"));
			m_UI_Item_CaptainStroy = new UI_Item_CaptainStroy_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_CaptainStroy"));
			m_char_anim_Animator = FindUI<Animator>(vb.transform ,"Panel/char_anim");
			m_char_anim_UIFlowPos = FindUI<UIFlowPos>(vb.transform ,"Panel/char_anim");

			m_btn_changeEquip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_changeEquip");
			m_btn_changeEquip_GameButton = FindUI<GameButton>(vb.transform ,"btn_changeEquip");
			m_btn_changeEquip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_changeEquip");
			m_btn_changeEquip_Animator = FindUI<Animator>(vb.transform ,"btn_changeEquip");
			m_btn_changeEquip_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"btn_changeEquip");
			m_btn_changeEquip_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"btn_changeEquip");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"btn_changeEquip/UI_Common_Redpoint"));
			m_UI_Item_CaptainEquip = new UI_Item_CaptainEquip_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_CaptainEquip"));
			m_pl_view = FindUI<RectTransform>(vb.transform ,"pl_view");
			m_UI_Item_CaptainStarUp = new UI_Item_CaptainStarUp_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/UI_Item_CaptainStarUp"));
			m_UI_Item_CaptainSkillUp = new UI_Item_CaptainSkillUp_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/UI_Item_CaptainSkillUp"));
			m_UI_Item_CaptainTalent = new UI_Item_CaptainTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/UI_Item_CaptainTalent"));

            CaptainMediator mt = new CaptainMediator(vb.gameObject);
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
