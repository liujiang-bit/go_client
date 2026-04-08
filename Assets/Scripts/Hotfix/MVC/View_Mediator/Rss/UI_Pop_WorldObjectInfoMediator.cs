// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Pop_WorldObjectInfoMediator
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
using Hotfix;
using System;
using Data;

namespace Game
{
    public class UI_Pop_WorldObjectInfoMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Pop_WorldObjectInfoMediator";

        private RssProxy m_RssProxy;
        private WorldMapObjectProxy m_worldProxy;
        private TroopProxy m_TroopProxy;
        private CityBuildingProxy m_CityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private SoldierProxy m_soldierProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;

        public MapObjectInfoEntity m_rssData;

        private Timer m_timer;
        private int m_totalCollectTime;
        private Int64 m_canCollectNum;//采集时间重置后总的采集个数
        private Int64 m_totalCollectNum;//总的采集个数
        private float m_residueTotalWeight; //采集时间重置后的剩余总负载
        private float m_TotalWeight; //剩余总负载
        private float m_collectSpeed; //当前采集速度
        private Int64 m_collectBeforeTime; //变速前所花费采集时间
        private float m_collectBeforeWeight; //变速前采集的总负载
        private int m_currResWeight; //当前资源负载

        private bool m_dispose;
        private bool m_firstShow = true;
        private MapObjectInfoEntity rsData;

        #endregion

        //IMediatorPlug needs
        public UI_Pop_WorldObjectInfoMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Pop_WorldObjectInfoView view;
        private int rssId;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.MapObjectChange,
                CmdConstant.MapObjectRemove,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.MapObjectChange: //资源点信息更新
                    {
                        MapObjectInfoEntity info = notification.Body as MapObjectInfoEntity;
                        if (m_rssData == null)
                        {
                            return;
                        }
                        if (info == null)
                        {
                            return;
                        }

                        if (info.objectId != m_rssData.objectId)
                        {
                            return;
                        }

                        Refresh();
                    }
                
