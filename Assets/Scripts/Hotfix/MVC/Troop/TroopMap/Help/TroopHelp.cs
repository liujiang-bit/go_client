using System;
using System.Collections.Generic;
using Client;
using Data;
using Game;
using Skyunion;
using UnityEngine;
using SprotoType;

namespace Hotfix
{
    
    public enum TroopAttackType
    {
        None = -1,

        /// <summary>
        /// 空地
        /// </summary>
        Space,

        /// <summary>
        /// 攻击
        /// </summary>
        Attack,

        /// <summary>
        /// 增援
        /// </summary>
        Reinforce,

        /// <summary>
        /// 集结
        /// </summary>
        Assembled,

        /// <summary>
        /// 采集
        /// </summary>
        Collect,

        ///撤退
        Retreat,

        /// <summary>
        /// 侦查
        /// </summary>    
        Scouts,

        /// <summary>
        /// 驻扎
        /// </summary>
        Stationed,

        /// <summary>
        /// 侦查回城
        /// </summary>
        ScoutsBack,
        /// <summary>
        /// 运输车回城
        /// </summary>
        TransportBack,
    }
    
      [Flags]
    public enum ArmyStatus
    {
        None = 0,

        /// <summary>
        /// 向空地行军
        /// </summary>
        SPACE_MARCH = 1 << 0,

        /// <summary>
        /// 攻击行军 2
        /// </summary>
        ATTACK_MARCH = 1 << 1,

        /// <summary>
        /// 采集行军 4
        /// </summary>
        COLLECT_MARCH = 1 << 2,

        /// <summary>
        /// 增援行军 8
        /// </summary>
        REINFORCE_MARCH = 1 << 3,

        /// <summary>
        /// 集结行军 16
        /// </summary>
        PALLY_MARCH = 1 << 4,

        /// <summary>
        /// 撤退行军 32
        /// </summary>
        RETREAT_MARCH = 1 << 5,

        /// <summary>
        /// 失败行军 64
        /// </summary>
        FAILED_MARCH = 1 << 6,

        /// <summary>
        /// 采集中 128
        /// </summary>
        COLLECTING = 1 << 7,

        /// <summary>
        /// 战斗中 256
        /// </summary>
        BATTLEING = 1 << 8,

        /// <summary>
        /// 驻扎中 512
        /// </summary>
        STATIONING = 1 << 9,

        /// <summary>
        /// 驻守中 1024
        /// </summary>
        GARRISONING = 1 << 10,

        /// <summary>
        /// 巡逻 2048
        /// </summary>
        PATROL = 1 << 11,

        /// <summary>
        /// 探索中（斥候）4096
        /// </summary>
        DISCOVER = 1 << 12,

        /// <summary>
        /// 返回中（斥候）8192
        /// </summary>
        RETURN = 1 << 13,

        /// <summary>
        ///待命中（斥候）16384
        /// </summary>
        STANBY = 1 << 14,

        /// <summary>
        ///返回城市中 32768
        /// </summary>
        BACK_CITY = 1 << 15,

        /// <summary>
        ///野蛮人溃败 65536
        /// </summary>
        MONSTER_FAILED = 1 << 16,

        /// <summary>
        ///军队待机 131072
        /// </summary>
        ARMY_SATNBY = 1 << 17,

        /// <summary>
        ///野蛮人待机 262144  这个服务器不用，用的是上面
        /// </summary>
        MONSTER_STANBY = 1 << 18,

        /// <summary>
        ///追击  524288
        /// </summary>
        FOLLOWUP = 1 << 19,

        /// <summary>
        /// 战斗移动 1048576  围击的时候使用
        /// </summary>
        MOVE = 1 << 20,

        /// <summary>
        /// 战斗调整位置 2097152  这个服务器不用，用的是上面
        /// </summary>
        ADJUST = 1 << 21,

        /// <summary>
        /// 集结等待中 4194304
        /// </summary>
        RALLY_WAIT = 1<<22,

        /// <summary>
        /// 加入集结中 8388608
        /// </summary>
        RALLY_JOIN_MARCH = 1<<23,

        /// <summary>
        /// 采集大地图物件 16777216
        /// </summary>
        COLLECTING_NO_DELETE = 1 << 24,

        /// <summary>
        /// 侦察（斥候侦查中）
        /// </summary>
        SCOUNT = 1 << 25,
        /// <summary>
        /// 斥候调查中
        /// </summary>
        SURVEYING = 1<<26,
        /// <summary>
        /// 集结部队战斗中
        /// </summary>
        RALLY_BATTLE=1<<27
    }

    public enum TouchTargetEfeectObjectType
    {
        City = 1,
        SelfTroop = 2,
        Monster = 3,
        Resource = 4,
        OtherPlayerTroop = 5,
        Rune = 6,
        AllianceBuilding=7,
        CheckPoint=8
    }

    public enum TroopBehavior
    {
        None=0,
        SpyOn, //侦查
        Attack, //攻击
        Play // 集结        
    }

    /// <summary>
    /// 行军迷雾数据
    /// </summary>
    public class TroopFogData
    {
        public Vector2Int[] nowTileList;
        public Vector2Int[] lastTileList;

        public TroopFogData()
        {
            nowTileList = new Vector2Int[9];
            lastTileList = new Vector2Int[9];
            for (int i = 0; i < 9; i++)
            {
                nowTileList[i] = Vector2Int.one * -1;
                lastTileList[i] = Vector2Int.one * -1;
            }
        }

