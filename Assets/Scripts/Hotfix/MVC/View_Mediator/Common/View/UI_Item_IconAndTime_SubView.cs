// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_IconAndTime_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_IconAndTime_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_IconAndTime";

        public UI_Item_IconAndTime_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_IconAndTime;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_timebg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_btn_timebg_ArabLayoutCompment;
		[HideInInspector] public GameButton m_btn_timebg_GameButton;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public PolygonImage m_img_timeIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_timeIcon_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_IconAndTime = gameObject.GetComponent<RectTransform>();
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_btn_timebg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon/btn_timebg");
			m_btn_timebg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon/btn_timebg");
			m_btn_timebg_GameButton = FindUI<GameButton>(gameObject.transform ,"img_icon/btn_timebg");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_icon/btn_timebg/lbl_time");

			m_img_timeIcon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon/btn_timebg/lbl_time/img_timeIcon");
			m_img_timeIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon/btn_timebg/lbl_time/img_timeIcon");


			BindEvent();
        }

        #endregion
    }
}