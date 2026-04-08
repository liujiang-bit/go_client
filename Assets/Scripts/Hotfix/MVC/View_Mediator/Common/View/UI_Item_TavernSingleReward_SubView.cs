// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TavernSingleReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_TavernSingleReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TavernSingleReward";

        public UI_Item_TavernSingleReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_TavernSingleReward;
		[HideInInspector] public PolygonImage m_img_table_PolygonImage;

		[HideInInspector] public RectTransform m_pl_CaptainName;
		[HideInInspector] public SkeletonGraphic m_pl_spine_SkeletonGraphic;

		[HideInInspector] public LanguageText m_lbl_civi_LanguageText;

		[HideInInspector] public LanguageText m_lbl_capName_LanguageText;

		[HideInInspector] public RectTransform m_pl_ItemName;
		[HideInInspector] public LanguageText m_lbl_ItemName_LanguageText;

		[HideInInspector] public UI_Item_Bag_SubView m_UI_Item_Bag;
		[HideInInspector] public LanguageText m_lbl_item_count_LanguageText;

		[HideInInspector] public RectTransform m_pl_effect;


        private void UIFinder()
        {       
			m_UI_Item_TavernSingleReward = gameObject.GetComponent<RectTransform>();
			m_img_table_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_table");

			m_pl_CaptainName = FindUI<RectTransform>(gameObject.transform ,"pl_CaptainName");
			m_pl_spine_SkeletonGraphic = FindUI<SkeletonGraphic>(gameObject.transform ,"pl_CaptainName/pl_spine");

			m_lbl_civi_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_CaptainName/lbl_civi");

			m_lbl_capName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_CaptainName/lbl_capName");

			m_pl_ItemName = FindUI<RectTransform>(gameObject.transform ,"pl_ItemName");
			m_lbl_ItemName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_ItemName/lbl_ItemName");

			m_UI_Item_Bag = new UI_Item_Bag_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_ItemName/UI_Item_Bag"));
			m_lbl_item_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_ItemName/lbl_item_count");

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");

			BindEvent();
        }

        #endregion
    }
}