// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_GuildHolyItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_GuildHolyItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_GuildHolyItem";

        public UI_Model_GuildHolyItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_GuildHolyItem;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_pl_pos;
		[HideInInspector] public LanguageText m_lbl_state_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;

		[HideInInspector] public RectTransform m_pl_build;
		[HideInInspector] public VerticalLayoutGroup m_pl_data_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_data1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_data1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_data2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_data2_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_GuildHolyItem = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_name");

			m_pl_pos = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"bg/pl_pos"));
			m_lbl_state_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_state");
			m_lbl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_state");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"bg/btn_info");

			m_pl_build = FindUI<RectTransform>(gameObject.transform ,"bg/pl_build");
			m_pl_data_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"bg2/pl_data");

			m_lbl_data1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg2/pl_data/lbl_data1");
			m_lbl_data1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg2/pl_data/lbl_data1");

			m_lbl_data2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg2/pl_data/lbl_data2");
			m_lbl_data2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg2/pl_data/lbl_data2");


			BindEvent();
        }

        #endregion
    }
}