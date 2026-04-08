using System;
using UnityEngine;

namespace ROK
{
    public class RiverFlowController : MonoBehaviour
    {
        public GameObject m_river_plane;

        public void SetFlowDirection(float direction)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetFloat("_FlowDirection", direction);
            MeshRenderer component = this.m_river_plane.GetComponent<MeshRenderer>();
            component.SetPropertyBlock(materialPropertyBlock);
        }
    }
}