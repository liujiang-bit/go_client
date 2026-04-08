
namespace Client
{
    public class PeopleFSM : MonoFSM
    {
        public People Owner { get; set; }

        public override void AddStates()
        {
            //set the custom update frequenct
            SetUpdateFrequency(0.1f);

            //add the states
            AddState<IdleMainState>();
            AddState<PatrolMainState>();

            //set the initial state
            SetInitialState<IdleMainState>();
        }

        
    }
}