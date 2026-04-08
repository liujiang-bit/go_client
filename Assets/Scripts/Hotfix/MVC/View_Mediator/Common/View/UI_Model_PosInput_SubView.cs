// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_PosInput_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_PosInput_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_PosInput";

        public UI_Model_PosInput_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_PosInput;
		[HideInInspector] public LanguageText m_lbl_key_LanguageText;

		[HideInInspector] public PolygonImage m_ipt_val_PolygonImage;
		[HideInInspector] public GameInput m_ipt_val_GameInput;



        private void UIFinder()
        {       
			m_UI_Model_PosInput = gameObject.GetComponent<RectTransform>();
			m_lbl_key_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_key");

			m_ipt_val_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ipt_val");
			m_ipt_val_GameInput = FindUI<GameInput>(gameObject.transform ,"ipt_val");


			BindEvent();
        }

        #endregion
    }
}