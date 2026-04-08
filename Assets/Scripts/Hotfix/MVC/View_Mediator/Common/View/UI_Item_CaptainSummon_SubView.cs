// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainSummon_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_CaptainSummon_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainSummon";

        public UI_Item_CaptainSummon_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainSummon;
		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_count_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public PolygonImage m_img_summon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_summon_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_CaptainSummon = gameObject.GetComponent<RectTransform>();
			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_ClickAnimeMsg_btn"));
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));
			m_img_count_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_count");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_count/lbl_count");

			m_img_summon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_summon");

			m_lbl_summon_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_summon/lbl_summon");


			BindEvent();
        }

        #endregion
    }
}