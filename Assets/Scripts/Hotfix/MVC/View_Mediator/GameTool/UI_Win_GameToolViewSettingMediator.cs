// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月1日
// Update Time         :    2020年7月1日
// Class Description   :    UI_Win_GameToolViewSettingMediator
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
using UnityEngine.UI;

namespace Game {
    public class UI_Win_GameToolViewSettingMediator : GameMediator {
        #region Member

        public static string NameMediator = "UI_Win_GameToolViewSettingMediator";

        private Dictionary<string, int> OptionInfoDic;
        private Dictionary<string, Toggle> ToggleItemDic;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GameToolViewSettingMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GameToolViewSettingView view;

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
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            OptionInfoDic = view.data as Dictionary<string, int>;
            ToggleItemDic = new Dictionary<string, Toggle>();

            foreach (string optionName in OptionInfoDic.Keys)
            {
                GameObject toggleItem = GameObject.Instantiate(view.m_ck_viewtype_Toggle.gameObject, view.m_glg_content_GridLayoutGroup.transform);
                toggleItem.transform.Find("Label").GetComponent<Text>().text = optionName;
                toggleItem.SetActive(true);

                Toggle Toggle = toggleItem.GetComponent<Toggle>();
                Toggle.isOn = PlayerPrefs.GetInt(optionName, 1) == 1;
                ToggleItemDic.Add(optionName, Toggle);
            }            
        }

        protected override void BindUIEvent()
        {
            view.m_btn_confirm_Button.onClick.AddListener(OnConfirm);
        }

        protected override void BindUIData()
        {

        }

        private void OnConfirm()
        {
            foreach (string optionName in OptionInfoDic.Keys)
            {
                PlayerPrefs.SetInt(optionName, ToggleItemDic[optionName].isOn ? 1 : 0);

                //999特指Debug相关UI
                if (OptionInfoDic[optionName] == 999)
                {
                    GameObject debugConsole = GameObject.Find("IngameDebugConsole");
                    GameObject graphy = GameObject.Find("Graphy");

                    if (ToggleItemDic[optionName].isOn)
                    {
                        if (debugConsole == null)
                        {
                            CoreUtils.assetService.Instantiate("IngameDebugConsole", (gameObject) => {
                                gameObject.name = "IngameDebugConsole";
                            });
                        }
                        if (graphy == null)
                        {
                            CoreUtils.assetService.Instantiate("Graphy", (gameObject) => {
                                gameObject.name = "Graphy";
                            });
                        }
                    }
                    else {
                        if (debugConsole != null) GameObject.Destroy(debugConsole);
                        if (graphy != null) GameObject.Destroy(graphy);
                    }
                }
                else {
                    CoreUtils.uiManager.GetUILayer(OptionInfoDic[optionName]).gameObject.SetActive(ToggleItemDic[optionName].isOn);
                }
            }

            CoreUtils.uiManager.CloseUI(UI.s_gameToolViewSetting);
        }
       
        #endregion
    }
}