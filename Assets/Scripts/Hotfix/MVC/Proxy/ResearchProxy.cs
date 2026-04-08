// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, December 31, 2019
// Update Time         :    Tuesday, December 31, 2019
// Class Description   :    ResearchProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game {

    public class EResearchType
    {
        //经济
        public static int economy = 1;
        
        //军事
        public static int military = 2;

    }

    public class ResearchProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "ResearchProxy";


        private List<StudyDefine> m_researchs = null;
        private Dictionary<int, StudyDefine> m_mapResearchs = new Dictionary<int, StudyDefine>();
        
        
        private Dictionary<int,StudyDataDefine> m_StudyDatas = new Dictionary<int, StudyDataDefine>();
        
        //大经济军事映射
        private Dictionary<int,List<List<StudyDefine>>> m_resTypes = new Dictionary<int, List<List<StudyDefine>>>(5);
        
        //小类型映射
        
        private Dictionary<int,List<StudyDefine>> m_resStudyTypes = new Dictionary<int, List<StudyDefine>>();
        
        //最大等级
        private Dictionary<int,int> m_resMaxLevel = new Dictionary<int, int>();
        //子集映射
        
        private Dictionary<int,List<StudyDefine>> m_resSub = new Dictionary<int, List<StudyDefine>>();

        private PlayerProxy m_playerProxy;
        
        #endregion

        // Use this for initialization
        public ResearchProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" ResearchProxy register");   
            
            
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }


        public override void OnRemove()
        {
            Debug.Log(" ResearchProxy remove");
        }


        public StudyDataDefine IsSoldierRes(int resID)
        {
            if (m_StudyDatas.Count == 0)
            {
                var datas = CoreUtils.dataService.QueryRecords<StudyDataDefine>();

                foreach (var data in datas)
                {
                    m_StudyDatas.Add(data.civilizationID*1000000 + data.studyID,data);
                }
            }
            int countryId = (int)m_playerProxy.CurrentRoleInfo.country*1000000 +resID;
            StudyDataDefine st;
            m_StudyDatas.TryGetValue(countryId, out st);
            
            return st;
        }


        public List<StudyDefine> Technologys()
        {
            if (m_researchs== null)
            {
                m_researchs = CoreUtils.dataService.QueryRecords<StudyDefine>();
                var len = m_researchs.Count;
                m_mapResearchs = new Dictionary<int, StudyDefine>(len);
                for (int i = 0; i < len ; i++)
                {
                    m_mapResearchs.Add(m_researchs[i].ID,m_researchs[i]);
                }
            }

            return m_researchs;
        }


       
        /// <summary>
        /// 获取所有研究的第一个等级
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<List<StudyDefine>> GetTechnologyByType(int type)
        {
            if (m_resTypes.ContainsKey(type))
            {
                return m_resTypes[type];
            }

            var list = new List<List<StudyDefine>>();
            var ress = Technologys();
            var len = ress.Count;

            for (int i = 0; i < len; i++)
            {
                var res = ress[i];
                if (res.type == type && res.studyLv == 1)
                {
                    //TODO fix
                    var lv = res.columns;
                    if (list.Count < lv)
                    {
                        for (int j = 0; j < lv - list.Count; j++)
                        {
                            list.Add(new List<StudyDefine>());
                        }
                        
                    }
                    list[lv-1].Add(res);
                    
                    
                    
                    //子集映射
                    if (res.preconditionStudy1>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy1))
                        {
                            m_resSub.Add(res.preconditionStudy1, new List<StudyDefine>());
                        }

                        m_resSub[res.preconditionStudy1].Add(res);
                    }
                
                    if (res.preconditionStudy2>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy2))
                        {
                            m_resSub.Add(res.preconditionStudy2, new List<StudyDefine>());
                        }

                        m_resSub[res.preconditionStudy2].Add(res);
                    }
                
                    if (res.preconditionStudy3>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy3))
                        {
                            m_resSub.Add(res.preconditionStudy3, new List<StudyDefine>());
                        }

                        m_resSub[res.preconditionStudy3].Add(res);
                    }
                
                    if (res.preconditionStudy4>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy4))
                        {
                            m_resSub.Add(res.preconditionStudy4, new List<StudyDefine>());
                        }

                        m_resSub[res.preconditionStudy4].Add(res);
                    }
                }
                //保存最大值
                if (m_resMaxLevel.ContainsKey(res.studyType))
                {
                    if (m_resMaxLevel[res.studyType] < res.studyLv)
                    {
                        m_resMaxLevel[res.studyType] = res.studyLv;
                    }
                }
                else
                {
                    m_resMaxLevel.Add(res.studyType,res.studyLv);
                }
                
                //级别分类
                if (!m_resStudyTypes.ContainsKey(res.studyType))
                {
                    m_resStudyTypes.Add(res.studyType, new List<StudyDefine>());
                }

                if (!m_resStudyTypes[res.studyType].Contains(res))
                {
                    m_resStudyTypes[res.studyType].Add(res);
                }
                
                
              
            }
            m_resTypes[type] = list;
            return list;
        }

        
        //获取所有级别数据
        public List<StudyDefine> GetTechnologyList(int studyType)
        {

            if (m_resStudyTypes.Count == 0)
            {
                GetTechnologyByType(EResearchType.economy);
                GetTechnologyByType(EResearchType.military);
            }
            
            List<StudyDefine> data;
            m_resStudyTypes.TryGetValue(studyType, out data);
            
            return data;
        }
        
        //获取下一级的的
        public StudyDefine GetTechnologyNextLevel(StudyDefine studyDefine)
        {
            List<StudyDefine> list = GetTechnologyList(studyDefine.studyType);
            int next = studyDefine.studyLv+1;
            if (list.Count>=next)
            {
                return list[next-1];
            }

            return studyDefine;
        }

        //获取前一个级数据
        public StudyDefine GetPerTechnology(int studyType, int level)
        {
            List<StudyDefine> list = GetTechnologyList(studyType);
            
            if (list.Count>=level)
            {
                return list[level-1];
            }
            return null;
        }


        public int GuessNestTechnology()
        {
            foreach (var studyDefine in Technologys())
            {
                var level = GetCrrTechnologyLv(studyDefine.studyType);
                if ( level== 0 || level< GetTechnologyMaxLv(studyDefine.studyType))
                {
                    return studyDefine.studyType;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取科技等级的战力
        /// </summary>
        /// <param name="studyType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public StudyDefine GetTechnologyByLevel(int studyType, int level)
        {
            List<StudyDefine> list = GetTechnologyList(studyType);
            
            if (list !=null && list.Count>=level)
            {
                return list[level-1];
            }
            return null;
        }


        //获取最大等级
        public int GetTechnologyMaxLv(int studyType)
        {

            return m_resMaxLevel[studyType];
        }

        //服务器数据
        public int GetCrrTechnologyLv(int studyType)
        {
            
            var technologies = m_playerProxy.GetTechnologies();

            if (technologies == null || !technologies.ContainsKey(studyType))
            {
                return 0;
            }  
            
            return (int)technologies[studyType].level;
        }

        /// <summary>
        /// 获取科技战力
        /// </summary>
        /// <returns></returns>
        public long GetTechnologyPower()
        {
            var technologies = m_playerProxy.GetTechnologies();

            if (technologies == null)
            {
                return 0;
            }

            long v = 0;

            foreach (var technology in technologies)
            {
                var tinfo = GetTechnologyByLevel((int)technology.Key, (int)technology.Value.level);
                if (tinfo!=null)
                {
                    v += tinfo.power;
                }
            }
            
            return v;
        }





        public QueueInfo GetCrrTechnologying()
        {
            QueueInfo info = m_playerProxy.GetTechnologieQueue();
            return info;
        }


        public List<StudyDefine> GetTechnologySub(int studyType)
        {
            if (m_resSub.ContainsKey(studyType))
            {
                return m_resSub[studyType];
            }

            return null;
        }

        //领取科技
        public void SendEndTechnology()
        {
            Debug.Log("领取科技");
            Technology_AwardTechnology.request req = new Technology_AwardTechnology.request();
            
            AppFacade.GetInstance().SendSproto(req);
        }

        public void StopTechnology()
        {
            Debug.Log("停止科技");
            Technology_StopTechnology.request req = new Technology_StopTechnology.request();
            
            AppFacade.GetInstance().SendSproto(req);
        }


        public void StudyTechnology(long studyType ,bool now)
        {
            Technology_ResearchTechnology.request request = new Technology_ResearchTechnology.request();

            request.technologyType = studyType;
            request.immediately = now;
            
            Debug.LogFormat("研究科技{0} {1}",studyType,now);
                
            AppFacade.GetInstance().SendSproto(request);
        }


        /// <summary>
        /// 获取未解锁的所有前置条件
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        public List<StudyDefine> CheckPreAllTechnology(StudyDefine study)
        {
            var list = new List<StudyDefine>();


            if (study.preconditionStudy1>0)
            {
                if (!this.PreTechnologyIsUnlock(study.preconditionStudy1,study.preconditionLv1))
                {
                    list.Add(this.GetPerTechnology(study.preconditionStudy1,study.preconditionLv1));
                }
            }
            if (study.preconditionStudy2>0)
            {
                if (!this.PreTechnologyIsUnlock(study.preconditionStudy2,study.preconditionLv2))
                {
                    list.Add(this.GetPerTechnology(study.preconditionStudy2,study.preconditionLv2));
                }
            }
            if (study.preconditionStudy3>0)
            {
                if (!this.PreTechnologyIsUnlock(study.preconditionStudy3,study.preconditionLv3))
                {
                    list.Add(this.GetPerTechnology(study.preconditionStudy3,study.preconditionLv3));
                }
            }
            if (study.preconditionStudy4>0)
            {
                if (!this.PreTechnologyIsUnlock(study.preconditionStudy4,study.preconditionLv4))
                {
                    list.Add(this.GetPerTechnology(study.preconditionStudy4,study.preconditionLv4));
                }
            }

            return list;
        }


        
        
        private bool PreTechnologyIsUnlock(int studyType,int level = 1)
        {
            return GetCrrTechnologyLv(studyType) >= level;
        }
        
        


    }
    
   


}