using System;
using UnityEngine;

namespace ROK
{
    public class UIAnimationTweak : MonoBehaviour
    {
        private Animation m_animation;

        private void Awake()
        {
            this.m_animation = base.GetComponent<Animation>();
        }

        private void OnEnable()
        {
            this.m_animation.cullingType = AnimationCullingType.AlwaysAnimate;
        }

        private void OnDisable()
        {
            this.m_animation.cullingType = AnimationCullingType.BasedOnRenderers;
        }
    }
}
