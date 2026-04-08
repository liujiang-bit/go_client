// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月8日
// Update Time         :    2020年1月8日
// Class Description   :    UI_Pop_WorldObjectLodView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_WorldObjectLodView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_WorldObjectLod";

        public UI_Pop_WorldObjectLodView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_nohead_PolygonImage;

		[HideInInspector] public RectTransform m_pl_head;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_stateicon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_resfood_PolygonImage;

		[HideInInspector] public PolygonImage m_img_reswood_PolygonImage;

		[HideInInspector] public PolygonImage m_img_resgold_PolygonImage;

		[HideInInspector] public PolygonImage m_img_resgem_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_lod2lv_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_nohead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"state/img_nohead");

			m_pl_head = FindUI<RectTransform>(vb.transform ,"state/pl_head");
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"state/pl_head/UI_Model_CaptainHead"));
			m_img_stateicon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"state/pl_head/img_stateicon");

			m_img_resfood_PolygonImage = FindUI<PolygonImage>(vb.transform ,"res/img_resfood");

			m_img_reswood_PolygonImage = FindUI<PolygonImage>(vb.transform ,"res/img_reswood");

			m_img_resgold_PolygonImage = FindUI<PolygonImage>(vb.transform ,"res/img_resgold");

			m_img_resgem_PolygonImage = FindUI<PolygonImage>(vb.transform ,"res/img_resgem");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"level/img_bg");

			m_lbl_lod2lv_LanguageText = FindUI<LanguageText>(vb.transform ,"level/img_bg/lbl_lod2lv");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}