// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BoxTipsItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_BoxTipsItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BoxTipsItem";

        public UI_Item_BoxTipsItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BoxTipsItem;
		[HideInInspector] public UI_Model_Item_SubView m_pl_item;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_BoxTipsItem = gameObject.GetComponent<RectTransform>();
			m_pl_item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_item"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_num");


			BindEvent();
        }

        #endregion
    }
}