        public static bool IsValidTile(Vector2Int value)
        {
            if (value == (Vector2Int.one * -1))
            {
                return false;
            }
            
            return true;
        }

        public static bool IsTheSameValue(Vector2Int value1, Vector2Int value2)
        {
            if (value1.x == value2.x && value1.y == value2.y)
            {
                return true;
            }
            
            return false;
        }



        public void UpdateTile(Vector2Int pos)
        {
            for (int i = 0; i < nowTileList.Length; i++)
            {
                lastTileList[i] = nowTileList[i];    // 处理之前的坐标
            }

            nowTileList[0] = pos;
            nowTileList[1] = new Vector2Int(pos.x - 1, pos.y);
            nowTileList[2] = new Vector2Int(pos.x - 1, pos.y + 1);
            nowTileList[3] = new Vector2Int(pos.x, pos.y + 1);
            nowTileList[4] = new Vector2Int(pos.x + 1, pos.y + 1);
            nowTileList[5] = new Vector2Int(pos.x + 1, pos.y);
            nowTileList[6] = new Vector2Int(pos.x + 1, pos.y - 1);
            nowTileList[7] = new Vector2Int(pos.x, pos.y - 1);
            nowTileList[8] = new Vector2Int(pos.x - 1, pos.y - 1);
        }

    }

    public static class TroopHelp
    {
        public static int m_ToouchAllianceName = 0;

        /// <summary>
        /// 获取线终点偏移。 如是否攻击军队，加上自身偏移（其实是处理行军线到达终点位置得 半径那一圈）
        /// </summary>
        /// <param name="armyData"></param>
        /// <returns></returns>
        public static Vector2 GetLineDestOffset(ArmyData armyData)
        {
            Vector2 offset = Vector2.zero;

            if (armyData.movePath.Count >= 2 && (!IsHaveState((long)armyData.armyStatus,ArmyStatus.SPACE_MARCH)))
            {
                int count = armyData.movePath.Count;
                offset = (armyData.movePath[count - 1] - armyData.movePath[count - 2]).normalized *armyData.armyRadius;
            }
            
            return offset;
        }

        /// <summary>
        /// 获取添加上偏移的路径
        /// </summary>
        /// <param name="movePath"></param>
        /// <param name="offset"></param>
        /// <param name="outMovePath"></param>
        /// <returns></returns>
        public static List<Vector2> GetMovePathWithOffset(List<Vector2> movePath, Vector2 offset)
        {
            var outMovePath = new List<Vector2>(movePath.ToArray());
            if (movePath.Count > 0)
            {
                outMovePath[movePath.Count - 1] = outMovePath[movePath.Count - 1] + offset;
            }
            return outMovePath;
        }

        //degress : y轴逆时针旋转的角度
        public static Vector2 Rotated(Vector2 this_, float degress)
        {
            Vector3 result = Quaternion.Euler(0, -degress, 0) * new Vector3(this_.x, 0, this_.y);
            return new Vector2(result.x, result.z);
        }

        //获取两点之间的距离
        public static float GetDistance(Vector2 pos1, Vector2 pos2)
        {
            return Vector2.Distance(pos1, pos2);
        }

        //计算角度
        public static float CalAnagle(Vector2 fromPos, Vector2 toPos)
        {
            return Mathf.Atan2(toPos.y - fromPos.y, toPos.x - fromPos.x) * Convert.ToSingle((180 / Math.PI));
        }

        //计算出城坐标
        public static Vector2 CalOutCityPos(Vector2 fromPos, Vector2 toPos, float radius = 350)
        {
            float angle = CalAnagle(fromPos, toPos);
            double radians = (Math.PI / 180) * angle;
            //Debug.LogErrorFormat("angle: {0} radians:{1} float:{2}", angle, radians, (float)radians);
            float speedx = Mathf.Floor(radius * Mathf.Cos((float) radians));
            float speedy = Mathf.Floor(radius * Mathf.Sign((float) radians));
            Vector2 vec = new Vector2(fromPos.x + speedx, fromPos.y + speedy);
            return vec;
        }

        //计算回城坐标
        public static Vector2 CalBackCityPos(Vector2 fromPos, Vector2 toPos)
        {
            float angle = CalAnagle(fromPos, toPos);
            float speedx = Mathf.Floor(350 * Mathf.Cos(Convert.ToSingle(Math.PI / 180) * angle));
            float speedy = Mathf.Floor(350 * Mathf.Sign(Convert.ToSingle(Math.PI / 180) * angle));
            Vector2 vec = new Vector2(toPos.x - speedx, toPos.y - speedy);
            return vec;
        }

        //计算回城坐标
        public static Vector2 CalMonsterPos(Vector2 fromPos, Vector2 toPos, float radius = 200)
        {
            float angle = CalAnagle(fromPos, toPos);
            double radians = (Math.PI / 180) * angle;
            float speedx = Mathf.Floor(radius * Mathf.Cos((float) radians));
            float speedy = Mathf.Floor(radius * Mathf.Sign((float) radians));
            //Debug.LogErrorFormat("speedx:{0} speedy:{1}", speedx, speedy);
            Vector2 vec = new Vector2(toPos.x - speedx, toPos.y - speedy);
            return vec;
        }

