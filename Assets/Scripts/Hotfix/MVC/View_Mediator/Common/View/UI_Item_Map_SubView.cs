// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_Map_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_Map_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_Map";

        public UI_Item_Map_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_Map_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_Map_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_Map_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_minimapbg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_minimapbg_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_guild_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_master_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_mark_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_self_ArabLayoutCompment;

		[HideInInspector] public MiniMapImage m_map_minimapViewPort_MiniMapImage;
		[HideInInspector] public ArabLayoutCompment m_map_minimapViewPort_ArabLayoutCompment;

		[HideInInspector] public Empty4Raycast m_et_minimap_Empty4Raycast;
		[HideInInspector] public UIEventTrigger m_et_minimap_UIEventTrigger;
		[HideInInspector] public ArabLayoutCompment m_et_minimap_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_Map_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_Map_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
			m_UI_Item_Map_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_img_minimapbg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_minimapbg");
			m_img_minimapbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_minimapbg");

			m_pl_guild_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_guild");

			m_pl_master_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_master");

			m_pl_mark_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mark");

			m_pl_self_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_self");

			m_map_minimapViewPort_MiniMapImage = FindUI<MiniMapImage>(gameObject.transform ,"map_minimapViewPort");
			m_map_minimapViewPort_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"map_minimapViewPort");

			m_et_minimap_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"et_minimap");
			m_et_minimap_UIEventTrigger = FindUI<UIEventTrigger>(gameObject.transform ,"et_minimap");
			m_et_minimap_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"et_minimap");


			BindEvent();
        }

        #endregion
    }
}