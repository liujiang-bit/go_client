// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, May 18, 2020
// Update Time         :    Monday, May 18, 2020
// Class Description   :    UI_Win_GuildMemberMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game
{
    public class UI_Win_GuildMemberMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Win_GuildMemberMediator";

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildMemberMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Win_GuildMemberView view;

        private long m_guildID;
        private GuildInfo m_guildInfo;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Guild_GetOtherGuildMembers.TagName
            }.ToArray();
        }

        private AllianceProxy m_allianceProxy;

        private Dictionary<long, GuildMemberInfoEntity>
            m_guildMemberDic = new Dictionary<long, GuildMemberInfoEntity>();

        private Dictionary<long, GuildOfficerInfoEntity>
            m_officersDic = new Dictionary<long, GuildOfficerInfoEntity>(4);

        private Dictionary<long, GuildOfficerInfoEntity> m_officersIDDic =
            new Dictionary<long, GuildOfficerInfoEntity>(4);

        private List<AllianceMemberLevel> m_guildMember = new List<AllianceMemberLevel>(4);

        private Dictionary<long, AllianceMemberLevel> m_guildMemberLvDic = new Dictionary<long, AllianceMemberLevel>(5);

        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<AllianceMemberLevel> m_guildMemberList = new List<AllianceMemberLevel>(4);

        private PlayerProxy m_playerProxy;

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_GetOtherGuildMembers.TagName:
                    Guild_GetOtherGuildMembers.response request =
                        notification.Body as Guild_GetOtherGuildMembers.response;
                    if (request.HasGuildMembers)
                    {
                        foreach (var apply in request.guildMembers)
                        {
                            var member = apply.Value;
                            long rid = member.rid;
                            GuildMemberInfoEntity info;

                            bool hasChangeLv = false;

                            long oldLv = 0;

                            if (!m_guildMemberDic.TryGetValue(rid, out info))
                            {
                                info = new GuildMemberInfoEntity();
                                m_guildMemberDic.Add(rid, info);

                                AddToMemberLv(member.guildJob, info);
                            }

                            GuildMemberInfoEntity.updateEntity(info, member);
                        }
                    }

                    if (request.HasGuildOfficers)
                    {
                        foreach (var valuePair in request.guildOfficers)
                        {
                            long officerId = valuePair.Value.officerId;
                            GuildOfficerInfoEntity info;
                            if (!m_officersIDDic.TryGetValue(officerId, out info))
                            {
                                info = new GuildOfficerInfoEntity();

                                m_officersIDDic.Add(officerId, info);
                            }

                            if (info.rid > 0 && valuePair.Value.rid == 0)
                            {
                                m_officersDic.Remove(info.rid);
                            }

//                    Debug.Log(info.rid+" 官职  "+valuePair.Value.rid+"  "+valuePair.Value.officerId);

                            GuildOfficerInfoEntity.updateEntity(info, valuePair.Value);

                            if (info.rid > 0)
                            {
                                if (!m_officersDic.ContainsKey(info.rid))
                                {
                                    m_officersDic.Add(info.rid, info);
                                }
                            }
                        }

//                Debug.Log(m_officersIDDic.Count+" iD 官职 mem "+m_officersDic.Count);
                    }

                    ReMemberList();
                    RePlayer();

                    break;

                default:
                    break;
            }
        }

        public void AddToMemberLv(long guildJob, GuildMemberInfoEntity infoEntity)
        {
            AllianceMemberLevel lv;
            if (m_guildMemberLvDic.TryGetValue(guildJob, out lv))
            {
                lv.LevelMembers.Add(infoEntity);
            }
        }

        public void RemoveToMemberLv(long guildJob, GuildMemberInfoEntity infoEntity)
        {
            AllianceMemberLevel lv;
            if (m_guildMemberLvDic.TryGetValue(guildJob, out lv))
            {
                lv.LevelMembers.Remove(infoEntity);
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
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            
            getAllianceMembers();
            if (view.data is GuildInfo)
            {
                SendGetGuildInfo((GuildInfo) view.data);
            }
        }

        private void SendGetGuildInfo(GuildInfo guildInfo)
        {
            m_guildID = guildInfo.guildId;
            m_guildInfo = guildInfo;
            Guild_GetOtherGuildMembers.request request = new Guild_GetOtherGuildMembers.request();

            request.guildId = m_guildID;

            AppFacade.GetInstance().SendSproto(request);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);
            view.m_btn_PlayerHead_GameButton.onClick.AddListener(onMasterHeader);
        }

        private void onMasterHeader()
        {
            var data = m_guildMemberDic[m_guildInfo.leaderRid];

            if (data != null)
            {
                ShowPopMemu(data, view.m_btn_PlayerHead_GameButton.transform as RectTransform);
            }
        }


        public void RePlayer()
        {

            if (m_guildMemberDic.ContainsKey(m_guildInfo.leaderRid))
            {
                var lvInfo = m_guildMemberDic[m_guildInfo.leaderRid];

                if (lvInfo != null)
                {
                    view.m_lbl_name_LanguageText.text = lvInfo.name;


                    view.m_lbl_power1_LanguageText.text = ClientUtils.FormatComma(lvInfo.combatPower);

                    view.m_lbl_kill_LanguageText.text =
                        ClientUtils.FormatComma(m_playerProxy.KillCountOther(lvInfo.killCount));
                    view.m_UI_PlayerHead.LoadHeadCountry((int) lvInfo.headId);

                    if (lvInfo != null && lvInfo.guildJob > 0)
                    {
                        var cdata = CoreUtils.dataService.QueryRecord<AllianceMemberDefine>((int) lvInfo.guildJob);
                        if (cdata != null)
                        {
                            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage, cdata.icon);
                        }
                    }
                }
            }
           
        }


        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMember);
        }

        protected override void BindUIData()
        {
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                funcTab.GetItemSize = GetItemSize;
                funcTab.GetItemPrefabName = GetItemPrefabName;

                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);

                ReMemberList();
                RePlayer();
            });
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            var data = m_guildMemberList[item.index];

            return view.m_sv_list_ListView.ItemPrefabDataList[data.prefab_index];
        }

        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_guildMemberList[item.index];

            if (data.prefab_index == 0)
            {
                return 73f;
            }
            else if (data.prefab_index == 1)
            {
                return 150f;
            }

            return 90f;
        }
        
        public GuildOfficerInfoEntity getMemberOfficer(long rid)
        {
            GuildOfficerInfoEntity info;
            m_officersDic.TryGetValue(rid, out info);
            return info;
        }


        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = m_guildMemberList[scrollItem.index];


            if (data.prefab_index == 0)
            {
                UI_Item_GuildMemRankView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildMemRankView>(scrollItem.go);


                if (itemView != null)
                {
                    itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(data.data.l_nameID);

                    if (data.data.lv == 4)
                    {
                        itemView.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(730117,
                            data.LevelMembers.Count, data.data.researchersLimit);
                    }
                    else
                    {
                        itemView.m_lbl_count_LanguageText.text = data.LevelMembers.Count.ToString();
                    }

                    ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, data.data.icon);

                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);


                    var index = scrollItem.index;

                    itemView.m_btn_bg_GameButton.onClick.RemoveAllListeners();

                    itemView.m_btn_bg_GameButton.onClick.AddListener(() =>
                        {
                            data.isSelected = !data.isSelected;

                            itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                            itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);

                            if (data.isSelected)
                            {
                                if (data.LevelMembers != null && data.LevelMembers.Count > 0)
                                {
                                    AddMember(data.data.lv, index, data);
                                    view.m_sv_list_ListView.FillContent(m_guildMemberList.Count);
                                }
                            }
                            else
                            {
                                if (data.LevelMembers != null && data.LevelMembers.Count > 0)
                                {
                                    RemoveMember(data.data.lv);
                                    view.m_sv_list_ListView.FillContent(m_guildMemberList.Count);
                                }
                            }
                        }
                    );
                }
            }
            else if (data.prefab_index == 1)
            {
                UI_LC_GuildMemR4View itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_LC_GuildMemR4View>(scrollItem.go);

                if (itemView != null)
                {
                    var subItems =
                        MonoHelper.GetHotFixViewComponentsInChildren<UI_Item_GuildMemR4View>(itemView.gameObject);
                    var len = subItems.Length;
                    for (int i = 0; i < len; i++)
                    {
                        var subItem = subItems[i];
                        var subData = data.subItemsData.Count - 1 >= i ? data.subItemsData[i] : null;

                        subItem.gameObject.SetActive(subData != null);

                        if (subData != null)
                        {
                            subItem.m_UI_PlayerHead.LoadHeadCountry((int) subData.headId);
                            subItem.m_lbl_name_LanguageText.text = subData.name;
                            subItem.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(subData.combatPower);
                            subItem.m_btn_online_GameButton.gameObject.SetActive(false); //盟主

                            var officer = getMemberOfficer(subData.rid);
                            if (officer != null && officer.officerId > 0)
                            {
                                var cdata =
                                    CoreUtils.dataService
                                        .QueryRecord<AllianceOfficiallyDefine>((int) officer.officerId);

                                if (cdata != null)
                                {
                                    ClientUtils.LoadSprite(subItem.m_img_office_PolygonImage, cdata.icon);
                                }
                                subItem.m_img_office_PolygonImage.gameObject.SetActive(true);
                                subItem.m_btn_office_GameButton.gameObject.SetActive(true);

                                subItem.m_btn_job_GameButton.gameObject.SetActive(false);
                            }
                            else
                            {
                                subItem.m_img_office_PolygonImage.gameObject.SetActive(false);
                                subItem.m_btn_office_GameButton.gameObject.SetActive(false);
                                subItem.m_btn_job_GameButton.gameObject.SetActive(false);
                            }
                        }
                        
                        subItem.m_btn_rect_GameButton.onClick.RemoveAllListeners();

                        subItem.m_btn_rect_GameButton.onClick.AddListener(() =>
                        {
                            ShowPopMemu(subData, subItem.m_btn_rect_GameButton.transform as RectTransform);
                        });
                    }
                }
            }
            else
            {
                UI_LC_GuildMemR0View itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_LC_GuildMemR0View>(scrollItem.go);

                if (itemView != null)
                {
                    var subItems =
                        MonoHelper.GetHotFixViewComponentsInChildren<UI_Item_GuildMemR0View>(itemView.gameObject);
                    var len = subItems.Length;
                    for (int i = 0; i < len; i++)
                    {
                        var subItem = subItems[i];
                        var subData = data.subItemsData.Count - 1 >= i ? data.subItemsData[i] : null;

                        subItem.gameObject.SetActive(subData != null);

                        if (subData != null)
                        {
                            subItem.m_UI_PlayerHead.LoadHeadCountry((int) subData.headId);
                            subItem.m_lbl_name_LanguageText.text = subData.name;
                            subItem.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(subData.combatPower);

                            subItem.m_btn_bg_GameButton.onClick.RemoveAllListeners();

                            subItem.m_btn_bg_GameButton.onClick.AddListener(() =>
                            {
                                ShowPopMemu(subData, subItem.m_btn_bg_GameButton.transform as RectTransform);
                            });
                        }
                    }
                }
            }
        }

        private HelpTip m_tipView;
        private UI_Pop_GuildMemTipView powTipView;

        private void ShowPopMemu(GuildMemberInfoEntity subData, RectTransform transform)
        {
            CoreUtils.assetService.Instantiate("UI_Pop_GuildMemTip", (obj) =>
            {
                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_GuildMemTipView>(obj);

                powTipView.m_UI_res_help.gameObject.SetActive(false);
                powTipView.m_UI_help.gameObject.SetActive(false);

                powTipView.m_UI_plus.gameObject.SetActive(false);
                powTipView.m_UI_remove.gameObject.SetActive(false);
                
                ClientUtils.UIReLayout(powTipView.m_pl_bg_ContentSizeFitter.gameObject);


                if (LanguageUtils.IsArabic())
                {
                    m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.GetComponent<RectTransform>().sizeDelta, transform)
                        .SetStyle(HelpTipData.Style.arrowRight).Show();
                }
                else
                {
                    m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.GetComponent<RectTransform>().sizeDelta, transform)
                        .SetStyle(HelpTipData.Style.arrowLeft).Show();

                }

               
                
                powTipView.m_UI_info.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_OtherPlayerInfo,null,subData.rid);
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);

                });


               powTipView.m_UI_mail.m_btn_languageButton_GameButton.onClick.AddListener(() =>
               {
                   
                   WriteAMailData mailData = new WriteAMailData();

                   mailData.stableName = subData.name;
                   mailData.stableRid = subData.rid;
                   
                   CoreUtils.uiManager.ShowUI(UI.s_writeAMail,null,mailData);
                   CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);

               });


               
            });
        }


        public void AddMember(int grade, int index, AllianceMemberLevel lvmember)
        {
            if (grade == 4)
            {
                int len = lvmember.LevelMembers.Count;

                for (int i = 0; i < len; i = i + 4)
                {
                    AllianceMemberLevel nMemberLevel = new AllianceMemberLevel();

                    nMemberLevel.prefab_index = 1;
                    nMemberLevel.data = lvmember.data;

                    nMemberLevel.subItemsData = new List<GuildMemberInfoEntity>();


                    for (int j = i; j < i + 4; j++)
                    {
                        if (j < len)
                        {
                            nMemberLevel.subItemsData.Add(lvmember.LevelMembers[j]);
                        }
                    }

                    m_guildMemberList.Insert(index + 1, nMemberLevel);
                    index++;
                }
            }
            else
            {
                int len = lvmember.LevelMembers.Count;

                for (int i = 0; i < len; i = i + 2)
                {
                    AllianceMemberLevel nMemberLevel = new AllianceMemberLevel();

                    nMemberLevel.prefab_index = 2;
                    nMemberLevel.data = lvmember.data;

                    nMemberLevel.subItemsData = new List<GuildMemberInfoEntity>();


                    for (int j = i; j < i + 2; j++)
                    {
                        if (j < len)
                        {
                            nMemberLevel.subItemsData.Add(lvmember.LevelMembers[j]);
                        }
                    }

                    m_guildMemberList.Insert(index + 1, nMemberLevel);
                    index++;
                }
            }
        }

        public void RemoveMember(int lv)
        {
            int startIndex = 0;

            int count = 0;

            int len = m_guildMemberList.Count;

            for (int i = 0; i < len; i++)
            {
                var item = m_guildMemberList[i];

                if (item.data.lv == lv)
                {
                    if (startIndex == 0)
                    {
                        startIndex = i + 1;
                    }
                    else
                    {
                        count = count + 1;
                    }
                }
            }

            m_guildMemberList.RemoveRange(startIndex, count);
        }

        public List<AllianceMemberLevel> getAllianceMembers()
        {
            if (m_guildMember.Count == 0)
            {
                var members = m_allianceProxy.AllianceMemberDefines();

                if (members.Count > 0 && members[0].lv == 1)
                {
                    members.Reverse();
                }

                foreach (var member in members)
                {
                    var ml = new AllianceMemberLevel();

                    ml.data = member;
                    ml.num = 0;
                    ml.prefab_index = 0;

                    if (member.lv == 4)
                    {
                        ml.isSelected = true;
                    }

                    if (member.lv < 5)
                    {
                        m_guildMember.Add(ml);
                    }

                    m_guildMemberLvDic.Add(member.lv, ml);
                }
            }

            return m_guildMember;
        }


        public void ReMemberList()
        {
            if (m_assetDic.Count > 0)
            {
                var initdatas = getAllianceMembers();


                m_guildMemberList.Clear();

                m_guildMemberList.AddRange(initdatas);


                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 0; j < m_guildMemberList.Count; j++)
                    {
                        var level = m_guildMemberList[j];

                        if (level.data.lv == i && level.isSelected)
                        {
                            AddMember(i, j, level);
                        }
                    }
                }

                view.m_sv_list_ListView.FillContent(m_guildMemberList.Count);
            }
        }

        #endregion
    }
}