                    break;
                case CmdConstant.MapObjectRemove:
                    {
                        MapObjectInfoEntity info = notification.Body as MapObjectInfoEntity;
                        if (m_rssData == null)
                        {
                            return;
                        }
                        if (info == null)
                        {
                            return;
                        }

                        if (info.objectId != m_rssData.objectId)
                        {
                            return;
                        }

                        CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldInfo);
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
        }

        public override void WinClose()
        {
        }

        public override void OnRemove()
        {
            m_dispose = true;
            CancelTimer();
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_CityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_playerAttributeProxy =
                AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_RallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            Init();
        }

        private void Init()
        {
            if (view.data == null)
            {
                return;
            }

            string[] s = view.data.ToString().Split('_');
            int id = int.Parse(s[1]);
            rssId = id;
            rsData = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            SetActive(s[0]);
            switch (s[0])
            {
                case "RssItem":
                case "RssItemLod":
                    m_rssData = m_worldProxy.GetWorldMapObjectByobjectId(id);
                    if (m_rssData != null)
                    {
                        if (m_rssData.resourceGatherTypeDefine == null)
                        {
                            return;
                        }

                        //功能介绍引导
                        int resType = m_rssData.resourceGatherTypeDefine.type;
                        if (resType == (int)EnumResType.Food || resType == (int)EnumResType.Wood || resType == (int)EnumResType.Stone ||
                            resType == (int)EnumResType.Gold || resType == (int)EnumResType.Diamond)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.CollectRes);
                        }

                        ChangeWinPos();
                        CalCollectData();
                        view.m_lbl_name_LanguageText.text =
                            LanguageUtils.getText(m_rssData.resourceGatherTypeDefine.l_nameId);
                        view.m_lbl_recommend_LanguageText.text = "";
                        ClientUtils.LoadSprite(
                            view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_IconAndTime.m_img_icon_PolygonImage,
                            m_rssData.resourceGatherTypeDefine.icon);
                        ClientUtils.LoadSprite(view.m_UI_Item_WorldObjInfoTCollect.m_img_icon_PolygonImage,
                            m_rssData.resourceGatherTypeDefine.icon);
                        view.m_btn_descinfo_GameButton.onClick.AddListener(() => { ShowTips(true, m_rssData); });
                        view.m_btn_descBack_GameButton.onClick.AddListener(() => { ShowTips(false, m_rssData); });
                        view.m_lbl_position_LanguageText.text = PosHelper.FormatServerPos(m_rssData.objectPos);


                        view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line1.m_lbl_title_LanguageText.text =
                            LanguageUtils.getText(500004);
                        view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line2.m_lbl_title_LanguageText.text =
                            LanguageUtils.getText(500005);
                        view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line3.m_lbl_title_LanguageText.text =
                            LanguageUtils.getText(500006);

                        view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_rssData.resourceGatherTypeDefine.l_descId);

                        RefreshAllianceName();
                        SetBtnLayerVisible();
                        RefreshResTotal();
                        RefreshName();
                        IsStartCountDown();
                    }

                    break;
            }
        }

        private void RefreshReturnOrCollect()
        {
            if (m_rssData.rssPointStateType == RssPointState.CollectedByMe) //返回
            {
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_1gbtn1.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500010);
            }
            else if (m_rssData.rssPointStateType == RssPointState.None || m_rssData.rssPointStateType == RssPointState.Uncollected)//采集
            {
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_1gbtn1.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500007);
            }
        }

        private void ShowTips(bool isshow, MapObjectInfoEntity rssData)
        {
            view.m_pl_description_Animator.gameObject.SetActive(isshow);
            view.m_pl_normalInfo_Animator.gameObject.SetActive(!isshow);

            if (isshow)
            {
                view.m_pl_description_Animator.Play("Show");
            }
            else
            {
                view.m_pl_normalInfo_Animator.Play("Show");
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_1gbtn1.m_btn_languageButton_GameButton.onClick.AddListener(
                OnBtnGatherClick);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn1.m_btn_languageButton_GameButton.onClick.AddListener(
                OnBtnSpyOnClick);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(
                OnBtnAttackClick);
            view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line2.m_btn_more_GameButton.onClick.AddListener(() =>
            {
                Debug.LogError("点击查看采集者");
            });
        }

        protected override void BindUIData()
        {
            view.m_UI_Common_PopFun.InitSubView(rsData);
        }

        //采集
        private void OnBtnGatherClick()
        {
            if (rsData == null)
            {
                Debug.Log("采集点消失了");
                CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldInfo);
                return;
            }

            //召回
            if (rsData.rssPointStateType == RssPointState.CollectedByMe)
            {
                OnReturn();
                return;
            }

            CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldInfo);
            FightHelper.Instance.Gather(rssId);
        }

        //召回
        private void OnReturn()
        {
            //播放音效
            var armyInfo = m_TroopProxy.GetArmyByIndex((int)m_rssData.armyIndex);
            if (armyInfo != null)
            {
                var heroConfig = CoreUtils.dataService.QueryRecord<HeroDefine>((int)armyInfo.mainHeroId);
                if(heroConfig != null)
                {
                    CoreUtils.audioService.PlayOneShot(heroConfig.voiceMove);
                }
            }

            var sp = new Map_March.request();
            sp.armyIndex = m_rssData.armyIndex;
            sp.targetType = 5;
            AppFacade.GetInstance().SendSproto(sp);

            CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldInfo);
        }

        //攻击
        private void OnBtnAttackClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldInfo);
            FightHelper.Instance.Attack(rssId);
     
        }

        //侦查
        private void OnBtnSpyOnClick()
        {
            if (rsData == null)
            {
                return;
            }

            CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldInfo);           
            FightHelper.Instance.Scout(rsData.objectPos.x,rsData.objectPos.y, (int)rsData.objectId);

        }

        private void SetSpyOnData(MapObjectInfoEntity rssData)
        {

        }


        private void SetActive(string name)
        {
            view.m_UI_Item_WorldObjInfoTCollect.gameObject.SetActive(name == "RssItem" || name == "RssItemLod");
            view.m_UI_Item_WorldObjectPopBtns.m_pl_1g.gameObject.SetActive(
                name == "RssItem" || name == "RssItemLod");
            view.m_UI_Item_WorldObjectPopBtns.m_pl_2g_GridLayoutGroup.gameObject
                .SetActive(name == "BarbarianFormation");
        }

        // 计算负载量
        private void CalCollectData()
        {
            if (m_rssData.rssPointStateType == RssPointState.None ||
                m_rssData.rssPointStateType == RssPointState.Uncollected)
            {
                return;
            }

            //每秒采集速度
            m_collectSpeed = (float) m_rssData.collectSpeed / 10000;
            m_collectSpeed = (m_collectSpeed == 0) ? 1 : m_collectSpeed;

            //当前资源单个负载
            m_currResWeight = 0;
            ResourceGatherTypeDefine define =
                CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>((int) m_rssData.resourceId);
            if (define != null)
            {
                m_currResWeight = m_RssProxy.GetResWeight(define.type);
            }
            else
            {
                Debug.LogErrorFormat("ResourceGatherTypeDefine not find:{0}", m_rssData.resourceId);
            }

            m_currResWeight = m_currResWeight == 0 ? 1 : m_currResWeight;

            //变速前已采集负载
            m_collectBeforeTime = 0;
            m_collectBeforeWeight = 0;
            if (m_rssData.collectSpeeds != null)
            {
                for (int i = 0; i < m_rssData.collectSpeeds.Count; i++)
                {
                    float collectVal = (float) m_rssData.collectSpeeds[i].collectSpeed / 10000 *
                                       m_rssData.collectSpeeds[i].collectTime;
                    m_collectBeforeWeight = m_collectBeforeWeight + m_currResWeight * collectVal;
                    m_collectBeforeTime = m_collectBeforeTime + m_rssData.collectSpeeds[i].collectTime;
                }
            }

            ArmyInfoEntity armyInfo = m_TroopProxy.GetArmyByIndex(m_rssData.armyIndex);
            if (m_playerProxy.CurrentRoleInfo.rid == m_rssData.collectRid && armyInfo != null)
            {
                //总负载
                Int64 armyWeight = 0;
                if (armyInfo.soldiers != null)
                {
                    foreach (var soldier in armyInfo.soldiers.Values)
                    {
                        int tempId = m_soldierProxy.GetTemplateId((int) soldier.type, (int) soldier.level);
                        ArmsDefine define2 = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                        armyWeight = armyWeight + define2.capacity * soldier.num;
                    }
                }

             //   float multi = (float) (1 + m_playerAttributeProxy.GetCityAttribute(attrType.troopsSpaceMulti).value);
               float multi = (1 + m_playerAttributeProxy.GetTroopAttribute(armyInfo.mainHeroId, armyInfo.deputyHeroId, attrType.troopsSpaceMulti).value);

                Int64 totalWeight = (int) Mathf.Floor(armyWeight * multi);

                //已使用负载
                float usedWeight = 0;
                if (armyInfo.resourceLoads != null)
                {
                    ResourceGatherTypeDefine define1 = null;
                    for (int i = 0; i < armyInfo.resourceLoads.Count; i++)
                    {
                        define1 = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(
                            (int) armyInfo.resourceLoads[i].resourceTypeId);
                        if (define1 != null)
                        {
                            usedWeight = usedWeight +
                                         m_RssProxy.GetResWeight(define1.type) * armyInfo.resourceLoads[i].load;
                        }
                    }
                }

                //剩余总负载
                m_residueTotalWeight = totalWeight - usedWeight - m_collectBeforeWeight;
                m_TotalWeight = totalWeight - usedWeight ;

                //剩余可采集个数
                 m_totalCollectNum = (Int64) Mathf.Floor(m_TotalWeight / m_currResWeight);
                Int64 canCollectNum = (Int64) Mathf.Floor(m_residueTotalWeight / m_currResWeight);
                canCollectNum = (Int64) Mathf.Min(canCollectNum, m_rssData.resourceAmount);
                m_canCollectNum = canCollectNum;
                //采集需要的总时间
                m_totalCollectTime = (int) Mathf.Floor((float) canCollectNum / m_collectSpeed);

                Debug.LogFormat("总负载：{0}", totalWeight);
                Debug.LogFormat("已使用负载：{0}", usedWeight);
                Debug.LogFormat("开始采集时间:{0}", m_rssData.collectTime);
                Debug.LogFormat("采集需要的总时间：{0}", m_totalCollectTime);
            }
            else
            {
                m_canCollectNum = m_rssData.resourceAmount;
                m_totalCollectNum = m_rssData.resourceAmount;
            }

            Debug.LogFormat("剩余可采集个数：{0}", m_canCollectNum);
            Debug.LogFormat("采集速度:{0}", m_rssData.collectSpeed);
            Debug.LogFormat("当前资源负载：{0}", m_currResWeight);
            Debug.LogFormat("加速前已采集负载：{0}", m_collectBeforeWeight);
        }

        private void RefreshCountDown(bool isFirst = false)
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 costTime = serverTime - m_rssData.collectTime - m_collectBeforeTime;
            float collectWeight = costTime * m_collectSpeed * m_currResWeight + m_collectBeforeWeight;
            Int64 collectNum = (Int64) Mathf.Floor(collectWeight / m_currResWeight);

            collectNum = (collectNum > m_totalCollectNum) ? m_totalCollectNum : collectNum;
            if (m_rssData.rssPointStateType == RssPointState.CollectedByMe) // 自己
            {
                Int64 residueTime = m_totalCollectTime - costTime;
                residueTime = (residueTime <= 0) ? 0 : residueTime;

                //总量刷新
                view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line1.m_lbl_content_LanguageText.text =
                    ClientUtils.FormatComma(m_rssData.resourceAmount - collectNum);
                if (m_rssData.resourceGatherTypeDefine.type == (int) EnumResType.Diamond) //宝石
                {
                    view.m_UI_Item_WorldObjInfoTCollect.m_lbl_time_LanguageText.text =
                        LanguageUtils.getTextFormat(500009, ClientUtils.FormatCountDown((int) residueTime));
                    view.m_UI_Item_WorldObjInfoTCollect.m_lbl_count_diamond_LanguageText.text =
                        LanguageUtils.getTextFormat(500008, ClientUtils.FormatComma(collectNum),
                            ClientUtils.FormatComma(m_totalCollectNum));
                }
                else //其他资源 切换显示
                {
                    bool isChange = (costTime % 5 == 0) ? true : false;
                    if (isChange || isFirst)
                    {
                        isChange = !view.m_UI_Item_WorldObjInfoTCollect.m_lbl_time_LanguageText.gameObject.activeSelf;
                        view.m_UI_Item_WorldObjInfoTCollect.m_lbl_time_LanguageText.gameObject.SetActive(isChange);
                        view.m_UI_Item_WorldObjInfoTCollect.m_lbl_count_LanguageText.gameObject.SetActive(!isChange);
                    }

                    view.m_UI_Item_WorldObjInfoTCollect.m_lbl_time_LanguageText.text =
                        LanguageUtils.getTextFormat(500009, ClientUtils.FormatCountDown((int) residueTime));
                    view.m_UI_Item_WorldObjInfoTCollect.m_lbl_count_LanguageText.text =
                        LanguageUtils.getTextFormat(500008, ClientUtils.FormatComma(collectNum),
                            ClientUtils.FormatComma(m_totalCollectNum));
                }

                view.m_UI_Item_WorldObjInfoTCollect.m_pb_bar_GameSlider.value = (float)collectNum / m_totalCollectNum;
            }
            else if (m_rssData.rssPointStateType == RssPointState.CollectedByally) //盟友
            {
            }
            else
            {
                view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line1.m_lbl_content_LanguageText.text =
                    ClientUtils.FormatComma(m_rssData.resourceAmount - collectNum);
            }

            if (collectNum == m_totalCollectNum) //采集结束
            {
                Debug.Log("采集结束");
              //  CancelTimer();
              //  CollectEndRefresh();
            }
        }

        private void UpdateCountDown()
        {
            RefreshCountDown();
        }

        //采集结束
        private void CollectEndRefresh()
        {
            view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line1.m_lbl_content_LanguageText.text =
                m_rssData.resourceAmount == 0
                    ? ClientUtils.FormatComma(m_rssData.resourceGatherTypeDefine.resAmount)
                    : ClientUtils.FormatComma(m_rssData.resourceAmount);
            view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line2.m_lbl_content_LanguageText.text =
                LanguageUtils.getText(570029);

            view.m_UI_Item_WorldObjInfoTCollect.m_pb_bar_GameSlider.gameObject.SetActive(false);
            view.m_UI_Item_WorldObjectPopBtns.m_pl_1g.gameObject.SetActive(true);
            view.m_UI_Item_WorldObjectPopBtns.m_pl_2g_GridLayoutGroup.gameObject.SetActive(false);
            view.m_UI_Item_WorldObjectPopBtns.m_pl_3g_ArabLayoutCompment.gameObject.SetActive(false);
        }

        private void SetBtnLayerVisible()
        {
            //注:召回 和 采集 共用按钮
            view.m_UI_Item_WorldObjInfoTCollect.m_pb_bar_GameSlider.gameObject.SetActive(m_rssData.rssPointStateType == RssPointState.CollectedByMe);
            view.m_UI_Item_WorldObjectPopBtns.m_pl_1g.gameObject.SetActive(m_rssData.rssPointStateType == RssPointState.Uncollected || m_rssData.rssPointStateType == RssPointState.CollectedByMe);
            view.m_UI_Item_WorldObjectPopBtns.m_pl_2g_GridLayoutGroup.gameObject.SetActive(m_rssData.rssPointStateType == RssPointState.CollectedNoByally);
            view.m_UI_Item_WorldObjectPopBtns.m_pl_3g_ArabLayoutCompment.gameObject.SetActive(false);
            if (m_rssData.rssPointStateType == RssPointState.CollectedNoByally)
            {
                view.m_UI_Item_WorldObjectPopBtns.SetAttackFightActive();
            }
            else if (m_rssData.rssPointStateType == RssPointState.CollectedByMe)
            {
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.gameObject.SetActive(false);
            }

            RefreshReturnOrCollect();
        }

        private void Refresh()
        {
            CancelTimer();

            SetBtnLayerVisible();
            RefreshResTotal();
            RefreshName();
            RefreshAllianceName();

            if (m_rssData.collectRid > 0)
            {
                CalCollectData();
            }

            IsStartCountDown();
        }

        private void RefreshResTotal()
        {
            //资源剩余总量 
            string str = m_rssData.resourceAmount == 0
                ? ClientUtils.FormatComma(m_rssData.resourceGatherTypeDefine.resAmount)
                : ClientUtils.FormatComma(m_rssData.resourceAmount);
            view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line1.m_lbl_content_LanguageText.text = str;
        }

        private void RefreshName()
        {
            //名称
            string name = "";
            if (m_rssData.collectRid <= 0)
            {
                name = LanguageUtils.getText(570029);
            }
            else
            {
                name = string.IsNullOrEmpty(m_rssData.guildAbbName) ? m_rssData.name : LanguageUtils.getTextFormat(300030, m_rssData.guildAbbName, m_rssData.name);
            }
            view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line2.m_lbl_content_LanguageText.text = name;
        }

        private void RefreshAllianceName()
        {
            view.m_UI_Item_WorldObjInfoTCollect.m_UI_Item_line3.m_lbl_content_LanguageText.text =
                string.IsNullOrEmpty(m_rssData.resourceGuildAbbName) ? LanguageUtils.getText(570029) : LanguageUtils.getTextFormat(730138, m_rssData.resourceGuildAbbName);
        }

        private void IsStartCountDown()
        {
            if (!(m_rssData.rssPointStateType == RssPointState.None ||
                  m_rssData.rssPointStateType == RssPointState.Uncollected))
            {
                RefreshCountDown(true);
                m_timer = Timer.Register(1.0f, UpdateCountDown, null, true, true,view.vb);
            }
        }

        //改变窗口位置
        private void ChangeWinPos()
        {
            if (m_rssData.gameobject == null)
            {
                return;
            }

            //屏幕坐标转界面局部坐标
            Vector2 localPos;
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(),
                m_rssData.gameobject.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                view.m_pl_pos.gameObject.GetComponent<RectTransform>(),
                pos,
                CoreUtils.uiManager.GetUICamera(),
                out localPos);

            RectTransform viewRect = view.gameObject.GetComponent<RectTransform>();
            var rect = view.m_pl_content_Animator.gameObject.GetComponent<RectTransform>().rect;

            float diffNum = 50f;

            // 左
            if (localPos.x < viewRect.rect.width / 2)
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    localPos.y = localPos.y - (rect.height / 2) - diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideTop_PolygonImage.transform.localPosition = new Vector2(offset,
                            view.m_img_arrowSideTop_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }

                    view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                            view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }

                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x + (rect.width / 2) + diffNum;
                    view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                }
            }
            // 右
            else
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.y - (viewRect.rect.height - rect.height / 2);
                        view.m_img_arrowSideR_PolygonImage.transform.localPosition = new Vector2(
                            view.m_img_arrowSideR_PolygonImage.transform.localPosition.x,
                            offset);
                        view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);

                        localPos.x = localPos.x - (rect.width / 2) - diffNum;
                        localPos.y = viewRect.rect.height - rect.height / 2;
                    }
                    else
                    {
                        localPos.y = localPos.y - (rect.height / 2) - diffNum;
                        view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                    }
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.x - (viewRect.rect.width - rect.width / 2);
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                            view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = (viewRect.rect.width - rect.width / 2);
                    }

                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x - (rect.width / 2) - diffNum;
                    view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                }
            }

            view.m_pl_content_Animator.gameObject.GetComponent<RectTransform>().transform.localPosition = localPos;
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        #endregion
    }
}