// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月4日
// Update Time         :    2020年2月4日
// Class Description   :    HelpTipMediator
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
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Game {

    public class HelpTip
    {
        private HelpTipData m_data = new HelpTipData();

        public static HelpTip CreateTip(int tipID, Transform trans, bool isUIObj = true)
        {
            RectTransform rectTrans = trans as RectTransform;
            var pos = trans.position;
            if (rectTrans != null)
            {
                var offset = new Vector3(rectTrans.rect.center.x, rectTrans.rect.center.y, trans.localPosition.z);
                pos = trans.localToWorldMatrix.MultiplyPoint(offset);
            }
            return CreateTip(tipID, pos, isUIObj);
        }

        public static HelpTip CreateTip(string tip, Transform trans, bool isUIObj = true)
        {
            RectTransform rectTrans = trans as RectTransform;
            var pos = trans.position;
            if (rectTrans != null)
            {
                var offset = new Vector3(rectTrans.rect.center.x, rectTrans.rect.center.y, trans.localPosition.z);
                pos = trans.localToWorldMatrix.MultiplyPoint(offset);
            }
            return CreateTip(tip, pos, isUIObj);
        }

        public static HelpTip CreateTip(int tipID, Vector3 pos, bool isUIObj = true)
        {
            HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(tipID);
            string str = LanguageUtils.getTextFormat(define.l_typeID, LanguageUtils.getText(define.l_data1), LanguageUtils.getText(define.l_data2), LanguageUtils.getText(define.l_data3), LanguageUtils.getText(define.l_data4), LanguageUtils.getText(define.l_data5));
            return CreateTip(str,pos, isUIObj).SetWidth(define.width);
        }

        public static HelpTip CreateTip(int tipID, Transform trans, bool isUIObj = true, params object[] praram)
        {
            HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(tipID);
            string str = LanguageUtils.getTextFormat(define.l_typeID, LanguageUtils.getTextFormat(define.l_data1,praram));
            
            RectTransform rectTrans = trans as RectTransform;
            var pos = trans.position;
            if (rectTrans != null)
            {
                var offset = new Vector3(rectTrans.rect.center.x, rectTrans.rect.center.y, trans.localPosition.z);
                pos = trans.localToWorldMatrix.MultiplyPoint(offset);
            }
            return CreateTip(str, pos, isUIObj).SetWidth(define.width);
        }

        public static HelpTip CreateTip(string tip,Vector3 pos,bool isUIObj = true)
        {
            HelpTip helpTip = new HelpTip();
            helpTip.m_data.tipStr = tip;
            if (isUIObj)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(pos), CoreUtils.uiManager.GetUICamera(), out helpTip.m_data.position);
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, pos, CoreUtils.uiManager.GetUICamera(), out helpTip.m_data.position);
            }
            return helpTip;
        }

        public static HelpTip CreateTip(GameObject child, Vector2 childSzie, RectTransform transform)
        {
            var pos = transform.position;
            if (transform != null)
            {
                var offset = new Vector3(transform.rect.center.x, transform.rect.center.y, transform.localPosition.z);
                pos = transform.localToWorldMatrix.MultiplyPoint(offset);
            }
            return CreateTip(child, childSzie, pos);
        }

        public static HelpTip CreateTip(GameObject child,Vector2 childSize,Vector3 pos,bool isUIObj = true)
        {
            HelpTip helpTip = new HelpTip();
            helpTip.m_data.tipObj = child;
            helpTip.m_data.childSize = childSize;
            if (isUIObj)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(pos), CoreUtils.uiManager.GetUICamera(), out helpTip.m_data.position);
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, pos, CoreUtils.uiManager.GetUICamera(), out helpTip.m_data.position);
            }
            return helpTip;
        }

        public  HelpTip SetStyle(HelpTipData.Style style)
        {
            m_data.style = style;
            return this;
        }

        
        public HelpTip SetAutoFilter(bool horizenOrVertical = true)
        {
            if (horizenOrVertical)
            {
                this.SetStyle(m_data.position.x > 0 ? HelpTipData.Style.arrowRight : HelpTipData.Style.arrowLeft);
            }
            else
            {
                this.SetStyle(m_data.position.y > 0 ? HelpTipData.Style.arrowUp : HelpTipData.Style.arrowDown);
            }
            return this;
        }

        public HelpTip SetWidthBorder(float min ,float max)
        {
            this.m_data.m_border[0] = min;
            this.m_data.m_border[1] = max;
            return this;
        }

        public HelpTip SetHeightBorder(float min, float max)
        {
            this.m_data.m_border[2] = min;
            this.m_data.m_border[3] = max;
            return this;
        }

        public  HelpTip SetOffset(float offset)
        {
            m_data.offset = offset;
            return this;
        }

        public HelpTip SetWidth(float width)
        {
            m_data.width = width;
            return this;
        }

        public HelpTip SetMaxWidth(float width)
        {
            m_data.max_width = width;
            return this;
        }

        public HelpTip UnEnableTouchClose()
        {
            m_data.touchClose = false;
            return this;
        }

        public HelpTip TouchCloseOneselfClose()
        {
            m_data.touchOneselfClose = true;
            return this;
        }
        public HelpTip SetFilterPerFrame(bool value)
        {
            m_data.FilterPerFrame = value;
            return this;
        }

        public  HelpTip Show(Action callback = null)
        {
            CoreUtils.uiManager.CloseUI(UI.s_helpTip);
            CoreUtils.uiManager.ShowUI(UI.s_helpTip, callback, this.m_data);
            return this;
        }

        public HelpTip Close()
        {
            this.m_data.IsDispose = true;
            CoreUtils.uiManager.CloseUI(UI.s_helpTip);
            return this;
        }
    }

    public class HelpTipData
    {
        public enum Style
        {
            arrowDown,
            arrowUp,
            arrowRight,
            arrowLeft,
            autoFilter
        }

        public GameObject tipObj;
        public Vector2 childSize;
        public string tipStr;
        public Style style;
        public Vector2 position;
        public float offset;
        public float width;
        public float max_width = 550f;
        public bool touchClose = true;
        public bool touchOneselfClose = false;//点击到自身也会关闭
        public bool FilterPerFrame = false;

        public float[] m_border = new float[4];

        public bool IsDispose;

        public HelpTipData()
        {
            m_border[0] = 0;
            m_border[2] = 0;
            m_border[1] = Screen.width;
            m_border[3] = Screen.height;
        }
    }
    public class HelpTipMediator : GameMediator {
        #region Member
        public static string NameMediator = "HelpTipMediator";

        public HelpTipData m_helpTipData;

        //private const float MaxSize = 550f;
        private static Vector2 Frame = new Vector2(40f,40f);
        private Vector2 m_canvasSize = new Vector2();

        private Timer m_timer;
        #endregion

        //IMediatorPlug needs
        public HelpTipMediator(object viewComponent ):base(NameMediator, viewComponent ) {
            this.IsOpenUpdate = true;
        }


        public HelpTipView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.FilterHelpTip
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.FilterHelpTip:
                    Vector2 screenPos = (Vector2)notification.Body;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, screenPos, CoreUtils.uiManager.GetUICamera(), out screenPos);
                    view.m_pl_pos.anchoredPosition = screenPos;
                    if (m_helpTipData!=null)
                    {
                        CalcPopupPos();
                        m_helpTipData.style = HelpTipData.Style.autoFilter;
                    }
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
            
        }

        public override void PrewarmComplete(){
            
        }

        public override void OnRemove()
        {
            base.OnRemove();
            CancelTimer();
        }

        public override void Update()
        {
            if(m_helpTipData!=null)
            {
                if (Input.GetMouseButtonDown(0)&& m_helpTipData.touchClose)
                {
                    if (m_helpTipData.touchOneselfClose)
                    {
                        OnClose();
                        return;
                    }
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;
                    List<RaycastResult> result = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, result);
                    if (result.Count > 0)
                    {
                        for (int i = result.Count - 1; i >= 0; i--)
                        {
                            if (result[i].gameObject.transform.IsChildOf(view.gameObject.transform))
                            {
                                return;
                            }
                        }
                        OnClose();
                    }
                }
                if(m_helpTipData.FilterPerFrame)
                {
                    CalcPopupPos();
                }
            }

        }
        
        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_helpTip);
        }

        protected override void InitData()
        {
            m_helpTipData = view.data as HelpTipData;
            RectTransform rect = CoreUtils.uiManager.GetCanvas().GetComponent<RectTransform>();
            if(rect)
            {
                m_canvasSize = rect.sizeDelta;
            }
            CancelTimer();
            if (m_helpTipData.IsDispose)
            {
                m_timer = Timer.Register(0.02f, OnClose);
            }
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
            if(m_helpTipData==null)
            {
                return;
            }
            if(m_helpTipData.tipObj!=null)
            {
                SetChild();
            }
            else
            {
                SetTipString();
            }
            SetOffset();
        }

        private void SetChild()
        {
            m_helpTipData.tipObj.transform.SetParent(view.m_img_bg_PolygonImage.transform);
            m_helpTipData.tipObj.transform.localScale = Vector3.one;
            view.m_lbl_text_LanguageText.text = string.Empty;
            view.m_img_bg_PolygonImage.rectTransform.sizeDelta = m_helpTipData.childSize + Frame;
            view.m_pl_pos.anchoredPosition = m_helpTipData.position;
        }

        private void SetTipString()
        {
            float width = m_helpTipData.width == 0 ? m_helpTipData.max_width : m_helpTipData.width;
            view.m_lbl_text_LanguageText.text = m_helpTipData.tipStr;
            if (m_helpTipData.width != 0 || view.m_lbl_text_LanguageText.preferredWidth >= width)
            {
                view.m_lbl_text_LanguageText.rectTransform.sizeDelta = new Vector2(width, 0);
                view.m_lbl_text_LanguageText.rectTransform.sizeDelta = new Vector2(width, view.m_lbl_text_LanguageText.preferredHeight);
            }
            else
            {
                view.m_lbl_text_LanguageText.rectTransform.sizeDelta = new Vector2(view.m_lbl_text_LanguageText.preferredWidth, 0);
                view.m_lbl_text_LanguageText.rectTransform.sizeDelta = new Vector2(view.m_lbl_text_LanguageText.preferredWidth, view.m_lbl_text_LanguageText.preferredHeight);
            }
            view.m_img_bg_PolygonImage.rectTransform.sizeDelta = view.m_lbl_text_LanguageText.rectTransform.sizeDelta + Frame;

            view.m_pl_pos.anchoredPosition = m_helpTipData.position;
        }

        private void SetStyle()
        {
            switch(m_helpTipData.style)
            {
                case HelpTipData.Style.arrowDown:
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                    break;
                case HelpTipData.Style.arrowUp:
                    view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                    break;
                case HelpTipData.Style.arrowLeft:
                    view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                    break;
                case HelpTipData.Style.arrowRight:
                    view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                    break;
                default:break;
            }
        }

        private void SetOffset(bool reversal = false)
        {
            float offset = reversal ? -m_helpTipData.offset : m_helpTipData.offset;
            switch (m_helpTipData.style)
            {
                case HelpTipData.Style.arrowDown:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.y / 2;
                    offset += view.m_img_arrowSideButtom_PolygonImage.rectTransform.sizeDelta.y;
                    view.m_pl_pos.anchoredPosition += new Vector2(0, offset);
                    FilterInCanvasLeftAndRight();
                    SetStyle();
                    break;
                case HelpTipData.Style.arrowUp:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.y / 2;
                    offset += view.m_img_arrowSideTop_PolygonImage.rectTransform.sizeDelta.y;
                    view.m_pl_pos.anchoredPosition -= new Vector2(0, offset);
                    FilterInCanvasLeftAndRight();
                    SetStyle();
                    break;
                case HelpTipData.Style.arrowLeft:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.x / 2;
                    offset += view.m_img_arrowSideL_PolygonImage.rectTransform.sizeDelta.x;
                    view.m_pl_pos.anchoredPosition += new Vector2(offset, 0);
                    FilterInCanvasUpAndDown();
                    SetStyle();
                    break;
                case HelpTipData.Style.arrowRight:
                    offset += view.m_img_bg_PolygonImage.rectTransform.sizeDelta.x / 2;
                    offset += view.m_img_arrowSideR_PolygonImage.rectTransform.sizeDelta.x;
                    view.m_pl_pos.anchoredPosition -= new Vector2(offset, 0);
                    FilterInCanvasUpAndDown();
                    SetStyle();
                    break;
                case HelpTipData.Style.autoFilter:
                    CalcPopupPos();
                    break;
                default:break;
            }
        }


        //上下自适应平移
        private void FilterInCanvasUpAndDown()
        {
            Vector3[] corners = new Vector3 [4];
            var localScale = view.m_pl_pos.localScale;
            view.m_pl_pos.localScale = Vector3.one;
            view.m_img_bg_PolygonImage.rectTransform.GetWorldCorners(corners);
            view.m_pl_pos.localScale = localScale;
            float yDown = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[0]).y;
            float yUp = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[1]).y;
            Vector2 offset = new Vector2();
            if (yDown < m_helpTipData.m_border[2])
            {
                offset = new Vector2(0, -yDown * (m_canvasSize.y / m_helpTipData.m_border[3]));
            }
            else if (yUp > m_helpTipData.m_border[3])
            {
                offset = new Vector2(0, (m_helpTipData.m_border[3] - yUp) * (m_canvasSize.y / m_helpTipData.m_border[3]));
            }
            view.m_pl_pos.anchoredPosition += offset;
            view.m_img_arrowSideL_PolygonImage.rectTransform.anchoredPosition -= offset;
            view.m_img_arrowSideR_PolygonImage.rectTransform.anchoredPosition -= offset;
        }

        //左右自适应平移
        private void FilterInCanvasLeftAndRight()
        {
            Vector3[] corners = new Vector3[4];
            var localScale = view.m_pl_pos.localScale;
            view.m_pl_pos.localScale = Vector3.one;
            view.m_img_bg_PolygonImage.rectTransform.GetWorldCorners(corners);
            view.m_pl_pos.localScale = localScale;

            float xLeft = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[0]).x;
            float xRight = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[2]).x;
            Vector2 offset = new Vector2();
            if (xLeft < m_helpTipData.m_border[0])
            {
                offset = new Vector2(-xLeft * (m_canvasSize.x/ m_helpTipData.m_border[1]),0);
            }
            else if(xRight> m_helpTipData.m_border[1])
            {
                offset = new Vector2((m_helpTipData.m_border[1]-xRight)* m_canvasSize.x/ m_helpTipData.m_border[1], 0);
            }
            view.m_pl_pos.anchoredPosition += offset;
            view.m_img_arrowSideButtom_PolygonImage.rectTransform.anchoredPosition -= offset;
            view.m_img_arrowSideTop_PolygonImage.rectTransform.anchoredPosition -= offset;
        }

        private void CalcPopupPos()
        {
            Vector2 size = view.m_img_bg_PolygonImage.rectTransform.sizeDelta;
            Vector2 pos = view.m_pl_pos.anchoredPosition;
            float borderHeight = m_helpTipData.m_border[3] - m_helpTipData.m_border[2];
            float borderWidth = m_helpTipData.m_border[1] - m_helpTipData.m_border[0];

            bool horizen = borderWidth / pos.x > borderHeight/ pos.y;

            if(horizen)
            {
                if(pos.x >=0)
                {
                    m_helpTipData.style = HelpTipData.Style.arrowRight;
                }
                else
                {
                    m_helpTipData.style = HelpTipData.Style.arrowLeft;
                }
            }
            else if(pos.y >=0)
            {
                m_helpTipData.style = HelpTipData.Style.arrowUp;
            }
            else
            {
                m_helpTipData.style = HelpTipData.Style.arrowDown;
            }
            SetOffset();
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }
    }
}