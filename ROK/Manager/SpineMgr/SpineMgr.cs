using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class SpineMgr : MonoBehaviour
    {
        private float groupAlpha = -1f;

        private CanvasGroup canvasGroup;

        public List<SpineAni> spineList = new List<SpineAni>();

        private void Awake()
        {
            this.canvasGroup = base.gameObject.GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (this.groupAlpha != this.canvasGroup.alpha)
            {
                this.groupAlpha = this.canvasGroup.alpha;
                foreach (SpineAni current in this.spineList)
                {
                    if (current != null && current.gameObject != null)
                    {
                        current.SetAlpha(this.groupAlpha);
                    }
                }
            }
        }

        public void AddSpine(GameObject go)
        {
            SpineAni component = go.GetComponent<SpineAni>();
            if (component != null)
            {
                foreach (SpineAni current in this.spineList)
                {
                    if (component == current)
                    {
                        return;
                    }
                }
                bool flag = false;
                for (int i = 0; i < this.spineList.Count; i++)
                {
                    if (this.spineList[i] == null)
                    {
                        this.spineList[i] = component;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this.spineList.Add(component);
                }
                component.SetAlpha(this.groupAlpha);
            }
        }
    }
}