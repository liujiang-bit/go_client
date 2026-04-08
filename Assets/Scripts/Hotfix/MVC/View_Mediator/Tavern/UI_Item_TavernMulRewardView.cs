// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月15日
// Update Time         :    2020年4月15日
// Class Description   :    UI_Item_TavernMulRewardView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_TavernMulRewardView : GameView
    {
		public const string VIEW_NAME = "UI_Item_TavernMulReward";

        public UI_Item_TavernMulRewardView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public UI_Model_CaptainHeadBtn_SubView m_UI_Model_CaptainHeadBtn;
		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public Shadow m_lbl_count_Shadow;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item"));
			m_UI_Model_CaptainHeadBtn = new UI_Model_CaptainHeadBtn_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_CaptainHeadBtn"));
			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_count");
			m_lbl_count_Shadow = FindUI<Shadow>(vb.transform ,"lbl_count");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}