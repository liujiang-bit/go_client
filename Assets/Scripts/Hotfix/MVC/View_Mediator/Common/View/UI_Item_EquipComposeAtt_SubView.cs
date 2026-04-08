// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipComposeAtt_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EquipComposeAtt_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipComposeAtt";

        public UI_Item_EquipComposeAtt_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_UI_Item_EquipComposeAtt_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EquipComposeAtt_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EquipComposeAtt_LanguageText = gameObject.GetComponent<LanguageText>();
			m_UI_Item_EquipComposeAtt_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();


			BindEvent();
        }

        #endregion
    }
}