// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月17日
// Update Time         :    2020年9月17日
// Class Description   :    UI_Item_PlayerManageItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_PlayerManageItem_SubView : UI_SubView
    {
        private RoleInfoProxy m_RoleInfoProxy;
        private PlayerProxy m_PlayerProxy;
        
        public void SetData(RoleInfoEntity info)
        {
            if (info == null)
            {
                this.gameObject.SetActive(false);
                return;
            }

            if (m_RoleInfoProxy == null)
            {
                m_RoleInfoProxy=AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            }

            if (m_PlayerProxy == null)
            {
                m_PlayerProxy=AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            }

            //Debug.LogError("设置数据"+info.name+"***"+info.rid+"***"+info.gameNode);
            this.gameObject.SetActive(true);
            if (info.rid == UI_Win_PlayerManageMediator.copyRid)
            {
                int num = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).createRoleMax;
                m_lbl_CreatName_LanguageText.text = LanguageUtils.getTextFormat(100516, num);
            }
            m_btn_creat_PolygonImage.gameObject.SetActive(info.rid == UI_Win_PlayerManageMediator.copyRid);
            string name = string.Empty;
            if (!string.IsNullOrEmpty(info.guildAbbName))
            {
                name = string.Format("[{0}]{1}", info.guildAbbName, info.name);
            }
            else
            {
                name = info.name;
            }
            m_lbl_name_LanguageText.text = name;
            long power = 0;
            if(m_PlayerProxy?.CurrentRoleInfo.rid == info.rid)
            {
                power = m_PlayerProxy.CurrentRoleInfo.combatPower;
            }
            else
            {
                power = info.combatPower;
            }
            m_lbl_power_LanguageText.text = LanguageUtils.getTextFormat(785015, ClientUtils.FormatComma(power));
            
            m_UI_Model_PlayerHead.LoadPlayerIcon(info.headId,info.headFrameID);
            if (!string.IsNullOrEmpty(info.gameNode))
            {
                m_lbl_kingdom_LanguageText.text = RoleInfoHelp.GetServerIdDes(info.gameNode);
                m_lbl_kingdomName_LanguageText.text =RoleInfoHelp.GetServerNameId(info.gameNode);
            }

            bool isLogin = m_PlayerProxy?.CurrentRoleInfo.rid == info.rid;
            m_img_ck_PolygonImage.gameObject.SetActive(isLogin);
            m_btn_creat_GameButton.onClick.RemoveAllListeners();
            m_btn_creat_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_PlayerNewChar);
            });
            
            m_btn_player_GameButton.onClick.RemoveAllListeners();
            m_btn_player_GameButton.onClick.AddListener(() =>
            {
                if (!isLogin)
                {             
                    RoleInfoPanelData data= new RoleInfoPanelData(RoleInfoPanelType.Login,(int)info.rid);
                    data.m_RoleInfoEntity = info;
                    CoreUtils.uiManager.ShowUI(UI.s_PlayerChangeSure,null,data);  
                }
                 
            });
        }
    }
}