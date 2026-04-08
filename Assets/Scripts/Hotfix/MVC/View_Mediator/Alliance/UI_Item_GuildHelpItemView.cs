// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, 05 November 2020
// Update Time         :    Thursday, 05 November 2020
// Class Description   :    UI_Item_GuildHelpItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_GuildHelpItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildHelpItem";

        public UI_Item_GuildHelpItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bgNomal_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgmy_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public ArabLayoutCompment m_pl_effect_ArabLayoutCompment;

		[HideInInspector] public UI_10083_SubView m_UI_10083;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_helpTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_helpTime_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bgNomal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bgNomal");

			m_img_bgmy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bgmy");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_PlayerHead"));
			m_pl_effect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_effect");

			m_UI_10083 = new UI_10083_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_effect/UI_10083"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_desc");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pb_rogressBar/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pb_rogressBar/lbl_time");

			m_lbl_helpTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pb_rogressBar/lbl_helpTime");
			m_lbl_helpTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pb_rogressBar/lbl_helpTime");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}