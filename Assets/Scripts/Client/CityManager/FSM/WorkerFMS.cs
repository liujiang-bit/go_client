using System;
using System.Collections.Generic;
//using Facebook.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Client.FSM
{
    public enum WorkerState
    {
        IDLE,
        WalkAround, //游走
        Building, //建筑中
        GOCarry, //去帮运中
        BackCarry,//搬运回来
        FindHome,//训练士兵找家
        RunToCitizen,//逃跑
        
    }

    public class WorkerFMS : MonoFSM
    {

        private bool debug = false;
        
        public People Owner { get; set; }


        public Vector2 BornPos = Vector2.negativeInfinity;


        public List<Vector2> FindPaths = new List<Vector2>();

        public TownSearch Finder;

        public long buildType = 0;


        public long buildIndex = 0;

        public long ResBuildIndex = 0;
        
        public int index;

        protected int pathIndex = 0;

        //建筑外围
        private List<Vector2> m_workerBuildOutTilePaths;
        private List<Vector2> m_workerBuildOutPaths = new List<Vector2>();

        private List<Vector2> m_getResBuildOutTilePaths;
        private List<Vector2> m_getResBuildOutPaths = new List<Vector2>();

        private List<Vector2> m_storageBuildOutPaths = new List<Vector2>();

        private List<People.ENMU_CARRY_RESOURCE_TYPE> m_resList;
        private int m_resTypeIndex = -1;

        private Vector2 startWorldPos;
        private Vector2 endWorldPos;

        private Vector2 startPos;
        private Vector2 endPos;
        public Vector2 buildDirPos;

        private Vector2 resBuildDirPos;

        private WorkerState m_crrWorkState = WorkerState.WalkAround;

        private People.ENMU_CITIZEN_STAT m_crrWalkState;

        private People.ENMU_CARRY_RESOURCE_TYPE m_crrResType = People.ENMU_CARRY_RESOURCE_TYPE.NUM;

        public People.ENMU_CITIZEN_STAT CrrWalkState
        {
            get => m_crrWalkState;
            set => m_crrWalkState = value;
        }


        /// <summary>
        /// 
        /// </summary>
        private int m_buildIndexStart;

        private int m_buildIndexEnd;
        private bool reverseBuild;

        public WorkerState CrrWorkState
        {
            get => m_crrWorkState;
            set => m_crrWorkState = value;
        }

        public float CrrMoveSpeed
        {
            get
            {
                switch (m_crrWorkState)
                {
                    case WorkerState.WalkAround:
                        return WalkSpeed;
                    case WorkerState.Building:
                    case WorkerState.GOCarry:
                    case WorkerState.BackCarry:
                        return WorkSpeed;
                    case WorkerState.RunToCitizen:
                        return RunSpeed;
                    case WorkerState.FindHome:
                        return RunSpeed;
                    case WorkerState.IDLE:
                        return 0f;
                }

                return 0f;
            }
        }


        //走路速度
        public static float WalkSpeed = 0.00015f * 1000f;

        //工作
        public static float WorkSpeed = 0.00025f * 1000f;

        //跑步
        public static float RunSpeed = 0.0004f * 1000f;

        public override void AddStates()
        {
            //set the custom update frequenct
            SetUpdateFrequency(0.1f);


            //add the states
            AddState<WorkerIdeState>();
            AddState<WorkerPatrolState>();
            AddState<WorkerBuildingState>();

            //set the initial state
            SetInitialState<WorkerPatrolState>();
        }

        public void Idle()
        {
            CrrWorkState = WorkerState.IDLE;
            CrrWalkState = People.ENMU_CITIZEN_STAT.IDLE;


            ChangeState<WorkerIdeState>();
            People.SetStateS(Owner, CrrWalkState, StepEnd(), StepEnd(), CrrMoveSpeed);
        }
        /// <summary>
        /// 清楚状态等待下一个指令
        /// </summary>
        public void Clear()
        {
            wait = true;
            buildIndex = 0;
            ResBuildIndex = 0;
            FindPaths.Clear();
            CrrWalkState = People.ENMU_CITIZEN_STAT.NUMBER;
            m_filrCountCallBack = null;
        }
        bool wait = false;

        public void Buildering()
        {
            CrrWorkState = WorkerState.Building;


            if (m_crrResType == People.ENMU_CARRY_RESOURCE_TYPE.BUCKET)
            {
                CrrWalkState = People.ENMU_CITIZEN_STAT.FIREFIGHTING;
            }
            else
            {
                CrrWalkState = People.ENMU_CITIZEN_STAT.BUILD;
            }

            

            ChangeState<WorkerBuildingState>();
            People.SetStateS(Owner, CrrWalkState, StepEnd(), buildDirPos, CrrMoveSpeed);
        }

        //切换到建筑休息模式
        public void FetchingRes()
        {
            CrrWorkState = WorkerState.GOCarry;
            CrrWalkState = People.ENMU_CITIZEN_STAT.IDLE;

            ChangeState<WorkerBuildingState>();
            People.SetStateS(Owner, CrrWalkState, StepEnd(), buildDirPos, CrrMoveSpeed);
        }

        //切换拉取回去的动作
        public void TakeBaskResBuilding()
        {
            if (buildIndex != 0)
            {
                CrrWorkState = WorkerState.BackCarry;
                if (m_crrResType == People.ENMU_CARRY_RESOURCE_TYPE.BUCKET)
                {
                    CrrWalkState = People.ENMU_CITIZEN_STAT.FIREFIGHTING;
                }
                else
                {
                    CrrWalkState = People.ENMU_CITIZEN_STAT.BUILD;
                }


                m_crrResType = People.ENMU_CARRY_RESOURCE_TYPE.NONE;

                ChangeState<WorkerBuildingState>();
                People.SetStateS(Owner, CrrWalkState, StepEnd(), buildDirPos, CrrMoveSpeed, m_crrResType);
            }
        }

        public void MoveNextPos()
        {
            People.SetStateS(Owner, CrrWalkState, StepStart(), StepEnd(), CrrMoveSpeed,m_crrResType);


            
            if (debug && FindPaths !=null && pathIndex<FindPaths.Count-2)
            {
                Debug.Log(m_crrWorkState+"  "+FindPaths[pathIndex]+" => "+FindPaths[pathIndex+1]+"  pathIndex:"+pathIndex+"/"+FindPaths.Count+ " WorkIndex:"+index);
            }
        }

        public void HideBuilder()
        {
//            Citizen.FadeOutS(Owner,false);
            Owner.gameObject.SetActive(false);
        }



        public void SetBorn(Vector2 born)
        {
            this.BornPos = born;
            Owner.transform.localPosition = Finder.ConvertTileToLocal(BornPos.x, BornPos.y);
        }

        public void RunToCitizen(Vector2 endPos)
        {
            if (m_filrCountCallBack != null)
            {
                m_filrCountCallBack = null;
            }
               buildIndex = 0;
            ResBuildIndex = 0;
            if (CrrWorkState == WorkerState.RunToCitizen)
            {
                return;
            }
            m_crrResType = People.ENMU_CARRY_RESOURCE_TYPE.NONE;
            
            startPos = Finder.ConvertLocalToTile(Owner.transform.localPosition);

            List<Vector2> findPath;
            int count = 0;
            do
            {
                findPath = Finder.GetFindWorldPaths(startPos, endPos);

                if (count < 2)
                {
                    count++;
                }
                else
                {
                    if (debug)
                    {
                        Debug.Log("死循环了" + startPos+ "WorkIndex:"+index);
                    }

                }
            } while (findPath.Count == 0 && count<2);

      
            CrrWorkState = WorkerState.RunToCitizen;
            CrrWalkState = People.ENMU_CITIZEN_STAT.RUN;
            
            if (findPath.Count == 0)
            {
                Idle();
                return;
            }

            FindPaths = findPath;
            
            FindPaths.Add(FindPaths[FindPaths.Count-1]+new Vector2(-0.1f,0.1f));

            pathIndex = 0;
            
            ChangeState<WorkerPatrolState>();
        }
        
        
        //去建筑边上灭火
        public void GoOnAroundFireBuild()
        {
            List<Vector2> buildOutSide = new List<Vector2>();
            long buildIndex = 0;
            this.buildIndex = buildIndex;
            
            m_workerBuildOutTilePaths = buildOutSide;
            m_workerBuildOutPaths = Finder.ConverTilePathToWorldPath(buildOutSide);

            buildDirPos = (m_workerBuildOutPaths[0]+m_workerBuildOutPaths[m_workerBuildOutPaths.Count/2])/2;
            pathIndex = 0;


            

            int nestPoint = 0;
            float mindis = 100000;
            
            Vector2 playerPos = new Vector2(Owner.transform.position.x,Owner.transform.position.z);
            
            for (int i = 1; i < m_workerBuildOutPaths.Count; i++)
            {
                var crrDis = Vector2.Distance(m_workerBuildOutPaths[i], playerPos);
                if (crrDis < mindis)
                {
                    mindis = crrDis;
                    nestPoint = i;
                }
            }

            if (debug)
            {
                Debug.Log(Owner.transform.position+"建筑最近的位置"+m_buildIndexStart+" index: "+index);
            }
            
            m_buildIndexStart = nestPoint;
            m_buildIndexEnd = nestPoint;

            //重新巡路到建筑位置上
            var startPos = Finder.ConvertLocalToTile(Owner.transform.localPosition);

            endPos = m_workerBuildOutTilePaths[m_buildIndexStart];
            FindPaths = Finder.GetFindWorldPaths(startPos, endPos);

            CrrWorkState = WorkerState.Building;
            CrrWalkState = People.ENMU_CITIZEN_STAT.MOVE;

            //MoveNextPos();

            if (debug)
            {
                Debug.Log(startPos + "=>" + endPos + "WorkIndex:"+index+" 走到建筑去  当前距离建筑步数:" + FindPaths.Count + "  随机建筑点:" +
                          m_buildIndexStart + "/" + m_workerBuildOutPaths.Count);
            }
            
            ChangeState<WorkerPatrolState>();

        }


        /// <summary>
        /// 随机游走
        /// </summary>
        public void WalkAround()
        {
            buildIndex = 0;
            ResBuildIndex = 0;

            m_crrResType = People.ENMU_CARRY_RESOURCE_TYPE.NONE;
            
            startPos = Finder.ConvertLocalToTile(Owner.transform.localPosition);

            List<Vector2> findPath;
            int count = 0;
            do
            {
                endPos = Finder.GetRandPos();
                findPath = Finder.GetFindWorldPaths(startPos, endPos);

                if (count < 2)
                {
                    count++;
                }
                else
                {
                    if (debug)
                    {
                        Debug.Log("死循环了" + startPos+ "WorkIndex:"+index);
                    }

                }
            } while (findPath.Count == 0 && count<2);

            
            CrrWorkState = WorkerState.WalkAround;
            CrrWalkState = People.ENMU_CITIZEN_STAT.MOVE;
            
            if (findPath.Count == 0)
            {
                //Idle();
                return;
            }

            FindPaths = findPath;

            pathIndex = 0;

        }

        public void GoOnWalkAround()
        {
            WalkAround();
            ChangeState<WorkerPatrolState>();
        }

        public void CheckWaklAroundEndPoint()
        {
            if (CrrWorkState == WorkerState.WalkAround)
            {
                if (Finder.CheckEndPointCanWalk(endPos)==false)
                {
                    
                    Debug.Log(index+" 重新寻路"+Owner.transform.localPosition);
                    Idle();
                }
            }
        }




        //去建筑
        public void GoOnAroundBuild(List<Vector2> buildOutSide,long buildIndex)
        {
            this.buildIndex = buildIndex;
            
            m_workerBuildOutTilePaths = buildOutSide;
            m_workerBuildOutPaths = Finder.ConverTilePathToWorldPath(buildOutSide);

            buildDirPos = (m_workerBuildOutPaths[0]+m_workerBuildOutPaths[m_workerBuildOutPaths.Count/2])/2;
            pathIndex = 0;


            

            int nestPoint = 0;
            float mindis = 100000;
            
            Vector2 playerPos = new Vector2(Owner.transform.position.x,Owner.transform.position.z);
            
            for (int i = 1; i < m_workerBuildOutPaths.Count; i++)
            {
                var crrDis = Vector2.Distance(m_workerBuildOutPaths[i], playerPos);
                if (crrDis < mindis)
                {
                    mindis = crrDis;
                    nestPoint = i;
                }
            }

            if (debug)
            {
                Debug.Log(Owner.transform.position+"建筑最近的位置"+m_buildIndexStart+" index: "+index);
            }
            
            m_buildIndexStart = nestPoint;
            m_buildIndexEnd = nestPoint;

            //重新巡路到建筑位置上
            var startPos = Finder.ConvertLocalToTile(Owner.transform.localPosition);

            endPos = m_workerBuildOutTilePaths[m_buildIndexStart];
            FindPaths = Finder.GetFindWorldPaths(startPos, endPos);

            CrrWorkState = WorkerState.Building;
            CrrWalkState = People.ENMU_CITIZEN_STAT.MOVE;

            //MoveNextPos();

            if (debug)
            {
                Debug.Log(startPos + "=>" + endPos + "WorkIndex:"+index+" 走到建筑去  当前距离建筑步数:" + FindPaths.Count + "  随机建筑点:" +
                      m_buildIndexStart + "/" + m_workerBuildOutPaths.Count);
            }
            
            ChangeState<WorkerPatrolState>();

        }

        //围绕的建筑开始走路，停止的时候开始播建筑动作
        public void AroundBuild()
        {
            CrrWorkState = WorkerState.Building;
            CrrWalkState = People.ENMU_CITIZEN_STAT.MOVE;

           

            if (m_buildIndexEnd > 0 && FindPaths.Count>0)
            {
//                Debug.Log(buildIndex+" 建筑AroundBuildn"+m_buildIndexStart+"  work:"+index);
                m_buildIndexStart = m_buildIndexEnd;
            }
            
            FindPaths.Clear();

            if (m_buildIndexStart <= m_workerBuildOutPaths.Count / 2)
            {
                m_buildIndexEnd = Random.Range(m_buildIndexStart + 1, m_workerBuildOutPaths.Count);

                for (int i = m_buildIndexStart; i <= m_buildIndexEnd; i++)
                {
                    FindPaths.Add(m_workerBuildOutPaths[i]);
                }
            }
            else
            {
                m_buildIndexEnd = Random.Range(1, m_buildIndexStart);
                for (int i = m_buildIndexStart; i >= m_buildIndexEnd; i--)
                {
                    FindPaths.Add(m_workerBuildOutPaths[i]);
                }
            }

            pathIndex = 0;

            if (debug)
            {
                Debug.Log(m_buildIndexStart + "=>" + m_buildIndexEnd + " 建筑走圈  " + FindPaths.Count+ "WorkIndex:"+index);
            }
            ChangeState<WorkerPatrolState>();
        }
        

        //去建筑物提前资源到建筑去
        public void GoGetResToBuild(List<Vector2> resBuildOutSide, List<Vector2> buildOutSide,long buildID,long resBuildID,List<People.ENMU_CARRY_RESOURCE_TYPE> resourceTypes)
        {

            
            
            if (buildID>0)
            {
                this.buildIndex = buildID;
                ResBuildIndex = resBuildID;
            }

            if (resBuildOutSide!=null)
            {
                m_getResBuildOutTilePaths = resBuildOutSide;
                m_getResBuildOutPaths = Finder.ConverTilePathToWorldPath(resBuildOutSide);
                resBuildDirPos = (m_getResBuildOutPaths[0]+m_getResBuildOutPaths[m_getResBuildOutPaths.Count/2])/2;
            }

            if (buildOutSide!=null)
            {
                m_workerBuildOutTilePaths = buildOutSide;
                m_workerBuildOutPaths = Finder.ConverTilePathToWorldPath(buildOutSide);
                buildDirPos = (m_workerBuildOutPaths[0]+m_workerBuildOutPaths[m_workerBuildOutPaths.Count/2])/2;
            }

            if (debug)
            {
                Debug.Log(buildIndex+" 建筑GoGetResToBuild"+m_buildIndexStart+"  work:"+index);
            }


            if (resourceTypes!=null)
            {
                m_resList = resourceTypes;
                m_resTypeIndex = Random.Range(0,m_resList.Count);
            }


            if (CrrWorkState != WorkerState.BackCarry)
            {
                GoOnTakeRes();
                ChangeState<WorkerPatrolState>();
            }

           
        }

        public Action<long,long> m_filrCountCallBack;
        
        //去拿水给建筑灭火
        public void GoGetWaterToFireBuild(List<Vector2> resBuildOutSide, List<Vector2> buildOutSide,long buildID,long resBuildID,List<People.ENMU_CARRY_RESOURCE_TYPE> resourceTypes,Action<long,long> fireCountCall)
        {
            if (buildID>0)
            {
                this.buildIndex = buildID;
                ResBuildIndex = resBuildID;
            }

            if (resBuildOutSide!=null)
            {
                m_getResBuildOutTilePaths = resBuildOutSide;
                m_getResBuildOutPaths = Finder.ConverTilePathToWorldPath(resBuildOutSide);
                resBuildDirPos = (m_getResBuildOutPaths[0]+m_getResBuildOutPaths[m_getResBuildOutPaths.Count/2])/2;
            }

            if (buildOutSide!=null)
            {
                m_workerBuildOutTilePaths = buildOutSide;
                m_workerBuildOutPaths = Finder.ConverTilePathToWorldPath(buildOutSide);
                buildDirPos = (m_workerBuildOutPaths[0]+m_workerBuildOutPaths[m_workerBuildOutPaths.Count/2])/2;
            }

            if (debug)
            {
                Debug.Log(buildIndex+" 建筑GoGetResToBuild"+m_buildIndexStart+"  work:"+index);
            }


            if (resourceTypes!=null)
            {
                m_resList = resourceTypes;
                m_resTypeIndex = Random.Range(0,m_resList.Count);
            }


            if (CrrWorkState != WorkerState.BackCarry||wait)
            {
                GoOnTakeRes();
             //   ChangeState<WorkerPatrolState>();
            }


            m_filrCountCallBack = fireCountCall;
            
            

        }
        
        
        
        //将物品带回到建筑边上
        public void GetResBackToBuild()
        {

            m_buildIndexStart = Random.Range(0, m_workerBuildOutTilePaths.Count);

            //重新巡路到建筑位置上
            var startPos = Finder.ConvertLocalToTile(Owner.transform.localPosition);

            endPos = m_workerBuildOutTilePaths[m_buildIndexStart];
            FindPaths = Finder.GetFindWorldPaths(startPos, endPos);

            CrrWorkState = WorkerState.BackCarry;
            CrrWalkState = People.ENMU_CITIZEN_STAT.CARRY;


            m_resTypeIndex++;
            if (m_resTypeIndex>=m_resList.Count)
            {

                m_resTypeIndex = 0;
            }
            
            m_crrResType = m_resList[m_resTypeIndex];
            
            
            pathIndex = 0;

            MoveNextPos();

            if (debug)
            {
//                Debug.Log(startPos + "=>" + endPos + " 走到建筑去  当前距离建筑步数:" + FindPaths.Count + "  随机建筑点:" +
//                          m_buildIndexStart + "/" + m_workerBuildOutPaths.Count+ " WorkIndex:"+index);
            }
            
        }

        //继续去获取资源
        public void GoOnTakeRes()
        {

            if (m_filrCountCallBack!=null && m_resList.Count==1)
            {
                m_filrCountCallBack.Invoke(buildIndex,index);
                return;
            }
        
            m_buildIndexStart = Random.Range(0, m_getResBuildOutTilePaths.Count);

            var startPos = Finder.ConvertLocalToTile(Owner.transform.localPosition);

            endPos = m_getResBuildOutTilePaths[m_buildIndexStart];
            FindPaths = Finder.GetFindWorldPaths(startPos, endPos);

            CrrWorkState = WorkerState.GOCarry;
            CrrWalkState = People.ENMU_CITIZEN_STAT.MOVE;
            
            pathIndex = 0;

            MoveNextPos();
            
            ChangeState<WorkerPatrolState>();
        }


        public bool IsWorkForBuild(long buildID)
        {
            return buildIndex == buildID;
        }

        public bool IsWorking()
        {
            return buildIndex > 0;
        }


        public bool IsWorkResBuildMove(long StbuildID,long buildID)
        {
            return ResBuildIndex == StbuildID && ResBuildIndex == buildID;
        }
        public bool IsWorkFireBuildMove(long StbuildID, long buildID)
        {
            return ResBuildIndex == StbuildID && buildIndex == buildID;
        }

        #region 建筑状态下绕圈走

        #endregion


        #region 游走状态

        public bool NextStep()
        {
            if (debug)
            {
                Debug.Log(pathIndex+" 建筑行走 "+FindPaths.Count+ " WorkIndex:"+index);
            }
            if (pathIndex < FindPaths.Count - 2)
            {
                pathIndex = pathIndex + 1;
                return false;
            }

            return true;
        }

        public bool IsEndStep()
        {
            if (pathIndex < FindPaths.Count - 2)
            {
                return false;
            }

           
            return true;
        }

        public Vector2 StepStart()
        {
            if (pathIndex<=FindPaths.Count-2)
            {
                return FindPaths[pathIndex];
            }

            return Finder.MakeWorldPosFormLocal(Owner.transform.localPosition.x,Owner.transform.localPosition.z);

        }

        public Vector2 StepEnd()
        {
            if (pathIndex<=FindPaths.Count-2)
            {
                return FindPaths[pathIndex + 1];
            }
            return Finder.MakeWorldPosFormLocal(Owner.transform.localPosition.x,Owner.transform.localPosition.z);

        }

        #endregion
    }
}