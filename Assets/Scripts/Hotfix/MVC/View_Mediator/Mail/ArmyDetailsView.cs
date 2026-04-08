// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月6日
// Update Time         :    2020年7月6日
// Class Description   :    ArmyDetailsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class ArmyDetailsView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ArmyDetails";

        public ArmyDetailsView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public RectTransform m_UI_Item_PlayerArmyColunmName;
		[HideInInspector] public GridLayoutGroup m_UI_Model_ArmyDetailsColunm_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Model_ArmyDetailsColunm_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_col1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_col1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_col2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_col2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_col3_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_col3_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_col4_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_col4_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_col5_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_col5_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Item_PlayerArmyColunmName = FindUI<RectTransform>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName");
			m_UI_Model_ArmyDetailsColunm_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm");
			m_UI_Model_ArmyDetailsColunm_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm");

			m_lbl_col1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col1");
			m_lbl_col1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col1");

			m_lbl_col2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col2");
			m_lbl_col2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col2");

			m_lbl_col3_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col3");
			m_lbl_col3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col3");

			m_lbl_col4_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col4");
			m_lbl_col4_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col4");

			m_lbl_col5_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col5");
			m_lbl_col5_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PlayerArmyColunmName/UI_Model_ArmyDetailsColunm/lbl_col5");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/content/sv_list_view");


            ArmyDetailsMediator mt = new ArmyDetailsMediator(vb.gameObject);
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
