// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年4月15日
// Update Time         :    2020年4月15日
// Class Description   :    TavernBoxDescMediator 酒馆宝箱奖励概率列表
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
using System;

namespace Game {
    public class TavernBoxDescMediator : GameMediator {
        #region Member
        public static string NameMediator = "TavernBoxDescMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<TavernProbabilityDefine> m_dataList;

        #endregion

        //IMediatorPlug needs
        public TavernBoxDescMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public TavernBoxDescView view;

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
            int type = (int)view.data;
            if (type == 1)
            {
                view.m_UI_Model_Window_Type2.m_lbl_title_LanguageText.text = LanguageUtils.getText(760030);
            }
            else
            {
                view.m_UI_Model_Window_Type2.m_lbl_title_LanguageText.text = LanguageUtils.getText(760031);
            }

            m_dataList = new List<TavernProbabilityDefine>();
            int index = type * 1000;
            for (int i = 0; i < 1000; i++)
            {
                TavernProbabilityDefine define = CoreUtils.dataService.QueryRecord<TavernProbabilityDefine>(index + i);
                if (define == null)
                {
                    break;
                }
                else
                {
                    m_dataList.Add(define);
                }
            }

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(Close);
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

            InitList();
        }

        private void InitList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_ListView.FillContent(m_dataList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            UI_Item_TavernBoxDescView itemView;
            if (listItem.data == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_TavernBoxDescView>(listItem.go);
            }
            else
            {
                itemView = listItem.data as UI_Item_TavernBoxDescView;
            }

            TavernProbabilityDefine define = m_dataList[listItem.index];
            itemView.m_lbl_lbl1_LanguageText.text = LanguageUtils.getText(define.l_nameID);
            float f1 = Convert.ToSingle(define.probability, System.Globalization.CultureInfo.InvariantCulture);
            itemView.m_lbl_lbl2_LanguageText.text = LanguageUtils.getTextFormat(180357, f1 * 100);
            
        }

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_tavernBoxDesc);
        }
    }
}