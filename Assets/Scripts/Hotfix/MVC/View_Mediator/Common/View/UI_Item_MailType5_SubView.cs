// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType5_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType5_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType5";

        public UI_Item_MailType5_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailType5_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_PolygonImage;
		[HideInInspector] public Mask m_v_list_Mask;

		[HideInInspector] public RectTransform m_c_list;
		[HideInInspector] public VerticalLayoutGroup m_UI_Item_MailType5Item_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_mes_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_check;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;


        private void UIFinder()
        {       
			m_UI_Item_MailType5_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"sv_list");

			m_v_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_list/v_list");
			m_v_list_Mask = FindUI<Mask>(gameObject.transform ,"sv_list/v_list");

			m_c_list = FindUI<RectTransform>(gameObject.transform ,"sv_list/v_list/c_list");
			m_UI_Item_MailType5Item_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item/lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item/lbl_mes");
			m_lbl_mes_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item/lbl_mes");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item/player/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item/player/lbl_name");

			m_btn_check = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item/player/btn_check"));
			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"sv_list/v_list/c_list/UI_Item_MailType5Item/player/UI_Model_PlayerHead"));

			BindEvent();
        }

        #endregion
    }
}