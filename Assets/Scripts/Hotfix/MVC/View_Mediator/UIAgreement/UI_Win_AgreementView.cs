// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月12日
// Update Time         :    2020年8月12日
// Class Description   :    UI_Win_AgreementView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_AgreementView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Agreement";

        public UI_Win_AgreementView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public ScrollRect m_sv_content_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_content_PolygonImage;
		[HideInInspector] public ListView m_sv_content_ListView;

		[HideInInspector] public LanguageText m_lbl_Content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_Content_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_Content_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_links_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_links_ArabLayoutCompment;

		[HideInInspector] public UI_Item_AgreementLink_SubView m_UI_Item_AgreementLink;
		[HideInInspector] public PolygonImage m_img_logo_PolygonImage;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_btnOK;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;



        private void UIFinder()
        {
			m_sv_content_ScrollRect = FindUI<ScrollRect>(vb.transform ,"bg/content/sv_content");
			m_sv_content_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/content/sv_content");
			m_sv_content_ListView = FindUI<ListView>(vb.transform ,"bg/content/sv_content");

			m_lbl_Content_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/content/sv_content/v/c/lbl_Content");
			m_lbl_Content_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"bg/content/sv_content/v/c/lbl_Content");
			m_lbl_Content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/content/sv_content/v/c/lbl_Content");

			m_pl_links_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"bg/content/pl_links");
			m_pl_links_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/content/pl_links");

			m_UI_Item_AgreementLink = new UI_Item_AgreementLink_SubView(FindUI<RectTransform>(vb.transform ,"bg/content/pl_links/UI_Item_AgreementLink"));
			m_img_logo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/img_logo");

			m_UI_btnOK = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"bg/UI_btnOK"));
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_title");


            UI_Win_AgreementMediator mt = new UI_Win_AgreementMediator(vb.gameObject);
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
            vb.onMenuBackCallback = mt.onMenuBackCallback;
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
