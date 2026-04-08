using System.Collections.Generic;

namespace Hotfix
{
    public class WorldMapLogicMgr : TSingleton<WorldMapLogicMgr>
    {
        private readonly List<IBaseHandler> m_handlers = new List<IBaseHandler>();

        public IBattleUIHandler BattleUIHandler { get; private set; }
        public IBattleUIEffectHandler BattleUIEffectHandler { get; private set; }
        public IBattleBuffHandler BattleBuffHandler { get; private set; }
        public IMapBuildingFightHandler MapBuildingFightHandler { get; private set; }
        public IUpdateDataHandler UpdateDataHandler { get; private set; }
        public ITroopLinesHandler TroopLinesHandler { get; private set; }
        public IBehaviorHandler BehaviorHandler { get; private set; }
        public IMapTroopHandler MapTroopHandler { get; private set; }
        public IBattleSoundHandler BattleSoundHandler { get; private set; }
        
        public IBgSoundHandler BgSoundHandler { get; private set; }
        public IBattleBroadcastsHandler BattleBroadcastsHandler { get; private set; }
        public IPlayTroopCheckHandler PlayTroopCheckHandler { get; private set; }
        public IUpdateMapDataHandler UpdateMapDataHandler { get; private set; }
        public IGuardianHandler GuardianHandler { get; private set; }
        public IMapSelectEffectHandler MapSelectEffectHandler { get; private set; }
        public IMapSoundHandler MapSoundHandler { get; private set; }
        public IBattleRemainSoldiersHandler BattleRemainSoldiersHandler { get; private set; }
        public IMapTouchHandler MapTouchHandler { get; private set; }

        public void Initialize()
        {
            BattleUIHandler = AddHandler<BattleUIHandler>() as IBattleUIHandler;
            BattleUIEffectHandler = AddHandler<BattleUIEffectHandler>() as IBattleUIEffectHandler;
            BattleBuffHandler = AddHandler<BattleBuffHandler>() as IBattleBuffHandler;
            MapBuildingFightHandler = AddHandler<MapBuildingFightHandler>() as IMapBuildingFightHandler;
            UpdateDataHandler = AddHandler<TroopUpdateDataMgr>() as IUpdateDataHandler;
            TroopLinesHandler = AddHandler<TroopLinesMgr>() as ITroopLinesHandler;
            BehaviorHandler = AddHandler<BehaviorMgr>() as IBehaviorHandler;
            MapTroopHandler = AddHandler<MapTroopHandler>() as IMapTroopHandler;
            BattleSoundHandler = AddHandler<BattleSoundHandler>() as IBattleSoundHandler;
            BgSoundHandler = AddHandler<BgSoundHandler>() as IBgSoundHandler;
            BattleBroadcastsHandler = AddHandler<BattleBroadcastsHandler>() as IBattleBroadcastsHandler;
            PlayTroopCheckHandler= AddHandler<PlayTroopCheckHandler>() as IPlayTroopCheckHandler;
            UpdateMapDataHandler = AddHandler<UpdateMapDataHandler>() as IUpdateMapDataHandler;
            GuardianHandler= AddHandler<GuardianHandler>() as IGuardianHandler;
            MapSelectEffectHandler = AddHandler<MapSelectEffectHandler>() as IMapSelectEffectHandler;
            MapSoundHandler = AddHandler<MapSoundHandler>() as IMapSoundHandler;
            BattleRemainSoldiersHandler = AddHandler<BattleRemainSoldiersHandler>() as IBattleRemainSoldiersHandler;
            MapTouchHandler = AddHandler<MapTouchHandler>() as IMapTouchHandler;
            InitData();
        }
        
        private IBaseHandler AddHandler<T>() where T : class , IBaseHandler, new()
        {
            T t = new T();
            m_handlers.Add(t);
            return t;
        }


        public void Clear()
        {
            foreach (var handler in m_handlers)
            {
                handler.Clear();
            }
        }

        private void InitData()
        {
            foreach (var handler in m_handlers)
            {
                handler.Init();
            }
        }
    }
}