// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyConstitute_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyConstitute_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyConstitute";

        public UI_Item_ArmyConstitute_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ArmyConstitute;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCount_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ArmyConstitute = gameObject.GetComponent<RectTransform>();
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_armyCount");
			m_lbl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_armyCount");


			BindEvent();
        }

        #endregion
    }
}