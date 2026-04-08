using Client;
using Skyunion;
using UnityEngine;
using Hotfix;

namespace Game
{
    public sealed class RuneCollectHudView
    {
        private UI_Pop_TroopsCollectBarView m_view = null;
        private MapObjectInfoEntity m_mapObjectInfo = null;
        private HUDUI m_hudUI = null;
        private Data.ConfigDefine m_config = null;

        public void Create(MapObjectInfoEntity data)
        {
            m_mapObjectInfo = data;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)m_mapObjectInfo.objectId);
            if (armyData == null) return;  
            m_hudUI = HUDUI
                   .Register(UI_Pop_TroopsCollectBarView.VIEW_NAME, typeof(UI_Pop_TroopsCollectBarView),
                       HUDLayer.world, armyData.go).SetInitCallback(OnCollectRuneHudInitCallback)
                       .SetUpdateCallback(UpdateCollectRuneProgressBar, 1)
                   .SetTargetGameObject(armyData.go)
                   .SetCameraLodDist(0, 3000f).SetPositionAutoAnchor(true);
            ClientUtils.hudManager.ShowHud(m_hudUI);
            m_config = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0);
        }

        public void Close()
        {
            if(m_hudUI != null)
            {
                HUDManager.Instance().CloseSingleHud(ref m_hudUI);
            }
            m_view = null;
            m_hudUI = null;
            m_mapObjectInfo = null;
        }

        private void OnCollectRuneHudInitCallback(HUDUI hudUI)
        {
            m_view = hudUI.gameView as UI_Pop_TroopsCollectBarView;
            UpdateCollectRuneProgressBar(hudUI);
        }


        private void UpdateCollectRuneProgressBar(HUDUI hudui)
        {
            if (m_view == null) return;
            long time = ServerTimeModule.Instance.GetServerTime() - m_mapObjectInfo.collectRuneTime;
            float fillAmount = time * 1.0f / m_config.collectCircleTime;
            m_view.m_img_bar_PolygonImage.fillAmount = fillAmount;
        }

    }
}