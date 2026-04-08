// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_Win_TalentChangeNameMediator
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

namespace Game {
    
    public class TalentChangeNameData
    {
        public HeroProxy.Hero m_hero;
        public int m_index;

        public TalentChangeNameData(HeroProxy.Hero hero,int index)
        {
            this.m_hero = hero;
            this.m_index = index;
        }
    }
    
    public class UI_Win_TalentChangeNameMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_TalentChangeNameMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_TalentChangeNameMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_TalentChangeNameView view;

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
            
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.m_btn_close_GameButton.onClick.AddListener(CloseUI);
            view.m_btn_sure.m_btn_languageButton_GameButton.onClick.AddListener(OnConfirmEvent);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void CloseUI()
        {
            CoreUtils.uiManager.CloseUI(UI.s_talentChangeNameAlert);
        }

        private void OnConfirmEvent()
        {
            string name = view.m_ipt_name_GameInput.text;
            List<int> LimitLength = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).heroNameLimit;
            int countOfBytes = System.Text.Encoding.Default.GetByteCount(name);
            if (countOfBytes < LimitLength[0] || countOfBytes > LimitLength[1])
            {
                Tip.CreateTip(175026).Show();
                return;
            }
            
            ChangeName(name);
        }

        private void ChangeName(string name)
        {
            TalentChangeNameData data = view.data as TalentChangeNameData;
            if (data != null)
            {
                Hero_ModifyTalentName.request request = new Hero_ModifyTalentName.request();
                request.heroId = data.m_hero.data.heroId;
                request.index = data.m_index;
                request.name = name;
                AppFacade.GetInstance().SendSproto(request);
                CloseUI();
            }
        }
    }
}