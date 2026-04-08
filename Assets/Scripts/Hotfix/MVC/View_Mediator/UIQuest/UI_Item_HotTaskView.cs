// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年11月2日
// Update Time         :    2020年11月2日
// Class Description   :    UI_Item_HotTaskView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_HotTaskView : GameView
    {
		public const string VIEW_NAME = "UI_Item_HotTask";

        public UI_Item_HotTaskView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public GameButton m_img_bg_GameButton;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_effect_ArabLayoutCompment;

		[HideInInspector] public UI_10035_SubView m_UI_10035;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_GameButton = FindUI<GameButton>(vb.transform ,"img_bg");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_title");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_desc");

			m_pl_effect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_effect");

			m_UI_10035 = new UI_10035_SubView(FindUI<RectTransform>(vb.transform ,"pl_effect/UI_10035"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}