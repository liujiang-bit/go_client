using System;
using UnityEngine;

namespace ROK
{
    public class AdorningTweak : MonoBehaviour
    {
        public void UpdateAdorningRotation(float euler)
        {
            base.GetComponent<MeshRenderer>().material.SetFloat("_TreeRot", euler * 3.14159274f / 180f);
        }
    }
}