// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTurntablePro_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EventTurntablePro_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTurntablePro";

        public UI_Item_EventTurntablePro_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTurntablePro;
		[HideInInspector] public LanguageText m_lbl_itemName_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_itemName_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemNum_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_itemNum_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemNum_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_prob_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_prob_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventTurntablePro = gameObject.GetComponent<RectTransform>();
			m_lbl_itemName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_itemName");
			m_lbl_itemName_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_itemName");
			m_lbl_itemName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_itemName");

			m_lbl_itemNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_itemName/lbl_itemNum");
			m_lbl_itemNum_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_itemName/lbl_itemNum");
			m_lbl_itemNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_itemName/lbl_itemNum");

			m_lbl_prob_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_prob");
			m_lbl_prob_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_prob");


			BindEvent();
        }

        #endregion
    }
}