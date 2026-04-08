// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GuildRankingBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GuildRankingBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GuildRankingBtn";

        public UI_Item_GuildRankingBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_GuildRankingBtn;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_flag_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_flag_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mid_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mid_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_GuildRankingBtn = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_flag_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_flag");
			m_img_flag_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/img_flag");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_flag/img_icon");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/lbl_title");

			m_lbl_mid_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_mid");
			m_lbl_mid_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/lbl_mid");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/lbl_name");


			BindEvent();
        }

        #endregion
    }
}