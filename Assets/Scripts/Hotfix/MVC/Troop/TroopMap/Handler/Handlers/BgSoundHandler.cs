using System.Collections.Generic;
using Client;
using Game;
using Skyunion;

namespace Hotfix
{
    public class BgSoundHandler : IBgSoundHandler
    {

        private const string PVP_BGM = "Bgm_Battle";
        private const string Normal_DayTime = "Bgm_PeaceDay";
        private const string Normal_NightTime = "Bgm_PeaceNight";
        private TroopProxy m_TroopProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;

        private Dictionary<int, bool> behaviorIds = new Dictionary<int, bool>();
        
        public enum BGMType
        {
            Normal,
            PVP,
        }

        private BGMType curBGMType = BGMType.Normal;
        
        public void Init()
        {
            //PlayCurBGMSound();
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_TroopProxy=AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
        }
        // 重登清理buf
        public void Clear()
        {
            curBGMType = BGMType.Normal;
            //PlayCurBGMSound();
            behaviorIds.Clear();
        }
        
        public void WantChangeBGMOnBehaviorStateChange(int id)
        {
            if (!behaviorIds.ContainsKey(id))
            {
                behaviorIds.Add(id, true);
            }
            
            BGMType newBgmType = BGMType.Normal;
            if (IsInPVPStatus())
            {
                newBgmType = BGMType.PVP;
            }

            if (newBgmType != curBGMType)
            {
                curBGMType = newBgmType;
                PlayCurBGMSound();
            }
        }

        private void PlayCurBGMSound()
        {
            switch (curBGMType)
            {
                case BGMType.Normal:
                    CoreUtils.audioService.PlayBgm(Normal_DayTime);
                    break;
                case BGMType.PVP:
                    CoreUtils.audioService.PlayBgm(PVP_BGM);
                    break;
            }
        }

        /// <summary>
        /// 是否处于PVP状态。
        /// PVP状态判定规则：攻打别人城池，圣地，或者被部队和城池被别人打，则认为进入PVP状态
        /// </summary>
        /// <returns></returns>
        private bool IsInPVPStatus()
        {
            foreach (var dict in behaviorIds)
            {
                int id = dict.Key;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                if (armyData != null )
                {
                    if (armyData.isPlayerHave && armyData.state == Troops.ENMU_SQUARE_STAT.FIGHT)    // 处于战斗状态
                    {
                        MapObjectInfoEntity targetMapObjectInfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(armyData.targetId);
                        if (targetMapObjectInfo != null && IsPVPRssType(targetMapObjectInfo.rssType))
                        {
                            return true;
                        }
                    }
                }
                
                /*// 建筑数据走这里
                MapObjectInfoEntity mapobjectinfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
                if (mapobjectinfo != null)
                {
                    if (TroopHelp.GetTroopState(mapobjectinfo.status) == Formation.ENMU_SQUARE_STAT.FIGHT)
                    {
                        RssType rssType = mapobjectinfo.rssType;
                        
                        if (rssType >= RssType.City && rssType <= RssType.Gem)
                        {
                            
                        }
                        
                        
                    }
                    
                    continue;       
                }*/
            }
            
            
            /*var troops = m_TroopProxy.GetArmys();
            
            for (int i = 0; i < troops.Count; i++)
            {
                ArmyInfoEntity armyInfo = troops[i];
                Formation.ENMU_SQUARE_STAT state = TroopHelp.GetTroopState(armyInfo.status);
                
                if (state == Formation.ENMU_SQUARE_STAT.FIGHT)    // 处于战斗状态
                {
                    // 攻击目标为PVP目标
                    RssType targetType = (RssType) armyInfo.targetType;
                    if (targetType == RssType.Troop || 
                        (targetType >= RssType.City && targetType <= RssType.Gem) || 
                        targetType >= RssType.Village && targetType <= RssType.Checkpoint_3)
                    {

                        return true;
                    }
                }
            }*/
            
            
            
            
            
            return false;
        }

        // 是否类型为PVP类型
        private bool IsPVPRssType(RssType rssType)
        {
            if (rssType == RssType.Troop || 
                (rssType >= RssType.City && rssType <= RssType.Gem) || 
                rssType >= RssType.Village && rssType <= RssType.Checkpoint_3)
            {

                return true;
            }
            
            return false;
        }

    }
}