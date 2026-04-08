// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月4日
// Update Time         :    2020年6月4日
// Class Description   :    UI_Win_SettingNoticeMediator
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
    public class UI_Win_SettingNoticeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_SettingNoticeMediator";

        private PlayerProxy m_playerProxy;
        
        private Dictionary<string ,GameObject> m_assetDic=new Dictionary<string, GameObject>();
        private List<PushSetting> m_itemInfos = new List<PushSetting>();

        private bool m_isSendingMsg;
        #endregion

        //IMediatorPlug needs
        public UI_Win_SettingNoticeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_SettingNoticeView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_SettingPush.TagName,
                CmdConstant.NoticeSettingChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_SettingPush.TagName:
                    SetSendingMsgStatus(false);
                    break;
                case CmdConstant.NoticeSettingChange:
                    var changeList = notification.Body as Dictionary<long,PushSetting> ;
                    UpdatePushSettingInfo(changeList);
                    SetSendingMsgStatus(false);
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
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            
            ClientUtils.PreLoadRes(view.gameObject,view.m_sv_list_ListView.ItemPrefabDataList,LoadItemFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.setCloseHandle(OnClose);
        }

        protected override void BindUIData()
        {
            SetPushSettingInfo();
        }
       
        #endregion

        private void SetPushSettingInfo()
        {
            m_itemInfos.Clear();
            foreach (var pushSetting in m_playerProxy.CurrentRoleInfo.pushSetting)
            {
                m_itemInfos.Add(pushSetting.Value);
            }
        }

        private void UpdatePushSettingInfo(Dictionary<long,PushSetting> changeList)
        {
            List<int> changeIndexs = new List<int>();
            foreach (var pushSetting in changeList)
            {
                var index = m_itemInfos.FindIndex(x => x.id == pushSetting.Key);
                m_itemInfos[index].open = pushSetting.Value.open;
                changeIndexs.Add(index);
            }

            foreach (var index in changeIndexs)
            {
                view.m_sv_list_ListView.RefreshItem(index);
            }
        }

        private void LoadItemFinish(Dictionary<string,IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitItem;
            view.m_sv_list_ListView.SetInitData(m_assetDic,functab);
            RefreshItemInfo();
        }

        private void InitItem(ListView.ListItem item)
        {
            UI_Item_SettingNoticeItem_SubView itemView = null;

            if (item.data != null)
            {
                itemView = item.data as UI_Item_SettingNoticeItem_SubView;
            }
            else
            {
                itemView = new UI_Item_SettingNoticeItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            
            itemView.SetInfo(m_itemInfos[item.index],OnChangeSetting);
        }

        private void RefreshItemInfo()
        {
            if (m_assetDic.Count <= 0)
            {
                return;
            }
            view.m_sv_list_ListView.FillContent(m_itemInfos.Count);
        }

        private void OnChangeSetting(int ID)
        {
            if (m_isSendingMsg)
            {
                return;
            }
            SetSendingMsgStatus(true);
            Role_SettingPush.request req = new Role_SettingPush.request()
            {
                id = ID
            };
            AppFacade.GetInstance().SendSproto(req);
        }

        private void SetSendingMsgStatus(bool value)
        {
            m_isSendingMsg = value;
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_NoticeSetting);
        }
    }
}