using System;
using UnityEngine;

namespace Client
{
    public class LockAngle45 : MonoBehaviour
    {
        public void LockAngle()
        {
            base.transform.eulerAngles = new Vector3(45f, 0f, 0f);
        }
    }
}