// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_AliGuide_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_AliGuide_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_AliGuide";

        public UI_Item_AliGuide_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_AliGuide;
		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public UI_10067_SubView m_UI_10067;
		[HideInInspector] public PolygonImage m_btn_aliguide_PolygonImage;
		[HideInInspector] public GameButton m_btn_aliguide_GameButton;
		[HideInInspector] public BtnAnimation m_btn_aliguide_BtnAnimation;



        private void UIFinder()
        {       
			m_UI_Item_AliGuide = gameObject.GetComponent<RectTransform>();
			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");
			m_UI_10067 = new UI_10067_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_effect/UI_10067"));
			m_btn_aliguide_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_aliguide");
			m_btn_aliguide_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_aliguide");
			m_btn_aliguide_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_aliguide");


			BindEvent();
        }

        #endregion
    }
}