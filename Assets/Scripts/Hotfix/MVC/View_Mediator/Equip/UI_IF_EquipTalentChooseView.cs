// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_IF_EquipTalentChooseView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_EquipTalentChooseView : GameView
    {
        public const string VIEW_NAME = "UI_IF_EquipTalentChoose";

        public UI_IF_EquipTalentChooseView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_talent;
		[HideInInspector] public PolygonImage m_img_equip_PolygonImage;

		[HideInInspector] public UI_Item_EquipTalent_SubView m_UI_Item_EquipTalent1;
		[HideInInspector] public UI_Item_EquipTalent_SubView m_UI_Item_EquipTalent2;
		[HideInInspector] public UI_Item_EquipTalent_SubView m_UI_Item_EquipTalent3;
		[HideInInspector] public UI_Item_EquipTalent_SubView m_UI_Item_EquipTalent4;
		[HideInInspector] public UI_Item_EquipTalent_SubView m_UI_Item_EquipTalent5;
		[HideInInspector] public RectTransform m_pl_talentPreview;
		[HideInInspector] public UI_Item_EquipTalent_SubView m_pl_preview;
		[HideInInspector] public RectTransform m_pl_att;
		[HideInInspector] public GridLayoutGroup m_pl_equipatt_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_equipatt_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_pl_equipatt_ContentSizeFitter;

		[HideInInspector] public UI_Model_EquipAtt_SubView m_UI_Model_EquipAtt;
		[HideInInspector] public LanguageText m_lbl_talentTip_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_heroHead_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_heroHead_GridLayoutGroup;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_back;
		[HideInInspector] public UI_Model_StandardButton_Yellow_SubView m_btn_sure;


        private void UIFinder()
        {
			m_pl_talent = FindUI<RectTransform>(vb.transform ,"pl_talent");
			m_img_equip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_talent/img_equip");

			m_UI_Item_EquipTalent1 = new UI_Item_EquipTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_talent/UI_Item_EquipTalent1"));
			m_UI_Item_EquipTalent2 = new UI_Item_EquipTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_talent/UI_Item_EquipTalent2"));
			m_UI_Item_EquipTalent3 = new UI_Item_EquipTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_talent/UI_Item_EquipTalent3"));
			m_UI_Item_EquipTalent4 = new UI_Item_EquipTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_talent/UI_Item_EquipTalent4"));
			m_UI_Item_EquipTalent5 = new UI_Item_EquipTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_talent/UI_Item_EquipTalent5"));
			m_pl_talentPreview = FindUI<RectTransform>(vb.transform ,"pl_talentPreview");
			m_pl_preview = new UI_Item_EquipTalent_SubView(FindUI<RectTransform>(vb.transform ,"pl_talentPreview/pl_preview"));
			m_pl_att = FindUI<RectTransform>(vb.transform ,"pl_talentPreview/pl_att");
			m_pl_equipatt_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_talentPreview/pl_att/pl_equipatt");
			m_pl_equipatt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_talentPreview/pl_att/pl_equipatt");
			m_pl_equipatt_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_talentPreview/pl_att/pl_equipatt");

			m_UI_Model_EquipAtt = new UI_Model_EquipAtt_SubView(FindUI<RectTransform>(vb.transform ,"pl_talentPreview/pl_att/pl_equipatt/UI_Model_EquipAtt"));
			m_lbl_talentTip_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_talentPreview/pl_att/lbl_talentTip");

			m_pl_heroHead_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_talentPreview/pl_heroHead");
			m_pl_heroHead_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_talentPreview/pl_heroHead");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_talentPreview/pl_heroHead/UI_Model_CaptainHead"));
			m_btn_back = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_talentPreview/btn_back"));
			m_btn_sure = new UI_Model_StandardButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_talentPreview/btn_sure"));

            UI_IF_EquipTalentChooseMediator mt = new UI_IF_EquipTalentChooseMediator(vb.gameObject);
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
