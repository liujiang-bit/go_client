// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyCount_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyCount_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyCount";

        public UI_Item_ArmyCount_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ArmyCount_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_ArmyName_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_head_PolygonImage;

		[HideInInspector] public PolygonImage m_img_level_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;

		[HideInInspector] public GameSlider m_sd_count_GameSlider;

		[HideInInspector] public PolygonImage m_ipt_ArmyInput_PolygonImage;
		[HideInInspector] public GameInput m_ipt_ArmyInput_GameInput;

		[HideInInspector] public LanguageText m_lbl_show_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ArmyCount_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_ArmyName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_ArmyName");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");

			m_img_head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon/img_head");

			m_img_level_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon/img_level");

			m_lbl_level_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_icon/img_level/lbl_level");

			m_sd_count_GameSlider = FindUI<GameSlider>(gameObject.transform ,"sd_count");

			m_ipt_ArmyInput_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ipt_ArmyInput");
			m_ipt_ArmyInput_GameInput = FindUI<GameInput>(gameObject.transform ,"ipt_ArmyInput");

			m_lbl_show_LanguageText = FindUI<LanguageText>(gameObject.transform ,"ipt_ArmyInput/lbl_show");


			BindEvent();
        }

        #endregion
    }
}