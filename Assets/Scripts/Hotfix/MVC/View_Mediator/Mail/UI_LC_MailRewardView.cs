// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月3日
// Update Time         :    2020年3月3日
// Class Description   :    UI_LC_MailRewardView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_LC_MailRewardView : GameView
    {
		public const string VIEW_NAME = "UI_LC_MailReward";

        public UI_LC_MailRewardView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public RectTransform m_pl_cur;
		[HideInInspector] public PolygonImage m_img_iconcur_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_countcur_LanguageText;
		[HideInInspector] public Shadow m_lbl_countcur_Shadow;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public Shadow m_lbl_name_Shadow;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Item"));
			m_pl_cur = FindUI<RectTransform>(vb.transform ,"UI_Model_Item/pl_cur");
			m_img_iconcur_PolygonImage = FindUI<PolygonImage>(vb.transform ,"UI_Model_Item/pl_cur/img_iconcur");

			m_lbl_countcur_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_Item/pl_cur/lbl_countcur");
			m_lbl_countcur_Shadow = FindUI<Shadow>(vb.transform ,"UI_Model_Item/pl_cur/lbl_countcur");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_Item/lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(vb.transform ,"UI_Model_Item/lbl_name");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}