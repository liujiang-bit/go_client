// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月17日
// Update Time         :    2020年6月17日
// Class Description   :    UI_Pop_GuildBuildView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_GuildBuildView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_GuildBuild";

        public UI_Pop_GuildBuildView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public Animator m_img_bg_Animator;
		[HideInInspector] public UIDefaultValue m_img_bg_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public PolygonImage m_btn_mes_PolygonImage;
		[HideInInspector] public GameButton m_btn_mes_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_mes_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_mes_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_mes_PolygonImage;
		[HideInInspector] public ListView m_sv_mes_ListView;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_languageText_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_res_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_res_ArabLayoutCompment;

		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res1;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res2;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res3;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res4;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res5;
		[HideInInspector] public RectTransform m_pl_description;
		[HideInInspector] public PolygonImage m_btn_descBack_PolygonImage;
		[HideInInspector] public GameButton m_btn_descBack_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descBack_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;

		[HideInInspector] public LanguageText m_lbl_desc2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc2_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_desc2_ContentSizeFitter;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_build;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");
			m_img_bg_Animator = FindUI<Animator>(vb.transform ,"pl_pos/img_bg");
			m_img_bg_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_mes");
			m_btn_mes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/btn_mes");
			m_btn_mes_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/btn_mes");
			m_btn_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/btn_mes");

			m_sv_mes_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/sv_mes");
			m_sv_mes_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/sv_mes");
			m_sv_mes_ListView = FindUI<ListView>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/sv_mes");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/sv_mes/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/sv_mes/v/c/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/sv_mes/v/c/lbl_desc");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/lbl_languageText");
			m_lbl_languageText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/lbl_languageText");

			m_pl_res_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/pl_res");
			m_pl_res_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/pl_res");

			m_pl_res1 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/pl_res/pl_res1"));
			m_pl_res2 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/pl_res/pl_res2"));
			m_pl_res3 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/pl_res/pl_res3"));
			m_pl_res4 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/pl_res/pl_res4"));
			m_pl_res5 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_mes/pl_res/pl_res5"));
			m_pl_description = FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_description");
			m_btn_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/pl_description/btn_descBack");
			m_btn_descBack_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/img_bg/rect/pl_description/btn_descBack");
			m_btn_descBack_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/pl_description/btn_descBack");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/img_bg/rect/pl_description/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/pl_description/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(vb.transform ,"pl_pos/img_bg/rect/pl_description/sv_desc");

			m_lbl_desc2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc2");
			m_lbl_desc2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc2");
			m_lbl_desc2_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/img_bg/rect/pl_description/sv_desc/v/c/lbl_desc2");

			m_btn_build = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/btn_build"));

            UI_Pop_GuildBuildMediator mt = new UI_Pop_GuildBuildMediator(vb.gameObject);
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
