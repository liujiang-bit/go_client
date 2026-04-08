// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月17日
// Update Time         :    2020年9月17日
// Class Description   :    UI_Pop_WorldObjectVillageCaveFinishView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Pop_WorldObjectVillageCaveFinishView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_WorldObjectVillageCaveFinish";

        public UI_Pop_WorldObjectVillageCaveFinishView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_position_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_position_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_UI_Model_StandardButton_Yellow;
		[HideInInspector] public UI_Common_PopFun_SubView m_UI_Common_PopFun;
		[HideInInspector] public Animator m_pl_description_Animator;
		[HideInInspector] public CanvasGroup m_pl_description_CanvasGroup;

		[HideInInspector] public PolygonImage m_btn_descBack_PolygonImage;
		[HideInInspector] public GameButton m_btn_descBack_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descBack_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;

		[HideInInspector] public CanvasGroup m_pl_normalInfo_CanvasGroup;
		[HideInInspector] public Animator m_pl_normalInfo_Animator;

		[HideInInspector] public RectTransform m_pl_villageReward;
		[HideInInspector] public LanguageText m_lbl_rewardName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rewardName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_rewardCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rewardCount_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ItemSize65_SubView m_UI_Item_ItemSize65;
		[HideInInspector] public RectTransform m_pl_caveReward;
		[HideInInspector] public LanguageText m_lbl_cave_rewardName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_cave_rewardName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_cave_rewardCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_cave_rewardCount_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_descin_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_descin_PolygonImage;
		[HideInInspector] public ListView m_sv_descin_ListView;

		[HideInInspector] public LanguageText m_lbl_normalInfo_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_normalInfo_desc_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_normalInfo_desc_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_descinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_descinfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descinfo_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Tag_PopAnime_SkillTip_SubView m_UI_Tag_PopAnime_SkillTip;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/img_arrowSideL");

			m_lbl_position_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/lbl_position");
			m_lbl_position_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/lbl_position");

			m_UI_Model_StandardButton_Yellow = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/btns/UI_Model_StandardButton_Yellow"));
			m_UI_Common_PopFun = new UI_Common_PopFun_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/standard/UI_Common_PopFun"));
			m_pl_description_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description");
			m_pl_description_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description");

			m_btn_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/btn_descBack");
			m_btn_descBack_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/btn_descBack");
			m_btn_descBack_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/btn_descBack");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc");

			m_pl_normalInfo_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo");
			m_pl_normalInfo_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo");

			m_pl_villageReward = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_villageReward");
			m_lbl_rewardName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_villageReward/lbl_rewardName");
			m_lbl_rewardName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_villageReward/lbl_rewardName");

			m_lbl_rewardCount_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_villageReward/lbl_rewardCount");
			m_lbl_rewardCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_villageReward/lbl_rewardCount");

			m_UI_Item_ItemSize65 = new UI_Item_ItemSize65_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_villageReward/UI_Item_ItemSize65"));
			m_pl_caveReward = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_caveReward");
			m_lbl_cave_rewardName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_caveReward/lbl_cave_rewardName");
			m_lbl_cave_rewardName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_caveReward/lbl_cave_rewardName");

			m_lbl_cave_rewardCount_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_caveReward/lbl_cave_rewardCount");
			m_lbl_cave_rewardCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/pl_caveReward/lbl_cave_rewardCount");

			m_sv_descin_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/sv_descin");
			m_sv_descin_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/sv_descin");
			m_sv_descin_ListView = FindUI<ListView>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/sv_descin");

			m_lbl_normalInfo_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/sv_descin/v/c/lbl_normalInfo_desc");
			m_lbl_normalInfo_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/sv_descin/v/c/lbl_normalInfo_desc");
			m_lbl_normalInfo_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/sv_descin/v/c/lbl_normalInfo_desc");

			m_btn_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/btn_descinfo");
			m_btn_descinfo_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/btn_descinfo");
			m_btn_descinfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/btn_descinfo");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/img_bg/rect/pl_normalInfo/lbl_name");

			m_UI_Tag_PopAnime_SkillTip = new UI_Tag_PopAnime_SkillTip_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/UI_Tag_PopAnime_SkillTip"));

            UI_Pop_WorldObjectVillageCaveFinishMediator mt = new UI_Pop_WorldObjectVillageCaveFinishMediator(vb.gameObject);
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
