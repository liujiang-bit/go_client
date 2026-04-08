// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Item_AllianceProjectView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_AllianceProjectView : GameView
    {
		public const string VIEW_NAME = "UI_Item_AllianceProject";

        public UI_Item_AllianceProjectView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_project_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_project_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_allianceName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_allianceName_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_rank_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_rank_LanguageText;

		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_project_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_project");
			m_lbl_project_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_project");

			m_lbl_allianceName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_allianceName");
			m_lbl_allianceName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_allianceName");

			m_img_rank_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ranking/img_rank");

			m_lbl_rank_LanguageText = FindUI<LanguageText>(vb.transform ,"ranking/lbl_rank");

			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlag"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}