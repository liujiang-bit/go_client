using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client
{
    public class FlowDirection : MonoBehaviour
    {
        [Header("Ñ²Âßµã")]
        [FormerlySerializedAs("m_river_plane")]
        public GameObject m_plane;

        public void SetFlowDirection(float direction)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetFloat("_FlowDirection", direction);
            MeshRenderer component = this.m_plane.GetComponent<MeshRenderer>();
            component.SetPropertyBlock(materialPropertyBlock);
        }
    }
}