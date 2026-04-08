using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class LodMeshHelper : MonoBehaviour
    {
        public float[] weight = { 1.0f, 1.0f, 0.15f, 0.12f, 0.06f, 0.02f };
        public bool[] keepBorder = { true, true, true, true, false, false };
    }
}