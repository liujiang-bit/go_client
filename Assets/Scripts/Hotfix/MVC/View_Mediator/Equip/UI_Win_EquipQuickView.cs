// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月18日
// Update Time         :    2020年8月18日
// Class Description   :    UI_Win_EquipQuickView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_EquipQuickView : GameView
    {
        public const string VIEW_NAME = "UI_Win_EquipQuick";

        public UI_Win_EquipQuickView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_btn_make;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ItemEffect_SubView m_UI_Item_ItemEffect;
		[HideInInspector] public PolygonImage m_img_equip_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_equip_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_att_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_att_PolygonImage;
		[HideInInspector] public ListView m_sv_att_ListView;

		[HideInInspector] public UI_Item_EquipAtt_SubView m_UI_Item_EquipAtt;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_right/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_right/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/pl_right/sv_list");

			m_btn_make = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/btn_make"));
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left");

			m_UI_Item_ItemEffect = new UI_Item_ItemEffect_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_left/UI_Item_ItemEffect"));
			m_img_equip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/img_equip");
			m_img_equip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_left/img_equip");

			m_sv_att_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_left/sv_att");
			m_sv_att_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_left/sv_att");
			m_sv_att_ListView = FindUI<ListView>(vb.transform ,"rect/pl_left/sv_att");

			m_UI_Item_EquipAtt = new UI_Item_EquipAtt_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_left/sv_att/v/c/UI_Item_EquipAtt"));

            UI_Win_EquipQuickMediator mt = new UI_Win_EquipQuickMediator(vb.gameObject);
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
