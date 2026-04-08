using Client.Base;
using System;
using Client;
using Client.FSM;
using UnityEngine;

namespace Client.FSM.TrainState
{
    public class TrainFindHomeState: BState
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
//            Debug.Log("开始走"+SuperFSM.Step()+" / "+SuperFSM.WorldPaths.Count);
        }

        public override void Execute()
        {
            base.Execute();
            //go to idle state if we have reached our target
            if (Owner.IsStop())
            {
                if (SuperFSM.NextStep())
                {
                    SuperFSM.Idle();
                    SuperMachine.ChangeState<TrainIdeState>();
                    
//                     Debug.Log("走到了 休息一下"+SuperFSM.Step()+" / "+SuperFSM.WorldPaths.Count);
                }
                else
                {
//                    Debug.Log("走下一个点"+SuperFSM.Step()+" / "+SuperFSM.WorldPaths.Count);
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
        public TrainSoldiersFMS SuperFSM
        {
            get
            {
                return (TrainSoldiersFMS)SuperMachine;
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