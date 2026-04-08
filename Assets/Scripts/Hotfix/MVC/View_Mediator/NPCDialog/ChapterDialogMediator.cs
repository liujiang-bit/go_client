// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月4日
// Update Time         :    2020年3月4日
// Class Description   :    ChapterDialogMediator
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
    public class ChapterDialogMediator : GameMediator {
        #region Member
        public static string NameMediator = "ChapterDialogMediator";

        private float TalkTextMaxWidth = 400f;

        private List<TaskDialogDefine> m_defines;

        private int m_currentDialogCount = 0;

        private int m_currentGroup;

        private AudioHandler m_tmpAudioHandler;

        private PlayerProxy m_playerProxy;

        private CityBuildingProxy m_buildingProxy;

        private bool bDispose;

        private int m_lastPos;

        private string m_rightOnAni = "Right_On";
        private string m_rightOffAni = "Right_Off";
        private string m_leftOnAni = "Left_On";
        private string m_leftOffAni = "Left_Off";
        #endregion

        //IMediatorPlug needs
        public ChapterDialogMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ChapterDialogView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ShowChapterDiaglog,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ShowChapterDiaglog:
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
            AppFacade.GetInstance().SendNotification(CmdConstant.OnChapterDiaglogEnd, m_currentGroup);
            bDispose = true;
            if(m_tmpAudioHandler!=null)
            {
                CoreUtils.audioService.StopByHandler(m_tmpAudioHandler);
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
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
            m_defines = CoreUtils.dataService.QueryRecords<TaskDialogDefine>().FindAll((i)=>
            {
                return i.group == m_currentGroup;
            });

            if(m_defines==null)
            {
                Debug.LogError("TaskDialog配置异常："+m_currentGroup);
                Timer.Register(1f,()=> { CoreUtils.uiManager.CloseUI(UI.s_ChapterDialog); });
                return;
            }

            m_defines.Sort((r1, r2) => { return r1.step.CompareTo(r2.step); });
            m_currentDialogCount = m_defines.Count;
            if (m_currentDialogCount <= 0)
            {
                Debug.LogError("TaskDialog配置异常：" + m_currentGroup);
                Timer.Register(1f, () =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_ChapterDialog);
                });
                return;
            }

            CoreUtils.logService.Info($"章节对话{m_currentDialogCount}条");
            PlayDialog();
        }

        private void PlayDialog()
        {
            if(bDispose)
            {
                return;
            }
            if(m_currentDialogCount<=0)
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
                    CoreUtils.uiManager.CloseUI(UI.s_ChapterDialog);
                });
                return;
            }
            view.m_btn_noTextButton_GameButton.onClick.RemoveAllListeners();
            if (m_tmpAudioHandler != null)
            {
                CoreUtils.audioService.StopByHandler(m_tmpAudioHandler);
            }
            TaskDialogDefine define = m_defines[m_defines.Count - m_currentDialogCount];
            m_currentDialogCount--;
            UI_Pop_TalkTip_SubView talkView;
            Spine.Unity.SkeletonGraphic role;
            if (define.leftOrRight == 1)
            {
                talkView = view.m_UI_Pop_TalkTip_left;
                role = view.m_spine_hero_left_SkeletonGraphic;
            }
            else
            {
                talkView = view.m_UI_Pop_TalkTip_right;
                role = view.m_spine_hero_right_SkeletonGraphic;
            }
            talkView.m_lbl_text_LanguageText.text = LanguageUtils.getText(define.text);
            talkView.UpdateBgSize(TalkTextMaxWidth);

            if (define.npcImg == "1")
            {
                ClientUtils.LoadSpine(role, CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_playerProxy.CurrentRoleInfo.country).heroRes, () => {
                    LoadModelCallback(define);
                });
            }
            else if(define.npcAgeModel!=null&& define.npcAgeModel.Count>=(int)m_buildingProxy.GetAgeType())
            {
                ClientUtils.LoadSpine(role, define.npcAgeModel[(int)m_buildingProxy.GetAgeType()-1], () =>
                {
                    LoadModelCallback(define);
                });
            }
            else 
            {
                ClientUtils.LoadSpine(role, define.npcImg, () =>
                {
                    LoadModelCallback(define);
                });
            }


            if (!string.IsNullOrEmpty(define.heroSound))
            {
                CoreUtils.audioService.PlayOneShot(define.heroSound, (ad) =>
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

        private void LoadModelCallback(TaskDialogDefine define)
        {
            if (view.m_pl_right_Animator == null)
            {
                return;
            }
            int currentPos = define.leftOrRight == 1 ? 1 : 2;
            if (define.leftOrRight == 1)
            {
                if (view.m_pl_right_Animator.gameObject.activeSelf)
                    view.m_pl_right_Animator.Play(m_rightOffAni);
                view.m_pl_left_Animator.gameObject.SetActive(true);
                if (m_lastPos != currentPos)
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
                    view.m_pl_right_Animator.Play(m_rightOnAni, -1, 0);
                }
            }
            m_lastPos = currentPos;
            view.m_btn_noTextButton_GameButton.onClick.AddListener(() =>
            {
                PlayDialog();
            });
        }
    }
}