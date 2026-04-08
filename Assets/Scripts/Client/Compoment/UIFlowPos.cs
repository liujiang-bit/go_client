using System;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(RectTransform))]
    public class UIFlowPos : MonoBehaviour
    {
        public bool invert = true;
        private RectTransform m_owner;
        public RectTransform m_target;

        private void Start()
        {
            m_owner = GetComponent<RectTransform>();
        }
        private void Update()
        {
            if (m_target == null)
                return;

            if (invert)
            {
                Vector3 pos = m_target.position;
                pos.x = m_owner.position.x;
                pos.y = m_owner.position.y;
                m_target.position = pos;
            }
            else
            {
                Vector3 pos = m_owner.position;
                pos.x = m_target.position.x;
                pos.y = m_target.position.y;
                m_owner.position = pos;
            }
        }
    }
}