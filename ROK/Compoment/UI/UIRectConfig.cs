using System;
using UnityEngine;

namespace ROK
{
    public class UIRectConfig : MonoBehaviour
    {
        public GameObject LeftBottom;

        public GameObject LeftTop;

        public GameObject RightBottom;

        public GameObject RightTop;

        private Camera UICamera;

        private void Awake()
        {
            GameObject gameObject = GameObject.Find("Canvas");
            GameObject gameObject2 = gameObject.transform.Find("Camera").gameObject;
            this.UICamera = gameObject2.GetComponent<Camera>();
        }

        public void GetScreenSize(out float x, out float y)
        {
            Vector3 vector = this.UICamera.WorldToScreenPoint(this.LeftBottom.transform.position);
            Vector3 vector2 = this.UICamera.WorldToScreenPoint(this.LeftTop.transform.position);
            Vector3 vector3 = this.UICamera.WorldToScreenPoint(this.RightBottom.transform.position);
            Vector3 vector4 = this.UICamera.WorldToScreenPoint(this.RightTop.transform.position);
            x = vector3.x - vector.x;
            y = vector4.y - vector3.y;
        }
    }
}