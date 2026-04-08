// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    ChangeNameMediator
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
using Data;
using System.Text.RegularExpressions;

namespace Game {
    public class ChangeNameMediator : GameMediator {
        #region Member
        public static string NameMediator = "ChangeNameMediator";

        PlayerProxy m_playerProxy;
        BagProxy m_bagProxy;

        int nameMaxLimit;
        int nameMinLimit;
        int itemID;
        int gemCost;

        string lastName;

        GameButton button;

        bool isUseDenar;
        #endregion

        //IMediatorPlug needs
        public ChangeNameMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ChangeNameView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_ModifyName.TagName,

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_ModifyName.TagName:  
                    if(notification.Body is Role_ModifyName.response )
                    {
                        Role_ModifyName.response res = notification.Body as Role_ModifyName.response;
                        Tip.CreateTip(LanguageUtils.getTextFormat(100512, res.name)).Show();
                    }
                    if(notification.Body is ErrorMessage)
                    {
                        ErrorMessage msg = notification.Body as ErrorMessage;
                        if(msg.errorCode==(long)ErrorCode.ROLE_NAME_REPEAT)
                        {
                            Tip.CreateTip(100511).SetStyle(Tip.TipStyle.Middle).Show();
                        }
                        break;
                    }
                    onClose();
                    break;
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
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

            ConfigDefine define = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            nameMinLimit = define.playerNameLimit[0];
            nameMaxLimit = define.playerNameLimit[1];
            gemCost = define.playerNameCostDenar;
            itemID = define.playerNameCostItem;

            OnButtonChange();
        }

        private void OnButtonChange()
        {
            if(m_bagProxy.GetItemNum(itemID) >0)
            {
                isUseDenar = false;
                button = view.m_btn_item.m_btn_languageButton_GameButton;
                view.m_btn_item.m_lbl_line2_LanguageText.text = " x 1";
                view.m_btn_item.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                view.m_btn_item.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                view.m_btn_cur.gameObject.SetActive(false);
                view.m_btn_item.gameObject.SetActive(true);
            }
            else
            {
                isUseDenar = true;
                view.m_btn_cur.m_lbl_line2_LanguageText.text = gemCost.ToString("N0");
                button = view.m_btn_cur.m_btn_languageButton_GameButton;
                view.m_btn_cur.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                view.m_btn_cur.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                view.m_btn_cur.gameObject.SetActive(true);
                view.m_btn_item.gameObject.SetActive(false);
            }
            button.interactable = false;
            view.m_lbl_des_LanguageText.text = LanguageUtils.getTextFormat(100513,nameMinLimit,nameMaxLimit);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.setCloseHandle(onClose);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnOK);
            view.m_ipt_name_GameInput.onValueChanged.AddListener(OnTextChange);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_PlayerChangeName);
        }

        protected override void BindUIData()
        {
            
        }
       
        #endregion

        private void OnTextChange(string text)
        {
            view.m_ipt_name_GameInput.onValueChanged.RemoveListener(OnTextChange);
            string temp = text;//Regex.Replace(text, @"[\u4e00-\u9fa5\u0800-\u4e00\uac00-\ud7ff]+", "");
            if(temp.Length<nameMinLimit)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
            if(temp.Length>nameMaxLimit)
            {
                temp = temp.Remove(nameMaxLimit-1);
            }
            view.m_ipt_name_GameInput.text = temp;
            view.m_ipt_name_GameInput.onValueChanged.AddListener(OnTextChange);
        }

        private void OnOK()
        {
            string name = view.m_ipt_name_GameInput.text;
            if(string.IsNullOrEmpty(name))
            {
                Tip.CreateTip(100014).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            if((Client.Utils.BannedWord.ChackHasBannedWord(name)))
            {
                Tip.CreateTip(300128).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            //纯数字
            if(IsInt(name))
            {
                Tip.CreateTip(100509).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            //与原名相同
            if(m_playerProxy.CurrentRoleInfo.name==name)
            {
                Tip.CreateTip(100510).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            if(!string.IsNullOrEmpty(lastName)&&lastName == name)
            {
                Tip.CreateTip(100511).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }

            if(isUseDenar&&m_playerProxy.CurrentRoleInfo.denar<gemCost)
            {
                //Tip.CreateTip(300095).SetStyle(Tip.TipStyle.Middle).Show();
                CoreUtils.uiManager.ShowUI(UI.s_GemShort);
                return;
            }

            Role_ModifyName.request req = new Role_ModifyName.request();
            req.name = name;
            lastName = name;
            AppFacade.GetInstance().SendSproto(req);
        }

        public bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
    }
}