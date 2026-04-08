// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ServerItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ServerItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ServerItem";

        public UI_Item_ServerItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ServerItem;
		[HideInInspector] public PolygonImage m_btn_kingdom_PolygonImage;
		[HideInInspector] public GameButton m_btn_kingdom_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_kingdom_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;
		[HideInInspector] public LanguageText m_lbl_kingdomName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kingdomNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomNum_ArabLayoutCompment;

		[HideInInspector] public HorizontalLayoutGroup m_pl_view_HorizontalLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_view_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ServerItem = gameObject.GetComponent<RectTransform>();
			m_btn_kingdom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_kingdom");
			m_btn_kingdom_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_kingdom");
			m_btn_kingdom_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_kingdom");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_kingdom/UI_Common_Redpoint"));
			m_lbl_kingdomName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_kingdom/lbl_kingdomName");
			m_lbl_kingdomName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_kingdom/lbl_kingdomName");

			m_lbl_kingdomNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_kingdom/lbl_kingdomNum");
			m_lbl_kingdomNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_kingdom/lbl_kingdomNum");

			m_pl_view_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"btn_kingdom/pl_view");
			m_pl_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_kingdom/pl_view");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_kingdom/pl_view/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_kingdom/pl_view/lbl_time");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_kingdom/pl_view/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_kingdom/pl_view/img_icon");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_kingdom/pl_view/img_icon/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_kingdom/pl_view/img_icon/lbl_num");


			BindEvent();
        }

        #endregion
    }
}