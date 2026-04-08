// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月21日
// Update Time         :    2020年9月21日
// Class Description   :    ItemTrainUpView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class ItemTrainUpView : GameView
    {
		public const string VIEW_NAME = "UI_Item_TrainUp";

        public ItemTrainUpView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_headAfter_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_numAfter_LanguageText;

		[HideInInspector] public PolygonImage m_img_headBefore_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_numBefore_LanguageText;

		[HideInInspector] public RectTransform m_pl_upgrade_cost;
		[HideInInspector] public UI_Model_TrainResCost_SubView m_UI_Model_UpResCost;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue_SubView m_UI_btn_Uptrain;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_btn_Upcomplete;
		[HideInInspector] public RectTransform m_pl_num;
		[HideInInspector] public LanguageText m_lbl_num_up_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_up_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_last_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_last_num_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_headAfter_PolygonImage = FindUI<PolygonImage>(vb.transform ,"up/headAfter/img_headAfter");

			m_lbl_numAfter_LanguageText = FindUI<LanguageText>(vb.transform ,"up/headAfter/lbl_numAfter");

			m_img_headBefore_PolygonImage = FindUI<PolygonImage>(vb.transform ,"up/headBefore/img_headBefore");

			m_lbl_numBefore_LanguageText = FindUI<LanguageText>(vb.transform ,"up/headBefore/lbl_numBefore");

			m_pl_upgrade_cost = FindUI<RectTransform>(vb.transform ,"pl_upgrade_cost");
			m_UI_Model_UpResCost = new UI_Model_TrainResCost_SubView(FindUI<RectTransform>(vb.transform ,"pl_upgrade_cost/UI_Model_UpResCost"));
			m_UI_btn_Uptrain = new UI_Model_DoubleLineButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_upgrade_cost/UI_btn_Uptrain"));
			m_UI_btn_Upcomplete = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_upgrade_cost/UI_btn_Upcomplete"));
			m_pl_num = FindUI<RectTransform>(vb.transform ,"pl_num");
			m_lbl_num_up_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_num/lbl_num_up");
			m_lbl_num_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_num/lbl_num_up");

			m_lbl_last_num_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_num/lbl_last_num");
			m_lbl_last_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_num/lbl_last_num");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}