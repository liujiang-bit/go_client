// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType16_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType16_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType16";

        public UI_Item_MailType16_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public VerticalLayoutGroup m_UI_Item_MailType16_VerticalLayoutGroup;
		[HideInInspector] public ViewBinder m_UI_Item_MailType16_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public LanguageText m_lbl_Content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_Content_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_Content_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_player_message_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_Model_GuildFlag;
		[HideInInspector] public PolygonImage m_img_build_PolygonImage;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_wallhp_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_wallhp_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_resources;
		[HideInInspector] public UI_Item_MailType16MessageList_SubView m_pl_resources_title;
		[HideInInspector] public UI_Model_ResCost_SubView m_UI_Model_ResCost;
		[HideInInspector] public VerticalLayoutGroup m_pl_troops_VerticalLayoutGroup;

		[HideInInspector] public UI_Item_MailType16MessageList_SubView m_pl_troops_title;
		[HideInInspector] public UI_Item_MailHero_SubView m_pl_troops_heromain;
		[HideInInspector] public UI_Item_MailHero_SubView m_pl_troops_herosub;
		[HideInInspector] public LanguageText m_lbl_none_hero_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_none_hero_ArabLayoutCompment;

		[HideInInspector] public UI_Item_MailType16Soldiers_SubView m_pl_troops_soldiers;
		[HideInInspector] public LanguageText m_lbl_none_troops_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_none_troops_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_reinforce_VerticalLayoutGroup;

		[HideInInspector] public UI_Item_MailType16MessageList_SubView m_pl_reinforce_title;
		[HideInInspector] public UI_Item_MailType16Soldiers_SubView m_pl_reinforce_soldiers;
		[HideInInspector] public LanguageText m_lbl_none_reinforce_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_none_reinforce_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_mass_VerticalLayoutGroup;

		[HideInInspector] public UI_Item_MailType16MessageList_SubView m_pl_mass_title;
		[HideInInspector] public UI_Item_MailHero_SubView m_pl_mass_heromain;
		[HideInInspector] public UI_Item_MailHero_SubView m_pl_mass_herosub;
		[HideInInspector] public UI_Item_MailType16Soldiers_SubView m_pl_mass_soldiers;
		[HideInInspector] public LanguageText m_lbl_none_mass_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_none_mass_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_tower;
		[HideInInspector] public UI_Item_MailType16MessageList_SubView m_pl_tower_title;
		[HideInInspector] public LanguageText m_lbl_towername_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_towername_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_tower_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_tower_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_towerhp_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_towerhp_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailType16_VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
			m_UI_Item_MailType16_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_lbl_Content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_Content");
			m_lbl_Content_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_Content");
			m_lbl_Content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_Content");

			m_pl_player_message_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_player_message");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_player_message/UI_Model_PlayerHead"));
			m_UI_Model_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_player_message/UI_Model_GuildFlag"));
			m_img_build_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_player_message/img_build");

			m_UI_Model_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_player_message/UI_Model_Link"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_player_message/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_player_message/lbl_name");

			m_lbl_wallhp_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_player_message/lbl_wallhp");
			m_lbl_wallhp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_player_message/lbl_wallhp");

			m_pl_resources = FindUI<RectTransform>(gameObject.transform ,"pl_resources");
			m_pl_resources_title = new UI_Item_MailType16MessageList_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_resources/pl_resources_title"));
			m_UI_Model_ResCost = new UI_Model_ResCost_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_resources/UI_Model_ResCost"));
			m_pl_troops_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_troops");

			m_pl_troops_title = new UI_Item_MailType16MessageList_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_troops/pl_troops_title"));
			m_pl_troops_heromain = new UI_Item_MailHero_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_troops/pl_troops_heromain"));
			m_pl_troops_herosub = new UI_Item_MailHero_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_troops/pl_troops_herosub"));
			m_lbl_none_hero_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_troops/lbl_none_hero");
			m_lbl_none_hero_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_troops/lbl_none_hero");

			m_pl_troops_soldiers = new UI_Item_MailType16Soldiers_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_troops/pl_troops_soldiers"));
			m_lbl_none_troops_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_troops/lbl_none_troops");
			m_lbl_none_troops_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_troops/lbl_none_troops");

			m_pl_reinforce_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_reinforce");

			m_pl_reinforce_title = new UI_Item_MailType16MessageList_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reinforce/pl_reinforce_title"));
			m_pl_reinforce_soldiers = new UI_Item_MailType16Soldiers_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reinforce/pl_reinforce_soldiers"));
			m_lbl_none_reinforce_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_reinforce/lbl_none_reinforce");
			m_lbl_none_reinforce_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_reinforce/lbl_none_reinforce");

			m_pl_mass_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_mass");

			m_pl_mass_title = new UI_Item_MailType16MessageList_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mass/pl_mass_title"));
			m_pl_mass_heromain = new UI_Item_MailHero_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mass/pl_mass_heromain"));
			m_pl_mass_herosub = new UI_Item_MailHero_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mass/pl_mass_herosub"));
			m_pl_mass_soldiers = new UI_Item_MailType16Soldiers_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mass/pl_mass_soldiers"));
			m_lbl_none_mass_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mass/lbl_none_mass");
			m_lbl_none_mass_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mass/lbl_none_mass");

			m_pl_tower = FindUI<RectTransform>(gameObject.transform ,"pl_tower");
			m_pl_tower_title = new UI_Item_MailType16MessageList_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_tower/pl_tower_title"));
			m_lbl_towername_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_tower/lbl_towername");
			m_lbl_towername_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_tower/lbl_towername");

			m_img_tower_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_tower/img_tower");
			m_img_tower_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_tower/img_tower");

			m_lbl_towerhp_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_tower/lbl_towerhp");
			m_lbl_towerhp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_tower/lbl_towerhp");


			BindEvent();
        }

        #endregion
    }
}