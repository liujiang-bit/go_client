// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType16Soldiers_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType16Soldiers_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType16Soldiers";

        public UI_Item_MailType16Soldiers_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_MailType16Soldiers_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_MailType16Soldiers_ArabLayoutCompment;

		[HideInInspector] public UI_Item_SoldierHead_SubView m_UI_Item_SoldierHead;


        private void UIFinder()
        {       
			m_UI_Item_MailType16Soldiers_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_MailType16Soldiers_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item_SoldierHead = new UI_Item_SoldierHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_SoldierHead"));

			BindEvent();
        }

        #endregion
    }
}