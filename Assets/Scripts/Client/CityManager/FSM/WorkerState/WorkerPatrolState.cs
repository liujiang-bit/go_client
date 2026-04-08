using Client.Base;
using System;
using Client;
using Client.FSM;
using UnityEngine;


namespace Client
{
    public class WorkerPatrolState: BState
    {
         /// <summary>
        /// A reference to the manual execute frequency
        /// </summary>
        float _manualExecuteRate = 0f;

        /// <summary>
        /// A reference to the previous manual execute time
        /// </summary>
        DateTime _prevManualExecuteTime;

        public override void Enter()
        {
            base.Enter();

            //set the previous manual execute time
            _prevManualExecuteTime = DateTime.Now;

            //set the custom update frequency
            ((MonoFSM)SuperMachine).SetUpdateFrequency(1f);
            _manualExecuteRate = ((MonoFSM)SuperMachine).ManualUpdateFrequency;
            
            SuperFSM.MoveNextPos();
        }

        public override void Execute()
        {
            base.Execute();
            //go to idle state if we have reached our target
            if (Owner.IsStop())
            {
                if (SuperFSM.NextStep())
                {
                    //走到目标点进入休息或工作状态
                    if (SuperFSM.CrrWorkState == WorkerState.WalkAround)
                    {
                        SuperFSM.Idle();
                    }
                    else if (SuperFSM.CrrWorkState == WorkerState.Building)
                    {
                        SuperFSM.Buildering();
                    }
                    else if (SuperFSM.CrrWorkState == WorkerState.GOCarry)
                    {
                        SuperFSM.FetchingRes();
                        
                    }else if (SuperFSM.CrrWorkState == WorkerState.BackCarry)
                    {
                        SuperFSM.TakeBaskResBuilding();
                    }
                    else if (SuperFSM.CrrWorkState == WorkerState.RunToCitizen)
                    {
                        SuperFSM.HideBuilder();
                    }
                    

                }
                else
                {
                    SuperFSM.MoveNextPos();
                }
            }
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //find the time difference
            double timeDiff = DateTime.Now.Subtract(_prevManualExecuteTime).TotalSeconds;
            
            //set the previous manual execute time
            _prevManualExecuteTime = DateTime.Now;
        }

        /// <summary>
        /// Retries the FSM that owns this state
        /// </summary>
        public WorkerFMS SuperFSM
        {
            get
            {
                return (WorkerFMS)SuperMachine;
            }
        }

        /// <summary>
        /// A little extra stuff. Accessing info inside the OwnerFSM
        /// </summary>
        public People Owner
        {
            get
            {
                return SuperFSM.Owner;
            }
        }
    }
}