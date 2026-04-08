// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_StrongerPlayerRank_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_StrongerPlayerRank_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_StrongerPlayerRank";

        public UI_Item_StrongerPlayerRank_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_StrongerPlayerRank;
		[HideInInspector] public PolygonImage m_img_title_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_item_VerticalLayoutGroup;

		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem1;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem2;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem3;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem4;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem5;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem6;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem7;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem8;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem9;
		[HideInInspector] public UI_Item_EventTypeRankItem_SubView m_UI_Item_EventTypeRankItem10;


        private void UIFinder()
        {       
			m_UI_Item_StrongerPlayerRank = gameObject.GetComponent<RectTransform>();
			m_img_title_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_title");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_time");

			m_pl_item_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"rect/pl_item");

			m_UI_Item_EventTypeRankItem1 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem1"));
			m_UI_Item_EventTypeRankItem2 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem2"));
			m_UI_Item_EventTypeRankItem3 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem3"));
			m_UI_Item_EventTypeRankItem4 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem4"));
			m_UI_Item_EventTypeRankItem5 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem5"));
			m_UI_Item_EventTypeRankItem6 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem6"));
			m_UI_Item_EventTypeRankItem7 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem7"));
			m_UI_Item_EventTypeRankItem8 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem8"));
			m_UI_Item_EventTypeRankItem9 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem9"));
			m_UI_Item_EventTypeRankItem10 = new UI_Item_EventTypeRankItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/pl_item/UI_Item_EventTypeRankItem10"));

			BindEvent();
        }

        #endregion
    }
}