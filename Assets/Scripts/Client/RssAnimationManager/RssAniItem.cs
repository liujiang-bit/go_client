using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class RssAniItem
    {
        public GameObject m_gameObject;

        public Vector3 m_spos;

        public Vector3 m_dir;

        public float m_a;

        public float m_v0;

        public float m_timeShift = -0.001f;

        public int m_step = 1;

        public float m_startFrame;

        public float m_step2LeftClock;

        public Vector3 m_baseScale = Vector3.zero;
    }
}
