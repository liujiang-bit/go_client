// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjInfoTDrop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjInfoTDrop_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjInfoTDrop";

        public UI_Item_WorldObjInfoTDrop_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WorldObjInfoTDrop;
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_languageText_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ItemSize65_SubView m_UI_Item_item1;
		[HideInInspector] public UI_Item_ItemSize65_SubView m_UI_Item_item2;
		[HideInInspector] public UI_Item_ItemSize65_SubView m_UI_Item_Item3;
		[HideInInspector] public UI_Item_ItemSize65_SubView m_UI_Item_Item4;
		[HideInInspector] public UI_Item_ItemSize65_SubView m_UI_Item_Item5;
		[HideInInspector] public UI_Item_ItemSize65_SubView m_UI_Item_Item6;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_time_ContentSizeFitter;



        private void UIFinder()
        {       
			m_UI_Item_WorldObjInfoTDrop = gameObject.GetComponent<RectTransform>();
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_languageText");
			m_lbl_languageText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_languageText");

			m_UI_Item_item1 = new UI_Item_ItemSize65_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_item1"));
			m_UI_Item_item2 = new UI_Item_ItemSize65_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_item2"));
			m_UI_Item_Item3 = new UI_Item_ItemSize65_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_Item3"));
			m_UI_Item_Item4 = new UI_Item_ItemSize65_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_Item4"));
			m_UI_Item_Item5 = new UI_Item_ItemSize65_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_Item5"));
			m_UI_Item_Item6 = new UI_Item_ItemSize65_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_Item6"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"timebg/lbl_time");
			m_lbl_time_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"timebg/lbl_time");


			BindEvent();
        }

        #endregion
    }
}