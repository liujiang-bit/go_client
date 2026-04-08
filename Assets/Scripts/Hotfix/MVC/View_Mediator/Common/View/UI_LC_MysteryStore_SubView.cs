// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_LC_MysteryStore_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_LC_MysteryStore_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_LC_MysteryStore";

        public UI_LC_MysteryStore_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_LC_MysteryStore;
		[HideInInspector] public LanguageText m_lbl_lineText_LanguageText;

		[HideInInspector] public UI_Model_MysticStoreItem_SubView m_img_shoping1;
		[HideInInspector] public UI_Model_MysticStoreItem_SubView m_img_shoping2;
		[HideInInspector] public UI_Model_MysticStoreItem_SubView m_img_shoping3;
		[HideInInspector] public UI_Model_MysticStoreItem_SubView m_img_shoping4;


        private void UIFinder()
        {       
			m_UI_LC_MysteryStore = gameObject.GetComponent<RectTransform>();
			m_lbl_lineText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_lineText");

			m_img_shoping1 = new UI_Model_MysticStoreItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/img_shoping1"));
			m_img_shoping2 = new UI_Model_MysticStoreItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/img_shoping2"));
			m_img_shoping3 = new UI_Model_MysticStoreItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/img_shoping3"));
			m_img_shoping4 = new UI_Model_MysticStoreItem_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/img_shoping4"));

			BindEvent();
        }

        #endregion
    }
}