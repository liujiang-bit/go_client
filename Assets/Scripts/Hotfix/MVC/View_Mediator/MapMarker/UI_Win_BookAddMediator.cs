// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月15日
// Update Time         :    2020年9月15日
// Class Description   :    UI_Win_BookAddMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Client.Utils;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using System.Text;

namespace Game {
    public enum MapMarkerOperationType
    {
        Add_Or_Edit_Guild,
        Add_Or_Edit_Person,
        Edit_Guild,
        Edit_Person,
    }

    public enum MapMarkerPageType
    {
        Guild,
        Person,
    }

    public class MapMarkerOperationData
    {
        public MapMarkerOperationType type = MapMarkerOperationType.Add_Or_Edit_Guild;
        public long guildMarkerId = 0;
        public long personMarkerIndex = 0;
        public MapObjectInfoEntity mapObjectInfoEntity = null;
        public float x = 0;
        public float y = 0;
    }

    public class UI_Win_BookAddMediator : GameMediator {
        #region Member

        public static string NameMediator = "UI_Win_BookAddMediator";

        private PlayerProxy m_playerProxy;
        private MapMarkerProxy m_mapMarkerProxy;
        private ChatProxy m_chatProxy;

        private MapMarkerOperationData m_OpenPanelData;
        private UI_Model_SideToggle_SubView[] m_pageBtnArray = new UI_Model_SideToggle_SubView[2];
        private MapMarkerOperationType m_currentOperationType = MapMarkerOperationType.Add_Or_Edit_Guild;
        private MapMarkerPageType m_currentPageType = MapMarkerPageType.Person;
        private Dictionary<long, UI_Item_SignType_SubView> m_GuildMapMarkerViewDic = new Dictionary<long, UI_Item_SignType_SubView>();
        private Dictionary<long, UI_Item_BookType_SubView> m_PersonMapMarkerViewDic = new Dictionary<long, UI_Item_BookType_SubView>();
        private long m_CurrentMarkerId = 0;
        private MapMarkerInfo m_CurrentMapMarkerInfo = null;
        private string m_mapMarkerDescription;
        private bool m_editFlag = false;

        #endregion

        //IMediatorPlug needs
        public UI_Win_BookAddMediator(object viewComponent ):base(NameMediator, viewComponent ) {}

