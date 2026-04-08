// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Common_ScrollMessageMediator
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
using DG.Tweening;

namespace Game {
    public class UI_Common_ScrollMessageMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Common_ScrollMessageMediator";

        private float m_moveSpeed = 30;
        private float m_showTime = 1.2f;
        private float m_closeTime = 1.5f;
        private string m_closeAniName = "UA_ScrollMessageClose";
        private string m_showAniName = "UA_ScrollMessageShow";
        private Queue<ScrollMessage> m_queue = new Queue<ScrollMessage>();
        private ScrollMessageProxy m_msgProxy;
        private Timer m_timer = null;
        #endregion

        //IMediatorPlug needs
        public UI_Common_ScrollMessageMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Common_ScrollMessageView view;

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
            ClearTimer();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_msgProxy = AppFacade.GetInstance().RetrieveProxy(ScrollMessageProxy.ProxyNAME) as ScrollMessageProxy;

            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            m_moveSpeed *= config.scrollMessageSpeedMul;
            m_showTime = UIHelper.GetLengthByName(view.m_pl_offset_Animator, m_showAniName);
            m_closeTime = UIHelper.GetLengthByName(view.m_pl_offset_Animator, m_closeAniName);
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            
            Timer.Register(1.5f, InitShow);
        }

        #endregion

        private void InitShow()
        {
            if (view.gameObject == null)
            {
                return;
            }
            ScrollMessage msg = view.data as ScrollMessage;
            ClearTimer();
            view.m_lbl_content_LanguageText.text = msg.msg;
            view.m_pl_offset_Animator.Play("Show");
            MoveUIPos();
            m_timer = Timer.Register(m_showTime, () =>
            {
                if (view.gameObject == null)
                {
                    return;
                }
                var endValue = view.m_pl_offset_Animator.GetComponent<RectTransform>().rect.width + view.m_lbl_content_LanguageText.rectTransform.rect.width;
                endValue = LanguageUtils.IsArabic() ? endValue : endValue * -1;
                //加上自身偏移
                endValue += view.m_lbl_content_LanguageText.rectTransform.localPosition.x;
                view.m_lbl_content_LanguageText.transform.DOLocalMoveX(endValue,Mathf.Abs(endValue)/m_moveSpeed).SetEase(Ease.Linear).Play().onComplete = ()=>
                {
                    if (view.gameObject == null)
                    {
                        return;
                    }
                    m_timer = Timer.Register(m_closeTime,()=>
                    {
                        if (view.gameObject == null)
                        {
                            return;
                        }
                        CoreUtils.uiManager.CloseUI(UI.s_scrollMessage);
                        m_msgProxy.Dequeue();
                    });
                    view.m_pl_offset_Animator.Play("Close");
                }; 
            });
        }

        private void ClearTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void MoveUIPos()
        {
            if (LanguageUtils.IsArabic())
            {
                view.m_lbl_content_LanguageText.rectTransform.pivot= new Vector2(1,0.5f);
                view.m_lbl_content_LanguageText.rectTransform.anchorMax= new Vector2(0,0.5f);
                view.m_lbl_content_LanguageText.rectTransform.anchorMin= new Vector2(0,0.5f);
            }
            else
            {
                view.m_lbl_content_LanguageText.rectTransform.pivot = new Vector2(0,0.5f);
                view.m_lbl_content_LanguageText.rectTransform.anchorMax= new Vector2(1,0.5f);
                view.m_lbl_content_LanguageText.rectTransform.anchorMin= new Vector2(1,0.5f);
            }
            view.m_lbl_content_LanguageText.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}