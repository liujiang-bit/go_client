// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GuildTerrtroyBuildingSingle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GuildTerrtroyBuildingSingle_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GuildTerrtroyBuildingSingle";

        public UI_Item_GuildTerrtroyBuildingSingle_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Empty4Raycast m_UI_Item_GuildTerrtroyBuildingSingle_Empty4Raycast;
		[HideInInspector] public ViewBinder m_UI_Item_GuildTerrtroyBuildingSingle_ViewBinder;

		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;

		[HideInInspector] public RectTransform m_pl_data;
		[HideInInspector] public UI_Model_Link_SubView m_btn_link;
		[HideInInspector] public LanguageText m_lbl_state_LanguageText;

		[HideInInspector] public LanguageText m_lbl_process_LanguageText;

		[HideInInspector] public PolygonImage m_btn_army_PolygonImage;
		[HideInInspector] public GameButton m_btn_army_GameButton;

		[HideInInspector] public RectTransform m_pl_build;
		[HideInInspector] public PolygonImage m_btn_plus_PolygonImage;
		[HideInInspector] public GameButton m_btn_plus_GameButton;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_build;
		[HideInInspector] public LanguageText m_lbl_limit_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_GuildTerrtroyBuildingSingle_Empty4Raycast = gameObject.GetComponent<Empty4Raycast>();
			m_UI_Item_GuildTerrtroyBuildingSingle_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_content = FindUI<RectTransform>(gameObject.transform ,"pl_content");
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_info");

			m_pl_data = FindUI<RectTransform>(gameObject.transform ,"pl_data");
			m_btn_link = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_data/btn_link"));
			m_lbl_state_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_data/lbl_state");

			m_lbl_process_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_data/lbl_process");

			m_btn_army_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_data/btn_army");
			m_btn_army_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_data/btn_army");

			m_pl_build = FindUI<RectTransform>(gameObject.transform ,"pl_build");
			m_btn_plus_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_build/btn_plus");
			m_btn_plus_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_build/btn_plus");

			m_btn_build = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_build/btn_build"));
			m_lbl_limit_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_limit");


			BindEvent();
        }

        #endregion
    }
}