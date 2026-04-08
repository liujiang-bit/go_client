// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GuildDepotRes_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GuildDepotRes_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GuildDepotRes";

        public UI_Item_GuildDepotRes_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Empty4Raycast m_UI_Item_GuildDepotRes_Empty4Raycast;

		[HideInInspector] public LanguageText m_lbl_timeNum_LanguageText;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_GuildDepotRes_Empty4Raycast = gameObject.GetComponent<Empty4Raycast>();

			m_lbl_timeNum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_timeNum");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");


			BindEvent();
        }

        #endregion
    }
}