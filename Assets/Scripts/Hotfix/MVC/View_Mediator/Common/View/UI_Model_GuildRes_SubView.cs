// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_GuildRes_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_GuildRes_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_GuildRes";

        public UI_Model_GuildRes_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Model_GuildRes_PolygonImage;
		[HideInInspector] public GameButton m_UI_Model_GuildRes_GameButton;
		[HideInInspector] public BtnAnimation m_UI_Model_GuildRes_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_resicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_resnum_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_GuildRes_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Model_GuildRes_GameButton = gameObject.GetComponent<GameButton>();
			m_UI_Model_GuildRes_ButtonAnimation = gameObject.GetComponent<BtnAnimation>();

			m_img_resicon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_resicon");

			m_lbl_resnum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_resnum");


			BindEvent();
        }

        #endregion
    }
}