        public static float GetAngle(Vector3 a, Vector3 b)
        {
            b.x -= a.x;
            b.z -= a.z;

            float deltaAngle = 0;
            if (b.x == 0 && b.z == 0)
            {
                return 0;
            }
            else if (b.x > 0 && b.z > 0)
            {
                deltaAngle = 0;
            }
            else if (b.x > 0 && b.z == 0)
            {
                return 90;
            }
            else if (b.x > 0 && b.z < 0)
            {
                deltaAngle = 180;
            }
            else if (b.x == 0 && b.z < 0)
            {
                return 180;
            }
            else if (b.x < 0 && b.z < 0)
            {
                deltaAngle = -180;
            }
            else if (b.x < 0 && b.z == 0)
            {
                return -90;
            }
            else if (b.x < 0 && b.z > 0)
            {
                deltaAngle = 0;
            }

            float angle = Mathf.Atan(b.x / b.z) * Mathf.Rad2Deg + deltaAngle;
            return angle;
        }

        public static float GetMoveSpeed(List<Vector2> path, long time, long startTime)
        {
            float moveSpeed = 0f;

            float distance = 0f;
            for (int i = 0; i < path.Count; i++)
            {
                int index = i;
                if (i != path.Count-1)
                {
                    var nextIndex = i+1;
                    Vector2 curPos = path[index];
                    Vector2 nextPos = path[nextIndex];  
                    distance += Vector2.Distance(curPos, nextPos);
                }
            }

            if (startTime <= 0)
            {
                startTime = ServerTimeModule.Instance.GetServerTime();
            }

            long times = time - startTime;

            //服务端在寻路不到的情况下，可能下发一样的路径点，一样的起止时间
            if (times > 0)
            {
                moveSpeed = distance / times;
            }            

            return moveSpeed;
        }

        public static int GetRemainingTime(long arrivalTimes, long startTimes)
        {
            long curtime = ServerTimeModule.Instance.GetServerTime();
            long times = arrivalTimes - curtime;
            return (int) times;
        }
        
        public static void DestroyGoChild(Transform t)
        {
            int childCount = t.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject.Destroy(t.GetChild(i).gameObject);
            }
        }

        public static void CalcPosAndIndex(out Vector2 resultPos,out int resultIndex 
            ,long armyStatus 
            ,Vector2 defaultPos
            ,List<Vector2> movePath
            ,long startTime
            ,long arrivalTime)
        {
            if (IsHaveState(armyStatus, ArmyStatus.COLLECTING) 
                ||IsHaveState(armyStatus, ArmyStatus.GARRISONING)
                || IsHaveState(armyStatus, ArmyStatus.RALLY_WAIT))
            {
                resultPos = defaultPos;
                resultIndex = 0;
                return;
            }

            if (movePath.Count == 0) //return Vector2.zero;
            {
                resultPos = defaultPos;
                resultIndex = 0;
                return;
            }
                
            //时间
            long times = arrivalTime - startTime;
            if (times <= 0)
            {
                //容错处理：服务端下发相同路径点，起止时间一致。取最后一个路径点。
                resultPos = movePath[movePath.Count - 1]; 
                //resultPos = defaultPos;
                resultIndex = 0;
                return;
            }
            
            float distance = 0;
            for (int i = 0; i < movePath.Count - 1; ++i)
            {
                distance += Vector2.Distance(movePath[i], movePath[i + 1]);
            }
            //速度
            float speed = distance / times;
            // 消耗的时间
            long milli = ServerTimeModule.Instance.GetServerTimeMilli();
            float timsOver = (milli - startTime * 1000) / 1000f;
            float moved = timsOver * speed;
            Vector2 pos = Vector2.zero;
            var index = 0;
            for (int i = 0; i < movePath.Count - 1; ++i)
            {
                distance = Vector2.Distance(movePath[i], movePath[i + 1]);
                if (moved > distance)
                {
                    moved -= distance;
                    continue;
                }
                Vector2 forward = movePath[i + 1] - movePath[i];
                pos = movePath[i] + forward.normalized * moved;
                index = i;
                moved = 0;
                break;
            }
            if (moved > 0)
            {
                pos = movePath[movePath.Count - 1];
                index = movePath.Count - 1;
            }

            resultPos = pos;
            resultIndex = index;
        }

        public static bool IsHaveState(long state, ArmyStatus status)
        {
            var v = state & (long) status;
            return v == (long) status ;
        }

        public static bool IsHaveAnyState(long state, long status)
        {
            var v = state & status;
            return v != 0;
        }

        public static Troops.ENMU_SQUARE_STAT GetTroopState(long status)
        {
            long moveState = (long)(ArmyStatus.SPACE_MARCH|ArmyStatus.ATTACK_MARCH| ArmyStatus.COLLECT_MARCH | ArmyStatus.REINFORCE_MARCH | ArmyStatus.PALLY_MARCH | ArmyStatus.RETREAT_MARCH |
                          ArmyStatus.FAILED_MARCH | ArmyStatus.PATROL | ArmyStatus.DISCOVER | ArmyStatus.SCOUNT | ArmyStatus.SURVEYING | ArmyStatus.RETURN | ArmyStatus.BACK_CITY | ArmyStatus.FOLLOWUP |
                          ArmyStatus.MOVE | ArmyStatus.ADJUST | ArmyStatus.RALLY_JOIN_MARCH);

            if (IsHaveAnyState(status, moveState))
            {
                return Troops.ENMU_SQUARE_STAT.MOVE;
            }
            // 只有战斗或者战斗+驻扎或者战斗+集结战斗是属于战斗状态
            if(status == (int)ArmyStatus.BATTLEING || IsHaveState(status, (ArmyStatus.STATIONING| ArmyStatus.BATTLEING))||
                    IsHaveState(status, (ArmyStatus.ARMY_SATNBY| ArmyStatus.BATTLEING))||
                    IsHaveState(status,(ArmyStatus.RALLY_BATTLE| ArmyStatus.BATTLEING)))
            {
                return Troops.ENMU_SQUARE_STAT.FIGHT;
            }
            return Troops.ENMU_SQUARE_STAT.IDLE;
        }

