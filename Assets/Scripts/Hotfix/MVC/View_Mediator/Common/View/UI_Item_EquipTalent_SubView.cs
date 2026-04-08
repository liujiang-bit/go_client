// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipTalent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EquipTalent_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipTalent";

        public UI_Item_EquipTalent_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EquipTalent;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_talent_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_EquipTalent = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_talent_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_talent");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_name");


			BindEvent();
        }

        #endregion
    }
}