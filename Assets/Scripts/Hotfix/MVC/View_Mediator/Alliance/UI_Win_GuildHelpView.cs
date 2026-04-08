// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月23日
// Update Time         :    2020年6月23日
// Class Description   :    UI_Win_GuildHelpView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildHelpView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildHelp";

        public UI_Win_GuildHelpView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public UI_Item_PBInTech_SubView m_UI_Item_PBinTech;
		[HideInInspector] public LanguageText m_lbl_nextTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_nextTime_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_tip_PolygonImage;
		[HideInInspector] public GameButton m_btn_tip_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_tip_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_nohelp_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_btn_help;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_UI_Item_PBinTech = new UI_Item_PBInTech_SubView(FindUI<RectTransform>(vb.transform ,"rect/top/UI_Item_PBinTech"));
			m_lbl_nextTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/lbl_nextTime");
			m_lbl_nextTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/lbl_nextTime");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_info");

			m_btn_tip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_tip");
			m_btn_tip_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_tip");
			m_btn_tip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_tip");

			m_lbl_nohelp_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/top/lbl_nohelp");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_btn_help = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_help"));

            UI_Win_GuildHelpMediator mt = new UI_Win_GuildHelpMediator(vb.gameObject);
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
