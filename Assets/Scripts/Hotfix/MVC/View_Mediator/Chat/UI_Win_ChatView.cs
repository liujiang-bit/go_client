// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月9日
// Update Time         :    2020年7月9日
// Class Description   :    UI_Win_ChatView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ChatView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Chat";

        public UI_Win_ChatView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public PolygonImage m_img_mainbg_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public RectTransform m_pl_chat;
		[HideInInspector] public PolygonImage m_img_chat_PolygonImage;

		[HideInInspector] public RectTransform m_pl_up;
		[HideInInspector] public PolygonImage m_btn_chatsetting_PolygonImage;
		[HideInInspector] public GameButton m_btn_chatsetting_GameButton;

		[HideInInspector] public PolygonImage m_btn_chatundisturb_PolygonImage;
		[HideInInspector] public GameButton m_btn_chatundisturb_GameButton;

		[HideInInspector] public PolygonImage m_img_chatundisturb_PolygonImage;

		[HideInInspector] public PolygonImage m_img_chatdisturb_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_chatnotice_PolygonImage;
		[HideInInspector] public GameButton m_btn_chatnotice_GameButton;

		[HideInInspector] public PolygonImage m_btn_chatflip_PolygonImage;
		[HideInInspector] public GameButton m_btn_chatflip_GameButton;

		[HideInInspector] public PolygonImage m_btn_chatshrink_PolygonImage;
		[HideInInspector] public GameButton m_btn_chatshrink_GameButton;

		[HideInInspector] public PolygonImage m_img_shrinkreddot_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_shrinkreddot_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public ScrollRect m_sv_chat_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_chat_PolygonImage;
		[HideInInspector] public ListView m_sv_chat_ListView;

		[HideInInspector] public PolygonImage m_v_chat_PolygonImage;
		[HideInInspector] public Mask m_v_chat_Mask;

		[HideInInspector] public RectTransform m_c_chat;
		[HideInInspector] public PolygonImage m_btn_emojimask_PolygonImage;
		[HideInInspector] public GameButton m_btn_emojimask_GameButton;

		[HideInInspector] public PolygonImage m_btn_currentchatunread_PolygonImage;
		[HideInInspector] public GameButton m_btn_currentchatunread_GameButton;

		[HideInInspector] public LanguageText m_lbl_currentchatunread_LanguageText;

		[HideInInspector] public RectTransform m_pl_down;
		[HideInInspector] public PolygonImage m_ipt_chat_PolygonImage;
		[HideInInspector] public GameInput m_ipt_chat_GameInput;

		[HideInInspector] public PolygonImage m_btn_emoji_PolygonImage;
		[HideInInspector] public GameButton m_btn_emoji_GameButton;

		[HideInInspector] public PolygonImage m_btn_more_PolygonImage;
		[HideInInspector] public GameButton m_btn_more_GameButton;

		[HideInInspector] public PolygonImage m_btn_voice_PolygonImage;
		[HideInInspector] public GameButton m_btn_voice_GameButton;

		[HideInInspector] public PolygonImage m_btn_send_PolygonImage;
		[HideInInspector] public GameButton m_btn_send_GameButton;

		[HideInInspector] public RectTransform m_pl_emoji;
		[HideInInspector] public ScrollRect m_sv_emoji_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_emoji_PolygonImage;
		[HideInInspector] public ListView m_sv_emoji_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public ScrollRect m_sv_emojimenu_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_emojimenu_PolygonImage;
		[HideInInspector] public ListView m_sv_emojimenu_ListView;

		[HideInInspector] public RectTransform m_pl_contact;
		[HideInInspector] public ScrollRect m_sv_contact_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_contact_PolygonImage;
		[HideInInspector] public ListView m_sv_contact_ListView;

		[HideInInspector] public RectTransform m_pl_contactup;
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public PolygonImage m_btn_contactmenu_PolygonImage;
		[HideInInspector] public GameButton m_btn_contactmenu_GameButton;

		[HideInInspector] public RectTransform m_pl_detail;
		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public RectTransform m_pl_detailtitle;
		[HideInInspector] public PolygonImage m_btn_returnfromdetail_PolygonImage;
		[HideInInspector] public GameButton m_btn_returnfromdetail_GameButton;

		[HideInInspector] public PolygonImage m_btn_detailsetting_PolygonImage;
		[HideInInspector] public GameButton m_btn_detailsetting_GameButton;

		[HideInInspector] public VerticalLayoutGroup m_pl_detailmenu_VerticalLayoutGroup;

		[HideInInspector] public RectTransform m_pl_groupname;
		[HideInInspector] public LanguageText m_lbl_groupname_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_groupname_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_playerhead_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_playerhead_PolygonImage;
		[HideInInspector] public ListView m_sv_playerhead_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_playerhead_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_addmember;
		[HideInInspector] public PolygonImage m_btn_addmember_PolygonImage;
		[HideInInspector] public GameButton m_btn_addmember_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_addmember_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_deletemember;
		[HideInInspector] public PolygonImage m_btn_deletemember_PolygonImage;
		[HideInInspector] public GameButton m_btn_deletemember_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_deletemember_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_blockthis;
		[HideInInspector] public GameToggle m_ck_blockthis_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_blockthis_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_blockthis_open_PolygonImage;

		[HideInInspector] public PolygonImage m_img_blockthis_unopen_PolygonImage;

		[HideInInspector] public RectTransform m_pl_nodisturb;
		[HideInInspector] public ArabLayoutCompment m_lbl_languageText_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_nodisturb_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_nodisturb_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_nodisturb_open_PolygonImage;

		[HideInInspector] public PolygonImage m_img_nodisturb_unopen_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_btn_GridLayoutGroup;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_detailMsg;
		[HideInInspector] public UI_Model_StandardButton_Red_SubView m_btn_detailDelete;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_detailfunc;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_cancel;
		[HideInInspector] public RectTransform m_pl_contactmenu;
		[HideInInspector] public PolygonImage m_btn_contactmenumask_PolygonImage;
		[HideInInspector] public GameButton m_btn_contactmenumask_GameButton;

		[HideInInspector] public PolygonImage m_img_contactmenu_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_img_contactmenu_GridLayoutGroup;

		[HideInInspector] public PolygonImage m_btn_myfriendss_PolygonImage;
		[HideInInspector] public GameButton m_btn_myfriendss_GameButton;

		[HideInInspector] public PolygonImage m_btn_startgroupchat_PolygonImage;
		[HideInInspector] public GameButton m_btn_startgroupchat_GameButton;

		[HideInInspector] public PolygonImage m_btn_addfriend_PolygonImage;
		[HideInInspector] public GameButton m_btn_addfriend_GameButton;

		[HideInInspector] public UI_Model_ChatEmojiPreview_SubView m_UI_Model_ChatEmojiPreview;


        private void UIFinder()
        {
			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_content");
			m_img_mainbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/img_mainbg");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/btn_close");

			m_pl_chat = FindUI<RectTransform>(vb.transform ,"pl_content/pl_chat");
			m_img_chat_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/img_chat");

			m_pl_up = FindUI<RectTransform>(vb.transform ,"pl_content/pl_chat/pl_up");
			m_btn_chatsetting_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatsetting");
			m_btn_chatsetting_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatsetting");

			m_btn_chatundisturb_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatundisturb");
			m_btn_chatundisturb_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatundisturb");

			m_img_chatundisturb_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatundisturb/img_chatundisturb");

			m_img_chatdisturb_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatundisturb/img_chatdisturb");

			m_btn_chatnotice_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatnotice");
			m_btn_chatnotice_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatnotice");

			m_btn_chatflip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatflip");
			m_btn_chatflip_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatflip");

			m_btn_chatshrink_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatshrink");
			m_btn_chatshrink_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatshrink");

			m_img_shrinkreddot_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatshrink/img_shrinkreddot");

			m_lbl_shrinkreddot_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/pl_chat/pl_up/btn_chatshrink/img_shrinkreddot/lbl_shrinkreddot");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/pl_chat/pl_up/lbl_name");

			m_sv_chat_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_chat/sv_chat");
			m_sv_chat_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/sv_chat");
			m_sv_chat_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_chat/sv_chat");

			m_v_chat_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/sv_chat/v_chat");
			m_v_chat_Mask = FindUI<Mask>(vb.transform ,"pl_content/pl_chat/sv_chat/v_chat");

			m_c_chat = FindUI<RectTransform>(vb.transform ,"pl_content/pl_chat/sv_chat/v_chat/c_chat");
			m_btn_emojimask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/sv_chat/btn_emojimask");
			m_btn_emojimask_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/sv_chat/btn_emojimask");

			m_btn_currentchatunread_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/sv_chat/btn_currentchatunread");
			m_btn_currentchatunread_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/sv_chat/btn_currentchatunread");

			m_lbl_currentchatunread_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/pl_chat/sv_chat/btn_currentchatunread/lbl_currentchatunread");

			m_pl_down = FindUI<RectTransform>(vb.transform ,"pl_content/pl_chat/pl_down");
			m_ipt_chat_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_down/ipt_chat");
			m_ipt_chat_GameInput = FindUI<GameInput>(vb.transform ,"pl_content/pl_chat/pl_down/ipt_chat");

			m_btn_emoji_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_down/btn_emoji");
			m_btn_emoji_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_down/btn_emoji");

			m_btn_more_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_down/btn_more");
			m_btn_more_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_down/btn_more");

			m_btn_voice_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_down/btn_voice");
			m_btn_voice_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_down/btn_voice");

			m_btn_send_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_down/btn_send");
			m_btn_send_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_chat/pl_down/btn_send");

			m_pl_emoji = FindUI<RectTransform>(vb.transform ,"pl_content/pl_chat/pl_emoji");
			m_sv_emoji_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emoji");
			m_sv_emoji_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emoji");
			m_sv_emoji_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emoji");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emoji/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emoji/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emoji/v_list_view/c_list_view");
			m_sv_emojimenu_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emojimenu");
			m_sv_emojimenu_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emojimenu");
			m_sv_emojimenu_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_chat/pl_emoji/sv_emojimenu");

			m_pl_contact = FindUI<RectTransform>(vb.transform ,"pl_content/pl_contact");
			m_sv_contact_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_contact/sv_contact");
			m_sv_contact_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_contact/sv_contact");
			m_sv_contact_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_contact/sv_contact");

			m_pl_contactup = FindUI<RectTransform>(vb.transform ,"pl_content/pl_contact/pl_contactup");
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/pl_contact/pl_contactup/lbl_languageText");

			m_btn_contactmenu_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_contact/pl_contactup/btn_contactmenu");
			m_btn_contactmenu_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_contact/pl_contactup/btn_contactmenu");

			m_pl_detail = FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail");
			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/img_polygonImage");

			m_pl_detailtitle = FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_detailtitle");
			m_btn_returnfromdetail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailtitle/btn_returnfromdetail");
			m_btn_returnfromdetail_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_detail/pl_detailtitle/btn_returnfromdetail");

			m_btn_detailsetting_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailtitle/btn_detailsetting");
			m_btn_detailsetting_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_detail/pl_detailtitle/btn_detailsetting");

			m_pl_detailmenu_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_content/pl_detail/pl_detailmenu");

			m_pl_groupname = FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_groupname");
			m_lbl_groupname_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_groupname/lbl_groupname");
			m_lbl_groupname_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_groupname/lbl_groupname");

			m_sv_playerhead_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/sv_playerhead");
			m_sv_playerhead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/sv_playerhead");
			m_sv_playerhead_ListView = FindUI<ListView>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/sv_playerhead");
			m_sv_playerhead_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/sv_playerhead");

			m_pl_addmember = FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_addmember");
			m_btn_addmember_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_addmember/btn_addmember");
			m_btn_addmember_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_addmember/btn_addmember");
			m_btn_addmember_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_addmember/btn_addmember");

			m_pl_deletemember = FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_deletemember");
			m_btn_deletemember_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_deletemember/btn_deletemember");
			m_btn_deletemember_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_deletemember/btn_deletemember");
			m_btn_deletemember_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_deletemember/btn_deletemember");

			m_pl_blockthis = FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_blockthis");
			m_ck_blockthis_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_blockthis/ck_blockthis");
			m_ck_blockthis_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_blockthis/ck_blockthis");

			m_img_blockthis_open_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_blockthis/ck_blockthis/img_blockthis_open");

			m_img_blockthis_unopen_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_blockthis/ck_blockthis/img_blockthis_unopen");

			m_pl_nodisturb = FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_nodisturb");
			m_lbl_languageText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_nodisturb/lbl_languageText");

			m_ck_nodisturb_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_nodisturb/ck_nodisturb");
			m_ck_nodisturb_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_nodisturb/ck_nodisturb");

			m_img_nodisturb_open_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_nodisturb/ck_nodisturb/img_nodisturb_open");

			m_img_nodisturb_unopen_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_detail/pl_detailmenu/pl_nodisturb/ck_nodisturb/img_nodisturb_unopen");

			m_pl_btn_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_content/pl_detail/pl_btn");

			m_btn_detailMsg = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_btn/btn_detailMsg"));
			m_btn_detailDelete = new UI_Model_StandardButton_Red_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_btn/btn_detailDelete"));
			m_btn_detailfunc = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_btn/btn_detailfunc"));
			m_btn_cancel = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/pl_detail/pl_btn/btn_cancel"));
			m_pl_contactmenu = FindUI<RectTransform>(vb.transform ,"pl_content/pl_contactmenu");
			m_btn_contactmenumask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_contactmenu/btn_contactmenumask");
			m_btn_contactmenumask_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_contactmenu/btn_contactmenumask");

			m_img_contactmenu_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu");
			m_img_contactmenu_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu");

			m_btn_myfriendss_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu/btn_myfriendss");
			m_btn_myfriendss_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu/btn_myfriendss");

			m_btn_startgroupchat_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu/btn_startgroupchat");
			m_btn_startgroupchat_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu/btn_startgroupchat");

			m_btn_addfriend_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu/btn_addfriend");
			m_btn_addfriend_GameButton = FindUI<GameButton>(vb.transform ,"pl_content/pl_contactmenu/img_contactmenu/btn_addfriend");

			m_UI_Model_ChatEmojiPreview = new UI_Model_ChatEmojiPreview_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/UI_Model_ChatEmojiPreview"));

            UI_Win_ChatMediator mt = new UI_Win_ChatMediator(vb.gameObject);
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
