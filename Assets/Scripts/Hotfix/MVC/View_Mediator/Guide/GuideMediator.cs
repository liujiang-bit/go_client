// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年2月4日
// Update Time         :    2020年2月4日
// Class Description   :    GuideMediator 新手引导界面
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
using System;
using UnityEngine.UI;

namespace Game {
    public class GuideTargetParam
    {
        public GameObject ListNode;             //list节点
        public GameObject AreaTarget;           //引导区域对象
        public GameObject EffectMountTarget;    //特效挂载目标
        public Vector2 ScaleSize;
        public int ScaleStatus; // 0无需处理缩放 1使用ScaleSize 2使用ParentScale
        public Transform ScaleParentTrans;
        public GuideDefine DefineData;
        public GuideExDefine DefineData2;
    }

    public class GuideMediator : GameMediator {
        #region Member
        public static string NameMediator = "GuideMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private GuideProxy m_guideProxy;

        private GuideDefine m_guideDefine;
        private GuideTargetParam m_targetParam;

        private GameObject m_areaTarget;
        private Vector3 m_beforePosition;
        private Vector3 m_vector3;
        private Vector2 m_beforeScale = new Vector2(0, 0);

        private bool m_isCheckPosChange = false;

        //list滚动状态保存
        private bool m_isVertical = false;
        private bool m_isHorizontal = false;

        private bool m_isBuildingClickGuide = false;
        private RectTransform m_guideAreaRect;

        private int m_areaTargetNodeType;

        private GameObject m_effect;

        private int m_findWay;

        private float m_waitTime = 0f;
        private bool m_isAllowClick = false;

        #endregion

