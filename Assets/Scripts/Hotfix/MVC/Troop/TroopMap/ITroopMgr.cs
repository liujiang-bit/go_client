using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Client;
using PureMVC.Interfaces;
using UnityEngine;

namespace Hotfix
{
    public interface ITroopMgr
    {
        void CreateTroopData(INotification notification);
        void UpdateArmyData(int objectId, SprotoType.MapObjectInfo info);

        #region create army gameobject
        void AddTroop(int objectId, Action callback);
        #endregion

        void ChangeFormationState(Troops.ENMU_MATRIX_TYPE type, int objectId, Troops.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f); 
        void Clear();
        Troops GetTroop(int objectId);
        List<Troops> GetTroops();
        List<int> GetArmyDatas();
        Troops GetFormationBarbarian(int objectId);
        bool IsContainTroop(int objectId);
        bool IsContainBarbarian(int objectId);
        void SwitchShowMode(int objectId,string info);
        ArmyData GetArmyData(int objectId);
        ArmyData GetArmyDataByArmyId(int armyIndex);
        ArmyData GetScoutData(int objectId);
        ArmyData GetScoutDataByScoutId(int scoutId);
        ArmyData GetTransportData(int objectId);
        ArmyData GetTransportDataById(int transportId);

        bool RemoveTroop(int objectId);
        bool RemoveOwnTroop(int objectId);
        void SetFormationInfo(int objectId, string des);
        void TriggerSkillS(int objectId, string heroId, Vector3 pos);
        bool IsShowEffect(int objectId);
        bool isHeroPlaySkill(int objectId, int skillId);
        bool isVicePlaySkill(int objectId, int skillId);
        bool IsRallyTroop(int objectId);

        void UpdateTarget(int objectId, int target_id);
        void UpdateArmyDir();
        void UpdateAttackerDir(int beAttackId);
        bool GetAttackerPos(int beAttackId, out int attackId, out Vector3 attackPos);
        int CalStanceIndex(int targetId, float stanceAngle);
        void RemoveSound(int id);
        void UpdateTroopsColor();
        void UpdateTroopColor(int troopId);
    }
}