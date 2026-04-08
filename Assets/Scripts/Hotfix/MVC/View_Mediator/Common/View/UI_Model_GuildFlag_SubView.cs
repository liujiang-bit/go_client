// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_GuildFlag_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_GuildFlag_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_GuildFlag";

        public UI_Model_GuildFlag_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_GuildFlag;
		[HideInInspector] public PolygonImage m_pl_flagEffect_PolygonImage;

		[HideInInspector] public PolygonImage m_img_flag_PolygonImage;

		[HideInInspector] public PolygonImage m_img_flag_noali_PolygonImage;

		[HideInInspector] public PolygonImage m_img_flagBigIcon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_flagIcon_PolygonImage;

		[HideInInspector] public GameButton m_btn_flag_GameButton;
		[HideInInspector] public PolygonImage m_btn_flag_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_GuildFlag = gameObject.GetComponent<RectTransform>();
			m_pl_flagEffect_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_flagEffect");

			m_img_flag_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_flag");

			m_img_flag_noali_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_flag_noali");

			m_img_flagBigIcon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_flagBigIcon");

			m_img_flagIcon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_flagIcon");

			m_btn_flag_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_flag");
			m_btn_flag_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_flag");


			BindEvent();
        }

        #endregion
    }
}