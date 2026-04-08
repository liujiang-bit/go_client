// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月28日
// Update Time         :    2020年7月28日
// Class Description   :    UI_Pop_UseAddItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_UseAddItemView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_UseAddItem";

        public UI_Pop_UseAddItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_pl_bg_GridLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_bg_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_pl_bg_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_right;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_left;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_bg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_bg");
			m_pl_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_bg");
			m_pl_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_bg");

			m_UI_right = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_right"));
			m_UI_left = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_left"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}