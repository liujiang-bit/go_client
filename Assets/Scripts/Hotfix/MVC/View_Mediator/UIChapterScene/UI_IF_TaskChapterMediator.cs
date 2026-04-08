// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月10日
// Update Time         :    2020年8月10日
// Class Description   :    UI_IF_TaskChapterMediator
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

namespace Game {
    public class UI_IF_TaskChapterMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_TaskChapterMediator";
        TaskChapterDataDefine taskChapterDataDefine = null;
        #endregion

        //IMediatorPlug needs
        public UI_IF_TaskChapterMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_TaskChapterView view;

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
            taskChapterDataDefine = view.data as TaskChapterDataDefine;
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            ClientUtils.LoadSprite(view.m_img_chapter_PolygonImage, taskChapterDataDefine.prebg);
            view.m_lbl_text_LanguageText.text = LanguageUtils.getText( taskChapterDataDefine.preText);
            CoreUtils.audioService.PlayOneShot(taskChapterDataDefine.preSound);
            Animator animator = view.m_pl_view_Animator;
            AnimationClip[] clips = view.m_pl_view_Animator.runtimeAnimatorController.animationClips;
            if (clips.Length < 1)
            {
                return;
            }
            Timer.Register(clips[0].length, () => {
                PlayerProxy m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                if (m_playerProxy.CurrentRoleInfo != null)
                {
                    TaskChapterDataDefine taskChapterDataDefine = CoreUtils.dataService.QueryRecord<TaskChapterDataDefine>((int)m_playerProxy.CurrentRoleInfo.chapterId);
                    int m_country = (int)m_playerProxy.GetCivilization() / 100;
                    CoreUtils.uiManager.CloseUI(UI.s_chapterScene);
                    if (taskChapterDataDefine != null)
                    {
                        if (taskChapterDataDefine.dialogBegin.Count >= m_country)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.ShowChapterDiaglog, taskChapterDataDefine.dialogBegin[m_country - 1]);
                        }
                        else
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.OnChapterDiaglogEnd, taskChapterDataDefine.dialogBegin[m_country - 1]);
                        }
                    }
                }
            },null,false,false,view.vb);
        }
       
        #endregion
    }
}