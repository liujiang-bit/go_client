using System;
using UnityEngine;

namespace ROK
{
    public class Pass : MonoBehaviour
    {
        private Animation m_animation;

        private void Start()
        {
            this.m_animation = base.GetComponent<Animation>();
        }

        private void OnTriggerEnter(Collider enter_collider)
        {
            Animation component = base.transform.GetComponent<Animation>();
            component[component.clip.name].speed = 1f;
            component.Play();
        }

        private void OnTriggerExit(Collider enter_collider)
        {
            Animation component = base.transform.GetComponent<Animation>();
            component[component.clip.name].speed = -1f;
            component.Play();
        }
    }
}