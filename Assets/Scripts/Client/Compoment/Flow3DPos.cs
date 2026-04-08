using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(Transform))]
    public class Flow3DPos : MonoBehaviour
    {
        public bool invert = true;
        private Transform m_owner;
        public Transform m_target;

        private void Start()
        {
            m_owner = GetComponent<Transform>();
        }
        private void Update()
        {
            if (m_target == null)
                return;

            if (invert)
            {
                Vector3 pos = m_target.position;
                pos.x = m_owner.position.x;
                pos.z = m_owner.position.z;
                m_target.position = pos;
            }
            else
            {
                Vector3 pos = m_owner.position;
                pos.x = m_target.position.x;
                pos.z = m_target.position.z;
                m_owner.position = pos;
            }
        }
    }
}