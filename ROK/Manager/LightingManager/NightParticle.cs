using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Skyunion;

namespace ROK
{
    public class NightParticle : NightObject
    {
        public string m_particle_path = string.Empty;

        private GameObject m_particle_obj;

        public void DestroyParticle()
        {
            if (this.m_particle_obj != null)
            {
                CoreUtils.assetService.Destroy(this.m_particle_obj);
                this.m_particle_obj = null;
            }
        }

        public void StopParticle()
        {
            if (this.m_particle_obj != null)
            {
                ParticleSystem component = this.m_particle_obj.GetComponent<ParticleSystem>();
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
                if (this.m_particle_obj == null)
                {
                    CoreUtils.assetService.Instantiate(this.m_particle_path, (GameObject obj) =>
                    {
                        if (obj != null)
                        {
                            this.m_particle_obj = obj;
                            this.m_particle_obj.transform.SetParent(base.transform, false);
                            this.m_particle_obj.transform.position = base.transform.position;
                            ParticleSystem component = this.m_particle_obj.GetComponent<ParticleSystem>();
                            if (component != null)
                            {
                                component.Play();
                            }
                        }
                    });
                }
            }
            else
            {
                this.DestroyParticle();
            }
        }
    }
}