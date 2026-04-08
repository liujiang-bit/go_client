// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, July 14, 2020
// Update Time         :    Tuesday, July 14, 2020
// Class Description   :    UI_Item_GuildMessageSubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildMessageSubView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildMessageSub";

        public UI_Item_GuildMessageSubView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_date_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_date_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;
		[HideInInspector] public ArabLayoutCompment m_pl_pos_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_rep_PolygonImage;
		[HideInInspector] public GameButton m_btn_rep_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_rep_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideR_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_messageText_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_messageText_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_messageText_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_messageImage_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_translation_PolygonImage;
		[HideInInspector] public GameButton m_btn_translation_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_translation_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_PlayerHead"));
			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_playerName");
			m_lbl_playerName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_playerName");

			m_lbl_date_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_date");
			m_lbl_date_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_date");

			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos");
			m_pl_pos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos");

			m_btn_rep_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/btn_rep");
			m_btn_rep_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/btn_rep");
			m_btn_rep_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/btn_rep");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/btn_rep/img_arrowSideR");
			m_img_arrowSideR_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/btn_rep/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/btn_rep/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/btn_rep/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/btn_rep/img_arrowSideL");

			m_lbl_messageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/lbl_messageText");
			m_lbl_messageText_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/lbl_messageText");
			m_lbl_messageText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/lbl_messageText");

			m_img_messageImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_messageImage");

			m_btn_translation_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_translation");
			m_btn_translation_GameButton = FindUI<GameButton>(vb.transform ,"btn_translation");
			m_btn_translation_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_translation");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}