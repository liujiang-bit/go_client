using Client.Base;
using UnityEngine;

namespace Client
{
    public class ChoosePatrolPoint : BState
    {
        public override void Enter()
        {
            base.Enter();
            
            

            SuperFSM.ChangeState<PatrolMainState>();
        }

        #region Properties
        ///=================================================================================================
        /// <summary>   Gets the super fsm. </summary>
        ///
        /// <value> The super fsm. </value>
        ///=================================================================================================

        public PeopleFSM SuperFSM
        {
            get
            {
                return (PeopleFSM)SuperMachine;
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
