// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月2日
// Update Time         :    2020年4月2日
// Class Description   :    GeneralSettingMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using System;
using Data;

namespace Game {

    public class GeneralSettingItemData
        {
       public int type;//1,2,3 画质，声音，个性设置
        public SettingPersonalityDefine settingPersonalityDefine;//个性设置id

        public GeneralSettingItemData(int type)
        {
            this.type = type;
        }

        public GeneralSettingItemData(int type, SettingPersonalityDefine settingPersonalityDefine)
        {
            this.type = type;
            this.settingPersonalityDefine = settingPersonalityDefine;
        }
    }
    public class GeneralSettingMediator : GameMediator {
        #region Member
        public static string NameMediator = "GeneralSettingMediator";

        private bool change = false;

        private List<GeneralSettingItemData> m_generalSettingItemDatas = new List<GeneralSettingItemData>();
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private GeneralSettingProxy m_generalSettingProxy;
        #endregion

        //IMediatorPlug needs
        public GeneralSettingMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public GeneralSettingView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
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

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            if(change)
            {
                Tip.CreateTip(300224).Show();
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
            m_generalSettingItemDatas = m_generalSettingProxy.GetItemDatas();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_generalSetting);
        }

        protected override void BindUIData()
        {
            InitView();
            m_preLoadRes.AddRange(view.m_sv_scroll_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                AssetLoadFinish();

            });
        }


        #endregion

        private void AssetLoadFinish()
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListViewItemByIndex;
            funcTab.GetItemPrefabName = GetPrefabName;
            funcTab.GetItemSize = GetItemSize;
            view.m_sv_scroll_view_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_scroll_view_ListView.FillContent(m_generalSettingItemDatas.Count);
        }

        private float GetItemSize(ListView.ListItem item)
        {
            int index = item.index;
            GeneralSettingItemData generalSettingItemData = m_generalSettingItemDatas[index];
            switch (generalSettingItemData.type)
            {
                case 1:
                    return 367;
                    break;
                case 2:
                    return 367;
                    break;
                case 3:
                    return 94.1f;
                    break;
                default:
                    return 94.1f;
                    break;
            }
        }

        private string GetPrefabName(ListView.ListItem item)
        {
            int index = item.index;
            GeneralSettingItemData generalSettingItemData = m_generalSettingItemDatas[index];
            switch (generalSettingItemData.type)
            {
                case 1:
                   return  "UI_Item_GraphicsSetting";
                    break;
                case 2:
                    return "UI_Item_MusicSetting";
                    break;
                case 3:
                    return "UI_Item_GeneralSetting";
                    break;
                default:
                    return "UI_Item_GeneralSetting";
                    break;
            }
        }

        private void ListViewItemByIndex(ListView.ListItem item)
        {
            int index = item.index;
            GeneralSettingItemData generalSettingItemData = m_generalSettingItemDatas[index];
            switch (generalSettingItemData.type)
            {
                case 1:
                    SetItemGraphicsSetting(item, generalSettingItemData, index);
                    break;
                case 2:
                    SetItemMusicSetting(item, generalSettingItemData, index);
                    break;
                case 3:
                    SetItemGeneralSetting(item, generalSettingItemData, index);
                    break;
            }
        }

        private void SetItemGraphicsSetting(ListView.ListItem item, GeneralSettingItemData generalSettingItemData ,int index)
        {
            UI_Item_GraphicsSetting_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_GraphicsSetting_SubView;
            }
            else
            {
                itemView = new UI_Item_GraphicsSetting_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
                itemView.Refresh();
                itemView.AddSdPictureEvent(OnPictureSetting);
            }
        }
        private void SetItemMusicSetting(ListView.ListItem item, GeneralSettingItemData generalSettingItemData, int index)
        {
            UI_Item_MusicSetting_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_MusicSetting_SubView;
            }
            else
            {
                itemView = new UI_Item_MusicSetting_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
                itemView.Refresh();
            }
        }
        private void SetItemGeneralSetting(ListView.ListItem item, GeneralSettingItemData generalSettingItemData, int index)
        {
            UI_Item_GeneralSetting_SubView itemView = null;
            SettingPersonalityDefine  settingPersonalityDefine = generalSettingItemData.settingPersonalityDefine;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_GeneralSetting_SubView;
            }
            else
            {
                itemView = new UI_Item_GeneralSetting_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
                itemView.addCKSwitchEvent((open) => {
                    itemView.SetOpen(open);
                    if (open)
                    {
                        m_generalSettingProxy.OpenGeneralSettingItem(settingPersonalityDefine.ID);
                    }
                    else
                    {
                        m_generalSettingProxy.CloseGeneralSettingItem(settingPersonalityDefine.ID);
                    }

                });
            }
            itemView.SetTitle(LanguageUtils.getText(settingPersonalityDefine.l_nameID));
            itemView.SetDesc2(LanguageUtils.getText(settingPersonalityDefine.l_decID));
            itemView.SetToggle(m_generalSettingProxy.GetGeneralSettingByID(settingPersonalityDefine.ID));
        }

        private void InitView()
        {
          
        }


        private void OnSfxSliderChange(float value)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            CoreUtils.audioService.SetSfxVolume(value);
            PlayerPrefs.SetFloat(RS.SfxVolume, value);
        }

        private void OnBGMSliderChange(float value)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            CoreUtils.audioService.SetMusicVolume(value);
            PlayerPrefs.SetFloat(RS.BGMVolume, value);
        }

        private void OnPictureSetting(float value)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            change = true;
            QualitySetting.SetGraphicLevel((CoreUtils.GraphicLevel)(value));
            WeatherManager.Instance().StartUpdateRain(true);
        }

        private void OnFramerateSetting(float value)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            QualitySetting.SetFrameRateLevel((FrameRateLevel)value);
        }
    }
}