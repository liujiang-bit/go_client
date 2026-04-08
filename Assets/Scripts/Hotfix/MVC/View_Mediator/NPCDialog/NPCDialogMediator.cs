// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月6日
// Update Time         :    2020年2月6日
// Class Description   :    NPCDialogMediator
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
using Hotfix;
using System;
using System.IO;

namespace Game {
    public class NPCDialogMediator : GameMediator {
        #region Member
        public static string NameMediator = "NPCDialogMediator";

        private float TalkTextMaxWidth = 400f;

        private List<GuideDialogDefine> m_defines = new List<GuideDialogDefine>();
        private int m_currentGroup;
        private int m_currentDialogCount = 0;

        private int m_lastPos = 0;
        private string m_lastModel = string.Empty;
        private AudioHandler m_tmpAudioHandler;

        private GlobalFilmMediator m_filmMediator;

        PlayerProxy m_playerProxy;
        CityBuildingProxy m_buildingProxy;

        private bool bDispose;


        private string m_rightOnAni = "Right_On";
        private string m_rightOffAni = "Right_Off";
        private string m_leftOnAni = "Left_On";
        private string m_leftOffAni = "Left_Off";
        #endregion

        //IMediatorPlug needs
        public NPCDialogMediator(object viewComponent ):base(NameMediator, viewComponent ) {
            this.IsOpenUpdate = true;
        }


        public NPCDialogView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ShowNPCDiaglog,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ShowNPCDiaglog:
                    m_currentGroup = (int)notification.Body;
                    Prepare();
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
            if (m_tmpAudioHandler != null)
            {
                CoreUtils.audioService.StopByHandler(m_tmpAudioHandler);
            }
            bDispose = true;
        }

        public override void OnRemove()
        {
            //对话结束可以显示模块信息
            AppFacade.GetInstance().SendNotification(CmdConstant.OnNPCDiaglogEnd, m_currentGroup);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {

        }        

        protected override void InitData()
        {
            m_filmMediator = AppFacade.GetInstance().RetrieveMediator(GlobalFilmMediator.NameMediator) as GlobalFilmMediator;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_btn_noTextButton_GameButton.SetMute();
            m_currentGroup = (int)view.data;
            Prepare();
        }
       
        #endregion

        private void Prepare()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.HideGuideMask);
            m_defines = CoreUtils.dataService.QueryRecords<GuideDialogDefine>().FindAll((i)=>
            {
                return i.group == m_currentGroup;
            });

