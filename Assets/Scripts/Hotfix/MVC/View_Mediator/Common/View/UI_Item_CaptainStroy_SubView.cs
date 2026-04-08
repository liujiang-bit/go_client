// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainStroy_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainStroy_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainStroy";

        public UI_Item_CaptainStroy_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_CaptainStroy_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_img_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_summon_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_summon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_summonData_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_summonData_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_starlv_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_starlv_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_starlvs_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_starlvs_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar;
		[HideInInspector] public LanguageText m_lbl_quality_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_quality_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_qualityData_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_qualityData_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kill_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kill_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_killData_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_killData_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_killNpc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_killNpc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_killNpcData_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_killNpcData_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_stroy_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_stroy_PolygonImage;
		[HideInInspector] public ListView m_sv_list_stroy_ListView;

		[HideInInspector] public LanguageText m_lbl_stroy_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_stroy_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_stroy_ContentSizeFitter;

		[HideInInspector] public UI_Tag_CapIFAnime_Left_SubView m_UI_Tag_CapIFAnime_Left;


        private void UIFinder()
        {       
			m_UI_Item_CaptainStroy_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_pl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_title");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_title/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_title/lbl_title");

			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_title/img_img");
			m_img_img_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_title/img_img");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_lbl_summon_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect1/lbl_summon");
			m_lbl_summon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect1/lbl_summon");

			m_lbl_summonData_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect1/lbl_summonData");
			m_lbl_summonData_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect1/lbl_summonData");

			m_lbl_starlv_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect2/lbl_starlv");
			m_lbl_starlv_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect2/lbl_starlv");

			m_pl_starlvs_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"img_bg/rect2/pl_starlvs");
			m_pl_starlvs_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect2/pl_starlvs");

			m_UI_Model_CaptainStar = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/rect2/pl_starlvs/UI_Model_CaptainStar"));
			m_lbl_quality_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect3/lbl_quality");
			m_lbl_quality_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect3/lbl_quality");

			m_lbl_qualityData_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect3/lbl_qualityData");
			m_lbl_qualityData_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect3/lbl_qualityData");

			m_lbl_kill_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect4/lbl_kill");
			m_lbl_kill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect4/lbl_kill");

			m_lbl_killData_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect4/lbl_killData");
			m_lbl_killData_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect4/lbl_killData");

			m_lbl_killNpc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect5/lbl_killNpc");
			m_lbl_killNpc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect5/lbl_killNpc");

			m_lbl_killNpcData_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/rect5/lbl_killNpcData");
			m_lbl_killNpcData_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/rect5/lbl_killNpcData");

			m_sv_list_stroy_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"img_bg/sv_list_stroy");
			m_sv_list_stroy_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/sv_list_stroy");
			m_sv_list_stroy_ListView = FindUI<ListView>(gameObject.transform ,"img_bg/sv_list_stroy");

			m_lbl_stroy_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/sv_list_stroy/v/c/lbl_stroy");
			m_lbl_stroy_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/sv_list_stroy/v/c/lbl_stroy");
			m_lbl_stroy_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"img_bg/sv_list_stroy/v/c/lbl_stroy");

			m_UI_Tag_CapIFAnime_Left = new UI_Tag_CapIFAnime_Left_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_CapIFAnime_Left"));

			BindEvent();
        }

        #endregion
    }
}