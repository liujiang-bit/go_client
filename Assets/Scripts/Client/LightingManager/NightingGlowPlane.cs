using System;
using System.Collections;
using UnityEngine;
using Skyunion;

namespace Client
{
    public class NightingGlowPlane : NightingObject
    {
        private MeshRenderer m_glow_plane;

        private void Awake()
        {
            m_glow_plane = base.gameObject.GetComponent<MeshRenderer>();
            m_glow_plane.enabled = false;
        }

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

        protected override void DoSetLightOnNow(bool b)
        {
            if (CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.HIGH || CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.MEDIUM)
            {
                m_glow_plane.enabled = b;
            }
        }
    }
}