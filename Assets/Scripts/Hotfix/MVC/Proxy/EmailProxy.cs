// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    EmailProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sproto;
using Skyunion;
using Data;
using SprotoType;
using Client;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game {
    public enum EmailType
    {
        /// <summary>
        /// 系统邮件
        /// </summary>
        system = 1,     
        /// <summary>
        /// 报告邮件
        /// </summary>
        report = 2,
        /// <summary>
        /// 联盟邮件
        /// </summary>
        alliance = 3,
        /// <summary>
        /// 个人邮件
        /// </summary>
        personal = 4,
        /// <summary>
        /// 已发送邮件
        /// </summary>
        sent = 5,
        /// <summary>
        /// 收藏邮件
        /// </summary>
        collection = 6,
    }

    public enum EmailDataType
    {
        System, //系统邮件不带附件
        SystemWithEnclosure,//系统邮件带附件
        CollectReport, //采集报告
    }

    public class EmailProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "EmailProxy";

        private PlayerProxy m_playerProxy;

        private long m_lastSendTime;
        private Dictionary<long, bool> m_redPointsIndex = new Dictionary<long, bool>();
        /// <summary>
        /// 个人邮件列表
        /// </summary>
        public Dictionary<long, bool> PersonalIndexDic = new Dictionary<long, bool>();

        /// <summary>
        /// 已经获取过战报的列表
        /// </summary>
        public List<long> BattleReportIndexList = new List<long>();

        //已经请求过的邮件
        public Dictionary<long, bool> RequestedIndexDic = new Dictionary<long, bool>();

        public int MailCurrentPage = 0;

        public bool IsFirstOpenMail = true;

        public BattleReportExDetail CuurentArmyDetail;

        private Skyunion.Timer m_saveEmailTimer = null;

        public int MenuSwitchVal;

        public long DeleteEmailIndex = -1;
        public long DeleteEmailListIndex = -1;

        public bool m_isUpdateEmail;
        public bool m_isAddEmail;
        public bool m_isEmailBubble;//同列表下发只发送一次
        public bool m_isDelEMail;
        public List<long> m_emailChangeList = new List<long>();

        //正在加载中的战斗详情列表
        private Dictionary<long, string> m_loadingFightDetailDic = new Dictionary<long, string>();
        //战斗详情列表
        private Dictionary<long, BattleReportEx> m_fightDetailDic = new Dictionary<long, BattleReportEx>();

        private bool m_isDispose;

        public string m_emailSavePath;

        #endregion

        // Use this for initialization
        public EmailProxy(string proxyName)
            : base(proxyName)
        {

        }

        public override void OnRegister()
        {
            Debug.Log(" EmailProxy register");
            IsFirstOpenMail = true;
        }


        public override void OnRemove()
        {
            Debug.Log(" EmailProxy remove");
            PersonalIndexDic.Clear();
            BattleReportIndexList.Clear();
            m_isDispose = true;
            if(m_saveEmailTimer != null)
            {
                m_saveEmailTimer.Cancel();
            }
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        //登录游戏时请求邮件
        public void OnLoginToGame()
        {
            EmailReceived = false;
            string tmpStr = PlayerPrefs.GetString(string.Format("EmailVersion{0}", m_playerProxy.CurrentRoleInfo.rid), "0");
            long emailVersion = long.Parse(tmpStr);
            Debug.Log($"服务器邮件版本号：{m_playerProxy.CurrentRoleInfo.emailVersion}  客户端邮件版本号:{emailVersion}");
            if (emailVersion != m_playerProxy.CurrentRoleInfo.emailVersion)
            {
                Debug.Log("请求邮件");
                DeleteEmail();
                GetEmails.Clear();
                Email_GetEmails.request req = new Email_GetEmails.request();
                AppFacade.GetInstance().SendSproto(req);
            }
            else
            {
                EmailReceived = true;
                ReadEmail();
            }
        }

        public bool EmailReceived
        {
            get; set;
        }

        public void UpdateEmailVersion()
        {
            Debug.LogFormat("邮件 UpdateEmailVersion：{0}", m_playerProxy.CurrentRoleInfo.emailVersion);
            if (!EmailReceived)
            {
                return;
            }
            Debug.Log("保存邮件版本号：" + m_playerProxy.CurrentRoleInfo.emailVersion);
            PlayerPrefs.SetString(string.Format("EmailVersion{0}", m_playerProxy.CurrentRoleInfo.rid), m_playerProxy.CurrentRoleInfo.emailVersion.ToString());
        }

        public void UpdateEmail(object body)
        {
            Debug.Log("邮件 UpdateEmail");
            bool isDeleteSingleEmail = false;
            m_isDelEMail = false;
            m_isAddEmail = false;
            m_isUpdateEmail = false;
            m_isEmailBubble = false;
            m_emailChangeList.Clear();
            if (body is Email_EmailList.request)
            {
                Email_EmailList.request infos = body as Email_EmailList.request;
                //Debug.LogFormat("邮件 UpdateEmail 数量:{0}", infos.emailInfo.Count);
                foreach (var element in infos.emailInfo)
                {
                    if (EmailReceived)
                    {
                        m_emailChangeList.Add(element.Key);
                    }
                    if (GetEmails.ContainsKey(element.Key)) //邮件已存在
                    {
                        if (element.Value.emailId == -1)//删除邮件
                        {
                            m_isDelEMail = true;
                            if (DeleteEmailIndex > -1 && element.Key == DeleteEmailIndex)
                            {
                                isDeleteSingleEmail = true;
                            }
                            GetEmails.Remove(element.Key);
                            m_fightDetailDic.Remove(element.Key);
                            m_redPointsIndex.Remove(element.Key);
                        }
                        else //更新邮件
                        {
                            if (element.Value.HasStatus && element.Value.status == 1)
                            {
                                m_redPointsIndex.Remove(element.Key);
                            }

                            m_isUpdateEmail = true;
                            EmailInfoEntity emailInfo = GetEmails[element.Value.emailIndex];
                            EmailInfoEntity.updateEntity(emailInfo, element.Value);
                            UpdateFightDetialDic(emailInfo);
                        }
                    }
                    else if (element.Value.emailId != -1)//新邮件
                    {
                        if (element.Value.HasStatus && element.Value.status == 0)
                        {
                            m_redPointsIndex[element.Key] = true;
                        }

                        m_isEmailBubble = false;
                        MailDefine define = null;
                        if (EmailReceived)
                        {
                            m_isAddEmail = true;
                            define = CoreUtils.dataService.QueryRecord<MailDefine>((int)element.Value.emailId);
                            if (!(define != null && define.type == 5))
                            {
                                Tip.CreateTip(570051).Show();
                            }
                            m_isEmailBubble = true;
                        }
                        EmailInfoEntity emailInfo = new EmailInfoEntity();
                        EmailInfoEntity.updateEntity(emailInfo, element.Value);
                        GetEmails[element.Value.emailIndex] = emailInfo;
                        UpdateFightDetialDic(emailInfo); 

                        //邮件气泡
                        if (m_isEmailBubble)
                        {
                            if (define != null && define.messageName != 0)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.AddEmailBubble, emailInfo);
                            }
                        }
                    }
                }
            }

            if (EmailReceived)
            {
                if (m_isAddEmail)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateEmail);
                }
                else if (m_isDelEMail)
                {
                    if (isDeleteSingleEmail)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ChangeSelectEmailIndex);
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.UpdateEmail);
                }
                else if (m_isUpdateEmail)
                {
                    if (m_emailChangeList.Count == 1)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateEmail, m_emailChangeList[0]);
                    }
                    else
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.UpdateEmail);
                    }
                }
            }
            else
            {
                foreach (var element in GetEmails)
                {
                    if (element.Value.status == 0)
                    {
                        if (IsFirstOpenMail)
                        {
                            MailDefine define = CoreUtils.dataService.QueryRecord<MailDefine>((int)element.Value.emailId);
                            if (define != null)
                            {
                                MailCurrentPage = define.type - 1;
                                IsFirstOpenMail = false;
                            }
                        }
                    }
                }

                AppFacade.GetInstance().SendNotification(CmdConstant.UpdateEmail);
            }
            if (m_saveEmailTimer == null)
            {
                m_saveEmailTimer = Skyunion.Timer.Register(1.0f, ()=>
                {
                    m_saveEmailTimer = null;
                    SaveEmail();
                });
            }
        }

        public void UpdateFightDetialDic(EmailInfoEntity emailInfo)
        {
            if (emailInfo.subType == 2)
            {
                if (emailInfo.reportStatus == 2 && emailInfo.battleReportExContent != null)
                {
                    m_fightDetailDic[emailInfo.emailIndex] = emailInfo.battleReportExContent;
                    emailInfo.battleReportExContent = null; //这个数据量太大了 存文件不太好  所以设为null
                }
            }
        }

        public void ForceRemoveEmail(long index)
        {
            GetEmails.Remove(index);
            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateEmail);
        }

        public int GetEmailRedPoint()
        {
            return m_redPointsIndex.Count;
        }

        public bool IsEmailHasRedPoint()
        {
            return m_redPointsIndex.Count > 0;
        }

        public Dictionary<long, bool> RedPointsIndex
        {
            get { return m_redPointsIndex; }
        }

        public Dictionary<long, EmailInfoEntity> GetEmails { get; private set; } = new Dictionary<long, EmailInfoEntity>();


        #region 邮件格式化解析
        static Regex m_regex = new Regex(@"\{[0-9a-zA-Z]+_[0-9]+\}");
        static Regex m_nameRegex = new Regex(@"(?<=\{)[0-9a-zA-Z]+(?=_[0-9]+\})");
        static Regex m_numRegex = new Regex(@"(?<=\{[0-9a-zA-Z]+_)[0-9]+(?=\})");

        public string OnTextFormat(string text, List<string> args)
        {
            if (args == null)
            {
                return text;
            }
            string str = "";
            StringBuilder sb = new StringBuilder(text);
            try
            {
                MatchCollection names = m_nameRegex.Matches(text);
                MatchCollection nums = m_numRegex.Matches(text);
                MatchCollection matchs = m_regex.Matches(text);

                for (int i = 0; i < matchs.Count; i++)
                {
                    sb.Replace(matchs[i].Value, TranslateParam(names[i].Value, args[int.Parse(nums[i].Value)]));
                }
                str = string.Format(sb.ToString(), args.ToArray());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                Debug.LogErrorFormat("参数格式化异常：text:{0} args:{1}", text, string.Join(" ", args));
                return "";
            }
            return str;
        }

        public string TranslateParam(string type, string value)
        {
            switch (type)
            {
                case "item":
                    ItemDefine item = CoreUtils.dataService.QueryRecord<ItemDefine>(int.Parse(value));
                    return LanguageUtils.getText(item.l_nameID);
                case "monster":
                    return FormatMonsterName(value);
                case "rune":
                    return FormatMapItemTypeName(value);
                case "activity":
                    ActivityCalendarDefine activity = CoreUtils.dataService.QueryRecord<ActivityCalendarDefine>(int.Parse(value));
                    return LanguageUtils.getText(activity.l_nameID);
                case "activityKill":
                    ActivityKillTypeDefine killtype = CoreUtils.dataService.QueryRecord<ActivityKillTypeDefine>(int.Parse(value));
                    return LanguageUtils.getText(killtype.l_nameID);
                case "price":
                    PriceDefine price = CoreUtils.dataService.QueryRecord<PriceDefine>(int.Parse(value));
                    return LanguageUtils.getText(price.l_nameID);
                case "resourceBuild":
                      return FormatResourceBuildName(value);
                case "allianceBuild":                 
                    return FormatAllianceBuildName(value);
                case "strongHold":
                    return FormatStrongHold(value);
                case "name":
                    {
                        return GuideAbNameFormat(value);
                    }
                case "playerNames":
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            return "";
                        }
                        string[] strArr = value.Split('|');
                        List<string> strList = new List<string>();
                        int len = strArr.Length;
                        if (len > 0)
                        {
                            for (int i = 0; i < len; i++)
                            {
                                strList.Add(GuideAbNameFormat(strArr[i]));
                            }
                            return string.Join("、", strList);
                        }
                        return "";
                    }
                case "reasonForLeaving": //踢出联盟原因
                    return GetKickCauseByType(int.Parse(value));
                case "coordinate"://坐标
                    {
                        string[] strArr = value.Split(',');
                        if (strArr.Length == 2)
                        {
                            int jumpX = int.Parse(strArr[0]);
                            int jumpY = int.Parse(strArr[1]);
                            jumpX = Mathf.FloorToInt(jumpX / 600);
                            jumpY = Mathf.FloorToInt(jumpY / 600);
                            return LanguageUtils.getTextFormat(300032, jumpX, jumpY);
                        }
                        else if (strArr.Length == 3)
                        {
                            int jumpK = int.Parse(strArr[0]);
                            int jumpX = int.Parse(strArr[1]);
                            int jumpY = int.Parse(strArr[2]);
                            return LanguageUtils.getTextFormat(300279, jumpK, jumpX, jumpY);
                        }
                        return "";
                    }
                case "coordinateK"://坐标
                    {
                        string[] strArr = value.Split(',');
                        if (strArr.Length == 2)
                        {
                            int jumpX = int.Parse(strArr[0]);
                            int jumpY = int.Parse(strArr[1]);
                            jumpX = Mathf.FloorToInt(jumpX / 600);
                            jumpY = Mathf.FloorToInt(jumpY / 600);
                            return LanguageUtils.getTextFormat(300032, jumpX, jumpY);
                        }
                        else if (strArr.Length == 3)
                        {
                            int jumpK = int.Parse(strArr[0]);
                            int jumpX = int.Parse(strArr[1]);
                            int jumpY = int.Parse(strArr[2]);
                            return LanguageUtils.getTextFormat(300279, jumpK, jumpX, jumpY);
                        }
                        return "";
                    }
                case "allianceDailyUpperLimit"://每日联盟捐献
                    {
                        ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                        return ClientUtils.FormatComma(config.AllianceDailyUpperLimit);
                    }
                case "allianceWeeklyUpperLimit"://每周联盟捐献
                    {
                        ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                        return ClientUtils.FormatComma(config.AllianceWeeklyUpperLimit);
                    }
                case "allianceStudy"://联盟科技名称
                    {
                        AllianceStudyDefine config = CoreUtils.dataService.QueryRecord<AllianceStudyDefine>(int.Parse(value));
                        return LanguageUtils.getText(config.l_nameID);
                    }
                case "allianceMember"://联盟成员等级名称
                    {
                        int id = int.Parse(value) * 100 + 1;
                        AllianceMemberDefine config = CoreUtils.dataService.QueryRecord<AllianceMemberDefine>(id);
                        if (config == null)
                        {
                            return "";
                        }
                        return LanguageUtils.getText(config.l_nameID);
                    }
                case "allianceOfficially"://联盟职位名称
                    {
                        AllianceOfficiallyDefine config = CoreUtils.dataService.QueryRecord<AllianceOfficiallyDefine>(int.Parse(value));
                        if (config == null)
                        {
                            Debug.LogErrorFormat("AllianceOfficiallyDefine not find:{0}", value);
                            return "";
                        }
                        return LanguageUtils.getText(config.l_officiallyID);
                    }
                case "allianceOfficiallyDes"://联盟职位描述
                    {
                        AllianceOfficiallyDefine config = CoreUtils.dataService.QueryRecord<AllianceOfficiallyDefine>(int.Parse(value));
                        return LanguageUtils.getText(config.l_desID);
                    }
                case "allianceOfficiallyBuff"://联盟职位加成
                    {
                        AllianceOfficiallyDefine config = CoreUtils.dataService.QueryRecord<AllianceOfficiallyDefine>(int.Parse(value));
                        return ClientUtils.SafeFormat(LanguageUtils.getText(config.l_addDesID), config.addDesData);
                    }
                case "hero"://英雄名称
                    {
                        int heroId = int.Parse(value);
                        if (heroId == 0)
                        {
                            return LanguageUtils.getText(300065);
                        }
                        HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>(heroId);
                        if (hero == null)
                        {
                            Debug.LogErrorFormat("HeroDefine not find:{0}", heroId);
                            return "";
                        }
                        return LanguageUtils.getText(hero.l_nameID);
                    }
                case "language":
                    {
                        return LanguageUtils.getText(int.Parse(value));
                    }
                default:
                    return value ?? string.Empty;
            }
        }

        //获取联盟踢出原因
        private string GetKickCauseByType(int cause)
        {
            int languageId = 0;
            if (cause == 1)
            {
                languageId = 730157;
            }
            else if (cause == 2)
            {
                languageId = 730158;
            }
            else if (cause == 3)
            {
                languageId = 730159;
            }
            else if (cause == 4)
            {
                languageId = 730160;
            }
            else if (cause == 5)
            {
                languageId = 730161;
            }
            else if (cause == 6)
            {
                languageId = 730162;
            }
            return LanguageUtils.getText(languageId);
        }

        public string FormatStrongHold(string str)
        {
            string[] tArr = str.Split(',');
            int len = tArr.Length;
            string guideAbName = "";
            string buildName = "";
            if (len > 1)
            {
                if (string.IsNullOrEmpty(tArr[0]))
                {
                    buildName = tArr[1];
                }
                else
                {
                    guideAbName = tArr[0];
                    buildName = tArr[1];
                }
            }
            else
            {
                if (tArr.Length > 0)
                {
                    buildName = tArr[0];
                }
                else
                {
                    return "";
                }
            }
            string name = "";
            if (string.IsNullOrEmpty(buildName))
            {
                return name;
            }
            StrongHoldDataDefine strongHoldData = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>(int.Parse(buildName));
            if (strongHoldData != null)
            {
                StrongHoldTypeDefine strongHold = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldData.type);
                if (strongHold != null)
                {
                    name = LanguageUtils.getText(strongHold.l_nameId);
                }
            }
            if (string.IsNullOrEmpty(name))
            {
                name = string.Format("(未知世界建筑类型:{0})", buildName);
            }

            if (string.IsNullOrEmpty(guideAbName))
            {
                return name;
            }
            else
            {
                return LanguageUtils.getTextFormat(300030, guideAbName, name);
            }
        }
        public string FormatMonsterName(string str)
        {
            if (string.IsNullOrEmpty(str))
                    {
                        return "";
                    }
            int monsterId = 0;
            if (int.TryParse(str, out monsterId))
            {
                MonsterDefine monster = CoreUtils.dataService.QueryRecord<MonsterDefine>(monsterId);
                if (monster == null)
                {
                    return string.Format("未知怪物类型({0})", str);
                }
                return LanguageUtils.getText(monster.l_nameId);
            }
            return "";
        }
        public string FormatMapItemTypeName(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            int mapItemId = 0;
            if (int.TryParse(str, out mapItemId))
            {
                MapItemTypeDefine rune = CoreUtils.dataService.QueryRecord<MapItemTypeDefine>(mapItemId);
                if (rune == null)
                {
                    return string.Format("未知怪物类型({0})", str);
                }
                return LanguageUtils.getText(rune.l_nameId);
            }
            return "";
        }
        
