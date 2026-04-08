// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildUpGoTime_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_BuildUpGoTime_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildUpGoTime";

        public UI_Item_BuildUpGoTime_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BuildUpGoTime;
		[HideInInspector] public GameToggle m_ck_toogle_GameToggle;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_BuildUpGoTime = gameObject.GetComponent<RectTransform>();
			m_ck_toogle_GameToggle = FindUI<GameToggle>(gameObject.transform ,"ck_toogle");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"ck_toogle/lbl_time");


			BindEvent();
        }

        #endregion
    }
}