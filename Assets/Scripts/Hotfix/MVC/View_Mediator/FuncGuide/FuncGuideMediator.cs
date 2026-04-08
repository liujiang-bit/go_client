// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    FuncGuideMediator 功能介绍引导
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
using UnityEngine.UI;

namespace Game {
    public class FuncGuideMediator : GameMediator {
        #region Member
        public static string NameMediator = "FuncGuideMediator";

        private GuideExDefine m_targetDefine;
        private GuideTargetParam m_targetParam;

        private int m_findWay;

        private GameObject m_areaTarget;
        private Vector3 m_beforePosition;
        private Vector2 m_beforeScale = new Vector2(0, 0);
        private Vector3 m_vector3;
        private RectTransform m_guideAreaRect;
        private int m_areaTargetNodeType;

        private float m_waitTime = 0f;
        private bool m_isCheckPosChange = false;
        private bool m_isAllowClick = false;
        private bool m_isDispose = false;

        private bool m_isForceGuide = false; //是否强制引导
        private bool m_isTouchEndClose = false;

        private GameObject m_effect;

        //list滚动状态保存
        private bool m_isVertical = false;
        private bool m_isHorizontal = false;

        private float m_tipMaxWidth = 0;

        private GameButton m_targetGameButton;

        #endregion

        //IMediatorPlug needs
        public FuncGuideMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public FuncGuideView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {

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

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {
            Refresh();
        }

        public override void WinClose() {

        }

        public override void OnRemove()
        {
            m_isDispose = true;
        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {
            if (!m_isCheckPosChange)
            {
                return;
            }
            if (m_areaTarget == null)
            {
                return;
            }
            if (m_findWay >= 3)
            {
                m_vector3 = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), m_areaTarget.transform.position);
                if (m_beforePosition != m_vector3)
                {
                    m_beforePosition = m_vector3;
                    UpdateArrorTipPos();
                }
            }
            else
            {
                if (m_beforePosition != m_areaTarget.transform.position || SizeIsChange()) //目标节点坐标变更 则需要重设箭头位置
                {
                    m_beforePosition = m_areaTarget.transform.position;
                    UpdateArrorTipPos();
                }
            }
            if (!m_isAllowClick)
            {
                if (Time.realtimeSinceStartup - m_waitTime > 0.2f)
                {
                    m_isAllowClick = true;
                    if (m_isTouchEndClose)
                    {
                        view.m_img_fullMask_PolygonImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        view.m_img_fullMask_PolygonImage.gameObject.SetActive(false);
                    }
                }
            }
            if (m_isAllowClick)
            {
                if (m_isForceGuide)
                {
                    return;
                }
                if (m_isTouchEndClose)
                {
                    if (IsTouchEnd())
                    {
                        NextGuide();
                    }
                }
                else
                {
                    if (IsTouchBegin())
                    {
                        Hide();
                    }
                }
            }
        }

        protected override void InitData()
        {
            IsOpenUpdate = true;

            Transform trans = view.gameObject.transform.Find("pl_content/UI_10009");
            m_effect = trans.gameObject;
            m_effect.gameObject.SetActive(false);

            HelpTipsDefine helpTipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(1);
            if (helpTipDefine != null)
            {
                m_tipMaxWidth = helpTipDefine.width;
            }
            else
            {
                m_tipMaxWidth = 600;
            }

            RectTransform rect = view.m_lbl_desc_LanguageText.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(m_tipMaxWidth - 50, rect.sizeDelta.y);

            //屏蔽引导按钮
            bool isTest = HotfixUtil.IsDebugable();
            view.m_btn_jump_GameButton.gameObject.SetActive(isTest);
            view.m_btn_neverJump_GameButton.gameObject.SetActive(false);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_jump_GameButton.onClick.AddListener(OnJump);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        //跳过引导
        private void OnJump()
        {
            CoreUtils.uiManager.CloseUI(UI.s_funcGuide);
        }

        private void Refresh()
        {
            m_targetParam = view.data as GuideTargetParam;
            m_targetDefine = m_targetParam.DefineData2;
            m_findWay = m_targetDefine.findWay;
            m_areaTarget = m_targetParam.AreaTarget;
            if (m_areaTarget != null)
            {
                if (m_findWay == 4)
                {
                    m_beforePosition = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), m_areaTarget.transform.position);
                }
                else
                {
                    m_beforePosition = m_targetParam.AreaTarget.transform.position;
                }
                m_isCheckPosChange = true;
            }

