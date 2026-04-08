// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年10月27日
// Update Time         :    2020年10月27日
// Class Description   :    UI_Item_MailType18_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using System;
using SprotoType;

namespace Game {

    public class MailMemberActiveInfo
    {
        public long LastTime;
        public string roleName;
        public long roleRid;
        public long roleHeadId;
        public long roleHeadFrameId;
        public long guildId;
    } 

    public partial class UI_Item_MailType18_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;

        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;
        private bool m_isInitList;
        private int m_prefabLoadStatus;  //1未加载 2加载中 3已加载

        private Dictionary<string, GameObject> m_assetDic;

        private List<InactiveMembersInfo> m_memberList;

        private HelpTip m_tipView;

        private bool m_isLoadingTip;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            m_isLoadingTip = false;
            if (!m_isInit)
            {
                m_prefabLoadStatus = 1;
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_isInit = true;
            }
            if (mailInfo.guildEmail == null)
            {
                return;
            }

            m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), mailInfo.titleContents);
            m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_subheadingID), mailInfo.subTitleContents);
            m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(mailInfo.sendTime);

            if (!string.IsNullOrEmpty(mailDefine.poster))
            {
                ClientUtils.LoadSprite(m_UI_Item_MailTitle.m_img_bg_PolygonImage, mailDefine.poster);
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }

            if (mailInfo.guildEmail.inactiveMembers == null)
            {
                return;
            }

            ReadData();

            //显示列表
            if (m_prefabLoadStatus == 1)
            {
                m_prefabLoadStatus = 2;
                ClientUtils.PreLoadRes(gameObject, m_sv_list_view_ListView.ItemPrefabDataList, OnLoadFinish);
            }
            else if (m_prefabLoadStatus == 3)
            {
                RefreshList();
            }
        }

        private void ReadData()
        {
            m_memberList = m_emailInfo.guildEmail.inactiveMembers;
        }

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assetDic = asset;

            m_prefabLoadStatus = 3;

            RefreshList();
        }

        private void RefreshList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ListViewItemByIndex;
                m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
                m_isInitList = true;
            }
            m_sv_list_view_ListView.FillContent(m_memberList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            InactiveMembersInfo itemData = m_memberList[listItem.index];
            UI_Item_MailType18List_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_MailType18List_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.BtnClickEvent = ClickEvent;
            }
            else
            {
                subView = listItem.data as UI_Item_MailType18List_SubView;
            }
            subView.Refresh(itemData);
        }

        private void ClickEvent(UI_Item_MailType18List_SubView subView)
        {
            if (m_isLoadingTip)
            {
                return;
            }
            m_isLoadingTip = true;
            InactiveMembersInfo itemData = subView.GetItemData();
            CoreUtils.assetService.Instantiate("UI_Pop_MailType18", (obj) => {
                if (gameObject == null)
                {
                    CoreUtils.assetService.Destroy(obj);
                    return;
                }
                UI_Pop_MailType18_SubView nodeView = new UI_Pop_MailType18_SubView(obj.GetComponent<RectTransform>());

                if (LanguageUtils.IsArabic())
                {
                    m_tipView = HelpTip.CreateTip(nodeView.gameObject, 
                                                  nodeView.m_pl_pos_Animator.GetComponent<RectTransform>().sizeDelta, 
                                                  subView.m_UI_Model_PlayerHead.m_root_RectTransform)
                                                  .SetStyle(HelpTipData.Style.arrowRight).Show(TipCreateFinishCallback);
                }
                else
                {
                    m_tipView = HelpTip.CreateTip(nodeView.gameObject, 
                                                  nodeView.m_pl_pos_Animator.GetComponent<RectTransform>().sizeDelta, 
                                                  subView.m_UI_Model_PlayerHead.m_root_RectTransform)
                                                  .SetStyle(HelpTipData.Style.arrowLeft).Show(TipCreateFinishCallback);
                }

                //邮件
                nodeView.m_btn_mail.m_btn_languageButton_GameButton.onClick.AddListener(()=> {
                    WriteAMailData mailData = new WriteAMailData();
                    mailData.stableName = itemData.name;
                    mailData.stableRid = itemData.rid;
                    CoreUtils.uiManager.ShowUI(UI.s_writeAMail, null, mailData);
                    CoreUtils.assetService.Destroy(obj.gameObject.transform.parent.gameObject);
                });

                //删除
                nodeView.m_btn_remove.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                    GuildMemberInfoEntity memberInfo = new GuildMemberInfoEntity();
                    memberInfo.rid = itemData.rid;
                    Alert.CreateAlert(730164, LanguageUtils.getText(730163))
                                      .SetLeftButton(() => {                                          
                                          CoreUtils.uiManager.ShowUI(UI.s_AllianceMemRemove, null, memberInfo);
                                      },
                                      LanguageUtils.getText(730154)).SetRightButton(null, LanguageUtils.getText(730155))
                                      .Show();
                    CoreUtils.assetService.Destroy(obj.gameObject.transform.parent.gameObject);
                });
            });
        }

        private void TipCreateFinishCallback()
        {
            m_isLoadingTip = false;
        }
    }
}