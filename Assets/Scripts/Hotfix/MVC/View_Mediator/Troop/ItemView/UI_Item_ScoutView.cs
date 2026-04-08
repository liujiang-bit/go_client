// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月17日
// Update Time         :    2020年3月17日
// Class Description   :    UI_Item_ScoutView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ScoutView : GameView
    {
		public const string VIEW_NAME = "UI_Item_Scout";

        public UI_Item_ScoutView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_Link_SubView m_pl_Link;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_Background_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_Background_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;
		[HideInInspector] public Shadow m_lbl_barText_Shadow;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_state_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_state_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_btn_Yellow;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue_SubView m_UI_btn_Blue;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"icon/pl_Link"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"hero/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"hero/lbl_name");

			m_img_Background_PolygonImage = FindUI<PolygonImage>(vb.transform ,"hero/img_Background");
			m_img_Background_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"hero/img_Background");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"hero/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"hero/lbl_desc");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"hero/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"hero/pb_rogressBar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(vb.transform ,"hero/pb_rogressBar/lbl_barText");
			m_lbl_barText_Shadow = FindUI<Shadow>(vb.transform ,"hero/pb_rogressBar/lbl_barText");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"hero/UI_Model_CaptainHead"));
			m_img_state_PolygonImage = FindUI<PolygonImage>(vb.transform ,"hero/img_state");
			m_img_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"hero/img_state");

			m_UI_btn_Yellow = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"UI_btn_Yellow"));
			m_UI_btn_Blue = new UI_Model_DoubleLineButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_btn_Blue"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}