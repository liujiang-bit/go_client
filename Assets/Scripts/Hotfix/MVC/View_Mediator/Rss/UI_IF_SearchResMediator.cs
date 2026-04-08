// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_IF_SearchResMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using DG.Tweening;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game
{
    public class SearchJump
    {
        public SearchType searchType;
        public int level;

        public SearchJump(SearchType searchType, int level)
        {
            this.searchType = searchType;
            this.level = level;
        }
    }
    public class UI_IF_SearchResMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_IF_SearchResMediator";
        private SearchProxy m_SearchProxy;
        private RssProxy m_RssProxy;
        private MonsterProxy m_MonsterProxy;
        private PlayerProxy m_playerProxy;


        #endregion

        //IMediatorPlug needs
        public UI_IF_SearchResMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_IF_SearchResView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Map_SearchResource.TagName,
                Map_SearchBarbarian.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Map_SearchResource.TagName:
                    {
                        Map_SearchResource.response info = notification.Body as Map_SearchResource.response;
                                if (info != null&& info.resources.Count > 0)
                            {
                            CoreUtils.uiManager.CloseUI(UI.s_iF_SearchRes);
                        }
                    }
                            break;
                    break;
                default:
                    break;
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
            m_SearchProxy = AppFacade.GetInstance().RetrieveProxy(SearchProxy.ProxyNAME) as SearchProxy;
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_SearchProxy.SetValue();
            if (view.data is int)
            {
                m_SearchProxy.searchType = (SearchType)((int)view.data);
            }
            else if (view.data is GOScrptGuide)
            {
                GOScrptGuide GOScrptGuide = view.data as GOScrptGuide;
                m_SearchProxy.searchType = (SearchType)GOScrptGuide.param1;             
                if (GOScrptGuide.param2 != 0)
                {
                    if (m_SearchProxy.searchType == SearchType.Barbarian)
                    {
                        m_SearchProxy.currBarbarianLevel = GOScrptGuide.param2;
                    }
                    else
                    {
                        m_SearchProxy.currCurrLevel = GOScrptGuide.param2;
                    }
                }
            }
            else if (view.data is SearchJump)
            {
                SearchJump searchJump = view.data as SearchJump;
                m_SearchProxy.searchType = searchJump.searchType;
                if (m_SearchProxy.searchType == SearchType.Barbarian)
                {
                    m_SearchProxy.currBarbarianLevel = searchJump.level;
                }
                else
                {
                    m_SearchProxy.currCurrLevel = searchJump.level;
                }
            }
            Init();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Interface.m_btn_back_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_iF_SearchRes);
            });
            view.m_btn_add_GameButton.onClick.AddListener(OnBtnAddClick);
            view.m_btn_lower_GameButton.onClick.AddListener(OnBtnLowerClick);
            view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(OnBtnSearchClick);
        }

        protected override void BindUIData()
        {
        }

        private void MovePopPos(float x, SearchType type)
        {
            view.m_pl_Pop.transform.DOLocalMoveX(x, 0);
            m_SearchProxy.searchType = type;
            OnRefreshView();
        }

        private void MovePopPos(float x, SearchType type, int level)
        {
            view.m_pl_Pop.localPosition = new Vector3(x, view.m_pl_Pop.localPosition.y, view.m_pl_Pop.localPosition.z);
            view.m_pl_Pop.gameObject.SetActive(true);
            OnRefreshView();
        }

        private void SetDes(SearchType type)
        {
            SearchData data = m_SearchProxy.GetSearchData((int) type);
            if (data != null)
            {
                view.m_lbl_tip_LanguageText.text = data.des;
                if (type == SearchType.Barbarian)
                {
                    view.m_lbl_MonsterTip_LanguageText.text = m_SearchProxy.currBarbarianLevel > m_playerProxy.CurrentRoleInfo.barbarianLevel + 1 ? LanguageUtils.getTextFormat(500307, m_playerProxy.CurrentRoleInfo.barbarianLevel + 1, LanguageUtils.getText(500200)) : string.Empty;
                    view.m_sd_GameSlider_GameSlider.value = m_SearchProxy.currBarbarianLevel;
                    view.m_lbl_tip_LanguageText.text = LanguageUtils.getText(500302);
                }
                else
                {
                    if (type == SearchType.Farmland)
                    {
                        view.m_lbl_tip_LanguageText.text = LanguageUtils.getText(500303);
                    }
                    else if (type == SearchType.Mill)
                    {
                        view.m_lbl_tip_LanguageText.text = LanguageUtils.getText(500304);
                    }
                    else if (type == SearchType.Stone)
                    {
                        view.m_lbl_tip_LanguageText.text = LanguageUtils.getText(500305);
                    }
                    else if (type == SearchType.Gold)
                    {
                        view.m_lbl_tip_LanguageText.text = LanguageUtils.getText(500306);
                    }
                    view.m_lbl_MonsterTip_LanguageText.text = string.Empty;
                    view.m_sd_GameSlider_GameSlider.value = m_SearchProxy.currCurrLevel;
                }
                view.m_sd_GameSlider_GameSlider.maxValue = type == SearchType.Barbarian ? SearchProxy.MaxBarbarianLevel : SearchProxy.MaxCurrLevel;
            }
        }


    private void Init()
        {
            view.m_sd_GameSlider_GameSlider.minValue = 1;
            view.m_sd_GameSlider_GameSlider.wholeNumbers = true;
            view.m_sd_GameSlider_GameSlider.onValueChanged.AddListener((v) =>
            {
                GameHelper.PlaySoundSlider();
                if (m_SearchProxy.searchType == SearchType.Barbarian)
                {
                    m_SearchProxy.currBarbarianLevel = (int)v;
                }
                else
                {
                    m_SearchProxy.currCurrLevel = (int)v;

                }
                OnRefreshView();
            });
            {
                SearchData data = m_SearchProxy.GetSearchData(0);
                view.m_UI_Item_ResSearchBtn.gameObject.SetActive(true);
                view.m_UI_Item_ResSearchBtn.SetResSearchInfo(data.name);
                view.m_UI_Item_ResSearchBtn.AddClickEvent(() => { MovePopPos(view.m_UI_Item_ResSearchBtn.gameObject.transform.localPosition.x, (SearchType)data.id); });
            }
            {
                SearchData data = m_SearchProxy.GetSearchData(1);
                view.m_UI_Item_ResSearchBtn1.gameObject.SetActive(true);
                view.m_UI_Item_ResSearchBtn1.SetResSearchInfo(data.name);
                int id = data.id * 10000 + 1;
                bool unlock = true;
                ResourceGatherTypeDefine resourceGatherTypeDefine = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(id);
                if (resourceGatherTypeDefine != null)
                {
                    //前置条件是否满足
                    if (resourceGatherTypeDefine.scienceReq > 0 && !m_playerProxy.IsTechnologyUnlockByType(resourceGatherTypeDefine.scienceReq))
                    {
                        unlock = false;

                    }
                }
                if (!unlock)
                {
                    view.m_UI_Item_ResSearchBtn1.SetGray();
                    StudyDefine define = CoreUtils.dataService.QueryRecord<StudyDefine>((int)resourceGatherTypeDefine.scienceLevReq);
                    view.m_UI_Item_ResSearchBtn1.AddClickEvent(() => {
                    Tip.CreateTip(LanguageUtils.getTextFormat(500101,LanguageUtils.getText(define.l_nameID), data.name), Tip.TipStyle.Middle).Show();
                    });
                }
                else
                {
                    view.m_UI_Item_ResSearchBtn1.AddClickEvent(() => { MovePopPos(view.m_UI_Item_ResSearchBtn1.gameObject.transform.localPosition.x, (SearchType)data.id); });
                }
            }
            {
                SearchData data = m_SearchProxy.GetSearchData(2);
                view.m_UI_Item_ResSearchBtn2.gameObject.SetActive(true);
                view.m_UI_Item_ResSearchBtn2.SetResSearchInfo(data.name);
                int id = data.id * 10000 + 1;
                bool unlock = true;
                ResourceGatherTypeDefine resourceGatherTypeDefine = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(id);
                if (resourceGatherTypeDefine != null)
                {
                    //前置条件是否满足
                    if (resourceGatherTypeDefine.scienceReq > 0 && !m_playerProxy.IsTechnologyUnlockByType(resourceGatherTypeDefine.scienceReq))
                    {
                        unlock = false;

                    }
                }
                if (!unlock)
                {
                    view.m_UI_Item_ResSearchBtn2.SetGray();
                    StudyDefine define = CoreUtils.dataService.QueryRecord<StudyDefine>((int)resourceGatherTypeDefine.scienceLevReq);
                    view.m_UI_Item_ResSearchBtn2.AddClickEvent(() => {
                        Tip.CreateTip(LanguageUtils.getTextFormat(500101, LanguageUtils.getText(define.l_nameID), data.name), Tip.TipStyle.Middle).Show();
                    });
                }
                else
                {
                    view.m_UI_Item_ResSearchBtn2.AddClickEvent(() => { MovePopPos(view.m_UI_Item_ResSearchBtn2.gameObject.transform.localPosition.x, (SearchType)data.id); });
                }
            }
            {
                SearchData data = m_SearchProxy.GetSearchData(3);
                view.m_UI_Item_ResSearchBtn3.gameObject.SetActive(true);
                view.m_UI_Item_ResSearchBtn3.SetResSearchInfo(data.name);
                int id = data.id * 10000 + 1;
                bool unlock = true;
                ResourceGatherTypeDefine resourceGatherTypeDefine = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(id);
                if (resourceGatherTypeDefine != null)
                {
                    //前置条件是否满足
                    if (resourceGatherTypeDefine.scienceReq > 0 && !m_playerProxy.IsTechnologyUnlockByType(resourceGatherTypeDefine.scienceReq))
                    {
                        unlock = false;

                    }
                }
                if (!unlock)
                {
                    view.m_UI_Item_ResSearchBtn3.SetGray();
                    StudyDefine define = CoreUtils.dataService.QueryRecord<StudyDefine>((int)resourceGatherTypeDefine.scienceLevReq);
                    view.m_UI_Item_ResSearchBtn3.AddClickEvent(() => {
                        Tip.CreateTip(LanguageUtils.getTextFormat(500101, LanguageUtils.getText(define.l_nameID), data.name), Tip.TipStyle.Middle).Show();
                    });
                }
                else
                {
                    view.m_UI_Item_ResSearchBtn3.AddClickEvent(() => { MovePopPos(view.m_UI_Item_ResSearchBtn3.gameObject.transform.localPosition.x, (SearchType)data.id); });
                }
            }
            {
                SearchData data = m_SearchProxy.GetSearchData(4);
                view.m_UI_Item_ResSearchBtn4.gameObject.SetActive(true);
                view.m_UI_Item_ResSearchBtn4.SetResSearchInfo(data.name);
                int id = data.id * 10000 + 1;
                bool unlock = true;
                ResourceGatherTypeDefine resourceGatherTypeDefine = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(id);
                if (resourceGatherTypeDefine != null)
                {
                    //前置条件是否满足
                    if (resourceGatherTypeDefine.scienceReq > 0 && !m_playerProxy.IsTechnologyUnlockByType(resourceGatherTypeDefine.scienceReq))
                    {
                        unlock = false;

                    }
                }
                if (!unlock)
                {
                    view.m_UI_Item_ResSearchBtn4.SetGray();
                    StudyDefine define = CoreUtils.dataService.QueryRecord<StudyDefine>((int)resourceGatherTypeDefine.scienceLevReq);
                    view.m_UI_Item_ResSearchBtn4.AddClickEvent(() => {
                        Tip.CreateTip(LanguageUtils.getTextFormat(500101, LanguageUtils.getText(define.l_nameID), data.name), Tip.TipStyle.Middle).Show();
                    });
                }
                else
                {
                    view.m_UI_Item_ResSearchBtn4.AddClickEvent(() => { MovePopPos(view.m_UI_Item_ResSearchBtn4.gameObject.transform.localPosition.x, (SearchType)data.id); });
                }
            }

            Timer.Register(0.1f,()=> { if(view!=null && view.m_pl_btns_GridLayoutGroup!=null) InitPos(); });
            OnRefreshView();
            if (view.data is GOScrptGuide)
            {
                FingerTargetParam param = new FingerTargetParam();
                param.AreaTarget =view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.gameObject;
                param.ArrowDirection = (int)EnumArrorDirection.Up;
                CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
            }
        }

        private void InitPos()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                view.m_pl_btns_GridLayoutGroup.GetComponent<RectTransform>());
            if (m_SearchProxy.searchType != SearchType.None)
            {

                MovePopPos(view.m_pl_btns_GridLayoutGroup.transform.GetChild((int)m_SearchProxy.searchType).localPosition.x,
                    m_SearchProxy.searchType, m_SearchProxy.searchType == SearchType.Barbarian? m_SearchProxy .currBarbarianLevel: m_SearchProxy.currCurrLevel);
            }
            switch (m_SearchProxy.searchType)
            {
                case SearchType.Barbarian:
                    {
                        MovePopPos(view.m_UI_Item_ResSearchBtn.m_root_RectTransform.localPosition.x, m_SearchProxy.searchType, m_SearchProxy.currBarbarianLevel);
                    }
                    break;
                case SearchType.Farmland:
                    {
                        MovePopPos(view.m_UI_Item_ResSearchBtn1.m_root_RectTransform.localPosition.x, m_SearchProxy.searchType, m_SearchProxy.currCurrLevel);

                    }
                    break;
                case SearchType.Mill:
                    {
                        MovePopPos(view.m_UI_Item_ResSearchBtn2.m_root_RectTransform.localPosition.x, m_SearchProxy.searchType, m_SearchProxy.currCurrLevel);

                    }
                    break;
                case SearchType.Stone:
                    {
                        MovePopPos(view.m_UI_Item_ResSearchBtn3.m_root_RectTransform.localPosition.x, m_SearchProxy.searchType, m_SearchProxy.currCurrLevel);

                    }
                    break;
                case SearchType.Gold:
                    {
                        MovePopPos(view.m_UI_Item_ResSearchBtn4.m_root_RectTransform.localPosition.x, m_SearchProxy.searchType, m_SearchProxy.currCurrLevel);

                    }
                    break;

            }
        }

        private void OnBtnAddClick()
        {
            if (m_SearchProxy.searchType == SearchType.Barbarian)
            {
                if (m_SearchProxy.currBarbarianLevel < SearchProxy.MaxBarbarianLevel)
                {
                    m_SearchProxy.currBarbarianLevel += 1;
                    view.m_sd_GameSlider_GameSlider.value = m_SearchProxy.currBarbarianLevel;
                }
            }
            else
            {
                if (m_SearchProxy.currCurrLevel < SearchProxy.MaxCurrLevel)
                {
                    m_SearchProxy.currCurrLevel += 1;
                    view.m_sd_GameSlider_GameSlider.value = m_SearchProxy.currCurrLevel;
                }
            }
            OnRefreshView();
        }

        private void OnBtnLowerClick()
        {

            if (m_SearchProxy.searchType == SearchType.Barbarian)
            {
                if (m_SearchProxy.currBarbarianLevel > 1)
                {
                    m_SearchProxy.currBarbarianLevel -= 1;
                    view.m_sd_GameSlider_GameSlider.value = m_SearchProxy.currBarbarianLevel;
                }
            }
            else
            {
                if (m_SearchProxy.currCurrLevel > 1)
                {
                    m_SearchProxy.currCurrLevel -= 1;
                    view.m_sd_GameSlider_GameSlider.value = m_SearchProxy.currCurrLevel;
                }
            }
            OnRefreshView();
        }

        private void OnBtnSearchClick()
        {
            if(GuideManager.Instance.IsGuideFightBarbarian)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.GuideFindMonster);
                CoreUtils.uiManager.CloseUI(UI.s_iF_SearchRes);
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
            bool isCd = m_RssProxy.IsCd();
            if (m_RssProxy.OldLevel == m_SearchProxy.currCurrLevel && m_RssProxy.OldSearchType == m_SearchProxy.searchType && isCd)
            {
                Debug.Log("正在当前cd中");
                m_RssProxy.SearchRssDef();
                CoreUtils.uiManager.CloseUI(UI.s_iF_SearchRes);
                return;
            }

            if (m_SearchProxy.searchType == SearchType.Barbarian)
            {
                m_RssProxy.SendSearchBarbarian(m_SearchProxy.currBarbarianLevel);
            }
            else
            {
                m_RssProxy.SendSearchRss((int)m_SearchProxy.searchType, m_SearchProxy.currCurrLevel);
            }
        }

        private void OnRefreshView()
        {
           
            if (m_SearchProxy.searchType == SearchType.Barbarian)
            {
                view.m_lbl_level_LanguageText.text = LanguageUtils.getTextFormat(300003, m_SearchProxy.currBarbarianLevel);
            }
            else
            {
                view.m_lbl_level_LanguageText.text = LanguageUtils.getTextFormat(300003, m_SearchProxy.currCurrLevel);
            }
            SetDes(m_SearchProxy.searchType);
        }

        #endregion
    }
}