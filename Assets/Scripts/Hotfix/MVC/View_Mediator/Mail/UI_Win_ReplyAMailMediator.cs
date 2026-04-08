// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月10日
// Update Time         :    2020年6月10日
// Class Description   :    UI_Win_ReplyAMailMediator 回复邮件
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
using Client.Utils;

namespace Game {
    public class UI_Win_ReplyAMailMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ReplyAMailMediator";

        private EmailProxy m_emailProxy;
        private PlayerProxy m_playerProxy;

        private EmailInfoEntity m_replyEmail;
        private MailDefine m_mailDefine;

        private int m_contentLimit;
        private int m_emailTimeInterval;

        private bool m_isCanSend;
        private string m_lastContent = "";
        private string m_lastTitle = "";

        private string m_replyTitle = "";

        private bool m_isSaveDraft = true;

        #endregion

        //IMediatorPlug needs
        public UI_Win_ReplyAMailMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ReplyAMailView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Email_MsgSendPrivateEmail.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Email_MsgSendPrivateEmail.TagName:
                    var response = notification.Body as Email_MsgSendPrivateEmail.response;
                    if (response == null)
                    {
                        Debug.LogError("数据为空");
                        return;
                    }
                    if (response.result)
                    {
                        Tip.CreateTip(570056).Show();
                        m_isSaveDraft = false;
                        OnClose();
                    }
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

        public override void OnRemove()
        {
            EmailDraftData data = new EmailDraftData();
            if (m_isSaveDraft)
            {
                data.Title = m_lastTitle;
                data.Content = m_lastContent;
                m_emailProxy.SaveDraftInfoToFile(data);
            }
            else
            {
                m_emailProxy.SaveDraftInfoToFile(data);
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            m_replyEmail = view.data as EmailInfoEntity;

            MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)m_replyEmail.emailId);
            m_mailDefine = mailDefine;

            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_contentLimit = config.emailContentLimit;
            m_emailTimeInterval = config.emailTimeInterval;

