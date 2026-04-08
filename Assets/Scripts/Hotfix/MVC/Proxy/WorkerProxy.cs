using System;
using System.Collections.Generic;
using Client;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public enum BuildQueueState
    {
        LOCK =0,//未解锁
        LEISURE1 = 1,//空闲
        LEISURE2 = 2,//空闲但时间不足
        NOLEISURE = 3,//没有空闲
    }
    public class WorkerProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "WorkerProxy";
        private PlayerProxy m_playerProxy;


        QueueInfo m_mainQueueInfo = null;//主队列
        QueueInfo m_secendQueueInfo = null;//第二队列
        BuildQueueState m_mainQueueState = BuildQueueState.LEISURE1;
        BuildQueueState m_secendQueueState = BuildQueueState.LEISURE1;
        #endregion

        // Use this for initialization
        public WorkerProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" WorkerProxy register");
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        public void Init()
        {
           
        }

        public override void OnRemove()
        {
            Debug.Log(" WorkerProxy remove");
        }



        public void UpdateBuildQueue()
        {
            m_playerProxy.GetBuildQueue().TryGetValue(1, out m_mainQueueInfo);
            m_playerProxy.GetBuildQueue().TryGetValue(2, out m_secendQueueInfo);
            long finishTime = -1;
            finishTime = m_mainQueueInfo.finishTime;
            
            if (finishTime == -1 || m_mainQueueInfo.buildingIndex == 0)
            {
                m_mainQueueState = BuildQueueState.LEISURE1;
            }
            else
            {
                m_mainQueueState = BuildQueueState.NOLEISURE;
            }

            if (m_secendQueueInfo == null)
            {
                m_secendQueueState = BuildQueueState.LOCK;
            }
            else
            {
                if (m_secendQueueInfo.expiredTime == -1 || m_secendQueueInfo.expiredTime >= ServerTimeModule.Instance.GetServerTime())
                {
                    if (m_secendQueueInfo.finishTime == -1 || m_secendQueueInfo.buildingIndex == 0)
                    {
                        m_secendQueueState = BuildQueueState.LEISURE1;
                    }
                    else
                    {
                        m_secendQueueState = BuildQueueState.NOLEISURE;
                    }
                }
                else
                {
                    m_secendQueueState = BuildQueueState.LOCK;
                }

            }
        }
        public BuildQueueState GetMainQueueState()
        {
            return m_mainQueueState;
        }
        /// <summary>
        /// 获取当前开启的条数
        /// </summary>
        /// <returns></returns>
        public int GetBuildQueueCount()
        {
            int count = 1;
            if (m_secendQueueState != BuildQueueState.LOCK)
            {
                count = 2;
            }
            return count;
        }
        public BuildQueueState GetSecendQueueState(long finishTime = 0)
        {
            BuildQueueState state = m_secendQueueState;
            if (finishTime != 0)
            {
                switch (state)
                {
                    case BuildQueueState.LEISURE1:
                        {
                            if (m_secendQueueInfo.expiredTime == -1)
                            {
                                state = BuildQueueState.LEISURE1;
                            }
                            else
                            {
                                if (m_secendQueueInfo.expiredTime < finishTime + ServerTimeModule.Instance.GetServerTime())
                                {
                                    state = BuildQueueState.LEISURE2;
                                }
                                else
                                {
                                    state = BuildQueueState.LEISURE1;
                                }
                            }
                             
                        }
                        break;
                    default:
                        break;
                }
            }
            return state;
        }

        public BuildQueueState GetSecendQueueState()
        {
            return m_secendQueueState;
        }
    }
}