        public UI_Win_BookAddView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.GuildMapMarkerInfoChanged,
                CmdConstant.GuildMapMarkerInfoCleared
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.GuildMapMarkerInfoChanged:
                    RefreshView();
                    break;
                case CmdConstant.GuildMapMarkerInfoCleared:
                    if (m_currentOperationType == MapMarkerOperationType.Add_Or_Edit_Guild || m_currentOperationType == MapMarkerOperationType.Edit_Guild)
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_MapMarkerOperation);
                    }
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
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;
            m_chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.setCloseHandle(OnClose);
            view.m_ipt_enter_GameInput.onValueChanged.AddListener(onDescValueChanged);
            view.m_ipt_enter_GameInput.onEndEdit.AddListener(onDescEndEdit);
            view.m_btn_sure.m_btn_languageButton_GameButton.onClick.AddListener(OnConfirmBtnClick);
            view.m_btn_delete_GameButton.onClick.AddListener(OnDeleteBtnClick);
        }

        protected override void BindUIData()
        {
            m_OpenPanelData = view.data as MapMarkerOperationData;

            if (m_OpenPanelData == null)
            {
                Debug.LogError("MapMarkerOperationView Data Error.");
                return;
            }

            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            view.m_ipt_enter_GameInput.characterLimit = configDefine.mapMarkerNameLimit;

            m_currentOperationType = m_OpenPanelData.type;

            if (m_currentOperationType == MapMarkerOperationType.Add_Or_Edit_Guild)
            {
                view.m_pl_page_GridLayoutGroup.gameObject.SetActive(true);
                m_currentPageType = MapMarkerPageType.Guild;
            }
            else if (m_currentOperationType == MapMarkerOperationType.Add_Or_Edit_Person)
            {
                view.m_pl_page_GridLayoutGroup.gameObject.SetActive(false);
                m_currentPageType = MapMarkerPageType.Person;
            }
            else if (m_currentOperationType == MapMarkerOperationType.Edit_Guild)
            {
                view.m_pl_page_GridLayoutGroup.gameObject.SetActive(false);
                m_currentPageType = MapMarkerPageType.Guild;
            }
            else if (m_currentOperationType == MapMarkerOperationType.Edit_Person)
            {
                view.m_pl_page_GridLayoutGroup.gameObject.SetActive(false);
                m_currentPageType = MapMarkerPageType.Person;
            }
            else
            {
                Debug.LogError("MapMarkerOperationView MapMarkerOperationType Error.");
                return;
            }

            InitView();
        }

        #endregion

        private void InitView()
        {
            //联盟
            m_pageBtnArray[0] = view.m_pl_side1;
            //个人
            m_pageBtnArray[1] = view.m_pl_side2;

            for (int i = 0; i < m_pageBtnArray.Length; i++)
            {
                int pageIndex = i;
                m_pageBtnArray[pageIndex].m_ck_ck_GameToggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        if ((MapMarkerPageType)pageIndex != m_currentPageType)
                        {
                            SwitchPage((MapMarkerPageType)pageIndex);
                        }
                    }
                });

                m_pageBtnArray[pageIndex].m_ck_ck_GameToggle.isOn = ((MapMarkerPageType)pageIndex == m_currentPageType);
            }

            view.m_UI_Item_SignType.gameObject.SetActive(false);
            List<MapMarkerTypeDefine> mapMarkerTypeDefineList = CoreUtils.dataService.QueryRecords<MapMarkerTypeDefine>();
            foreach (var mapMarkerTypeDefine in mapMarkerTypeDefineList)
            {
                if (mapMarkerTypeDefine.type == 2)
                {
                    GameObject go = CoreUtils.assetService.Instantiate(view.m_UI_Item_SignType.gameObject);
                    go.SetActive(true);
                    go.transform.SetParent(view.m_pl_sign_GridLayoutGroup.transform);
                    go.transform.localScale = Vector3.one;

                    UI_Item_SignType_SubView subView = new UI_Item_SignType_SubView(go.GetComponent<RectTransform>());
                    subView.setClickCallback(OnGuildMapMarkerClick);
                    subView.Init(mapMarkerTypeDefine.ID);
                    m_GuildMapMarkerViewDic.Add(mapMarkerTypeDefine.ID, subView);
                }
            }

            view.m_UI_Item_BookType3.setClickCallback(OnPersonMapMarkerClick);
            view.m_UI_Item_BookType3.Init(1000);
            m_PersonMapMarkerViewDic.Add(1000, view.m_UI_Item_BookType3);

            view.m_UI_Item_BookType2.setClickCallback(OnPersonMapMarkerClick);
            view.m_UI_Item_BookType2.Init(1001);
            m_PersonMapMarkerViewDic.Add(1001, view.m_UI_Item_BookType2);

            view.m_UI_Item_BookType1.setClickCallback(OnPersonMapMarkerClick);
            view.m_UI_Item_BookType1.Init(1002);
            m_PersonMapMarkerViewDic.Add(1002, view.m_UI_Item_BookType1);

            SwitchPage(m_currentPageType);
        }

        private void SwitchPage(MapMarkerPageType pageType)
        {
            m_currentPageType = pageType;

            RefreshView();
        }

        private void RefreshView()
        {            
            view.m_pl_sign_GridLayoutGroup.gameObject.SetActive(false);
            view.m_pl_book_GridLayoutGroup.gameObject.SetActive(false);

            if (m_currentPageType == MapMarkerPageType.Guild)
            {
                view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(910009));

                view.m_pl_sign_GridLayoutGroup.gameObject.SetActive(true);                

                m_CurrentMarkerId = 2000;
                m_CurrentMapMarkerInfo = null;

                if (m_OpenPanelData.x > 0 && m_OpenPanelData.y > 0)
                {
                    if (m_mapMarkerProxy.IsGuildContainsByPos(m_OpenPanelData.x, m_OpenPanelData.y, out m_CurrentMapMarkerInfo))
                    {
                        m_CurrentMarkerId = m_CurrentMapMarkerInfo.markerId;
                    }
                }

                if (m_OpenPanelData.guildMarkerId != 0)
                {
                    m_CurrentMapMarkerInfo = m_mapMarkerProxy.GetGuildMapMarkerInfo(m_OpenPanelData.guildMarkerId);
                    m_CurrentMarkerId = m_CurrentMapMarkerInfo.markerId;
                }

                if (m_CurrentMapMarkerInfo != null)
                {
                    view.m_lbl_coordinate_LanguageText.text = LanguageUtils.getTextFormat(910010, m_playerProxy.GetGameNode().ToString("N0"), (int)(m_CurrentMapMarkerInfo.pos.x / 600), (int)(m_CurrentMapMarkerInfo.pos.y / 600));
                    view.m_ipt_enter_GameInput.text = m_CurrentMapMarkerInfo.description;
                    view.m_btn_delete_GameButton.gameObject.SetActive(true);

                    m_mapMarkerDescription = view.m_ipt_enter_GameInput.text;

                    m_editFlag = true;
                }
                else
                {
                    view.m_lbl_coordinate_LanguageText.text = LanguageUtils.getTextFormat(910010, m_playerProxy.GetGameNode().ToString("N0"), (int)(m_OpenPanelData.x / 600), (int)(m_OpenPanelData.y / 600));
                    //联盟标记文本编辑框不做默认输入
                    view.m_btn_delete_GameButton.gameObject.SetActive(false);

                    m_mapMarkerDescription = view.m_ipt_enter_GameInput.text;

                    m_editFlag = false;
                }

                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)m_CurrentMarkerId);
                if (mapMarkerTypeDefine != null)
                {
                    view.m_lbl_choosetype_LanguageText.text = LanguageUtils.getText(mapMarkerTypeDefine.l_nameId);
                }

                foreach (var guildMapMarkerView in m_GuildMapMarkerViewDic)
                {
                    guildMapMarkerView.Value.SetUseState(m_mapMarkerProxy.IsGuildContainsByMarkerId(guildMapMarkerView.Key));
                }

                foreach (var guildMapMarkerView in m_GuildMapMarkerViewDic.Values)
                {
                    guildMapMarkerView.SetSelectState(false);
                }
                UI_Item_SignType_SubView mapMarkerView;
                if (m_GuildMapMarkerViewDic.TryGetValue(m_CurrentMarkerId, out mapMarkerView))
                {
                    mapMarkerView.SetSelectState(true);
                }
            }
            else if (m_currentPageType == MapMarkerPageType.Person)
            {
                view.m_pl_book_GridLayoutGroup.gameObject.SetActive(true);

                m_CurrentMarkerId = 1000;
                m_CurrentMapMarkerInfo = null;

                if (m_OpenPanelData.x > 0 && m_OpenPanelData.y > 0)
                {
                    if (m_mapMarkerProxy.IsPersonContainsByPos(m_OpenPanelData.x, m_OpenPanelData.y, out m_CurrentMapMarkerInfo))
                    {
                        m_CurrentMarkerId = m_CurrentMapMarkerInfo.markerId;
                    }
                }

                if (m_OpenPanelData.personMarkerIndex != 0)
                {
                    m_CurrentMapMarkerInfo = m_mapMarkerProxy.GetPersonMapMarkerInfo(m_OpenPanelData.personMarkerIndex);
                    m_CurrentMarkerId = m_CurrentMapMarkerInfo.markerId;
                }

                if (m_CurrentMapMarkerInfo != null)
                {
                    view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(910014));

                    view.m_lbl_coordinate_LanguageText.text = LanguageUtils.getTextFormat(910010, m_playerProxy.GetGameNode().ToString("N0"), (int)(m_CurrentMapMarkerInfo.pos.x / 600), (int)(m_CurrentMapMarkerInfo.pos.y / 600));
                    view.m_ipt_enter_GameInput.text = m_CurrentMapMarkerInfo.description;
                    view.m_btn_delete_GameButton.gameObject.SetActive(true);

                    m_mapMarkerDescription = view.m_ipt_enter_GameInput.text;

                    m_editFlag = true;
                }
                else
                {
                    view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(910013));

                    view.m_lbl_coordinate_LanguageText.text = LanguageUtils.getTextFormat(910010, m_playerProxy.GetGameNode().ToString("N0"), (int)(m_OpenPanelData.x / 600), (int)(m_OpenPanelData.y / 600));
                    view.m_ipt_enter_GameInput.text = m_mapMarkerProxy.GetDefaultDescription(m_OpenPanelData.mapObjectInfoEntity);
                    view.m_btn_delete_GameButton.gameObject.SetActive(false);

                    m_mapMarkerDescription = view.m_ipt_enter_GameInput.text;

                    m_editFlag = false;
                }

                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)m_CurrentMarkerId);
                if (mapMarkerTypeDefine != null)
                {
                    view.m_lbl_choosetype_LanguageText.text = LanguageUtils.getText(mapMarkerTypeDefine.l_nameId);
                }

                foreach (var personMapMarkerView in m_PersonMapMarkerViewDic.Values)
                {
                    personMapMarkerView.SetSelectState(false);
                }
                UI_Item_BookType_SubView mapMarkerView;
                if (m_PersonMapMarkerViewDic.TryGetValue(m_CurrentMarkerId, out mapMarkerView))
                {
                    mapMarkerView.SetSelectState(true);
                }
            }
            else
            {
                Debug.LogError("MapMarkerOperationView MapMarkerPageType Error.");
            }
        }

        private void OnGuildMapMarkerClick(long markerId)
        {
            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)markerId);
            if (mapMarkerTypeDefine == null)
            {
                return;
            }

            view.m_lbl_choosetype_LanguageText.text = LanguageUtils.getText(mapMarkerTypeDefine.l_nameId);

            foreach (var guildMapMarkerView in m_GuildMapMarkerViewDic.Values)
            {
                guildMapMarkerView.SetSelectState(false);
            }
            UI_Item_SignType_SubView mapMarkerView;
            if (m_GuildMapMarkerViewDic.TryGetValue(markerId, out mapMarkerView))
            {
                mapMarkerView.SetSelectState(true);
            }

            m_CurrentMarkerId = markerId;
        }

        private void OnPersonMapMarkerClick(long markerId)
        {
            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)markerId);
            if (mapMarkerTypeDefine == null)
            {
                return;
            }

            view.m_lbl_choosetype_LanguageText.text = LanguageUtils.getText(mapMarkerTypeDefine.l_nameId);

            foreach (var personMapMarkerView in m_PersonMapMarkerViewDic.Values)
            {
                personMapMarkerView.SetSelectState(false);
            }
            UI_Item_BookType_SubView mapMarkerView;
            if (m_PersonMapMarkerViewDic.TryGetValue(markerId, out mapMarkerView))
            {
                mapMarkerView.SetSelectState(true);
            }

            m_CurrentMarkerId = markerId;
        }

        private void onDescValueChanged(string text)
        {
            m_mapMarkerDescription = text;
        }

        private void onDescEndEdit(string text)
        {
           m_mapMarkerDescription = text;
        }

        private void OnConfirmBtnClick()
        {
            //if (BannedWord.ChackHasBannedWord(m_mapMarkerDescription))
            //{
            //    Tip.CreateTip(300128).SetStyle(Tip.TipStyle.Middle).Show();
            //    return;
            //}
            //替换脏字
            Client.Utils.BannedWord.CheckChatBannedWord(m_mapMarkerDescription);
            if (m_currentPageType == MapMarkerPageType.Guild)
            {
                if (m_editFlag)
                {
                    long oldMarkerId = 0;
                    if (m_CurrentMarkerId != m_CurrentMapMarkerInfo.markerId)
                    {
                        oldMarkerId = m_CurrentMapMarkerInfo.markerId;
                    }
                    m_mapMarkerProxy.EditGuildMapMarker(m_CurrentMarkerId, m_mapMarkerDescription, m_CurrentMapMarkerInfo.pos.x, m_CurrentMapMarkerInfo.pos.y, oldMarkerId);
                }
                else
                {
                    m_mapMarkerProxy.AddPersonOrGuildMapMarker(m_CurrentMarkerId, m_mapMarkerDescription, m_OpenPanelData.x, m_OpenPanelData.y); 
                }

                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)m_CurrentMarkerId);
                if (mapMarkerTypeDefine != null)
                {
                    if (mapMarkerTypeDefine.chatMessage != 0)
                    {
                        if (m_chatProxy.CheckAliance(m_chatProxy.AllianceContact))
                        {
                            if (m_chatProxy.CheckMemberJurisdiction(m_chatProxy.AllianceContact))
                            {
                                if (m_chatProxy.CheckSilence())
                                {
                                    if (m_chatProxy.CheckChannelInterva(m_chatProxy.AllianceContact))
                                    {
                                        if (m_chatProxy.CheckLvLimit(m_mapMarkerDescription, m_chatProxy.AllianceContact))
                                        {
                                            if (m_chatProxy.CheckMyMsgCount(m_mapMarkerDescription, m_chatProxy.AllianceContact, EnumMsgType.ATUser))
                                            {
                                                m_chatProxy.SendMsgMapMarkerType(m_mapMarkerDescription, mapMarkerTypeDefine.ID, m_chatProxy.AllianceContact);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (m_currentPageType == MapMarkerPageType.Person)
            {
                if (m_editFlag)
                {
                    m_mapMarkerProxy.EditPersonMapMarker(m_CurrentMapMarkerInfo.markerIndex, m_CurrentMarkerId, m_mapMarkerDescription);
                }
                else
                {
                    ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                    if (m_mapMarkerProxy.CalPersonMapMarkerInfoNum() < configDefine.personMarkerLimit)
                    {
                        m_mapMarkerProxy.AddPersonOrGuildMapMarker(m_CurrentMarkerId, m_mapMarkerDescription, m_OpenPanelData.x, m_OpenPanelData.y);
                    }
                    else
                    {
                        Tip.CreateTip(910001).Show();
                    }
                }
            }
            else
            {
                Debug.LogError("MapMarkerOperationView MapMarkerPageType Error.");
            }

            CoreUtils.uiManager.CloseUI(UI.s_MapMarkerOperation);
        }

        private void OnDeleteBtnClick()
        {
            m_mapMarkerProxy.DeleteMapMarker(m_CurrentMapMarkerInfo.markerIndex, m_CurrentMapMarkerInfo.markerId);

            CoreUtils.uiManager.CloseUI(UI.s_MapMarkerOperation);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_MapMarkerOperation);
        }
    }
}