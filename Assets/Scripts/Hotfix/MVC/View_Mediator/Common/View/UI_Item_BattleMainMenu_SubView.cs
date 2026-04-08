// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BattleMainMenu_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_BattleMainMenu_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BattleMainMenu";

        public UI_Item_BattleMainMenu_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BattleMainMenu;
		[HideInInspector] public PolygonImage m_btn_menu_PolygonImage;
		[HideInInspector] public GameButton m_btn_menu_GameButton;

		[HideInInspector] public PolygonImage m_img_menu_PolygonImage;

		[HideInInspector] public RectTransform m_pl_EffectPoint;
		[HideInInspector] public PolygonImage m_img_redpot_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_redCount_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_BattleMainMenu = gameObject.GetComponent<RectTransform>();
			m_btn_menu_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_menu");
			m_btn_menu_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_menu");

			m_img_menu_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_menu/img_menu");

			m_pl_EffectPoint = FindUI<RectTransform>(gameObject.transform ,"btn_menu/pl_EffectPoint");
			m_img_redpot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_menu/img_redpot");

			m_lbl_redCount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_menu/img_redpot/lbl_redCount");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_menu/lbl_name");


			BindEvent();
        }

        #endregion
    }
}