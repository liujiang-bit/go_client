// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_PlayerResources_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_PlayerResources_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_PlayerResources";

        public UI_Item_PlayerResources_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_PlayerResources_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_PlayerResources_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_PlayerResources_ArabLayoutCompment;
		[HideInInspector] public Empty4Raycast m_UI_Item_PlayerResources_Empty4Raycast;
		[HideInInspector] public NodeHorizontalLayoutGroup m_UI_Item_PlayerResources_NodeHorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;
		[HideInInspector] public LayoutElement m_img_frame_LayoutElement;

		[HideInInspector] public ArabLayoutCompment m_pl_view1_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_gem;
		[HideInInspector] public GridLayoutGroup m_pl_view_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_view_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_gold;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_stone;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_wood;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_food;
		[HideInInspector] public UI_Tag_MainIFAnime_Right_SubView m_UI_Tag_MainIFAnime_Right;
		[HideInInspector] public Empty4Raycast m_img_mask_Empty4Raycast;
		[HideInInspector] public LayoutElement m_img_mask_LayoutElement;
		[HideInInspector] public GameButton m_img_mask_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_PlayerResources_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_PlayerResources_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
			m_UI_Item_PlayerResources_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();
			m_UI_Item_PlayerResources_Empty4Raycast = gameObject.GetComponent<Empty4Raycast>();
			m_UI_Item_PlayerResources_NodeHorizontalLayoutGroup = gameObject.GetComponent<NodeHorizontalLayoutGroup>();

			m_img_frame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_frame");
			m_img_frame_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"img_frame");

			m_pl_view1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1");

			m_UI_Model_gem = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view1/UI_Model_gem"));
			m_pl_view_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_view");
			m_pl_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view");

			m_UI_Model_gold = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/UI_Model_gold"));
			m_UI_Model_stone = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/UI_Model_stone"));
			m_UI_Model_wood = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/UI_Model_wood"));
			m_UI_Model_food = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/UI_Model_food"));
			m_UI_Tag_MainIFAnime_Right = new UI_Tag_MainIFAnime_Right_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_MainIFAnime_Right"));
			m_img_mask_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"img_mask");
			m_img_mask_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"img_mask");
			m_img_mask_GameButton = FindUI<GameButton>(gameObject.transform ,"img_mask");


			BindEvent();
        }

        #endregion
    }
}