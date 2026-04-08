// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月3日
// Update Time         :    2020年7月3日
// Class Description   :    MailView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class MailView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Mail";

        public MailView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public UI_Item_MailBtnOnButtom_SubView m_UI_Item_MailBtnOnButtomRe;
		[HideInInspector] public UI_Item_MailBtnOnButtom_SubView m_UI_Item_MailBtnOnButtomTrans;
		[HideInInspector] public UI_Item_MailBtnOnButtom_SubView m_UI_Item_MailBtnOnButtomCollect;
		[HideInInspector] public UI_Item_MailBtnOnButtom_SubView m_UI_Item_MailBtnOnButtomDel;
		[HideInInspector] public ArabLayoutCompment m_pl_all_ArabLayoutCompment;

		[HideInInspector] public UI_Item_MailBtnOnButtom_SubView m_UI_Item_MailBtnOnButtomRead;
		[HideInInspector] public UI_Item_MailBtnOnButtom_SubView m_UI_Item_MailBtnOnButtomDeleteRead;
		[HideInInspector] public PolygonImage m_pl_mailcontentbg_PolygonImage;

		[HideInInspector] public RectTransform m_pl_mailNone;
		[HideInInspector] public ScrollRect m_sv_haveMail_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_haveMail_PolygonImage;
		[HideInInspector] public ListView m_sv_haveMail_ListView;

		[HideInInspector] public UI_Item_MailFightReport_SubView m_UI_Item_MailFightReport;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public UI_Item_MailBtnOnButtom_SubView m_UI_Item_newMail;
		[HideInInspector] public UI_Item_MailPageBtn_SubView m_UI_Item_MailPageBtnSys;
		[HideInInspector] public UI_Item_MailPageBtn_SubView m_UI_Item_MailPageBtnReport;
		[HideInInspector] public UI_Item_MailPageBtn_SubView m_UI_Item_MailPageBtnGuild;
		[HideInInspector] public UI_Item_MailPageBtn_SubView m_UI_Item_MailPageBtnCollect;
		[HideInInspector] public UI_Item_MailPageBtn_SubView m_UI_Item_MailPageBtnPerson;
		[HideInInspector] public UI_Item_MailPageBtn_SubView m_UI_Item_MailPageBtnSend;


        private void UIFinder()
        {
			m_pl_view = FindUI<RectTransform>(vb.transform ,"pl_view");
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_view/win/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"pl_view/win/lbl_title");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_view/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_view/btn_close");

			m_UI_Item_MailBtnOnButtomRe = new UI_Item_MailBtnOnButtom_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/buttom/group/UI_Item_MailBtnOnButtomRe"));
			m_UI_Item_MailBtnOnButtomTrans = new UI_Item_MailBtnOnButtom_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/buttom/group/UI_Item_MailBtnOnButtomTrans"));
			m_UI_Item_MailBtnOnButtomCollect = new UI_Item_MailBtnOnButtom_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/buttom/group/UI_Item_MailBtnOnButtomCollect"));
			m_UI_Item_MailBtnOnButtomDel = new UI_Item_MailBtnOnButtom_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/buttom/group/UI_Item_MailBtnOnButtomDel"));
			m_pl_all_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_view/buttom/pl_all");

			m_UI_Item_MailBtnOnButtomRead = new UI_Item_MailBtnOnButtom_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/buttom/pl_all/UI_Item_MailBtnOnButtomRead"));
			m_UI_Item_MailBtnOnButtomDeleteRead = new UI_Item_MailBtnOnButtom_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/buttom/pl_all/UI_Item_MailBtnOnButtomDeleteRead"));
			m_pl_mailcontentbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_view/content/mailContent/pl_mailcontentbg");

			m_pl_mailNone = FindUI<RectTransform>(vb.transform ,"pl_view/content/mailContent/pl_mailNone");
			m_sv_haveMail_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_view/content/mailContent/sv_haveMail");
			m_sv_haveMail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_view/content/mailContent/sv_haveMail");
			m_sv_haveMail_ListView = FindUI<ListView>(vb.transform ,"pl_view/content/mailContent/sv_haveMail");

			m_UI_Item_MailFightReport = new UI_Item_MailFightReport_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/content/mailContent/UI_Item_MailFightReport"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_view/content/mails/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_view/content/mails/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"pl_view/content/mails/sv_list_view");

			m_UI_Item_newMail = new UI_Item_MailBtnOnButtom_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/title/UI_Item_newMail"));
			m_UI_Item_MailPageBtnSys = new UI_Item_MailPageBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/title/pageRect/UI_Item_MailPageBtnSys"));
			m_UI_Item_MailPageBtnReport = new UI_Item_MailPageBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/title/pageRect/UI_Item_MailPageBtnReport"));
			m_UI_Item_MailPageBtnGuild = new UI_Item_MailPageBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/title/pageRect/UI_Item_MailPageBtnGuild"));
			m_UI_Item_MailPageBtnCollect = new UI_Item_MailPageBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/title/pageRect/UI_Item_MailPageBtnCollect"));
			m_UI_Item_MailPageBtnPerson = new UI_Item_MailPageBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/title/pageRect/UI_Item_MailPageBtnPerson"));
			m_UI_Item_MailPageBtnSend = new UI_Item_MailPageBtn_SubView(FindUI<RectTransform>(vb.transform ,"pl_view/title/pageRect/UI_Item_MailPageBtnSend"));

            MailMediator mt = new MailMediator(vb.gameObject);
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
