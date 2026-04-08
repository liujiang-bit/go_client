using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    public class UISortOrderConfig : MonoBehaviour
    {
        [Serializable]
        public enum RenderType
        {
            UI = 1,
            Partical
        }

        [Serializable]
        public struct OrderConfig
        {
            public UISortOrderConfig.RenderType Type;

            public GameObject Target;

            public int Order;
        }

        public List<UISortOrderConfig.OrderConfig> config_list = new List<UISortOrderConfig.OrderConfig>();

        private void Start()
        {
        }

        public bool SetOrderConfigTarget(int index, GameObject obj, int Order)
        {
            if (this.config_list.Count <= index)
            {
                return false;
            }
            UISortOrderConfig.OrderConfig item = this.config_list[index];
            item.Target = obj;
            item.Order = Order;
            this.config_list.RemoveAt(index);
            this.config_list.Insert(index, item);
            return true;
        }

        public void SetSortLayer()
        {
            Canvas canvas = base.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = base.gameObject.GetComponentInParent<Canvas>();
            }
            if (canvas != null)
            {
                foreach (UISortOrderConfig.OrderConfig current in this.config_list)
                {
                    int order = current.Order + canvas.sortingOrder;
                    if (current.Type == UISortOrderConfig.RenderType.Partical)
                    {
                        if (current.Target != null)
                        {
                            this.SetParticalOrder(current.Target, order);
                            this.ForeachChild(current.Target.transform, order);
                        }
                    }
                    else if (current.Type == UISortOrderConfig.RenderType.UI)
                    {
                        this.SetUIOrder(current.Target, order);
                    }
                }
            }
        }

        public void ForeachChild(Transform tf, int order)
        {
            IEnumerator enumerator = tf.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform transform = (Transform)enumerator.Current;
                    this.SetParticalOrder(transform.gameObject, order);
                    this.ForeachChild(transform, order);
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumerator as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public void SetParticalOrder(GameObject go, int order)
        {
            Renderer component = go.GetComponent<Renderer>();
            if (component != null)
            {
                component.sortingOrder = order;
            }
        }

        public void SetUIOrder(GameObject go, int order)
        {
            Canvas component = go.GetComponent<Canvas>();
            if (component == null)
            {
                go.AddComponent<GraphicRaycaster>();
                component = go.GetComponent<Canvas>();
            }
            component.overrideSorting = true;
            component.sortingOrder = order;
        }
    }
}
