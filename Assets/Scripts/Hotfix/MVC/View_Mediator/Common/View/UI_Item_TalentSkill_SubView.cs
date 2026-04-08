// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TalentSkill_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_TalentSkill_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TalentSkill";

        public UI_Item_TalentSkill_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_TalentSkill;
		[HideInInspector] public Empty4Raycast m_pl_mes_Empty4Raycast;
		[HideInInspector] public GameButton m_pl_mes_GameButton;

		[HideInInspector] public PolygonImage m_img_choose_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_cover_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_TalentSkill = gameObject.GetComponent<RectTransform>();
			m_pl_mes_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_mes");
			m_pl_mes_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes");

			m_img_choose_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_choose");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_icon");

			m_img_cover_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_cover");


			BindEvent();
        }

        #endregion
    }
}