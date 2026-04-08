// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月22日
// Update Time         :    2020年7月22日
// Class Description   :    UI_Pop_WorldObjectInfoView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_WorldObjectInfoView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_WorldObjectInfo";

        public UI_Pop_WorldObjectInfoView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public Animator m_pl_content_Animator;

		[HideInInspector] public RectTransform m_img_bg;
		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_position_LanguageText;

		[HideInInspector] public UI_Item_WorldObjectPopBtns_SubView m_UI_Item_WorldObjectPopBtns;
		[HideInInspector] public RectTransform m_pl_inSitu;
		[HideInInspector] public GameToggle m_ck_Situ_GameToggle;

		[HideInInspector] public UI_Common_PopFun_SubView m_UI_Common_PopFun;
		[HideInInspector] public CanvasGroup m_pl_description_CanvasGroup;
		[HideInInspector] public Animator m_pl_description_Animator;

		[HideInInspector] public PolygonImage m_btn_descBack_PolygonImage;
		[HideInInspector] public GameButton m_btn_descBack_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descBack_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_descBack_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;

		[HideInInspector] public Animator m_pl_normalInfo_Animator;
		[HideInInspector] public CanvasGroup m_pl_normalInfo_CanvasGroup;

		[HideInInspector] public UI_Item_WorldObjInfoTCollect_SubView m_UI_Item_WorldObjInfoTCollect;
		[HideInInspector] public UI_Item_WorldObjInfoTGuildBuilding_SubView m_UI_Item_WorldObjInfoTGuildBuilding;
		[HideInInspector] public UI_Item_WorldObjInfoTNeutral_SubView m_UI_Item_WorldObjInfoTNeutral;
		[HideInInspector] public PolygonImage m_btn_descinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_descinfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descinfo_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_descinfo_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_recommend_LanguageText;

		[HideInInspector] public UI_Tag_PopAnime_SkillTip_SubView m_UI_Tag_PopAnime_SkillTip;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_content_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content");

			m_img_bg = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg");
			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideL");

			m_lbl_position_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/lbl_position");

			m_UI_Item_WorldObjectPopBtns = new UI_Item_WorldObjectPopBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Item_WorldObjectPopBtns"));
			m_pl_inSitu = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/pl_inSitu");
			m_ck_Situ_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/pl_inSitu/LabelSitu/ck_Situ");

			m_UI_Common_PopFun = new UI_Common_PopFun_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/UI_Common_PopFun"));
			m_pl_description_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description");
			m_pl_description_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description");

			m_btn_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/btn_descBack");
			m_btn_descBack_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/btn_descBack");
			m_btn_descBack_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/btn_descBack");

			m_img_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/btn_descBack/img_descBack");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc");

			m_pl_normalInfo_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo");
			m_pl_normalInfo_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo");

			m_UI_Item_WorldObjInfoTCollect = new UI_Item_WorldObjInfoTCollect_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/SpecInfo/UI_Item_WorldObjInfoTCollect"));
			m_UI_Item_WorldObjInfoTGuildBuilding = new UI_Item_WorldObjInfoTGuildBuilding_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/SpecInfo/UI_Item_WorldObjInfoTGuildBuilding"));
			m_UI_Item_WorldObjInfoTNeutral = new UI_Item_WorldObjInfoTNeutral_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/SpecInfo/UI_Item_WorldObjInfoTNeutral"));
			m_btn_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/btn_descinfo");
			m_btn_descinfo_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/btn_descinfo");
			m_btn_descinfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/btn_descinfo");

			m_img_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/btn_descinfo/img_descinfo");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/lbl_name");

			m_lbl_recommend_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/lbl_recommend");

			m_UI_Tag_PopAnime_SkillTip = new UI_Tag_PopAnime_SkillTip_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_PopAnime_SkillTip"));

            UI_Pop_WorldObjectInfoMediator mt = new UI_Pop_WorldObjectInfoMediator(vb.gameObject);
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
