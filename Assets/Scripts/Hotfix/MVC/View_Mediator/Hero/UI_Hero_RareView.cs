// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月27日
// Update Time         :    2019年12月27日
// Class Description   :    UI_Hero_RareView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Hero_RareView : GameView
    {
        public const string VIEW_NAME = "UI_Hero_Rare";

        public UI_Hero_RareView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public SkeletonGraphic m_spine_hero_SkeletonGraphic;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public RectTransform m_pl_right;
		[HideInInspector] public HorizontalLayoutGroup m_pl_star_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_head_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public PolygonImage m_img_civilization_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_rare_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public PolygonImage m_img_talent_1_PolygonImage;

		[HideInInspector] public HrefText m_lbl_talent_1_LinkImageText;

		[HideInInspector] public PolygonImage m_img_talent_2_PolygonImage;

		[HideInInspector] public HrefText m_lbl_talent_2_LinkImageText;

		[HideInInspector] public PolygonImage m_img_talent_3_PolygonImage;

		[HideInInspector] public HrefText m_lbl_talent_3_LinkImageText;



        private void UIFinder()
        {
			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_spine_hero_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"pl_mes/spine_hero");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/btn_close");

			m_pl_right = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_right");
			m_pl_star_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"pl_mes/pl_right/pl_star");

			m_img_head_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/img_head");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_title");

			m_img_civilization_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/img_civilization");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_name");

			m_lbl_rare_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_rare");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_right/lbl_desc");

			m_img_talent_1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/pltalent/img_talent_1");

			m_lbl_talent_1_LinkImageText = FindUI<HrefText>(vb.transform ,"pl_mes/pl_right/pltalent/img_talent_1/lbl_talent_1");

			m_img_talent_2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/pltalent/img_talent_2");

			m_lbl_talent_2_LinkImageText = FindUI<HrefText>(vb.transform ,"pl_mes/pl_right/pltalent/img_talent_2/lbl_talent_2");

			m_img_talent_3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/pl_right/pltalent/img_talent_3");

			m_lbl_talent_3_LinkImageText = FindUI<HrefText>(vb.transform ,"pl_mes/pl_right/pltalent/img_talent_3/lbl_talent_3");


            UI_Hero_RareMediator mt = new UI_Hero_RareMediator(vb.gameObject);
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
