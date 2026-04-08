using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Client;
using Game;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Hotfix
{      
    public class ArmyData
    {
        protected int id;

        #region From Server Data
        public int objectId { get; set; }
        public int targetId { get { return m_mapObjectInfo != null ? (int)m_mapObjectInfo.targetObjectIndex : 0; } }
        public int heroId { get; set; }
        public int viceId { get; set; }
        public int scoutIndex { get { return m_mapObjectInfo != null ? (int)m_mapObjectInfo.scoutsIndex : 0; } }
        public string armyName { get { return m_mapObjectInfo != null ? m_mapObjectInfo.armyName : string.Empty; } }
        public virtual float armyRadius { get { return m_mapObjectInfo != null ? m_mapObjectInfo.armyRadius / 100.0f : 0; } }
        public long armyStatus { get; set; }
        public virtual int troopId { get { return m_mapObjectInfo != null ? (int)m_mapObjectInfo.armyIndex : 0; } }
        public int troopNums { get; set; }
        public long arrivalTime { get; set; }
        public long startTime { get; set; }
        public virtual RssType troopType {  get { return m_mapObjectInfo != null ? (RssType)m_mapObjectInfo.objectType : RssType.None; } }
        public float attackAngle { get { return m_mapObjectInfo != null ? m_mapObjectInfo.targetAngle / 100f : 0; } }
        public long guild { get { return m_mapObjectInfo != null ? m_mapObjectInfo.guildId : 0; } }
        public string guildAbbName { get { return m_mapObjectInfo != null ? m_mapObjectInfo.guildAbbName : string.Empty; } }
        public long anger { get { return m_mapObjectInfo != null ? m_mapObjectInfo.sp : 0; } }
        public long angerMax { get { return m_mapObjectInfo != null ? m_mapObjectInfo.maxSp : 0; } }
        public List<SkillInfo> mainSkillInfo { get { return m_mapObjectInfo != null ? m_mapObjectInfo.mainHeroSkills : null; } }
        public List<SkillInfo> viceSkillInfo { get { return m_mapObjectInfo != null ? m_mapObjectInfo.deputyHeroSkills : null; } }
        public List<long> guildFlagSigns { get { return m_mapObjectInfo != null ? m_mapObjectInfo.guildFlagSigns : null; } }
        public bool isRally { get { return m_mapObjectInfo != null ? m_mapObjectInfo.isRally : false; } }
        public virtual long armyRid { get { return m_mapObjectInfo != null ? m_mapObjectInfo.armyRid : 0; } }
        public bool isGuide { get { return m_mapObjectInfo != null ? m_mapObjectInfo.isGuide : false; } }
        #endregion

        #region Extend Data
        public string des { get; set; }
        public GameObject go { get; set; }
        public Troops.ENMU_SQUARE_STAT state { get { return TroopHelp.GetTroopState((long)armyStatus); } }
        public virtual bool isPlayerHave { get { return false; } }

        private bool _isCreate;
        public bool isCreate
        {
            get
            {
                if (_isCreate)
                {
                    _isCreate = false;
                    return true;
                }

                return false;
            }
            
            set { _isCreate = value; }
        }

        public int troopNumMax { get; set; }
        public AudioHandler m_attackAudioHandler { get; set; }
        public AudioHandler m_stopAudioHandler { get; set; }
        public AudioHandler m_moveAudioHandler { get; set; }
        public AudioHandler m_failAudioHandler { get; set; }
        public int autoMoveIndex { get; set; }
        public Dictionary<Int64, SoldierInfo> soldiers { get; set; }
        public List<long> buffID { get; } = new List<long>();
        public virtual Vector2 Pos { get; set; } = Vector2.zero;
        public Vector2 FormationInitTargetPos { get; set; } = Vector2.zero;
        public List<Vector2> movePath { get; } = new List<Vector2>();
        public virtual int dataIndex { get { if (m_mapObjectInfo.rssType == RssType.Scouts) return (int)m_mapObjectInfo.scoutsIndex; return 0; } }
        public long targetArgObjectId { get; set; }

        #endregion

        protected MapObjectInfoEntity m_mapObjectInfo = null;

        public ArmyData(int id)
        {
            this.id = id;
            Initialize();
        }

        protected void Initialize()
        {
            FillMapObjectInfo(id);
        }

        public void ClearMovePath()
        {
            autoMoveIndex = 0;
            movePath.Clear();
        }

        public virtual void SetMovePath(Vector2 v2)
        {
            movePath.Add(v2);
        }

        public void FillMapObjectInfo(int objectId)
        {
            var worldObjetProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            if (worldObjetProxy != null)
            {
                m_mapObjectInfo = worldObjetProxy.GetWorldMapObjectByobjectId(objectId);
                this.objectId = objectId;
            }
        }

        private void CalcPosAndIndex(out Vector2 resultPos,out int resultIndex)
        {
            TroopHelp.CalcPosAndIndex(out resultPos, out resultIndex
                , armyStatus
                , Pos
                , movePath
                , startTime
                , arrivalTime);
        }
        public Vector2 GetMovePos()
        {
            CalcPosAndIndex(out var pos , out var index);
            return pos;
        }
        public int GetMoveIndex()
        {
            CalcPosAndIndex(out var pos , out var index);
            return index;
        }

        public void RemoveSoundHandler()
        {
            if (m_stopAudioHandler != null)
            {
                WorldMapLogicMgr.Instance.BattleSoundHandler.DestroySound(m_stopAudioHandler,
                    () => { m_stopAudioHandler = null; });
            }
            if (m_failAudioHandler != null)
            {
                WorldMapLogicMgr.Instance.BattleSoundHandler.DestroySound(m_failAudioHandler,
                    () => { m_failAudioHandler = null; });               
            }
            if (m_moveAudioHandler != null)
            {
                WorldMapLogicMgr.Instance.BattleSoundHandler.DestroySound(m_moveAudioHandler,
                    () => { m_moveAudioHandler = null; });
            }
            if (m_attackAudioHandler != null)
            {
                WorldMapLogicMgr.Instance.BattleSoundHandler.DestroySound(m_attackAudioHandler,
                    () => { m_attackAudioHandler = null; });
            }
        }
    }

    public class SelfTroopData : ArmyData
    {
        private PlayerProxy m_playerProxy;
        public SelfTroopData(int id) : base(id)
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }
 
        public override int troopId { get { return id; } }  
        public override bool isPlayerHave { get { return true; } }
        public override RssType troopType { get { return RssType.Troop; } }    
        public override int dataIndex { get { return id; } }
        public override long armyRid { get { return m_playerProxy.CurrentRoleInfo.rid; } }
    }

    public class SelfTransportData : ArmyData
    {
        private PlayerProxy m_playerProxy;
        public SelfTransportData(int id) : base(id)
        {
            heroId = id;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }
        
        public override bool isPlayerHave { get { return true; } }
        public override RssType troopType { get { return RssType.Transport; } }        
        public override int dataIndex { get { return id; } }
        public override long armyRid { get { return m_playerProxy.CurrentRoleInfo.rid; } }
    }

    public class SelfScoutData : ArmyData
    {
        private PlayerProxy m_playerProxy;
        public SelfScoutData(int id) : base(id)
        {
            heroId = id;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        public override bool isPlayerHave { get { return true; } }
        public override RssType troopType { get { return RssType.Scouts; } }       
        public override int dataIndex { get { return id; } }
        public override long armyRid { get { return m_playerProxy.CurrentRoleInfo.rid; } }
    }

    public class ExpeditionArmyData : ArmyData
    {
        public ExpeditionArmyData(int id) : base(id)
        {

        }

        public bool IsEnemy { get { return armyRid == 0; } }

        public override int dataIndex { get { return id; } }
        public override bool isPlayerHave { get { return !IsEnemy; } }
        public override Vector2 Pos
        {
            get
            {
                return base.Pos;
            }
            set
            {
                ExpeditionProxy expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
                if (expeditionProxy != null)
                {
                    value = expeditionProxy.ExpeditionPosToWorldPos(value.x, value.y);
                }
                base.Pos = value;
            }
        }
        public float MonsterRadius { get; set; }
        public override float armyRadius {  get { if (IsEnemy) return MonsterRadius; return base.armyRadius; } }

        public override void SetMovePath(Vector2 v2)
        {
            ExpeditionProxy expeditionProxy = AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
            if(expeditionProxy != null)
            {
                v2 = expeditionProxy.ExpeditionPosToWorldPos(v2.x, v2.y);
            }
            base.SetMovePath(v2);
        }
    }
}