        public static Troops.ENMU_SQUARE_STAT GetScoutState(ScoutProxy.ScoutState status)
        {
            switch (status)
            {
                case ScoutProxy.ScoutState.Scouting:
                case  ScoutProxy.ScoutState.Fog:
                case  ScoutProxy.ScoutState.Return:
                case  ScoutProxy.ScoutState.Back_City: 
                    return Troops.ENMU_SQUARE_STAT.MOVE;
                case ScoutProxy.ScoutState.None:
                    return Troops.ENMU_SQUARE_STAT.IDLE;
            }

            Debug.LogWarning($"GetScoutState 状态有问题:{status}");
            return Troops.ENMU_SQUARE_STAT.IDLE;
        }

        public static bool GetTroopIsFight(long status)
        {
            if (IsHaveState(status, ArmyStatus.BATTLEING) ||
                IsHaveState(status, ArmyStatus.FOLLOWUP) ||
                IsHaveState(status, ArmyStatus.MOVE) ||
                IsHaveState(status, ArmyStatus.ADJUST) ||
                IsHaveState(status, ArmyStatus.RALLY_BATTLE))
            {
                return true;
            }

            return false;
        }


        public static string GetIcon(long status)
        {
            string icon = string.Empty;
            if (IsHaveState(status, ArmyStatus.FAILED_MARCH))
            {
                icon = "ui_map[img_map_state_fail]";
            }
            else if (IsHaveState(status, ArmyStatus.BATTLEING) ||
                     IsHaveState(status, ArmyStatus.FOLLOWUP) ||
                     IsHaveState(status, ArmyStatus.MOVE) ||
                     IsHaveState(status, ArmyStatus.ADJUST) ||
                     IsHaveState(status, ArmyStatus.RALLY_BATTLE))
            {
                icon = "ui_map[img_map_state_war]";
            }
            else if (IsHaveState(status, ArmyStatus.SPACE_MARCH) ||
                     IsHaveState(status, ArmyStatus.ATTACK_MARCH) ||
                     IsHaveState(status, ArmyStatus.COLLECT_MARCH) ||
                     IsHaveState(status, ArmyStatus.REINFORCE_MARCH) ||
                     IsHaveState(status, ArmyStatus.RALLY_JOIN_MARCH))
            {
                icon = "ui_map[img_map_state_move]";
            }
            else if (IsHaveState(status, ArmyStatus.RETREAT_MARCH) ||
                     IsHaveState(status, ArmyStatus.BACK_CITY))
            {
                icon = "ui_map[img_map_state_back]";
            }
            else if (IsHaveState(status, ArmyStatus.PALLY_MARCH))
            {
                icon = "ui_map[img_map_state_massmove]";
            }
            else if (IsHaveState(status, ArmyStatus.COLLECTING) ||
                     IsHaveState(status, ArmyStatus.COLLECTING_NO_DELETE))

            {
                icon = "ui_map[img_map_state_collect1]";
            }
            else if (IsHaveState(status, ArmyStatus.STATIONING) ||
                     IsHaveState(status, ArmyStatus.ARMY_SATNBY))
            {
                icon = "ui_map[img_map_state_stop1]";
            }
            else if (IsHaveState(status, ArmyStatus.DISCOVER))
            {
                icon = "ui_map[img_map_state_explore]";
            }
            else if (IsHaveState(status, ArmyStatus.SCOUNT) || IsHaveState(status, ArmyStatus.SURVEYING))

        {
                icon = "ui_map[img_map_state_scout]";
            }else if (IsHaveState(status, ArmyStatus.RETURN))
            {
                icon = "ui_map[img_map_state_back]";
            }else if (IsHaveState(status, ArmyStatus.STANBY))
            {
                icon = "ui_map[img_map_state_rest]";
            }else if (IsHaveState(status, ArmyStatus.RALLY_WAIT)||
                      IsHaveState(status, ArmyStatus.GARRISONING))
            {
                icon = "ui_map[img_map_state_massing]";
            }
            else
            {
                Debug.LogWarning($"状态没有合适的图标：{status}");
            }
            return icon;
        }
        
        // 需要向空地行军的类型
        public static bool IsTouchMoveAllianceBuilding(RssType rssType)
        {
            switch (rssType)
            {
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
  
                    return true;  
            }

            return false;
        }

        public static bool IsTouchGuildRss(RssType rssType)
        {
            switch (rssType)
            {   
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                case RssType.GuildFoodResCenter:
                    return true;                   
            }

            return false;
        }

