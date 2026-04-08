// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_TrainSoldierSkill_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_TrainSoldierSkill_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_TrainSoldierSkill";

        public UI_Model_TrainSoldierSkill_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_TrainSoldierSkill;
		[HideInInspector] public PolygonImage m_btn_bottom_PolygonImage;
		[HideInInspector] public GameButton m_btn_bottom_GameButton;
		[HideInInspector] public BtnAnimation m_btn_bottom_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_TrainSoldierSkill = gameObject.GetComponent<RectTransform>();
			m_btn_bottom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bottom");
			m_btn_bottom_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_bottom");
			m_btn_bottom_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_bottom");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bottom/img_icon");


			BindEvent();
        }

        #endregion
    }
}