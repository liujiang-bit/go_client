// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Ck_MainIFLod_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_Ck_MainIFLod_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Ck_MainIFLod";

        public UI_Model_Ck_MainIFLod_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GameToggle m_UI_Model_Ck_MainIFLod_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_UI_Model_Ck_MainIFLod_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_Ck_MainIFLod_GameToggle = gameObject.GetComponent<GameToggle>();
			m_UI_Model_Ck_MainIFLod_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();


			BindEvent();
        }

        #endregion
    }
}