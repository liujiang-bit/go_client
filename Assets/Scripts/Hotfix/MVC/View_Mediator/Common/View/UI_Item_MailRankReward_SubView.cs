// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailRankReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailRankReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailRankReward";

        public UI_Item_MailRankReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailRankReward;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public LanguageText m_lbl_num_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_MailRankReward = gameObject.GetComponent<RectTransform>();
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");


			BindEvent();
        }

        #endregion
    }
}