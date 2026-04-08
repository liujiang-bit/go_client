// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    FuncGuideGlobalMediator 功能介绍引导
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections.Generic;
using PureMVC.Interfaces;
using Skyunion;
using System;
using Client;
using Data;
using UnityEngine.UI;
using SprotoType;
using DG.Tweening;
using Hotfix;

namespace Game
{
    public enum EnumFuncGuide
    {
        CommanderSkill = 1,     //统帅技能
        CommanderTalent = 2,    //统帅天赋
        Bag = 3,                //背包
        AllianceJoinIntro = 4,  //联盟加入介绍
        AllianceFuncIntro = 5,  //联盟功能介绍
        AllianceMoveCity = 6,   //联盟迁城
        ReceiveVipPoint = 7,    //领取VIP点数引导
        ReceiveVipBox = 8,      //领取VIP专属宝箱
        ReceiveVipBoxIntro = 9, //VIP特别尊享宝箱介绍
        UseSpeedItem = 10,      //加速道具引导
        AgeChangeIntro = 11,    //时代变迁介绍
        MonsterFightFail1 = 12,    //野蛮人等级1
        MonsterFightFail2 = 13,    //野蛮人等级2
        MonsterFightFail3 = 14,    //野蛮人等级2
        MonsterFightFail4 = 15,    //野蛮人等级2
        MonsterFightFail5 = 16,    //野蛮人等级2
        TroopsLack = 17,           //兵力不足提醒
        TechnologyStudy = 18,      //科技研究引导
        Expedition      = 19,      //远征引导
        CollectStone =  20,        //采集石头
        CollectGold = 21,          //采集金矿
        CollectDenar = 22,         //采集宝石
        CollectRes = 23,           //采集资源
        AllianceFlag = 24,         //联盟旗帜
        OtherPlayerCity = 25,      //其他玩家城市
        WonderGroup1 = 26,         //奇观介绍1
        WonderGroup2 = 27,         //奇观介绍2
        WonderGroup3 = 28,         //奇观介绍3
        WonderGroup4 = 29,         //奇观介绍4
        CustomsPass = 30,          //关卡介绍
        Guardian = 31,             //守护者介绍
        BarbarianCity = 32,        //野蛮人城寨介绍
        Rune = 33,                 //符文介绍
        AllianceRes = 34,          //（不可采集的）联盟资源点介绍
        Barbarian = 35,            //野蛮人介绍
        AllianceBuildCreate = 36,  //联盟建筑创建引导
        BuilderHut = 37,           //工人小屋第二队列引导
        GoOnTaskGuide = 38,        //继续任务引导
        MoveCityEffect = 39,       //迁城移动引导光效
    }

    public class FuncGuideGlobalMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "FuncGuideGlobalMediator";

        private FuncGuideProxy m_funcGuideProxy;
        private CityBuildingProxy m_cityBuildingProxy;

        private GuideExDefine m_targetDefine;

        private int m_specialStatus = 0;
        private int m_findStatus;    //查找状态: 0不需要查找 1查找中 2已找到 3剧情
        private int m_findWay;       //查找方式

        private bool m_isDispose;

        private bool m_isOpen = true;

        private GameObject m_maskLayer;
        private int m_hudLayerId;

        private bool m_isGuideSpeedItemUse; //是否引导5分钟加速道具使用

        #endregion

