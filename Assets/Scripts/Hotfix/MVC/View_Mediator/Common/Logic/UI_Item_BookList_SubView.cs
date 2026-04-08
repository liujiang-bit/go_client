// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月15日
// Update Time         :    2020年9月15日
// Class Description   :    UI_Item_BookList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using Data;

namespace Game {
    public partial class UI_Item_BookList_SubView : UI_SubView
    {
        private MapMarkerInfo m_mapMarkerInfo;
        
        protected override void BindEvent()
        {
            m_btn_change_GameButton.onClick.AddListener(onEdit);
            m_btn_delete_GameButton.onClick.AddListener(onDelete);
            m_btn_share_GameButton.onClick.AddListener(onShare);
            m_btn_go.m_btn_languageButton_GameButton.onClick.AddListener(onGo);
        }

        public void Refresh(MapMarkerInfo mapMarkerInfo)
        {
            if (mapMarkerInfo == null)
            {
                return;
            }

            m_mapMarkerInfo = mapMarkerInfo;

            if (mapMarkerInfo.status == 0)
            {
                m_UI_Common_Redpoint.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Common_Redpoint.gameObject.SetActive(false);
            }

            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)mapMarkerInfo.markerId);
            if (mapMarkerTypeDefine != null)
            {
                if (mapMarkerTypeDefine.type == 1)
                {
                    m_lbl_name_LanguageText.text = string.Empty;
                    m_lbl_lbl_LanguageText.gameObject.SetActive(false);
                }
                else if (mapMarkerTypeDefine.type == 2)
                {
                    m_lbl_name_LanguageText.text = mapMarkerInfo.createName;
                    m_lbl_lbl_LanguageText.gameObject.SetActive(true);                    
                }
                else
                {
                    Debug.LogErrorFormat("MapMarker Type Error. ID:{0}", mapMarkerTypeDefine.ID);
                }

                m_lbl_dec_LanguageText.text = mapMarkerInfo.description;

                ClientUtils.LoadSprite(m_img_booktype_PolygonImage, mapMarkerTypeDefine.iconImg);
            }

            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_lbl_coordinate_LanguageText.text = LanguageUtils.getTextFormat(910010, playerProxy.GetGameNode().ToString("N0"), mapMarkerInfo.pos.x / 600, mapMarkerInfo.pos.y / 600);
        }

        private void onEdit()
        {
            if (m_mapMarkerInfo == null)
            {
                return;
            }

            MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)m_mapMarkerInfo.markerId);
            if (mapMarkerTypeDefine == null)
            {
                return;
            }

            //个人
            if (mapMarkerTypeDefine.type == 1)
            {
                MapMarkerOperationData mapMarkerOperationData = new MapMarkerOperationData();
                mapMarkerOperationData.type = MapMarkerOperationType.Edit_Person;
                mapMarkerOperationData.personMarkerIndex = m_mapMarkerInfo.markerIndex;
                CoreUtils.uiManager.ShowUI(UI.s_MapMarkerOperation, null, mapMarkerOperationData);
            }
            //联盟
            else if (mapMarkerTypeDefine.type == 2)
            {
                AllianceProxy allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

                if (allianceProxy.HasJionAlliance() && allianceProxy.getMemberInfo(playerProxy.Rid).guildJob >= 4)
                {
                    MapMarkerOperationData mapMarkerOperationData = new MapMarkerOperationData();
                    mapMarkerOperationData.type = MapMarkerOperationType.Edit_Guild;
                    mapMarkerOperationData.guildMarkerId = m_mapMarkerInfo.markerId;
                    CoreUtils.uiManager.ShowUI(UI.s_MapMarkerOperation, null, mapMarkerOperationData);
                }
                else
                {
                    Tip.CreateTip(730136).Show();
                }
            }
        }

        private void onDelete()
        {
            if (m_mapMarkerInfo == null)
            {
                return;
            }

            MapMarkerProxy mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;
            mapMarkerProxy.DeleteMapMarker(m_mapMarkerInfo.markerIndex, m_mapMarkerInfo.markerId);
        }

        private void onShare()
        {
            if (m_mapMarkerInfo == null)
            {
                return;
            }

            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            ChatShareData chatShareData = new ChatShareData();
            chatShareData.vector2Int = new Vector2Int(PosHelper.ServerUnitToClientPos(m_mapMarkerInfo.pos.x), PosHelper.ServerUnitToClientPos(m_mapMarkerInfo.pos.y));
            chatShareData.gameNode = playerProxy.GetGameNode().ToString();
            chatShareData.shareType = 0;
            chatShareData.desc = m_mapMarkerInfo.description;
            chatShareData.ChatShareDefine = CoreUtils.dataService.QueryRecord<ChatShareDefine>(99);
            CoreUtils.uiManager.ShowUI(UI.s_choseChat, null, chatShareData);
        }

        private void onGo()
        {
            if (m_mapMarkerInfo == null)
            {
                return;
            }

            WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 500, () =>
            {
                WorldCamera.Instance().ViewTerrainPos(PosHelper.ServerUnitToClientUnit(m_mapMarkerInfo.pos.x), PosHelper.ServerUnitToClientUnit(m_mapMarkerInfo.pos.y), 500, () => { });
            });

            CoreUtils.uiManager.CloseUI(UI.s_MapMarker);
        }
    }
}