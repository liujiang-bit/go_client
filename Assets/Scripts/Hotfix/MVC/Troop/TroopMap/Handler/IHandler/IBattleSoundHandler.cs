using System;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public interface IBattleSoundHandler:IBaseHandler
    {
        void AddBattleSoundByBattleHit(ArmyData armyData,MapObjectInfoEntity infoEntity);
        void RemoveBattleSoundByBattleHit(ArmyData armyData, MapObjectInfoEntity infoEntity);
        void RefreshBattleSoundHitPos();
        void AddBattleStopSound(GameObject go, Action<AudioHandler> callback);
        void AddBattleMoveSound(GameObject go, Action<AudioHandler> callback);
        void AddBattleFailSound(GameObject go, Action<AudioHandler> callback);
        void RemoveSound(AudioHandler ah,bool onlyAudioComp = false);
        void DestroySound(AudioHandler ah,Action callback);
        void PlaySound(AudioHandler ah);
        void StopSound(AudioHandler ah);
        void Update();
    }
}