        public FuncGuideGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        //IMediatorPlug needs
        public FuncGuideGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.FuncGuideTrigger,
                CmdConstant.OnNPCDiaglogEnd,
                CmdConstant.NextFuncGuideStep,
                CmdConstant.HideGuideMask,
                CmdConstant.FuncGuideCheck,
                CmdConstant.BuidingMenuOpen,
                CmdConstant.FuncGuideId,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            if (!m_isOpen)
            {
                return;
            }
            switch (notification.Name)
            {
                case CmdConstant.FuncGuideTrigger:
                    int stage = (int)notification.Body;
                    ProcessStageTrigger(stage);
                    break;
                case CmdConstant.OnNPCDiaglogEnd:
                    NPCDiaglogEnd(notification.Body);
                    break;
                case CmdConstant.NextFuncGuideStep:
                    NextGuideStep(notification.Body as GuideExDefine);
                    break;
                case CmdConstant.HideGuideMask:
                    HideMask();
                    break;
                case CmdConstant.FuncGuideCheck:
                    FuncGuideTriggerCheck(notification.Body);
                    break;
                case CmdConstant.BuidingMenuOpen:
                    BuildingMenuOpenProcess(notification.Body);
                    break;
                case CmdConstant.FuncGuideId:
                    FuncGuideIdProcess(notification.Body);
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_funcGuideProxy = AppFacade.GetInstance().RetrieveProxy(FuncGuideProxy.ProxyNAME) as FuncGuideProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_hudLayerId = (int)UILayer.HUDLayer;

            if (CoreUtils.dataService.QueryRecord<ConfigDefine>(0).guideExSwich == 1)
            {
                m_isOpen = true;
            }
            else
            {
                m_isOpen = false;
            }
        }

        public void ClearData()
        {

        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        public override void OnRemove()
        {
            m_isDispose = true;
            m_targetDefine = null;
            m_findStatus = 0;
        }

        public override void Update()
        {
            if (m_findStatus > 1)
            {
                FindTargetNode();
            }
        }

        public override void LateUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }

        #endregion