        //可直接攻击的。不分盟友或者采集的
        public static bool IsAttackBuildings(RssType type)
        {
            switch (type)
            {
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                case RssType.City:
                case RssType.BarbarianCitadel:
                case RssType.HolyLand:
                case RssType.CheckPoint:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:  
                    return true;  
            } 

            return false;
        }

        public static bool IsAttackBuilding(RssType type)
        {
            switch (type)
            {
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Wood:
                case RssType.Gold:
                case RssType.Gem:
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                case RssType.City:
                case RssType.BarbarianCitadel:
                case RssType.HolyLand:
                case RssType.CheckPoint:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:  
                    return true;  
            } 

            return false;
        }


        public static bool IsNoTouchType(RssType type)
        {
            switch (type)
            {
                case RssType.Village:
                case RssType.Cave:
                    return true;                   
            }
            
            return false;
        }

        public static bool IsTroopType(RssType type)
        {
            switch (type)
            {
                case RssType.Troop:
                case RssType.Scouts:
                case RssType.Transport:
                    return true;
            }           
            return false;
        }


        public  static bool GetIsGuildType(RssType type)
        {
            switch (type)
            {
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                    return true;
            }

            return false;
        }
        

        public static bool IsAttackGuildType(RssType type)
        {
            switch (type)
            {
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                    return true;  
            }

            return false;
        }


        public static bool IsStrongHoldType(RssType type)
        {
            switch (type)
            {
                case RssType.CheckPoint:
                case RssType.HolyLand:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:
                    return true;
            }

            return false;
        }

        public static bool IsCollectType(RssType type)
        {
            if (IsCollectGuildType(type))
            {
                return true;
            }

            switch (type)
            {
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Wood:
                case RssType.Gold:
                case RssType.Gem:
                case RssType.Rune:
                    return true;
            }

            return false;
        }


        public static bool IsCollectGuildType(RssType type)
        {
            switch (type)
            {
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                    return true;    
            }

            return false;
        }


        public static bool IsPlayByState(RssType type)
        {
            switch (type)
            {
                case RssType.City:
                case RssType.Stone:
                case RssType.Farmland:
                case RssType.Wood:
                case RssType.Gold:
                case RssType.Gem:
                case RssType.Scouts:
                case RssType.Village:
                case RssType.Cave:
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                case RssType.Rune:
                    return true;                  
            }

            return false;
        }


        public static string[] GetOnClickTargetType(RssType type)
        {
            string[] str;
            switch (type)
            {
                case RssType.City:
                    str = new string[] { "TownCenter" , "Farm", "Sawmill", "Quarry", "SilverMine", "CityWall", "BuilderHut", "GuardTower", "Barracks", "Stable", "ArcheryRange", "SiegeWorkshop", "Academy", "Hospital", "Storage", "AllianceCenter", "Castel", "Tavern", "TradingPost", "Market", "CourierStation", "ScoutCamp", "BulletinBoard", "Monument", "Smithy", "Road", "tree", "tree2", "tree3", "tree4" };
                    break;
                case RssType.Monster:
                    str = new string[] { "BarbarianFormation" , "BarbarianWalled", "GuardianFormation", "SummonBarbarianFormation" };
                    break;
                case RssType.Troop:
                    str = new string[] { "Formation" };
                    break;
                case RssType.Stone:
                     str = new string[] { "RssItem" };
                    break;
                case RssType.GuildCenter:
                    str = new string[] { "GuildBuild", "CheckPoint", "HolyLand" };
                    break;
                default:str = new string[] { string.Empty };break;
            }

            return str;
        }

        public static int GetTroopObjectIdByColliderName(string colliderName)
        {
            string[] strP = colliderName.Split('_');
            if (strP.Length != 2) return 0 ;
            if (strP[0] != "Formation") return 0;
            int objectId = 0;
            int.TryParse(strP[1], out objectId);
            return objectId;
        }

        public static void ShowDistanceCheckPanel(Action callBack, string str)
        {
            string des = LanguageUtils.getTextFormat(730269, str);                     
            Alert.CreateAlert(des, LanguageUtils.getText(610021)).SetLeftButton().SetRightButton(() =>
            {
                if (callBack != null)
                {
                    callBack.Invoke();
                }
            }).Show();
        }

        public static void ShowIsAttackOtherTroop(Action callBack, Action<bool> isOnCallBack,TroopBehavior troopBehavior= TroopBehavior.None)
        {             
            int languageId = 610019;
            string des =string.Empty;
            bool isHasBuff = false;
            CityBuffProxy cityBuffProxy=  AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            if (cityBuffProxy != null)
            {
                des= cityBuffProxy.GetBattleBuffTime();
                if (cityBuffProxy.HasType2Buff())
                {
                    isHasBuff = true;
                    switch (troopBehavior)
                    {
                        case TroopBehavior.SpyOn:
                            languageId = 181192;
                            break;
                        case TroopBehavior.Attack:
                            languageId = 181266;
                            break;
                        case TroopBehavior.Play:
                            languageId = 181267;
                            break;
                    }
                }
            }
          
            string str =string.Empty;
            if (isHasBuff)
            {
                str = LanguageUtils.getTextFormat(languageId, des);
            }
            else
            {
                str = LanguageUtils.getTextFormat(languageId, "3%", des);
            }
      
            Alert.CreateAlert(str, LanguageUtils.getText(610021))
                .SetLeftButton()
                .SetRightButton(() =>
                {
                    if (callBack != null)
                    {
                        callBack.Invoke();
                    }
                }).SetAlertToggle((isOn) =>
                {
                    if (isOnCallBack != null)
                    {
                        isOnCallBack.Invoke(isOn);
                    }
                }).Show();
        }

