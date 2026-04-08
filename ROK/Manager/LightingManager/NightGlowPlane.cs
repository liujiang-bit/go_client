using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    public class NightGlowPlane : NightObject
    {
        private MeshRenderer m_glow_plane;

        private void Awake()
        {
            this.m_glow_plane = base.gameObject.GetComponent<MeshRenderer>();
            this.m_glow_plane.enabled = false;
        }

        protected override void DoSetLightOnNow(bool b)
        {
            if (GraphicSettingMgr.GetGraphicLevel() == GraphicSettingMgr.GraphicLevel.HIGH || GraphicSettingMgr.GetGraphicLevel() == GraphicSettingMgr.GraphicLevel.MEDIUM)
            {
                this.m_glow_plane.enabled = b;
            }
        }
    }
}