        //IMediatorPlug needs
        public GuideMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public GuideView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.BuidingMenuOpen,
                CmdConstant.GuideClickMonster,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.BuidingMenuOpen:
                    if (!m_isBuildingClickGuide)
                    {
                        return;
                    }
                    if (notification.Body == null)
                    {
                        return;
                    }
                    Int64 buildingIndex = (Int64)notification.Body;
                    BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
                    if (info == null)
                    {
                        return;
                    }
                    if (m_guideDefine != null && info.type == m_guideDefine.guideNodeType)
                    {
                        CityBuildingClick();
                    }
                    break;
                case CmdConstant.GuideClickMonster:
                    if (m_guideDefine != null)
                    {
                        if(m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.AttackMonster, 3) ||
                           m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.AttackMonster, 10))
                        {
                            NextGuide();
                        }
                    }
                    break;
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
                    Debug.Log("坐标变更");
                    m_beforePosition = m_vector3;
                    UpdateArrorTipPos();
                }
            }
            else
            {
                if (m_beforePosition != m_areaTarget.transform.position || SizeIsChange()) //目标节点坐标变更 则需要重设箭头位置
                {
                    Debug.Log("坐标变更");
                    m_beforePosition = m_areaTarget.transform.position;
                    UpdateArrorTipPos();
                }
            }
            if (!m_isAllowClick)
            {
                if (Time.realtimeSinceStartup - m_waitTime > 0.2f)
                {
                    m_isAllowClick = true;
                    view.m_img_fullMask_PolygonImage.gameObject.SetActive(false);
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

        protected override void InitData()
        {
            IsOpenUpdate = true;

            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_guideProxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;

            Transform trans = view.gameObject.transform.Find("pl_content/UI_10009");
            if (trans != null)
            {
                m_effect = trans.gameObject;
            }

            //屏蔽引导按钮
            bool isTest = HotfixUtil.IsDebugable();
            view.m_btn_jump_GameButton.gameObject.SetActive(isTest);
            view.m_btn_neverJump_GameButton.gameObject.SetActive(isTest);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_jump_GameButton.onClick.AddListener(OnJump);
            view.m_btn_neverJump_GameButton.onClick.AddListener(OnNeverJump);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        //跳过引导
        private void OnJump()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.ForceCloseGuide);
        }

        //永远跳过
        private void OnNeverJump()
        {
            GuideProxy proxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
            Int64 noviceGuideStep = 0;
            for (int i = 1; i < 13; i++)
            {
                noviceGuideStep = noviceGuideStep | proxy.ConvertStage(i);
            }
            var sp = new Role_NoviceGuideStep.request();
            sp.noviceGuideStep = noviceGuideStep;
            sp.noviceGuideDetailStep = 0;
            AppFacade.GetInstance().SendSproto(sp);

            OnJump();
        }

        private void Refresh()
        {
            if (view.data == null)
            {
                m_targetParam = null;
                m_guideDefine = null;

                view.m_img_fullMask_PolygonImage.gameObject.SetActive(true);
                view.m_pl_content.gameObject.SetActive(false);
                return;
            }
            else
            {
                m_targetParam = view.data as GuideTargetParam;
                //m_guideDefine = CoreUtils.dataService.QueryRecord<GuideDefine>(608);
                m_guideDefine = m_targetParam.DefineData;
                m_findWay = m_guideDefine.findWay;
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

            }

            //引导特殊处理
            if (m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.CreateScoutCamp, 3)) //引导建造斥候营地
            {
                GuideManager.Instance.IsGuideBuildScoutCamp = true;
            }
            else if (m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.TrainSoldier, 5))//引导领取训练士兵
            {
                GuideManager.Instance.IsGuideSoldierGet = true;
            }else if(m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.SearchMonster, 4))//引导搜索怪物
            {
                GuideManager.Instance.IsGuideFightBarbarian = true;
            }

            m_isAllowClick = false;
            m_waitTime = Time.realtimeSinceStartup;
            view.m_img_fullMask_PolygonImage.gameObject.SetActive(true);

            view.m_pl_content.gameObject.SetActive(true);

            m_areaTargetNodeType = 0;
            if (m_guideDefine.findWay == 1)//ShowUI节点
            {
                m_areaTargetNodeType = m_guideDefine.uiType;
            }
            else if (m_guideDefine.findWay == 2)//HUDLayer节点
            {
                m_areaTargetNodeType = m_guideDefine.guideNodeType;
            }
            else if (m_guideDefine.findWay == 3)//城市建筑节点
            {
                m_areaTargetNodeType = m_guideDefine.uiType;
            }
            else if (m_guideDefine.findWay == 4)//查找野蛮人
            {
                m_areaTargetNodeType = 5;
            }

            UpdateArrorTipPos();

            //追加监听事件
            if (m_areaTargetNodeType == 1) //按钮
            {
                BtnAddListener();
            }
            else if(m_areaTargetNodeType == 2) //list
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

                if (m_guideDefine.guideNodeType == 1)
                {
                    BtnAddListener();
                }
            }

            //播放音效
            if (m_guideDefine.civilization != null && m_guideDefine.civilization.Count > 0)
            {
                PlayerProxy m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                if (m_playerProxy != null)
                {
                    int civilization = (int)m_playerProxy.GetCivilization();
                    for (int i = 0; i < m_guideDefine.civilization.Count; i++)
                    {
                        if (civilization == m_guideDefine.civilization[i])
                        {
                            if (m_guideDefine.heroSound != null && i < m_guideDefine.heroSound.Count)
                            {
                                Debug.LogFormat("播放文明音效：{0}", m_guideDefine.heroSound[i]);
                                PlaySound(m_guideDefine.heroSound[i]);
                                return;
                            }
                        }
                    }
                } 
            }

            if (m_guideDefine.languageSet != null && m_guideDefine.languageSet.Count > 0)
            {
                int language = (int)LanguageUtils.GetLanguage();
                int findIndex = -1;
                for (int i = 0; i < m_guideDefine.languageSet.Count; i++)
                {
                    if (m_guideDefine.languageSet[i] == language)
                    {
                        findIndex = i;
                        break;
                    }
                }
                if (findIndex > -1)
                {
                    if (m_guideDefine.soundClient != null && findIndex < m_guideDefine.soundClient.Count)
                    {
                        //Debug.LogError("播放音效--");
                        PlaySound(m_guideDefine.soundClient[findIndex]);
                    }
                }    
            }
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
                if (m_guideDefine.guideNodeType == 1) //按钮
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
                m_isBuildingClickGuide = true;
                //禁止地图拖动
                WorldCamera.Instance().SetCanDrag(false);
                SetMapPointArea();
            }
            else if (m_areaTargetNodeType == 5)//地图上野蛮人
            {
                //禁止地图拖动
                WorldCamera.Instance().SetCanDrag(false);
                SetMapPointArea();
            }
            else
            {
                Debug.LogErrorFormat("异常Type:{0} guideId:{1}", m_areaTargetNodeType, m_guideDefine.ID);
            }

            //设置箭头位置和旋转角度
            SetArrowPosAngle();

            //设置tip坐标
            SetTipPosAngle();
        }

        private void SetArrowPosAngle()
        {
            //设置箭头坐标 以及 旋转角度
            float rectWidth = m_guideAreaRect.sizeDelta.x;
            float rectHeight = m_guideAreaRect.sizeDelta.y;
            float centerX = view.m_img_target_PolygonImage.transform.localPosition.x - Mathf.Abs(m_guideAreaRect.pivot.x) * rectWidth + rectWidth / 2;
            float centerY = view.m_img_target_PolygonImage.transform.localPosition.y - Mathf.Abs(m_guideAreaRect.pivot.y) * rectHeight + rectHeight / 2;
            int arrowDirection = m_guideDefine.guideArrowheadAr;

            //阿语处理
            if (!LanguageUtils.IsArabic())
            {
                arrowDirection = m_guideDefine.guideArrowhead;
            }
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

        //设置tip位置和角度
        private void SetTipPosAngle()
        {
            if (m_guideDefine.l_guideTipsID > 0)
            {
                float rectWidth = m_guideAreaRect.sizeDelta.x;
                float rectHeight = m_guideAreaRect.sizeDelta.y;
                float centerX = view.m_img_target_PolygonImage.transform.localPosition.x - Mathf.Abs(m_guideAreaRect.pivot.x) * rectWidth + rectWidth / 2;
                float centerY = view.m_img_target_PolygonImage.transform.localPosition.y - Mathf.Abs(m_guideAreaRect.pivot.y) * rectHeight + rectHeight / 2;
                int arrowDirection = m_guideDefine.guideArrowhead;

                view.m_pl_tip.gameObject.SetActive(true);

                view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_guideDefine.l_guideTipsID);

                float tipWidth = view.m_lbl_desc_LanguageText.preferredWidth + 50f;

                RectTransform tipRect = view.m_img_tipbg_PolygonImage.GetComponent<RectTransform>();
                tipRect.sizeDelta = new Vector2(tipWidth, tipRect.sizeDelta.y);

                float offsetPos = 20;

                Vector2 tipPos = Vector2.zero;
                int tipDirection = m_guideDefine.guideTipsPosAr;
                //阿语处理
                if (!LanguageUtils.IsArabic())
                {
                    tipDirection = m_guideDefine.guideTipsPos;
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

            if (m_effect != null)
            {
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
                        return;
                    }
                }

                Vector2 newPos = Vector2.zero;
                newPos.Set(localPos.x - guideRect.pivot.x * targetAreaRect.rect.width + targetAreaRect.rect.width / 2,
                           localPos.y - guideRect.pivot.y * targetAreaRect.rect.height + targetAreaRect.rect.height / 2);
                Vector3 newPos2 = view.gameObject.transform.TransformPoint(newPos);
                m_effect.transform.position = newPos2;
            }
        }

        private void SetCityBuildingArea()
        {
            Vector2 localPos; 
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), m_targetParam.AreaTarget.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform guideRect = view.m_img_target_PolygonImage.GetComponent<RectTransform>();
            guideRect.anchorMin = new Vector2(0.5f, 0.5f);
            guideRect.anchorMax = new Vector2(0.5f, 0.5f);
            guideRect.pivot = new Vector2(0.5f, 0.5f);
            view.m_img_target_PolygonImage.transform.localPosition = localPos;
            guideRect.sizeDelta = new Vector2(100,100);

            if (m_guideDefine.guideNodeType == (int)EnumCityBuildingType.CityWall)
            {
                localPos.Set(localPos.x+90, localPos.y+20);
                view.m_img_target_PolygonImage.transform.localPosition = localPos;
            }

            m_guideAreaRect = guideRect;

            if (m_effect != null)
            {
                m_effect.transform.position = view.m_img_target_PolygonImage.transform.position;
            }
        }

        private void SetMapPointArea()
        {
            Vector2 localPos;
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), m_targetParam.AreaTarget.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform guideRect = view.m_img_target_PolygonImage.GetComponent<RectTransform>();
            guideRect.pivot = new Vector2(0.5f, 0.5f);
            view.m_img_target_PolygonImage.transform.localPosition = localPos;
            guideRect.sizeDelta = new Vector2(100, 100);
            if (m_guideDefine.guideNodeType == (int)EnumCityBuildingType.CityWall)
            {
                guideRect.sizeDelta = new Vector2(280, 180);
                localPos.Set(localPos.x + 40, localPos.y + 35f);
                view.m_img_target_PolygonImage.transform.localPosition = localPos;
            }
            m_guideAreaRect = guideRect;

            if (m_effect != null)
            {
                m_effect.transform.position = view.m_img_target_PolygonImage.transform.position;
            }
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
        }

        private void BtnClick()
        {
            //Debug.LogError("移除按钮事件--");
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

            NextGuide();
        }

        private void CityBuildingClick()
        {
            Debug.Log("城市建筑点击事件引导");
            m_isBuildingClickGuide = false;
            NextGuide();
        }

        private void NextGuide()
        {
            m_areaTarget = null;
            m_isCheckPosChange = false;
            if (m_guideDefine!=null)
            {
                Debug.LogFormat("引导结束:{0}", m_guideDefine.ID);
                if (m_guideDefine.stage == (int)EnumNewbieGuide.TrainSoldier) //第2阶段 士兵训练需要特殊处理
                {
                    // 引导特殊处理
                    if (m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.TrainSoldier, 2))
                    {
                        // 士兵训练引导开启 
                        GuideManager.Instance.IsGuideSoldierTrain = true;
                    }
                    else if (m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.TrainSoldier, 4))
                    {
                        // 士兵训练引导结束
                        GuideManager.Instance.IsGuideSoldierTrain = false;
                    }
                    else if (m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.TrainSoldier, 5))
                    {
                        GuideManager.Instance.IsGuideSoldierGet = false;
                    }
                }
                else if (m_guideDefine.stage == (int)EnumNewbieGuide.CreateScoutCamp)//引导建造斥候营地
                {
                    //引导特殊处理
                    if (m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.CreateScoutCamp, 3))
                    {
                        GuideManager.Instance.IsGuideBuildScoutCamp = false;
                    }
                    else if (m_guideProxy.IdEqual(m_guideDefine.ID, (int)EnumNewbieGuide.CreateScoutCamp, 10))//关闭任务界面
                    {
                        if (!WarFogMgr.IsAllFogOpen())
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_Taskinfo);
                        }
                    }
                }
            }

            //Debug.LogError("下一步");
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

            AppFacade.GetInstance().SendNotification(CmdConstant.NextGuideStep, m_guideDefine);
        }

        //播放音效
        private void PlaySound(string sound)
        {
            CoreUtils.audioService.PlayOneShot(sound, null);
        }
    }
}