        public static void ShowCheckCityBuffPanel(Action callBack)
        {
            Alert.CreateAlert(LanguageUtils.getText(181188), LanguageUtils.getText(610021)).SetLeftButton()
                .SetRightButton(
                    () =>
                    {
                        if (callBack != null)
                        {
                            callBack.Invoke();
                        }
                    }).Show();
        }


        public static void SetShowAttackOtherTroopPlayerPrefs(string key, int value)
        {
            PlayerPrefs.SetInt(key,value);
            PlayerPrefs.Save();
        }

        public static bool IsShowAttackOtherTroopView(string key)
        {
            bool isShow = false;
            int times = PlayerPrefs.GetInt(key);
            if (times > 0)
            {
                DateTime time1 = ServerTimeModule.Instance.GetCurrServerDateTime();
                DateTime time2 = ServerTimeModule.Instance.ConverToServerDateTime(times);
                if (time1.Month != time2.Month || time1.Day != time2.Day)
                {
                    PlayerPrefs.DeleteKey(key);
                    isShow = true;
                }
            }
            else
            {
                isShow = true;
            }
            return isShow;
        }


        public static bool SetStringArray(string key, char separator, params string[] stringArray)
        {
            if (stringArray.Length == 0) return false;
            try
            { PlayerPrefs.SetString(key, String.Join(separator.ToString(), stringArray)); }
            catch (Exception e)
            { return false; }
            return true;
        }
        
        public static bool SetStringArray(string key, params string[] stringArray)
        {
            if (!SetStringArray(key, "\n"[0], stringArray))
                return false;
            return true;
        }
        
