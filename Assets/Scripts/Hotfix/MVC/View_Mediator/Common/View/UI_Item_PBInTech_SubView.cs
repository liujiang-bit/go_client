// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_PBInTech_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_PBInTech_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_PBInTech";

        public UI_Item_PBInTech_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_PBInTech;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_stop_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_stop_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_stopOverSize_PolygonImage;
		[HideInInspector] public GameButton m_btn_stopOverSize_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_PBInTech = gameObject.GetComponent<RectTransform>();
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pb_rogressBar/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pb_rogressBar/lbl_time");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_img_stop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon/img_stop");
			m_img_stop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon/img_stop");

			m_btn_stopOverSize_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon/btn_stopOverSize");
			m_btn_stopOverSize_GameButton = FindUI<GameButton>(gameObject.transform ,"img_icon/btn_stopOverSize");


			BindEvent();
        }

        #endregion
    }
}