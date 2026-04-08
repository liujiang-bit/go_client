// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_LC_GuildHoly2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_LC_GuildHoly2_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_LC_GuildHoly2";

        public UI_LC_GuildHoly2_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_LC_GuildHoly2_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_LC_GuildHoly2_ArabLayoutCompment;

		[HideInInspector] public UI_Model_GuildHolyItem_SubView m_UI_Item_GuildHolyItem;


        private void UIFinder()
        {       
			m_UI_LC_GuildHoly2_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_LC_GuildHoly2_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item_GuildHolyItem = new UI_Model_GuildHolyItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_GuildHolyItem"));

			BindEvent();
        }

        #endregion
    }
}