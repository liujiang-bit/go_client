using System.Collections.Generic;
using Data;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    
    public class EAllianceResearchType
    {
        //发展
        public static int develop = 1;
        
        //领土
        public static int territory = 2;
        
        //战争
        public static int war = 3;
        
        //skill
        public static int skill = 4;

    }

    public class AllianceAttributeInfo
    {

        public AllianceResarchProxy proxy;
        
        private long value;

        public long Multi
        {
            get
            {
                if (recalculate)
                {
                    value = 0;
                    int index = 0;
                    mapList.ForEach((st) =>
                    {
                        int lv = proxy.GetCrrTechnologyLv(st.studyType);
                        if (lv>0)
                        {
                            var lvSt = proxy.GetTechnologyByLevel(st.studyType,lv);
                            var bufIndex = mapIndex[index];

                            if (bufIndex<=lvSt.buffData.Count-1)
                            {
                                value += lvSt.buffData[bufIndex];

                            }
                            else
                            {
                                Debug.LogError("联盟科技加成参数不匹配"+st.studyType+"  "+lvSt.buffData.Count+"  "+bufIndex);
                            }
                        }
                        index++;
                    });

                    recalculate = false;
                }
                return value;
            }
        }

        public bool recalculate = true;
        
        public List<AllianceStudyDefine> mapList = new List<AllianceStudyDefine>();
        public List<int> mapIndex = new List<int>();

    }

    public class AllianceResarchProxy: GameProxy {

        #region Member
        public const string ProxyNAME = "AllianceResarchProxy";


        private List<AllianceStudyDefine> m_researchs = null;
        private Dictionary<int, AllianceStudyDefine> m_mapResearchs = new Dictionary<int, AllianceStudyDefine>();
        
        
        private Dictionary<int,StudyDataDefine> m_StudyDatas = new Dictionary<int, StudyDataDefine>();
        
        //大经济军事映射
        private Dictionary<int,List<List<AllianceStudyDefine>>> m_resTypes = new Dictionary<int, List<List<AllianceStudyDefine>>>(5);
        
        //小类型映射
        
        private Dictionary<int,List<AllianceStudyDefine>> m_resStudyTypes = new Dictionary<int, List<AllianceStudyDefine>>();
        
        //最大等级
        private Dictionary<int,int> m_resMaxLevel = new Dictionary<int, int>();
        //子集映射
        
        private Dictionary<int,List<AllianceStudyDefine>> m_resSub = new Dictionary<int, List<AllianceStudyDefine>>();

        private PlayerProxy m_playerProxy;
        
        #endregion



        #region 联盟属性加成

        
        
        
        public Dictionary<allianceAttrType,AllianceAttributeInfo> m_guildAttr = new Dictionary<allianceAttrType,AllianceAttributeInfo>();



        public long GetAttrMulti(allianceAttrType type,long baseCost)
        {
            Technologys();
            AllianceAttributeInfo attributeInfo;

            if (m_guildAttr.TryGetValue(type,out attributeInfo))
            {
                return (long)(baseCost*(1000f+attributeInfo.Multi)/1000f);
            }

            return baseCost;
        }

        private void ResetAttrMulti()
        {
            foreach (var kv in m_guildAttr)
            {
                kv.Value.recalculate = true;
            }
        }

        #endregion

        // Use this for initialization
        public AllianceResarchProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" AllianceResarchProxy register");   
            
            
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }


        public override void OnRemove()
        {
            Debug.Log(" AllianceResarchProxy remove");
        }



        public List<AllianceStudyDefine> Technologys()
        {
            if (m_researchs== null)
            {
                m_researchs = CoreUtils.dataService.QueryRecords<AllianceStudyDefine>();
                var len = m_researchs.Count;
                m_mapResearchs = new Dictionary<int, AllianceStudyDefine>(len);
                for (int i = 0; i < len ; i++)
                {
                    var st = m_researchs[i];
                    m_mapResearchs.Add(st.ID,st);


       
                    if (st.allianceBuffTypeNew!=null && st.allianceBuffTypeNew.Count>0 && st.studyLv==1)
                    {
                        int index = 0;
                        st.allianceBuffTypeNew.ForEach((type =>
                        {
                            AllianceAttributeInfo attrList;
                            if (!m_guildAttr.TryGetValue(type, out attrList))
                            {
                                attrList = new AllianceAttributeInfo();
                                attrList.proxy = this;
                                m_guildAttr.Add(type,attrList);
                            }
                            
                            attrList.mapList.Add(st);
                            attrList.mapIndex.Add(index);
                            index++;
                        }));
                    }
                    
                }
            }

            return m_researchs;
        }


       
        /// <summary>
        /// 获取所有研究的第一个等级
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<List<AllianceStudyDefine>> GetTechnologyByType(int type)
        {
            if (m_resTypes.ContainsKey(type))
            {
                return m_resTypes[type];
            }

            var list = new List<List<AllianceStudyDefine>>();
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
                            list.Add(new List<AllianceStudyDefine>());
                        }
                        
                    }
                    list[lv-1].Add(res);
                    
                    
                    
                    //子集映射
                    if (res.preconditionStudy1>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy1))
                        {
                            m_resSub.Add(res.preconditionStudy1, new List<AllianceStudyDefine>());
                        }

                        m_resSub[res.preconditionStudy1].Add(res);
                    }
                
                    if (res.preconditionStudy2>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy2))
                        {
                            m_resSub.Add(res.preconditionStudy2, new List<AllianceStudyDefine>());
                        }

                        m_resSub[res.preconditionStudy2].Add(res);
                    }
                
                    if (res.preconditionStudy3>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy3))
                        {
                            m_resSub.Add(res.preconditionStudy3, new List<AllianceStudyDefine>());
                        }

                        m_resSub[res.preconditionStudy3].Add(res);
                    }
                
                    if (res.preconditionStudy4>0)
                    {
                        if (!m_resSub.ContainsKey(res.preconditionStudy4))
                        {
                            m_resSub.Add(res.preconditionStudy4, new List<AllianceStudyDefine>());
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
                    m_resStudyTypes.Add(res.studyType, new List<AllianceStudyDefine>());
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
        public List<AllianceStudyDefine> GetTechnologyList(int studyType)
        {

            if (m_resStudyTypes.Count == 0)
            {
                GetTechnologyByType(EResearchType.economy);
                GetTechnologyByType(EResearchType.military);
            }
            
            List<AllianceStudyDefine> data;
            m_resStudyTypes.TryGetValue(studyType, out data);
            
            return data;
        }
        
        //获取下一级的的
        public AllianceStudyDefine GetTechnologyNextLevel(AllianceStudyDefine AllianceStudyDefine)
        {
            List<AllianceStudyDefine> list = GetTechnologyList(AllianceStudyDefine.studyType);
            int next = AllianceStudyDefine.studyLv+1;
            if (list.Count>=next)
            {
                return list[next-1];
            }

            return AllianceStudyDefine;
        }

        //获取前一个级数据
        public AllianceStudyDefine GetPerTechnology(int studyType, int level)
        {
            List<AllianceStudyDefine> list = GetTechnologyList(studyType);
            
            if (list.Count>=level)
            {
                return list[level-1];
            }
            return null;
        }
        
        /// <summary>
        /// 获取科技等级的战力
        /// </summary>
        /// <param name="studyType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public AllianceStudyDefine GetTechnologyByLevel(int studyType, int level)
        {
            List<AllianceStudyDefine> list = GetTechnologyList(studyType);
            
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
            

            if (m_techs == null || !m_techs.ContainsKey(studyType))
            {
                return 0;
            }  
            
            return (int)m_techs[studyType].level;    
        }

//        /// <summary>
//        /// 获取科技战力
//        /// </summary>
//        /// <returns></returns>
//        public long GetTechnologyPower()
//        {
//            var technologies = m_playerProxy.GetTechnologies();
//
//            if (technologies == null)
//            {
//                return 0;
//            }
//
//            long v = 0;
//
//            foreach (var technology in technologies)
//            {
//                var tinfo = GetTechnologyByLevel((int)technology.Key, (int)technology.Value.level);
//                if (tinfo!=null)
//                {
//                    v += tinfo.;
//                }
//            }
//            
//            return v;
//        }

        private Dictionary<long,GuildTechnologyInfoEntity> m_techs = new Dictionary<long, GuildTechnologyInfoEntity>();

        private long m_markTechType = 0;

        private long m_researchTechType;

        private long m_researchTime;

        private long m_donateNum;


        public void Clear()
        {
            m_techs.Clear();
            m_markTechType = 0;
            m_researchTechType = 0;
            m_researchTime = 0;
            m_donateNum = 0;
            
        }

        public void UpdateTech(Guild_GuildTechnologies.request request)
        {
            if (request.HasTechnologies)
            {
                foreach (var kv in request.technologies)
                {
                    var type = kv.Key;

                    GuildTechnologyInfoEntity info;

                    if (!m_techs.TryGetValue(type,out info))
                    {
                        info = new GuildTechnologyInfoEntity();
                        info.type = type;
                        m_techs.Add(type,info);
                    }

                    GuildTechnologyInfoEntity.updateEntity(info, kv.Value);
                }
                ResetAttrMulti();
            }
            
            if (request.HasRecommendTechnologyType)
            {
                m_markTechType = request.recommendTechnologyType;
            }
            
            if (request.HasResearchTechnologyType)
            {
                m_researchTechType = request.researchTechnologyType;
            }
            
            if (request.HasResearchTime)
            {
                m_researchTime = request.researchTime;
            }

            if (request.HasDonateNum)
            {
                m_donateNum = request.donateNum;
            }
            
            Debug.Log(m_markTechType+"联盟科技"+m_researchTime+" m_researchTechType:"+m_researchTechType+" m_donateNum： "+m_donateNum+"  m_markTechType"+m_markTechType);
            
            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceTechUpdate);
        }

        public long GetMarkType()
        {
            return m_markTechType;
        }


        public long GetDonateNum()
        {
            return m_donateNum;
        }


        public long GetCrrTechnologying()
        {
            return m_researchTechType;
        }

        public long GetTechEndTime()
        {
            return m_researchTime;
        }


        public List<AllianceStudyDefine> GetTechnologySub(int studyType)
        {
            if (m_resSub.ContainsKey(studyType))
            {
                return m_resSub[studyType];
            }

            return null;
        }

        //设置联盟推荐科技
        public void SendSetMarkTechnology(long studyType)
        {
            Debug.Log("设置联盟推荐科技"+studyType);
            Guild_RecommendTechnology.request req = new Guild_RecommendTechnology.request();
            req.technologyType = studyType;
            
            AppFacade.GetInstance().SendSproto(req);
        }

        //获取联盟科技捐献点信息
        public void SendGetDonate(long technologyType)
        {
            Debug.Log("获取联盟科技捐献点信息");
            Guild_GetTechnologyDonate.request req = new Guild_GetTechnologyDonate.request();
            req.technologyType = technologyType;
            
            AppFacade.GetInstance().SendSproto(req);
        }
        
        //联盟科技捐献 #1 资源捐献 2 代币捐献
        public void SendDonateTechnology(int dType,int techType)
        {
            Debug.Log(dType+"联盟科技捐献"+techType);
            Guild_DonateTechnology.request req = new Guild_DonateTechnology.request();
            req.donateType = dType;
            req.technologyType = techType;
            
            AppFacade.GetInstance().SendSproto(req);
        }


        public void SendStudyTechnology(long studyType )
        {
            Guild_ResearchTechnology.request request = new Guild_ResearchTechnology.request();

            request.technologyType = studyType;
            
            Debug.LogFormat("研究联盟科技{0}",studyType);
                
            AppFacade.GetInstance().SendSproto(request);
        }


        /// <summary>
        /// 获取未解锁的所有前置条件
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        public List<AllianceStudyDefine> CheckPreAllTechnology(AllianceStudyDefine study)
        {
            var list = new List<AllianceStudyDefine>();


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