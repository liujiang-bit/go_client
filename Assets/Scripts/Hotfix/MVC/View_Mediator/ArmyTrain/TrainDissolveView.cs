// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月18日
// Update Time         :    2020年5月18日
// Class Description   :    TrainDissolveView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class TrainDissolveView : GameView
    {
        public const string VIEW_NAME = "UI_Win_TrainDissolve";

        public TrainDissolveView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public Image m_pl_mes_Image;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_img_army_icon;
		[HideInInspector] public GameSlider m_sd_count_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_count_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_count_PolygonImage;
		[HideInInspector] public GameInput m_ipt_count_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_count_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_combatDown_LanguageText;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_anim;


        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_pl_mes_Image = FindUI<Image>(vb.transform ,"pl_mes");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/lbl_desc");

			m_pl_view = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_view");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContentBg/pl_view/img_bg");

			m_img_army_icon = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_view/img_army_icon"));
			m_sd_count_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_mes/imgContentBg/pl_view/sd_count");
			m_sd_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_view/sd_count");

			m_ipt_count_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContentBg/pl_view/ipt_count");
			m_ipt_count_GameInput = FindUI<GameInput>(vb.transform ,"pl_mes/imgContentBg/pl_view/ipt_count");
			m_ipt_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_view/ipt_count");

			m_lbl_combatDown_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/lbl_combatDown");

			m_btn_anim = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/btn_anim"));

            TrainDissolveMediator mt = new TrainDissolveMediator(vb.gameObject);
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
