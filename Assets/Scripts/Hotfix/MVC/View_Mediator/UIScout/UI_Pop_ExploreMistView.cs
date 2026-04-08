// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月13日
// Update Time         :    2020年3月13日
// Class Description   :    UI_Pop_ExploreMistView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_ExploreMistView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_ExploreMist";

        public UI_Pop_ExploreMistView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_mist_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_img_mist_GridLayoutGroup;

		[HideInInspector] public Image m_fog_img_Image;

		[HideInInspector] public LanguageText m_lbl_pos2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_pos1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_UI_Model_StandardButton_Yellow;
		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;



        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");

			m_img_mist_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_mist");
			m_img_mist_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/img_bg/img_mist");

			m_fog_img_Image = FindUI<Image>(vb.transform ,"pl_pos/img_bg/img_mist/fog_img");

			m_lbl_pos2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/pos2/lbl_pos2");

			m_lbl_pos1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/pos1/lbl_pos1");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/img_bg/lbl_languageText");

			m_UI_Model_StandardButton_Yellow = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/UI_Model_StandardButton_Yellow"));
			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/arrows/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/arrows/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/arrows/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/arrows/img_arrowSideL");


            UI_Pop_ExploreMistMediator mt = new UI_Pop_ExploreMistMediator(vb.gameObject);
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
