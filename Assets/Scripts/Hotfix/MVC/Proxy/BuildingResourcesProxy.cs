// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月2日
// Update Time         :    2020年1月2日
// Class Description   :    BuildingResourcesProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Skyunion;
using Data;

namespace Game {
    public class BuildingResourcesProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "BuildingResourcesProxy";

        public Dictionary<long, float> FoodRss = new Dictionary<long, float>();
        public Dictionary<long, float> WoodRss = new Dictionary<long, float>();
        public Dictionary<long, float> StoneRss = new Dictionary<long, float>();
        public Dictionary<long, float> GoldRss = new Dictionary<long, float>();

        public List<BuildingInfoEntity> FoodBuilding = new List<BuildingInfoEntity>();

        public List<BuildingInfoEntity> WoodBuilding = new List<BuildingInfoEntity>();

        public List<BuildingInfoEntity> StoneBuilding = new List<BuildingInfoEntity>();

        public List<BuildingInfoEntity> GoldBuilding = new List<BuildingInfoEntity>();

        public List<BuildingResourcesProduceDefine> m_buildingRssDefine;

        public List<BuildingResourcesProduceDefine> BuildingRssDefine
        {
            get
            {
                if(m_buildingRssDefine == null)
                {
                    m_buildingRssDefine = CoreUtils.dataService.QueryRecords<BuildingResourcesProduceDefine>();
                }
                return m_buildingRssDefine;
            }
        }

        private PlayerAttributeProxy m_playerAttributeProxy;
        private PlayerAttributeProxy playerAttributeProxy
        {
            get
            {
                if(m_playerAttributeProxy == null)
                {
                    m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
                }
                return m_playerAttributeProxy;
            }
        }

        private Timer m_timer;
        public bool IsCollecting;
        #endregion