public string FormatResourceBuildName(string str)
        {
            string resourceBuildName = string.Empty;
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            ResourceGatherTypeDefine reourceBuildDefine = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(int.Parse(str));
            return LanguageUtils.getText(reourceBuildDefine.l_nameId);
        }
        public string FormatAllianceBuildName(string str)
        {
            string[] tArr = str.Split(',');
            int len = tArr.Length;
            string guideAbName = "";
            string buildName = "";
            if (len > 1)
            {
                if (string.IsNullOrEmpty(tArr[0]))
                {
                    buildName = tArr[1];
                }
                else
                {
                    guideAbName = tArr[0];
                    buildName = tArr[1];
                }
            }
            else
            {
                if (tArr.Length > 0)
                {
                    buildName = tArr[0];
                }
                else
                {
                    return "";
                }
            }
            AllianceBuildingTypeDefine buildingDefine = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(int.Parse(buildName));
            string name = "";
            if (buildingDefine == null)
            {
                name = string.Format("(未知联盟建筑类型:{0})", buildName);
            }
            else
            {
                name = LanguageUtils.getText(buildingDefine.l_nameId);
            }
            if (string.IsNullOrEmpty(guideAbName))
            {
                return name;
            }
            else
            {
                return LanguageUtils.getTextFormat(300030, guideAbName, name);
            }
        }

        //联盟简称玩家名称格式化
        public string GuideAbNameFormat(string str)
        {
            string[] tArr = str.Split(',');
            int len = tArr.Length;
            if (len > 1)
            {
                if (string.IsNullOrEmpty(tArr[0]))
                {
                    return tArr[1];
                }
                else
                {
                    return LanguageUtils.getTextFormat(300030, tArr[0], tArr[1]);
                }
            }
            else
            {
                if (tArr.Length > 0)
                {
                    return tArr[0];
                }
                return "";
            }
        }
        /// <summary>
        /// 邮件气泡Message字段填充
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mes"></param>
        /// <param name="strList"></param>
        /// <returns></returns>
        public string FormatBubbleMessage(EmailInfoEntity emailInfo, MailDefine mailDefine)
        {
            string str = "";
            int id = mailDefine.ID;

            string langStr = LanguageUtils.getText(mailDefine.message);

            //系统邮件
            if (mailDefine.type == 1)
            {
                str = OnTextFormat(langStr, emailInfo.subTitleContents);
                return str;
            }

            //战报
            if (id >= 200000 && id <= 200051)
            {
                str = OnTextFormat(langStr, new List<string> { emailInfo.mainHeroId.ToString() });
                return str;
            }

            //侦查邮件 反侦察邮件
            if (mailDefine.group == 16 || mailDefine.group == 17)
            {
                if (mailDefine.ID == 200123)
                {
                    if (emailInfo.subTitleContents.Count > 1)
                    {
                        str = OnTextFormat(langStr, new List<string> { emailInfo.subTitleContents[1] });
                    }
                }
                else
                {
                    str = OnTextFormat(langStr, emailInfo.subTitleContents);
                }
                return str;
            }
            else
            {
                return langStr;
            }
        }

        public string FormatBattleReportSubTitle(int id, string mes, List<string> strList)
        {
            if (strList == null || strList.Count < 7)
            {
                return "";
            }
            List<string> contentList = new List<string>();
            //resportSubTitle ={
            //  己方角色rid            0
            //  己方角色名称           1
            //  己方联盟名称           2
            //  防御方角色rid          3
            //  防御方对象id（建筑类） 4
            //  防御方角色名称         5    
            //  防御方联盟名称         6
            //  己方对象id（建筑类）   7
            //}
            switch (id)
            {
                case 200000://部队之间战斗
                case 200001:
                case 200011:
                    {
                        Int64 selfId = Int64.Parse(strList[0]);
                        contentList.Add(GetNameFormat(strList[6], strList[5]));
                    }
                    break;
                case 200012://部队之间战斗
                case 200013:
                case 200014:
                    {
                        Int64 selfId = Int64.Parse(strList[0]);
                        contentList.Add(GetNameFormat(strList[6], strList[5]));
                    }
                    break;
                case 200004://进攻城市
                case 200005:
                case 200048:
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                case 200002://城市防守
                case 200003:
                case 200044:
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                case 200017://城市防守
                case 200018:
                case 200045:
                    contentList.Add(GetNameFormat(strList[2], strList[1]));
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                case 200015://城市防守
                case 200016:
                case 200046:
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                case 200019://城市防守
                case 200020:
                case 200047:
                    contentList.Add(GetNameFormat(strList[2], strList[1]));
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                case 200021://攻击联盟建筑
                case 200022:
                case 200023:
                    contentList.Add(strList[6]);
                    contentList.Add(strList[4]);
                    break;
                case 200024://联盟建筑防守
                case 200025:
                case 200026:
                    contentList.Add(strList[7]);
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                case 200027://联盟建筑防守
                case 200028:
                case 200029:
                    contentList.Add(strList[7]);
                    contentList.Add(GetNameFormat(strList[2], strList[1]));
                    break;
                case 200030://攻击关卡圣地
                case 200031:
                case 200032:
                    contentList.Add(strList[6]);
                    contentList.Add(strList[4]);
                    break;
                case 200033://关卡圣地防守
                case 200034:
                case 200035:
                    contentList.Add(strList[7]);
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                case 200036://关卡圣地防守
                case 200037:
                case 200038:
                    contentList.Add(strList[7]);
                    contentList.Add(GetNameFormat(strList[6], strList[5]));
                    break;
                default://打怪
                    contentList.Add(strList[4]);
                    break;
                    //case 20006:
                    //case 20007:
                    //case 20009:
                    //case 20010:
                    //case 20039:
                    //case 20040:
                    //case 20041:
                    //case 20042:
                    //case 20043:
            }
            return OnTextFormat(mes, contentList);
        }

        public string GetNameFormat(string guildAbName, string name)
        {
            return string.Format("{0},{1}", guildAbName, name);
        }

        #endregion


        #region 本地存储和读取邮件
        private string GetPath()
        {
            string folder = Path.Combine(Application.persistentDataPath, "Email");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (m_playerProxy.CurrentRoleInfo == null)
            {
                Debug.LogError("读取邮件路径失败 roleInfo尚未下发");
                return "";
            }
            return Path.Combine(folder, m_playerProxy.CurrentRoleInfo.rid.ToString());
        }

        private void InitEmailSavePath()
        {
            if (m_emailSavePath == null)
            {
                m_emailSavePath = GetPath();
            }
        }

        public void SaveEmail()
        {
            InitEmailSavePath();
            float time1 = Time.realtimeSinceStartup;

            List<EmailInfoEntity> list = GetEmails.Values.ToList();
            float time2 = Time.realtimeSinceStartup;

            string data = LitJson.JsonMapper.ToJson(list);
            float time3 = Time.realtimeSinceStartup;

            File.WriteAllText(m_emailSavePath, data);
            float time4 = Time.realtimeSinceStartup;

            Debug.LogFormat("邮件toList:{0} {1} toJson:{2} writeFile:{3} length:{4} kb:{5}",
                                time1, time2, time3-time2, time4-time3, data.Length, (float)data.Length/1024);

            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Debug.LogError(ex);
            //}

            //    //Thread th = new Thread(new ThreadStart(ThreadMethod));
            //    //th.Start();

            //Task t = new Task(ThreadMethod);
            //t.Start();
            //t.ContinueWith((task) =>
            //{
            //    Debug.LogError("存储完成");
            //});
        }

        public void ThreadMethod()
        {
            //string data = LitJson.JsonMapper.ToJson(GetEmails.Values.ToList());
            //File.WriteAllText(m_emailSavePath, data);
            //try
            //{
            //TestCls cls = new TestCls();
            //cls.EmailList = GetEmails.Values.ToList();
            ////var data = SerializeObject(cls);
            //byte[] data = new byte[1];
            //data[0] = 9;
            //File.WriteAllBytes(m_emailSavePath, data);

            //string data = LitJson.JsonMapper.ToJson(GetEmails.Values.ToList());
            //File.WriteAllText(m_emailSavePath, data);
            //}
            //catch (Exception ex1)
            //{
            //    Debug.LogError("222222");
            //    Debug.LogError(ex1);
            //}
        }

        private void ReadEmail()
        {
            try
            {
                InitEmailSavePath();
                string path = m_emailSavePath;
                //Debug.LogFormat("ReadEmail path:{0}", path);

                if (File.Exists(path))
                {
                    LitJson.JsonReader reader = new LitJson.JsonReader(File.ReadAllText(path));
                    List<EmailInfoEntity> tmpDic = LitJson.JsonMapper.ToObject<List<EmailInfoEntity>>(reader);
                    if (tmpDic == null)
                    {
                        return;
                    }
                    GetEmails.Clear();
                    foreach (var element in tmpDic)
                    {
                        if (element.sendTime >= 0)
                        {
                            GetEmails[element.emailIndex] = element;
                        }
                    }
                }
                m_redPointsIndex.Clear();
                foreach (var element in GetEmails)
                {
                    if (element.Value.status == 0)
                    {
                        m_redPointsIndex[element.Key] = true;
                        if (IsFirstOpenMail)
                        {
                            MailDefine define = CoreUtils.dataService.QueryRecord<MailDefine>((int)element.Value.emailId);
                            if (define != null)
                            {
                                MailCurrentPage = define.type - 1;
                                IsFirstOpenMail = false;
                            }
                        }
                    }
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.UpdateEmail);
            }
            catch (Exception ex)
            {
                Debug.Log("读取本地邮件失败：");
                Debug.Log(ex);
                DeleteEmail();
                EmailReceived = false;
                Email_GetEmails.request req = new Email_GetEmails.request();
                AppFacade.GetInstance().SendSproto(req);
            }
        }

        private void DeleteEmail()
        {
            InitEmailSavePath();
            if (File.Exists(m_emailSavePath))
            {
                File.Delete(m_emailSavePath);
            }
        }

        public void DeleteAlreadyReadEmails(List<EmailInfoEntity> mails)
        {
            for (int i = 0; i < mails.Count; i++)
            {
                if (mails[i].status == 1)
                {
                    mails[i].sendTime = -1;
                }
            }
        }

        public void DeleteEmail(EmailInfoEntity mail)
        {

            //mail.sendTime = -1;
        }

        //获取最近联系人保存路径
        public string GetEmailContactPath()
        {
            string folder = Path.Combine(Application.persistentDataPath, "EmailContact");
            //string folder = Path.Combine(Application.persistentDataPath, "Email/Contact");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return Path.Combine(folder, m_playerProxy.CurrentRoleInfo.rid.ToString());
        }

        //保存联系人到本地
        public void SaveNewContactInfo(List<WriteAMailData> newList)
        {
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            //找出新的联系人
            List<WriteAMailData> localList = GetContactInfo();
            if (localList.Count > 0)
            {
                Dictionary<long, bool> tDic = new Dictionary<long, bool>();
                for (int i = 0; i < localList.Count; i++)
                {
                    tDic[localList[i].stableRid] = true;
                }

                int count = newList.Count - 1;
                for (int i = count; i >= 0; i--)
                {
                    if (tDic.ContainsKey(newList[i].stableRid))
                    {
                        newList.RemoveAt(i);
                    }
                }
            }
            //如果联系人数量超出范围 则移除旧的联系人
            int total = newList.Count;
            if (total > 0)
            {
                for (int i = 0; i < total; i++)
                {
                    localList.Insert(0, newList[i]);
                }
                total = localList.Count;
                int diff = total - config.emailRecentContactsNum;
                if (diff > 0)
                {
                    int tTotal = total - 1;
                    for (int i = tTotal; i >= 0; i--)
                    {
                        if (diff > 0)
                        {
                            localList.RemoveAt(i);
                            diff = diff - 1;
                        }
                    }
                }
            }
            SaveContactToFile(localList);
        }

        //保存最近连续人信息到文件里
        public void SaveContactToFile(List<WriteAMailData> info)
        {
            try
            {
                string path = GetEmailContactPath();
                //Debug.LogError("文件路径:"+ path);
                string data = LitJson.JsonMapper.ToJson(info);
                File.WriteAllText(path, data);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        //获取最近联系人信息
        public List<WriteAMailData> GetContactInfo()
        {
            string path = GetEmailContactPath();
            List<WriteAMailData> list = null;
            if (File.Exists(path))
            {
                LitJson.JsonReader reader = new LitJson.JsonReader(File.ReadAllText(path));
                list = LitJson.JsonMapper.ToObject<List<WriteAMailData>>(reader);
            }
            if (list == null)
            {
                list = new List<WriteAMailData>();
            }
            return list;
        }

        //获取邮件草稿路径
        public string GetEmailDraftPath()
        {
            string folder = Path.Combine(Application.persistentDataPath, "EmailDraft");
            //string folder = Path.Combine(Application.persistentDataPath, "Email/Draft");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return Path.Combine(folder, m_playerProxy.CurrentRoleInfo.rid.ToString());
        }

        //保存草稿内容到文件
        public void SaveDraftInfoToFile(EmailDraftData info)
        {
            try
            {
                string path = GetEmailDraftPath();
                string data = LitJson.JsonMapper.ToJson(info);
                File.WriteAllText(path, data);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        //获取邮件草稿信息
        public EmailDraftData GetDraftInfo()
        {
            string path = GetEmailDraftPath();
            EmailDraftData data = null;
            if (File.Exists(path))
            {
                LitJson.JsonReader reader = new LitJson.JsonReader(File.ReadAllText(path));
                data = LitJson.JsonMapper.ToObject<EmailDraftData>(reader);
            }
            return data;
        }

        #endregion

        public void AddMenuSwitchVal()
        {
            MenuSwitchVal = MenuSwitchVal + 1;
            if (MenuSwitchVal > 10000)
            {
                MenuSwitchVal = 0;
            }
        }

        //坐标跳转
        public void CoordinateJump(string str)
        {
            string[] strList = str.Split(',');
            if (strList == null || strList.Length < 2)
            {
                return;
            }
            int jumpX = int.Parse(strList[0]);
            int jumpY = int.Parse(strList[1]);
            jumpX = Mathf.FloorToInt(jumpX / 600);
            jumpY = Mathf.FloorToInt(jumpY / 600);
            CoreUtils.uiManager.CloseUI(UI.s_Email);
            GameHelper.CoordinateJump(jumpX, jumpY);
            //WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 1000f, null);
            ////WorldCamera.Instance().ViewTerrainPos(jumpX * 6 + 3, jumpY * 6 + 3, 1000f, null);
            //WorldCamera.Instance().ViewTerrainPos(jumpX*6+3f, jumpY*6, 1000f, null);
        }

        public string CoordinateReverse(string str, bool isReverse)
        {
            if (isReverse && !string.IsNullOrEmpty(str))
            {
                string[] strList = str.Split(',');
                if (strList.Length == 2)
                {
                    return string.Format("{0},{1}", strList[1], strList[0]);
                }
            }
            return str;
        }

        #region 战斗详情

        //获取战报详情
        public BattleReportEx GetBattleReportEx(long emailIndex)
        {
            BattleReportEx info = null;
            m_fightDetailDic.TryGetValue(emailIndex, out info);
            return info;
        }

        //是否获取过战报详情
        public bool IsGetBattleReportEx(long emailIndex)
        {
            if (m_fightDetailDic.ContainsKey(emailIndex))
            {
                return true;
            }
            return false;
        }

        public void RequestFightDetail(long index, string url)
        {
            if (m_loadingFightDetailDic.ContainsKey(index))
            {
                return;
            }
            m_loadingFightDetailDic[index] = url;
            Debug.LogFormat("url:{0}", url);
            IGGStorageService.shareInstance().DownloadFile(url, (bool successed, IGGStorageService.WebRequestReturn respon) =>
            {
                if (m_isDispose)
                {
                    return;
                }
                if (respon == null)
                {
                    return;
                }
                long emailIndex = -1;
                foreach (var data in m_loadingFightDetailDic)
                {
                    if (data.Value == respon.url)
                    {
                        emailIndex = data.Key;
                        break;
                    }
                }
                if (emailIndex < 0 )
                {
                    return;
                }
                m_loadingFightDetailDic.Remove(emailIndex);

                if (successed && respon.errcode == 0)
                {
                    if (!File.Exists(respon.data))
                    {
                        Debug.LogErrorFormat("not find file:{0}", respon.data);
                        return;
                    }
                    var byteArr = File.ReadAllBytes(respon.data);
                    if (byteArr == null)
                    {
                        Debug.LogErrorFormat("read fightReport fail:{0}", respon.data);
                        return;
                    }
                    //string text = File.ReadAllText(respon.data);
                    //byte[] byteArr = Convert.FromBase64String(text);

                    //删除文件 避免文件漏删 越来越多 
                    File.Delete(respon.data);

                    //解包
                    SprotoPack packObj = new SprotoPack();
                    byte[] unData = packObj.unpack(byteArr);
                    if (unData == null)
                    {
                        Debug.LogError("unpack fightReport fail");
                        return;
                    }
                    BattleReportEx exInfo = new BattleReportEx(unData);
                    m_fightDetailDic[emailIndex] = exInfo;

                    Debug.LogFormat("战报下载完成 len:{0}", byteArr.Length);
                }
                else
                {
                    m_fightDetailDic[emailIndex] = null;
                    if (respon != null)
                    {
                        Debug.LogFormat(string.Format("战报下载失败:code:{0} message:", respon.errcode, respon.errmsg));
                    }
                    else
                    {
                        Debug.Log("战报下载失败:网络问题");
                    }
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.GetEmailFightDetail, emailIndex);
            }, true);
        }
        #endregion
    }
}