using Client.Base;
using Client.FSM;
using UnityEngine;

namespace Client
{
    public class WorkerChoosePatrol : BState
    {
        public override void Enter()
        {
            base.Enter();
            
            

            SuperFSM.ChangeState<WorkerPatrolState>();
        }

        #region Properties
        ///=================================================================================================
        /// <summary>   Gets the super fsm. </summary>
        ///
        /// <value> The super fsm. </value>
        ///=================================================================================================

        public WorkerFMS SuperFSM
        {
            get
            {
                return (WorkerFMS)SuperMachine;
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
                return SuperFSM.Owner;
            }
        }
        #endregion
    }
}