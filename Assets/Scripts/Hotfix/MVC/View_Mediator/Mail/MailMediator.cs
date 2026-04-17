// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    MailMediator
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
using System.Text;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

namespace SprotoType
{
    public class BattleReport2 : BattleReport
    {
        public bool isSavage;       //是否是野蛮人

        public long totalBattlePower;   //开始阶段总战斗力
        public long leftBattlePower;    //战斗结束剩余战斗力

        public long startTime;          //战斗开始时间
        public long endTime;            //战斗结束时间

        public string StartWarPlayerIcon; //发起战斗角色头像
        public string StartWarPlayerName; //发起战斗角色名字

        public List<BattleData> battleData = new List<BattleData>();   //具体每一场战斗信息

        public long food;               //战斗获得资源
        public long wood;
        public long stone;
        public long gold;
        public long denar;

        public List<long> rewards = new List<long>();

        public class BattleData
        {
            public long startTime;  //本场战斗开始时间
            public long endTime;    //本场战斗结束时间

            public PosInfo pos;     //战斗位置
            public List<PersonalData> personalData = new List<PersonalData>();//角色信息
        }

        public class PersonalData
        {
            public long rid;
            public string name;
            public string icon;
            public PosInfo pos;
            public long power;//扣除战力
            public HeroInfo hero1;
            public long exp1;
            public HeroInfo hero2;
            public long exp2;

            public long total;//部队总数
            public long heal;//治疗数量
            public long dead;//死亡
            public long hurt;//重伤
            public long left;//剩余
            public long demage;//警戒塔

            public bool winner;//是否战胜
        }

    }
}
namespace Game
{

    public class MailMediator : GameMediator {
        #region Member
        public static string NameMediator = "MailMediator";

        private EmailProxy m_emailProxy;
        private PlayerProxy m_playerProxy;

        private UI_Item_MailPageBtn_SubView[] m_pages = new UI_Item_MailPageBtn_SubView[6];

        private int m_currentPage;
        private EmailType m_currentType;

        private bool m_loadAssetFinished;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private Dictionary<long, EmailInfoEntity> m_allEmails = new Dictionary<long, EmailInfoEntity>();
        private List<EmailInfoEntity> m_currentTypeEmails = new List<EmailInfoEntity>();
        //private List<ItemPackageDefine> m_itemPacks = new List<ItemPackageDefine>();
        //红点
        private int[] m_pageRedpoints = new int[6];

        //采集报告
        private List<EmailInfoEntity> m_collectionReport = new List<EmailInfoEntity>();
        //探索报告
        private List<EmailInfoEntity> m_exploreReport = new List<EmailInfoEntity>();
        //援助报告
        private List<EmailInfoEntity> m_assistReport = new List<EmailInfoEntity>();
        //联盟留言
        private List<EmailInfoEntity> m_leaveMsgList = new List<EmailInfoEntity>();
        //被侦查
        private List<EmailInfoEntity> m_spyOnList = new List<EmailInfoEntity>();

        private long m_selectEmailIndex;
        private int m_currentSelectItemIndex;
        private EmailInfoEntity m_currentSelectEmail;
        private MailDefine m_currentMailDefine;

        //战斗报告
        private EmailInfoEntity m_currentBattleReport;

        private bool m_isEmailItemListInited;
        private bool m_isEmailContentListInited;

        //折线图的颜色
        private Color FormulaColor = Color.white;

        //上次的邮件标题的图片背景
        private string m_lastEmailTitleImgBg = string.Empty;

        private GameObject m_replyEmailReport;

        //private IGGTranslator m_translator;
        private long m_translatorEmailIndex;

        private UI_Item_MailBtn_SubView m_selectItem;

        private bool m_isJumpHandle;
        private int m_jumpPageType;
        private long m_jumpEmailIndex = -1;
        private int m_jumpIndex = -1;

        private string m_contentAssetName;
        private GameObject m_contentNode;

        #endregion

        //IMediatorPlug needs
        public MailMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public MailView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdateEmail,
                Email_DeleteEmail.TagName,
                CmdConstant.ChangeSelectEmailIndex,
                CmdConstant.GetEmailFightDetail,
                Email_GetEmailInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateEmail:
                    if (!m_loadAssetFinished)
                    {
                        return;
                    }
                    if (notification.Body == null)
                    {
                        RefreshView();
                    }
                    else
                    {
                        ReadPageData();
                        RefreshPageMenuReddot();

                        int findItemIndex = -1;
                        long emailIndex = (long)notification.Body;
                        for (int i = 0; i < m_currentTypeEmails.Count; i++)
                        {
                            if (m_currentTypeEmails[i].emailIndex == emailIndex)
                            {
                                findItemIndex = i;
                                break;
                            }
                        }
                        if (findItemIndex == -1)
                        {
                            RefreshView();
                        }
                        else
                        {
                            if (emailIndex == m_selectEmailIndex && findItemIndex == m_currentSelectItemIndex)
                            {
                                view.m_sv_list_view_ListView.RefreshItem(m_currentSelectItemIndex);
                                RefreshRightContent();
                            }
                            else
                            {
                                RefreshView();
                            }
                        }
                    }
                    break;
                case Email_DeleteEmail.TagName:
                    Email_DeleteEmail.response delReponse = notification.Body as Email_DeleteEmail.response;
                    if (delReponse == null)
                    {
                        return;
                    }
                    m_emailProxy.DeleteEmailIndex = -1;
                    m_emailProxy.DeleteEmailListIndex = -1;
                    if (delReponse.type == 1)
                    {
                        //服务器端未成功删除文件 客户端强制删除
                        if (m_emailProxy.GetEmails.ContainsKey(delReponse.data))
                        {
                            Debug.LogFormat("强制删除邮件：{0}", delReponse.data);
                            m_emailProxy.ForceRemoveEmail(delReponse.data);
                            RefreshView();
                        }
                    }
                    break;
                case CmdConstant.ChangeSelectEmailIndex:
                    //邮件切换到上一封
                    if (m_emailProxy.DeleteEmailListIndex > 0)
                    {
                        Debug.Log("切换到上一封邮件");
                        int index = (int)m_emailProxy.DeleteEmailListIndex - 1;
                        if (index < m_currentTypeEmails.Count)
                        {
                            SetSelectItemData(index);
                        }
                    }
                    break;
                case CmdConstant.GetEmailFightDetail://收到战斗详情数据
                    RefreshFightDetail((long)notification.Body);
                    break;
                case Email_GetEmailInfo.TagName:
                    Email_GetEmailInfo.response response = notification.Body as Email_GetEmailInfo.response;
                    if (response != null)
                    {
                        if (response.emailIndexs != null)
                        {
                            for (int i = 0; i < response.emailIndexs.Count; i++)
                            {
                                m_emailProxy.PersonalIndexDic.Remove(response.emailIndexs[i]);
                            }
                        }
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

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            if (IsTouchBegin())
            { 
                if (m_replyEmailReport != null && m_replyEmailReport.gameObject.activeSelf)
                {
                    //Touch touch = Input.GetTouch(0);                    
                    //Vector2 screenPos = new Vector2(touch.position.x, touch.position.y);
                    Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    bool isClickRect = RectTransformUtility.RectangleContainsScreenPoint(m_replyEmailReport.GetComponent<RectTransform>(), screenPos, CoreUtils.uiManager.GetUICamera());
                    if (!isClickRect)
                    {
                        m_replyEmailReport.gameObject.SetActive(false);
                        m_replyEmailReport = null;
                    }
                }
               
            }
        }        

        protected override void InitData()
        {
            IsOpenUpdate = true;
            ColorUtility.TryParseHtmlString("#019ABF", out FormulaColor);
            m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            view.m_pl_mailNone.gameObject.SetActive(false);

            //m_translator = GameHelper.GetTranslator();
        }

        protected override void BindUIEvent()
        {
            view.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_UI_Item_newMail.m_btn_newMail_GameButton.onClick.AddListener(OnWriteEmail);
        }

        protected override void BindUIData()
        {
            List<string> prefabs = new List<string>();
            prefabs.AddRange(view.m_sv_haveMail_ListView.ItemPrefabDataList);
            prefabs.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            prefabs.Add("UI_Item_CollectReport");//采集报告
            prefabs.Add("UI_Model_MailGift");//物品图标
            prefabs.Add("UI_Item_WarMailBar");//战报进度条
            prefabs.Add("UI_Item_WarMailWar");//战报
            prefabs.Add("UI_Item_ItemSize65");//战报战利品
            prefabs.Add("UI_Item_Assistance");//联盟援助
            ClientUtils.PreLoadRes(view.gameObject,prefabs, OnLoadFinish);
        }
       
