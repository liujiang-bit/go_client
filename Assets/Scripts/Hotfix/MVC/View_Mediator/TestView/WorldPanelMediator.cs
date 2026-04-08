// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年11月19日
// Update Time         :    2019年11月19日
// Class Description   :    WorldPanelMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using PureMVC.Interfaces;
using Client;

namespace Game {
    public class WorldPanelMediator : GameMediator {
        #region Member
        public static string NameMediator = "WorldPanelMediator";


        #endregion

        //IMediatorPlug needs
        public WorldPanelMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public WorldPanelView view;

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

            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
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
            view.m_m_btn_home_Button.onClick.AddListener(() =>
            {
               // WorldCamera.Instance().ViewTerrainPos(3600, 3600, 1000, null);
                WorldCamera.Instance().SetCameraDxf(250, 1000, null);
            });
            view.m_m_btn_test1_Button.onClick.AddListener(() =>
            {
                int num = Random.Range(3, 6);
                for (int i = 0; i < num; i++)
                {
                    Timer.Register(UnityEngine.Random.Range(0f, 2f), ()=>
                    {
                        float new_direction_intensity = UnityEngine.Random.Range(0.1f, 2.0f);
                        ClientUtils.lightingManager.UpdateThunderLighting(Color.white, Color.white, new_direction_intensity, 0.2f, 0.2f, 0.0f);
                    });
                }
            });
            WorldCamera.Instance().AddViewChange(OnWorldViewChange);
        }

        void PlayThunder()
        {
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void OnWorldViewChange(float x, float y, float dxf)
        {
            view.m_m_WorldPos_Text.text = $"X:{Mathf.FloorToInt(x)/6}Y:{Mathf.FloorToInt(y)/6}";
        }
    }
}