// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_Item_EventType3Item_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_EventType3Item_SubView : UI_SubView
    {
        private bool m_isInit;
        private ActivityProxy m_activityProxy;
        private int m_acEventId;
        private int m_jumpType;

        public void Refresh(int id)
        {
            InitData();
            m_acEventId = id;

            ActivityDropTypeDefine define = m_activityProxy.GetDropTypeDefine(id);
            m_jumpType = define.jumpType;

            ClientUtils.LoadSprite(m_img_icon_PolygonImage, define.icon);
            m_lbl_desc_LanguageText.text = LanguageUtils.getText(define.l_desID);

            PlayerBehaviorDataDefine behaviorDefine = CoreUtils.dataService.QueryRecord<PlayerBehaviorDataDefine>(define.playerBehavior);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(behaviorDefine.l_behaviorID);
        }

        private void InitData()
        {
            if (!m_isInit)
            {
                m_activityProxy = AppFacade.GetInstance().RetrieveProxy(ActivityProxy.ProxyNAME) as ActivityProxy;
                m_btn_go.m_btn_languageButton_GameButton.onClick.AddListener(OnGo);
                m_isInit = true;
            }
        }

        //前往
        private void OnGo()
        {
            m_activityProxy.GoJump(m_jumpType);
        }
    }
}