        // Use this for initialization
        public BuildingResourcesProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" BuildingResourcesProxy register");
        }


        public override void OnRemove()
        {
            Debug.Log(" BuildingResourcesProxy remove");
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }


        public void OnUpdateRss()
        {

            m_timer =  Timer.Register(5f,()=>{
                if (!IsCollecting)
                {
                    UpdateRss();
                }
            },null,true,false,null);//5s更新一次
        }

        public void UpdateBuilding()
        {
            CityBuildingProxy m_cityBuildingProxy =  AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            if(m_cityBuildingProxy==null)
            {
                return;
            }
            FoodBuilding = m_cityBuildingProxy.GetAllBuildingInfoByType((int)EnumCityBuildingType.Farm);
            WoodBuilding = m_cityBuildingProxy.GetAllBuildingInfoByType((int)EnumCityBuildingType.Sawmill);
            StoneBuilding = m_cityBuildingProxy.GetAllBuildingInfoByType((int)EnumCityBuildingType.Quarry);
            GoldBuilding = m_cityBuildingProxy.GetAllBuildingInfoByType((int)EnumCityBuildingType.SilverMine);
            //UpdateRss();
        }

        public void UpdateRss()
        {
            int sendRssNotification = 0;
            sendRssNotification |= UpdateSingleRss(FoodBuilding, FoodRss,0x01);
            sendRssNotification |= UpdateSingleRss(WoodBuilding, WoodRss,0x02);
            sendRssNotification |= UpdateSingleRss(StoneBuilding, StoneRss,0x04);
            sendRssNotification |= UpdateSingleRss(GoldBuilding, GoldRss,0x08);
            if (sendRssNotification>0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.UpdateBuildingResourcesHud, sendRssNotification);
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.UpdateBuildingTavernHud);
        }

        public int UpdateSingleRss(List<BuildingInfoEntity> building, Dictionary<long, float> rss,int id)
        {
            bool sendRssNotification = false;
            foreach (var element in building)
            {
                if (element.finishTime != -1)//建筑没有建好
                {
                    rss[element.buildingIndex] = 0;
                    continue;
                }
                BuildingResourcesProduceDefine define = BuildingRssDefine.Find((i) => { return i.type == element.type && i.level == element.level; });
                if (define != null)
                {
                    float speed = GetSpeed(id) + 1;

                        long interval = ServerTimeModule.Instance.GetServerTime()- element.buildingGainInfo.changeTime;
                        float produces = ((interval * define.produceSpeed / 3600f) * speed) + element.buildingGainInfo.num;
                        long max = Mathf.FloorToInt(define.gatherMax * speed) ;
                        produces = produces > max ? max : produces;
                        rss[element.buildingIndex] = produces;

                  //  Debug.LogError(element.type.ToString() + "  " + element.buildingIndex + " " + rss[element.buildingIndex] + " "+ element.buildingGainInfo.changeTime+" " +  element.buildingGainInfo.num);
                }



                sendRssNotification = true;
            }
            id = sendRssNotification ? id : 0;
            return id;
        }

        private float GetSpeed(int id)
        {
            if(id == 0x01)
            {
                return playerAttributeProxy.GetCityAttribute(attrType.foodCapacityMulti).value;
            }
            else if(id == 0x02)
            {
                return playerAttributeProxy.GetCityAttribute(attrType.woodCapacityMulti).value;
            }
            else if(id == 0x04)
            {
                return playerAttributeProxy.GetCityAttribute(attrType.stoneCapacityMulti).value;
            }
            else
            {
                return playerAttributeProxy.GetCityAttribute(attrType.getGlodSpeedMulti).value;
            }
        }
        private float GetSpeed(long type)
        {
            if (type == (long)EnumCityBuildingType.Farm)
            {
                return playerAttributeProxy.GetCityAttribute(attrType.foodCapacityMulti).value;
            }
            else if (type == (long)EnumCityBuildingType.Sawmill)
            {
                return playerAttributeProxy.GetCityAttribute(attrType.woodCapacityMulti).value;
            }
            else if (type == (long)EnumCityBuildingType.Quarry)
            {
                return playerAttributeProxy.GetCityAttribute(attrType.stoneCapacityMulti).value;
            }
            else
            {
                return playerAttributeProxy.GetCityAttribute(attrType.getGlodSpeedMulti).value;
            }
        }
        /// <summary>
        /// 获取资源收获量
        /// </summary>
        public long GetCurrencyNum(BuildingInfoEntity tmpEntity)
        {
            float speed = 0;
            BuildingResourcesProduceDefine define = BuildingRssDefine.Find((i) => { return i.type == tmpEntity.type && i.level == tmpEntity.level; });
             speed = GetSpeed(tmpEntity.type) + 1;
                long interval = ServerTimeModule.Instance.GetServerTime() - tmpEntity.buildingGainInfo.changeTime;
            long produces = Mathf.FloorToInt(((interval * define.produceSpeed / 3600f) * speed) + tmpEntity.buildingGainInfo.num);
            long max = Mathf.FloorToInt(define.gatherMax * speed);
                produces = produces > max ? max : produces;
            return produces;
            }

            public void Harvest(EnumCityBuildingType type)
        {
            switch(type)
            {
                case EnumCityBuildingType.Farm:
                    foreach(var element in FoodBuilding)
                    {
                        FoodRss[element.buildingIndex] = 0;
                    }
                    break;
                case EnumCityBuildingType.Sawmill:
                    foreach (var element in WoodBuilding)
                    {
                        WoodRss[element.buildingIndex] = 0;
                    }
                    break;
                case EnumCityBuildingType.Quarry:
                    foreach (var element in StoneBuilding)
                    {
                        StoneRss[element.buildingIndex] = 0;
                    }
                    break;
                case EnumCityBuildingType.SilverMine:
                    foreach (var element in GoldBuilding)
                    {
                        GoldRss[element.buildingIndex] = 0;
                    }
                    break;
            }
        }
    }
}