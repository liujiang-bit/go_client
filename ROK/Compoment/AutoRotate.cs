using System;
using UnityEngine;
using Skyunion;

namespace ROK
{
    public class AutoRotate : MonoBehaviour
    {
        public Vector3 m_rotateSpeed = Vector3.zero;

        private void Start()
        {
        }

        private void Update()
        {
            try
            {
                if (this.m_rotateSpeed != Vector3.zero)
                {
                    Vector3 localEulerAngles = base.transform.localEulerAngles;
                    if (this.m_rotateSpeed.x != 0f)
                    {
                        localEulerAngles.x += this.m_rotateSpeed.x * Time.deltaTime;
                    }
                    if (this.m_rotateSpeed.y != 0f)
                    {
                        localEulerAngles.y += this.m_rotateSpeed.y * Time.deltaTime;
                    }
                    if (this.m_rotateSpeed.z != 0f)
                    {
                        localEulerAngles.z += this.m_rotateSpeed.z * Time.deltaTime;
                    }
                    base.transform.localEulerAngles = localEulerAngles;
                }
            }
            catch (Exception e)
            {
                CoreUtils.logService.Error(e.ToString());
            }
        }
    }
}