// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_IF_CaptainSkillUpSuccessMediator
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

    public class CaptainSkillUpSuccessViewData
    {
        public HeroProxy.Hero Hero { get; set; }

        public int SkillId { get; set; }

        public int SkillLevel { get; set; }
    }


    public class UI_IF_CaptainSkillUpSuccessMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_IF_CaptainSkillUpSuccessMediator";


        #endregion

        //IMediatorPlug needs
        public UI_IF_CaptainSkillUpSuccessMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_IF_CaptainSkillUpSuccessView view;

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
            if (m_skillUpEffectTimer != null)
            {
                m_skillUpEffectTimer.Cancel();
                m_skillUpEffectTimer = null;
            }
            
            AppFacade.GetInstance().SendNotification(CmdConstant.HeroSkillUpSuccess);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            CaptainSkillUpSuccessViewData data = view.data as CaptainSkillUpSuccessViewData;
            if (data == null || data.Hero == null) return;
            m_data = data;
            RefreshUI();
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void RefreshUI()
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiSkillLvUp);
            int skillIndex = 0;
            for(int i = 0; i < m_data.Hero.config.skill.Count; ++i)
            {
                if(m_data.Hero.config.skill[i] == m_data.SkillId)
                {
                    skillIndex = i;
                    break;
                }
            }
            view.m_UI_Item_CaptainSkill.SetSkillInfo(m_data.Hero, skillIndex, 0);
            view.m_lbl_levelBefore_LanguageText.text = $"{m_data.SkillLevel -1}";
            view.m_lbl_levelAfter_LanguageText.text = $"{m_data.SkillLevel}";
            var heroSkillCfg = CoreUtils.dataService.QueryRecord<Data.HeroSkillDefine>(m_data.SkillId);
            if(heroSkillCfg != null)
            {
                view.m_lbl_text_LanguageText.text = view.m_UI_Item_CaptainSkill.getSkillEffect(heroSkillCfg, m_data.SkillLevel);
            }
            PlayPowerUpEffect();
        }

        private void PlayPowerUpEffect()
        {
            CoreUtils.assetService.Instantiate("UE_CaptainPower", (go) =>
            {
                if (m_skillUpEffectGO != null)
                {
                    CoreUtils.assetService.Destroy(m_skillUpEffectGO);
                    m_skillUpEffectGO = null;
                }
                m_skillUpEffectGO = go;
                go.transform.SetParent(view.gameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                var textRect = go.transform.Find("pl_offset/lbl_text");
                if (textRect != null)
                {
                    var languageText = textRect.gameObject.GetComponent<UnityEngine.UI.LanguageText>();
                    if (languageText != null)
                    {
                        int lastScore = 0;
                        if (m_data.SkillLevel > 1)
                        {
                            int lastHeroSkillEffectId = m_data.SkillId * 1000 + (m_data.SkillLevel - 1);
                            var lastHeroSkillEffectCfg = CoreUtils.dataService.QueryRecord<Data.HeroSkillEffectDefine>(lastHeroSkillEffectId);
                            if (lastHeroSkillEffectCfg != null)
                            {
                                lastScore = lastHeroSkillEffectCfg.score;
                            }
                        }

                        int heroSkillEffectId = m_data.SkillId * 1000 + m_data.SkillLevel;
                        var heroSkillEffectCfg = CoreUtils.dataService.QueryRecord<Data.HeroSkillEffectDefine>(heroSkillEffectId);
                        if (heroSkillEffectCfg != null)
                        {
                            languageText.text = string.Format(LanguageUtils.getText(145051), heroSkillEffectCfg.score - lastScore);
                        }
                    }
                }
                if (m_skillUpEffectTimer != null)
                {
                    Timer.Cancel(m_skillUpEffectTimer);
                    m_skillUpEffectTimer = null;
                }

                var animator = go.GetComponentInChildren<Animator>();
                float delayTime = ClientUtils.GetAnimationLength(animator, 0);
                m_skillUpEffectTimer = Timer.Register(delayTime, () =>
                {
                    if (m_skillUpEffectGO != null)
                    {
                        CoreUtils.assetService.Destroy(m_skillUpEffectGO);
                        m_skillUpEffectGO = null;
                    }
                    m_skillUpEffectTimer = null;
                });
            });
        }

        private CaptainSkillUpSuccessViewData m_data = null;
        private GameObject m_skillUpEffectGO = null;
        private Timer m_skillUpEffectTimer = null;
    }
}