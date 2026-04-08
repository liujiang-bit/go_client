// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月31日
// Update Time         :    2019年12月31日
// Class Description   :    UI_LC_Captain_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using System;

namespace Game {
    public partial class UI_LC_Captain_SubView : UI_SubView
    {
        public UI_Item_CaptainHead_SubView m_selectCaptainHead;

        public void AddClickEvent(Action<HeroProxy.Hero> call)
        {
            m_UI_Item_CaptainHead1.AddClickEvent(call);
            m_UI_Item_CaptainHead2.AddClickEvent(call);
        }
        public void SetHero(List<HeroProxy.Hero> heros)
        {
            m_UI_Item_CaptainHead1.SetHero(heros[0]);
            if (heros.Count > 1)
            {
                m_UI_Item_CaptainHead2.SetHero(heros[1]);
                m_UI_Item_CaptainHead2.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Item_CaptainHead2.gameObject.SetActive(false);
            }
        }
        public void SelecteHero(HeroProxy.Hero hero)
        {
            m_selectCaptainHead = null;
            if (m_UI_Item_CaptainHead1.Selected(hero))
            {
                m_selectCaptainHead = m_UI_Item_CaptainHead1;
            }
            if (m_UI_Item_CaptainHead2.Selected(hero))
            {
                m_selectCaptainHead = m_UI_Item_CaptainHead2;
            }
        }

        public void AddClickEvent(Action<HeroProxy.Hero, UI_Item_CaptainHead_SubView> call)
        {
            m_UI_Item_CaptainHead1.AddClickEvent(call);
            m_UI_Item_CaptainHead2.AddClickEvent(call);
        }
    }
}