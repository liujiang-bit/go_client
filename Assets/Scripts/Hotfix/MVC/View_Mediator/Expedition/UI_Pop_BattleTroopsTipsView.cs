// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月6日
// Update Time         :    2020年7月6日
// Class Description   :    UI_Pop_BattleTroopsTipsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_BattleTroopsTipsView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_BattleTroopsTips";

        public UI_Pop_BattleTroopsTipsView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideR_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideL_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_pos_Animator;

		[HideInInspector] public ScrollRect m_sv_page_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_page_PolygonImage;
		[HideInInspector] public ListView m_sv_page_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_page_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_page_PolygonImage;
		[HideInInspector] public Mask m_v_page_Mask;

		[HideInInspector] public RectTransform m_c_page;
		[HideInInspector] public VerticalLayoutGroup m_pl_armyInfo_VerticalLayoutGroup;

		[HideInInspector] public UI_Item_BattleTroopsTipsCaptain_SubView m_UI_Item_BattleTroopsTipsCaptain1;
		[HideInInspector] public UI_Item_BattleTroopsTipsCaptain_SubView m_UI_Item_BattleTroopsTipsCaptain2;
		[HideInInspector] public RectTransform m_pl_armyNum;
		[HideInInspector] public LanguageText m_lbl_armyNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyNum_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_allarmys_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_allarmys_ArabLayoutCompment;

		[HideInInspector] public UI_Item_SoldierHead_SubView m_UI_Item_SoldierHead;


        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_arrowSideR");
			m_img_arrowSideR_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bg/img_arrowSideR");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_arrowSideL");
			m_img_arrowSideL_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bg/img_arrowSideL");

			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"img_bg/pl_pos");

			m_sv_page_ScrollRect = FindUI<ScrollRect>(vb.transform ,"img_bg/pl_pos/sv_page");
			m_sv_page_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_pos/sv_page");
			m_sv_page_ListView = FindUI<ListView>(vb.transform ,"img_bg/pl_pos/sv_page");
			m_sv_page_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bg/pl_pos/sv_page");

			m_v_page_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_pos/sv_page/v_page");
			m_v_page_Mask = FindUI<Mask>(vb.transform ,"img_bg/pl_pos/sv_page/v_page");

			m_c_page = FindUI<RectTransform>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page");
			m_pl_armyInfo_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo");

			m_UI_Item_BattleTroopsTipsCaptain1 = new UI_Item_BattleTroopsTipsCaptain_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/UI_Item_BattleTroopsTipsCaptain1"));
			m_UI_Item_BattleTroopsTipsCaptain2 = new UI_Item_BattleTroopsTipsCaptain_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/UI_Item_BattleTroopsTipsCaptain2"));
			m_pl_armyNum = FindUI<RectTransform>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/pl_armyNum");
			m_lbl_armyNum_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/pl_armyNum/lbl_armyNum");
			m_lbl_armyNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/pl_armyNum/lbl_armyNum");

			m_pl_allarmys_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/pl_allarmys");
			m_pl_allarmys_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/pl_allarmys");

			m_UI_Item_SoldierHead = new UI_Item_SoldierHead_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/pl_pos/sv_page/v_page/c_page/pl_armyInfo/pl_allarmys/UI_Item_SoldierHead"));

            UI_Pop_BattleTroopsTipsMediator mt = new UI_Pop_BattleTroopsTipsMediator(vb.gameObject);
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
