// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月7日
// Update Time         :    2020年1月7日
// Class Description   :    LanguageSetMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class LanguageSetMediator : GameMediator
    {
        public enum OpenType
        {
            Start = 0,
            Setting = 1,
        }
        #region Member
        public static string NameMediator = "LanguageSetMediator";
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<Data.LanguageSetDefine> m_languages = new List<Data.LanguageSetDefine>();
        private int m_curLanguage;
        private OpenType m_openType = OpenType.Start;

        #endregion

        //IMediatorPlug needs
        public LanguageSetMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public LanguageSetView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_openType = (OpenType)view.data;
            m_curLanguage = (int)LanguageUtils.GetLanguage();
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_view_ListView.ItemPrefabDataList, LoadFinish);
        }


        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            view.m_UI_Model_StandardButton_Blue_sure.AddClickEvent(OnSureClick);
            view.m_UI_Model_Window_TypeMid.SetCloseVisible(m_openType != OpenType.Start);
            view.m_UI_Model_Window_TypeMid.setCloseHandle(()=>
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_LanguageSet);
            });


        }

        public override bool onMenuBackCallback()
        {
            if (m_openType == OpenType.Start)
            {
                SendNotification(CmdConstant.ExitGame);
                return true;
            }
            return false;
        }

        #endregion
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            InitUI();
        }

        private ToggleGroup m_group;
        private void InitUI()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = OnItemEnter;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);

            var langages = CoreUtils.dataService.QueryRecords<Data.LanguageSetDefine>();
            for (int i = 0; i < langages.Count; i++)
            {
                var lanConfig = langages[i];
                if (lanConfig.enumSwitch == 0)
                    continue;
                //if(!Application.isEditor && lanConfig.ID == (int)SystemLanguage.ChineseSimplified && !HotfixUtil.IsDebugable())
                //{
                //    continue;
                //}
                m_languages.Add(lanConfig);
            }
            m_group = view.m_sv_list_view_ListView.gameObject.AddComponent<ToggleGroup>();
            view.m_sv_list_view_ListView.FillContent((m_languages.Count+1)/2);
        }

        private void OnValueChange(int id)
        {
            if (m_curLanguage == id)
                return;
            m_curLanguage = id;

            //LanguageUtils.SetLanguage((SystemLanguage)m_curLanguage);
        }
        private void OnItemEnter(ListView.ListItem listItem)
        {
            UI_LC_Language_SubView subView;
            if (listItem.isInit == false)
            {
                subView = new UI_LC_Language_SubView(listItem.go.GetComponent<RectTransform>());
                subView.BindGroup(m_group);
                listItem.data = subView;
                listItem.isInit = true;
                subView.AddValueChange(OnValueChange);
            }
            else
            {
                subView = (UI_LC_Language_SubView)listItem.data;
            }
            int left = m_languages[listItem.index * 2].ID;
            int right = -1;
            if (listItem.index * 2 + 1 < m_languages.Count)
            {
                right = m_languages[listItem.index * 2 + 1].ID;
            }
            subView.SetLanguageId(left, right);
            subView.Selected(m_curLanguage);
        }
        private void OnSureClick()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.SetLanguage));
            if (m_openType == OpenType.Start)
            {
                LanguageUtils.SetLanguage((SystemLanguage)m_curLanguage);
                LanguageUtils.SaveCache();

                SendNotification(CmdConstant.ReloadGame);
            }
            else
            {
                if ((int)LanguageUtils.GetLanguage() != m_curLanguage)
                {
                    LanguageUtils.SetLanguage((SystemLanguage)m_curLanguage);
                    LanguageUtils.SaveCache();
                    SendNotification(CmdConstant.ReloadGame);
                }
                else
                {
                    OnCloseClick();
                    return;
                }
            }
        }
        private void OnCloseClick()
        {
            if (m_openType == OpenType.Start)
            {
                return;
            }
            else
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_LanguageSet);
            }
        }
    }
}