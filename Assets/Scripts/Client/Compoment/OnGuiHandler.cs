using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using System;

namespace Client
{
    public class OnGuiHandler : MonoBehaviour
    {
        private Action<bool> m_applicationFocus;

        private Action mOnGUIAction;
        private void OnGUI()
        {
            mOnGUIAction?.Invoke();
        }

        public void BindEvent(Action action)
        {
            mOnGUIAction = action;
        }

        public void SetApplicationFocus(Action<bool> func)
        {
            m_applicationFocus = func;
        }

        private void OnApplicationPause(bool pause)
        {
            if (m_applicationFocus != null)
            {
                m_applicationFocus(pause);
            }
        }
    }
}