// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, 12 October 2020
// Update Time         :    Monday, 12 October 2020
// Class Description   :    UI_Item_ServerTitleView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_ServerTitleView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ServerTitle";

        public UI_Item_ServerTitleView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_title_PolygonImage;
		[HideInInspector] public GameButton m_btn_title_GameButton;

		[HideInInspector] public LanguageText m_lbl_kingdomName1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomName1_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_kingdomName1_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_kingdomName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomName_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowdown_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowdown_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowup_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowup_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kingdomNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomNum_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_title_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_title");
			m_btn_title_GameButton = FindUI<GameButton>(vb.transform ,"btn_title");

			m_lbl_kingdomName1_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_title/lbl_kingdomName1");
			m_lbl_kingdomName1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_title/lbl_kingdomName1");
			m_lbl_kingdomName1_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"btn_title/lbl_kingdomName1");

			m_lbl_kingdomName_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_title/lbl_kingdomName1/lbl_kingdomName");
			m_lbl_kingdomName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_title/lbl_kingdomName1/lbl_kingdomName");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_title/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_title/img_icon");

			m_img_arrowdown_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_title/img_arrowdown");
			m_img_arrowdown_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_title/img_arrowdown");

			m_img_arrowup_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_title/img_arrowup");
			m_img_arrowup_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_title/img_arrowup");

			m_lbl_kingdomNum_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_title/lbl_kingdomNum");
			m_lbl_kingdomNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_title/lbl_kingdomNum");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}