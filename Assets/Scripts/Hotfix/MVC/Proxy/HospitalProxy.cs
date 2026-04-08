// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    SoldierProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Hotfix;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public enum EnumHospitalStatus
    {
        /// <summary>
        /// 无伤兵
        /// </summary>
        None = 1,
        /// <summary>
        /// 有伤兵
        /// </summary>
        Wound = 2, 
        /// <summary>
        /// 治疗中
        /// </summary>
        Treatment = 3,
        /// <summary>
        /// 治疗完成 尚未收取
        /// </summary>
        Finished = 4,
    }

    public class HospitalProxy : GameProxy
    {
        #region Member

        public const string ProxyNAME = "HospitalProxy";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;

        private Dictionary<Int64, Int64> SortTypeMap = new Dictionary<Int64, Int64>();

        private bool m_seriousInjuredIsChange = true;
        private bool m_isHasWound = false;

        #endregion

        // Use this for initialization
        public HospitalProxy(string proxyName)
            : base(proxyName)
        {
        }

        public override void OnRegister()
        {
            Debug.Log(" HospitalProxy register");
        }

        public override void OnRemove()
        {
            Debug.Log(" HospitalProxy remove");
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            SortTypeMap[1] = 1;
            SortTypeMap[2] = 3;
            SortTypeMap[3] = 2;
            SortTypeMap[4] = 4;
        }

        public int GetHospitalStatus()
        {
            //是否治疗中
            QueueInfo info = m_playerProxy.GetTreatmentQueue();
            if (info != null)
            {
                if (info.finishTime > 0)
                {
                    if (info.finishTime > ServerTimeModule.Instance.GetServerTime())
                    {
                        return (int)EnumHospitalStatus.Treatment;
                    }
                    return (int)EnumHospitalStatus.Finished;
                }
            }
            //是否有伤兵
            if (m_seriousInjuredIsChange)
            {
                var woundData = m_playerProxy.GetWoundedInfo();
                if (woundData != null && woundData.Count > 0)
                {
                    foreach (var data in woundData)
                    {
                        if (data.Value.num > 0)
                        {
                            m_isHasWound = true;
                            return (int)EnumHospitalStatus.Wound;
                        }
                    }
                }
                m_isHasWound = false;
            }
            else
            {
                if (m_isHasWound)
                {
                    return (int)EnumHospitalStatus.Wound;
                }
            }
            return (int)EnumHospitalStatus.None;
        }

        public void SetSeriousInjuredChange()
        {
            m_seriousInjuredIsChange = true;
        }

        //获取医院容量
        public Int64 GetHospitalCapacity()
        {
            List<BuildingInfoEntity> list = m_cityBuildingProxy.GetAllBuildingInfoByType((int)EnumCityBuildingType.Hospital);

            if (list != null)
            {
                Int64 capacity = 0;
                BuildingHospitalDefine define;
                for (int i = 0; i < list.Count; i++)
                {
                    define = CoreUtils.dataService.QueryRecord<BuildingHospitalDefine>((int)list[i].level);
                    if (define != null)
                    {
                        capacity = capacity + (Int64)Mathf.Floor(define.armyCnt * (1 + m_playerAttributeProxy.GetCityAttribute(attrType.hospitalSpaceMulti).value));
                    }
                }
                return capacity;
            }

            return 0;
        }

        //获取治疗中数据
        public List<SoldierInfo> GetTreatmentData(int sortType = 1)
        {
            List<SoldierInfo> list = new List<SoldierInfo>();
            QueueInfo info = m_playerProxy.GetTreatmentQueue();
            if (info != null)
            {
                if (info.finishTime >0 )
                {
                    foreach (var data in info.treatmentSoldiers)
                    {
                        list.Add(data.Value);
                    }
                    if (sortType == 1) //治疗中
                    {
                        list.Sort(delegate (SoldierInfo x, SoldierInfo y)
                        {
                            int re = y.level.CompareTo(x.level);
                            if (re == 0)
                            {
                                re = SortTypeMap[x.type].CompareTo(SortTypeMap[y.type]);
                            }
                            //int re = y.num.CompareTo(x.num);
                            //if (re == 0)
                            //{
                            //}
                            return re;
                        });
                    }
                    else //治疗完成
                    {
                        list.Sort(delegate (SoldierInfo x, SoldierInfo y)
                        {
                            int re = y.level.CompareTo(x.level);
                            if (re == 0)
                            {
                                re = SortTypeMap[x.type].CompareTo(SortTypeMap[y.type]);
                            }
                            return re;
                        });
                    }
                }
            }        
            return list;
        }

        //获取伤兵数据（包含未治疗 治疗中）
        public List<SoldierInfo> GetWoundedData()
        {
            List<SoldierInfo> list = new List<SoldierInfo>();
            var woundData = m_playerProxy.GetWoundedInfo();
            if (woundData != null)
            {
                foreach (var data in woundData)
                {
                    if (data.Value.num > 0)
                    {
                        list.Add(data.Value);
                    }
                }
            }
            //排序
            list.Sort(delegate (SoldierInfo x, SoldierInfo y)
            {
                int re = y.level.CompareTo(x.level);
                if (re == 0)
                {
                    re = SortTypeMap[x.type].CompareTo(SortTypeMap[y.type]);
                }
                return re;
            });
            return list;
        }

        //获取伤兵数据（仅包含未治疗）
        public List<SoldierInfo> GetWoundedData2()
        {
            QueueInfo treatmentQueue = m_playerProxy.GetTreatmentQueue();
            List<SoldierInfo> list = new List<SoldierInfo>();
            var woundData = m_playerProxy.GetWoundedInfo();
            if (woundData != null)
            {
                foreach (var data in woundData)
                {
                    if (data.Value.num > 0)
                    {
                        if (treatmentQueue != null && treatmentQueue.treatmentSoldiers != null)
                        {
                            if (treatmentQueue.treatmentSoldiers.ContainsKey(data.Value.id))
                            {
                                Int64 numDiff = data.Value.num - treatmentQueue.treatmentSoldiers[data.Value.id].num;
                                if (numDiff > 0)
                                {
                                    SoldierInfo tInfo = new SoldierInfo();
                                    tInfo.id = data.Value.id;
                                    tInfo.num = numDiff;
                                    tInfo.level = data.Value.level;
                                    tInfo.type = data.Value.type;
                                    list.Add(tInfo);
                                }
                            }
                            else
                            {
                                list.Add(data.Value);
                            }
                        }
                        else
                        {
                            list.Add(data.Value);
                        }
                    }
                }
            }
            return list;
        }
    }
}