using System;
using System.Collections;
using UnityEngine;
using Skyunion;

namespace Client
{
    public class NightingParticle : NightingObject
    {
        public string m_particle_path = string.Empty;

        private GameObject m_particle_obj;

        protected override IEnumerator DoSetLightOn(bool b, float delay)
        {
            yield return new WaitForSeconds(delay);
            try
            {
                DoSetLightOnNow(b);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void DestroyParticle()
        {
            if (m_particle_obj != null)
            {
                CoreUtils.assetService.Destroy(m_particle_obj);
                m_particle_obj = null;
            }
        }

        public void StopParticle()
        {
            if (m_particle_obj != null)
            {
                ParticleSystem component = m_particle_obj.GetComponent<ParticleSystem>();
                if (component != null)
                {
                    component.Stop();
                }
            }
        }

        protected override void DoSetLightOnNow(bool b)
        {
            if (b)
            {
                if (!(m_particle_obj == null) || string.IsNullOrEmpty(m_particle_path))
                {
                    return;
                }
                CoreUtils.assetService.Instantiate(m_particle_path, (gameObject)=>
                {
                    if (gameObject)
                    {
                        m_particle_obj = gameObject;
                        gameObject.transform.SetParent(transform, worldPositionStays: false);
                        gameObject.transform.position = transform.position;
                        ParticleSystem component = m_particle_obj.GetComponent<ParticleSystem>();
                        if (component != null)
                        {
                            component.Play();
                        }
                    }
                });
            }
            else
            {
                DestroyParticle();
            }
        }
    }
}