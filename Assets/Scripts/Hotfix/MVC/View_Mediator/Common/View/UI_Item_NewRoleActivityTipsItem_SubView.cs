// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_NewRoleActivityTipsItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_NewRoleActivityTipsItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_NewRoleActivityTipsItem";

        public UI_Item_NewRoleActivityTipsItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_NewRoleActivityTipsItem;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet;
		[HideInInspector] public ArabLayoutCompment m_pl_target_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_target_cur_LanguageText;

		[HideInInspector] public LanguageText m_lbl_day_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_day_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_NewRoleActivityTipsItem = gameObject.GetComponent<RectTransform>();
			m_UI_Model_RewardGet = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_RewardGet"));
			m_pl_target_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_target");

			m_lbl_target_cur_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_target/lbl_target_cur");

			m_lbl_day_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_day");
			m_lbl_day_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_day");


			BindEvent();
        }

        #endregion
    }
}