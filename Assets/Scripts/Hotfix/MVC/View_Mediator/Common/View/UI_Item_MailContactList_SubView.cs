// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailContactList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailContactList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailContactList";

        public UI_Item_MailContactList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_MailContactList_GridLayoutGroup;
		[HideInInspector] public ViewBinder m_UI_Item_MailContactList_ViewBinder;

		[HideInInspector] public UI_Item_MailContact_SubView m_UI_Item_MailContact1;
		[HideInInspector] public UI_Item_MailContact_SubView m_UI_Item_MailContact2;


        private void UIFinder()
        {       
			m_UI_Item_MailContactList_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_MailContactList_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailContact1 = new UI_Item_MailContact_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailContact1"));
			m_UI_Item_MailContact2 = new UI_Item_MailContact_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailContact2"));

			BindEvent();
        }

        #endregion
    }
}