            m_isAllowClick = false;
            m_waitTime = Time.realtimeSinceStartup;

            bool isTest = HotfixUtil.IsDebugable();
            view.m_btn_jump_GameButton.gameObject.SetActive(isTest);
            view.m_img_fullMask_PolygonImage.gameObject.SetActive(true);
            view.m_pl_content.gameObject.SetActive(true);
            m_effect.gameObject.SetActive(false);

            m_areaTargetNodeType = 0;
            if (m_targetDefine.findWay == 1)//ShowUI节点
            {
                m_areaTargetNodeType = m_targetDefine.uiType;
            }
            else if (m_targetDefine.findWay == 2)//HUDLayer节点
            {
                m_areaTargetNodeType = m_targetDefine.guideNodeType;
            }
            else
            {
                m_areaTargetNodeType = m_targetDefine.uiType;
            }

            //是否强制引导
            if (m_targetDefine.guideCancel == 1)
            {
                view.m_img_mask_GuideHighlightMask.gameObject.SetActive(true);
                m_isForceGuide = true;
            }
            else
            {
                view.m_img_mask_GuideHighlightMask.gameObject.SetActive(false);
                m_isForceGuide = false;
            }

            if (m_targetDefine.guideArena == 1)
            {
                m_isTouchEndClose = true;
            }
            else
            {
                m_isTouchEndClose = false;
            }

            UpdateArrorTipPos();

            m_targetGameButton = null;
            //追加监听事件
            if (m_areaTargetNodeType == 1) //按钮
            {
                BtnAddListener();
            }
            else if (m_areaTargetNodeType == 2) //list
            {
                //将list滚动禁用
                if (m_targetParam.ListNode != null)
                {
                    ScrollRect component = m_targetParam.ListNode.GetComponent<ScrollRect>();
                    if (component != null)
                    {
                        m_isVertical = component.vertical;
                        m_isHorizontal = component.horizontal;
                        component.vertical = false;
                        component.horizontal = false;
                    }
                }
                if (m_targetDefine.guideNodeType == 1)
                {
                    BtnAddListener();
                }
            }

            //播放音效
            PlaySound();
        }

        private void PlaySound()
        {
            if (m_targetDefine.civilization != null && m_targetDefine.civilization.Count > 0)
            {
                PlayerProxy m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                if (m_playerProxy != null)
                {
                    int civilization = (int)m_playerProxy.GetCivilization();
                    for (int i = 0; i < m_targetDefine.civilization.Count; i++)
                    {
                        if (civilization == m_targetDefine.civilization[i])
                        {
                            if (m_targetDefine.heroSound != null && i < m_targetDefine.heroSound.Count)
                            {
                                Debug.LogFormat("播放文明音效：{0}", m_targetDefine.heroSound[i]);
                                PlaySoundByName(m_targetDefine.heroSound[i]);
                                return;
                            }
                        }
                    }
                }
            }

            if (m_targetDefine.languageSet != null && m_targetDefine.languageSet.Count > 0)
            {
                int language = (int)LanguageUtils.GetLanguage();
                int findIndex = -1;
                for (int i = 0; i < m_targetDefine.languageSet.Count; i++)
                {
                    if (m_targetDefine.languageSet[i] == language)
                    {
                        findIndex = i;
                        break;
                    }
                }
                if (findIndex > -1)
                {
                    if (m_targetDefine.soundClient != null && findIndex < m_targetDefine.soundClient.Count)
                    {
                        PlaySoundByName(m_targetDefine.soundClient[findIndex]);
                    }
                }
            }
        }

