using Client.Base;
using Client.FSM;
using UnityEngine;
using UnityEngine.UI;


namespace Client.FSM.TrainState
{
    public class TrainSleepSubState: BState
    {
        float sleepTime;

        public override void Initialize()
        {
            base.Initialize();

            //set a specific name for this state
            StateName = "Sleep";

        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute()
        {
            base.Execute();

           
                
        }

        #region Properties
        ///=================================================================================================
        /// <summary>   Gets the fsm that owns this item. </summary>
        ///
        /// <value> The owner fsm. </value>
        ///=================================================================================================

        public WorkerIdeState OwnerFSM
        {
            get
            {
                return (WorkerIdeState)Machine;
            }
        }

        ///=================================================================================================
        /// <summary>   Gets the owner. </summary>
        ///
        /// <value> The owner. </value>
        ///=================================================================================================

        public People Owner
        {
            get
            {
                return ((WorkerFMS)SuperMachine).Owner;
            }
        }
        #endregion
    }
}