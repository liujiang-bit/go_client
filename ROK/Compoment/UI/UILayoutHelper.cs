using System;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    public class UILayoutHelper : MonoBehaviour
    {
        private void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform.GetComponent<RectTransform>());
        }
    }
}