        //播放音效
        private void PlaySoundByName(string sound)
        {
            CoreUtils.audioService.PlayOneShot(sound, null);
        }

        private void BtnAddListener()
        {
            if (m_targetParam.AreaTarget == null)
            {
                return;
            }
            GameButton btn = m_targetParam.AreaTarget.GetComponent<GameButton>();
            if (btn == null)
            {
                Debug.LogError("GameButton not find");
                return;
            }
            //Debug.LogError("添加按钮事件：" + btn.gameObject.name);
            btn.onClick.AddListener(BtnClick);
            m_targetGameButton = btn;
        }

        private void BtnClick()
        {
            RemoveEvent();
            NextGuide();
        }

        private void RemoveEvent()
        {
            //Debug.LogError("移除按钮事件--");
            if (m_targetParam == null)
            {
                return;
            }
            if (m_targetParam.AreaTarget != null) //有可能节点会被移除掉
            {
                GameButton btn = m_targetParam.AreaTarget.GetComponent<GameButton>();
                if (btn != null)
                {
                    //Debug.LogError("移除按钮事件：" + btn.gameObject.name);
                    btn.onClick.RemoveListener(BtnClick);
                }
            }
            //恢复list滚动
            if (m_targetParam.ListNode != null)
            {
                ScrollRect component = m_targetParam.ListNode.GetComponent<ScrollRect>();
                if (component != null)
                {
                    component.vertical = m_isVertical;
                    component.horizontal = m_isHorizontal;
                    m_isVertical = false;
                    m_isHorizontal = false;
                }
            }
        }

