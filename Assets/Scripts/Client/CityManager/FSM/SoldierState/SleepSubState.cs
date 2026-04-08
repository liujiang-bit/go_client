using Client.Base;
using UnityEngine;

namespace Client
{
    public class SleepSubState : BState
    {
        float sleepTime;

        public override void Initialize()
        {
            base.Initialize();

            //set a specific name for this state
            StateName = "Sleep";

            //set the sleep time
            sleepTime = Random.Range(3f, 5f);
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
//                Debug.Log("开始巡逻");
                OwnerFSM.ChangeState<ChoosePatrolPoint>();
            }
                
        }

        #region Properties
        ///=================================================================================================
        /// <summary>   Gets the fsm that owns this item. </summary>
        ///
        /// <value> The owner fsm. </value>
        ///=================================================================================================

        public IdleMainState OwnerFSM
        {
            get
            {
                return (IdleMainState)Machine;
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
                return ((PeopleFSM)SuperMachine).Owner;
            }
        }
        #endregion
    }
}
