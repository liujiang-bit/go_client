// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailBattleLogContent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailBattleLogContent_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailBattleLogContent";

        public UI_Item_MailBattleLogContent_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailBattleLogContent;
		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_MailBattleLogContent = gameObject.GetComponent<RectTransform>();
			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_desc");


			BindEvent();
        }

        #endregion
    }
}