        private void BuildingMenuOpenProcess(object body)
        {
            if (body == null)
            {
                return;
            }
            Int64 buildingIndex = (Int64)body;
            BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
            if (info == null)
            {
                return;
            }
            if (info.type == (int)EnumCityBuildingType.Academy)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.TechnologyStudy);
            }
        }

        private void FuncGuideTriggerCheck(object body)
        {
            int stage = (int)body;
            if (stage == (int)EnumFuncGuide.UseSpeedItem) // 加速道具引导
            {
                if (m_isGuideSpeedItemUse)
                {
                    return;
                }
                m_isGuideSpeedItemUse = true;
                if (m_targetDefine != null && m_targetDefine.ID == 1003) //使用5分钟加速道具
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideFiveSpeed);
                    return;
                }
            }
            if (m_funcGuideProxy.IsCompletedByStage(stage))
            {
                return;
            }
            if (stage == (int)EnumFuncGuide.ReceiveVipBox) // 领取VIP专属宝箱引导
            {
                UI_Win_VipMediator vipMediator = AppFacade.GetInstance().RetrieveMediator(UI_Win_VipMediator.NameMediator) as UI_Win_VipMediator;
                if (vipMediator == null)
                {
                    return;
                }
                PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                if (!playerProxy.CurrentRoleInfo.vipFreeBox)
                {
                    if (vipMediator.GetClaimableBoxIndex() == vipMediator.GetCurrShowIndex())
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, stage);
                    }
                }
            }
            else if (stage == (int)EnumFuncGuide.ReceiveVipBoxIntro) // 领取VIP专属宝箱引导
            {
                UI_Win_VipMediator vipMediator = AppFacade.GetInstance().RetrieveMediator(UI_Win_VipMediator.NameMediator) as UI_Win_VipMediator;
                if (vipMediator == null)
                {
                    return;
                }
                if (vipMediator.GetClaimableBoxIndex() == vipMediator.GetCurrShowIndex())
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, stage);
                }
            }
            else if (stage == (int)EnumFuncGuide.BuilderHut)//工人小屋第二队列引导
            {
                var workerProxy = AppFacade.GetInstance().RetrieveProxy(WorkerProxy.ProxyNAME) as WorkerProxy;
                if (workerProxy.GetSecendQueueState() == BuildQueueState.LOCK)
                {
                    var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

                    if (bagProxy.GetItemNum(502020001) > 0)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, stage);
                    }
                }
            }
            else if (stage == (int)EnumFuncGuide.MoveCityEffect) // 迁城移动引导-位置光效
            {
                if (m_funcGuideProxy.IsCompletedByStage((int)EnumFuncGuide.AllianceMoveCity))
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, stage);
                }
            }
        }

        private void FuncGuideIdProcess(object body)
        {
            int id = (int)body;
            if (m_targetDefine != null && m_targetDefine.ID == id)
            {
                if (id == 1003)
                {
                    m_findStatus = (int)EnumGuideFindStatus.Finding;
                    FindTargetNode();
                }
            }
        }

        public void NPCDiaglogEnd(object body)
        {
            int dialogId = (int)body;
            if (m_targetDefine != null && m_targetDefine.guideDialog == dialogId)
            {
                if (m_targetDefine.ID == 1701)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideMonsterPowerHide);
                }
                NextGuideStep(m_targetDefine);
            }
        }

        private void NextGuideStep(GuideExDefine currDefine)
        {
            if (currDefine.stage == (int)EnumFuncGuide.AllianceBuildCreate)
            {
                CoreUtils.uiManager.ShowUI(UI.s_AlliancePopGuideAnim);
                return;
            }
            else if (currDefine.stage == (int)EnumFuncGuide.MoveCityEffect)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideMoveCityEffect);
                return;
            }
            int id = currDefine.ID + 1;
            GuideExDefine exDefine = CoreUtils.dataService.QueryRecord<GuideExDefine>(id);
            if (exDefine == null)
            {
                CoreUtils.uiManager.CloseUI(UI.s_funcGuide);
                return;
            }
            m_targetDefine = exDefine;
            StartGuide(exDefine);
        }

        public void ProcessStageTrigger(int stage)
        {
            if (m_funcGuideProxy.IsCompletedByStage(stage))
            {
                return;
            }
            int id = stage * 100 + 1;
            if (m_targetDefine != null && id == m_targetDefine.ID)
            {
                return;
            }
            GuideExDefine exDefine = CoreUtils.dataService.QueryRecord<GuideExDefine>(id);
            if (exDefine == null)
            {
                return;
            }
            m_targetDefine = exDefine;
            StartGuide(exDefine);
        }

        public void StartGuide(GuideExDefine exDefine)
        {
            Debug.LogFormat("功能介绍引导id：{0}", exDefine.ID);
            if (!m_funcGuideProxy.IsCompletedByStage(exDefine.stage))
            {
                //阶段引导结束
                m_funcGuideProxy.RequestRecordGuideStage(exDefine.stage);
            }

            m_funcGuideProxy.RequestRecordGuideId(exDefine.stage, exDefine.ID);

            if (exDefine.guideDialog > 0) //通知播放剧情
            {
                if (exDefine.ID == 601)
                {
                    //判断一下是否是盟主 如果是盟主则不需要接下去引导
                    var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                    if (allianceProxy.HasJionAlliance())
                    {
                        GuildInfoEntity guildInfo = allianceProxy.GetAlliance();
                        if (guildInfo != null)
                        {
                            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                            if (guildInfo.leaderRid == playerProxy.CurrentRoleInfo.rid)
                            {
                                BreakGuide();
                                CoreUtils.uiManager.CloseUI(UI.s_funcGuide);
                                return;
                            }
                        }
                    }
                }
                if (exDefine.ID == 602)
                {
                    m_findStatus = (int)EnumGuideFindStatus.DialogSpecial;
                    m_specialStatus = 1;
                    FindTargetNode();
                    return;
                }
                else if (exDefine.ID == 1701)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideMonsterPowerShow);
                }
                ShowMask();
                CoreUtils.uiManager.CloseUI(UI.s_funcGuide);
                AppFacade.GetInstance().SendNotification(CmdConstant.ShowNPCDiaglog, exDefine.guideDialog);
            }
            else
            {
                m_specialStatus = 0;
                m_findWay = m_targetDefine.findWay;
                if (m_targetDefine.guideStartNext == 1)
                {
                    HideMask();
                    CoreUtils.uiManager.CloseUI(UI.s_funcGuide);
                    m_findStatus = (int)EnumGuideFindStatus.None;
                }
                else
                {
                    m_findStatus = (int)EnumGuideFindStatus.Finding;
                    FindTargetNode();
                }
            }
        }

        //查找目标节点
        public void FindTargetNode()
        {
            if (m_targetDefine == null)
            {
                return;
            }
            if (m_specialStatus > 0)
            {
                SpecialStatusProcess();
                return;
            }
            if (m_findWay == 1) //查找ShowUI界面上的节点
            {
                if (CoreUtils.uiManager.ExistUI(m_targetDefine.uiId))
                {
                    UIInfo info = CoreUtils.uiManager.GetUI(m_targetDefine.uiId);
                    if (info.uiObj == null)
                    {
                        return;
                    }
                    FindUiNode(info.uiObj.transform);
                }
            }
            else if (m_findWay == 2) //查找HUDLayer下面的节点
            {
                RectTransform trans = CoreUtils.uiManager.GetUILayer(m_hudLayerId);
                if (trans == null)
                {
                    return;
                }
                FindHudUiNode(trans);
            }
        }

        private void SpecialStatusProcess()
        {
            if (m_targetDefine.ID == 602)
            {
                if (m_specialStatus > 1)
                {
                    return;
                }
                m_specialStatus = 2;
                var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                GuildInfoEntity guideInfo = allianceProxy.GetAlliance();
                if (guideInfo == null)
                {
                    Debug.Log("引导中断 未发现联盟数据");
                    //中断引导
                    BreakGuide();
                    return;
                }
                var minimapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
                var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                MemberPosInfo memberPos = null;
                minimapProxy.MemberPos.TryGetValue(guideInfo.leaderRid, out memberPos);

                if (memberPos == null)
                {
                    Debug.Log("引导中断 联盟成员数据未找到");
                    //中断引导
                    BreakGuide();
                    return;
                }
                ShowMask();
                //Debug.LogError("pos x:" + memberPos.pos.x + "y:" + memberPos.pos.y);
                //Debug.LogError("pos x:" + memberPos.pos.x / 100 + "y:" + memberPos.pos.y / 100);
                //Debug.LogError("pos x:" + (float)memberPos.pos.x / 100 + "y:" + (float)memberPos.pos.y / 100);
                float dxf = WorldCamera.Instance().getCameraDxf("TacticsToStrategy1");
                WorldCamera.Instance().SetCameraDxf(dxf, 0f, () => { });
                WorldCamera.Instance().ViewTerrainPos(memberPos.pos.x / 100, memberPos.pos.y / 100, 500, () => {
                    if (m_isDispose)
                    {
                        return;
                    }
                    Timer.Register(0.2f, () =>
                    {
                        if (m_isDispose)
                        {
                            return;
                        }
                        if (m_targetDefine == null)
                        {
                            return;
                        }
                        if (m_targetDefine.ID != 602)
                        {
                            return;
                        }
                        AppFacade.GetInstance().SendNotification(CmdConstant.ShowNPCDiaglog, m_targetDefine.guideDialog);
                    });
                });
            }
        }

        //中断引导
        private void BreakGuide()
        {
            m_findStatus = (int)EnumGuideFindStatus.None;
            m_specialStatus = 0;
            m_targetDefine = null;
        }

        //显示遮罩
        private void ShowMask()
        {
            if (m_maskLayer == null)
            {
                RectTransform trans = CoreUtils.uiManager.GetUILayer((int)UILayer.GuideLayer);
                GameObject maskObj = new GameObject("GuideMaskLayer2");
                maskObj.layer = LayerMask.NameToLayer("UI");
                maskObj.transform.SetParent(trans);
                maskObj.transform.SetAsFirstSibling();
                maskObj.AddComponent<Empty4Raycast>();
                m_maskLayer = maskObj;
            }
            m_maskLayer.gameObject.SetActive(true);
        }

        //隐藏遮罩
        private void HideMask()
        {
            if (m_maskLayer != null)
            {
                m_maskLayer.gameObject.SetActive(false);
            }
        }

        #region 节点查找

        private void FindUiNode(Transform parent)
        {
            Transform trans1 = parent.Find(m_targetDefine.path);
            if (trans1 == null)
            {
                return;
            }
            if (!trans1.gameObject.activeInHierarchy)
            {
                return;
            }
            if (m_targetDefine.uiType == 1) //按钮
            {
                m_findStatus = (int)EnumGuideFindStatus.Found;
                GuideTargetParam param = new GuideTargetParam();
                param.AreaTarget = trans1.gameObject;
                param.DefineData2 = m_targetDefine;
                if (!string.IsNullOrEmpty(m_targetDefine.guideEffectPos))
                {
                    Transform node2 = parent.transform.Find(m_targetDefine.guideEffectPos);
                    if (node2 != null)
                    {
                        param.EffectMountTarget = node2.gameObject;
                    }
                }
                CoreUtils.uiManager.ShowUI(UI.s_funcGuide, null, param);
            }
            else if (m_targetDefine.uiType == 6)
            {
                m_findStatus = (int)EnumGuideFindStatus.Found;
                GuideTargetParam param = new GuideTargetParam();
                param.AreaTarget = trans1.gameObject;
                param.DefineData2 = m_targetDefine;
                CoreUtils.uiManager.ShowUI(UI.s_funcGuide, null, param);
            }
            else
            {
                ListView listView = trans1.gameObject.GetComponent<ListView>();
                if (listView == null)
                {
                    return;
                }
                ListView.ListItem list_item = null;
                if (m_targetDefine.listIndex < 0)
                {
                    if (m_targetDefine.ID == 802 || m_targetDefine.ID == 902)
                    {
                        UI_Win_VipMediator vipMediator = AppFacade.GetInstance().RetrieveMediator(UI_Win_VipMediator.NameMediator) as UI_Win_VipMediator;
                        if (vipMediator != null)
                        {
                            int index = vipMediator.GetClaimableBoxIndex();
                            list_item = listView.GetItemByIndex(index);
                        }
                    }
                    else if (m_targetDefine.ID == 1003)
                    {
                        AddSpeedMediator addSpeedMediator = AppFacade.GetInstance().RetrieveMediator(AddSpeedMediator.NameMediator) as AddSpeedMediator;
                        if (addSpeedMediator != null)
                        {
                            int index = addSpeedMediator.GetSpeedItemIndex();
                            list_item = listView.GetItemByIndex(index);
                        }
                    }
                }
                else
                {
                    list_item = listView.GetItemByIndex(m_targetDefine.listIndex);
                }
                if (list_item == null)
                {
                    return;
                }
                if (list_item.go == null)
                {
                    return;
                }
                Transform node1 = list_item.go.transform.Find(m_targetDefine.guideNode);
                if (node1 == null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(m_targetDefine.guideEffectPos))
                {
                    Transform node2 = list_item.go.transform.Find(m_targetDefine.guideEffectPos);
                    if (node2 == null)
                    {
                        return;
                    }
                    m_findStatus = (int)EnumGuideFindStatus.Found;
                    GuideTargetParam param = new GuideTargetParam();
                    param.AreaTarget = node1.gameObject;
                    param.EffectMountTarget = node2.gameObject;
                    param.DefineData2 = m_targetDefine;
                    param.ListNode = trans1.gameObject;
                    CoreUtils.uiManager.ShowUI(UI.s_funcGuide, null, param);
                }
                else
                {
                    m_findStatus = (int)EnumGuideFindStatus.Found;
                    GuideTargetParam param = new GuideTargetParam();
                    param.AreaTarget = node1.gameObject;
                    param.EffectMountTarget = node1.gameObject;
                    param.DefineData2 = m_targetDefine;
                    param.ListNode = trans1.gameObject;
                    CoreUtils.uiManager.ShowUI(UI.s_funcGuide, null, param);
                }
            }
        }

        private void FindHudUiNode(Transform parent)
        {
            //获取父节点
            Transform trans1 = parent.Find(m_targetDefine.path);
            if (trans1 == null)
            {
                return;
            }
            Transform trans2 = parent.Find(m_targetDefine.guideNode);
            if (trans2 == null)
            {
                return;
            }
            if (!trans2.gameObject.activeInHierarchy)
            {
                return;
            }
            //获取父节点的缩放比率
            Vector3 localScale = trans1.localScale;
            if (m_targetDefine.guideNodeType == 1) //按钮
            {
                m_findStatus = (int)EnumGuideFindStatus.Found;
                GuideTargetParam param = new GuideTargetParam();
                param.AreaTarget = trans2.gameObject;
                param.DefineData2 = m_targetDefine;
                param.ScaleStatus = 2;
                param.ScaleParentTrans = trans1;
                CoreUtils.uiManager.ShowUI(UI.s_funcGuide, null, param);
            }
        }

        #endregion 
    }
}