using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hotfix;

namespace Game
{
    public enum GameModeType
    {
        /// <summary>
        /// 大世界
        /// </summary>
        World,
        /// <summary>
        /// 远征视角
        /// </summary>
        Expedition,
        /// <summary>
        /// 城市布局视角
        /// </summary>
        Citylayout,

    }

    public class GameModeManager: TSingleton<GameModeManager>
    {
        public GameModeType CurGameMode { get; private set; }

        protected override void Init()
        {
            CurGameMode = GameModeType.World;
        }

        public void ChangeMode(GameModeType mode)
        {
            CurGameMode = mode;
            AppFacade.GetInstance().SendNotification(CmdConstant.GameModeChanged);
        }

    }
}

