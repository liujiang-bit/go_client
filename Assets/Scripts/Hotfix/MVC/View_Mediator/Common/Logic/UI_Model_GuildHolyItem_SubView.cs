// =============================================================================== 
// Author              :    林光志
// Create Time         :    2020年5月13日
// Update Time         :    2020年5月13日
// Class Description   :    UI_Model_GuildHolyItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Model_GuildHolyItem_SubView : UI_SubView
    {
        private List<CityBuffDefine> m_cityBuffDefines = new List<CityBuffDefine>();
        
        private int lan_pos = 300032;
        private int lan_normal = 732060;
        private int lan_fighting = 732063;

        protected override void BindEvent()
        {
            m_cityBuffDefines = CoreUtils.dataService.QueryRecords<CityBuffDefine>();
            base.BindEvent();
        }

        public void Refresh(StrongHoldCardData cardData)
        {
            m_pl_pos.RemoveAllClickEvent();
            m_btn_info_GameButton.onClick.RemoveAllListeners();
            m_btn_info_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_WorldObjectInfoBuildInfo, null, cardData.descId);
            });

            CoreUtils.assetService.Instantiate(cardData.imgShow, (asset) =>
            {
                GameObject.Instantiate<GameObject>(asset, m_pl_build.transform);
            });
            
            m_lbl_data1_LanguageText.gameObject.SetActive(false);
            m_lbl_data2_LanguageText.gameObject.SetActive(false);
            
            m_lbl_name_LanguageText.text = LanguageUtils.getText(cardData.nameId);

            int clientPosX = PosHelper.ServerUnitToClientPos(cardData.pos.x);
            int clientPosY = PosHelper.ServerUnitToClientPos(cardData.pos.y);
            int serverPosX = PosHelper.ClientPosToServerPos(clientPosX);
            int serverPosY = PosHelper.ClientPosToServerPos(clientPosY);
            int serverUnitX = PosHelper.ClientPosToServerUnit(clientPosX);
            int serverUnitY = PosHelper.ClientPosToServerUnit(clientPosY);
            
            m_pl_pos.AddClickEvent(() =>
            {
                if (serverPosX >= 0 && serverPosY >= 0)
                {
                    WorldCamera.Instance().ViewTerrainPos(serverPosX, serverPosY, 1000, null);
//                    Map_Move.request req = new Map_Move.request();
//                    req.posInfo = new PosInfo();
//                    req.posInfo.x = serverUnitX;
//                    req.posInfo.y = serverUnitY;
//                    req.isPreview = false;
//                    AppFacade.GetInstance().SendSproto(req); 

                    var m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
                    m_RssProxy.SendMapMove(serverUnitX / 100 , serverUnitY / 100);
                    
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceHoly);
                    
                    
                }

            });
            m_pl_pos.SetLinkText(LanguageUtils.getTextFormat(lan_pos,  clientPosX, clientPosY));
            int stateLan = cardData.state == StrongHoldCardData.StrongHoldState.Fighting ? lan_fighting : lan_normal;
            m_lbl_state_LanguageText.text = LanguageUtils.getText(stateLan);

            int buffCountIndex = 0;
            for (int i = 0; i < cardData.buffDataLst.Count; i++)
            {
                int buffId = cardData.buffDataLst[i];
                if (buffId != 0)
                {
                    if (buffCountIndex == 0)
                    {
                        buffCountIndex++;
                        CityBuffDefine define = GetCityBuffDeine(buffId);
                        RefreshBuffDataText(m_lbl_data1_LanguageText, define);
                    }
                    else if (buffCountIndex == 1)
                    {
                        buffCountIndex++;
                        CityBuffDefine define = GetCityBuffDeine(buffId);
                        RefreshBuffDataText(m_lbl_data2_LanguageText, define);
                    }
                    else
                    {
                        CoreUtils.logService.Warn("圣地===  预制现在最多只支持2条buff配置！");
                    }
                }
            }
        }

        private void RefreshBuffDataText(LanguageText txt, CityBuffDefine define)
        {
            txt.gameObject.SetActive(true);
            object[] objs = new object[define.tagData.Count];
            for (int i = 0; i < define.tagData.Count; i++)
            {
                objs[i] = define.tagData[i];
            }
            
            txt.text = LanguageUtils.getTextFormat(define.tag, objs);
        }

        private CityBuffDefine GetCityBuffDeine(int id)
        {
            for (int i = 0; i < m_cityBuffDefines.Count; i++)
            {
                if (m_cityBuffDefines[i].ID == id)
                {
                    return m_cityBuffDefines[i];
                }
            }
            
            return null;
        }

    }
}