// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType19Text_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType19Text_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType19Text";

        public UI_Item_MailType19Text_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailType19Text;
		[HideInInspector] public HrefText m_lbl_mes_link_HrefText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_link_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailType19Text = gameObject.GetComponent<RectTransform>();
			m_lbl_mes_link_HrefText = FindUI<HrefText>(gameObject.transform ,"lbl_mes_link");
			m_lbl_mes_link_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_mes_link");


			BindEvent();
        }

        #endregion
    }
}