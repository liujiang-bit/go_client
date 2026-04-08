// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, March 9, 2020
// Update Time         :    Monday, March 9, 2020
// Class Description   :    CreateCharFlagItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class CreateCharFlagItemView : GameView
    {
		public const string VIEW_NAME = "CreateCharFlagItem";

        public CreateCharFlagItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_selectBox_PolygonImage;
		[HideInInspector] public Animator m_img_selectBox_Animator;

		[HideInInspector] public PolygonImage m_btn_selectCountry_PolygonImage;
		[HideInInspector] public GameButton m_btn_selectCountry_GameButton;

		[HideInInspector] public RectTransform m_pl_SelectPos;
		[HideInInspector] public RectTransform m_pl_unSelectPos;
		[HideInInspector] public LanguageText m_lbl_countryButtomName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_selectName_LanguageText;

		[HideInInspector] public PolygonImage m_img_black_PolygonImage;

		[HideInInspector] public RectTransform m_pl_finderPos;
		[HideInInspector] public RectTransform m_pl_finderUIPos;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_selectBox_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_selectBox");
			m_img_selectBox_Animator = FindUI<Animator>(vb.transform ,"img_selectBox");

			m_btn_selectCountry_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_selectCountry");
			m_btn_selectCountry_GameButton = FindUI<GameButton>(vb.transform ,"btn_selectCountry");

			m_pl_SelectPos = FindUI<RectTransform>(vb.transform ,"pl_SelectPos");
			m_pl_unSelectPos = FindUI<RectTransform>(vb.transform ,"pl_unSelectPos");
			m_lbl_countryButtomName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_countryButtomName");

			m_lbl_selectName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_selectName");

			m_img_black_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_black");

			m_pl_finderPos = FindUI<RectTransform>(vb.transform ,"pl_finderPos");
			m_pl_finderUIPos = FindUI<RectTransform>(vb.transform ,"pl_finderUIPos");


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}