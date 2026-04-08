// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月17日
// Update Time         :    2020年3月17日
// Class Description   :    UI_IF_SearchResView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_SearchResView : GameView
    {
        public const string VIEW_NAME = "UI_IF_SearchRes";

        public UI_IF_SearchResView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;
		[HideInInspector] public PolygonImage m_pl_btns_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_pl_btns_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_btns_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ResSearchBtn_SubView m_UI_Item_ResSearchBtn4;
		[HideInInspector] public UI_Item_ResSearchBtn_SubView m_UI_Item_ResSearchBtn3;
		[HideInInspector] public UI_Item_ResSearchBtn_SubView m_UI_Item_ResSearchBtn2;
		[HideInInspector] public UI_Item_ResSearchBtn_SubView m_UI_Item_ResSearchBtn1;
		[HideInInspector] public UI_Item_ResSearchBtn_SubView m_UI_Item_ResSearchBtn;
		[HideInInspector] public RectTransform m_pl_Pop;
		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public PolygonImage m_pl_arrow_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_size_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_MonsterTip_LanguageText;

		[HideInInspector] public LanguageText m_lbl_tip_LanguageText;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_sd_GameSlider_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_GameSlider_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_lower_PolygonImage;
		[HideInInspector] public GameButton m_btn_lower_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_lower_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Yellow_big_SubView m_UI_Model_StandardButton_Blue;
		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;

		[HideInInspector] public UI_Tag_PopAnime_SkillTip_SubView m_UI_Tag_PopAnime_SkillTip;


        private void UIFinder()
        {
			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));
			m_pl_btns_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_btns");
			m_pl_btns_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_btns");
			m_pl_btns_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_btns");

			m_UI_Item_ResSearchBtn4 = new UI_Item_ResSearchBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_btns/UI_Item_ResSearchBtn4"));
			m_UI_Item_ResSearchBtn3 = new UI_Item_ResSearchBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_btns/UI_Item_ResSearchBtn3"));
			m_UI_Item_ResSearchBtn2 = new UI_Item_ResSearchBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_btns/UI_Item_ResSearchBtn2"));
			m_UI_Item_ResSearchBtn1 = new UI_Item_ResSearchBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_btns/UI_Item_ResSearchBtn1"));
			m_UI_Item_ResSearchBtn = new UI_Item_ResSearchBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_btns/UI_Item_ResSearchBtn"));
			m_pl_Pop = FindUI<RectTransform>(vb.transform ,"pl_Pop");
			m_pl_offset = FindUI<RectTransform>(vb.transform ,"pl_Pop/pl_offset");
			m_pl_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Pop/pl_offset/pl_arrow");

			m_pl_size_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size");

			m_lbl_MonsterTip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/lbl_MonsterTip");

			m_lbl_tip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/bg/lbl_tip");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/lbl_level");
			m_lbl_level_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/lbl_level");

			m_sd_GameSlider_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/sd_GameSlider");
			m_sd_GameSlider_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/sd_GameSlider");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/btn_add");

			m_btn_lower_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/btn_lower");
			m_btn_lower_GameButton = FindUI<GameButton>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/btn_lower");
			m_btn_lower_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/btn_lower");

			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_Yellow_big_SubView(FindUI<RectTransform>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/UI_Model_StandardButton_Blue"));
			m_img_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Pop/pl_offset/pl_arrow/pl_size/img_arrow");

			m_UI_Tag_PopAnime_SkillTip = new UI_Tag_PopAnime_SkillTip_SubView(FindUI<RectTransform>(vb.transform ,"pl_Pop/UI_Tag_PopAnime_SkillTip"));

            UI_IF_SearchResMediator mt = new UI_IF_SearchResMediator(vb.gameObject);
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