        #endregion

        private void OnLoadFinish(Dictionary<string,GameObject> asset)
        {
            m_loadAssetFinished = true;
            m_assetDic = asset;
            InitPage();
        }

        private void InitPage()
        {
            if (view.data is EmailInfoEntity)
            {
                EmailInfoEntity emailInfo = view.data as EmailInfoEntity;
                MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)emailInfo.emailId);
                if (mailDefine != null)
                {
                    m_jumpEmailIndex = emailInfo.emailIndex;
                    m_isJumpHandle = true;
                    m_jumpPageType = mailDefine.type - 1;
                }
            }

            m_pages[0] = view.m_UI_Item_MailPageBtnSys;
            m_pages[1] = view.m_UI_Item_MailPageBtnReport;
            m_pages[2] = view.m_UI_Item_MailPageBtnGuild;
            m_pages[3] = view.m_UI_Item_MailPageBtnPerson;
            m_pages[4] = view.m_UI_Item_MailPageBtnSend;
            m_pages[5] = view.m_UI_Item_MailPageBtnCollect;

            if (m_isJumpHandle)
            {
                m_currentPage = m_jumpPageType;
                m_emailProxy.MailCurrentPage = m_jumpPageType;
            }
            else
            {
                m_currentPage = m_emailProxy.MailCurrentPage >= m_pages.Length ? 0 : m_emailProxy.MailCurrentPage;
            }

            for (int i = 0; i < m_pages.Length; i++)
            {
                int k = i;
                m_pages[k].m_btn_btn_GameButton.onClick.AddListener(() =>
                {
                    m_emailProxy.MailCurrentPage = k;
                    if (m_currentPage != k)
                    {
                        m_emailProxy.AddMenuSwitchVal();
                    }
                    m_currentPage = k;
                    RefreshPageMenuSelectStatus();

                    SwitchMenu();
                });
                m_pages[k].m_img_active_PolygonImage.gameObject.SetActive(k == m_currentPage);
            }
            RefreshPageMenuSelectStatus();

            SwitchMenu();

