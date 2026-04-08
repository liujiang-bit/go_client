// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Common_PopFun_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Common_PopFun_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Common_PopFun";

        public UI_Common_PopFun_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Common_PopFun_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_share_Animator;

		[HideInInspector] public PolygonImage m_img_fra_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_share_PolygonImage;
		[HideInInspector] public GameButton m_btn_share_GameButton;

		[HideInInspector] public VerticalLayoutGroup m_pl_list_VerticalLayoutGroup;

		[HideInInspector] public PolygonImage m_btn_pos_PolygonImage;
		[HideInInspector] public GameButton m_btn_pos_GameButton;

		[HideInInspector] public PolygonImage m_btn_city_PolygonImage;
		[HideInInspector] public GameButton m_btn_city_GameButton;

		[HideInInspector] public PolygonImage m_btn_collect_PolygonImage;
		[HideInInspector] public GameButton m_btn_collect_GameButton;

		[HideInInspector] public PolygonImage m_btn_sign_PolygonImage;
		[HideInInspector] public GameButton m_btn_sign_GameButton;

		[HideInInspector] public PolygonImage m_btn_face_PolygonImage;
		[HideInInspector] public GameButton m_btn_face_GameButton;



        private void UIFinder()
        {       
			m_UI_Common_PopFun_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_pl_share_Animator = FindUI<Animator>(gameObject.transform ,"topBtns/pl_share");

			m_img_fra_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"topBtns/pl_share/img_fra");

			m_btn_share_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"topBtns/pl_share/btn_share");
			m_btn_share_GameButton = FindUI<GameButton>(gameObject.transform ,"topBtns/pl_share/btn_share");

			m_pl_list_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"topBtns/pl_share/pl_list");

			m_btn_pos_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"topBtns/pl_share/pl_list/btn_pos");
			m_btn_pos_GameButton = FindUI<GameButton>(gameObject.transform ,"topBtns/pl_share/pl_list/btn_pos");

			m_btn_city_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"topBtns/pl_share/pl_list/btn_city");
			m_btn_city_GameButton = FindUI<GameButton>(gameObject.transform ,"topBtns/pl_share/pl_list/btn_city");

			m_btn_collect_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"topBtns/btn_collect");
			m_btn_collect_GameButton = FindUI<GameButton>(gameObject.transform ,"topBtns/btn_collect");

			m_btn_sign_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"topBtns/btn_sign");
			m_btn_sign_GameButton = FindUI<GameButton>(gameObject.transform ,"topBtns/btn_sign");

			m_btn_face_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"topBtns/btn_face");
			m_btn_face_GameButton = FindUI<GameButton>(gameObject.transform ,"topBtns/btn_face");


			BindEvent();
        }

        #endregion
    }
}