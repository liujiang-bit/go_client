// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, August 27, 2020
// Update Time         :    Thursday, August 27, 2020
// Class Description   :    UI_Win_GuildMainView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildMainView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildMain";

        public UI_Win_GuildMainView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public UI_Model_SideToggleGroupBtns_SubView m_UI_Side;
		[HideInInspector] public RectTransform m_pl_rect0;
		[HideInInspector] public PolygonImage m_btn_trans_PolygonImage;
		[HideInInspector] public GameButton m_btn_trans_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_trans_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;

		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_war;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_terrtroy;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_holy;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_help;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_depot;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_technology;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_gift;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_Shop;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_flag;
		[HideInInspector] public LanguageText m_lbl_guildName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_guildName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_master_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_master_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_terrtroy_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_terrtroy_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_gift_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_gift_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_member_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_member_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_msgBoard_PolygonImage;
		[HideInInspector] public GameButton m_btn_msgBoard_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_msgBoard_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_msgRedPoint;
		[HideInInspector] public PolygonImage m_btn_mail_PolygonImage;
		[HideInInspector] public GameButton m_btn_mail_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_mail_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_rect1;
		[HideInInspector] public RectTransform m_pl_blet;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public GameButton m_btn_PlayerHead_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_PlayerHead_ArabLayoutCompment;
		[HideInInspector] public PolygonImage m_btn_PlayerHead_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_member_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_member_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_member_info_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kill_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kill_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public RectTransform m_pl_rect2;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_setting;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_list;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_board;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_check;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_invide;
		[HideInInspector] public UI_Item_GuildFeatureBtns_SubView m_UI_quit;


        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_UI_Side = new UI_Model_SideToggleGroupBtns_SubView(FindUI<RectTransform>(vb.transform ,"UI_Side"));
			m_pl_rect0 = FindUI<RectTransform>(vb.transform ,"pl_rect0");
			m_btn_trans_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect0/left/btn_trans");
			m_btn_trans_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect0/left/btn_trans");
			m_btn_trans_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/left/btn_trans");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/left/sv/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/left/sv/v/c/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_rect0/left/sv/v/c/lbl_desc");

			m_UI_war = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_war"));
			m_UI_terrtroy = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_terrtroy"));
			m_UI_holy = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_holy"));
			m_UI_help = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_help"));
			m_UI_depot = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_depot"));
			m_UI_technology = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_technology"));
			m_UI_gift = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_gift"));
			m_UI_Shop = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/left/features/UI_Shop"));
			m_UI_flag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/right/flagbg/UI_flag"));
			m_lbl_guildName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_guildName");
			m_lbl_guildName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_guildName");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_power");

			m_lbl_master_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_master");
			m_lbl_master_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_master");

			m_lbl_terrtroy_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_terrtroy");
			m_lbl_terrtroy_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_terrtroy");

			m_lbl_gift_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_gift");
			m_lbl_gift_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_gift");

			m_lbl_member_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect0/right/info/lbl_member");
			m_lbl_member_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/lbl_member");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect0/right/info/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect0/right/info/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/btn_info");

			m_btn_msgBoard_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect0/right/info/btn_msgBoard");
			m_btn_msgBoard_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect0/right/info/btn_msgBoard");
			m_btn_msgBoard_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/btn_msgBoard");

			m_UI_msgRedPoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect0/right/info/btn_msgBoard/UI_msgRedPoint"));
			m_btn_mail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect0/right/info/btn_mail");
			m_btn_mail_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect0/right/info/btn_mail");
			m_btn_mail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect0/right/info/btn_mail");

			m_pl_rect1 = FindUI<RectTransform>(vb.transform ,"pl_rect1");
			m_pl_blet = FindUI<RectTransform>(vb.transform ,"pl_rect1/top/pl_blet");
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect1/top/UI_PlayerHead"));
			m_btn_PlayerHead_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect1/top/btn_PlayerHead");
			m_btn_PlayerHead_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/btn_PlayerHead");
			m_btn_PlayerHead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/top/btn_PlayerHead");

			m_btn_member_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/top/btn_member_info");
			m_btn_member_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect1/top/btn_member_info");
			m_btn_member_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/btn_member_info");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/top/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/top/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/lbl_name");

			m_lbl_power1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/top/lbl_power1");
			m_lbl_power1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/lbl_power1");

			m_lbl_kill_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect1/top/lbl_kill");
			m_lbl_kill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect1/top/lbl_kill");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_rect1/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect1/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_rect1/sv_list");

			m_pl_rect2 = FindUI<RectTransform>(vb.transform ,"pl_rect2");
			m_UI_setting = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect2/features/UI_setting"));
			m_UI_list = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect2/features/UI_list"));
			m_UI_board = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect2/features/UI_board"));
			m_UI_check = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect2/features/UI_check"));
			m_UI_invide = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect2/features/UI_invide"));
			m_UI_quit = new UI_Item_GuildFeatureBtns_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect2/features/UI_quit"));

            UI_Win_GuildMainMediator mt = new UI_Win_GuildMainMediator(vb.gameObject);
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
