// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月16日
// Update Time         :    2020年3月16日
// Class Description   :    ItemHospitalArmyCountView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class ItemHospitalArmyCountView : GameView
    {
        public const string VIEW_NAME = "UI_Item_HospitalArmyCount";

        public ItemHospitalArmyCountView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_ArmyName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_ArmyName_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_ArmyInput_PolygonImage;
		[HideInInspector] public GameInput m_ipt_ArmyInput_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_ArmyInput_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_show_LanguageText;

		[HideInInspector] public GameSlider m_sd_count_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_count_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_handle_PolygonImage;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_level_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;



        private void UIFinder()
        {
			m_lbl_ArmyName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_ArmyName");
			m_lbl_ArmyName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_ArmyName");

			m_ipt_ArmyInput_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_ArmyInput");
			m_ipt_ArmyInput_GameInput = FindUI<GameInput>(vb.transform ,"ipt_ArmyInput");
			m_ipt_ArmyInput_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"ipt_ArmyInput");

			m_lbl_show_LanguageText = FindUI<LanguageText>(vb.transform ,"ipt_ArmyInput/lbl_show");

			m_sd_count_GameSlider = FindUI<GameSlider>(vb.transform ,"sd_count");
			m_sd_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"sd_count");

			m_img_handle_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sd_count/Handle Slide Area/Handle/img_handle");

			m_pl_effect = FindUI<RectTransform>(vb.transform ,"sd_count/Handle Slide Area/Handle/pl_effect");
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"head/img_icon");

			m_img_level_PolygonImage = FindUI<PolygonImage>(vb.transform ,"head/img_level");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"head/img_level/lbl_level");


            ItemHospitalArmyCountMediator mt = new ItemHospitalArmyCountMediator(vb.gameObject);
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
