// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainList";

        public UI_Item_CaptainList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_CaptainList_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_arr_PolygonImage;
		[HideInInspector] public GameButton m_btn_arr_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_arr_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_arrtext_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_arrtext_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainPartline_SubView m_UI_Item_CaptainPartline;
		[HideInInspector] public ScrollRect m_sv_captainHead_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_captainHead_PolygonImage;
		[HideInInspector] public ListView m_sv_captainHead_ListView;

		[HideInInspector] public UI_Tag_CapIFAnime_Right_SubView m_UI_Tag_CapIFAnime_Right;
		[HideInInspector] public ArabLayoutCompment m_UI_Pop_arrType_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_Recomend;
		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_Quality;
		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_star;
		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_level;
		[HideInInspector] public UI_Item_CaptionArrType_SubView m_UI_Item_CaptionArrType_power;


        private void UIFinder()
        {       
			m_UI_Item_CaptainList_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_count");
			m_lbl_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/lbl_count");

			m_btn_arr_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"title/btn_arr");
			m_btn_arr_GameButton = FindUI<GameButton>(gameObject.transform ,"title/btn_arr");
			m_btn_arr_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/btn_arr");

			m_lbl_arrtext_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/btn_arr/lbl_arrtext");
			m_lbl_arrtext_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/btn_arr/lbl_arrtext");

			m_UI_Item_CaptainPartline = new UI_Item_CaptainPartline_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainPartline"));
			m_sv_captainHead_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_captainHead");
			m_sv_captainHead_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_captainHead");
			m_sv_captainHead_ListView = FindUI<ListView>(gameObject.transform ,"sv_captainHead");

			m_UI_Tag_CapIFAnime_Right = new UI_Tag_CapIFAnime_Right_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_CapIFAnime_Right"));
			m_UI_Pop_arrType_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"UI_Pop_arrType");

			m_UI_Item_CaptionArrType_Recomend = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_Recomend"));
			m_UI_Item_CaptionArrType_Quality = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_Quality"));
			m_UI_Item_CaptionArrType_star = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_star"));
			m_UI_Item_CaptionArrType_level = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_level"));
			m_UI_Item_CaptionArrType_power = new UI_Item_CaptionArrType_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Pop_arrType/bg/rect/UI_Item_CaptionArrType_power"));

			BindEvent();
        }

        #endregion
    }
}