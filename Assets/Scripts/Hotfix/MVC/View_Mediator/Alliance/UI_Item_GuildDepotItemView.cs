// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, May 12, 2020
// Update Time         :    Tuesday, May 12, 2020
// Class Description   :    UI_Item_GuildDepotItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildDepotItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildDepotItem";

        public UI_Item_GuildDepotItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bgmy_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgNomal_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_view_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_view_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_leaguePoints;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Food;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Wood;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Stone;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Gold;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bgmy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bgmy");

			m_img_bgNomal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bgNomal");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_num");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_name");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_time");

			m_pl_view_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_view");
			m_pl_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_view");

			m_UI_leaguePoints = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_view/UI_leaguePoints"));
			m_UI_Food = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_view/UI_Food"));
			m_UI_Wood = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_view/UI_Wood"));
			m_UI_Stone = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_view/UI_Stone"));
			m_UI_Gold = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_view/UI_Gold"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}