            m_defines.Sort((r1,r2)=> { return r1.step.CompareTo(r2.step); });
            m_currentDialogCount = m_defines.Count;
            if(m_currentDialogCount<=0)
            {
                CoreUtils.logService.Error($"NPC剧情对话{m_currentDialogCount}条");
                //AppFacade.GetInstance().SendNotification(CmdConstant.OnNPCDiaglogEnd, m_currentGroup);
                Timer.Register(1f,()=>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_NPCDialog);
                });
                return;
            }
            CoreUtils.logService.Info($"NPC剧情对话{m_currentDialogCount}条");
            ProcessFilmDialog();
        }

        private void ProcessFilmDialog()
        {
            if (bDispose)
            {
                return;
            }
            view.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
            if (m_tmpAudioHandler != null)
            {
                CoreUtils.audioService.StopByHandler(m_tmpAudioHandler);
            }

            if (m_currentDialogCount <= 0)
            {
                if (view.m_pl_right_Animator.gameObject.activeSelf)
                {
                    view.m_pl_right_Animator.Play(m_rightOffAni);
                }
                if (view.m_pl_left_Animator.gameObject.activeSelf)
                {
                    view.m_pl_left_Animator.Play(m_leftOffAni);
                }
                Timer.Register(0.2f, () =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_NPCDialog);
                });
                return;
            }

            GuideDialogDefine define = m_defines[m_defines.Count - m_currentDialogCount];
            if (define.delay == 1)//第二段动画开始
            {
                if (view.m_pl_right_Animator.gameObject.activeSelf)
                {
                    view.m_pl_right_Animator.Play(m_rightOffAni);
                }
                if (view.m_pl_left_Animator.gameObject.activeSelf)
                {
                    view.m_pl_left_Animator.Play(m_leftOffAni);
                }
                m_filmMediator.InitFilm2();
                Timer.Register(2f,PlayDialog);
                return;
            }
            else if(define.delay == 2) //等野蛮人攻击村庄结束触发该对话
            {
                m_lastPos = 0;
                m_filmMediator.SetCitizenFilmCallback(PlayDialog);
                if (m_filmMediator.film2== null)
                {
                    CoreUtils.uiManager.CloseUI(UI.s_NPCDialog);
                    Debug.LogError("野蛮人动画为空?");
                }
                return;
            }
            PlayDialog();
        }

        private void PlayDialog()
        {
            if(bDispose)
            {
                return;
            }
            if (m_currentDialogCount <= 0)
            {
                view.m_pl_right_Animator.Play(m_rightOffAni);
                view.m_pl_left_Animator.Play(m_leftOffAni);
                Timer.Register(0.2f, () =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_NPCDialog);
                });
                return;
            }
            view.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
            GuideDialogDefine define = m_defines[m_defines.Count - m_currentDialogCount];
            m_currentDialogCount--;
            UI_Pop_TalkTip_SubView talkView;
            Spine.Unity.SkeletonGraphic role;

            //发送新手剧情日志
            Role_GuideDialog.request req = new Role_GuideDialog.request();
            req.plotId = define.ID;
            AppFacade.GetInstance().SendSproto(req);
            SendClientDeviceInfoMedia.ReportData();

            if (define.modelPos == "1")
            {
                talkView = view.m_UI_Pop_TalkTip_left;
                role = view.m_spine_hero_left_SkeletonGraphic;
            }
            else
            {
                talkView = view.m_UI_Pop_TalkTip_right;
                role = view.m_spine_hero_right_SkeletonGraphic;
            }

            string currentModel = string.Empty;
            if (define.model == "1")
            {
                currentModel = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_playerProxy.CurrentRoleInfo.country).heroRes;
            }
            else if (define.npcAgeModel != null && define.npcAgeModel.Count >= (int)m_buildingProxy.GetAgeType())
            {
                currentModel = define.npcAgeModel[(int)m_buildingProxy.GetAgeType()-1];
            }
            else
            {
                currentModel = define.model;
            }

            if (currentModel == m_lastModel)
            {
                LoadModelCallback(define);
            }
            else
            {
                ClientUtils.LoadSpine(role, currentModel, () =>
                {
                    LoadModelCallback(define);
                });
            }
            m_lastModel = currentModel;
            role.initialFlipX = define.modelMirror == 1;
            talkView.m_lbl_text_LanguageText.text = LanguageUtils.getText(define.l_dialogText);
            talkView.UpdateBgSize(TalkTextMaxWidth);

            if (define.civilization != null&&define.civilization.Count>0)
            {
                int civilizationIndex = define.civilization.FindIndex((i)=> { return i == m_playerProxy.CurrentRoleInfo.country; });
                if(civilizationIndex>=0&&define.heroSound.Count> civilizationIndex)
                {
                    string sound = define.heroSound[civilizationIndex];
                    if(!string.IsNullOrEmpty(sound))
                    {
                        CoreUtils.audioService.PlayOneShot(sound, (ad) =>
                        {
                            if (ad != null)
                            {
                                m_tmpAudioHandler = ad;
                            }
                            if(bDispose)
                            {
                                CoreUtils.audioService.StopByHandler(m_tmpAudioHandler);
                            }
                        });
                    }
                }
            }
            else
            {
                if (define.languageSet != null && define.languageSet.Count > 0)
                {
                    int language = (int)LanguageUtils.GetLanguage();
                    int findIndex = -1;
                    for (int i = 0; i < define.languageSet.Count; i++)
                    {
                        if (define.languageSet[i] == language)
                        {
                            findIndex = i;
                            break;
                        }
                    }
                    if (findIndex > -1)
                    {
                        if (define.soundClient != null && findIndex < define.soundClient.Count)
                        {
                            CoreUtils.audioService.PlayOneShot(define.soundClient[findIndex], (ad) =>
                            {
                                if (ad != null)
                                {
                                    m_tmpAudioHandler = ad;
                                    if (bDispose)
                                    {
                                        CoreUtils.audioService.StopByHandler(m_tmpAudioHandler);
                                    }
                                }
                            });
                        }
                    }
                } 
            }
        }

        private void LoadModelCallback(GuideDialogDefine define)
        {
            int currentPos = define.modelPos == "1" ? 1 : 2;
            if (define.modelPos == "1")
            {
                if (view.m_pl_right_Animator.gameObject.activeSelf)
                    view.m_pl_right_Animator.Play(m_rightOffAni);
                view.m_pl_left_Animator.gameObject.SetActive(true);
                if (m_lastPos!= currentPos)
                {
                    view.m_pl_left_Animator.Play(m_leftOnAni, -1, 0);
                }
            }
            else
            {
                if (view.m_pl_left_Animator.gameObject.activeSelf)
                    view.m_pl_left_Animator.Play(m_leftOffAni);
                view.m_pl_right_Animator.gameObject.SetActive(true);
                if (m_lastPos != currentPos)
                {
                    view.m_pl_right_Animator.Play(m_rightOnAni,-1,0);
                }
            }
            view.m_btn_noTextButton_GameButton.onClick.AddListener(()=>
            {
                if (currentPos != m_lastPos)
                {
                    if (m_lastPos == 1)
                    {
                        if (view.m_pl_left_Animator.gameObject.activeSelf)
                            view.m_pl_left_Animator.Play(m_leftOffAni);
                    }
                    else
                    {
                        if(view.m_pl_right_Animator.gameObject.activeSelf)
                        view.m_pl_right_Animator.Play(m_rightOffAni);
                    }
                    m_lastPos = currentPos;
                }
                if (define.dialogEvent == 1)
                {
                    if (view.m_pl_left_Animator.gameObject.activeSelf)
                    {
                        view.m_pl_left_Animator.Play(m_leftOffAni);
                    }
                    if(view.m_pl_right_Animator.gameObject.activeSelf)
                    {
                        view.m_pl_right_Animator.Play(m_rightOffAni);
                    }
                    m_filmMediator.InitFilm1(ProcessFilmDialog);
                    view.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
                    return;
                }
                else if(define.dialogEvent ==2)
                {
                    m_filmMediator.QuitFilm2(ProcessFilmDialog);
                }
                ProcessFilmDialog();
            });
        }

    }
}