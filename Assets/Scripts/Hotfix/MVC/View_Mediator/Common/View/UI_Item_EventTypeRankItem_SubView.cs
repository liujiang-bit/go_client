// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTypeRankItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTypeRankItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTypeRankItem";

        public UI_Item_EventTypeRankItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTypeRankItem;
		[HideInInspector] public PolygonImage m_img_rank_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_rank_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_rank_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rank_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_source_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_source_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_Model_GuildFlag;


        private void UIFinder()
        {       
			m_UI_Item_EventTypeRankItem = gameObject.GetComponent<RectTransform>();
			m_img_rank_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_rank");
			m_img_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_rank");

			m_lbl_rank_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_rank");
			m_lbl_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_rank");

			m_lbl_source_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_source");
			m_lbl_source_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_source");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_PlayerHead"));
			m_UI_Model_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_GuildFlag"));

			BindEvent();
        }

        #endregion
    }
}