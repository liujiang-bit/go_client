using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Client
{
    public class NightingObject : MonoBehaviour
    {
        public float lightOnRandomTime { get; set; } = 1f;

        private void OnEnable()
        {
            ClientUtils.lightingManager.RegisterNightObject(this);
            if (ClientUtils.lightingManager.isNight)
            {
                SetLightOn(ClientUtils.lightingManager.isNight);
            }
        }

        public void SetLightOn(bool b)
        {
            if (isActiveAndEnabled)
            {
                StartCoroutine(DoSetLightOn(b, UnityEngine.Random.Range(0f, lightOnRandomTime)));
            }
            else
            {
                DoSetLightOnNow(b);
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
