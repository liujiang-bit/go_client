// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_ArmyTrainRes_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_ArmyTrainRes_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_ArmyTrainRes";

        public UI_Model_ArmyTrainRes_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_ArmyTrainRes;
		[HideInInspector] public PolygonImage m_btn_area_PolygonImage;
		[HideInInspector] public GameButton m_btn_area_GameButton;
		[HideInInspector] public BtnAnimation m_btn_area_BtnAnimation;

		[HideInInspector] public PolygonImage m_img_resCost_Icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_resCost_num_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_ArmyTrainRes = gameObject.GetComponent<RectTransform>();
			m_btn_area_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_area");
			m_btn_area_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_area");
			m_btn_area_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_area");

			m_img_resCost_Icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_area/img_resCost_Icon");

			m_lbl_resCost_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_area/lbl_resCost_num");


			BindEvent();
        }

        #endregion
    }
}