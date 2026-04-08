using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    [RequireComponent(typeof(GameToggle))]
    public class SwitchToggle : MonoBehaviour
    {
        [SerializeField] private GameObject m_onObj;
        [SerializeField] private GameObject m_closeObj;

        private void Awake()
        {
            var toggle = GetComponent<GameToggle>();
            toggle.onValueChanged.AddListener(IsOn);
        }

        public void IsOn(bool ison)
        {
            m_closeObj.SetActive(!ison);
            m_onObj.SetActive(ison);
        }
    }
}