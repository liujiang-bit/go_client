// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_Common_Toggle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Common_Toggle_SubView : UI_SubView
    {
        private List<UI_Common_ToggleItem_SubView> m_toggles = new List<UI_Common_ToggleItem_SubView>();

        protected override void BindEvent()
        {
            base.BindEvent();
            m_toggles.Add(m_UI_Common_Toggle);
        }

        public void SetToggleInfo(int onIndex,List<string> txts,UnityAction<bool,int> callback)
        {
            DisableToggle();
            for (int index = 0; index < txts.Count; index++)
            {
                if (m_toggles.Count <= index)
                {
                    var toggle = GameObject.Instantiate(m_UI_Common_Toggle.gameObject,m_pl_rect_ToggleGroup.transform);
                    var toggleItem = new UI_Common_ToggleItem_SubView(toggle.GetComponent<RectTransform>());
                    m_toggles.Add(toggleItem);
                }
                m_toggles[index].gameObject.SetActive(true);
                m_toggles[index].SetInfo(index == onIndex,index, txts[index],callback);
            }
        }

        public void DisableToggle()
        {
            foreach (var toggle in m_toggles)
            {
                toggle.gameObject.SetActive(false);
            }
        }
    }
}