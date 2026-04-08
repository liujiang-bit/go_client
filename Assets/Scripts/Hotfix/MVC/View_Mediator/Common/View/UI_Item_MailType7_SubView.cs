// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType7_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType7_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType7";

        public UI_Item_MailType7_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailType7_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public HrefText m_lbl_mes_LinkImageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_node1;
		[HideInInspector] public LanguageText m_lbl_build_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_build_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public RectTransform m_pl_node2;
		[HideInInspector] public HorizontalLayoutGroup m_pl_res_root_HorizontalLayoutGroup;

		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res1;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res2;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res3;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res4;
		[HideInInspector] public UI_Model_GuildRes_SubView m_pl_res5;


        private void UIFinder()
        {       
			m_UI_Item_MailType7_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_lbl_mes_LinkImageText = FindUI<HrefText>(gameObject.transform ,"lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_mes");

			m_pl_node1 = FindUI<RectTransform>(gameObject.transform ,"pl_node1");
			m_lbl_build_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_node1/line0/lbl_build");
			m_lbl_build_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_node1/line0/lbl_build");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_node1/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_node1/lbl_name");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_node1/UI_PlayerHead"));
			m_pl_node2 = FindUI<RectTransform>(gameObject.transform ,"pl_node2");
			m_pl_res_root_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_node2/pl_res_root");

			m_pl_res1 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_node2/pl_res_root/pl_res1"));
			m_pl_res2 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_node2/pl_res_root/pl_res2"));
			m_pl_res3 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_node2/pl_res_root/pl_res3"));
			m_pl_res4 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_node2/pl_res_root/pl_res4"));
			m_pl_res5 = new UI_Model_GuildRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_node2/pl_res_root/pl_res5"));

			BindEvent();
        }

        #endregion
    }
}