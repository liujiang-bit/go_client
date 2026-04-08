// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Win_Ranking_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Win_Ranking_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Win_Ranking";

        public UI_Win_Ranking_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Win_Ranking_ViewBinder;

		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public GridLayoutGroup m_pl_Total_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_Total_ArabLayoutCompment;

		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Guild0;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Guild1;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Guild2;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Person0;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Person1;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Person2;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Other0;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Other1;
		[HideInInspector] public UI_Item_GuildRankingBtn_SubView m_UI_Other2;
		[HideInInspector] public RectTransform m_pl_Detail;
		[HideInInspector] public PolygonImage m_img_Blet_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_itemTitle0_LanguageText;

		[HideInInspector] public LanguageText m_lbl_itemTitle1_LanguageText;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;
		[HideInInspector] public LanguageText m_lbl_ranking_LanguageText;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_guildName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_playerPow_LanguageText;

		[HideInInspector] public PolygonImage m_img_arrowUp_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowUp_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowDown_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowDown_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Item_RankingItem_SubView m_UI_Item_RankingItem;


        private void UIFinder()
        {       
			m_UI_Win_Ranking_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Window_Type3"));
			m_pl_Total_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_Total");
			m_pl_Total_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_Total");

			m_UI_Guild0 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Guild0"));
			m_UI_Guild1 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Guild1"));
			m_UI_Guild2 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Guild2"));
			m_UI_Person0 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Person0"));
			m_UI_Person1 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Person1"));
			m_UI_Person2 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Person2"));
			m_UI_Other0 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Other0"));
			m_UI_Other1 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Other1"));
			m_UI_Other2 = new UI_Item_GuildRankingBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Total/UI_Other2"));
			m_pl_Detail = FindUI<RectTransform>(gameObject.transform ,"pl_Detail");
			m_img_Blet_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_Detail/top/img_Blet");

			m_lbl_itemTitle0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Detail/top/lbl_itemTitle0");

			m_lbl_itemTitle1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Detail/top/lbl_itemTitle1");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Detail/top/bletContent/UI_PlayerHead"));
			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Detail/top/bletContent/UI_GuildFlag"));
			m_lbl_ranking_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Detail/top/bletContent/lbl_ranking");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Detail/top/bletContent/lbl_playerName");

			m_lbl_guildName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Detail/top/bletContent/lbl_guildName");

			m_lbl_playerPow_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Detail/top/bletContent/lbl_playerPow");

			m_img_arrowUp_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_Detail/top/bletContent/img_arrowUp");
			m_img_arrowUp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_Detail/top/bletContent/img_arrowUp");

			m_img_arrowDown_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_Detail/top/bletContent/img_arrowDown");
			m_img_arrowDown_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_Detail/top/bletContent/img_arrowDown");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_Detail/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_Detail/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"pl_Detail/sv_list");

			m_UI_Item_RankingItem = new UI_Item_RankingItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_Detail/sv_list/v/c/UI_Item_RankingItem"));

			BindEvent();
        }

        #endregion
    }
}