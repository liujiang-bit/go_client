// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月31日
// Update Time         :    2020年7月31日
// Class Description   :    UI_Item_CityMgrItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_CityMgrItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_CityMgrItem";

        public UI_Item_CityMgrItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_iconframe_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_iconframe_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_use;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");

			m_img_iconframe_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_iconframe");
			m_img_iconframe_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_iconframe");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_desc");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_arrow");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"btn_btn/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/pb_rogressBar/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/pb_rogressBar/lbl_time");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/pb_rogressBar/lbl_dec");
			m_lbl_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/pb_rogressBar/lbl_dec");

			m_btn_use = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"btn_btn/btn_use"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}