// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_Scout_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_Scout_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_Scout";

        public UI_Item_Scout_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_Scout;
		[HideInInspector] public UI_Model_Link_SubView m_pl_Link;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_state_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_btn_Yellow;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue_SubView m_UI_btn_Blue;


        private void UIFinder()
        {       
			m_UI_Item_Scout = gameObject.GetComponent<RectTransform>();
			m_pl_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"icon/pl_Link"));
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"hero/UI_Model_CaptainHead"));
			m_img_state_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"hero/img_state");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"hero/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"hero/lbl_desc");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"hero/pb_rogressBar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"hero/pb_rogressBar/lbl_barText");

			m_UI_btn_Yellow = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_btn_Yellow"));
			m_UI_btn_Blue = new UI_Model_DoubleLineButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_btn_Blue"));

			BindEvent();
        }

        #endregion
    }
}