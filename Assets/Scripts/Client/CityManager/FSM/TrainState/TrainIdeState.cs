using System;
using Client;
using Client.Base;
using UnityEngine;

namespace Client.FSM.TrainState
{
    public class TrainIdeState: BHState
    {
        /// <summary>
        /// A reference to the manual execute frequency
        /// </summary>
        float _manualExecuteRate = 0f;

        /// <summary>
        /// A reference to the previous manual execute time
        /// </summary>
        DateTime _prevManualExecuteTime;

        /// <summary>
        /// Overriden add states method
        /// </summary>
        public override void AddStates()
        {
            AddState<TrainFindHomeState>();
            AddState<TrainSleepSubState>();

            SetInitialState<TrainSleepSubState>();
        }

        public override void Enter()
        {
            base.Enter();

            //set the previous manual execute time
            _prevManualExecuteTime = DateTime.Now;

            //set the custom update frequency
            ((MonoFSM)SuperMachine).SetUpdateFrequency(0.1f);
            _manualExecuteRate = ((MonoFSM)SuperMachine).ManualUpdateFrequency;
            
            
            SuperFSM.Idle();
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