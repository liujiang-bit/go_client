// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    BuildCityView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class BuildCityView : GameView
    {
        public const string VIEW_NAME = "UI_Win_BuildCity";

        public BuildCityView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_cityBuild_PolygonImage;
		[HideInInspector] public GameButton m_btn_cityBuild_GameButton;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public UI_Item_BuildCityBtn_SubView m_UI_Item_BuildCityBtn1;
		[HideInInspector] public UI_Item_BuildCityBtn_SubView m_UI_Item_BuildCityBtn2;
		[HideInInspector] public UI_Item_BuildCityBtn_SubView m_UI_Item_BuildCityBtn3;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public UI_Tag_WinAnime_BuildingCity_SubView m_UI_Tag_WinAnime_BuildingCity;


        private void UIFinder()
        {
			m_btn_cityBuild_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/citySetting/btn_cityBuild");
			m_btn_cityBuild_GameButton = FindUI<GameButton>(vb.transform ,"rect/citySetting/btn_cityBuild");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(vb.transform ,"rect/citySetting/UI_Tag_ClickAnimeMsg_btn"));
			m_UI_Item_BuildCityBtn1 = new UI_Item_BuildCityBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btns/btnRect/UI_Item_BuildCityBtn1"));
			m_UI_Item_BuildCityBtn2 = new UI_Item_BuildCityBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btns/btnRect/UI_Item_BuildCityBtn2"));
			m_UI_Item_BuildCityBtn3 = new UI_Item_BuildCityBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btns/btnRect/UI_Item_BuildCityBtn3"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/listRect/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/listRect/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/listRect/sv_list_view");

			m_UI_Tag_WinAnime_BuildingCity = new UI_Tag_WinAnime_BuildingCity_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_WinAnime_BuildingCity"));

            BuildCityMediator mt = new BuildCityMediator(vb.gameObject);
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
