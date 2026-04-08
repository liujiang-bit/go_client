// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月4日
// Update Time         :    2020年3月4日
// Class Description   :    FingerMediator
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
    public enum EnumArrorDirection
    {
        RightDown = 1,
        Down = 2,
        LeftDown = 3,
        Left = 4,
        LeftUp = 5,
        Up = 6,
        RightUp = 7,
        Right = 8,
        None = 9,
    }

    public class FingerTargetParam
    {
        public GameObject AreaTarget;           //引导区域对象
        public Vector2 AreaRect;
        public int NodeType = 1;                //节点类型 1：UI节点 2：地图上物体
        public bool IsUseRect;
        public Vector2 ScaleSize;
        public bool IsScale;
        public int ArrowDirection;              //箭头位置
        public bool IsAutoClose = true;         //超时是否自动关闭
        public bool IsClickClose = true;        //手放开才会关闭
        public bool IsTouchBeginClose = false;  //是否按下立即关闭
        public GameObject EffectMountTarget;    //特效挂载目标
        public bool IsShowGuideAreaBorder;      //是否显示引导边框
        public int SourceType = 0;              //1主界面任务引导
        public float AutoCloseTime;             //自动关闭时间
        public bool IsUseDefaultAutoCloseTime = true; //是否使用默认自动关闭时间
        public string EffectName;               //指定特效
    }

    public class FingerMediator : GameMediator
    {

        #region Member
        public static string NameMediator = "FingerMediator";

        private FingerTargetParam m_targetParam;
        private GameObject m_areaTarget;

        private Vector3 m_beforePosition;
        private Vector3 m_vector3;
        private RectTransform m_guideAreaRect;

        private int m_nodeType;
        private bool m_isAutoClose;
        private bool m_isClickClose;
        private bool m_isTouchBeginClose;

        private float m_times;
        private bool m_isDispose;

        private GameObject m_defaultEffect;

        private GameObject m_effect;

        private int m_sourceType;

        private float m_autoCloseTime;

        private GameObject m_assignEffect;//指定特效
        private string m_assignEffectName;//指定特效名称

        #endregion

        //IMediatorPlug needs
        public FingerMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public FingerView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.CancelGuideRemind,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.CancelGuideRemind:
                    int type = (int)notification.Body;
                    if (m_sourceType == type)
                    {
                        Close();
                    }
                    break;
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
            if (view.data == null)
            {
                return;
            }
            RefreshContent();
        }

        public override void OnRemove()
        {
            m_isDispose = true;
        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {
            if (m_areaTarget != null)
            {
                if (m_nodeType == 1)
                {
                    if (m_beforePosition != m_areaTarget.transform.position) //目标节点坐标变更 则需要重设箭头位置
                    {
                        m_beforePosition = m_areaTarget.transform.position;
                        UpdateAreaAndArrow();
                    }
                }
                else
                {
                    m_vector3 = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), m_areaTarget.transform.position);
                    if (m_beforePosition != m_vector3)
                    {
                        m_beforePosition = m_vector3;
                        UpdateAreaAndArrow();
                    }
                }
            }

            if (m_isClickClose)
            {
                if (m_isTouchBeginClose)
                {
                    if (IsTouchBegin())
                    {
                        Close();
                        return;
                    }
                }
                if (IsTouchEnd())
                {
                    Close();
                    return;
                }
            }
            if (m_isAutoClose)
            {
                m_times = m_times + Time.deltaTime;
                if (m_times > m_autoCloseTime)
                {
                    Close();
                }
            }
        }

        protected override void InitData()
        {
            IsOpenUpdate = true;
            Transform trans = view.gameObject.transform.Find("pl_content/UI_10009");
            m_defaultEffect = trans.gameObject;
            m_defaultEffect.gameObject.SetActive(false);

            view.m_pl_arrow.gameObject.SetActive(false);
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        private void RefreshContent()
        {
            m_effect = null;
            m_autoCloseTime = 5;
            m_defaultEffect.gameObject.SetActive(false);

            if (view.data != null)
            {
                view.m_pl_arrow.gameObject.SetActive(true);

                m_targetParam = view.data as FingerTargetParam;
                if (m_targetParam.AreaTarget == null)
                {
                    view.gameObject.SetActive(false);
                    Timer.Register(0.02f, Close);
                    return;
                }
                m_areaTarget = m_targetParam.AreaTarget;
                m_nodeType = m_targetParam.NodeType;
                m_isAutoClose = m_targetParam.IsAutoClose;
                m_isClickClose = m_targetParam.IsClickClose;
                m_isTouchBeginClose = m_targetParam.IsTouchBeginClose;
                m_sourceType = m_targetParam.SourceType;
                bool isUseAssignEffect = string.IsNullOrEmpty(m_targetParam.EffectName) ? false : true;

                //使用指定特效
                if (m_assignEffect != null)
                {
                    CoreUtils.assetService.Destroy(m_assignEffect);
                    m_assignEffect = null;
                }
                if (isUseAssignEffect)
                {
                    m_assignEffectName = m_targetParam.EffectName;
                }
                else
                {
                    m_assignEffect = null;
                    m_assignEffectName = "";
                    m_effect = m_defaultEffect;
                }

                if (m_targetParam.IsAutoClose && !m_targetParam.IsUseDefaultAutoCloseTime)
                {
                    m_autoCloseTime = m_targetParam.AutoCloseTime;
                }
                if (m_areaTarget != null)
                {
                    if (m_nodeType == 1)
                    {
                        m_beforePosition = m_targetParam.AreaTarget.transform.position;
                    }
                    else
                    {
                        m_beforePosition = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), m_targetParam.AreaTarget.transform.position);
                    }
                }
                m_times = 0;

                UpdateAreaAndArrow();

                //异步加载指定特效
                if (isUseAssignEffect)
                {
                    CoreUtils.assetService.Instantiate(m_assignEffectName, (effectObj) =>
                    {
                        if (m_isDispose)
                        {
                            CoreUtils.assetService.Destroy(effectObj);
                            return;
                        }
                        if (m_assignEffect != null)
                        {
                            CoreUtils.assetService.Destroy(m_assignEffect);
                        }
                        effectObj.transform.SetParent(view.m_pl_content.transform);
                        effectObj.transform.localScale = Vector3.one;
                        m_assignEffect = effectObj;
                        m_effect = m_assignEffect;
                        UpdateAreaAndArrow();
                    });
                }
            }
        }

        private void UpdateAreaAndArrow()
        {
            if (m_targetParam.NodeType == 1)
            {
                SetUiGuideArea();
            }
            else
            {
                SetOtherArea();
            }
            SetArrowPosAngle();
        }

        private void SetUiGuideArea()
        {
            Vector2 localPos;
            Vector3 pos = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(m_targetParam.AreaTarget.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform targetAreaRect = m_targetParam.AreaTarget.GetComponent<RectTransform>();
            RectTransform guideRect = view.m_img_target_PolygonImage.GetComponent<RectTransform>();
            //guideRect.anchorMin = targetAreaRect.anchorMin;
            //guideRect.anchorMax = targetAreaRect.anchorMax;
            guideRect.pivot = targetAreaRect.pivot;
            view.m_img_target_PolygonImage.transform.localPosition = localPos;
            if (m_targetParam.IsUseRect)
            {
                if (m_targetParam.AreaRect != null)
                {
                    guideRect.sizeDelta = new Vector2(m_targetParam.AreaRect.x, m_targetParam.AreaRect.y);
                }
                else
                {
                    guideRect.sizeDelta = new Vector2(100, 100);
                }
            }
            else
            {
                guideRect.sizeDelta = new Vector2(targetAreaRect.rect.width, targetAreaRect.rect.height);
                if (m_targetParam.IsScale)
                {
                    guideRect.sizeDelta = new Vector2(guideRect.sizeDelta.x * m_targetParam.ScaleSize.x,
                                                      guideRect.sizeDelta.y * m_targetParam.ScaleSize.y);
                }
            }
            m_guideAreaRect = guideRect;

            //是否显示引导边框
            if (m_targetParam.IsShowGuideAreaBorder)
            {
                view.m_UE_GuideGuild.gameObject.SetActive(true);
            }
            else
            {
                view.m_UE_GuideGuild.gameObject.SetActive(false);
            }

            //是否显示引导特效
            if (m_targetParam.EffectMountTarget != null && m_effect != null)
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
                    m_effect.SetActive(true);
                    return;
                }
            }
        }

        private void SetOtherArea()
        {
            Vector2 localPos;
            //Vector3 pos1 = new Vector3(m_targetParam.AreaTarget.transform.position.x, m_targetParam.AreaTarget.transform.position.z, m_targetParam.AreaTarget.transform.position.y);
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), m_targetParam.AreaTarget.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);


            RectTransform guideRect = view.m_img_target_PolygonImage.GetComponent<RectTransform>();
            guideRect.pivot = new Vector2(0.5f, 0.5f);
            view.m_img_target_PolygonImage.transform.localPosition = localPos;
            if (m_targetParam.IsUseRect)
            {
                if (m_targetParam.AreaRect != null)
                {
                    guideRect.sizeDelta = new Vector2(m_targetParam.AreaRect.x, m_targetParam.AreaRect.y);
                }
                else
                {
                    guideRect.sizeDelta = new Vector2(100, 100);
                }
            }
            else
            {
                guideRect.sizeDelta = new Vector2(100, 100);
            }
            m_guideAreaRect = guideRect;

             //是否显示引导特效
            if (m_targetParam.EffectMountTarget != null && m_effect!=null)
            {
                m_effect.transform.position = view.m_img_target_PolygonImage.transform.position;
                m_effect.SetActive(true);
            }
        }

        private void SetArrowPosAngle()
        {
            //设置箭头坐标 以及 旋转角度
            float rectWidth = m_guideAreaRect.sizeDelta.x;
            float rectHeight = m_guideAreaRect.sizeDelta.y;
            float centerX = view.m_img_target_PolygonImage.transform.localPosition.x - Mathf.Abs(m_guideAreaRect.pivot.x) * rectWidth + rectWidth / 2;
            float centerY = view.m_img_target_PolygonImage.transform.localPosition.y - Mathf.Abs(m_guideAreaRect.pivot.y) * rectHeight + rectHeight / 2;
            int arrowDirection = m_targetParam.ArrowDirection;
            //int arrowDirection = 8;
            Vector2 arrowPos = Vector2.zero;
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
                case 9://不显示箭头                   
                    break;
                default:
                    Debug.LogErrorFormat("未知方向：{0}", arrowDirection);
                    break;
            }
            view.m_pl_arrow.gameObject.SetActive((arrowDirection != 9));
            view.m_pl_arrow.localPosition = arrowPos;
            float rotationAngle = arrowDirection * 45f * -1;
            view.m_pl_arrow_rotation.localEulerAngles = new Vector3(0f, 0f, rotationAngle);
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

        private void Close()
        {
            if (m_isDispose)
            {
                return;
            }
            CoreUtils.uiManager.CloseUI(UI.s_fingerInfo);
        }

        #endregion
    }
}