        private bool SizeIsChange()
        {
            if (m_targetParam != null && m_targetParam.ScaleStatus > 1)
            {
                if (m_targetParam.ScaleParentTrans != null)
                {
                    if (m_beforeScale.x != m_targetParam.ScaleParentTrans.localScale.x || m_beforeScale.y != m_targetParam.ScaleParentTrans.localScale.y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void UpdateArrorTipPos()
        {
            m_waitTime = Time.realtimeSinceStartup;

            //设置可点击区域坐标
            if (m_areaTargetNodeType == 1) //按钮
            {
                SetUiGuideArea();
            }
            else if (m_areaTargetNodeType == 2)//list
            {
                if (m_targetDefine.guideNodeType == 1) //按钮
                {
                    SetUiGuideArea();
                }
            }
            else if (m_areaTargetNodeType == 3)//复选框
            {
                SetUiGuideArea();
            }
            else if (m_areaTargetNodeType == 4)//城市建筑
            {
                //m_isBuildingClickGuide = true;
                //禁止地图拖动
                WorldCamera.Instance().SetCanDrag(false);
                //SetMapPointArea();
            }
            else if (m_areaTargetNodeType == 5)//地图上野蛮人
            {
                //禁止地图拖动
                WorldCamera.Instance().SetCanDrag(false);
                //SetMapPointArea();
            }
            else if(m_areaTargetNodeType == 6)
            {
                SetUiGuideArea();
            }
            else
            {
                Debug.LogErrorFormat("异常Type:{0} guideId:{1}", m_areaTargetNodeType, m_targetDefine.ID);
            }

            //设置箭头位置和旋转角度
            SetArrowPosAngle();

            //设置tip坐标
            SetTipPosAngle();
        }

        private void SetArrowPosAngle()
        {
            if (m_guideAreaRect == null || m_guideAreaRect.gameObject == null)
            {
                return;
            }
            //设置箭头坐标 以及 旋转角度
            float rectWidth = m_guideAreaRect.sizeDelta.x;
            float rectHeight = m_guideAreaRect.sizeDelta.y;
            float centerX = view.m_img_target_PolygonImage.transform.localPosition.x - Mathf.Abs(m_guideAreaRect.pivot.x) * rectWidth + rectWidth / 2;
            float centerY = view.m_img_target_PolygonImage.transform.localPosition.y - Mathf.Abs(m_guideAreaRect.pivot.y) * rectHeight + rectHeight / 2;
            int arrowDirection = m_targetDefine.guideArrowheadAr;

            //阿语处理
            if (!LanguageUtils.IsArabic())
            {
                arrowDirection = m_targetDefine.guideArrowhead;
            }
            Vector2 arrowPos = Vector2.zero;
            bool isShowArrow = true;
            switch (arrowDirection)
            {
                case 1: //引导区域 右下方
                    arrowPos.Set(centerX + rectWidth / 2, centerY - rectHeight / 2);
                    break;
                case 2: //引导区域 下方
                    arrowPos.Set(centerX, centerY - rectHeight / 2);
                    break;
                case 3: //引导区域 左下方
                    arrowPos.Set(centerX - rectWidth / 2, centerY - rectHeight / 2);
                    break;
                case 4: //引导区域 左方
                    arrowPos.Set(centerX - rectWidth / 2, centerY);
                    break;
                case 5: //引导区域 左上方
                    arrowPos.Set(centerX - rectWidth / 2, centerY + rectHeight / 2);
                    break;
                case 6: //引导区域 上方
                    arrowPos.Set(centerX, centerY + rectHeight / 2);
                    break;
                case 7: //引导区域 右上方
                    arrowPos.Set(centerX + rectWidth / 2, centerY + rectHeight / 2);
                    break;
                case 8: //引导区域 右方
                    arrowPos.Set(centerX + rectWidth / 2, centerY);
                    break;
                default:
                    isShowArrow = false;
                    Debug.LogFormat("未知方向：{0}", arrowDirection);
                    break;
            }
            view.m_pl_arrow.gameObject.SetActive(isShowArrow);
            view.m_pl_arrow.localPosition = arrowPos;
            float rotationAngle = arrowDirection * 45f * -1;
            view.m_pl_arrow_rotation.localEulerAngles = new Vector3(0f, 0f, rotationAngle);
        }

        //设置tip位置和角度
        private void SetTipPosAngle()
        {
            if (m_targetDefine.l_guideTipsID > 0)
            {
                if (m_guideAreaRect == null || m_guideAreaRect.gameObject == null)
                {
                    return;
                }

                float rectWidth = m_guideAreaRect.sizeDelta.x;
                float rectHeight = m_guideAreaRect.sizeDelta.y;
                float centerX = view.m_img_target_PolygonImage.transform.localPosition.x - Mathf.Abs(m_guideAreaRect.pivot.x) * rectWidth + rectWidth / 2;
                float centerY = view.m_img_target_PolygonImage.transform.localPosition.y - Mathf.Abs(m_guideAreaRect.pivot.y) * rectHeight + rectHeight / 2;
                int arrowDirection = m_targetDefine.guideArrowhead;

                view.m_pl_tip.gameObject.SetActive(true);

                string desc = "";
                if (m_targetDefine.ID == 502)
                {
                    var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                    desc = LanguageUtils.getTextFormat(m_targetDefine.l_guideTipsID, allianceProxy.GetAbbreviationName());
                }
                else
                {
                    desc = LanguageUtils.getText(m_targetDefine.l_guideTipsID);
                }
                view.m_lbl_desc_LanguageText.text = desc;
                float tipWidth = view.m_lbl_desc_LanguageText.preferredWidth + 50f;
                float textHeight = view.m_lbl_desc_LanguageText.preferredHeight;
                if (tipWidth > m_tipMaxWidth)
                {
                    tipWidth = m_tipMaxWidth;
                }

                RectTransform tipRect = view.m_img_tipbg_PolygonImage.GetComponent<RectTransform>();
                tipRect.sizeDelta = new Vector2(tipWidth, textHeight+50f);

                float offsetPos = 20;

                Vector2 tipPos = Vector2.zero;
                int tipDirection = m_targetDefine.guideTipsPosAr;
                //阿语处理
                if (!LanguageUtils.IsArabic())
                {
                    tipDirection = m_targetDefine.guideTipsPos;
                }
                switch (tipDirection)
                {
                    case 1: //上
                        tipPos.Set(centerX, centerY + rectHeight / 2 + tipRect.sizeDelta.y / 2 + offsetPos);
                        view.m_pl_tipArrow.localPosition = new Vector2(0, -tipRect.sizeDelta.y / 2);
                        view.m_pl_tipArrow.localEulerAngles = new Vector3(0, 0, 0);
                        break;
                    case 2: //下
                        tipPos.Set(centerX, centerY - rectHeight / 2 - tipRect.sizeDelta.y / 2 - offsetPos);
                        view.m_pl_tipArrow.localPosition = new Vector2(0, tipRect.sizeDelta.y / 2);
                        view.m_pl_tipArrow.localEulerAngles = new Vector3(0, 0, 180);
                        break;
                    case 3: //左
                        tipPos.Set(centerX - rectWidth / 2 - tipRect.sizeDelta.x / 2 - offsetPos, centerY);
                        view.m_pl_tipArrow.localPosition = new Vector2(tipRect.sizeDelta.x / 2, 0);
                        view.m_pl_tipArrow.localEulerAngles = new Vector3(0, 0, 90);
                        break;
                    case 4: //右
                        tipPos.Set(centerX + rectWidth / 2 + tipRect.sizeDelta.x / 2 + offsetPos, centerY);
                        view.m_pl_tipArrow.localPosition = new Vector2(-tipRect.sizeDelta.x / 2, 0);
                        view.m_pl_tipArrow.localEulerAngles = new Vector3(0, 0, 270);
                        break;
                    default:
                        break;
                }
                view.m_pl_tip.transform.localPosition = tipPos;
            }
            else
            {
                view.m_pl_tip.gameObject.SetActive(false);
            }
        }

        private void SetUiGuideArea()
        {
            if (m_targetParam.AreaTarget == null)
            {
                return;
            }
            Vector2 localPos;
            Vector3 pos = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(m_targetParam.AreaTarget.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform targetAreaRect = m_targetParam.AreaTarget.GetComponent<RectTransform>();
            RectTransform guideRect = view.m_img_target_PolygonImage.GetComponent<RectTransform>(); 
            guideRect.pivot = targetAreaRect.pivot;
            view.m_img_target_PolygonImage.transform.localPosition = localPos;
            guideRect.sizeDelta = new Vector2(targetAreaRect.rect.width, targetAreaRect.rect.height);
            if (m_targetParam.ScaleStatus > 0)
            {
                if (m_targetParam.ScaleStatus == 1)
                {
                    guideRect.sizeDelta = new Vector2(guideRect.sizeDelta.x * m_targetParam.ScaleSize.x, guideRect.sizeDelta.y * m_targetParam.ScaleSize.y);
                }
                else
                {
                    if (m_targetParam.ScaleParentTrans != null)
                    {
                        guideRect.sizeDelta = new Vector2(guideRect.sizeDelta.x * m_targetParam.ScaleParentTrans.transform.localScale.x,
                                                          guideRect.sizeDelta.y * m_targetParam.ScaleParentTrans.transform.localScale.y);

                        m_beforeScale.Set(m_targetParam.ScaleParentTrans.localScale.x, m_targetParam.ScaleParentTrans.localScale.y);
                    }
                }
            }
            m_guideAreaRect = guideRect;
            if (m_targetDefine.guideArena == 1)
            {
                view.m_UE_GuideGuild.gameObject.SetActive(true);
            }
            else
            {
                view.m_UE_GuideGuild.gameObject.SetActive(false);
            }

            if (m_targetParam.EffectMountTarget != null)
            {
                Vector3 pos2 = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(m_targetParam.EffectMountTarget.transform.position);
                Vector2 localPos2;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                        pos2,
                                                                        CoreUtils.uiManager.GetUICamera(),
                                                                        out localPos2);
                RectTransform guideRect2 = m_targetParam.EffectMountTarget.GetComponent<RectTransform>();
                if (guideRect2 != null)
                {
                    Vector2 newPos3 = Vector2.zero;
                    newPos3.Set(localPos2.x - guideRect2.pivot.x * guideRect2.rect.width + guideRect2.rect.width / 2,
                                localPos2.y - guideRect2.pivot.y * guideRect2.rect.height + guideRect2.rect.height / 2);
                    Vector3 newPos4 = view.gameObject.transform.TransformPoint(newPos3);
                    m_effect.transform.position = newPos4;
                    m_effect.gameObject.SetActive(true);
                    return;
                }
            }

            Vector2 newPos = Vector2.zero;
            newPos.Set(localPos.x - guideRect.pivot.x * targetAreaRect.rect.width + targetAreaRect.rect.width / 2,
                       localPos.y - guideRect.pivot.y * targetAreaRect.rect.height + targetAreaRect.rect.height / 2);
            Vector3 newPos2 = view.gameObject.transform.TransformPoint(newPos);
            m_effect.transform.position = newPos2;
        }

        /// <summary>
        /// 是否结束点击屏幕
        /// </summary>
        /// <returns><c>true</c>, if end was touched, <c>false</c> otherwise.</returns>
        public static bool IsTouchEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                return true;
            }
            return false;
        }

        public static bool IsTouchBegin()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
            return false;
        }

        private void NextGuide()
        {
            m_areaTarget = null;
            m_isCheckPosChange = false;

            view.m_btn_jump_GameButton.gameObject.SetActive(false);

            //把当前引导内容隐藏掉 重新把遮罩显示出来
            view.m_img_fullMask_PolygonImage.gameObject.SetActive(true);
            view.m_pl_content.gameObject.SetActive(false);

            RectTransform guideRect = view.m_img_target_PolygonImage.GetComponent<RectTransform>();
            if (guideRect != null)
            {
                guideRect.sizeDelta = new Vector2(1f, 1f);
                view.m_img_target_PolygonImage.transform.localPosition = new Vector3(-1f, -1f, 0);
                view.m_img_mask_GuideHighlightMask.ForceUpdatePos();
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.NextFuncGuideStep, m_targetDefine);
        }

        private void Hide()
        {
            if (m_isDispose)
            {
                return;
            }
            //回复list滚动
            if (m_targetParam !=null && m_targetParam.ListNode != null)
            {
                ScrollRect component = m_targetParam.ListNode.GetComponent<ScrollRect>();
                if (component != null)
                {
                    component.vertical = m_isVertical;
                    component.horizontal = m_isHorizontal;
                    m_isVertical = false;
                    m_isHorizontal = false;
                }
                m_targetParam.ListNode = null;
            }

            m_isCheckPosChange = false;

            view.m_btn_jump_GameButton.gameObject.SetActive(false);

            //把当前引导内容隐藏掉 重新把遮罩显示出来
            view.m_img_fullMask_PolygonImage.gameObject.SetActive(false);
            view.m_pl_content.gameObject.SetActive(false);

            RectTransform guideRect = view.m_img_target_PolygonImage.GetComponent<RectTransform>();
            if (guideRect != null)
            {
                guideRect.sizeDelta = new Vector2(1f, 1f);
                view.m_img_target_PolygonImage.transform.localPosition = new Vector3(-1f, -1f, 0);
                view.m_img_mask_GuideHighlightMask.ForceUpdatePos();
            }
            //if (m_targetGameButton != null)
            //{
            //    if (m_targetGameButton.gameObject != null)
            //    {
            //        m_targetGameButton.onClick.RemoveListener(BtnClick);
            //    }
            //}
        }
    }
}