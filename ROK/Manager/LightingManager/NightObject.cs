using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    public class NightObject : MonoBehaviour
    {
        private float m_set_light_on_random_time = 1f;

        public float setLightOnRandomTime
        {
            get
            {
                return this.m_set_light_on_random_time;
            }
            set
            {
                this.m_set_light_on_random_time = value;
            }
        }

        private void OnEnable()
        {
            LightingManager.GetInstance().RegisterNightObject(this);
            if (LightingManager.GetInstance().isNight)
            {
                this.SetLightOn(LightingManager.GetInstance().isNight);
            }
        }

        public void SetLightOn(bool b)
        {
            if (base.isActiveAndEnabled)
            {
                base.StartCoroutine(this.DoSetLightOn(b, UnityEngine.Random.Range(0f, this.m_set_light_on_random_time)));
            }
            else
            {
                this.DoSetLightOnNow(b);
            }
        }

        protected virtual IEnumerator DoSetLightOn(bool b, float delay)
        {
            yield return new WaitForSeconds(delay);
            DoSetLightOnNow(b);
        }

        protected virtual void DoSetLightOnNow(bool b)
        {
        }
    }
}
