// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailWarKill_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailWarKill_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailWarKill";

        public UI_Item_MailWarKill_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailWarKill;
		[HideInInspector] public GridLayoutGroup m_pl_getreward_GridLayoutGroup;



        private void UIFinder()
        {       
			m_UI_Item_MailWarKill = gameObject.GetComponent<RectTransform>();
			m_pl_getreward_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_getreward");


			BindEvent();
        }

        #endregion
    }
}