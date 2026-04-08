// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeDayCheap_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeDayCheap_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeDayCheap";

        public UI_Item_ChargeDayCheap_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeDayCheap;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_box_PolygonImage;
		[HideInInspector] public GameButton m_btn_box_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_box_ArabLayoutCompment;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_UI_AnimationBox;
		[HideInInspector] public PolygonImage m_img_get_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_get_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ChargeDayCheap = gameObject.GetComponent<RectTransform>();
			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"sv_list");
			m_sv_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_list");

			m_btn_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_box");
			m_btn_box_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_box");
			m_btn_box_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_box");

			m_UI_AnimationBox = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_box/UI_AnimationBox"));
			m_img_get_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_box/img_get");

			m_lbl_get_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_box/img_get/lbl_get");


			BindEvent();
        }

        #endregion
    }
}