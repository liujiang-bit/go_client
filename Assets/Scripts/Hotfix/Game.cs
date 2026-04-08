using Skyunion;
using Client;
using Game;
using UnityEngine;

namespace Hotfix
{
    public class Game 
    {
        public void Initialize(IPluginManager pluginManager, string name)
        {
            // 阻止熄屏
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            AppFacade.GetInstance().StartUp(); float startTime1 = Time.realtimeSinceStartup;
        }
        public void Update()
        {
            Common.Update();
            HotfixUtil.Clear();
        }
        public void Shut()
        {
            DestroyInstance();
        }
        public static void DestroyInstance()
        {
            HotfixUtil.Clear();
            ArmyInfoHelp.Destroy();
            AlertManager.Destroy();
            AutoMoveMgr.Destroy();
            AutoMoveMgr.Destroy();
            CityHudCountDownManager.Destroy();
            FightHelper.Destroy();
            GameModeManager.Destroy();
            GlobalBehaviourManger.Destroy();
            GuideManager.Destroy();
            ServerTimeModule.Destroy();
            SquareHelper.Destroy();
            SubViewManager.Destroy();
            SummonerTroopMgr.Destroy();
            TipManager.Destroy();
            WorldMapLogicMgr.Destroy();
            AppFacade.Destroy();

            TroopsDatas.Instance.Clear();
            ClientUtils.ClearCore();
            CoreUtils.ClearCore();
        }
    }
}
