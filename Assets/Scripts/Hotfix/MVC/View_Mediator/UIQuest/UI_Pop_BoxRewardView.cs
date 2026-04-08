// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月22日
// Update Time         :    2020年7月22日
// Class Description   :    UI_Pop_BoxRewardView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_BoxRewardView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_BoxReward";

        public UI_Pop_BoxRewardView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public VerticalLayoutGroup m_pl_info_VerticalLayoutGroup;
		[HideInInspector] public GrayChildrens m_pl_info_MakeChildrenGray;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_dec_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_boxTips_GridLayoutGroup;

		[HideInInspector] public UI_Item_BoxTipsItem_SubView m_UI_Item_BoxTipsItem;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_info_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"pl_info");
			m_pl_info_MakeChildrenGray = FindUI<GrayChildrens>(vb.transform ,"pl_info");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_info/lbl_dec");
			m_lbl_dec_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_info/lbl_dec");
			m_lbl_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_info/lbl_dec");

			m_pl_boxTips_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_info/pl_boxTips");

			m_UI_Item_BoxTipsItem = new UI_Item_BoxTipsItem_SubView(FindUI<RectTransform>(vb.transform ,"pl_info/pl_boxTips/UI_Item_BoxTipsItem"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}