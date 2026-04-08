// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WarTarget_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WarTarget_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WarTarget";

        public UI_Item_WarTarget_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_WarTarget_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_flag_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_Link_SubView m_UI_LinkRight;
		[HideInInspector] public RectTransform m_pl_build;
		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;

		[HideInInspector] public PolygonImage m_img_build_PolygonImage;

		[HideInInspector] public RectTransform m_pl_dis;
		[HideInInspector] public LanguageText m_lbl_dis_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_WarTarget_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_img_flag_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_flag");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_PlayerHead"));
			m_UI_LinkRight = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_LinkRight"));
			m_pl_build = FindUI<RectTransform>(gameObject.transform ,"pl_build");
			m_img_frame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_build/img_frame");

			m_img_build_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_build/img_build");

			m_pl_dis = FindUI<RectTransform>(gameObject.transform ,"pl_dis");
			m_lbl_dis_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_dis/lbl_dis");


			BindEvent();
        }

        #endregion
    }
}