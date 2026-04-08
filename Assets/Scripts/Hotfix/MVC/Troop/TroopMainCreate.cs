using System.Collections.Generic;
using Client;
using Data;
using Game;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Hotfix
{
    public enum TroopMainCreateDataType
    {
        None = 0,
        Troop,
        Scout,
        Transport,
        All = Transport,
    }


    public class TroopMainCreateData
    {
        public int id; //data index
        public int level;
        public string icon;
        public string scoutIcon;
        public HeroProxy.Hero hero;
        public ArmyInfoEntity ArmyInfo;
        public long soldiersNum;
        public TroopMainCreateDataType type = TroopMainCreateDataType.None;
        public Troops.ENMU_SQUARE_STAT state;
        public ArmyStatus armyStatus;
        public bool isGray = false;
        public bool isShowLine = false;
    }

    public class TroopMainCreate
    {
        private HeroProxy m_HeroProxy;
        private TroopProxy m_TroopProxy;
        private ScoutProxy m_ScoutProxy;
        private ConfigDefine configInfo;

        private List<TroopMainCreateData> m_allDatas = new List<TroopMainCreateData>();
        private List<TroopMainCreateDataType> m_dataTypes = null;

        public void Init(List<TroopMainCreateDataType> dataTypes = null)
        {
            m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            configInfo = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if(dataTypes == null || dataTypes.Count == 0)
            {
                m_dataTypes = new List<TroopMainCreateDataType>();
                for (int i = (int)TroopMainCreateDataType.None + 1; i <= (int)TroopMainCreateDataType.All; ++i)
                {
                    m_dataTypes.Add((TroopMainCreateDataType)i);
                }               
            }
            else
            {
                m_dataTypes = dataTypes;
            }
            InitData();
        }

        private void InitData()
        {
            for (int i = 0; i < m_dataTypes.Count; ++i)
            {
                InitTypeData(m_dataTypes[i]);
            }
        }

        private void InitTypeData(TroopMainCreateDataType type)
        {
            List<TroopMainCreateData> dataList = null;
            switch(type)
            {
                case TroopMainCreateDataType.Troop:
                    dataList = InitTroops();
                    break;
                case TroopMainCreateDataType.Scout:
                    dataList = InitScouts();
                    break;
                case TroopMainCreateDataType.Transport:
                    dataList = InitTransports();
                    break;
            }
            if(dataList != null && dataList.Count> 0)
            {
                m_allDatas.AddRange(dataList);
            }          
            SetLine();
        }

        private List<TroopMainCreateData> InitTroops()
        {
            if (m_TroopProxy.GetArmyCount() == 0) return null;

            List<TroopMainCreateData> datas = new List<TroopMainCreateData>();
            var allArmy = m_TroopProxy.GetArmys();
            foreach(var army in allArmy)
            {
                datas.Add(CreateTroopData(army));
            }
            return datas;
        }

        private List<TroopMainCreateData> InitScouts()
        {
            if (m_ScoutProxy.GetActiveScountCount() == 0) return null;
            List<TroopMainCreateData> datas = new List<TroopMainCreateData>();
            var scountInfos = m_ScoutProxy.GetAllActiveScounts();
            foreach(var scountInfo in scountInfos)
            {
                datas.Add(CreateScoutData(scountInfo));
            }

            return datas;
        }

        private List<TroopMainCreateData> InitTransports()
        {
            var transportInfos = m_TroopProxy.GetAllTransportInfos();
            if (transportInfos.Count == 0) return null;            
            List<TroopMainCreateData> datas = new List<TroopMainCreateData>();
            foreach (var info in transportInfos)
            {
                datas.Add(CreateTransportData(info));
            }
            return datas;
        }

        public void Update()
        {
            m_allDatas.Clear();
            InitData();
        }

        public void Clear()
        {
            m_allDatas.Clear();
            m_dataTypes.Clear();
        }

        public TroopMainCreateData GetData(int index)
        {
            if (m_allDatas.Count <= index) return null;
            return m_allDatas[index];
        }

        public int GetDataCount()
        {
            return m_allDatas.Count;
        }

        public long GetSoldiersNum(Dictionary<long, SoldierInfo> soldiers)
        {
            long sum = 0;
            if(soldiers != null)
            {
                foreach (var item in soldiers.Values)
                {
                    sum += item.num;
                }
            } 

            return sum;
        }

        public void SetGray(bool gray)
        {
            foreach (var info in m_allDatas)
            {
                info.isGray = gray;
            }
        }

        public void SetLine()
        {
            foreach (var info in m_allDatas)
            {
                if (info.type == TroopMainCreateDataType.Scout)
                {
                    info.isShowLine = true;
                    return;
                }
            }
        }

        public TroopMainCreateData CreateTransportData(TransportInfoEntity transportInfo)
        {
            TroopMainCreateData data = new TroopMainCreateData();
            data.id = (int)transportInfo.transportIndex;
            data.type = TroopMainCreateDataType.Transport;
            string heroIcon = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).transportIcon;
            data.scoutIcon = heroIcon;
            data.icon = TroopHelp.GetIcon((long)ArmyStatus.ATTACK_MARCH);
            return data;
        }

        public TroopMainCreateData CreateTroopData(ArmyInfoEntity armyInfo)
        {
            TroopMainCreateData data = new TroopMainCreateData();
            data.type = TroopMainCreateDataType.Troop;
            data.id = (int)armyInfo.armyIndex;
            HeroProxy heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            if(heroProxy != null)
            {
                data.hero = heroProxy.GetHeroByID(armyInfo.mainHeroId);
                if (data.hero != null)
                {
                    data.level = data.hero.level;
                }
            }

            data.ArmyInfo = armyInfo;
            data.icon = TroopHelp.GetIcon(armyInfo.status);
            data.armyStatus = (ArmyStatus)armyInfo.status;
            data.state = TroopHelp.GetTroopState(armyInfo.status);
            data.soldiersNum = GetSoldiersNum(data.ArmyInfo.soldiers);
            return data;
        }

        public TroopMainCreateData CreateScoutData(ScoutProxy.ScoutInfo scoutInfo)
        {
            TroopMainCreateData data = new TroopMainCreateData();
            data.type = TroopMainCreateDataType.Scout;
            string heroIcon = ScoutProxy.GetScoutIcon((int)scoutInfo.id);
            data.id = scoutInfo.id;
            data.icon = ScoutProxy.GetScoutStateIcon(scoutInfo.state);
            data.scoutIcon = heroIcon;
            data.armyStatus = (ArmyStatus)scoutInfo.state;
            return data;
        }

        public TroopMainCreateData CreateGuildFadeData()
        {
            TroopMainCreateData fakeData = new TroopMainCreateData();
            GlobalFilmMediator mt =
                AppFacade.GetInstance().RetrieveMediator(GlobalFilmMediator.NameMediator) as GlobalFilmMediator;
            if (mt.film3 == null)
            {
                Debug.LogError("野蛮人战斗未创建");
                return null;
            }

            fakeData.type = TroopMainCreateDataType.Troop;
            fakeData.id = (int)mt.film3.FirstTroop.mapObjectInfo.objectId;
            fakeData.hero = m_HeroProxy.GetHeroByID(mt.film3.FirstTroop.mapObjectInfo.mainHeroId);
            fakeData.level = fakeData.hero.level;
            fakeData.ArmyInfo = new ArmyInfoEntity();
            fakeData.ArmyInfo.armyIndex = mt.film3.FirstTroop.mapObjectInfo.armyIndex;
            fakeData.ArmyInfo.arrivalTime = mt.film3.FirstTroop.mapObjectInfo.arrivalTime;
            fakeData.ArmyInfo.status = (int)ArmyStatus.STATIONING;
            fakeData.ArmyInfo.soldiers = mt.film3.FirstTroop.mapObjectInfo.soldiers;
            fakeData.ArmyInfo.soldiers[mt.film3.FirstTroopArmyID].num = mt.film3.FirstTroopArmyCount;
            fakeData.icon = TroopHelp.GetIcon((long)ArmyStatus.STATIONING);
            fakeData.soldiersNum = GetSoldiersNum(fakeData.ArmyInfo.soldiers);
            return fakeData;
        }
    }
}