using System;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    public class ImageTextMiddleHelper : MonoBehaviour
    {
        public GameObject[] m_UIElements;

        private void Start()
        {
            this.AlignMid();
        }

        public static void AlignMidS(ImageTextMiddleHelper helper)
        {
            if (helper)
            {
                helper.AlignMid();
            }
        }

        private void AlignMid()
        {
            float num = 0f;
            GameObject[] uIElements = this.m_UIElements;
            for (int i = 0; i < uIElements.Length; i++)
            {
                GameObject gameObject = uIElements[i];
                if (!(gameObject == null))
                {
                    Text component = gameObject.GetComponent<Text>();
                    if (component)
                    {
                        num += component.preferredWidth;
                    }
                    else
                    {
                        RectTransform component2 = gameObject.GetComponent<RectTransform>();
                        num += component2.sizeDelta.x;
                    }
                }
            }
            RectTransform component3 = base.GetComponent<RectTransform>();
            Vector2 sizeDelta = component3.sizeDelta;
            sizeDelta.x = num;
            component3.sizeDelta = sizeDelta;
            Vector2 anchoredPosition = component3.anchoredPosition;
            anchoredPosition.x = 0f;
            component3.anchoredPosition = anchoredPosition;
        }
    }
}