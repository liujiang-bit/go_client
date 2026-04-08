// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, July 14, 2020
// Update Time         :    Tuesday, July 14, 2020
// Class Description   :    UI_Item_GuildMemR4View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildMemR4View : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildMemR4";

        public UI_Item_GuildMemR4View () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GameButton m_btn_rect_GameButton;
		[HideInInspector] public PolygonImage m_btn_rect_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_PlayerHead_PolygonImage;
		[HideInInspector] public GameButton m_btn_PlayerHead_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_PlayerHead_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_pow_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_pow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_job_PolygonImage;
		[HideInInspector] public GameButton m_btn_job_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_job_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_online_PolygonImage;
		[HideInInspector] public GameButton m_btn_online_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_online_ArabLayoutCompment;

		[HideInInspector] public GameButton m_btn_office_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_office_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_office_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_office_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_rect_GameButton = FindUI<GameButton>(vb.transform ,"btn_rect");
			m_btn_rect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_rect");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_rect/UI_PlayerHead"));
			m_btn_PlayerHead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_rect/btn_PlayerHead");
			m_btn_PlayerHead_GameButton = FindUI<GameButton>(vb.transform ,"btn_rect/btn_PlayerHead");
			m_btn_PlayerHead_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_rect/btn_PlayerHead");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_rect/lbl_name");

			m_lbl_pow_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_rect/lbl_pow");
			m_lbl_pow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_rect/lbl_pow");

			m_btn_job_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_rect/btn_job");
			m_btn_job_GameButton = FindUI<GameButton>(vb.transform ,"btn_rect/btn_job");
			m_btn_job_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_rect/btn_job");

			m_btn_online_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_rect/btn_online");
			m_btn_online_GameButton = FindUI<GameButton>(vb.transform ,"btn_rect/btn_online");
			m_btn_online_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_rect/btn_online");

			m_btn_office_GameButton = FindUI<GameButton>(vb.transform ,"btn_rect/btn_office");
			m_btn_office_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_rect/btn_office");

			m_img_office_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_rect/btn_office/img_office");
			m_img_office_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_rect/btn_office/img_office");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}