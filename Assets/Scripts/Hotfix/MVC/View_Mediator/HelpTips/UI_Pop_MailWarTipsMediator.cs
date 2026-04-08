// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月30日
// Update Time         :    2020年6月30日
// Class Description   :    UI_Pop_MailWarTipsMediator 邮件 战斗报告 增援
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
using UnityEngine.UI;

namespace Game {
    public class FightReportReinforceTipData
    {
        public List<FightReportReinforceData> DataList;
        public Vector3 WorldPos;
    }

    public class UI_Pop_MailWarTipsMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_MailWarTipsMediator";

        private Dictionary<string, GameObject> m_assetDic;
        private List<FightReportReinforceData> m_dataList;
        private Vector3 m_worldPos;

        private float m_itemHeight;

        #endregion

        //IMediatorPlug needs
        public UI_Pop_MailWarTipsMediator(object viewComponent ):base(NameMediator, viewComponent ) {}

        
        public UI_Pop_MailWarTipsView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            //适配tip位置
            UIHelper.CalcPopupPos2(view.gameObject.transform as RectTransform,
                    view.m_pl_pos.GetComponent<RectTransform>(),
                    view.m_pl_content_Animator.GetComponent<RectTransform>(),
                    m_worldPos,
                    view.m_img_arrowSideL_PolygonImage.gameObject, view.m_img_arrowSideR_PolygonImage.gameObject,
                    view.m_img_arrowSideTop_PolygonImage.gameObject, view.m_img_arrowSideButtom_PolygonImage.gameObject,
                    10f, 4, true);
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
            FightReportReinforceTipData param = view.data as FightReportReinforceTipData;
            m_dataList = param.DataList;
            m_worldPos = param.WorldPos;

            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, OnLoadFinish);
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assetDic = asset;
            m_itemHeight = m_assetDic["UI_Item_MailWarTips"].GetComponent<RectTransform>().rect.height;

            RectTransform rect = view.m_pl_content_Animator.GetComponent<RectTransform>();

            if (m_dataList.Count > 1)
            {
                rect.sizeDelta = new Vector2(rect.rect.width, rect.rect.height + m_itemHeight * 0.5f);
            }
            else
            {
                view.m_sv_list_ListView.gameObject.GetComponent<ScrollRect>().vertical = false;
            }

            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_ListView.FillContent(m_dataList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            FightReportReinforceData itemData = m_dataList[listItem.index];
            if (listItem.data == null)
            {
                UI_Item_MailWarTips_SubView subView = new UI_Item_MailWarTips_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
                subView.Refresh(itemData);
            } else
            {
                UI_Item_MailWarTips_SubView subView = listItem.data as UI_Item_MailWarTips_SubView;
                subView.Refresh(itemData);
            }     
        }
    }
}