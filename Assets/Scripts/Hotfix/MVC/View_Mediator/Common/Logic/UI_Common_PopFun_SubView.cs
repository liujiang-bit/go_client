// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月22日
// Update Time         :    2020年7月22日
// Class Description   :    UI_Common_PopFun_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Common_PopFun_SubView : UI_SubView
    {
        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;
        private ChatProxy chatProxy;

        protected override void BindEvent() {
        }
        public void InitSubView(MapObjectInfoEntity mapObjectInfoEntity)
        {
            chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            if (mapObjectInfoEntity == null)
            {
                return;
            }
            
            m_btn_collect_GameButton.gameObject.SetActive(false);
            m_btn_sign_GameButton.gameObject.SetActive(false);

            m_btn_face_GameButton.gameObject.SetActive(false);

            m_pl_share_Animator.gameObject.SetActive(true);
            m_btn_share_GameButton.gameObject.SetActive(true);
            m_pl_list_VerticalLayoutGroup.gameObject.SetActive(false);

            InitcollectBtn(mapObjectInfoEntity);

            m_btn_share_GameButton.onClick.AddListener(() => { OnShareBtnClick(mapObjectInfoEntity); });
            m_btn_pos_GameButton.onClick.AddListener(() => { OnPosBtnClick(mapObjectInfoEntity); });
            m_btn_city_GameButton.onClick.AddListener(() => { OnCityBtnClick(mapObjectInfoEntity); });            
            m_btn_collect_GameButton.onClick.AddListener(() => { OnCollectBtnClick(mapObjectInfoEntity); });
            m_btn_sign_GameButton.onClick.AddListener(() => { OnSignBtnClick(mapObjectInfoEntity); });
            m_btn_face_GameButton.onClick.AddListener(() => { OnFaceBtnClick(mapObjectInfoEntity); });
        }
        private void InitcollectBtn(MapObjectInfoEntity mapObjectInfoEntity)
        {
            //玩家城市
            if (mapObjectInfoEntity.objectType == (long)RssType.City ||
                //野外资源田
                mapObjectInfoEntity.objectType == (long)RssType.Stone ||
                mapObjectInfoEntity.objectType == (long)RssType.Farmland ||
                mapObjectInfoEntity.objectType == (long)RssType.Wood ||
                mapObjectInfoEntity.objectType == (long)RssType.Gold ||
                mapObjectInfoEntity.objectType == (long)RssType.Gem ||
                //野蛮人要塞
                mapObjectInfoEntity.objectType == (long)RssType.BarbarianCitadel ||
                //联盟要塞、联盟旗帜、联盟资源中心
                mapObjectInfoEntity.objectType == (long)RssType.GuildCenter ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildFortress1 ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildFortress2 ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildFlag ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildFoodResCenter ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildWoodResCenter ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildGoldResCenter ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildGemResCenter ||
                //联盟资源点
                mapObjectInfoEntity.objectType == (long)RssType.GuildFood ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildWood ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildStone ||
                mapObjectInfoEntity.objectType == (long)RssType.GuildGold ||
                //圣地、关卡奇观建筑
                mapObjectInfoEntity.objectType == (long)RssType.CheckPoint ||
                mapObjectInfoEntity.objectType == (long)RssType.HolyLand ||
                mapObjectInfoEntity.objectType == (long)RssType.Sanctuary ||
                mapObjectInfoEntity.objectType == (long)RssType.Altar ||
                mapObjectInfoEntity.objectType == (long)RssType.Shrine ||
                mapObjectInfoEntity.objectType == (long)RssType.LostTemple ||
                mapObjectInfoEntity.objectType == (long)RssType.Checkpoint_1 ||
                mapObjectInfoEntity.objectType == (long)RssType.Checkpoint_2 ||
                mapObjectInfoEntity.objectType == (long)RssType.Checkpoint_3 ||
                //山洞、村庄
                mapObjectInfoEntity.objectType == (long)RssType.Cave ||
                mapObjectInfoEntity.objectType == (long)RssType.Village)
            {
                if (m_allianceProxy.HasJionAlliance() && m_allianceProxy.getMemberInfo(m_playerProxy.Rid)!=null && m_allianceProxy.getMemberInfo(m_playerProxy.Rid).guildJob >= 4)
                {
                    m_btn_sign_GameButton.gameObject.SetActive(true);
                }
                else
                {
                    m_btn_collect_GameButton.gameObject.SetActive(true);
                }
            }
        }

        public void OnShareBtnClick(MapObjectInfoEntity mapObjectInfoEntity)
        {
            switch (mapObjectInfoEntity.rssType)
            {
                case RssType.City:
                    {
                        m_pl_share_Animator.Play("UA_TagAct_Open");
                        Timer.Register(0.25f,()=>{
                            m_btn_share_GameButton.gameObject.SetActive(false);
                            m_pl_list_VerticalLayoutGroup.gameObject.SetActive(true);
                        },null,false,false, m_UI_Common_PopFun_ArabLayoutCompment);
                    }
                    break;
                default:
                    {
                        // Debug.LogError("PopFun SubView Share Button Logic Error.");
                        OnPosBtnClick(mapObjectInfoEntity);
                        CloseMapObjectPopWin();
                    }
                    break;
            }
        }

        public void OnPosBtnClick(MapObjectInfoEntity mapObjectInfoEntity)
        {
            m_pl_share_Animator.Play("UA_TagAct_Close");
            CloseMapObjectPopWin();
            ChatShareData chatShareData = new ChatShareData();
                        chatShareData.guildAbbName = mapObjectInfoEntity.guildAbbName;
                        chatShareData.vector2Int = new Vector2Int(PosHelper.ServerUnitToClientPos(mapObjectInfoEntity.objectPos.x), PosHelper.ServerUnitToClientPos(mapObjectInfoEntity.objectPos.y));
                        chatShareData.gameNode = m_playerProxy.GetGameNode().ToString();
                        chatShareData.shareType = 0;
            chatProxy.SetChatShareData(mapObjectInfoEntity, chatShareData);
                        CoreUtils.uiManager.ShowUI(UI.s_choseChat, null, chatShareData);
        }

        public void OnCityBtnClick(MapObjectInfoEntity mapObjectInfoEntity)
        {
            Tip.CreateTip(100045).Show();
           // CloseMapObjectPopWin();
        }

        public void OnCollectBtnClick(MapObjectInfoEntity mapObjectInfoEntity)
        {
            CloseMapObjectPopWin();
            MapMarkerOperationData mapMarkerOperationData = new MapMarkerOperationData();
            mapMarkerOperationData.type = MapMarkerOperationType.Add_Or_Edit_Person;
            mapMarkerOperationData.mapObjectInfoEntity = mapObjectInfoEntity;
            mapMarkerOperationData.x = mapObjectInfoEntity.objectPos.x;
            mapMarkerOperationData.y = mapObjectInfoEntity.objectPos.y;
            CoreUtils.uiManager.ShowUI(UI.s_MapMarkerOperation, null, mapMarkerOperationData);
        }

        public void OnSignBtnClick(MapObjectInfoEntity mapObjectInfoEntity)
        {
            CloseMapObjectPopWin();
            MapMarkerOperationData mapMarkerOperationData = new MapMarkerOperationData();
            mapMarkerOperationData.type = MapMarkerOperationType.Add_Or_Edit_Guild;
            mapMarkerOperationData.mapObjectInfoEntity = mapObjectInfoEntity;
            mapMarkerOperationData.x = mapObjectInfoEntity.objectPos.x;
            mapMarkerOperationData.y = mapObjectInfoEntity.objectPos.y;
            CoreUtils.uiManager.ShowUI(UI.s_MapMarkerOperation, null, mapMarkerOperationData);
        }

        public void OnFaceBtnClick(MapObjectInfoEntity mapObjectInfoEntity)
        {
            CloseMapObjectPopWin();
            Tip.CreateTip(100045).Show();
        }

        public void AddFaceEvent(UnityAction unityAction)
        {
            m_btn_face_GameButton.onClick.AddListener(unityAction);
        }

        public void AddCollectEvent(UnityAction unityAction)
        {
            m_btn_collect_GameButton.onClick.AddListener(unityAction);
        }

        public void AddSignEvent(UnityAction unityAction)
        {
            m_btn_sign_GameButton.onClick.AddListener(unityAction);
        }

        public void AddShareEvent(UnityAction unityAction)
        {
            m_btn_share_GameButton.onClick.AddListener(unityAction);
        }

        public void AddSharePosEvent(UnityAction unityAction)
        {
            m_btn_pos_GameButton.onClick.AddListener(unityAction);
        }

        public void AddShareCityEvent(UnityAction unityAction)
        {
            m_btn_city_GameButton.onClick.AddListener(unityAction);
        }

        private void CloseMapObjectPopWin()
        {
            if (CoreUtils.uiManager.ExistUI(UI.s_Pop_WorldInfo))
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldInfo);
            }
            if (CoreUtils.uiManager.ExistUI(UI.s_pop_WorldMonster))
            {
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
            }
            if (CoreUtils.uiManager.ExistUI(UI.s_pop_WorldObjectPlayer))
            {
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldObjectPlayer);
            }
            if (CoreUtils.uiManager.ExistUI(UI.s_Pop_WorldObjectVillageCave))
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldObjectVillageCave);
            }
            if (CoreUtils.uiManager.ExistUI(UI.s_Pop_WorldObjectVillageCaveFinish))
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldObjectVillageCaveFinish);
            }
            if (CoreUtils.uiManager.ExistUI(UI.s_WorldObjectInfoBuild))
            {
                CoreUtils.uiManager.CloseUI(UI.s_WorldObjectInfoBuild);
            }
            if (CoreUtils.uiManager.ExistUI(UI.s_pop_WorldTunes))
            {
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldTunes);
            }
            if (CoreUtils.uiManager.ExistUI(UI.s_AllianceBuildInfoTip))
            {
                CoreUtils.uiManager.CloseUI(UI.s_AllianceBuildInfoTip);
            }
        }
    }
}