// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeFirst_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeFirst_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeFirst";

        public UI_Item_ChargeFirst_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeFirst;
		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_char_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_spin_char_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_spin_char_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_heroname_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_heroname_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_bigBox_PolygonImage;
		[HideInInspector] public GameButton m_btn_bigBox_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_bigBox_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_item_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_item_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item5;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_btn_buy;
		[HideInInspector] public UI_Tip_BoxReward_SubView m_UI_Tip_BoxReward;


        private void UIFinder()
        {       
			m_UI_Item_ChargeFirst = gameObject.GetComponent<RectTransform>();
			m_pl_view = FindUI<RectTransform>(gameObject.transform ,"pl_view");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view/img_bg");

			m_pl_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/pl_char");

			m_spin_char_SkeletonGraphic = FindUI<SkeletonGraphic>(gameObject.transform ,"pl_view/pl_char/spin_char");
			m_spin_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/pl_char/spin_char");

			m_img_heroname_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view/img_heroname");
			m_img_heroname_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/img_heroname");

			m_btn_bigBox_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view/btn_bigBox");
			m_btn_bigBox_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_view/btn_bigBox");
			m_btn_bigBox_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/btn_bigBox");

			m_pl_item_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_view/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/pl_item");

			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_item/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_item/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_item/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_item/UI_Model_Item4"));
			m_UI_Model_Item5 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_item/UI_Model_Item5"));
			m_btn_buy = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/btn_buy"));
			m_UI_Tip_BoxReward = new UI_Tip_BoxReward_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/UI_Tip_BoxReward"));

			BindEvent();
        }

        #endregion
    }
}