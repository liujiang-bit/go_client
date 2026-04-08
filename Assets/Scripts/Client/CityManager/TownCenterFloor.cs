using UnityEngine;
using UnityEngine.Serialization;

namespace Client
{
    public class TownCenterFloor : MonoBehaviour
    {
        public enum State
        {
            None,
            City,
            Map
        }
        [Header("城内地表")]
        [FormerlySerializedAs("m_floor_tile")]
        public GameObject city;
        [FormerlySerializedAs("m_ground")]
        [Header("城外地表")]
        public GameObject map;
        private State m_cur_state = State.None;
        public void SetState(State state)
        {
            if (m_cur_state != state)
            {
                city.SetActive(state == State.City);
                map.SetActive(state == State.Map);
                m_cur_state = state;
            }
        }
    }
}