            view.m_lbl_wordsCount_LanguageText.text = LanguageUtils.getTextFormat(300001, 0,
                                                                                  ClientUtils.FormatComma(m_contentLimit));
        }

        protected override void BindUIEvent()
        {
            view.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_ipt_mailContent_GameInput.onEndEdit.AddListener(OnContentValueSubmit);
            view.m_ipt_mailContent_GameInput.onValueChanged.AddListener(OnContentValueChanged);
            view.m_btn_send_GameButton.onClick.AddListener(OnSend);

            Refresh();
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void Refresh()
        {
            //收件人
            string receiverStr = "";
            if (string.IsNullOrEmpty(m_replyEmail.senderInfo.guildAbbr))
            {
                receiverStr = m_replyEmail.senderInfo.name;

            }
            else
            {
                receiverStr = LanguageUtils.getTextFormat(300030, m_replyEmail.senderInfo.guildAbbr, m_replyEmail.senderInfo.name);
            }
            view.m_lbl_playername_LanguageText.text = receiverStr;

            //回复标题
            string replyTitle =m_emailProxy.OnTextFormat(LanguageUtils.getText(m_mailDefine.l_subheadingID), m_replyEmail.subTitleContents);
            view.m_lbl_mailtitle_LanguageText.text = replyTitle;
            m_replyTitle = m_replyEmail.subTitleContents[0];
            //头像
            view.m_UI_Model_PlayerHead.LoadPlayerIcon(m_replyEmail.senderInfo.headId, m_replyEmail.senderInfo.headFrameID);

            //草稿内容处理
            EmailDraftData draftData = m_emailProxy.GetDraftInfo();
            if (draftData != null)
            {
                if (!string.IsNullOrEmpty(draftData.Title))
                {
                    m_lastTitle = draftData.Title;
                }
                if (!string.IsNullOrEmpty(draftData.Content))
                {
                    view.m_ipt_mailContent_GameInput.text = draftData.Content;
                }
            }

            //更新发送按钮状态
            SetSendBtnStatus();

            //被回复邮件内容
            List<string> contentList = new List<string>();
            //1、被回复邮件的发送对象
            if (m_mailDefine.ID == 400000)
            {
                //普通邮件
                contentList.Add(receiverStr);
            }
            else if (m_mailDefine.ID == 400001)
            {
                //回复邮件
                contentList.Add(receiverStr);
            }
            else if (m_mailDefine.ID == 400006)
            {
                //联盟群发邮件
                contentList.Add(LanguageUtils.getText(570093));
            }
            //2、被回复邮件的标题
            contentList.Add(replyTitle);
            //3、收到改邮件的时间
            contentList.Add(UIHelper.GetServerLongTimeFormat(m_replyEmail.sendTime));
            //4、邮件内容
            contentList.Add(m_emailProxy.OnTextFormat(LanguageUtils.getText(m_mailDefine.l_mesID), m_replyEmail.emailContents));
            //刷新内容显示
            view.m_lbl_mes_LanguageText.text = string.Join("\n", contentList);
        }

        //内容改变
        private void OnContentValueChanged(string text)
        {
            if (text.Length > m_contentLimit)
            {
                view.m_ipt_mailContent_GameInput.text = m_lastContent;
            }
            else
            {
                m_lastContent = text;
                view.m_lbl_wordsCount_LanguageText.text = LanguageUtils.getTextFormat(300001,
                                          ClientUtils.FormatComma(text.Length),
                                          ClientUtils.FormatComma(m_contentLimit));
            }
        }

        //内容失去焦点
        private void OnContentValueSubmit(string text)
        {
            SetSendBtnStatus();
        }

        //更新发送按钮显示状态
        private void SetSendBtnStatus()
        {
            bool isGray = false;

            if (string.IsNullOrEmpty(view.m_ipt_mailContent_GameInput.text))
            {
                isGray = true;
            }
            if (isGray)
            {
                view.m_btn_send_MakeChildrenGray.Gray();
            }
            else
            {
                view.m_btn_send_MakeChildrenGray.Normal();
            }
            m_isCanSend = !isGray;
        }

        //发送邮件
        private void OnSend()
        {
            if (!m_isCanSend)
            {
                return;
            }

            //玩家发送邮件间隔是否超过配置的时间
            int times = PlayerPrefs.GetInt("LastSendEmailTime");
            if (times > 0)
            {
                int diffTime = (int)ServerTimeModule.Instance.GetServerTime() - times;
                if (diffTime >=0 && diffTime <= m_emailTimeInterval)
                {
                    Tip.CreateTip(570054).Show();
                    return;
                }
            }

            //邮件发送次数是否达到上限
            int level = (int)m_playerProxy.GetTownHall();
            MailLevelLimitDefine maillevelDefine = CoreUtils.dataService.QueryRecord<MailLevelLimitDefine>(level);
            int mailNum = -1;
            if (maillevelDefine != null)
            {
                mailNum = maillevelDefine.mailNum;
            }
            if (mailNum != -1)
            {
                if (m_playerProxy.CurrentRoleInfo.emailSendCntPerHour >= mailNum)
                {
                    string str = LanguageUtils.getTextFormat(570055, level, mailNum);
                    Tip.CreateTip(str).Show();
                    return;
                }
            }

            //敏感字检查
            string content = view.m_ipt_mailContent_GameInput.text;
            BannedWord.CheckChatBannedWord(content);
            //if ()
            //{
            //    Tip.CreateTip(570058).Show();
            //    return;
            //}

            List<WriteAMailData> titleList = new List<WriteAMailData>();
            List<Receiver> sendlist = new List<Receiver>();

            bool isGuildReply = (m_mailDefine.ID == 400006);

            WriteAMailData singleTargetData = new WriteAMailData();
            singleTargetData.stableName = m_replyEmail.senderInfo.name;
            singleTargetData.stableRid = m_replyEmail.senderInfo.rid;
            singleTargetData.GuildAbbName = m_replyEmail.senderInfo.guildAbbr;
            singleTargetData.headId = m_replyEmail.senderInfo.headId;
            singleTargetData.headFrameID = m_replyEmail.senderInfo.headFrameID;
            titleList.Add(singleTargetData);

            Receiver receiver = new Receiver();
            receiver.rid = m_replyEmail.senderInfo.rid;
            receiver.gameNode = m_playerProxy.GetRoleLoginRes().chatServerName;
            sendlist.Add(receiver);

            //拼接标题
            string nameStr = "";
            if (isGuildReply)
            {
                var alliacneProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                if (alliacneProxy.HasJionAlliance())
                {
                    nameStr = LanguageUtils.getTextFormat(300030, alliacneProxy.GetAbbreviationName(), m_playerProxy.CurrentRoleInfo.name);
                }
                else
                {
                    nameStr = m_playerProxy.CurrentRoleInfo.name;
                }
            }
            else
            {
                int count = titleList.Count;
                for (int i = 0; i < count; i++)
                {
                    nameStr = nameStr + string.Format("{0},{1}", titleList[i].GuildAbbName, titleList[i].stableName);
                    if (i != (count - 1))
                    {
                        nameStr = nameStr + "|";
                    }
                }
                Debug.LogFormat("print nameStr:{0}", nameStr);
            }

            //保存联系人
            int total = titleList.Count - 1;
            for (int i = total; i >= 0; i--)
            {
                if (titleList[i].stableRid == m_playerProxy.CurrentRoleInfo.rid)
                {
                    titleList.RemoveAt(i);
                }
            }
            if (titleList.Count > 0)
            {
                m_emailProxy.SaveNewContactInfo(titleList);
            }

            Debug.Log("print sendlist");
            //ClientUtils.Print(sendlist);

            //发包
            var sp = new Email_MsgSendPrivateEmail.request();
            sp.lst = sendlist;
            sp.title = m_replyTitle;
            sp.content = view.m_ipt_mailContent_GameInput.text;
            sp.receiverInfo = nameStr;
            sp.isReply = true;
            sp.isGuildReply = isGuildReply;
            AppFacade.GetInstance().SendSproto(sp);

            PlayerPrefs.SetInt("LastSendEmailTime", (int)ServerTimeModule.Instance.GetServerTime());
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_replyEmail);
        }
    }
}