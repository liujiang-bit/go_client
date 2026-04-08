// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    ArmyTrainStatusGlobalMediator 士兵训练状态
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

namespace Game
{
    public class SoldierTrainStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        public const int None = 0;
        /// <summary>
        /// 训练中
        /// </summary>
        public const int Training = 1;
        /// <summary>
        /// 已完成训练 尚未收取 
        /// </summary>
        public const int Finished = 2;
    }

    public class ArmyTrainStatusItem
    {
        public Int64 BuildingIndex;
        public HUDUI Hudui;
        public UIPopTrainOnBuildingView HudView;
        public QueueInfo QueueData;
    }

    public class ArmyTrainStatusGlobalMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "ArmyTrainStatusGlobalMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private SoldierProxy m_soldierProxy;

        public Dictionary<Int64, HUDUI> m_huds = new Dictionary<Int64, HUDUI>();
        private Dictionary<Int64, GameObject> m_freeEffectDic = new Dictionary<Int64, GameObject>();
        private Dictionary<Int64, bool> m_waitShowFreeEffectDic = new Dictionary<Int64, bool>();

        private List<int> m_idList = new List<int>() { (int)EnumCityBuildingType.Barracks ,
                                                 (int)EnumCityBuildingType.Stable,
                                                 (int)EnumCityBuildingType.ArcheryRange,
                                                 (int)EnumCityBuildingType.SiegeWorkshop};

        private Dictionary<Int64, QueueInfo> m_cacheQueue = new Dictionary<Int64, QueueInfo>();

        #endregion

        public ArmyTrainStatusGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public ArmyTrainStatusGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.CityBuildinginfoChange,
                CmdConstant.CityBuildingLevelUP,        //建筑创建升级
                CmdConstant.armyQueueChange,            //训练队列信息变更
                CmdConstant.CityBuildingDone,           //城市建筑创建完成
                CmdConstant.InSoldierInfoChange,        //城内待命士兵信息变更
                Role_AwardArmy.TagName,                 //领取训练成功的士兵通知
                Role_TrainEnd.TagName,                  //取消训练
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.CityBuildinginfoChange:
                    if (notification.Body != null)
                    {
                        List<BuildingInfoEntity> list = notification.Body as List<BuildingInfoEntity>;
                        for(int i=0; i<list.Count;i++)
                        {
                            if (list[i].buildingIndex > 0)
                            {
                                BuildingInfoEntity upInfo = m_cityBuildingProxy.GetBuildingInfoByindex(list[i].buildingIndex);
                                if (upInfo != null)
                                {
                                    if (upInfo.type == (int)EnumCityBuildingType.Barracks || upInfo.type == (int)EnumCityBuildingType.Stable ||
                                        upInfo.type == (int)EnumCityBuildingType.ArcheryRange || upInfo.type == (int)EnumCityBuildingType.SiegeWorkshop)
                                    {
                                        UpdateBuildingTrainStatus(upInfo);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.CityBuildingLevelUP:
                    Int64 buildingIndex2 = (Int64)notification.Body;
                    BuildingInfoEntity info2 = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex2);
                    if (info2 != null)
                    {
                        if (info2.type == (int)EnumCityBuildingType.Barracks || info2.type == (int)EnumCityBuildingType.Stable ||
                            info2.type == (int)EnumCityBuildingType.ArcheryRange || info2.type == (int)EnumCityBuildingType.SiegeWorkshop)
                        {
                            UpdateBuildingTrainStatus(info2);
                        }
                    }
                    break;
                case CmdConstant.armyQueueChange:
                    RoleInfo test1 = notification.Body as RoleInfo;
                    QueueChange(test1);
                    break;
                case CmdConstant.CityBuildingDone:
                    UpdateAllHudStatus();
                    break;
                case Role_AwardArmy.TagName:
                    var response = notification.Body as Role_AwardArmy.response;
                    if (response == null)
                    {
                        return;
                    }
                    if (response.soldiers == null)
                    {
                        return;
                    }
                    foreach (var data in response.soldiers)
                    {
                        ReceiveSoldierTip(data.Value);
                    }
                    break;
                case Role_TrainEnd.TagName:
                    var response1 = notification.Body as Role_TrainEnd.response;
                    AppFacade.GetInstance().SendNotification(CmdConstant.ArmyTrainEnd, response1.buildingIndex);
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        public override void OnRemove()
        {

        }

        public override void Update()
        {

        }

        public override void LateUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }
        #endregion

        private void QueueChange(RoleInfo info)
        {
            if (info != null && info.armyQueue != null)
            {
                foreach (var data in info.armyQueue)
                {
                    Debug.LogFormat("训练部队 firstTime:{0} finishTime:{1} beginTime:{2} costTime:{3} serverTime:{4} time:{5}",
                                    data.Value.firstFinishTime, data.Value.finishTime, data.Value.beginTime, data.Value.finishTime - data.Value.beginTime,
                                    ServerTimeModule.Instance.GetServerTime(), Time.realtimeSinceStartup);

                    BuildingInfoEntity info1 = m_cityBuildingProxy.GetBuildingInfoByindex(data.Value.buildingIndex);
                    if (info1 != null)
                    {
                        if (data.Value.finishTime < 0 && data.Value.armyNum > 0) //领取士兵 走一下表现
                        {
                            SoldierGetShow(data.Value, info1);
                        }
                        else
                        {
                            //判断一下是否加速                            
                            if (m_cacheQueue.ContainsKey(data.Key))
                            {
                                if (data.Value.beginTime == m_cacheQueue[data.Key].beginTime && data.Value.beginTime > 0 &&
                                    data.Value.finishTime < m_cacheQueue[data.Key].finishTime)
                                {
                                    if (data.Value.finishTime <= ServerTimeModule.Instance.GetServerTime())
                                    {
                                        //tip提醒
                                        FinishedTipShow(data.Value.buildingIndex);
                                    }
                                }
                            }
                            UpdateBuildingTrainStatus(info1);
                        }
                    }
                    m_cacheQueue[data.Key] = data.Value;
                }
            }
        }

        private void UpdateAllHudStatus()
        {
            for (int i = 0; i < m_idList.Count; i++)
            {
                BuildingInfoEntity info = m_cityBuildingProxy.GetBuildingInfoByType(m_idList[i]);
                if (info != null)
                {
                    UpdateBuildingTrainStatus(info);
                }
            }
        }

        //士兵领取表现
        private void SoldierGetShow(QueueInfo queue, BuildingInfoEntity info)
        {
            GameObject go = CityObjData.GeBuildTipTargetGameObject(info.buildingIndex);
            if (go == null)
            {
                return;
            }
            if (m_huds.ContainsKey(queue.buildingIndex) && m_huds[queue.buildingIndex].uiObj != null)
            {
                //刷新显示
                RefreshStatus(m_huds[info.buildingIndex]);
            }
            else
            {
                HudCreate(go, info);
            }
        }

        private void UpdateBuildingTrainStatus(BuildingInfoEntity info)
        {
            GameObject go = CityObjData.GeBuildTipTargetGameObject(info.buildingIndex);
            if (go == null)
            {
                return;
            }
            if (m_huds.ContainsKey(info.buildingIndex) && m_huds[info.buildingIndex].uiObj != null) //直接刷新
            {
                RefreshStatus(m_huds[info.buildingIndex]);
            }
            else //创建
            {
                HudCreate(go, info);
            }
        }

        private void HudCreate(GameObject go, BuildingInfoEntity info)
        {
            HUDUI curHud = HUDUI.Register(UIPopTrainOnBuildingView.VIEW_NAME, typeof(UIPopTrainOnBuildingView), HUDLayer.city, go);
            curHud.SetData(info);
            curHud.SetScaleAutoAnchor(true);
            curHud.SetCameraLodDist(0, 270f);
            curHud.SetInitCallback(HudCreateCallback);
            ClientUtils.hudManager.ShowHud(curHud);
            m_huds[info.buildingIndex] = curHud;
        }

        private void HudCreateCallback(HUDUI hud)
        {
            if (hud.targetObj == null || hud.uiObj == null)
            {
                Debug.LogWarning("节点被干掉了");
                return;
            }
            UIPopTrainOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UIPopTrainOnBuildingView>(hud.uiObj);
            hudView.m_btn_get_GameButton.onClick.AddListener(() =>
            {
                OnClickGet(hud);
            });
            RefreshStatus(hud);
        }

        private void TimeEndCallback(Int64 buildingIndex)
        {
            FinishedTipShow(buildingIndex);
            if (m_huds.ContainsKey(buildingIndex))
            {
                RefreshStatus(m_huds[buildingIndex]);
            }
        }

        private void FinishedTipShow(Int64 buildingIndex)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.ArmyTrainEnd, buildingIndex);
            if (m_playerProxy.TrainQueueMap.ContainsKey(buildingIndex))
            {
                int tempId = m_soldierProxy.GetTemplateId((int)m_playerProxy.TrainQueueMap[buildingIndex].armyType, (int)m_playerProxy.TrainQueueMap[buildingIndex].newArmyLevel);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                if (define != null)
                {
                    string desc = LanguageUtils.getTextFormat(192006, m_playerProxy.TrainQueueMap[buildingIndex].armyNum, LanguageUtils.getText(define.l_armsID));
                    Tip.CreateTip(desc).Show();
                }
            }
        }

        private void RefreshStatus(HUDUI hud)
        {
            if (hud.targetObj == null || hud.uiObj == null) //还没创建好 直接return
            {
                return;
            }
            BuildingInfoEntity buildingInfo = hud.data as BuildingInfoEntity;
            if (buildingInfo == null)
            {
                return;
            }

            int status = m_cityBuildingProxy.GetTrainStatus(buildingInfo.buildingIndex);

            UIPopTrainOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UIPopTrainOnBuildingView>(hud.uiObj);
            if (status == SoldierTrainStatus.None) //空闲
            {
                hudView.m_pl_none.gameObject.SetActive(true);
                hudView.m_pl_train.gameObject.SetActive(false);
                hudView.m_pl_get.gameObject.SetActive(false);

                if (m_cityBuildingProxy.IsUpgrading(buildingInfo.buildingIndex))//正在升级 建造 跳过处理
                {
                    hud.Close();
                    HideFreeEffect(buildingInfo.buildingIndex);
                    return;
                }
                ShowFreeEffect(buildingInfo.buildingIndex);
                return;
            }
            HideFreeEffect(buildingInfo.buildingIndex);
            if (status == SoldierTrainStatus.Training)//训练中
            {
                QueueInfo info = m_playerProxy.GetTrainInfo(buildingInfo.buildingIndex);
                if (info == null)
                {
                    return;
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.ArmyTrainStart, buildingInfo.buildingIndex);
                hudView.m_pl_none.gameObject.SetActive(false);
                hudView.m_pl_train.gameObject.SetActive(true);
                hudView.m_pl_get.gameObject.SetActive(false);

                Int64 totalTime = info.finishTime - info.beginTime;
                totalTime = (totalTime == 0) ? 1 : totalTime;
                Int64 costTime = ServerTimeModule.Instance.GetServerTime() - info.beginTime;
                costTime = (costTime < 0) ? 0 : costTime;
                Int64 residueTime = totalTime - costTime;
                float pro = (float)costTime / totalTime;

                int tempId = m_soldierProxy.GetTemplateId((int)info.armyType, (int)info.newArmyLevel);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);

                if (define != null)
                {
                    hudView.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(200030, LanguageUtils.getText(define.l_armsID), ClientUtils.FormatComma(info.armyNum));
                    ClientUtils.LoadSprite(hudView.m_img_icon_PolygonImage, define.icon);
                }
                bool isShowTime = CityHudCountDownManager.Instance.IsShowTime();
                hudView.m_lbl_desc_LanguageText.gameObject.SetActive(!isShowTime);
                hudView.m_lbl_time_LanguageText.gameObject.SetActive(isShowTime);
                hudView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)residueTime);
                hudView.m_pb_rogressBar_GameSlider.value = pro;

                CityHudCountDownManager.Instance.AddQueue(hudView.m_lbl_desc_LanguageText, hudView.m_lbl_time_LanguageText, hudView.m_pb_rogressBar_GameSlider, info, TimeEndCallback);
            }
            else if (status == SoldierTrainStatus.Finished)//训练完成
            {
                QueueInfo info = m_playerProxy.GetTrainInfo(buildingInfo.buildingIndex);
                if (info == null)
                {
                    return;
                }
                hudView.m_pl_none.gameObject.SetActive(false);
                hudView.m_pl_train.gameObject.SetActive(false);
                hudView.m_pl_get.gameObject.SetActive(true);

                int tempId = m_soldierProxy.GetTemplateId((int)info.armyType, (int)info.newArmyLevel);
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
                if (define != null)
                {
                    ClientUtils.LoadSprite(hudView.m_img_soldier_icon_PolygonImage, define.icon);
                }
            }
        }

        //收取已治疗完成士兵
        public void OnClickGet(HUDUI hud)
        {
            BuildingInfoEntity info = hud.data as BuildingInfoEntity;
            if (!m_playerProxy.TrainQueueMap.ContainsKey(info.buildingIndex))
            {
                return;
            }
            //发包
            m_soldierProxy.RequestReceiveSoldier(m_playerProxy.TrainQueueMap[info.buildingIndex].armyType);
            //飘飞表现
            UIPopTrainOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UIPopTrainOnBuildingView>(hud.uiObj);
            if (hudView != null)
            {
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                mt.FlyPowerUpEffect(hudView.m_img_soldier_icon_PolygonImage.gameObject, hudView.m_img_soldier_icon_PolygonImage.rectTransform, hudView.m_img_soldier_icon_PolygonImage.rectTransform.localScale,null, 1f);
            }
        }

        //领取士兵tip提醒
        private void ReceiveSoldierTip(SoldierInfo info)
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiTrainingEnd, null);

            //tip显示
            int tempId = m_soldierProxy.GetTemplateId((int)info.type, (int)info.level);
            ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(tempId);
            if (define != null)
            {
                string str = LanguageUtils.getTextFormat(192007, LanguageUtils.getText(define.l_armsID), info.num);
                Tip.CreateTip(str).Show();
            }
        }

        #region 空闲特效

        //显示空闲状态
        private void ShowFreeEffect(Int64 buildingIndex)
        {
            if (m_freeEffectDic.ContainsKey(buildingIndex) && m_freeEffectDic[buildingIndex] != null)
            {
                m_freeEffectDic[buildingIndex].SetActive(true);
            }
            else
            {
                m_waitShowFreeEffectDic[buildingIndex] = true;
                CoreUtils.assetService.LoadAssetAsync<GameObject>(RS.ArmyTrainFreeEffect, FreeEffectLoadFinish, null);
            }
        }

        // 隐藏空闲状态
        private void HideFreeEffect(Int64 buildingIndex)
        {
            if (m_freeEffectDic.ContainsKey(buildingIndex) && m_freeEffectDic[buildingIndex] != null)
            {
                m_freeEffectDic[buildingIndex].SetActive(false);
            }
        }

        private void FreeEffectLoadFinish(IAsset asset)
        {
            if (asset.asset() == null)
            {
                Debug.LogErrorFormat("load prefab fail:{0}", asset.assetName());
                return;
            }
            GameObject obj = asset.asset() as GameObject;
            asset.Attack(obj);

            if (m_waitShowFreeEffectDic.Count > 0)
            {
                int status = 0;
                foreach (var data in m_waitShowFreeEffectDic)
                {
                    if (m_cityBuildingProxy.IsUpgrading(data.Key))//正在升级 建造 跳过处理
                    {
                        return;
                    }
                    if (!m_huds.ContainsKey(data.Key))
                    {
                        return;
                    }
                    if (m_huds[data.Key].targetObj != null)
                    {
                        GameObject effect = GameObject.Instantiate(obj, m_huds[data.Key].targetObj.transform);
                        effect.transform.localScale = Vector3.one;
                        effect.transform.localPosition = Vector3.zero;
                        m_freeEffectDic[data.Key] = effect;
                        effect.SetActive(status == SoldierTrainStatus.None);
                    }
                }
            }
            m_waitShowFreeEffectDic.Clear();
        }
        #endregion
    }
}