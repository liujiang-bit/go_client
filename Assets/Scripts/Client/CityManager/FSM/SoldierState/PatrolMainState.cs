using Client.Base;
using System;
using UnityEngine;
using Client;

namespace Client
{
    public class PatrolMainState : BState
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
            
            People.SetStateS(Owner,People.ENMU_CITIZEN_STAT.MOVE,Owner.WorldPaths[0],Owner.WorldPaths[1],0.09f);
        }

        public override void Execute()
        {
            base.Execute();

            //go to idle state if we have reached our target
            if (Owner.IsStop())
            {
                SuperMachine.ChangeState<IdleMainState>();
                
                People.SetStateS(Owner,People.ENMU_CITIZEN_STAT.IDLE,Owner.WorldPaths[1],Owner.WorldPaths[1],0f);
                
                Owner.WorldPaths.Reverse();
//                Debug.Log("走到了");
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
        public PeopleFSM SuperFSM
        {
            get
            {
                return (PeopleFSM)SuperMachine;
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
