// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType19List_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType19List_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType19List";

        public UI_Item_MailType19List_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailType19List;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public GridLayoutGroup m_pl_rewards_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_rewards_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item5;


        private void UIFinder()
        {       
			m_UI_Item_MailType19List = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_pl_rewards_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_rewards");
			m_pl_rewards_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rewards");

			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rewards/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rewards/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rewards/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rewards/UI_Model_Item4"));
			m_UI_Model_Item5 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rewards/UI_Model_Item5"));

			BindEvent();
        }

        #endregion
    }
}