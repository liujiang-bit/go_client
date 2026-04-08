// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月20日
// Update Time         :    2020年8月20日
// Class Description   :    UI_Pop_MailGetNewMediator
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

namespace Game {
    public class EmailBubblesData
    {
        public int emailID;
        public string messageName;
        public string messageNameColor;
        public string icon;
        public string message;
    }
    public class UI_Pop_MailGetNewMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_MailGetNewMediator";
        private Timer timer;//2秒后消失
        private  EmailBubblesData emailBubblesData;
        private Color defaultColor = new Color32(77, 73, 69,255);
        #endregion

        //IMediatorPlug needs
        public UI_Pop_MailGetNewMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_MailGetNewView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
              //CmdConstant.AddEmailBubble
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                //case CmdConstant.AddEmailBubble:
                //    if (notification.Body is EmailBubblesData)
                //    {
                //        emailBubblesData = notification.Body as EmailBubblesData;
                //        RefreshView();
                //    }
                //    break;
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
            if (view.data is EmailBubblesData)
            {
                emailBubblesData = view.data as EmailBubblesData;
            }
            else
            {
                Debug.LogError("不能处理的类型");
            }

        }

        protected override void BindUIEvent()
        {
            RefreshView();
        }

        protected override void BindUIData()
        {
    
        }

        #endregion

        public void RefreshView()
        {
            if (timer != null)
            {
                timer.Cancel();
                timer = null;
            }
            timer = Timer.Register(2, () => { CoreUtils.uiManager.CloseUI(UI.s_emailBubbles); });
            view.m_btn_click_GameButton.onClick.RemoveAllListeners();
            view.m_btn_click_GameButton.onClick.AddListener(()=> {
                CoreUtils.uiManager.ShowUI(UI.s_Email,null, emailBubblesData.emailID);
            });
            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage, emailBubblesData.icon);
            view.m_lbl_name_LanguageText.text = emailBubblesData.messageName;
            view.m_lbl_desc_LanguageText.text = emailBubblesData.message;
            if (string.Equals(emailBubblesData.messageNameColor, "green"))
            {
                view.m_lbl_name_LanguageText.color = Color.green;
            }
            else if (string.Equals(emailBubblesData.messageNameColor, "red"))
            {
                view.m_lbl_name_LanguageText.color = Color.red;
            }
            else
            {
                view.m_lbl_name_LanguageText.color = defaultColor;
            }
        }
    }
}