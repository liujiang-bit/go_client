// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月21日
// Update Time         :    2020年4月21日
// Class Description   :    UI_Item_PlayerHeadSelectView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_PlayerHeadSelectView : GameView
    {
		public const string VIEW_NAME = "UI_Item_PlayerHeadSelect";

        public UI_Item_PlayerHeadSelectView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_img_using_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_using_LanguageText;

		[HideInInspector] public PolygonImage m_img_lock_PolygonImage;

		[HideInInspector] public PolygonImage m_img_selet_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_click_PolygonImage;
		[HideInInspector] public GameButton m_btn_click_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_PlayerHead"));
			m_img_using_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_using");

			m_lbl_using_LanguageText = FindUI<LanguageText>(vb.transform ,"img_using/lbl_using");

			m_img_lock_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_lock");

			m_img_selet_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_selet");

			m_btn_click_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_click");
			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"btn_click");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}