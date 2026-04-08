using System;
using Client;
using Client.Base;
using UnityEngine;

namespace Client
{
    public class WorkerIdeState: BHState
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
            AddState<WorkerChoosePatrol>();
            AddState<WorkerSleepSubState>();

            SetInitialState<WorkerSleepSubState>();
        }

        public override void Enter()
        {
            base.Enter();

            //set the previous manual execute time
            _prevManualExecuteTime = DateTime.Now;

            //set the custom update frequency
            ((MonoFSM)SuperMachine).SetUpdateFrequency(0.1f);
            _manualExecuteRate = ((MonoFSM)SuperMachine).ManualUpdateFrequency;
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
        /// Retrives the super state machine
        /// </summary>
        public PeopleFSM OwnerFSM
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
                return OwnerFSM.Owner;
            }
        }
    }
}