            //刷新分页红点
            RefreshPageMenuReddot();
        }

        private void RefreshPageMenuSelectStatus()
        {
            for (int i = 0; i < m_pages.Length; i++)
            {
                bool isSelect = i == m_currentPage;
                m_pages[i].m_img_active_PolygonImage.gameObject.SetActive(isSelect);
                if (isSelect)
                {
                    ClientUtils.TextSetColor(m_pages[i].m_lbl_languageText_LanguageText, "#ffffff");
                }
                else
                {
                    ClientUtils.TextSetColor(m_pages[i].m_lbl_languageText_LanguageText, "#a49d92");
                }
            }
        }

        private void ReadPageData()
        {
            m_allEmails = m_emailProxy.GetEmails;
            int page = m_currentPage + 1;
            m_currentTypeEmails = GetEmailListByType((EmailType)page);
        }

        private void SwitchMenu()
        {
            //获取分页数据
            ReadPageData();

            if (m_currentTypeEmails.Count > 0)
            {
                if (m_isJumpHandle)
                {
                    m_isJumpHandle = false;
                    for (int i = 0; i < m_currentTypeEmails.Count; i++)
                    {
                        if (m_currentTypeEmails[i].emailIndex == m_jumpEmailIndex)
                        {
                            m_jumpIndex = i;
                            break;
                        }
                    }
                    if (m_jumpIndex < 0)
                    {
                        SetSelectItemData(0);
                    }
                    else
                    {
                        SetSelectItemData(m_jumpIndex);
                    }
                }
                else
                {
                    SetSelectItemData(0);
                }
            }
            else
            {
                SetSelectItemData(-1);
            }

            //刷新邮件列表
            RefreshEmailList();

            //刷新底部 删除已读和一键领取按钮
            RefreshDelAndReceiveBtn();

            //更新内容
            RefreshRightContent();
        }

        private void RefreshView(int index = -1)
        {
            //获取当前分页数据
            ReadPageData();

            int count = m_currentTypeEmails.Count;
            bool hasSelectEmail = false;
            int findIndex = 0;
            for (int i = 0; i < count; i++)
            {
                if (m_currentTypeEmails[i].emailIndex == m_selectEmailIndex)
                {
                    hasSelectEmail = true;
                    findIndex = i;
                    break;
                }
            }
            if (hasSelectEmail)
            {
                SetSelectItemData(findIndex);
            }
            else
            {
                int tIndex = m_currentSelectItemIndex - 1;
                if (count > 0)
                {
                    if (tIndex >= 0 && tIndex < count)
                    {
                        SetSelectItemData(tIndex);
                    }
                    else
                    {
                        SetSelectItemData(0);
                    }
                }
                else
                {
                    SetSelectItemData(-1);
                }
            }

            //刷新分页红点
            RefreshPageMenuReddot();

            //刷新邮件简要列表
            RefreshEmailList();

            //更新内容
            RefreshRightContent();
        }

        private void RefreshRightContent()
        {
            if (m_currentTypeEmails.Count == 0)
            {
                //没有邮件
                view.m_pl_mailcontentbg_PolygonImage.gameObject.SetActive(false);
                view.m_pl_mailNone.gameObject.SetActive(true);
                view.m_sv_haveMail_ListView.gameObject.SetActive(false);
                view.m_UI_Item_MailFightReport.gameObject.SetActive(false);
                view.m_UI_Item_MailBtnOnButtomDeleteRead.gameObject.SetActive(false);
                view.m_UI_Item_MailBtnOnButtomRead.gameObject.SetActive(false);

                //刷新底部 其他按钮
                RefreshDownOtherBtn(null, null);
            }
            else
            {
                //有邮件
                UpdateMailDetail();
            }
        }

        private void UpdateMailDetail()
        {
            if (m_currentSelectItemIndex < 0 || m_currentSelectItemIndex >= m_currentTypeEmails.Count)
            {
                RefreshDownOtherBtn(null, null);
                return;
            }

            //设置邮件已读状态
            if (m_currentTypeEmails[m_currentSelectItemIndex].status == 0)
            {
                Email_UpdateEmailStatus.request req = new Email_UpdateEmailStatus.request
                {
                    emailIndexs = new List<long> { m_currentTypeEmails[m_currentSelectItemIndex].emailIndex }
                };
                AppFacade.GetInstance().SendSproto(req);
            }

            //有邮件
            view.m_pl_mailcontentbg_PolygonImage.gameObject.SetActive(true);
            view.m_pl_mailNone.gameObject.SetActive(false);
            view.m_UI_Item_MailBtnOnButtomDeleteRead.gameObject.SetActive(true);
            view.m_UI_Item_MailBtnOnButtomRead.gameObject.SetActive((EmailType)m_currentPage != EmailType.sent);

            //刷新底部 其他按钮
            RefreshDownOtherBtn(m_currentMailDefine, m_currentSelectEmail);

            //请求邮件详情内容
            bool isRequest = RequestEmailDetail(m_currentSelectEmail, m_currentMailDefine);
            if (isRequest)
            {
                //尚未获取到数据 清空详情
                ClearEmailContent();
            }
            else
            {
                //数据已请求 直接显示详情
                ShowEmailContent();
            }
        }

        //请求邮件内容
        private bool RequestEmailDetail(EmailInfoEntity info, MailDefine mailDefine)
        {
            bool reSendSproto = false;
            bool isSend = false;
            if (info.senderInfo != null) //个人邮件需要请求详情
            {
                if (info.emailContents == null)
                {
                    if (!m_emailProxy.PersonalIndexDic.ContainsKey(info.emailIndex))
                    {
                        Debug.Log("请求个人邮件内容");
                        m_emailProxy.PersonalIndexDic[info.emailIndex] = true;
                        reSendSproto = true;
                    }
                    isSend = true;
                }
            }
            else if (info.subType == 2) //战报需要请求信息
            {
                Debug.LogFormat("reportStatus：{0}", info.reportStatus);
                if (info.reportStatus == 0)
                {
                    if (!m_emailProxy.BattleReportIndexList.Contains(info.emailIndex))
                    {
                        m_emailProxy.BattleReportIndexList.Add(info.emailIndex);
                        reSendSproto = true;
                        isSend = true;
                    }
                }
                else if (info.reportStatus == 1) //请求url
                {
                    if (string.IsNullOrEmpty(info.battleReportEx))
                    {
                        //请求url
                        if (!m_emailProxy.BattleReportIndexList.Contains(info.emailIndex))
                        {
                            Debug.Log("请求url");
                            m_emailProxy.BattleReportIndexList.Add(info.emailIndex);
                            reSendSproto = true;
                            isSend = true;
                        }
                    }
                    else
                    {
                        //请求战斗详情
                        if (!m_emailProxy.IsGetBattleReportEx(info.emailIndex))
                        {
                            m_emailProxy.RequestFightDetail(info.emailIndex, info.battleReportEx);
                            return true;
                        }
                    }
                }
                else if (info.reportStatus == 2) //请求battleReportExContent
                {
                    if (!m_emailProxy.IsGetBattleReportEx(info.emailIndex))
                    {
                        //请求battleReportExContent
                        if (!m_emailProxy.BattleReportIndexList.Contains(info.emailIndex))
                        {
                            Debug.Log("请求battleReportExContent");
                            m_emailProxy.BattleReportIndexList.Add(info.emailIndex);
                            reSendSproto = true;
                            isSend = true;
                        }
                    }
                }
                else
                {
                    Debug.LogFormat("邮件战报异常状态reportStatus:{0}", info.reportStatus);
                }
            }
            else
            {
                if (mailDefine != null && mailDefine.group == 19 && info.roleList == null)
                {
                    if (!m_emailProxy.RequestedIndexDic.ContainsKey(info.emailIndex))
                    {
                        m_emailProxy.RequestedIndexDic[info.emailIndex] = true;
                        reSendSproto = true;
                        isSend = true;
                    }
                }
            }

            if (reSendSproto)
            {
                Debug.Log("请求邮件--");
                Email_GetEmailInfo.request req2 = new Email_GetEmailInfo.request
                {
                    emailIndexs = new List<long>
                    {
                        info.emailIndex
                    }
                };
                AppFacade.GetInstance().SendSproto(req2);
            }
            return isSend;
        }

        //刷新分页菜单红点
        private void RefreshPageMenuReddot()
        {
            for (int i = 0; i < m_pages.Length; i++)
            {
                if (m_pageRedpoints[i] > 0)
                {
                    m_pages[i].m_img_reddot_PolygonImage.gameObject.SetActive(true);
                    string countStr = UIHelper.NumerBeyondFormat(m_pageRedpoints[i]);
                    m_pages[i].m_lbl_reddotcount_LanguageText.text = countStr;
                }
                else
                {
                    m_pages[i].m_img_reddot_PolygonImage.gameObject.SetActive(false);
                }
            }
        }

        #region 邮件简要列表

        private void RefreshEmailList()
        {
            if (!m_isEmailItemListInited)
            {
                m_isEmailItemListInited = true;
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = OnEmailItemEnter;
                view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
            }
            view.m_sv_list_view_ListView.FillContent(m_currentTypeEmails.Count);
            if (m_jumpIndex > -1)
            {
                int jumpIndex = m_jumpIndex;
                m_jumpIndex = -1;
                view.m_sv_list_view_ListView.MovePanelToItemIndex(jumpIndex);
            }
        }

        // 邮件列表
        private void OnEmailItemEnter(ListView.ListItem item)
        {
            int index = item.index;
            UI_Item_MailBtn_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_MailBtn_SubView(item.go.GetComponent<RectTransform>());
                subView.BtnClickEvent = EmailClickEvent;
                subView.Refresh(m_currentTypeEmails[index], index, m_selectEmailIndex);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_MailBtn_SubView;
                subView.Refresh(m_currentTypeEmails[index], index, m_selectEmailIndex);
            }
            if (m_currentTypeEmails[index].emailIndex == m_selectEmailIndex)
            {
                m_selectItem = subView;
            }
        }

        private void EmailClickEvent(UI_Item_MailBtn_SubView subView)
        {
            int index = subView.GetIndex();
            if (m_currentSelectItemIndex != index)
            {
                m_emailProxy.AddMenuSwitchVal();
            }

            SetSelectItemData(index);

            //刷新旧的item
            if (m_selectItem != null && subView != m_selectItem)
            {
                view.m_sv_list_view_ListView.RefreshItem(m_selectItem.GetIndex());
            }
            m_selectItem = subView;
            //刷新选中item
            view.m_sv_list_view_ListView.RefreshItem(index);

            //刷新邮件详情
            UpdateMailDetail();
        }

        private void SetSelectItemData(int index)
        {
            if (index >= 0)
            {
                m_currentSelectItemIndex = index;
                m_currentSelectEmail = m_currentTypeEmails[m_currentSelectItemIndex];
                m_selectEmailIndex = m_currentSelectEmail.emailIndex;
                m_currentMailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)m_currentSelectEmail.emailId);
            }
            else
            {
                m_currentSelectItemIndex = -1;
                m_currentSelectEmail = null;
                m_selectEmailIndex = -1;
                m_currentMailDefine = null;
            }
        }

        #endregion

        #region 邮件详情模块

        private void ClearEmailContent()
        {
            switch (m_currentMailDefine.group)
            {
                case 2: //战斗战报
                    view.m_sv_haveMail_ListView.gameObject.SetActive(false);
                    view.m_UI_Item_MailFightReport.gameObject.SetActive(true);
                    view.m_UI_Item_MailFightReport.Refresh(m_currentMailDefine, m_currentSelectEmail);
                    break;
                default: //其他
                    view.m_sv_haveMail_ListView.gameObject.SetActive(true);
                    view.m_UI_Item_MailFightReport.gameObject.SetActive(false);
                    InitDetailList();
                    //刷新邮件内容
                    view.m_sv_haveMail_ListView.FillContent(0);
                    break;
            }
        }

        private void ShowEmailContent()
        {
            switch (m_currentMailDefine.group)
            {
                case 2: //战斗战报
                    view.m_sv_haveMail_ListView.gameObject.SetActive(false);
                    view.m_UI_Item_MailFightReport.gameObject.SetActive(true);
                    view.m_UI_Item_MailFightReport.Refresh(m_currentMailDefine, m_currentSelectEmail);
                    break;
                default: //其他
                    view.m_sv_haveMail_ListView.gameObject.SetActive(true);
                    view.m_UI_Item_MailFightReport.gameObject.SetActive(false);
                    InitDetailList();
                    //刷新邮件内容
                    view.m_sv_haveMail_ListView.FillContent(1);
                    break;
            }
        }

        //private void HideOtherNode()
        //{
        //    if (m_contentNode != null)
        //    {
        //        m_contentNode.SetActive(false);
        //    }
        //}

        private void InitDetailList()
        {
            if (!m_isEmailContentListInited)
            {
                m_isEmailContentListInited = true;
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = OnEmailContentItem;
                funcTab.GetItemPrefabName = GetEmailContentPrefabName;
                view.m_sv_haveMail_ListView.SetInitData(m_assetDic, funcTab);
            }
        }

        //邮件内容预制
        private string GetEmailContentPrefabName(ListView.ListItem item)
        {
            if (m_currentMailDefine != null)
            {
                Debug.LogFormat("emailId:{0}", m_currentMailDefine.ID);
                switch (m_currentMailDefine.group)
                {
                    case 1://系统邮件
                        return "UI_Item_MailTypeSystem";
                    //case 2://战报
                    //    return "UI_Item_MailTypeWar";
                    case 3://采集报告
                        return "UI_Item_MailTypeCollect";
                    case 4://探索报告
                        return "UI_Item_MailType4";
                    case 5://联盟任命报告
                        return "UI_Item_MailType5";
                    case 6://入盟邀请
                        return "UI_Item_MailType6";
                    case 7://联盟领土报告
                        return "UI_Item_MailType7";
                    case 8://联盟捐献 
                        return "UI_Item_MailType8";
                    case 10://联盟科技
                        return "UI_Item_MailType10";
                    case 11://联盟援助
                        return "UI_Item_MailType11";
                    case 12://联盟留言
                        return "UI_Item_MailType12";
                    case 13://单人邮件 多人邮件 普通邮件
                        return "UI_Item_MailType13";
                    case 14://回复邮件 
                        return "UI_Item_MailType13";
                    case 15://奇观占领 
                        return "UI_Item_MailType15";
                    case 16://侦查邮件
                        return "UI_Item_MailType16";
                    case 17://被侦查邮件
                        return "UI_Item_MailType17";
                    case 18://联盟不活跃成员报告邮件
                        return "UI_Item_MailType18";
                    case 19://活动邮件
                        return "UI_Item_MailType19";
                    case 20://图片邮件
                        return "UI_Item_MailType20";
                    default:
                        Debug.LogErrorFormat("not find group: {0}",m_currentMailDefine.group);
                        break;
                }
            }
            return string.Empty;
        }

        //邮件内容
        private void OnEmailContentItem(ListView.ListItem item)
        {
            if(m_currentMailDefine!=null)
            {
                Debug.LogFormat("email group:{0}", m_currentMailDefine.group);
                switch(m_currentMailDefine.group)
                {
                    case 1://系统邮件
                        OnSystemEmail(item);
                        break;
                    //case 2://战报
                    //    OnBattleReport2(item);
                    //    break;
                    case 3://采集报告
                        OnCollectionReport(item);
                        break;
                    case 4://探索报告
                        OnExploreReport(item);
                        break;
                    case 5://联盟任命报告
                        OnAllianceAppoint(item);
                        break;
                    case 6://入盟邀请
                        OnJoinAlliance(item);
                        break;
                    case 7://联盟领土报告
                        OnAllianceTerritory(item);
                        break;
                    case 8://联盟捐献
                        OnAllianceDonate(item);
                        break;
                    case 10://联盟科技
                        OnAllianceTechnology(item);
                        break;
                    case 11://联盟援助
                        OnAllianceAssist(item);
                        break;
                    case 12://联盟留言
                        OnAllianceLeaveMsg(item);
                        break;
                    case 13://玩家邮件|单人邮件 多人邮件 普通邮件
                        OnPersonalEmail(item);
                        break;
                    case 14://玩家邮件|回复邮件
                        OnPersonalEmail(item);
                        break;
                    case 15://奇观占领 
                        OnWonderOccupy(item);
                        break;
                    case 16://侦查邮件
                        SpyOnEmail(item);
                        break;
                    case 17://被侦查邮件
                        SpyOnEmail2(item);
                        break;
                    case 18://联盟不活跃成员报告邮件
                        AllianceNoActiveEmaill(item);
                        break;
                    case 19://活动邮件
                        ActivityEmail(item);
                        break;
                    case 20://图片邮件
                        ImageEmail(item);
                        break;
                    default:break;
                }
            }
        }

        //刷新战斗详情
        private void RefreshFightDetail(long emailIndex)
        {
            if(emailIndex != m_selectEmailIndex)
            {
                return;
            }
            if (m_currentTypeEmails.Count == 0)
            {
                return;
            }
            ShowEmailContent();
        }

        #region 个人|已发送

        //翻译邮件
        private void TranslateEmail(MailDefine mailDefine, EmailInfoEntity info)
        {
            List<LanguageText> textNodeList = GetNeedTranslationTextList(mailDefine);
            if (textNodeList.Count == 0)
            {
                Debug.LogErrorFormat("翻译尚未适配 group:{0} email:{1}", mailDefine.group, mailDefine.ID);
                return;
            }
            m_translatorEmailIndex = info.emailIndex;
            List<SDKTranslationSource> textList = new List<SDKTranslationSource>();
            for (int i = 0; i < textNodeList.Count; i++)
            {
                textList.Add(new SDKTranslationSource(textNodeList[i].BaseText));
            }
            GameHelper.GetTranslator().translateTexts(textList, (value1) => {
                if (m_currentSelectEmail != null && m_currentSelectEmail.emailIndex == m_translatorEmailIndex)
                {
                    List<LanguageText> textNodeList2 = GetNeedTranslationTextList(m_currentMailDefine);
                    for (int i = 0; i < textNodeList2.Count; i++)
                    {
                        SDKTranslation valTrans = value1.getByIndex(i);
                        if (valTrans != null)
                        {
                            textNodeList2[i].text = valTrans.getText();
                        }
                    }
                }
            }, (value2, value3) =>
            {
                Tip.CreateTip(750007).Show();
            });
        }

        private List<LanguageText> GetNeedTranslationTextList(MailDefine mailDefine)
        {
            List<LanguageText> textList = new List<LanguageText>();
            if (mailDefine.group == 13 || mailDefine.group == 14)
            {
                ListView.ListItem item = view.m_sv_haveMail_ListView.GetItemByIndex(0);
                if (item !=null)
                {
                    UI_Item_MailType13View itemView = MonoHelper.GetHotFixViewComponent<UI_Item_MailType13View>(item.go);
                    if (itemView != null)
                    {

                        textList.Add(itemView.m_UI_Item_MailTitle.m_lbl_desc_LanguageText);
                        textList.Add(itemView.m_lbl_Content_LanguageText);
                    }
                }
            }
            return textList;
        }

        private void OnPersonalEmail(ListView.ListItem item)
        {
            UI_Item_MailType13View itemView = MonoHelper.AddHotFixViewComponent<UI_Item_MailType13View>(item.go);

            if (m_currentSelectEmail.senderInfo != null)
            {
                //个人邮件
                itemView.m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = GetPersonalTilte(m_currentMailDefine, m_currentSelectEmail);
            }
            else
            {
                itemView.m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_nameID), m_currentSelectEmail.titleContents);
            }
            itemView.m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_subheadingID), m_currentSelectEmail.subTitleContents);
            itemView.m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(m_currentSelectEmail.sendTime);

            if (m_currentSelectEmail.emailContents == null)
            {
                itemView.m_lbl_Content_LanguageText.text = "";
            }
            else
            {
                itemView.m_lbl_Content_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_mesID), m_currentSelectEmail.emailContents);
            }

            if (m_currentMailDefine.type == 4) //可回复邮件
            {
                itemView.m_btn_careful_GameButton.gameObject.SetActive(true);
            }
            else
            {
                itemView.m_btn_careful_GameButton.gameObject.SetActive(false);
            }
            //举报
            itemView.m_btn_careful_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_careful_GameButton.onClick.AddListener(()=> {
                itemView.m_pl_pop_ArabLayoutCompment.gameObject.SetActive(true);
                m_replyEmailReport = itemView.m_pl_pop_ArabLayoutCompment.gameObject;
            });
            itemView.m_btn_report.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_report.m_btn_languageButton_GameButton.onClick.AddListener(()=> {
                itemView.m_pl_pop_ArabLayoutCompment.gameObject.SetActive(false);
                m_replyEmailReport = null;
                string content = itemView.m_lbl_Content_LanguageText.text;
                if (m_currentSelectEmail.senderInfo == null)
                {
                    CoreUtils.logService.Warn($"所举报邮件发送者信息为空，请检查！！！！ID:{m_currentSelectEmail.emailId}");
                    return;
                }
                UIHelper.Reporting(m_currentSelectEmail.senderInfo.rid,m_currentSelectEmail.senderInfo.name,content,LanguageUtils.getText(300141));
            });
        }

        private string GetPersonalTilte(MailDefine mailDefine, EmailInfoEntity emailInfo)
        {
            string str = "";
            SenderInfo senderData = emailInfo.senderInfo;
            if (senderData.rid == m_playerProxy.CurrentRoleInfo.rid)
            {
                str = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), emailInfo.titleContents);
            }
            else
            {
                str = string.IsNullOrEmpty(senderData.guildAbbr) ? senderData.name : LanguageUtils.getTextFormat(300030, senderData.guildAbbr, senderData.name);
            }
            return str;
        }

        #endregion

        #region 联盟相关邮件

        //联盟任命报告
        private void OnAllianceAppoint(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType5_SubView subView = new UI_Item_MailType5_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType5_SubView subView = item.data as UI_Item_MailType5_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        //加入联盟
        private void OnJoinAlliance(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType6_SubView subView = new UI_Item_MailType6_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType6_SubView subView = item.data as UI_Item_MailType6_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        //联盟领土报告
        private void OnAllianceTerritory(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType7_SubView subView = new UI_Item_MailType7_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType7_SubView subView = item.data as UI_Item_MailType7_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        //联盟捐献
        private void OnAllianceDonate(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType8_SubView subView = new UI_Item_MailType8_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType8_SubView subView = item.data as UI_Item_MailType8_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        //联盟科技
        private void OnAllianceTechnology(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType10_SubView subView = new UI_Item_MailType10_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType10_SubView subView = item.data as UI_Item_MailType10_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        //联盟援助
        private void OnAllianceAssist(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType11_SubView subView = new UI_Item_MailType11_SubView(item.go.GetComponent<RectTransform>());
                subView.AssetDic = m_assetDic;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_assistReport);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType11_SubView subView = item.data as UI_Item_MailType11_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_assistReport);
            }
        }

        //联盟留言
        private void OnAllianceLeaveMsg(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType12_SubView subView = new UI_Item_MailType12_SubView(item.go.GetComponent<RectTransform>());
                //subView.AssetDic = m_assetDic;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_leaveMsgList);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType12_SubView subView = item.data as UI_Item_MailType12_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_leaveMsgList);
            }
        }

        //奇观占领
        private void OnWonderOccupy(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType15_SubView subView = new UI_Item_MailType15_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType15_SubView subView = item.data as UI_Item_MailType15_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        #endregion

        #region 侦查邮件

        //侦查邮件
        private void SpyOnEmail(ListView.ListItem item)
        {
            UI_Item_MailType16_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_MailType16_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_MailType16_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }            
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.go.GetComponent<RectTransform>());
            VerticalLayoutGroup layout = subView.m_UI_Item_MailType16_VerticalLayoutGroup;
            if (layout != null)
            {
                layout.CalculateLayoutInputVertical();
                layout.SetLayoutVertical();
                view.m_sv_haveMail_ListView.UpdateItemSize(0, layout.preferredHeight);
            }
        }

        //被侦查邮件
        private void SpyOnEmail2(ListView.ListItem item)
        {
            UI_Item_MailType17_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_MailType17_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_spyOnList);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_MailType17_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_spyOnList);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(item.go.GetComponent<RectTransform>());
            VerticalLayoutGroup layout = subView.m_UI_Item_MailType17_VerticalLayoutGroup;
            if (layout != null)
            {
                layout.CalculateLayoutInputVertical();
                layout.SetLayoutVertical();
                view.m_sv_haveMail_ListView.UpdateItemSize(0, layout.preferredHeight);
            }
        }

        //联盟不活跃成员报告邮件
        private void AllianceNoActiveEmaill(ListView.ListItem item)
        {
            UI_Item_MailType18_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_MailType18_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_MailType18_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
            view.m_sv_haveMail_ListView.UpdateItemSize(0, item.go.GetComponent<RectTransform>().rect.height);            
        }

        //活动邮件
        private void ActivityEmail(ListView.ListItem item)
        {
            UI_Item_MailType19_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_MailType19_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_MailType19_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        //图片邮件
        private void ImageEmail(ListView.ListItem item)
        {
            UI_Item_MailType20_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_MailType20_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_MailType20_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail);
            }
        }

        #endregion

        #region 系统邮件
        private void OnSystemEmail(ListView.ListItem item)
        {
            float size = 0;
            UI_Item_MailTypeSystemView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_MailTypeSystemView>(item.go);
            itemView.m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_nameID), m_currentSelectEmail.titleContents);
            itemView.m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_subheadingID), m_currentSelectEmail.subTitleContents);         
            itemView.m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(m_currentSelectEmail.sendTime);

            itemView.m_lbl_Content_link_LinkImageText.gameObject.SetActive(false);
            itemView.m_lbl_Content_LanguageText.gameObject.SetActive(false);
            if (m_currentMailDefine.l_mesID>0)
            {
                if (m_currentSelectEmail.subType == 4)
                {
                    itemView.m_lbl_Content_LanguageText.gameObject.SetActive(true);
                    itemView.m_lbl_Content_LanguageText.text = LanguageUtils.getTextFormat(m_currentMailDefine.l_mesID, m_currentSelectEmail.acitonForceReturn);
                }
                else if (m_currentSelectEmail.subType == 6 && m_currentMailDefine.group == 1 && m_currentSelectEmail.discoverReport != null)
                {
                    //探索发现报告 调查类型

                    itemView.m_lbl_Content_link_LinkImageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_mesID), m_currentSelectEmail.emailContents);
                    itemView.m_lbl_Content_link_LinkImageText.onHrefClick.RemoveAllListeners();
                    itemView.m_lbl_Content_link_LinkImageText.onHrefClick.AddListener((str) =>
                    {
                        str = m_emailProxy.CoordinateReverse(str, itemView.m_lbl_Content_link_LinkImageText.isArabicText);
                        m_emailProxy.CoordinateJump(str);
                    });
                    itemView.m_lbl_Content_link_LinkImageText.gameObject.SetActive(true);
                }
                else if (m_currentMailDefine.ID == 300013)//联盟建筑被摧毁
                {
                    itemView.m_lbl_Content_link_LinkImageText.onHrefClick.RemoveAllListeners();
                    itemView.m_lbl_Content_link_LinkImageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_mesID), m_currentSelectEmail.emailContents);
                    itemView.m_lbl_Content_link_LinkImageText.onHrefClick.AddListener((str) =>
                    {
                        str = m_emailProxy.CoordinateReverse(str, itemView.m_lbl_Content_link_LinkImageText.isArabicText);
                        m_emailProxy.CoordinateJump(str);
                    });
                    itemView.m_lbl_Content_link_LinkImageText.gameObject.SetActive(true);
                }
                else
                {
                    if (m_currentMailDefine.ID == 230000) //战损补偿邮件
                    {
                        string str1 = "";
                        if (m_currentSelectEmail.emailContents != null && m_currentSelectEmail.emailContents.Count > 0)
                        {
                            str1 = string.Join(",", m_currentSelectEmail.emailContents.ToArray());
                        }
                        itemView.m_lbl_Content_LanguageText.text = LanguageUtils.getTextFormat(m_currentMailDefine.l_mesID, str1);
                    }
                    else
                    {
                        itemView.m_lbl_Content_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_mesID), m_currentSelectEmail.emailContents);
                    }
                    itemView.m_lbl_Content_LanguageText.gameObject.SetActive(true);
                }
            }
            VerticalLayoutGroup layout = itemView.gameObject.GetComponent<VerticalLayoutGroup>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.go.GetComponent<RectTransform>());//文本会被下方的节点叠加 所以要强制刷一下

            if (m_currentMailDefine.l_senderID>0)
            {
                itemView.m_lbl_Sender_LanguageText.gameObject.SetActive(true);
                itemView.m_lbl_Sender_LanguageText.text = LanguageUtils.getText(m_currentMailDefine.l_senderID);
            }
            else
            {
                itemView.m_lbl_Sender_LanguageText.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(m_currentMailDefine.poster))
            {
                ClientUtils.LoadSprite(itemView.m_UI_Item_MailTitle.m_img_bg_PolygonImage, m_currentMailDefine.poster);
                itemView.m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                itemView.m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }
            OnEnclosureList(itemView, itemView.m_pl_gift_GridLayoutGroup.transform);
            itemView.m_pl_gift_GridLayoutGroup.gameObject.SetActive(true);
            itemView.m_pl_res_GridLayoutGroup.gameObject.SetActive(false);
            if (m_currentMailDefine.receiveAuto == 1)//如果邮件是自动领取类型
            {
                itemView.m_btn_receive.gameObject.SetActive(false);
                itemView.m_pl_already_LanguageText.gameObject.SetActive(true);
                itemView.m_pl_already_LanguageText.text = LanguageUtils.getText(570006);
            }
            else if (m_currentSelectEmail.takeEnclosure)//是否已领取附件
            {
                itemView.m_btn_receive.gameObject.SetActive(false);
                itemView.m_pl_already_LanguageText.gameObject.SetActive(true);
                itemView.m_pl_already_LanguageText.text = LanguageUtils.getText(570009);
            }
            else if (m_currentSelectEmail.rewards == null)
            {
                itemView.m_pl_gift_GridLayoutGroup.gameObject.SetActive(false);
                itemView.m_btn_receive.gameObject.SetActive(false);
                itemView.m_pl_already_LanguageText.gameObject.SetActive(false);

                if (m_currentMailDefine.ID == 300019) //联盟援助邮件
                {
                    itemView.m_pl_btn.gameObject.SetActive(false);
                    itemView.m_pl_res_GridLayoutGroup.gameObject.SetActive(true);

                    itemView.m_UI_Item_AssistanceRes1.gameObject.SetActive(false);
                    itemView.m_UI_Item_AssistanceRes2.gameObject.SetActive(false);
                    itemView.m_UI_Item_AssistanceRes3.gameObject.SetActive(false);
                    itemView.m_UI_Item_AssistanceRes4.gameObject.SetActive(false);

                    if (m_currentSelectEmail.guildEmail != null && m_currentSelectEmail.guildEmail.transportResource != null)
                    {
                        if (m_currentSelectEmail.guildEmail.transportResource.Count > 0)
                        {
                            List<CurrencyInfo> infoList = m_currentSelectEmail.guildEmail.transportResource;
                            for (int i = 0; i < infoList.Count; i++)
                            {
                                if (infoList[i].type == (long)EnumCurrencyType.food)
                                {
                                    itemView.m_UI_Item_AssistanceRes1.gameObject.SetActive(true);
                                    itemView.m_UI_Item_AssistanceRes1.m_lbl_languageText_LanguageText.text = ClientUtils.CurrencyFormat(infoList[i].num);
                                }
                                else if (infoList[i].type == (long)EnumCurrencyType.wood)
                                {
                                    itemView.m_UI_Item_AssistanceRes2.gameObject.SetActive(true);
                                    itemView.m_UI_Item_AssistanceRes2.m_lbl_languageText_LanguageText.text = ClientUtils.CurrencyFormat(infoList[i].num);
                                }
                                else if (infoList[i].type == (long)EnumCurrencyType.stone)
                                {
                                    itemView.m_UI_Item_AssistanceRes3.gameObject.SetActive(true);
                                    itemView.m_UI_Item_AssistanceRes3.m_lbl_languageText_LanguageText.text = ClientUtils.CurrencyFormat(infoList[i].num);
                                }
                                else if (infoList[i].type == (long)EnumCurrencyType.gold)
                                {
                                    itemView.m_UI_Item_AssistanceRes4.gameObject.SetActive(true);
                                    itemView.m_UI_Item_AssistanceRes4.m_lbl_languageText_LanguageText.text = ClientUtils.CurrencyFormat(infoList[i].num);
                                }
                            }
                        }
                    }
                }
            }           
            else
            {
                itemView.m_btn_receive.gameObject.SetActive(true);
                itemView.m_pl_already_LanguageText.gameObject.SetActive(false);
                itemView.m_btn_receive.m_lbl_Text_LanguageText.text = LanguageUtils.getText(570008);
                itemView.m_btn_receive.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_receive.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    //领取邮件
                    Email_TakeEnclosure.request req = new Email_TakeEnclosure.request();
                    req.type = 1;
                    req.data = m_currentSelectEmail.emailIndex;
                    AppFacade.GetInstance().SendSproto(req);
                    OnEnclosureListFly(itemView);
                });
            }
            //VerticalLayoutGroup layout = itemView.gameObject.GetComponent<VerticalLayoutGroup>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.go.GetComponent<RectTransform>());
            if (layout != null)
            {
                layout.CalculateLayoutInputVertical();
                layout.SetLayoutVertical();
                size = layout.preferredHeight;
                view.m_sv_haveMail_ListView.UpdateItemSize(0, size);
            }
        }

        private void OnEnclosureList(UI_Item_MailTypeSystemView systemView,Transform parent)
        {
            if (m_currentSelectEmail.rewards == null)
            {
                systemView.m_pl_gift_GridLayoutGroup.gameObject.SetActive(false);
                return;
            }
            systemView.m_pl_gift_GridLayoutGroup.gameObject.SetActive(true);
            int count = 0;
            count += m_currentSelectEmail.rewards.food <= 0 ? 0 : 1;
            count += m_currentSelectEmail.rewards.wood <= 0 ? 0 : 1;
            count += m_currentSelectEmail.rewards.stone <= 0 ? 0 : 1;
            count += m_currentSelectEmail.rewards.gold <= 0 ? 0 : 1;
            count += m_currentSelectEmail.rewards.denar <= 0 ? 0 : 1;
            count += m_currentSelectEmail.rewards.soldiers != null ? m_currentSelectEmail.rewards.soldiers.Count : 0;
            count += m_currentSelectEmail.rewards.items != null ? m_currentSelectEmail.rewards.items.Count : 0;
            if(parent.childCount>count)
            {
                for (int i = parent.childCount - 1; i >= count; i--)
                {
                    GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
                }
            }

            int index = -1;
            if(m_currentSelectEmail.rewards.food>0)
            {
                SetCurrencyGift(InstantiateGift(parent, ++index),(int)EnumCurrencyType.food, m_currentSelectEmail.rewards.food);
            }
            if (m_currentSelectEmail.rewards.wood > 0)
            {
                SetCurrencyGift(InstantiateGift(parent, ++index), (int)EnumCurrencyType.wood, m_currentSelectEmail.rewards.wood);
            }
            if (m_currentSelectEmail.rewards.stone > 0)
            {
                SetCurrencyGift(InstantiateGift(parent, ++index), (int)EnumCurrencyType.stone, m_currentSelectEmail.rewards.stone);
            }
            if (m_currentSelectEmail.rewards.gold > 0)
            {
                SetCurrencyGift(InstantiateGift(parent, ++index), (int)EnumCurrencyType.gold, m_currentSelectEmail.rewards.gold);
            }
            if (m_currentSelectEmail.rewards.denar > 0)
            {
                SetCurrencyGift(InstantiateGift(parent, ++index), (int)EnumCurrencyType.denar, m_currentSelectEmail.rewards.denar);
            }
            if(m_currentSelectEmail.rewards.soldiers!=null)
            {
                for(int i = 0;i< m_currentSelectEmail.rewards.soldiers.Count;i++)
                {
                    UI_Model_MailGiftView itemView = InstantiateGift(parent, ++index);
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_count_LanguageText.text = m_currentSelectEmail.rewards.soldiers[i].num.ToString("N0");
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(false);

                    ArmsDefine armsDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)m_currentSelectEmail.rewards.soldiers[i].id);
                    itemView.m_lbl_name_LanguageText.text =LanguageUtils.getText(armsDefine.l_armsID);
                    //设置icon
                    ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_icon_PolygonImage, armsDefine.icon);

                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.AddListener(() =>
                    {
                        HelpTip.CreateTip(LanguageUtils.getText(armsDefine.l_armsID), itemView.gameObject.GetComponent<Transform>()).SetOffset(itemView.gameObject.GetComponent<RectTransform>().rect.height / 2).Show();
                    });
                }
            }
            if(m_currentSelectEmail.rewards.items!=null)
            {
                for (int i = 0; i < m_currentSelectEmail.rewards.items.Count; i++)
                {
                    UI_Model_MailGiftView itemView = InstantiateGift(parent, ++index);
                    itemView.m_UI_Item_Bag.gameObject.SetActive(true);
                    itemView.m_pl_cur_ViewBinder.gameObject.SetActive(false);
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_count_LanguageText.text = m_currentSelectEmail.rewards.items[i].itemNum.ToString("N0");
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_currentSelectEmail.rewards.items[i].itemId);
                    itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(true);
                    //设置icon
                    ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_icon_PolygonImage, itemDefine.itemIcon);
                    ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_quality_PolygonImage, RS.ItemQualityBg[itemDefine.quality - 1]);
                    if (itemDefine.l_topID >= 1)
                    {
                        itemView.m_UI_Item_Bag.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(true);
                        itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), itemDefine.topData);
                    }
                    else
                    {
                        itemView.m_UI_Item_Bag.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
                    }
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_UI_Item_Bag.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.AddListener(() =>
                    {
                        HelpTip.CreateTip(LanguageUtils.getText(itemDefine.l_nameID), itemView.gameObject.GetComponent<Transform>()).SetOffset(itemView.gameObject.GetComponent<RectTransform>().rect.height / 2).Show();
                    });
                }
            }

        }

        private void OnEnclosureListFly(UI_Item_MailTypeSystemView systemView)
        {
            if(!systemView.m_pl_gift_ContentSizeFitter.gameObject.activeSelf)
            {
                return;
            }
            Transform[] child = new Transform[systemView.m_pl_gift_ContentSizeFitter.transform.childCount];
            for(int i = 0;i<child.Length;i++)
            {
                child[i] = systemView.m_pl_gift_ContentSizeFitter.transform.GetChild(i);
            }
            int childCount = child.Length;
            if (childCount < 1)
            {
                return;
            }
            int index = 0;
            FlyCurrnecy((int)EnumCurrencyType.food, m_currentSelectEmail.rewards.food,child[index],ref index);
            FlyCurrnecy((int)EnumCurrencyType.wood, m_currentSelectEmail.rewards.wood, child[index],ref index);
            FlyCurrnecy((int)EnumCurrencyType.stone, m_currentSelectEmail.rewards.stone, child[index],ref index);
            FlyCurrnecy((int)EnumCurrencyType.gold, m_currentSelectEmail.rewards.gold, child[index],ref index);
            FlyCurrnecy((int)EnumCurrencyType.denar, m_currentSelectEmail.rewards.denar, child[index],ref index);

            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            if (m_currentSelectEmail.rewards.soldiers != null)
            {
                for (int i = 0; i < m_currentSelectEmail.rewards.soldiers.Count; i++)
                {
                    if (index < childCount)
                    {
                        mt.FlyPowerUpEffect(child[index].gameObject, child[index].GetComponent<RectTransform>(), Vector3.one);
                        index++;
                    }
                }
            }

            if (m_currentSelectEmail.rewards.items != null)
            {
                for (int i = 0; i < m_currentSelectEmail.rewards.items.Count; i++)
                {
                    if(index < childCount)
                    {
                        mt.FlyItemEffect((int)m_currentSelectEmail.rewards.items[i].itemId, (int)m_currentSelectEmail.rewards.items[i].itemNum, child[index].GetComponent<RectTransform>());
                        index++;
                    }
                }
            }
        }

        private void FlyCurrnecy(int id,long num,Transform trans,ref int index)
        {
            if(num>0)
            {
                index++;
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                mt.FlyUICurrency(id,num, trans.position);
            }
        }



        private UI_Model_MailGiftView InstantiateGift(Transform parent,int index)
        {
            GameObject go;
            if (parent.childCount <= index)
            {
                go = CoreUtils.assetService.Instantiate(m_assetDic["UI_Model_MailGift"]);
                go.transform.SetParent(parent);
                go.transform.localScale = Vector3.one;
            }
            else
            {
                go = parent.GetChild(index).gameObject;
            }

            UI_Model_MailGiftView giftView = MonoHelper.AddHotFixViewComponent<UI_Model_MailGiftView>(go);
            return giftView;
        }

        private void SetCurrencyGift(UI_Model_MailGiftView itemView,int id,long num)
        {
            itemView.m_UI_Item_Bag.gameObject.SetActive(false);
            itemView.m_pl_cur_ViewBinder.gameObject.SetActive(true);
            itemView.m_lbl_count_LanguageText.text = num.ToString("N0");
            CurrencyDefine currencyDefine = CoreUtils.dataService.QueryRecord<CurrencyDefine>(id);
            itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(currencyDefine.l_desID);
            //设置icon
            ClientUtils.LoadSprite(itemView.m_img_curicon_PolygonImage, currencyDefine.iconID);
        }
        #endregion

        #region 采集报告

        private void OnCollectionReport(ListView.ListItem item)
        {
            UI_Item_MailTypeCollect_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_MailTypeCollect_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_collectionReport);
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_MailTypeCollect_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_collectionReport);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(item.go.GetComponent<RectTransform>());
            VerticalLayoutGroup layout = subView.m_UI_Item_MailTypeCollect_VerticalLayoutGroup;
            if (layout != null)
            {
                layout.CalculateLayoutInputVertical();
                layout.SetLayoutVertical();
                view.m_sv_haveMail_ListView.UpdateItemSize(0, layout.preferredHeight);
            }
        }

        #endregion

        #region 探索报告
        private void OnExploreReport(ListView.ListItem item)
        {
            if (item.data == null)
            {
                UI_Item_MailType4_SubView subView = new UI_Item_MailType4_SubView(item.go.GetComponent<RectTransform>());
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_exploreReport);
                item.data = subView;
            }
            else
            {
                UI_Item_MailType4_SubView subView = item.data as UI_Item_MailType4_SubView;
                subView.Refresh(m_currentMailDefine, m_currentSelectEmail, m_exploreReport);
            }

            VerticalLayoutGroup layout = item.go.GetComponent<VerticalLayoutGroup>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(item.go.GetComponent<RectTransform>());
            if (layout != null)
            {
                layout.CalculateLayoutInputVertical();
                layout.SetLayoutVertical();
                float size = layout.preferredHeight;
                view.m_sv_haveMail_ListView.UpdateItemSize(0, size);
            }
        }

        #endregion

        #endregion

        #region 工具

        //获取指定类型的邮件并按时间排序 邮件数据xzl
        private List<EmailInfoEntity> GetEmailListByType(EmailType type)
        {
            m_currentType = type;
            List<EmailInfoEntity> m_list = new List<EmailInfoEntity>();

            m_collectionReport.Clear();
            m_exploreReport.Clear();
            m_leaveMsgList.Clear();
            m_assistReport.Clear();
            m_spyOnList.Clear();

            for (int i = 0; i < m_pageRedpoints.Length; i++)
            {
                m_pageRedpoints[i] = 0;
            }
            //ClientUtils.Print(m_allEmails);
            foreach (var element in m_allEmails)
            {
                if (element.Value.sendTime < 0)
                {
                    continue;
                }
                MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)element.Value.emailId);
                bool isCollection = element.Value.isCollect;
                if (mailDefine == null)
                {
                    continue;
                }
                OnPageRedPoint(mailDefine, element.Value);
                switch (type)
                {
                    case EmailType.system: //系统
                        if (element.Value.emailId > 0)
                        {
                            if (mailDefine != null && mailDefine.type == 1 && !isCollection)
                            {
                                if (mailDefine.group == 12)
                                {
                                    m_leaveMsgList.Add(element.Value);
                                }
                                else
                                {
                                    m_list.Add(element.Value);
                                }
                            }
                        }
                        break;
                    case EmailType.report: //报告
                        if (isCollection)
                        {
                            break;
                        }
                        //采集报告
                        if (element.Value.subType == 1 || element.Value.resourceCollectReport != null)
                        {
                            m_collectionReport.Add(element.Value);
                        }
                        else if (element.Value.subType == 3 || element.Value.discoverReport != null)
                        {
                            if (element.Value.subType == 6)
                            {
                                //调查类邮件
                                m_list.Add(element.Value);
                            }
                            else
                            {
                                m_exploreReport.Add(element.Value);
                            }
                        }
                        else if (mailDefine != null && mailDefine.type == 2)
                        {
                            if (mailDefine.group == 17)
                            {
                                //被侦查邮件
                                m_spyOnList.Add(element.Value);
                            }
                            else
                            {
                                m_list.Add(element.Value);
                            }
                        }
                        break;
                    case EmailType.alliance: //联盟
                        if (isCollection)
                        {
                            break;
                        }
                        if (mailDefine != null && mailDefine.type == 3)
                        {
                            if (mailDefine.group == 11)
                            {
                                m_assistReport.Add(element.Value);
                            }
                            else
                            {
                                m_list.Add(element.Value);
                            }
                        }
                        break;
                    case EmailType.personal: //个人
                        if (mailDefine != null && mailDefine.type == 4 && !isCollection)
                        {
                            m_list.Add(element.Value);
                        }
                        break;
                    case EmailType.sent:    //已发送
                        if (mailDefine != null && mailDefine.type == 5)
                        {
                            m_list.Add(element.Value);
                        }
                        break;
                    case EmailType.collection: //收藏
                        if(isCollection)
                        {
                            m_list.Add(element.Value);
                        }
                        break;
                    default:break;
                }
            }
            if(m_collectionReport.Count>0)
            {
                m_collectionReport.Sort(SortByTime);
                m_list.Add(m_collectionReport[0]);
            }
            if (m_exploreReport.Count > 0)
            {
                m_exploreReport.Sort(SortByTime);
                m_list.Add(m_exploreReport[0]);
                m_exploreReport.Sort(SortByExplore);
            }
            if (m_assistReport.Count > 0)
            {
                m_assistReport.Sort(SortByTime);
                m_list.Add(m_assistReport[0]);
            }
            if (m_leaveMsgList.Count > 0)
            {
                m_leaveMsgList.Sort(SortByTime);
                m_list.Add(m_leaveMsgList[0]);
            }
            if (m_spyOnList.Count > 0)
            {
                m_spyOnList.Sort(SortByTime);
                m_list.Add(m_spyOnList[0]);
            }
            m_list.Sort(SortByTime);
            return m_list;
        }

        private void OnPageRedPoint(MailDefine mail,EmailInfoEntity entity)
        {
            if(entity.status!=0)
            {
                return;
            }
            if(entity.isCollect)//收藏邮件
            {
                m_pageRedpoints[5]++;
                return;
            }
            switch(mail.type)
            {
                case 1://系统
                    m_pageRedpoints[0]++;
                    break;
                case 2://报告
                    m_pageRedpoints[1]++;
                    break;
                case 3://联盟
                    m_pageRedpoints[2]++;
                    break;
                case 4: //个人
                    m_pageRedpoints[3]++;
                    break;
                case 5: //已发送
                    m_pageRedpoints[4]++;
                    break;
                default:break;         
            }
        }

        //刷新删除 收藏 分享 翻译 回复按钮
        private void RefreshDownOtherBtn(MailDefine mailDefine,EmailInfoEntity info)
        {
            view.m_UI_Item_MailBtnOnButtomCollect.gameObject.SetActive(false);
            view.m_UI_Item_MailBtnOnButtomDel.gameObject.SetActive(false);
            view.m_UI_Item_MailBtnOnButtomTrans.gameObject.SetActive(false);
            view.m_UI_Item_MailBtnOnButtomRe.gameObject.SetActive(false);
            if (info==null)
            {
                return;
            }
            if(mailDefine==null)//个人邮件
            {
                return;          
            }
            if (mailDefine.operate != null)
            {
                for (int i = 0; i < mailDefine.operate.Count; i++)
                {
                    if (mailDefine.operate[i] == 1)//删除
                    {
                        view.m_UI_Item_MailBtnOnButtomDel.gameObject.SetActive(true);
                        view.m_UI_Item_MailBtnOnButtomDel.m_btn_newMail_GameButton.onClick.RemoveAllListeners();
                        view.m_UI_Item_MailBtnOnButtomDel.m_btn_newMail_GameButton.onClick.AddListener(() =>
                        {
                            if (mailDefine.enclosure > 0 && !info.takeEnclosure)//有奖励附近未领取 不能删除
                            {
                                Tip.CreateTip(570011, Tip.TipStyle.Middle).Show();
                            }
                            else if (info.rewards != null && !info.takeEnclosure) //有奖励附近未领取 不能删除
                            {
                                Tip.CreateTip(570011, Tip.TipStyle.Middle).Show();
                            }
                            else
                            {
                                m_emailProxy.DeleteEmailIndex = info.emailIndex;
                                m_emailProxy.DeleteEmailListIndex = m_currentSelectItemIndex;
                                Email_DeleteEmail.request req = new Email_DeleteEmail.request();
                                req.type = 1;
                                req.data = info.emailIndex;
                                AppFacade.GetInstance().SendSproto(req);

                            }
                        });
                    }
                    if (mailDefine.operate[i] == 2)//收藏
                    {
                        view.m_UI_Item_MailBtnOnButtomCollect.gameObject.SetActive(!info.isCollect);
                        view.m_UI_Item_MailBtnOnButtomCollect.m_btn_newMail_GameButton.onClick.RemoveAllListeners();
                        view.m_UI_Item_MailBtnOnButtomCollect.m_btn_newMail_GameButton.onClick.AddListener(() =>
                        {
                            if (mailDefine.enclosure > 0 && !info.takeEnclosure)
                            {
                                Tip.CreateTip(570045, Tip.TipStyle.Middle).Show();
                            }
                            else if (info.rewards != null && !info.takeEnclosure) //有奖励附件未领取 不能删除
                            {
                                Tip.CreateTip(570045, Tip.TipStyle.Middle).Show();
                            }
                            else
                            {
                                Email_CollectEmail.request req = new Email_CollectEmail.request();
                                req.emailIndex = info.emailIndex;
                                AppFacade.GetInstance().SendSproto(req);
                            }
                        });
                    }
                    if (mailDefine.operate[i] == 3)//分享
                    {
                        //无案子
                    }
                    if (mailDefine.operate[i] == 4)//翻译
                    {
                        //无案子
                        view.m_UI_Item_MailBtnOnButtomTrans.gameObject.SetActive(true);
                        view.m_UI_Item_MailBtnOnButtomTrans.m_btn_newMail_GameButton.onClick.RemoveAllListeners();
                        view.m_UI_Item_MailBtnOnButtomTrans.m_btn_newMail_GameButton.onClick.AddListener(() =>
                        {
                            TranslateEmail(mailDefine, info);
                        });
                    }
                    if (mailDefine.operate[i] == 5)//回复
                    {
                        view.m_UI_Item_MailBtnOnButtomRe.gameObject.SetActive(true);
                        view.m_UI_Item_MailBtnOnButtomRe.m_btn_newMail_GameButton.onClick.RemoveAllListeners();
                        view.m_UI_Item_MailBtnOnButtomRe.m_btn_newMail_GameButton.onClick.AddListener(() =>
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_replyEmail, null, info);
                        });
                    }
                }

            }
        }

        //刷新 删除已读 一键领取&已读
        private void RefreshDelAndReceiveBtn()
        {
            //删除已读
            view.m_UI_Item_MailBtnOnButtomDeleteRead.m_btn_newMail_GameButton.onClick.RemoveAllListeners();
            view.m_UI_Item_MailBtnOnButtomDeleteRead.m_btn_newMail_GameButton.onClick.AddListener(() =>
            {
                Alert.CreateAlert(570014).SetLeftButton().SetRightButton(() =>
                {
                    Email_DeleteEmail.request req = new Email_DeleteEmail.request();
                    req.type = 2;
                    if (m_currentPage == 5) //收藏 特殊处理
                    {
                        req.data = 99;
                    }
                    else
                    {
                        req.data = m_currentPage + 1;
                    } 
                    Debug.LogFormat("m_currentPage:{0}", req.data);
                    AppFacade.GetInstance().SendSproto(req);
                }).Show();
            });

            //一键领取已读
            if ((EmailType)m_currentPage == EmailType.collection)
            {
                view.m_UI_Item_MailBtnOnButtomRead.gameObject.SetActive(false);
            }
            else if ((EmailType)m_currentPage == EmailType.sent)
            {
                view.m_UI_Item_MailBtnOnButtomRead.gameObject.SetActive(false);
            }
            else
            {
                view.m_UI_Item_MailBtnOnButtomRead.gameObject.SetActive(true);
                view.m_UI_Item_MailBtnOnButtomRead.m_btn_newMail_GameButton.onClick.RemoveAllListeners();
                view.m_UI_Item_MailBtnOnButtomRead.m_btn_newMail_GameButton.onClick.AddListener(() =>
                {
                    Email_TakeEnclosure.request req = new Email_TakeEnclosure.request();
                    req.type = 2;
                    req.data = m_currentPage + 1;
                    Debug.LogFormat("m_currentPage:{0}", req.data);
                    AppFacade.GetInstance().SendSproto(req);
                });
            }
        }

        private int SortByTime(EmailInfoEntity t1,EmailInfoEntity t2)
        {
            return t2.sendTime.CompareTo(t1.sendTime);
        }

        //探索数据排序
        private int SortByExplore(EmailInfoEntity t1, EmailInfoEntity t2)
        {
            int re = re = GetExploreStatus(t2).CompareTo(GetExploreStatus(t1));
            if (re == 0)
            {
                re = t2.sendTime.CompareTo(t1.sendTime);
            }
            return re;
        }

        private int GetExploreStatus(EmailInfoEntity emailInfo)
        {
            DiscoverReportInfo report = emailInfo.discoverReport;
            int rssID = (int)report.mapFixPointId;

            if (rssID != 0)
            {
                int villageCaveIndex = Mathf.CeilToInt(rssID / 64f);
                bool explored = false;
                if (m_playerProxy.CurrentRoleInfo != null && m_playerProxy.CurrentRoleInfo.villageCaves != null && m_playerProxy.CurrentRoleInfo.villageCaves.ContainsKey(villageCaveIndex))
                {
                    explored = (ulong)(m_playerProxy.CurrentRoleInfo.villageCaves[villageCaveIndex].rule & (1L << (rssID % 64))) > 0;
                }
                if (explored)
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return 1;
            }
        }

        public static bool IsTouchEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                return true;
            }
            return false;
        }

        public static bool IsTouchBegin()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
            return false;
        }


        #endregion

        //写邮件
        private void OnWriteEmail()
        {
            CoreUtils.uiManager.ShowUI(UI.s_writeAMail);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_Email);
        }
    }
}