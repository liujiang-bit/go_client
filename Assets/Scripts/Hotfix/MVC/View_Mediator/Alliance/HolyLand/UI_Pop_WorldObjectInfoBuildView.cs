// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月22日
// Update Time         :    2020年7月22日
// Class Description   :    UI_Pop_WorldObjectInfoBuildView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_WorldObjectInfoBuildView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_WorldObjectInfoBuild";

        public UI_Pop_WorldObjectInfoBuildView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public Animator m_pl_content_Animator;

		[HideInInspector] public PolygonImage m_content_bg_PolygonImage;

		[HideInInspector] public RectTransform m_img_bg;
		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_btns_1_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_btns_1_GridLayoutGroup;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_reinforce;
		[HideInInspector] public GridLayoutGroup m_btns_2_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_btns_2_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_look;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_btn_gather;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_btn_attack;
		[HideInInspector] public UI_Common_PopFun_SubView m_UI_Common_PopFun;
		[HideInInspector] public PolygonImage m_btn_descinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_descinfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descinfo_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_descinfo_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_position_LanguageText;

		[HideInInspector] public RectTransform m_pl_up;
		[HideInInspector] public PolygonImage m_btn_more_PolygonImage;
		[HideInInspector] public GameButton m_btn_more_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_more_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_content_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_type1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_type2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_icon_buff2_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_icon_buff2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_buffatt2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffatt2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_icon_buff1_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_icon_buff1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_buffatt1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffatt1_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_type3_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title3_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title3_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kingName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingName_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_kingset_PolygonImage;
		[HideInInspector] public GameButton m_btn_kingset_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_kingset_ArabLayoutCompment;

		[HideInInspector] public UI_Item_IconAndTime_SubView m_UI_Item_IconAndTime;
		[HideInInspector] public RectTransform m_pl_reward;
		[HideInInspector] public GridLayoutGroup m_pl_items_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_items_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public UI_Tag_PopAnime_SkillTip_SubView m_UI_Tag_PopAnime_SkillTip;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_content_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content");

			m_content_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/content_bg");

			m_img_bg = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/content_bg/img_bg");
			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/content_bg/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/content_bg/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/content_bg/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/content_bg/img_bg/img_arrowSideL");

			m_btns_1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/btns_1");
			m_btns_1_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/pl_content/rect/btns_1");

			m_btn_reinforce = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/btns_1/btn_reinforce"));
			m_btns_2_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/pl_content/rect/btns_2");
			m_btns_2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/btns_2");

			m_btn_look = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/btns_2/btn_look"));
			m_btn_gather = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/btns_2/btn_gather"));
			m_btn_attack = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/btns_2/btn_attack"));
			m_UI_Common_PopFun = new UI_Common_PopFun_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/UI_Common_PopFun"));
			m_btn_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/btn_descinfo");
			m_btn_descinfo_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/rect/btn_descinfo");
			m_btn_descinfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/btn_descinfo");

			m_img_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/btn_descinfo/img_descinfo");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/lbl_name");

			m_lbl_position_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/lbl_position");

			m_pl_up = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_up");
			m_btn_more_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_up/btn_more");
			m_btn_more_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/rect/pl_up/btn_more");
			m_btn_more_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/btn_more");

			m_lbl_content_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/lbl_content");
			m_lbl_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/lbl_content");

			m_pl_type1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type1");

			m_lbl_title1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type1/lbl_title1");
			m_lbl_title1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type1/lbl_title1");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type1/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type1/lbl_desc");

			m_pl_type2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2");

			m_lbl_title2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/lbl_title2");
			m_lbl_title2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/lbl_title2");

			m_icon_buff2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff2");
			m_icon_buff2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff2");

			m_lbl_buffatt2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff2/lbl_buffatt2");
			m_lbl_buffatt2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff2/lbl_buffatt2");

			m_icon_buff1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff1");
			m_icon_buff1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff1");

			m_lbl_buffatt1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff1/lbl_buffatt1");
			m_lbl_buffatt1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type2/icon_buff1/lbl_buffatt1");

			m_pl_type3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3");

			m_lbl_title3_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3/lbl_title3");
			m_lbl_title3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3/lbl_title3");

			m_lbl_kingName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3/lbl_kingName");
			m_lbl_kingName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3/lbl_kingName");

			m_btn_kingset_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3/btn_kingset");
			m_btn_kingset_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3/btn_kingset");
			m_btn_kingset_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_up/pl_type3/btn_kingset");

			m_UI_Item_IconAndTime = new UI_Item_IconAndTime_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_up/UI_Item_IconAndTime"));
			m_pl_reward = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_reward");
			m_pl_items_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/pl_content/rect/pl_reward/pl_items");
			m_pl_items_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_reward/pl_items");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_reward/pl_items/UI_Model_Item"));
			m_UI_Tag_PopAnime_SkillTip = new UI_Tag_PopAnime_SkillTip_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_PopAnime_SkillTip"));

            UI_Pop_WorldObjectInfoBuildMediator mt = new UI_Pop_WorldObjectInfoBuildMediator(vb.gameObject);
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
