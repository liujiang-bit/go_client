using System;
using UnityEngine;

namespace Client
{
    public class LockRotate180 : MonoBehaviour
    {
        public void UpdateRotation(float euler)
        {
            base.GetComponent<MeshRenderer>().material.SetFloat("_TreeRot", euler * 3.14159274f / 180f);
        }
    }
}