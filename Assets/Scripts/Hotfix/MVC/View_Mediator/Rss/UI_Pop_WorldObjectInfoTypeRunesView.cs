// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月23日
// Update Time         :    2020年7月23日
// Class Description   :    UI_Pop_WorldObjectInfoTypeRunesView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_WorldObjectInfoTypeRunesView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_WorldObjectInfoTypeRunes";

        public UI_Pop_WorldObjectInfoTypeRunesView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public Animator m_pl_content_Animator;

		[HideInInspector] public RectTransform m_img_bg;
		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public CanvasGroup m_pl_description_CanvasGroup;
		[HideInInspector] public Animator m_pl_description_Animator;

		[HideInInspector] public PolygonImage m_btn_descBack_PolygonImage;
		[HideInInspector] public GameButton m_btn_descBack_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descBack_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_descBack_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_desc_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_desc_PolygonImage;
		[HideInInspector] public ListView m_sv_desc_ListView;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_desc_ContentSizeFitter;

		[HideInInspector] public Animator m_pl_normalInfo_Animator;
		[HideInInspector] public CanvasGroup m_pl_normalInfo_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_position_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_position_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_descinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_descinfo_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_descinfo_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_descinfo_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_WorldObjInfoTRunes;
		[HideInInspector] public UI_Item_WorldObjectInfoBuffItem_SubView m_pl_buff1;
		[HideInInspector] public UI_Item_WorldObjectInfoBuffItem_SubView m_pl_buff2;
		[HideInInspector] public ArabLayoutCompment m_pl_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_frame_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_timeIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_timeIcon_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_collect;
		[HideInInspector] public RectTransform m_pl_inSitu;
		[HideInInspector] public GameToggle m_ck_Situ_GameToggle;

		[HideInInspector] public UI_Common_PopFun_SubView m_UI_Common_PopFun;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_content_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content");

			m_img_bg = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/img_bg");
			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideL");

			m_pl_description_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/pl_content/rect/pl_description");
			m_pl_description_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content/rect/pl_description");

			m_btn_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_description/btn_descBack");
			m_btn_descBack_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/rect/pl_description/btn_descBack");
			m_btn_descBack_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_description/btn_descBack");

			m_img_descBack_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_description/btn_descBack/img_descBack");

			m_sv_desc_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/pl_content/rect/pl_description/sv_desc");
			m_sv_desc_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_description/sv_desc");
			m_sv_desc_ListView = FindUI<ListView>(vb.transform ,"pl_pos/pl_content/rect/pl_description/sv_desc");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_description/sv_desc/v/c/lbl_desc");
			m_lbl_desc_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_content/rect/pl_description/sv_desc/v/c/lbl_desc");

			m_pl_normalInfo_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo");
			m_pl_normalInfo_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo");

			m_lbl_position_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/lbl_position");
			m_lbl_position_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/lbl_position");

			m_btn_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/btn_descinfo");
			m_btn_descinfo_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/btn_descinfo");
			m_btn_descinfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/btn_descinfo");

			m_img_descinfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/btn_descinfo/img_descinfo");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/lbl_name");

			m_pl_WorldObjInfoTRunes = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes");
			m_pl_buff1 = new UI_Item_WorldObjectInfoBuffItem_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/textLayer/pl_buff1"));
			m_pl_buff2 = new UI_Item_WorldObjectInfoBuffItem_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/textLayer/pl_buff2"));
			m_pl_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/img_frame");
			m_img_frame_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/img_frame");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/img_icon");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/lbl_time");

			m_img_timeIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/lbl_time/img_timeIcon");
			m_img_timeIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/pl_content/rect/pl_normalInfo/pl_WorldObjInfoTRunes/pl_icon/lbl_time/img_timeIcon");

			m_btn_collect = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/btn_collect"));
			m_pl_inSitu = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/pl_inSitu");
			m_ck_Situ_GameToggle = FindUI<GameToggle>(vb.transform ,"pl_pos/pl_content/rect/pl_inSitu/LabelSitu/ck_Situ");

			m_UI_Common_PopFun = new UI_Common_PopFun_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/rect/UI_Common_PopFun"));

            UI_Pop_WorldObjectInfoTypeRunesMediator mt = new UI_Pop_WorldObjectInfoTypeRunesMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}
