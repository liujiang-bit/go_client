// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月18日
// Update Time         :    2020年5月18日
// Class Description   :    UI_Win_WriteAMailMediator
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

    public class MailContactNameData
    {
        public WriteAMailData MailData;
        public float NameWidth;
    }

    public class WriteAMailData
    {
        public bool isGuildMail;
        public long stableRid;          //玩家rid
        public string stableName;       //玩家名称
        public string GuildAbbName;     //联盟简称
        public long headId;              //头像id
        public long headFrameID;         //头像框id
    }

    public enum EnumWriteAMailType
    {
        Input, //文本输入模式
        List,  //列表模式
        Fixed  //文本固定模式
    }

    public class EmailDraftData
    {
        public string Title;
        public string Content;
    }

    public class UI_Win_WriteAMailMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_WriteAMailMediator";

        private PlayerProxy m_playerProxy;
        private EmailProxy m_emailProxy;
        private AllianceProxy m_allianceProxy;

        private WriteAMailData m_singleTargetData;

        private EnumWriteAMailType m_type;

        private List<MailContactNameData> m_targetList = new List<MailContactNameData>();

        private string m_lastName = "";
        private string m_lastTitle = "";
        private string m_lastContent = "";

        private int m_nameLimit;
        private int m_titleLimit;
        private int m_contentLimit;
        private int m_emailTimeInterval;
        private int m_emialSendLimit;

        private bool m_isCanSend;

        private bool m_isInitList;

        private bool m_isCheckNameValiding; //是否正在检查名称的有效性

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private bool m_isSaveDraft = true;

        #endregion

        //IMediatorPlug needs
        public UI_Win_WriteAMailMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_WriteAMailView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Email_MsgSendPrivateEmail.TagName,
                CmdConstant.OnSelectEmailTarget,
                Role_QueryRoleName.TagName,
                Email_SendGuildEmail.TagName,
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
                    SendResult( response.result);
                    break;
                case Email_SendGuildEmail.TagName:
                    var res = notification.Body as Email_SendGuildEmail.response;
                    if (res == null)
                    {
                        Debug.LogError("数据为空");
                        return;
                    }
                    SendResult(res.result);
                    break;
                case CmdConstant.OnSelectEmailTarget:
                    RefreshSelectTarget(notification.Body);
                    break;
                case Role_QueryRoleName.TagName:
                    QueryRoleProcess(notification.Body);
                    break;
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {

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

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            view.gameObject.SetActive(false);

            if (view.data is WriteAMailData)
            {
                m_singleTargetData = view.data as WriteAMailData;
                m_type = EnumWriteAMailType.Fixed;
            }
            else
            {
                m_type = EnumWriteAMailType.Input;
            }

            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_nameLimit = config.playerNameLimit[1];
            m_titleLimit = config.emailTitleLimit;
            m_contentLimit = config.emailContentLimit;
            m_emailTimeInterval = config.emailTimeInterval;

            view.m_lbl_wordsCount_LanguageText.text = LanguageUtils.getTextFormat(300001, 0,
                                                                                  ClientUtils.FormatComma(m_contentLimit));

            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_targetList_ListView.ItemPrefabDataList, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_ipt_mailtarget_GameInput.onEndEdit.AddListener(OnTargetSubmit);
            view.m_ipt_mailtarget_GameInput.onValueChanged.AddListener(OnTargetChange);

            view.m_ipt_mailTitle_GameInput.onEndEdit.AddListener(OnTitleValueSubmit);
            view.m_ipt_mailTitle_GameInput.onValueChanged.AddListener(OnTitleValueChanged);

            view.m_ipt_mailContent_GameInput.onEndEdit.AddListener(OnContentValueSubmit);
            view.m_ipt_mailContent_GameInput.onValueChanged.AddListener(OnContentValueChanged);

            view.m_btn_add_GameButton.onClick.AddListener(OpenTargetSelectWin);
            view.m_btn_checkError_GameButton.onClick.AddListener(OnError);
            view.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_btn_send_GameButton.onClick.AddListener(OnSend);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            RefreshTarget();

            //草稿内容处理
            EmailDraftData draftData = m_emailProxy.GetDraftInfo();
            if (draftData != null)
            {
                if (!string.IsNullOrEmpty(draftData.Title))
                {
                    view.m_ipt_mailTitle_GameInput.text = draftData.Title;
                }
                if (!string.IsNullOrEmpty(draftData.Content))
                {
                    view.m_ipt_mailContent_GameInput.text = draftData.Content;
                }
                SetSendBtnStatus();
            }

            view.gameObject.SetActive(true);
        }

        private void RefreshTarget()
        {
            switch (m_type)
            {
                case EnumWriteAMailType.Fixed:
                    view.m_ipt_mailtarget_GameInput.text = m_singleTargetData.stableName;
                    view.m_ipt_mailtarget_GameInput.interactable = false;
                    view.m_sv_targetList_ListView.gameObject.SetActive(false);

                    view.m_btn_add_GameButton.gameObject.SetActive(false);
                    view.m_btn_checkError_GameButton.gameObject.SetActive(false);
                    view.m_pl_checkGood_PolygonImage.gameObject.SetActive(true);
                    break;
                case EnumWriteAMailType.Input:
                    view.m_ipt_mailtarget_GameInput.gameObject.SetActive(true);
                    view.m_sv_targetList_ListView.gameObject.SetActive(false);

                    view.m_btn_add_GameButton.gameObject.SetActive(true);
                    view.m_btn_checkError_GameButton.gameObject.SetActive(false);
                    view.m_pl_checkGood_PolygonImage.gameObject.SetActive(false);
                    break;
                case EnumWriteAMailType.List:
                    view.m_ipt_mailtarget_GameInput.gameObject.SetActive(false);
                    view.m_sv_targetList_ListView.gameObject.SetActive(true);

                    view.m_btn_add_GameButton.gameObject.SetActive(true);
                    view.m_btn_checkError_GameButton.gameObject.SetActive(false);
                    view.m_pl_checkGood_PolygonImage.gameObject.SetActive(false);
                    break;
                default: break;
            }
            SetSendBtnStatus();
        }

        private void RefreshSelectTarget(object body)
        {
            List<WriteAMailData> dataList = body as List<WriteAMailData>;
            m_targetList = new List<MailContactNameData>();

            for (int i = 0; i < dataList.Count; i++)
            {
                MailContactNameData nameData = new MailContactNameData();
                nameData.MailData = dataList[i];
                nameData.NameWidth = -1000;
                m_targetList.Add(nameData);
            }
            if (m_targetList.Count > 0)
            {
                m_type = EnumWriteAMailType.List;
                RefreshTarget();
            }
            else
            {
                m_type = EnumWriteAMailType.Input;
                view.m_ipt_mailtarget_GameInput.text = "";
                RefreshTarget();
            }
            RefreshTargetList();
            SetSendBtnStatus();
        }

        private void RefreshTargetList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ListViewItemByIndex;
                funcTab.GetItemSize = OnGetItemSize;
                view.m_sv_targetList_ListView.SetInitData(m_assetDic, funcTab);
                m_isInitList = true;
            }
            view.m_sv_targetList_ListView.FillContent(m_targetList.Count);
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            if (m_targetList[listItem.index].NameWidth == -1000)
            {
                return 150;
            }
            return m_targetList[listItem.index].NameWidth;
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            WriteAMailData mailData = m_targetList[listItem.index].MailData;

            UI_Item_WriteAMailTarget_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_WriteAMailTarget_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.DeleteTargetCallback = DeleteTargetCallback;
            }
            else
            {
                subView = listItem.data as UI_Item_WriteAMailTarget_SubView;
            }

            subView.Refresh(mailData, listItem.index);
            if (m_targetList[listItem.index].NameWidth == -1000)
            {
                m_targetList[listItem.index].NameWidth = listItem.go.GetComponent<RectTransform>().rect.width;
                view.m_sv_targetList_ListView.RefreshItem(listItem.index);
            }
        }

        private void DeleteTargetCallback(int index)
        {
            m_targetList.RemoveAt(index);
            view.m_sv_targetList_ListView.RemoveAt(index);
            view.m_sv_targetList_ListView.ForceRefresh();
            if (m_targetList.Count < 1)
            {
                m_type = EnumWriteAMailType.Input;
                view.m_ipt_mailtarget_GameInput.text = "";
                RefreshTarget();
            }
        }

        //打开目标选择窗口
        private void OpenTargetSelectWin()
        {
            if (m_isCheckNameValiding)
            {
                return;
            }
            CoreUtils.uiManager.ShowUI(UI.s_mailContactList, null, m_targetList);
        }

        //点击错误图标
        private void OnError()
        {
            view.m_ipt_mailtarget_GameInput.text = "";
            view.m_pl_checkGood_PolygonImage.gameObject.SetActive(false);
            view.m_btn_checkError_GameButton.gameObject.SetActive(false);
            view.m_btn_add_GameButton.gameObject.SetActive(true);
        }

        //目标文本变更
        private void OnTargetChange(string text)
        {
            if (text.Length <= m_nameLimit)
            {
                m_lastName = text;
            }
        }

        //目标文本失去焦点
        private void OnTargetSubmit(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                view.m_btn_add_GameButton.gameObject.SetActive(true);
                view.m_btn_checkError_GameButton.gameObject.SetActive(false);
                view.m_pl_checkGood_PolygonImage.gameObject.SetActive(false);
                return;
            }
            if (text.Length > m_nameLimit)
            {
                view.m_ipt_mailtarget_GameInput.text = m_lastName;
                return;
            }
            else
            {
                m_lastName = text;
            }
            RequestNameValid(text);
        }

        //标题改变
        private void OnTitleValueChanged(string text)
        {
            if (text.Length > m_titleLimit)
            {
                view.m_ipt_mailTitle_GameInput.text = m_lastTitle;
            }
            else
            {
                m_lastTitle = text;
            }
        }

        //标题失去焦点
        private void OnTitleValueSubmit(string text)
        {
            SetSendBtnStatus();
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
            if (m_type == EnumWriteAMailType.List)
            {
                if (m_targetList.Count < 1)
                {
                    isGray = true;
                }
            }
            else
            {
                if (!view.m_pl_checkGood_PolygonImage.gameObject.activeSelf)
                {
                    isGray = true;
                }
            }
            if (string.IsNullOrEmpty(view.m_ipt_mailTitle_GameInput.text))
            {
                isGray = true;
            }
            if (string.IsNullOrEmpty(view.m_ipt_mailContent_GameInput.text))
            {
                isGray = true;
            }
            if (isGray)
            {
                view.m_btn_send_MakeChildrenGray.Gray();
            } else
            {
                view.m_btn_send_MakeChildrenGray.Normal();
            }
            m_isCanSend = !isGray;
        }

        //发送邮件
        private void OnSend()
        {
            if (m_isCheckNameValiding)
            {
                return;
            }
            if (!m_isCanSend)
            {
                return;
            }

            string title = view.m_ipt_mailTitle_GameInput.text;
            string content = view.m_ipt_mailContent_GameInput.text;

            //玩家发送邮件间隔是否超过配置的时间
            int times = PlayerPrefs.GetInt("LastSendEmailTime");
            if (times > 0)
            {
                int diffTime = (int)ServerTimeModule.Instance.GetServerTime() - times;
                if (diffTime >= 0 && diffTime <= m_emailTimeInterval)
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

            BannedWord.CheckChatBannedWord(title);
            BannedWord.CheckChatBannedWord(content);

            ////检查敏感字
            //if (BannedWord.CheckChatHasBannedWord(title))
            //{
            //    Tip.CreateTip(570057).Show();
            //    return;
            //}
            //if (BannedWord.CheckChatHasBannedWord(content))
            //{
            //    Tip.CreateTip(570058).Show();
            //    return;
            //}

            //联盟邮件处理
            if (m_singleTargetData!=null && m_singleTargetData.isGuildMail)
            {
                Email_SendGuildEmail.request res = new Email_SendGuildEmail.request()
                {
                    subTitle = view.m_ipt_mailTitle_GameInput.text,
                    emailContent = view.m_ipt_mailContent_GameInput.text,
                    receiverInfo = LanguageUtils.getTextFormat(300030, m_allianceProxy.GetAbbreviationName(), m_playerProxy.CurrentRoleInfo.name)
                };
                AppFacade.GetInstance().SendSproto(res);
                PlayerPrefs.SetInt("LastSendEmailTime", (int)ServerTimeModule.Instance.GetServerTime());
                return;
            }

            List<WriteAMailData> titleList = new List<WriteAMailData>();
            List<Receiver> sendlist = new List<Receiver>();
            if (m_type == EnumWriteAMailType.List) //列表多人模式
            {
                for (int i = 0; i < m_targetList.Count; i++)
                {
                    titleList.Add(m_targetList[i].MailData);
                    Receiver receiver = new Receiver();
                    receiver.rid = m_targetList[i].MailData.stableRid;
                    receiver.gameNode = m_playerProxy.GetRoleLoginRes().chatServerName;
                    sendlist.Add(receiver);
                }
            }
            else//单人
            {
                if (m_singleTargetData == null)
                {
                    Debug.LogError("目标不存在");
                    return;
                }

                WriteAMailData targetData = new WriteAMailData();
                targetData.stableRid = m_singleTargetData.stableRid;
                targetData.stableName = m_singleTargetData.stableName;
                targetData.GuildAbbName = m_singleTargetData.GuildAbbName;
                targetData.headId = m_singleTargetData.headId;
                targetData.headFrameID = m_singleTargetData.headFrameID;
                titleList.Add(targetData);

                Receiver receiver = new Receiver();
                receiver.rid = m_singleTargetData.stableRid;
                receiver.gameNode = m_playerProxy.GetRoleLoginRes().chatServerName;
                sendlist.Add(receiver);
            }
            if (sendlist.Count < 1)
            {
                Debug.LogError("目标为空--");
                return;
            }

            if (mailNum != -1)
            {
                int tCount = (int)m_playerProxy.CurrentRoleInfo.emailSendCntPerHour + sendlist.Count;
                int diff = mailNum - tCount;
                if (diff < 0)
                {
                    string str = LanguageUtils.getTextFormat(570055, level, mailNum);
                    Tip.CreateTip(str).Show();
                    return;
                }
            }

            //拼接标题
            string nameStr = "";
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

            //保存联系人 排除自己
            int total = titleList.Count - 1;
            for (int i = total; i >=0; i--)
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
            ClientUtils.Print(sendlist);

            //发包
            var sp = new Email_MsgSendPrivateEmail.request();
            sp.lst = sendlist;
            sp.title = view.m_ipt_mailTitle_GameInput.text;
            sp.content = view.m_ipt_mailContent_GameInput.text;
            sp.receiverInfo = nameStr;
            sp.isReply = false;
            AppFacade.GetInstance().SendSproto(sp);
            PlayerPrefs.SetInt("LastSendEmailTime", (int)ServerTimeModule.Instance.GetServerTime());
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_writeAMail);
        }

        //请求名称是否有用
        private void RequestNameValid(string name)
        {
            m_isCheckNameValiding = true;
            var sp = new Role_QueryRoleName.request();
            sp.name = name;
            AppFacade.GetInstance().SendSproto(sp);
        }

        private void QueryRoleProcess(object body)
        {
            m_isCheckNameValiding = false;
            var response = body as Role_QueryRoleName.response;
            if (response.rid == 0) //数据不存在
            {
                view.m_pl_checkGood_PolygonImage.gameObject.SetActive(false);
                view.m_btn_checkError_GameButton.gameObject.SetActive(true);
            }
            else
            {
                if (m_singleTargetData == null)
                {
                    m_singleTargetData = new WriteAMailData();
                }
                m_singleTargetData.stableName = response.name;
                m_singleTargetData.stableRid = response.rid;
                m_singleTargetData.GuildAbbName = response.guildAbbr;
                m_singleTargetData.headId = response.headId;
                m_singleTargetData.headFrameID = response.headFrameID;

                //Debug.LogError("发现数据");
                ClientUtils.Print(response);

                view.m_pl_checkGood_PolygonImage.gameObject.SetActive(true);
                view.m_btn_checkError_GameButton.gameObject.SetActive(false);
            }
            view.m_btn_add_GameButton.gameObject.SetActive(false);
            SetSendBtnStatus();
        }

        private void SendResult(bool isSuccess)
        {
            if (isSuccess)
            {
                Tip.CreateTip(570056).Show();
                m_isSaveDraft = false;
                OnClose();
            }
        }
    }
}