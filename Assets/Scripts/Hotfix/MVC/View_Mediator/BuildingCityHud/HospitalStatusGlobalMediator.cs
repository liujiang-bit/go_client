// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    HospitalStatusGlobalMediator 医院治疗状态
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
    public class HospitalTreatmentItem
    {
        public Int64 BuildingIndex;
        public HUDUI Hudui;
        public UIPopTreatmentOnBuildingView HudView;
    }

    public class HospitalStatusGlobalMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "HospitalStatusGlobalMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private HospitalProxy m_hospitalProxy;
        private PlayerProxy m_playerProxy;
        private SoldierProxy m_soldierProxy;

        //治疗相关
        private Dictionary<Int64, HUDUI> m_hospitalHuds = new Dictionary<Int64, HUDUI>();
        private List<ArmsDefine> m_treatmentSoldierList = new List<ArmsDefine>(); //已治疗士兵列表
        private Int64 m_treatmentNum;
        private QueueInfo m_treatmentQueue;

        private QueueInfo m_cacheQueue;

        //治疗特效
        private Dictionary<Int64, GameObject> m_effectDic = new Dictionary<Int64, GameObject>();
        private Dictionary<Int64, bool> m_waitShowEffectDic = new Dictionary<Int64, bool>();

        private bool m_isReceiveShowing;

        private bool m_dispose;

        #endregion

        public HospitalStatusGlobalMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public HospitalStatusGlobalMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.treatmentChange,            //治疗队列信息变更
                CmdConstant.CityBuildingDone,           //城市建筑创建完成
                CmdConstant.HospitalTreatmentStatus,    //单个医院刷新
                CmdConstant.SeriousInjuredChange,       //伤兵信息变更
                Role_AwardTreatment.TagName,            //领取治疗完毕的士兵通知
                CmdConstant.buildQueueChange,           //建筑建造通知
                CmdConstant.AwardTreatment,
                CmdConstant.CityBuildingLevelUPStart,
                CmdConstant.CityBuildingLevelUPCancel,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.treatmentChange:
                    RoleInfo info = notification.Body as RoleInfo;
                    QueueChange(info);
                    break;
                case CmdConstant.CityBuildingDone:
                    UpdateAllHospitalStatus();
                    break;
                case CmdConstant.CityBuildingLevelUPCancel:
                    UpdateAllHospitalStatus();
                    break;
                case CmdConstant.CityBuildingLevelUPStart:
                    {
                        Dictionary<Int64, QueueInfo> dic = m_playerProxy.CurrentRoleInfo.buildQueue;
                        if (dic != null)
                        {
                            Int64 diffTime = 0;
                            foreach (var queueInfo in dic)
                            {
                                if (queueInfo.Value.buildingIndex > 0)
                                {
                                    BuildingInfoEntity upInfo = m_cityBuildingProxy.GetBuildingInfoByindex(queueInfo.Value.buildingIndex);
                                    if (upInfo != null && upInfo.type == (int)EnumCityBuildingType.Hospital)
                                    {
                                        int status = m_hospitalProxy.GetHospitalStatus();
                                        diffTime = queueInfo.Value.finishTime - ServerTimeModule.Instance.GetServerTime();
                                        if (queueInfo.Value.finishTime <= 0 || diffTime <= 0)
                                        {
                                            UpdateStatus(upInfo, status, true);
                                        }
                                        else
                                        {
                                            UpdateStatus(upInfo, status);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.HospitalTreatmentStatus:
                    Int64 buildingIndex = (Int64)notification.Body;
                    BuildingInfoEntity info1 = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
                    if (info1 != null)
                    {
                        UpdateHospitalStatus(info1);
                    }
                    break;
                case CmdConstant.SeriousInjuredChange: //伤兵信息变更   
                    m_hospitalProxy.SetSeriousInjuredChange();
                    UpdateAllHospitalStatus();
                    break;
                case Role_AwardTreatment.TagName: //领取治疗完毕的士兵
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        if (m_isReceiveShowing)
                        {
                            UpdateAllHospitalStatus();
                        }
                    }
                    m_isReceiveShowing = false;
                    ReceiveSoldierTip(notification.Body);
                    break;
                case CmdConstant.buildQueueChange:
                    if (notification.Body != null)
                    {
                        Dictionary<Int64, QueueInfo> dic = notification.Body as Dictionary<Int64, QueueInfo>;
                        if (dic != null)
                        {
                            Int64 diffTime = 0;
                            foreach (var queueInfo in dic)
                            {
                                if (queueInfo.Value.buildingIndex > 0)
                                {
                                    BuildingInfoEntity upInfo = m_cityBuildingProxy.GetBuildingInfoByindex(queueInfo.Value.buildingIndex);
                                    if (upInfo != null && upInfo.type == (int)EnumCityBuildingType.Hospital)
                                    {
                                        int status = m_hospitalProxy.GetHospitalStatus();
                                        diffTime = queueInfo.Value.finishTime - ServerTimeModule.Instance.GetServerTime();
                                        if (queueInfo.Value.finishTime <= 0 || diffTime <= 0)
                                        {
                                            UpdateStatus(upInfo, status, true);
                                        }
                                        else
                                        {
                                            UpdateStatus(upInfo, status);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case CmdConstant.AwardTreatment: // 收取士兵战力飘飞表现
                    OnPowerUpdateShow();
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_hospitalProxy = AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;
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
            m_dispose = true;
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
            if (info != null && info.treatmentQueue != null)
            {
                Debug.LogFormat("治疗部队 firstTime:{0} finishTime:{1} beginTime:{2} costTime:{3} serverTime:{4}",
                                info.treatmentQueue.firstFinishTime, info.treatmentQueue.finishTime, info.treatmentQueue.beginTime, 
                                info.treatmentQueue.finishTime - info.treatmentQueue.beginTime,
                                ServerTimeModule.Instance.GetServerTime());

                if (info.treatmentQueue.finishTime < 0 && info.treatmentQueue.treatmentSoldiers != null) //走领取流程
                {
                    if (info.treatmentQueue.treatmentSoldiers.Count > 0)
                    {
                        SoldierGetShow(info.treatmentQueue);
                    }
                }
                else
                {
                    if (m_cacheQueue!=null)
                    {
                        if (info.treatmentQueue.beginTime == m_cacheQueue.beginTime && info.treatmentQueue.beginTime > 0 &&
                            info.treatmentQueue.finishTime < m_cacheQueue.finishTime)
                        {
                            if (info.treatmentQueue.finishTime <= ServerTimeModule.Instance.GetServerTime())
                            {
                                //tip提醒
                                string str = LanguageUtils.getTextFormat(181102, GetTreatmentNum());
                                Tip.CreateTip(str).Show();
                            }
                        }
                    }
                    //判断是否加速
                    UpdateAllHospitalStatus();
                }
                m_cacheQueue = info.treatmentQueue;
            }
        }

        //士兵领取表现
        private void SoldierGetShow(QueueInfo queue)
        { 
            if (queue.treatmentSoldiers != null)
            {
                //动画表现 todo
            }

            UpdateAllHospitalStatus();
        }

        //士兵领取tip
        private void ReceiveSoldierTip(object body)
        {
            var awardResponse = body as Role_AwardTreatment.response;
            if (awardResponse == null)
            {
                return;
            }
            if (awardResponse.soldiers == null)
            {
                return;
            }

            Int64 num = 0;
            foreach (var data in awardResponse.soldiers)
            {
                num = num + data.Value.num;
            }
            string str = LanguageUtils.getTextFormat(181101, num);
            Tip.CreateTip(str).Show();
        }

        private void UpdateHospitalStatus(BuildingInfoEntity info)
        {
            int status = m_hospitalProxy.GetHospitalStatus();
            if (status == (int)EnumHospitalStatus.Treatment) //治疗中
            {
                TreatmentDataUpdate();
            }
            else if (status == (int)EnumHospitalStatus.Finished) //治疗完成 可以领取
            {
                TreatmentFinishDataUpdate();
            }
            UpdateStatus(info, status);
        }

        private void UpdateAllHospitalStatus()
        {
            int status = m_hospitalProxy.GetHospitalStatus();
            if (status == (int)EnumHospitalStatus.Treatment) //治疗中
            {
                TreatmentDataUpdate();
            }
            else if (status == (int)EnumHospitalStatus.Finished) //治疗完成 可以领取
            {
                TreatmentFinishDataUpdate();
            }

            List<BuildingInfoEntity> buildingList = m_cityBuildingProxy.GetAllBuildingInfoByType((int)EnumCityBuildingType.Hospital);
            for (int i = 0; i < buildingList.Count; i++)
            {
                UpdateStatus(buildingList[i], status);
            }
        }

        private void UpdateStatus(BuildingInfoEntity info, int status, bool isJumpUpJudge = false)
        {
            if (!isJumpUpJudge && m_cityBuildingProxy.IsUpgrading(info.buildingIndex))//正在升级 建造 跳过处理
            {
                if (m_hospitalHuds.ContainsKey(info.buildingIndex) && m_hospitalHuds[info.buildingIndex].uiObj != null)
                {
                    m_hospitalHuds[info.buildingIndex].Close();
                }
                return;
            }
            if (info.level == 0 && info.finishTime == 0) //正准备创建中
            {
                return;
            }
            GameObject go = CityObjData.GeBuildTipTargetGameObject(info.buildingIndex);
            if (go == null)
            {
                return;
            }
            if (m_hospitalHuds.ContainsKey(info.buildingIndex)) 
            {
                //直接刷新
                if (!m_hospitalHuds[info.buildingIndex].bDispose)
                {
                    if (m_hospitalHuds[info.buildingIndex].uiObj != null)
                    {
                        RefreshTreatmentStatus(m_hospitalHuds[info.buildingIndex], status); 
                    }
                    return;
                }
            }

            //创建
            if (status == (int)EnumHospitalStatus.None)
            {
                return;
            }
            HUDUI curHud = HUDUI.Register(UIPopTreatmentOnBuildingView.VIEW_NAME, typeof(UIPopTreatmentOnBuildingView), HUDLayer.city, go);
            curHud.SetData(info);
            curHud.SetScaleAutoAnchor(true);
            curHud.SetCameraLodDist(0, 270f);
            curHud.SetInitCallback(HospitalTreatmentCreate);
            ClientUtils.hudManager.ShowHud(curHud);
            m_hospitalHuds[info.buildingIndex] = curHud;
        }

        private void TreatmentDataUpdate()
        {
            m_treatmentQueue = m_playerProxy.GetTreatmentQueue();

            //统计一下受伤总人数
            m_treatmentNum = GetTreatmentNum();
        }

        private Int64 GetTreatmentNum()
        {
            Int64 num = 0;
            List<SoldierInfo> list = m_hospitalProxy.GetTreatmentData();
            for (int i = 0; i < list.Count; i++)
            {
                num = num + list[i].num;
            }
            return num;
        }

        private void TreatmentFinishDataUpdate()
        {
            m_treatmentSoldierList.Clear();

            List<SoldierInfo> list = m_hospitalProxy.GetTreatmentData(2);
            if (list != null && list.Count > 0)
            {
                Dictionary<Int64, Int64> tDic = new Dictionary<Int64, Int64>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!tDic.ContainsKey(list[i].type))
                    {
                        tDic[list[i].type] = list[i].level;
                    }
                }
                ArmsDefine define;
                foreach (var data in tDic)
                {
                    int id = m_soldierProxy.GetTemplateId((int)data.Key, (int)data.Value);
                    define = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                    if (define != null)
                    {
                        m_treatmentSoldierList.Add(define);
                    }
                }
            } 
        }

        private void HospitalTreatmentCreate(HUDUI hud)
        {
            if (hud.targetObj == null || hud.uiObj == null)
            {
                Debug.LogWarning("节点被干掉了");
                return;
            }
            UIPopTreatmentOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UIPopTreatmentOnBuildingView>(hud.uiObj);
            hudView.m_btn_wound_GameButton.onClick.AddListener(() => {
                OnClickWound(hud);
            });
            hudView.m_btn_get_GameButton.onClick.AddListener(() => {
                OnClickGet(hud);
            });
            RefreshTreatmentStatus(hud, -1);
        }

        private void RefreshTreatmentStatus(HUDUI hud, int status)
        {
            if (hud.uiObj == null) //还没创建好 直接return
            {
                return;
            }
            BuildingInfoEntity buildingInfo = hud.data as BuildingInfoEntity;
            if (buildingInfo == null)
            {
                return;
            }
            if (m_cityBuildingProxy.IsUpgrading(buildingInfo.buildingIndex))//正在升级 建造 跳过处理
            {
                hud.Close();
                return;
            }
            if (status < 0)
            {
                status = m_hospitalProxy.GetHospitalStatus();
            }
            if (status == (int)EnumHospitalStatus.None) //无伤兵
            {
                hud.Close();
                return;
            }
            UIPopTreatmentOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UIPopTreatmentOnBuildingView>(hud.uiObj);
            if (status == (int)EnumHospitalStatus.Wound) //有伤兵
            {
                hudView.m_pl_wound.gameObject.SetActive(true);
                hudView.m_pl_get.gameObject.SetActive(false);
                hudView.m_pl_treatment.gameObject.SetActive(false);

                int id = (int)m_playerProxy.GetCivilization();
                CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>(id);
                if (define != null)
                {
                    ClientUtils.LoadSprite(hudView.m_img_civilization_PolygonImage, RS.HospitalMark[define.hospitalMark]);
                }
                HideTreatmentEffect(buildingInfo.buildingIndex);
                return;
            }
            else if (status == (int)EnumHospitalStatus.Treatment)//治疗中
            {
                hudView.m_pl_wound.gameObject.SetActive(false);
                hudView.m_pl_get.gameObject.SetActive(false);
                hudView.m_pl_treatment.gameObject.SetActive(true);

                QueueInfo treatmentInfo = m_playerProxy.GetTreatmentQueue();
                Int64 totalTime = treatmentInfo.finishTime - treatmentInfo.beginTime;
                totalTime = (totalTime == 0) ? 1 : totalTime;
                Int64 costTime = ServerTimeModule.Instance.GetServerTime() - treatmentInfo.beginTime;
                costTime = (costTime < 0) ? 0 : costTime;
                Int64 residueTime = totalTime - costTime;
                float pro = (float)costTime / totalTime;

                bool isShowTime = CityHudCountDownManager.Instance.IsShowTime();
                hudView.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(181109, ClientUtils.FormatComma(m_treatmentNum));
                hudView.m_lbl_desc_LanguageText.gameObject.SetActive(false);

                hudView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)residueTime);
                hudView.m_pb_rogressBar_GameSlider.value = pro;
                hudView.m_lbl_time_LanguageText.gameObject.SetActive(true);

                CityHudCountDownManager.Instance.AddQueue(hudView.m_lbl_desc_LanguageText, hudView.m_lbl_time_LanguageText, hudView.m_pb_rogressBar_GameSlider, treatmentInfo, TimeEndCallback);
                ShowTreatmentEffect(buildingInfo.buildingIndex);
            }
            else//治疗完成 待收取
            {
                HideTreatmentEffect(buildingInfo.buildingIndex);
                hudView.m_pl_wound.gameObject.SetActive(false);
                hudView.m_pl_get.gameObject.SetActive(true);
                hudView.m_pl_treatment.gameObject.SetActive(false);
                hudView.m_UI_Model_TreatmentHeadAlign.RefreshHead(m_treatmentSoldierList);
                AppFacade.GetInstance().SendNotification(CmdConstant.HospitalCanGetReward);
            }
        }

        private void TimeEndCallback(Int64 buildingIndex)
        {
            if (m_treatmentNum>0) // 如果治疗界面已打开 则由治疗界面进行tip提醒
            {
                string str = LanguageUtils.getTextFormat(181102, m_treatmentNum);
                Tip.CreateTip(str).Show();
                m_treatmentNum = 0;
                TreatmentFinishDataUpdate();
            }
            foreach (var data in m_hospitalHuds)
            {
                RefreshTreatmentStatus(data.Value, -1);
            }
        }

        //打开医院界面
        private void OnClickWound(HUDUI hud)
        {
            BuildingInfoEntity buildingInfo = hud.data as BuildingInfoEntity;
            if (buildingInfo == null)
            {
                return;
            }
            CoreUtils.uiManager.ShowUI(UI.s_hospitalInfo, null, buildingInfo.buildingIndex);
        }

        //收取已治疗完成士兵
        private void OnClickGet(HUDUI hud)
        {
            if (m_isReceiveShowing)
            {
                return;
            }
            m_isReceiveShowing = true;
            CoreUtils.audioService.PlayOneShot(RS.SoundUiEndHealing, null);
            //发包
            var sp = new Role_AwardTreatment.request();
            AppFacade.GetInstance().SendSproto(sp);
            OnPowerUpdateShow();
        }

        //收取士兵战力飘飞表现
        private void OnPowerUpdateShow()
        {

            foreach (var hud in m_hospitalHuds)
            {
                if(hud.Value==null||hud.Value.bDispose)
                {
                    continue;
                }
                UIPopTreatmentOnBuildingView hudView = MonoHelper.GetOrAddHotFixViewComponent<UIPopTreatmentOnBuildingView>(hud.Value.uiObj);
                int RandKey1 = (new System.Random(Guid.NewGuid().GetHashCode()).Next(1, 30));
                int RandKey2 = (new System.Random(Guid.NewGuid().GetHashCode()).Next(1, 30));

                if (hudView.m_pl_get.gameObject.activeSelf)
                {
                    float scale = hudView.gameObject.transform.localScale.x * hudView.m_pl_get.gameObject.transform.localScale.x;

                    FlySingleItem(hudView.m_UI_Model_TreatmentHeadAlign.m_UI_Item_TreatmentHeadAlign1, (float)RandKey1/100, 0, scale);
                    FlySingleItem(hudView.m_UI_Model_TreatmentHeadAlign.m_UI_Item_TreatmentHeadAlign2, (float)RandKey2 / 100, 0, scale);
                    FlySingleItem(hudView.m_UI_Model_TreatmentHeadAlign.m_UI_Item_TreatmentHeadAlign3, -1, 1, scale);
                    FlySingleItem(hudView.m_UI_Model_TreatmentHeadAlign.m_UI_Item_TreatmentHeadAlign4, -1, 2, scale);

                    //hudView.m_pl_get.gameObject.SetActive(false);
                }
            }
        }

        private void FlySingleItem(UI_Item_TreatmentHeadAlign_SubView itemView, float times, int index = 0, float scale = 0)
        {
            if (itemView.gameObject.activeSelf)
            {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(itemView.m_UI_Item_TreatmentHead.m_img_char_PolygonImage.transform.position), CoreUtils.uiManager.GetUICamera(), out pos);
                GameObject copyGO = GameObject.Instantiate(itemView.m_UI_Item_TreatmentHead.m_img_char_PolygonImage.gameObject, CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                copyGO.SetActive(false);
                RectTransform rectTrans = copyGO.GetComponent<RectTransform>();
                rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
                rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
                copyGO.transform.localPosition = pos;
                float scaleVal = itemView.m_UI_Item_TreatmentHead.m_img_char_PolygonImage.rectTransform.localScale.x * scale;
                copyGO.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);

                if (times == -1)
                {
                    times = index * 0.02f;
                    Timer.Register(times, () => {
                        if (m_dispose)
                        {
                            GameObject.DestroyImmediate(copyGO);
                            return;
                        }
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyPowerUpNoEffect(copyGO, rectTrans, rectTrans.localScale, null, 1f, false);
                    });
                }
                else
                {
                    Timer.Register(times, () => {
                        if (m_dispose)
                        {
                            GameObject.DestroyImmediate(copyGO);
                            return;
                        }
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyPowerUpEffect(copyGO, rectTrans, rectTrans.localScale, null, 1f, false);
                    });
                }
              
            }
        }

        #region

        //显示治疗特效
        private void ShowTreatmentEffect(Int64 buildingIndex)
        {
            if (m_effectDic.ContainsKey(buildingIndex) && m_effectDic[buildingIndex] != null)
            {
                m_effectDic[buildingIndex].SetActive(true);
            }
            else
            {
                m_waitShowEffectDic[buildingIndex] = true;
                CoreUtils.assetService.LoadAssetAsync<GameObject>(RS.HospitalTreatmentEffect, EffectLoadFinish, null);
            }
        }

        //隐藏治疗特效
        private void HideTreatmentEffect(Int64 buildingIndex)
        {
            if (m_effectDic.ContainsKey(buildingIndex) && m_effectDic[buildingIndex] != null)
            {
                m_effectDic[buildingIndex].SetActive(false);
            }
        }

        private void EffectLoadFinish(IAsset asset)
        {
            if (asset.asset() == null)
            {
                Debug.LogErrorFormat("load prefab fail:{0}", asset.assetName());
                return;
            }
            GameObject obj = asset.asset() as GameObject;
            asset.Attack(obj);
            if (m_waitShowEffectDic.Count > 0)
            {
                int status = 0;
                foreach (var data in m_waitShowEffectDic)
                {
                    if (m_cityBuildingProxy.IsUpgrading(data.Key))//正在升级 建造 跳过处理
                    {
                        continue;
                    }
                    if (!m_hospitalHuds.ContainsKey(data.Key))
                    {
                        continue;
                    }
                    if (m_hospitalProxy.GetHospitalStatus() != (int)EnumHospitalStatus.Treatment)
                    {
                        continue;
                    }
                    if (m_hospitalHuds[data.Key].targetObj != null)
                    {
                        GameObject effect = GameObject.Instantiate(obj, m_hospitalHuds[data.Key].targetObj.transform);
                        effect.transform.localScale = Vector3.one;
                        effect.transform.localPosition = Vector3.zero;
                        m_effectDic[data.Key] = effect;
                        effect.SetActive(status == SoldierTrainStatus.None);
                    }
                }
            }
            m_waitShowEffectDic.Clear();
        }

        #endregion
    }
}