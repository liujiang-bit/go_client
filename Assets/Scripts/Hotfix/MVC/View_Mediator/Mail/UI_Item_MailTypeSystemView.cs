// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_Item_MailTypeSystemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_MailTypeSystemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_MailTypeSystem";

        public UI_Item_MailTypeSystemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public LanguageText m_lbl_Content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_Content_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_Content_ArabLayoutCompment;

		[HideInInspector] public HrefText m_lbl_Content_link_LinkImageText;
		[HideInInspector] public ContentSizeFitter m_lbl_Content_link_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_Content_link_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_Sender_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_Sender_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_Sender_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_gift_GridLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_gift_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_pl_gift_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_btn;
		[HideInInspector] public LanguageText m_pl_already_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_receive;
		[HideInInspector] public GridLayoutGroup m_pl_res_GridLayoutGroup;

		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes1;
		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes2;
		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes3;
		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes4;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_MailTitle"));
			m_lbl_Content_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_Content");
			m_lbl_Content_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"lbl_Content");
			m_lbl_Content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_Content");

			m_lbl_Content_link_LinkImageText = FindUI<HrefText>(vb.transform ,"lbl_Content_link");
			m_lbl_Content_link_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"lbl_Content_link");
			m_lbl_Content_link_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_Content_link");

			m_lbl_Sender_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_Sender");
			m_lbl_Sender_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"lbl_Sender");
			m_lbl_Sender_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_Sender");

			m_pl_gift_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_gift");
			m_pl_gift_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_gift");
			m_pl_gift_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_gift");

			m_pl_btn = FindUI<RectTransform>(vb.transform ,"pl_btn");
			m_pl_already_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_btn/pl_already");

			m_btn_receive = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_btn/btn_receive"));
			m_pl_res_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_res");

			m_UI_Item_AssistanceRes1 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/UI_Item_AssistanceRes1"));
			m_UI_Item_AssistanceRes2 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/UI_Item_AssistanceRes2"));
			m_UI_Item_AssistanceRes3 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/UI_Item_AssistanceRes3"));
			m_UI_Item_AssistanceRes4 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/UI_Item_AssistanceRes4"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}