// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BookGroup_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_BookGroup_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BookGroup";

        public UI_Item_BookGroup_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BookGroup;
		[HideInInspector] public GameToggle m_ck_languageCheckBox_GameToggle;



        private void UIFinder()
        {       
			m_UI_Item_BookGroup = gameObject.GetComponent<RectTransform>();
			m_ck_languageCheckBox_GameToggle = FindUI<GameToggle>(gameObject.transform ,"ck_languageCheckBox");


			BindEvent();
        }

        #endregion
    }
}