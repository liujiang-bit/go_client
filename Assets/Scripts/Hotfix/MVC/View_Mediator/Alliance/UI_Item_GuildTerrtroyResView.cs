// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Sunday, June 28, 2020
// Update Time         :    Sunday, June 28, 2020
// Class Description   :    UI_Item_GuildTerrtroyResView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildTerrtroyResView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildTerrtroyRes";

        public UI_Item_GuildTerrtroyResView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes1;
		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes2;
		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes3;
		[HideInInspector] public UI_Item_GuildDepotRes_SubView m_UI_Item_GuildDepotRes4;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_text");

			m_UI_Item_GuildDepotRes1 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_GuildDepotRes1"));
			m_UI_Item_GuildDepotRes2 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_GuildDepotRes2"));
			m_UI_Item_GuildDepotRes3 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_GuildDepotRes3"));
			m_UI_Item_GuildDepotRes4 = new UI_Item_GuildDepotRes_SubView(FindUI<RectTransform>(vb.transform ,"res/UI_Item_GuildDepotRes4"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}