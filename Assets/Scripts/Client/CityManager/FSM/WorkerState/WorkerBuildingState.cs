using Client.Base;
using System;
using Client;
using Client.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    public class WorkerBuildingState: BState
    {
        float sleepTime;

        public override void Initialize()
        {
            base.Initialize();

            //set a specific name for this state
            StateName = "Sleep";

            //set the sleep time
            sleepTime = Random.Range(1f, 2f);
        }

        public override void Enter()
        {
            base.Enter();

            //set the sleep time
            sleepTime = Random.Range(2f, 3f);
            
//            Debug.Log("休息"+sleepTime);
            
            
        }

        public override void Execute()
        {
            base.Execute();

            sleepTime -= Time.deltaTime;

            //if time is exhausted go to choose patrol point state
            if (sleepTime <= 0)
            {
                if (SuperFSM.CrrWorkState == WorkerState.Building)
                {
                    SuperFSM.AroundBuild();
                }else if (SuperFSM.CrrWorkState == WorkerState.GOCarry)
                {
                    SuperFSM.GetResBackToBuild();
                }else if (SuperFSM.CrrWorkState == WorkerState.BackCarry)
                {
                    SuperFSM.GoOnTakeRes();
                }

                SuperFSM.ChangeState<WorkerPatrolState>();
            }
                
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