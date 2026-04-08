// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_CaptainTalent_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public partial class  UI_Item_CaptainTalent_SubView : UI_SubView
    {

        private HeroProxy.Hero m_Hero;
        private HeroTalentDefine m_talentDefineBlue;
        private HeroTalentDefine m_talentDefineRed;
        private HeroTalentDefine m_talentDefineYellow;
        private List<HeroTalentGainTreeDefine> m_talentTreeListBlue = new List<HeroTalentGainTreeDefine>();
        private List<HeroTalentGainTreeDefine> m_talentTreeListRed = new List<HeroTalentGainTreeDefine>();
        private List<HeroTalentGainTreeDefine> m_talentTreeListYellow = new List<HeroTalentGainTreeDefine>();

        private int m_MaxTalentLevel;
        private bool m_IsPageShow = false;
        private int m_CurIndex;
        private bool isFirst = true;
        

        protected override void BindEvent()
        {
            m_btn_back_GameButton.onClick.AddListener(OnPageClickEvent);
            m_btn_change.m_btn_languageButton_GameButton.onClick.AddListener(OnChangePageEvent);
        }
        
        
        public void Open(HeroProxy.Hero hero)
        {
            m_Hero = hero;
            gameObject.SetActive(true);
            /*var anim = gameObject.GetComponent<Animator>();
            if (!anim)
                return;
            if (LanguageUtils.IsArabic())
            {
                ClientUtils.PlayUIAnimation(anim, "OpenArb");
            }
            else
            {
                ClientUtils.PlayUIAnimation(anim, "OpenNoArb");
            }*/
            if (isFirst)
            {
                Init();
                isFirst = false;
            }

            InitData();
        }

        private void Init()
        {
            SubViewManager.Instance.AddListener(new string[] {
                CmdConstant.ClickTalentPage,
                Hero_ChangeTalentIndex.TagName,
                Hero_ResetTalent.TagName,
                Hero_TalentUp.TagName,
                Hero_ModifyTalentName.TagName
            },this.gameObject, OnNotification);
        }

        public void InitData()
        {
            if (m_Hero == null) return;

            m_CurIndex = m_Hero.talentIndex;
            var talentDatas = m_Hero.config.talent;
            if (talentDatas.Count > 0)
            {
                m_talentDefineRed = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>(talentDatas[0]);
                if (m_talentDefineRed != null)
                {
                    m_UI_Item_TalentTop2.gameObject.SetActive(true);
                    m_UI_Item_TalentTop2.InitData(m_Hero,m_talentDefineRed,m_UI_Item_TalentSpecialPop.gameObject.GetComponent<RectTransform>(),m_UI_Item_TalentSpecialPop);
                    m_UI_Item_TalentTop2.Refresh(m_Hero.talentIndex);
                }
                else
                {
                    m_UI_Item_TalentTop2.gameObject.SetActive(false);
                }
            }

            if (talentDatas.Count > 1)
            {
                m_talentDefineYellow = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>(talentDatas[1]);
                if (m_talentDefineYellow != null)
                {
                    m_UI_Item_TalentTop1.gameObject.SetActive(true);
                    m_UI_Item_TalentTop1.InitData(m_Hero,m_talentDefineYellow,m_UI_Item_TalentSpecialPop.gameObject.GetComponent<RectTransform>(),m_UI_Item_TalentSpecialPop);
                    m_UI_Item_TalentTop1.Refresh(m_Hero.talentIndex);
                }
                else
                {
                    m_UI_Item_TalentTop1.gameObject.SetActive(false);
                }
            }
            
            if (talentDatas.Count > 2)
            {
                m_talentDefineBlue = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>(talentDatas[2]);
                if (m_talentDefineBlue != null)
                {
                    m_UI_Item_TalentTop3.gameObject.SetActive(true);
                    m_UI_Item_TalentTop3.InitData(m_Hero,m_talentDefineBlue,m_UI_Item_TalentSpecialPop.gameObject.GetComponent<RectTransform>(),m_UI_Item_TalentSpecialPop);
                    m_UI_Item_TalentTop3.Refresh(m_Hero.talentIndex);
                }
                else
                {
                    m_UI_Item_TalentTop3.gameObject.SetActive(false);
                }
            }
            
            m_talentTreeListBlue.Clear();
            m_talentTreeListRed.Clear();
            m_talentTreeListYellow.Clear();
            var talentTreeDatas = CoreUtils.dataService.QueryRecords<Data.HeroTalentGainTreeDefine>();
            foreach (var treeData in talentTreeDatas)
            {
                if (m_talentDefineBlue != null && treeData.gainTree == int.Parse(m_talentDefineBlue.gainTree))
                {
                    m_talentTreeListBlue.Add(treeData);
                }
                
                if (m_talentDefineRed != null && treeData.gainTree == int.Parse(m_talentDefineRed.gainTree))
                {
                    m_talentTreeListRed.Add(treeData);
                }
                
                if (m_talentDefineYellow != null && treeData.gainTree == int.Parse(m_talentDefineYellow.gainTree))
                {
                    m_talentTreeListYellow.Add(treeData);
                }
            }
            m_talentTreeListBlue.Sort((HeroTalentGainTreeDefine a, HeroTalentGainTreeDefine b) =>
            {
                return a.level.CompareTo(b.level);
            });
            m_talentTreeListRed.Sort((HeroTalentGainTreeDefine a, HeroTalentGainTreeDefine b) =>
            {
                return a.level.CompareTo(b.level);
            });
            m_talentTreeListYellow.Sort((HeroTalentGainTreeDefine a, HeroTalentGainTreeDefine b) =>
            {
                return a.level.CompareTo(b.level);
            });
            m_MaxTalentLevel = Math.Max(m_talentTreeListBlue.Count, m_talentTreeListBlue.Count);
            m_MaxTalentLevel = Math.Max(m_MaxTalentLevel, m_talentTreeListYellow.Count);

            //m_sv_talentList_ListView.onValueChanged = OnValueChanged;
            
            RefreshTalentPoint();
            RefreshBar();
            RefreshTalentTab();
            
            ClientUtils.PreLoadRes(gameObject, m_sv_talentList_ListView.ItemPrefabDataList, LoadTalentItemFinish);
        }
        
        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ClickTalentPage:
                    int index = (int) notification.Body;
                    m_CurIndex = index;
                    Refresh();
                    break;
                case Hero_ModifyTalentName.TagName:
                    Hero_ModifyTalentName.response reName = notification.Body as Hero_ModifyTalentName.response;
                    if (reName.result)
                    {
                        Tip.CreateTip(175027).Show();
                        RefreshTalentTab();
                    }
                    else
                    {
                        Debug.LogWarning("Hero_ModifyTalentName response is false");
                    }

                    break;
                case Hero_ResetTalent.TagName:
                    Hero_ResetTalent.response reReset = notification.Body as Hero_ResetTalent.response;
                    if (reReset.result)
                    {
                        Refresh();
                        Tip.CreateTip(179000).Show();
                    }
                    else
                    {
                        Debug.LogWarning("Hero_ResetTalent response is false");
                    }

                    break;
                case Hero_ChangeTalentIndex.TagName:
                    Hero_ChangeTalentIndex.response reChange = notification.Body as Hero_ChangeTalentIndex.response;
                    if (reChange.result)
                    {
                        Refresh();
                    }
                    else
                    {
                        Debug.LogWarning("Hero_ChangeTalentIndex response is false");
                    }
                    break;
                case Hero_TalentUp.TagName:
                    Hero_TalentUp.response reUp = notification.Body as Hero_TalentUp.response;
                    if (reUp.result)
                    {
                        Refresh();
                    }
                    else
                    {
                        Debug.LogWarning("Hero_TalentUp response is false");
                    }
                    break;
                default:
                    break;
            }
        }
        
        private void LoadTalentItemFinish(Dictionary<string, GameObject> dic)
        {
            m_sv_talentList_ListView.Clear();
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitTalentListItem;
            m_sv_talentList_ListView.SetInitData(dic, functab);
            m_sv_talentList_ListView.FillContent(m_MaxTalentLevel);
        }

        private void InitTalentListItem(ListView.ListItem item)
        {
            UI_LC_talentList_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_LC_talentList_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_LC_talentList_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_MaxTalentLevel) return;
            var treeDefineBlue = item.index < m_talentTreeListBlue.Count ? m_talentTreeListBlue[item.index] : null;
            var treeDefineRed = item.index < m_talentTreeListRed.Count ? m_talentTreeListRed[item.index] : null;
            var treeDefineYellow = item.index < m_talentTreeListYellow.Count ? m_talentTreeListYellow[item.index] : null;
            subView.Init(m_Hero,m_CurIndex,treeDefineBlue,treeDefineRed,treeDefineYellow,m_UI_Item_TalentSkillPop.gameObject.GetComponent<RectTransform>(),m_UI_Item_TalentSkillPop);
        }

        
        private void Refresh()
        {
            RefreshTalentPoint();
            RefreshMastery();
            RefreshTalentList();
            RefreshBar();
            RefreshTalentTab();
        }

        private void RefreshTalentPoint()
        {
            m_lbl_talentPoint_LanguageText.text = LanguageUtils.getTextFormat(175016, m_Hero.GetCurPageRemainPoint(m_CurIndex));
            HeroInfo.TalentTrees talentTrees = m_Hero.GetTalentTreesByIndex(m_CurIndex);
            if (talentTrees != null && talentTrees.talentTree.Count > 0)
            {
                m_btn_reset.m_btn_languageButton_GameButton.onClick.AddListener(OnResetEvent);
                m_btn_reset.SetGray(false);
            }
            else
            {
                m_btn_reset.m_btn_languageButton_GameButton.onClick.RemoveListener(OnResetEvent);
                m_btn_reset.SetGray(true);
            }

        }

        private void RefreshBar()
        {
            if (m_Hero == null) return;
            int level = m_Hero.GetLevelByIndex(m_CurIndex);
            for (int i = 0; i < m_pl_point_GridLayoutGroup.transform.childCount; i++)
            {
                if (i < level)
                {
                    m_pl_point_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    m_pl_point_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            m_pb_line_GameSlider.value = level;
        }

        private void RefreshTalentTab()
        {
            if (m_Hero == null) return;

            m_UI_TalentPage0.Init(m_Hero,1,m_CurIndex);
            
            m_UI_TalentPage1.Init(m_Hero,2,m_CurIndex);
            
            m_UI_TalentPage2.Init(m_Hero,3,m_CurIndex);

            if (m_CurIndex != m_Hero.talentIndex)
            {
                m_btn_change.gameObject.SetActive(true);
            }
            else
            {
                m_btn_change.gameObject.SetActive(false);
            }
        }

        private void RefreshMastery()
        {
            if (m_Hero == null) return;
            m_UI_Item_TalentTop1.Refresh(m_CurIndex);
            m_UI_Item_TalentTop2.Refresh(m_CurIndex);
            m_UI_Item_TalentTop3.Refresh(m_CurIndex);
        }

        private void RefreshTalentList()
        {
            m_sv_talentList_ListView.ForceRefresh();
        }

        private void OnValueChanged(float posY)
        {
            m_pl_pointbg_GridLayoutGroup.transform.localPosition = new Vector3(0.0f,posY,0.0f);
            m_pl_point_GridLayoutGroup.transform.localPosition = new Vector3(0.0f,posY + 30,0.0f);
            m_pb_line_GameSlider.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f,posY - 596,0.0f);
        }
        

        private void OnPageClickEvent()
        {
            Vector3 oldScale = m_btn_back_GameButton.transform.localScale;
            if (m_IsPageShow)
            {
                m_img_bg_PolygonImage.gameObject.SetActive(false);
                m_pl_talentPages_GridLayoutGroup.gameObject.SetActive(false);
                m_btn_change.gameObject.SetActive(false);
            }
            else
            {
                m_img_bg_PolygonImage.gameObject.SetActive(true);
                m_pl_talentPages_GridLayoutGroup.gameObject.SetActive(true);
                if (m_CurIndex != m_Hero.talentIndex)
                {
                    m_btn_change.gameObject.SetActive(true);
                }
                else
                {
                    m_btn_change.gameObject.SetActive(false);
                }
            }
            m_btn_back_GameButton.transform.localScale = new Vector3(-oldScale.x,oldScale.y,oldScale.z);
            m_IsPageShow = !m_IsPageShow;
        }

        private void OnChangePageEvent()
        {
            CoreUtils.uiManager.ShowUI(UI.s_talentChangeAlert, null,
                new Game.TalentChangeData(LanguageUtils.getText(175028), () =>
                {
                    int resetItemId = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).talentResetItemID;
                    ItemDefine itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(resetItemId);
                    BagProxy bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                    PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                    CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(Game.CurrencyProxy.ProxyNAME) as CurrencyProxy;
                    long itemCount = bagProxy.GetItemNum(resetItemId);
                    bool isUseDenar = false;
            
                    if (itemCount <= 0)
                    {
                        if (playerProxy.CurrentRoleInfo.denar >= itemConfig.shortcutPrice)
                        {
                            isUseDenar = true;
                        }
                        else
                        {
                            currencyProxy.ShortOfDenar(itemConfig.shortcutPrice);
                            return;
                        }
                    }
                        
                    ChangeTalentPage(m_CurIndex,isUseDenar);
                }));
        }

        private void OnResetEvent()
        {    
            //当前天赋页重置需要弹窗
            if (m_CurIndex == m_Hero.talentIndex)
            {
                CoreUtils.uiManager.ShowUI(UI.s_talentChangeAlert, null,
                    new Game.TalentChangeData(LanguageUtils.getText(128020), () =>
                    {
                        int resetItemId = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).talentResetItemID;
                        ItemDefine itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>(resetItemId);
                        BagProxy bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                        PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                        CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(Game.CurrencyProxy.ProxyNAME) as CurrencyProxy;
                        long itemCount = bagProxy.GetItemNum(resetItemId);
                        bool isUseDenar = false;
            
                        if (itemCount <= 0)
                        {
                            if (playerProxy.CurrentRoleInfo.denar >= itemConfig.shortcutPrice)
                            {
                                isUseDenar = true;
                            }
                            else
                            {
                                currencyProxy.ShortOfDenar(itemConfig.shortcutPrice);
                                return;
                            }
                        }
                        
                        ResetTalent(m_Hero.talentIndex,isUseDenar);
                    }));
            }
            else
            {
                ResetTalent(m_CurIndex,false);
            }
        }

        private void ChangeTalentPage(int index,bool isUseDenar)
        {
            Hero_ChangeTalentIndex.request request = new Hero_ChangeTalentIndex.request();
            request.heroId = m_Hero.data.heroId;
            request.index = index;
            request.useDenar = isUseDenar;
            AppFacade.GetInstance().SendSproto(request);
        }

        private void ResetTalent(int index,bool isUseDenar)
        {
            Hero_ResetTalent.request request = new Hero_ResetTalent.request();
            request.heroId = m_Hero.data.heroId;
            request.index = index;
            request.useDenar = isUseDenar;
            AppFacade.GetInstance().SendSproto(request);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}