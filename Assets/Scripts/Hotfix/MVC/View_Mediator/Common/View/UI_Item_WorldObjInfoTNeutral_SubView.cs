// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjInfoTNeutral_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjInfoTNeutral_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjInfoTNeutral";

        public UI_Item_WorldObjInfoTNeutral_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WorldObjInfoTNeutral;
		[HideInInspector] public RectTransform m_pl_titleOwner;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line1;
		[HideInInspector] public RectTransform m_pl_titleEffect;
		[HideInInspector] public RectTransform m_pl_effectLevel;
		[HideInInspector] public LanguageText m_lbl_levelDesc_LanguageText;

		[HideInInspector] public PolygonImage m_pl_buff_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_pl_buff_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_buffDesc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffDesc_ArabLayoutCompment;

		[HideInInspector] public UI_Item_IconAndTime_SubView m_UI_Item_IconAndTime;


        private void UIFinder()
        {       
			m_UI_Item_WorldObjInfoTNeutral = gameObject.GetComponent<RectTransform>();
			m_pl_titleOwner = FindUI<RectTransform>(gameObject.transform ,"textLayer/pl_titleOwner");
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"textLayer/pl_titleOwner/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"textLayer/pl_titleOwner/lbl_title");

			m_UI_Item_line1 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line1"));
			m_pl_titleEffect = FindUI<RectTransform>(gameObject.transform ,"textLayer/pl_titleEffect");
			m_pl_effectLevel = FindUI<RectTransform>(gameObject.transform ,"textLayer/pl_effectLevel");
			m_lbl_levelDesc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"textLayer/pl_effectLevel/lbl_levelDesc");

			m_pl_buff_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"textLayer/pl_effectLevel/pl_buff");
			m_pl_buff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"textLayer/pl_effectLevel/pl_buff");

			m_lbl_buffDesc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"textLayer/pl_effectLevel/pl_buff/lbl_buffDesc");
			m_lbl_buffDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"textLayer/pl_effectLevel/pl_buff/lbl_buffDesc");

			m_UI_Item_IconAndTime = new UI_Item_IconAndTime_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_IconAndTime"));

			BindEvent();
        }

        #endregion
    }
}