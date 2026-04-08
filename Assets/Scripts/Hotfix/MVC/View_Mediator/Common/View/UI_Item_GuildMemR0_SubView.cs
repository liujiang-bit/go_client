// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GuildMemR0_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GuildMemR0_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GuildMemR0";

        public UI_Item_GuildMemR0_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_GuildMemR0_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_bg_PolygonImage;
		[HideInInspector] public GameButton m_btn_bg_GameButton;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_PlayerHead_PolygonImage;
		[HideInInspector] public GameButton m_btn_PlayerHead_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_PlayerHead_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_pow_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_pow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_online_PolygonImage;
		[HideInInspector] public GameButton m_btn_online_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_online_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_GuildMemR0_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bg");
			m_btn_bg_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_bg");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_bg/UI_PlayerHead"));
			m_btn_PlayerHead_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bg/btn_PlayerHead");
			m_btn_PlayerHead_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_bg/btn_PlayerHead");
			m_btn_PlayerHead_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_bg/btn_PlayerHead");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_bg/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_bg/lbl_name");

			m_lbl_pow_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_bg/lbl_pow");
			m_lbl_pow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_bg/lbl_pow");

			m_btn_online_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bg/btn_online");
			m_btn_online_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_bg/btn_online");
			m_btn_online_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_bg/btn_online");


			BindEvent();
        }

        #endregion
    }
}