// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月10日
// Update Time         :    2020年7月10日
// Class Description   :    UI_Win_GuildJoinView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildJoinView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildJoin";

        public UI_Win_GuildJoinView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public PolygonImage m_ipt_ipt_PolygonImage;
		[HideInInspector] public GameInput m_ipt_ipt_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_ipt_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_search_PolygonImage;
		[HideInInspector] public GameButton m_btn_search_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_search_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_tip_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_master_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_master_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_mail_PolygonImage;
		[HideInInspector] public GameButton m_btn_mail_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_mail_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_guildDesc_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_guildDesc_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_guildDesc_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_info;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_needJion;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_cancel;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_join;


        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_ipt_ipt_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/ipt_ipt");
			m_ipt_ipt_GameInput = FindUI<GameInput>(vb.transform ,"rect/top/ipt_ipt");
			m_ipt_ipt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/ipt_ipt");

			m_btn_search_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/top/btn_search");
			m_btn_search_GameButton = FindUI<GameButton>(vb.transform ,"rect/top/btn_search");
			m_btn_search_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/top/btn_search");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/left/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/left/sv_list");

			m_lbl_tip_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_tip");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/title/UI_PlayerHead"));
			m_lbl_master_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/title/lbl_master");
			m_lbl_master_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/title/lbl_master");

			m_btn_mail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_right/title/btn_mail");
			m_btn_mail_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_right/title/btn_mail");
			m_btn_mail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/title/btn_mail");

			m_lbl_guildDesc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/bg/sv/v/c/lbl_guildDesc");
			m_lbl_guildDesc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/pl_right/bg/sv/v/c/lbl_guildDesc");
			m_lbl_guildDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right/bg/sv/v/c/lbl_guildDesc");

			m_UI_info = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/UI_info"));
			m_UI_needJion = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/UI_needJion"));
			m_UI_cancel = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/UI_cancel"));
			m_UI_join = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/UI_join"));

            UI_Win_GuildJoinMediator mt = new UI_Win_GuildJoinMediator(vb.gameObject);
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
