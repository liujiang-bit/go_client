// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, April 20, 2020
// Update Time         :    Monday, April 20, 2020
// Class Description   :    UI_Item_GuildTerrtroyBuildingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildTerrtroyBuildingView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildTerrtroyBuilding";

        public UI_Item_GuildTerrtroyBuildingView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public UI_Item_GuildTerrtroyBuildingSingle_SubView m_UI_Item_build1;
		[HideInInspector] public UI_Item_GuildTerrtroyBuildingSingle_SubView m_UI_Item_build2;
		[HideInInspector] public UI_Item_GuildTerrtroyBuildingSingle_SubView m_UI_Item_build3;
		[HideInInspector] public UI_Item_GuildTerrtroyBuildingSingle_SubView m_UI_Item_build4;
		[HideInInspector] public UI_Item_GuildTerrtroyBuildingSingle_SubView m_UI_Item_build5;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_text");

			m_UI_Item_build1 = new UI_Item_GuildTerrtroyBuildingSingle_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_build1"));
			m_UI_Item_build2 = new UI_Item_GuildTerrtroyBuildingSingle_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_build2"));
			m_UI_Item_build3 = new UI_Item_GuildTerrtroyBuildingSingle_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_build3"));
			m_UI_Item_build4 = new UI_Item_GuildTerrtroyBuildingSingle_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_build4"));
			m_UI_Item_build5 = new UI_Item_GuildTerrtroyBuildingSingle_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_build5"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}