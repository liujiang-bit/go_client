// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月19日
// Update Time         :    2020年3月19日
// Class Description   :    AddSpeedView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class AddSpeedView : GameView
    {
        public const string VIEW_NAME = "UI_Win_AddSpeed";

        public AddSpeedView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_PBinTech_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_hospital_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_hospital_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_research_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_research_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_researchicon_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_build_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_stopOverSize_PolygonImage;
		[HideInInspector] public GameButton m_btn_stopOverSize_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_stopOverSize_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_stop_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_stop_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_NoneUse_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Item_PBinTech_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_time");

			m_img_hospital_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/img_hospital");
			m_img_hospital_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/img_hospital");

			m_img_research_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/img_research");
			m_img_research_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/img_research");

			m_img_researchicon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/img_research/img_researchicon");

			m_pl_build_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pl_build");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/img_icon");

			m_btn_stopOverSize_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/btn_stopOverSize");
			m_btn_stopOverSize_GameButton = FindUI<GameButton>(vb.transform ,"rect/UI_Item_PBinTech/btn_stopOverSize");
			m_btn_stopOverSize_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/btn_stopOverSize");

			m_img_stop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/btn_stopOverSize/img_stop");
			m_img_stop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/btn_stopOverSize/img_stop");

			m_lbl_NoneUse_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_NoneUse");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");


            AddSpeedMediator mt = new AddSpeedMediator(vb.gameObject);
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
