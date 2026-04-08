// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月30日
// Update Time         :    2020年7月30日
// Class Description   :    UI_Win_UseItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_UseItemView : GameView
    {
        public const string VIEW_NAME = "UI_Win_UseItem";

        public UI_Win_UseItemView () 
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
		[HideInInspector] public SmoothProgressBar m_pb_rogressBar_SmoothBar;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_vipLevel_LanguageText;
		[HideInInspector] public Outline m_lbl_vipLevel_Outline;

		[HideInInspector] public PolygonImage m_img_item_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_item_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Item_CaptainLevelUpOnHead_SubView m_UI_Item_LevelUp;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Item_PBinTech_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");
			m_pb_rogressBar_SmoothBar = FindUI<SmoothProgressBar>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_num");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/img_icon");

			m_lbl_vipLevel_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/img_icon/lbl_vipLevel");
			m_lbl_vipLevel_Outline = FindUI<Outline>(vb.transform ,"rect/UI_Item_PBinTech/img_icon/lbl_vipLevel");

			m_img_item_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/UI_Item_PBinTech/img_item");
			m_img_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/img_item");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_UI_Item_LevelUp = new UI_Item_CaptainLevelUpOnHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_LevelUp"));

            UI_Win_UseItemMediator mt = new UI_Win_UseItemMediator(vb.gameObject);
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
