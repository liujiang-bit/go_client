// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月11日
// Update Time         :    2020年2月11日
// Class Description   :    TalkTipMediator
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

namespace Game
{
    public class TalkTip
    {
        private TalkTipData m_data = new TalkTipData();

        public static TalkTip CreateTip(string tip, Transform trans, bool parentIsUI = true)
        {
            return CreateTip(tip, trans.position, parentIsUI);
        }

        public static TalkTip CreateTip(string tip, Vector3 pos, bool parentIsUI = true)
        {
            TalkTip TalkTip = new TalkTip();
            TalkTip.m_data.tipStr = tip;
            if (parentIsUI)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(pos), CoreUtils.uiManager.GetUICamera(), out TalkTip.m_data.position);
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, pos, CoreUtils.uiManager.GetUICamera(), out TalkTip.m_data.position);
            }
            return TalkTip;
        }

        public TalkTip SetStyle(TalkTipData.Style style)
        {
            m_data.style = style;
            return this;
        }

        public TalkTip SetOffset(float offset)
        {
            m_data.offset = offset;
            return this;
        }

        public TalkTip SetWidth(float width)
        {
            m_data.width = width;
            return this;
        }

        public TalkTip SetCloseTime(float time)
        {
            m_data.time = time;
            return this;
        }

        public TalkTip Show()
        {
            CoreUtils.uiManager.CloseUI(UI.s_talkTip);
            CoreUtils.uiManager.ShowUI(UI.s_talkTip, null, this.m_data);
            return this;
        }
    }

    public class TalkTipData
    {
        public enum Style
        {
            arrowDownLeft,
            arrowDownRight,
            arrowRight,
            arrowLeft
        }

        public string tipStr;
        public Style style;
        public Vector2 position;
        public float offset;
        public float width;
        public float time;
    }
    public class TalkTipMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "TalkTipMediator";

        public TalkTipData m_TalkTipData;

        private const float MaxSize = 550f;
        private static Vector2 Frame = new Vector2(20f, 20f);
        private static Vector2 DownArrow = new Vector2(0.5f, 0f);
        private static Vector2 UpArrow = new Vector2(0.5f, 1f);
        private static Vector2 RightArrow = new Vector2(1f, 0.5f);
        private static Vector2 LeftArrow = new Vector2(0f, 0.5f);
        #endregion

        //IMediatorPlug needs
        public TalkTipMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public TalkTipView view;
        private float m_closeTime;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {

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

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {
            if (Time.unscaledTime > m_closeTime)
            {
                CoreUtils.uiManager.CloseUI(UI.s_talkTip);
            }
        }

        protected override void InitData()
        {
            m_TalkTipData = view.data as TalkTipData;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            InitView();
        }

        #endregion

        private void InitView()
        {
            if (m_TalkTipData == null)
            {
                return;
            }
            IsOpenUpdate = true;

            float width = m_TalkTipData.width == 0 ? MaxSize : m_TalkTipData.width;
            view.m_lbl_text_LanguageText.text = m_TalkTipData.tipStr;
            if (view.m_lbl_text_LanguageText.rectTransform.sizeDelta.x != width)
            {
                view.m_lbl_text_LanguageText.rectTransform.sizeDelta = new Vector2(width, 0);
            }
            float tipBagWidth = view.m_lbl_text_LanguageText.preferredWidth;
            if (tipBagWidth > width)
            {
                tipBagWidth = width;
            }
            view.m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipBagWidth + 40, view.m_lbl_text_LanguageText.preferredHeight + 20);

            view.m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition = m_TalkTipData.position;
            SetStyle();
            SetOffset();

            if(m_TalkTipData.time == 0.0f)
            {
                m_TalkTipData.time = 2.0f;
            }

            m_closeTime = Time.unscaledTime + m_TalkTipData.time;
        }

        private void SetStyle()
        {
            switch (m_TalkTipData.style)
            {
                case TalkTipData.Style.arrowDownLeft:
                    view.m_img_arrowSideBL_PolygonImage.gameObject.SetActive(true);
                    break;
                case TalkTipData.Style.arrowDownRight:
                    view.m_img_arrowSideBR_PolygonImage.gameObject.SetActive(true);
                    break;
                case TalkTipData.Style.arrowLeft:
                    view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                    break;
                case TalkTipData.Style.arrowRight:
                    view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                    break;
                default: break;
            }
        }

        private void SetOffset(bool reversal = false)
        {
            float offset = reversal ? -m_TalkTipData.offset : m_TalkTipData.offset;
            switch (m_TalkTipData.style)
            {
                case TalkTipData.Style.arrowDownLeft:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.y / 2;
                    offset += view.m_img_arrowSideBR_PolygonImage.rectTransform.sizeDelta.y;
                    view.m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition += new Vector2(-view.m_img_arrowSideBL_PolygonImage.rectTransform.anchoredPosition.x + view.m_img_arrowSideBL_PolygonImage.rectTransform.sizeDelta.x / 2, offset);
                    FilterInCanvasLeftAndRight();
                    break;
                case TalkTipData.Style.arrowDownRight:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.y / 2;
                    offset += view.m_img_arrowSideBR_PolygonImage.rectTransform.sizeDelta.y;
                    view.m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition += new Vector2(-view.m_img_arrowSideBR_PolygonImage.rectTransform.anchoredPosition.x- view.m_img_arrowSideBR_PolygonImage.rectTransform.sizeDelta.x/2, offset);
                    FilterInCanvasLeftAndRight();
                    break;
                case TalkTipData.Style.arrowLeft:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.x / 2;
                    offset += view.m_img_arrowSideL_PolygonImage.rectTransform.sizeDelta.x;
                    view.m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition += new Vector2(offset, -view.m_img_arrowSideL_PolygonImage.rectTransform.sizeDelta.y / 2);
                    FilterInCanvasUpAndDown();
                    break;
                case TalkTipData.Style.arrowRight:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.x / 2;
                    offset += view.m_img_arrowSideL_PolygonImage.rectTransform.sizeDelta.x;
                    view.m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition -= new Vector2(offset, view.m_img_arrowSideL_PolygonImage.rectTransform.sizeDelta.y / 2);
                    FilterInCanvasUpAndDown();
                    break;
            }
        }


        //上下自适应平移
        private void FilterInCanvasUpAndDown()
        {
            Vector3[] corners = new Vector3[4];
            view.m_img_bg_PolygonImage.rectTransform.GetWorldCorners(corners);
            float yDown = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[0]).y;
            float yUp = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[1]).y;
            Vector2 offset = new Vector2();
            if (yDown < 0)
            {
                offset = new Vector2(0, -yDown);
            }
            else if (yUp > Screen.height)
            {
                offset = new Vector2(0, Screen.height - yUp);
            }
            view.m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition += offset;
            view.m_img_arrowSideL_PolygonImage.rectTransform.anchoredPosition -= offset;
            view.m_img_arrowSideR_PolygonImage.rectTransform.anchoredPosition -= offset;
        }

        //左右自适应平移
        private void FilterInCanvasLeftAndRight()
        {
            Vector3[] corners = new Vector3[4];
            view.m_img_bg_PolygonImage.rectTransform.GetWorldCorners(corners);

            float xLeft = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[0]).x;
            float xRight = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[2]).x;
            Vector2 offset = new Vector2();
            if (xLeft < 0)
            {
                offset = new Vector2(-xLeft, 0);
            }
            else if (xRight > Screen.width)
            {
                offset = new Vector2(Screen.width - xRight, 0);
            }
            view.m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition += offset;
            view.m_img_arrowSideBL_PolygonImage.rectTransform.anchoredPosition -= offset;
            view.m_img_arrowSideBR_PolygonImage.rectTransform.anchoredPosition -= offset;
        }
    }
}