        public static string[] GetStringArray(string key, char separator)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetString(key).Split(separator);
            return new string[0];
        }
        
        public static string[] GetStringArray(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetString(key).Split("\n"[0]);
            return new string[0];
        }

        public static Vector3 GetSelectEffectScale(TouchTargetEfeectObjectType targetType, float targetRadis = 0)
        {
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            Vector3 scale2= Vector3.one;
            if (config != null)
            {
                float scale = 1;
                switch(targetType)
                {
                    case TouchTargetEfeectObjectType.City:
                        scale = config.cityRadius / config.mapTargetEffectScale;
                        break;
                    case TouchTargetEfeectObjectType.SelfTroop:
                    case TouchTargetEfeectObjectType.Monster:                      
                    case TouchTargetEfeectObjectType.OtherPlayerTroop:
                    case TouchTargetEfeectObjectType.Rune:
                    case TouchTargetEfeectObjectType.AllianceBuilding:
                    case TouchTargetEfeectObjectType.CheckPoint :
                        scale = targetRadis / config.mapTargetEffectScale;
                        break;                   
                    case TouchTargetEfeectObjectType.Resource:
                        scale = config.resourceGatherRadius / config.mapTargetEffectScale;
                        break;                 
                }
                 scale2 = new Vector3(scale, scale, scale);
            }            
            return scale2;
        }

        public static string GetTroopHudIcon(TroopHudIconType iconType)
        {
            string iconName = string.Empty;
            switch (iconType)
            {
                case TroopHudIconType.Retreat:
                    iconName = "ui_hud[btn_hud_1002]";
                    break;
                case TroopHudIconType.Stationed:
                    iconName = "ui_hud[btn_hud_1037]";
                    break;
                case TroopHudIconType.Attack:
                    iconName = "ui_hud[btn_hud_1001]";
                    break;
                case TroopHudIconType.Investigation:
                    iconName = "ui_hud[btn_hud_1041]";
                    break;
            }
            return iconName;
        }

        public static bool IsShowCityBuffCheck(int objectId)
        {
            var m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            MapObjectInfoEntity mapObject = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(objectId);
            if (mapObject == null)
            {
                return false;
            }

            switch (mapObject.rssType)
            {
               case  RssType.BarbarianCitadel:
               case  RssType.Monster:
               case  RssType.Guardian:
               case  RssType.SummonAttackMonster:
               case  RssType.SummonConcentrateMonster:
                    return true;
                case RssType.CheckPoint:
                case RssType.Checkpoint_1:
                case RssType.Checkpoint_2:
                case RssType.Checkpoint_3:
                case RssType.HolyLand:
                case RssType.Sanctuary:
                case RssType.Altar:
                case RssType.Shrine:
                case RssType.LostTemple:
                    return mapObject.guildId == 0;
            }
            return false;
        }



        public static bool IsTroopMoving(ArmyInfoEntity info)
        {
            if (IsHaveState(info.status, ArmyStatus.SPACE_MARCH) ||
                IsHaveState(info.status, ArmyStatus.ATTACK_MARCH) ||
                IsHaveState(info.status, ArmyStatus.COLLECT_MARCH) ||
                IsHaveState(info.status, ArmyStatus.REINFORCE_MARCH) ||
                IsHaveState(info.status, ArmyStatus.RETREAT_MARCH) ||
                IsHaveState(info.status, ArmyStatus.FAILED_MARCH) ||
                IsHaveState(info.status, ArmyStatus.DISCOVER) ||
                IsHaveState(info.status, ArmyStatus.SCOUNT) ||
                IsHaveState(info.status, ArmyStatus.RETURN))
            {
                return true;
            }
            
            return false;
        }

        public static void OnClickAddActionPoint()
        {
            BagProxy  m_bagProxy=AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            int count = 0;
            var itemCfgs = CoreUtils.dataService.QueryRecords<ItemDefine>();
            foreach (var itemCfg in itemCfgs)
            {
                if (itemCfg.subType == 50208 &&
                    m_bagProxy.GetItemNum(itemCfg.ID) >0)
                {
                    count++;
                }               
            }
            if (count == 0)
            {
                CoreUtils.uiManager.ShowUI(UI.s_exchageActionPoint);
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_useItem, null, new UseItemViewData()
                {
                    ItemType = UseItemType.ActionPoint
                });
            }
        }

        public static bool GetRssPos(int rssId, out Vector3 rssPos)
        {
            rssPos = Vector3.zero;

            Troops troop = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(rssId);
            if (troop != null)
            {
                rssPos = new Vector3(troop.transform.position.x, 0, troop.transform.position.z);
                return true;
            }

            Troops barbarian = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetFormationBarbarian(rssId);
            if (barbarian != null)
            {
                rssPos = new Vector3(barbarian.transform.position.x, 0, barbarian.transform.position.z);
                return true;
            }

            var m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            MapObjectInfoEntity mapObject = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(rssId);
            if (mapObject != null)
            {
                if (mapObject.gameobject != null)
                {
                    rssPos = new Vector3(mapObject.gameobject.transform.position.x, 0, mapObject.gameobject.transform.position.z);
                }
                else
                {
                    rssPos = PosHelper.ServerUnitToClientUnit(mapObject.objectPos);
                }
                return true;
            }

            return false;
        }

        public static float GetRssRadius(int rssId)
        {
            float radius = 0;

            var m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            MapObjectInfoEntity infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(rssId);
            if (infoEntity != null)
            {
                switch (infoEntity.rssType)
                {
                    case RssType.City:
                        ConfigDefine configDefineToC = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                        if (configDefineToC != null)
                        {
                            radius = configDefineToC.cityRadius;
                        }
                        break;
                    case RssType.Monster:
                    case RssType.Guardian:
                    case RssType.SummonAttackMonster:
                    case RssType.SummonConcentrateMonster:
                        if (infoEntity.monsterDefine != null)
                        {
                            radius = infoEntity.armyRadius / 100.0f;
                        }

                        break;
                    case RssType.Troop:
                        ArmyData troopArmyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)infoEntity.objectId);
                        if (troopArmyData != null)
                        {
                            radius = troopArmyData.armyRadius;
                        }
                        break;
                    case RssType.Stone:
                    case RssType.Farmland:
                    case RssType.Wood:
                    case RssType.Gold:
                    case RssType.Gem:
                    case RssType.GuildFoodResCenter:
                    case RssType.GuildWoodResCenter:
                    case RssType.GuildGoldResCenter:
                    case RssType.GuildGemResCenter:
                        ConfigDefine configDefineToR = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                        if (configDefineToR != null)
                        {
                            radius = configDefineToR.resourceGatherRadiusCollide;
                        }                        
                        break;
                    case RssType.GuildCenter:
                    case RssType.GuildFortress1:
                    case RssType.GuildFortress2:
                    case RssType.GuildFlag:
                        var m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                        int type = m_allianceProxy.GetBuildServerTypeToConfigType(infoEntity.objectType);
                        if (type != 0)
                        {
                            AllianceBuildingTypeDefine typeDefine = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);
                            if (typeDefine != null)
                            {
                                radius = typeDefine.radius;
                            }
                        }
                        break;
                    case RssType.CheckPoint:
                    case RssType.Checkpoint_1:
                    case RssType.Checkpoint_2:
                    case RssType.Checkpoint_3:
                    case RssType.HolyLand:
                    case RssType.Sanctuary:
                    case RssType.Altar:
                    case RssType.Shrine:
                    case RssType.LostTemple:
                        StrongHoldDataDefine dataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)infoEntity.strongHoldId);
                        if (dataDefine != null)
                        {
                            StrongHoldTypeDefine typeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(dataDefine.type);
                            if (typeDefine != null)
                            {
                                radius = typeDefine.radius;
                            }
                        }
                        break;
                }
            }

            return radius;
        }

        public static float GetArmyRadius(Dictionary<int, int> soldiersDic, bool isRallyArmy = false)
        {
            float armyRadius = 0;

            // 计算兵种数量
            Dictionary<int, int> soldiersInfoDic = new Dictionary<int, int>();
            foreach (var soldiers in soldiersDic)
            {
                if (soldiers.Value <= 0)
                {
                    continue;
                }

                ArmsDefine armsDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(soldiers.Key);
                if (armsDefine != null)
                {
                    if (!soldiersInfoDic.ContainsKey(armsDefine.armsType))
                    {
                        soldiersInfoDic[armsDefine.armsType] = 0;                
                    }
                    soldiersInfoDic[armsDefine.armsType] = soldiersInfoDic[armsDefine.armsType] + soldiers.Value;
                }                
            }            

            // 将领高度
            SquareSpacingDefine squareSpacingDefine;
            squareSpacingDefine = CoreUtils.dataService.QueryRecord<SquareSpacingDefine>(10000);
            if (squareSpacingDefine != null)
            {
                armyRadius = armyRadius + squareSpacingDefine.spacing;
            }
            squareSpacingDefine = CoreUtils.dataService.QueryRecord<SquareSpacingDefine>(20000);
            if (squareSpacingDefine != null)
            {
                armyRadius = armyRadius + squareSpacingDefine.spacing;
            }
            
            // 兵种高度
            foreach (var soldiersType in soldiersInfoDic.Keys)
            {
                squareSpacingDefine = CoreUtils.dataService.QueryRecord<SquareSpacingDefine>(10000 + soldiersType);
                if (squareSpacingDefine != null)
                {
                    armyRadius = armyRadius + squareSpacingDefine.spacing;
                }
            }
            foreach (var soldiersType in soldiersInfoDic.Keys)
            {
                squareSpacingDefine = CoreUtils.dataService.QueryRecord<SquareSpacingDefine>(20000 + soldiersType);
                if (squareSpacingDefine != null)
                {
                    armyRadius = armyRadius + squareSpacingDefine.spacing;
                }
            }

            // 最大兵种高度
            int soldierHeight = 4;
            if (isRallyArmy)
            {
                soldierHeight = 7;
            }

            // 兵种额外高度
            if (soldiersInfoDic.Count < soldierHeight)
            {
                List<KeyValuePair<int, int>> soldiersInfoList = new List<KeyValuePair<int, int>>();
                foreach (var soldiersInfoInD in soldiersInfoDic)
                {
                    int insertIndex = 0;
                    if (soldiersInfoList.Count > 0)
                    {
                        foreach (var soldiersInfoInL in soldiersInfoList)
                        {
                            insertIndex = soldiersInfoList.IndexOf(soldiersInfoInL);

                            if (soldiersInfoInD.Value > soldiersInfoInL.Value)
                            {
                                break;
                            }
                            else
                            {
                                insertIndex = insertIndex + 1;
                            }
                        }
                    }
                    soldiersInfoList.Insert(insertIndex, new KeyValuePair<int, int>(soldiersInfoInD.Key, soldiersInfoInD.Value));
                }

                // 小于 soldierHeight 种兵种, 计算额外高度, 补足到 soldierHeight 行
                List<SquareNumberBySumDefine> squareNumberBySumDefineList = CoreUtils.dataService.QueryRecords<SquareNumberBySumDefine>();
                int leftShowRow = soldierHeight - soldiersInfoList.Count;
                while (leftShowRow > 0 && soldiersInfoList.Count > 0)
                {
                    // 获取兵种显示数量
                    int squareNumber = 0;
                    int squareType = 0;

                    foreach (var squareNumberBySumDefine in squareNumberBySumDefineList)
                    {
                        if (squareNumberBySumDefine.type == soldiersInfoList[0].Key &&
                            squareNumberBySumDefine.rangeMin <= soldiersInfoList[0].Value &&
                            squareNumberBySumDefine.rangeMax >= soldiersInfoList[0].Value)
                        {
                            squareNumber = squareNumberBySumDefine.num;
                            squareType = squareNumberBySumDefine.type;
                            break;
                        }
                    }

                    if (squareNumber > 0)
                    {
                        // 判断是否超过一行的显示                        
                        SquareMaxNumberDefine squareMaxNumberDefine = CoreUtils.dataService.QueryRecord<SquareMaxNumberDefine>(100 + squareType);
                        if (squareNumber > squareMaxNumberDefine.num)
                        {
                            int showRow = (int)Mathf.Ceil(squareNumber * 1.0f / squareMaxNumberDefine.num) - 1;
                            
                            squareSpacingDefine = CoreUtils.dataService.QueryRecord<SquareSpacingDefine>(10000 + squareType);
                            if (squareSpacingDefine != null)
                            {
                                armyRadius = armyRadius + squareSpacingDefine.spacing * showRow;
                            }
                            squareSpacingDefine = CoreUtils.dataService.QueryRecord<SquareSpacingDefine>(20000 + squareType);
                            if (squareSpacingDefine != null)
                            {
                                armyRadius = armyRadius + squareSpacingDefine.spacing * showRow;
                            }

                            leftShowRow = leftShowRow - showRow;
                        }
                        soldiersInfoList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return armyRadius / 2.0f;
        }

        public static void UpdateTroopSoldiers(int id, Dictionary<Int64, BattleRemainSoldiers> battleRemainSoldiers)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null && armyData.soldiers != null)
            {
                foreach (var info in armyData.soldiers.Values)
                {
                    info.num = 0;
                    info.minor = 0;
                }

                foreach (var remainSoldiers in battleRemainSoldiers.Values)
                {
                    foreach (var info in remainSoldiers.remainSoldier.Values)
                    {
                        if (armyData.soldiers.ContainsKey(info.id))
                        {
                            armyData.soldiers[info.id].num += info.num;
                            armyData.soldiers[info.id].minor += info.minor;
                        }
                    }
                }

                Troops.ENMU_MATRIX_TYPE type = Troops.ENMU_MATRIX_TYPE.COMMON;
                if (armyData.isRally)
                {
                    type = Troops.ENMU_MATRIX_TYPE.RALLY;
                }                
                armyData.des = SquareHelper.Instance.GetMapCreateTroopDes(armyData.heroId, armyData.viceId, armyData.soldiers, type);
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().SetFormationInfo(id, armyData.des);
            }
        }
    }
}