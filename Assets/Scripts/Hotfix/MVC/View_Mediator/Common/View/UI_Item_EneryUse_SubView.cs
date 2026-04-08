// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EneryUse_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EneryUse_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EneryUse";

        public UI_Item_EneryUse_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Empty4Raycast m_UI_Item_EneryUse_Empty4Raycast;
		[HideInInspector] public Animation m_UI_Item_EneryUse_Animation;

		[HideInInspector] public LanguageText m_lbl_use_LanguageText;
		[HideInInspector] public Outline m_lbl_use_Outline;



        private void UIFinder()
        {       
			m_UI_Item_EneryUse_Empty4Raycast = gameObject.GetComponent<Empty4Raycast>();
			m_UI_Item_EneryUse_Animation = gameObject.GetComponent<Animation>();

			m_lbl_use_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img/lbl_use");
			m_lbl_use_Outline = FindUI<Outline>(gameObject.transform ,"img/lbl_use");


			BindEvent();
        }

        #endregion
    }
}