// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjInfoTCollect_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjInfoTCollect_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjInfoTCollect";

        public UI_Item_WorldObjInfoTCollect_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WorldObjInfoTCollect;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line1;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line2;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line3;
		[HideInInspector] public GameSlider m_pb_bar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_bar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public LanguageText m_lbl_count_diamond_LanguageText;

		[HideInInspector] public UI_Item_IconAndTime_SubView m_UI_Item_IconAndTime;


        private void UIFinder()
        {       
			m_UI_Item_WorldObjInfoTCollect = gameObject.GetComponent<RectTransform>();
			m_UI_Item_line1 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line1"));
			m_UI_Item_line2 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line2"));
			m_UI_Item_line3 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line3"));
			m_pb_bar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"bar/pb_bar");
			m_pb_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bar/pb_bar");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bar/pb_bar/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bar/pb_bar/img_icon");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bar/pb_bar/lbl_time");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bar/pb_bar/lbl_count");

			m_lbl_count_diamond_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bar/pb_bar/lbl_count_diamond");

			m_UI_Item_IconAndTime = new UI_Item_IconAndTime_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_IconAndTime"));

			BindEvent();
        }

        #endregion
    }
}