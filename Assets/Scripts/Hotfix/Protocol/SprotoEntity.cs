using UnityEngine;
using System.Collections.Generic;
using System;
using SprotoType;
namespace Game
{
	public partial class ScheduleInfoEntity
	{
		public const string ScheduleInfoChange = "ScheduleInfoChange";
		public System.Int64 type;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.ScheduleInfo.DataInfo> data;
		public System.Int64 lastLoginTime;
		public System.Int64 times;

		public static HashSet<string> updateEntity(ScheduleInfoEntity et ,SprotoType.Activity.ScheduleInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasData){

				if (et.data == null) {
					 et.data = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.ScheduleInfo.DataInfo>();
				}
				foreach(var item in data.data){ 
					if(et.data.ContainsKey(item.Key)){
						et.data[item.Key] = item.Value;
					}else{
						et.data.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("data"); 
			}
			if(data.HasLastLoginTime){
				et.lastLoginTime = data.lastLoginTime;
				if(ret)ET.ATTR.Add("lastLoginTime");
			}
			if(data.HasTimes){
				et.times = data.times;
				if(ret)ET.ATTR.Add("times");
			}
			return ET.ATTR;
		}
	}
	public partial class ExchangeEntity
	{
		public const string ExchangeChange = "ExchangeChange";
		public System.Int64 id;
		public System.Int64 count;

		public static HashSet<string> updateEntity(ExchangeEntity et ,SprotoType.Activity.Exchange data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			return ET.ATTR;
		}
	}
	public partial class RewardsEntity
	{
		public const string RewardsChange = "RewardsChange";
		public System.Int64 index;

		public static HashSet<string> updateEntity(RewardsEntity et ,SprotoType.Activity.Rewards data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			return ET.ATTR;
		}
	}
	public partial class RanksEntity
	{
		public const string RanksChange = "RanksChange";
		public System.Int64 index;
		public System.Int64 rank;
		public System.Int64 score;

		public static HashSet<string> updateEntity(RanksEntity et ,SprotoType.Activity.Ranks data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasRank){
				et.rank = data.rank;
				if(ret)ET.ATTR.Add("rank");
			}
			if(data.HasScore){
				et.score = data.score;
				if(ret)ET.ATTR.Add("score");
			}
			return ET.ATTR;
		}
	}
	public partial class SoldierInfoEntity
	{
		public const string SoldierInfoChange = "SoldierInfoChange";
		public System.Int64 id;
		public System.Int64 type;
		public System.Int64 level;
		public System.Int64 num;
		public System.Int64 minor;

		public static HashSet<string> updateEntity(SoldierInfoEntity et ,SprotoType.SoldierInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			if(data.HasMinor){
				et.minor = data.minor;
				if(ret)ET.ATTR.Add("minor");
			}
			return ET.ATTR;
		}
	}
	public partial class ResourceCollectInfoEntity
	{
		public const string ResourceCollectInfoChange = "ResourceCollectInfoChange";
		public System.Int64 resourceTypeId;
		public SprotoType.PosInfo pos;
		public System.Int64 load;
		public System.Int64 resourceId;
		public System.Int64 resourceSum;
		public System.Int64 collectNum;
		public System.Int64 startTime;
		public System.Int64 endTime;
		public System.Int64 collectSpeed;
		public System.Int64 lastSpeedChangeTime;
		public System.Collections.Generic.List<SprotoType.CollectSpeedInfo> collectSpeeds;
		public System.Int64 guildBuildType;

		public static HashSet<string> updateEntity(ResourceCollectInfoEntity et ,SprotoType.ResourceCollectInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasResourceTypeId){
				et.resourceTypeId = data.resourceTypeId;
				if(ret)ET.ATTR.Add("resourceTypeId");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasLoad){
				et.load = data.load;
				if(ret)ET.ATTR.Add("load");
			}
			if(data.HasResourceId){
				et.resourceId = data.resourceId;
				if(ret)ET.ATTR.Add("resourceId");
			}
			if(data.HasResourceSum){
				et.resourceSum = data.resourceSum;
				if(ret)ET.ATTR.Add("resourceSum");
			}
			if(data.HasCollectNum){
				et.collectNum = data.collectNum;
				if(ret)ET.ATTR.Add("collectNum");
			}
			if(data.HasStartTime){
				et.startTime = data.startTime;
				if(ret)ET.ATTR.Add("startTime");
			}
			if(data.HasEndTime){
				et.endTime = data.endTime;
				if(ret)ET.ATTR.Add("endTime");
			}
			if(data.HasCollectSpeed){
				et.collectSpeed = data.collectSpeed;
				if(ret)ET.ATTR.Add("collectSpeed");
			}
			if(data.HasLastSpeedChangeTime){
				et.lastSpeedChangeTime = data.lastSpeedChangeTime;
				if(ret)ET.ATTR.Add("lastSpeedChangeTime");
			}
			if(data.HasCollectSpeeds){
				et.collectSpeeds = data.collectSpeeds;
				if(ret)ET.ATTR.Add("collectSpeeds");
			}
			if(data.HasGuildBuildType){
				et.guildBuildType = data.guildBuildType;
				if(ret)ET.ATTR.Add("guildBuildType");
			}
			return ET.ATTR;
		}
	}
	public partial class PosInfoEntity
	{
		public const string PosInfoChange = "PosInfoChange";
		public System.Int64 x;
		public System.Int64 y;

		public static HashSet<string> updateEntity(PosInfoEntity et ,SprotoType.PosInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasX){
				et.x = data.x;
				if(ret)ET.ATTR.Add("x");
			}
			if(data.HasY){
				et.y = data.y;
				if(ret)ET.ATTR.Add("y");
			}
			return ET.ATTR;
		}
	}
	public partial class MarchTargetArgEntity
	{
		public const string MarchTargetArgChange = "MarchTargetArgChange";
		public SprotoType.PosInfo pos;
		public System.Int64 targetObjectIndex;
		public System.String targetName;
		public System.String targetGuildName;
		public System.Int64 targetHolyLandId;
		public System.Int64 targetObjectType;
		public System.Int64 targetMonsterCityId;
		public SprotoType.PosInfo targetPos;
		public System.Int64 oldTargetObjectIndex;
		public System.Int64 targetResourceId;
		public System.Int64 targetGuildBuildType;
		public System.Int64 targetMapItemType;
		public System.Int64 targetMonsterId;

		public static HashSet<string> updateEntity(MarchTargetArgEntity et ,SprotoType.MarchTargetArg data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasTargetObjectIndex){
				et.targetObjectIndex = data.targetObjectIndex;
				if(ret)ET.ATTR.Add("targetObjectIndex");
			}
			if(data.HasTargetName){
				et.targetName = data.targetName;
				if(ret)ET.ATTR.Add("targetName");
			}
			if(data.HasTargetGuildName){
				et.targetGuildName = data.targetGuildName;
				if(ret)ET.ATTR.Add("targetGuildName");
			}
			if(data.HasTargetHolyLandId){
				et.targetHolyLandId = data.targetHolyLandId;
				if(ret)ET.ATTR.Add("targetHolyLandId");
			}
			if(data.HasTargetObjectType){
				et.targetObjectType = data.targetObjectType;
				if(ret)ET.ATTR.Add("targetObjectType");
			}
			if(data.HasTargetMonsterCityId){
				et.targetMonsterCityId = data.targetMonsterCityId;
				if(ret)ET.ATTR.Add("targetMonsterCityId");
			}
			if(data.HasTargetPos){
				et.targetPos = data.targetPos;
				if(ret)ET.ATTR.Add("targetPos");
			}
			if(data.HasOldTargetObjectIndex){
				et.oldTargetObjectIndex = data.oldTargetObjectIndex;
				if(ret)ET.ATTR.Add("oldTargetObjectIndex");
			}
			if(data.HasTargetResourceId){
				et.targetResourceId = data.targetResourceId;
				if(ret)ET.ATTR.Add("targetResourceId");
			}
			if(data.HasTargetGuildBuildType){
				et.targetGuildBuildType = data.targetGuildBuildType;
				if(ret)ET.ATTR.Add("targetGuildBuildType");
			}
			if(data.HasTargetMapItemType){
				et.targetMapItemType = data.targetMapItemType;
				if(ret)ET.ATTR.Add("targetMapItemType");
			}
			if(data.HasTargetMonsterId){
				et.targetMonsterId = data.targetMonsterId;
				if(ret)ET.ATTR.Add("targetMonsterId");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleRemainSoldiersEntity
	{
		public const string BattleRemainSoldiersChange = "BattleRemainSoldiersChange";
		public System.Int64 rid;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> remainSoldier;

		public static HashSet<string> updateEntity(BattleRemainSoldiersEntity et ,SprotoType.BattleRemainSoldiers data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasRemainSoldier){

				if (et.remainSoldier == null) {
					 et.remainSoldier = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.remainSoldier){ 
					if(et.remainSoldier.ContainsKey(item.Key)){
						et.remainSoldier[item.Key] = item.Value;
					}else{
						et.remainSoldier.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("remainSoldier"); 
			}
			return ET.ATTR;
		}
	}
	public partial class SkillDamageHealEntity
	{
		public const string SkillDamageHealChange = "SkillDamageHealChange";
		public System.Int64 skillId;
		public System.Int64 skillLevel;
		public System.Int64 skillDamage;
		public System.Int64 skillHeal;
		public System.Int64 heroId;
		public System.Int64 objectIndex;

		public static HashSet<string> updateEntity(SkillDamageHealEntity et ,SprotoType.SkillDamageHeal data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasSkillId){
				et.skillId = data.skillId;
				if(ret)ET.ATTR.Add("skillId");
			}
			if(data.HasSkillLevel){
				et.skillLevel = data.skillLevel;
				if(ret)ET.ATTR.Add("skillLevel");
			}
			if(data.HasSkillDamage){
				et.skillDamage = data.skillDamage;
				if(ret)ET.ATTR.Add("skillDamage");
			}
			if(data.HasSkillHeal){
				et.skillHeal = data.skillHeal;
				if(ret)ET.ATTR.Add("skillHeal");
			}
			if(data.HasHeroId){
				et.heroId = data.heroId;
				if(ret)ET.ATTR.Add("heroId");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleRallySoldierHurtDetailEntity
	{
		public const string BattleRallySoldierHurtDetailChange = "BattleRallySoldierHurtDetailChange";
		public System.Int64 armyIndex;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroLevel;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleSoldierHurt> rallySoldierDetail;
		public System.Int64 joinTime;

		public static HashSet<string> updateEntity(BattleRallySoldierHurtDetailEntity et ,SprotoType.BattleRallySoldierHurtDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasRallySoldierDetail){

				if (et.rallySoldierDetail == null) {
					 et.rallySoldierDetail = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleSoldierHurt>();
				}
				foreach(var item in data.rallySoldierDetail){ 
					if(et.rallySoldierDetail.ContainsKey(item.Key)){
						et.rallySoldierDetail[item.Key] = item.Value;
					}else{
						et.rallySoldierDetail.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("rallySoldierDetail"); 
			}
			if(data.HasJoinTime){
				et.joinTime = data.joinTime;
				if(ret)ET.ATTR.Add("joinTime");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleSoldierHurtEntity
	{
		public const string BattleSoldierHurtChange = "BattleSoldierHurtChange";
		public System.Int64 soldierId;
		public System.Int64 minor;
		public System.Int64 hardHurt;
		public System.Int64 die;
		public System.Int64 remain;
		public System.Int64 heal;

		public static HashSet<string> updateEntity(BattleSoldierHurtEntity et ,SprotoType.BattleSoldierHurt data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasSoldierId){
				et.soldierId = data.soldierId;
				if(ret)ET.ATTR.Add("soldierId");
			}
			if(data.HasMinor){
				et.minor = data.minor;
				if(ret)ET.ATTR.Add("minor");
			}
			if(data.HasHardHurt){
				et.hardHurt = data.hardHurt;
				if(ret)ET.ATTR.Add("hardHurt");
			}
			if(data.HasDie){
				et.die = data.die;
				if(ret)ET.ATTR.Add("die");
			}
			if(data.HasRemain){
				et.remain = data.remain;
				if(ret)ET.ATTR.Add("remain");
			}
			if(data.HasHeal){
				et.heal = data.heal;
				if(ret)ET.ATTR.Add("heal");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleReportExDetailEntity
	{
		public const string BattleReportExDetailChange = "BattleReportExDetailChange";
		public System.Int64 rid;
		public System.String name;
		public System.Int64 objectType;
		public System.Int64 headId;
		public System.Int64 beginArmyCount;
		public System.Int64 endArmyCount;
		public System.Int64 maxArmyCount;
		public SprotoType.PosInfo pos;
		public System.Int64 hurt;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroLevel;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleSoldierHurtWithObject> soldierDetail;
		public System.Int64 monsterId;
		public System.Int64 objectIndex;
		public System.Int64 guildId;
		public System.Int64 holyLandBuildMonsterId;
		public System.String guildName;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleRallySoldierHurt> battleRallySoldierHurt;

		public static HashSet<string> updateEntity(BattleReportExDetailEntity et ,SprotoType.BattleReportExDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasObjectType){
				et.objectType = data.objectType;
				if(ret)ET.ATTR.Add("objectType");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasBeginArmyCount){
				et.beginArmyCount = data.beginArmyCount;
				if(ret)ET.ATTR.Add("beginArmyCount");
			}
			if(data.HasEndArmyCount){
				et.endArmyCount = data.endArmyCount;
				if(ret)ET.ATTR.Add("endArmyCount");
			}
			if(data.HasMaxArmyCount){
				et.maxArmyCount = data.maxArmyCount;
				if(ret)ET.ATTR.Add("maxArmyCount");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasHurt){
				et.hurt = data.hurt;
				if(ret)ET.ATTR.Add("hurt");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasSoldierDetail){

				if (et.soldierDetail == null) {
					 et.soldierDetail = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleSoldierHurtWithObject>();
				}
				foreach(var item in data.soldierDetail){ 
					if(et.soldierDetail.ContainsKey(item.Key)){
						et.soldierDetail[item.Key] = item.Value;
					}else{
						et.soldierDetail.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldierDetail"); 
			}
			if(data.HasMonsterId){
				et.monsterId = data.monsterId;
				if(ret)ET.ATTR.Add("monsterId");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasHolyLandBuildMonsterId){
				et.holyLandBuildMonsterId = data.holyLandBuildMonsterId;
				if(ret)ET.ATTR.Add("holyLandBuildMonsterId");
			}
			if(data.HasGuildName){
				et.guildName = data.guildName;
				if(ret)ET.ATTR.Add("guildName");
			}
			if(data.HasBattleRallySoldierHurt){

				if (et.battleRallySoldierHurt == null) {
					 et.battleRallySoldierHurt = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleRallySoldierHurt>();
				}
				foreach(var item in data.battleRallySoldierHurt){ 
					if(et.battleRallySoldierHurt.ContainsKey(item.Key)){
						et.battleRallySoldierHurt[item.Key] = item.Value;
					}else{
						et.battleRallySoldierHurt.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("battleRallySoldierHurt"); 
			}
			return ET.ATTR;
		}
	}
	public partial class RewardInfoEntity
	{
		public const string RewardInfoChange = "RewardInfoChange";
		public System.Int64 food;
		public System.Int64 wood;
		public System.Int64 stone;
		public System.Int64 gold;
		public System.Int64 denar;
		public System.Collections.Generic.List<SprotoType.RewardItem> items;
		public System.Collections.Generic.List<SprotoType.SoldierInfo> soldiers;
		public System.Int64 groupId;
		public System.Int64 actionForce;
		public System.Collections.Generic.List<SprotoType.GuildGift> guildGifts;
		public System.Collections.Generic.List<SprotoType.Heros> heros;
		public System.Int64 expeditionCoin;
		public System.Int64 guildPoint;
		public System.Int64 vip;
		public System.Int64 leaguePoints;
		public System.Int64 activityActivePoint;

		public static HashSet<string> updateEntity(RewardInfoEntity et ,SprotoType.RewardInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasFood){
				et.food = data.food;
				if(ret)ET.ATTR.Add("food");
			}
			if(data.HasWood){
				et.wood = data.wood;
				if(ret)ET.ATTR.Add("wood");
			}
			if(data.HasStone){
				et.stone = data.stone;
				if(ret)ET.ATTR.Add("stone");
			}
			if(data.HasGold){
				et.gold = data.gold;
				if(ret)ET.ATTR.Add("gold");
			}
			if(data.HasDenar){
				et.denar = data.denar;
				if(ret)ET.ATTR.Add("denar");
			}
			if(data.HasItems){
				et.items = data.items;
				if(ret)ET.ATTR.Add("items");
			}
			if(data.HasSoldiers){
				et.soldiers = data.soldiers;
				if(ret)ET.ATTR.Add("soldiers");
			}
			if(data.HasGroupId){
				et.groupId = data.groupId;
				if(ret)ET.ATTR.Add("groupId");
			}
			if(data.HasActionForce){
				et.actionForce = data.actionForce;
				if(ret)ET.ATTR.Add("actionForce");
			}
			if(data.HasGuildGifts){
				et.guildGifts = data.guildGifts;
				if(ret)ET.ATTR.Add("guildGifts");
			}
			if(data.HasHeros){
				et.heros = data.heros;
				if(ret)ET.ATTR.Add("heros");
			}
			if(data.HasExpeditionCoin){
				et.expeditionCoin = data.expeditionCoin;
				if(ret)ET.ATTR.Add("expeditionCoin");
			}
			if(data.HasGuildPoint){
				et.guildPoint = data.guildPoint;
				if(ret)ET.ATTR.Add("guildPoint");
			}
			if(data.HasVip){
				et.vip = data.vip;
				if(ret)ET.ATTR.Add("vip");
			}
			if(data.HasLeaguePoints){
				et.leaguePoints = data.leaguePoints;
				if(ret)ET.ATTR.Add("leaguePoints");
			}
			if(data.HasActivityActivePoint){
				et.activityActivePoint = data.activityActivePoint;
				if(ret)ET.ATTR.Add("activityActivePoint");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleReinforceArmyEntity
	{
		public const string BattleReinforceArmyChange = "BattleReinforceArmyChange";
		public System.Int64 time;
		public System.Int64 headId;
		public System.Int64 headFrameID;
		public System.Int64 armyCount;
		public System.Int64 guildId;
		public System.String name;
		public System.Boolean isCityJoin;
		public System.Boolean isArmyBack;

		public static HashSet<string> updateEntity(BattleReinforceArmyEntity et ,SprotoType.BattleReinforceArmy data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTime){
				et.time = data.time;
				if(ret)ET.ATTR.Add("time");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasArmyCount){
				et.armyCount = data.armyCount;
				if(ret)ET.ATTR.Add("armyCount");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasIsCityJoin){
				et.isCityJoin = data.isCityJoin;
				if(ret)ET.ATTR.Add("isCityJoin");
			}
			if(data.HasIsArmyBack){
				et.isArmyBack = data.isArmyBack;
				if(ret)ET.ATTR.Add("isArmyBack");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleReportWithObjectIndexEntity
	{
		public const string BattleReportWithObjectIndexChange = "BattleReportWithObjectIndexChange";
		public System.Int64 targetObjectIndex;
		public System.Collections.Generic.List<SprotoType.BattleReport> battleDamageHeal;
		public System.Int64 battleBeginTime;
		public System.Int64 battleEndTime;

		public static HashSet<string> updateEntity(BattleReportWithObjectIndexEntity et ,SprotoType.BattleReportWithObjectIndex data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTargetObjectIndex){
				et.targetObjectIndex = data.targetObjectIndex;
				if(ret)ET.ATTR.Add("targetObjectIndex");
			}
			if(data.HasBattleDamageHeal){
				et.battleDamageHeal = data.battleDamageHeal;
				if(ret)ET.ATTR.Add("battleDamageHeal");
			}
			if(data.HasBattleBeginTime){
				et.battleBeginTime = data.battleBeginTime;
				if(ret)ET.ATTR.Add("battleBeginTime");
			}
			if(data.HasBattleEndTime){
				et.battleEndTime = data.battleEndTime;
				if(ret)ET.ATTR.Add("battleEndTime");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleSoldierHurtWithObjectEntity
	{
		public const string BattleSoldierHurtWithObjectChange = "BattleSoldierHurtWithObjectChange";
		public System.Int64 targetObjectIndex;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleSoldierHurt> battleSoldierHurt;

		public static HashSet<string> updateEntity(BattleSoldierHurtWithObjectEntity et ,SprotoType.BattleSoldierHurtWithObject data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTargetObjectIndex){
				et.targetObjectIndex = data.targetObjectIndex;
				if(ret)ET.ATTR.Add("targetObjectIndex");
			}
			if(data.HasBattleSoldierHurt){

				if (et.battleSoldierHurt == null) {
					 et.battleSoldierHurt = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleSoldierHurt>();
				}
				foreach(var item in data.battleSoldierHurt){ 
					if(et.battleSoldierHurt.ContainsKey(item.Key)){
						et.battleSoldierHurt[item.Key] = item.Value;
					}else{
						et.battleSoldierHurt.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("battleSoldierHurt"); 
			}
			return ET.ATTR;
		}
	}
	public partial class BattleRallySoldierHurtEntity
	{
		public const string BattleRallySoldierHurtChange = "BattleRallySoldierHurtChange";
		public System.Int64 rallyRid;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleRallySoldierHurtDetail> rallyHurt;
		public System.String rallyRoleName;
		public System.Boolean isLeader;

		public static HashSet<string> updateEntity(BattleRallySoldierHurtEntity et ,SprotoType.BattleRallySoldierHurt data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRallyRid){
				et.rallyRid = data.rallyRid;
				if(ret)ET.ATTR.Add("rallyRid");
			}
			if(data.HasRallyHurt){

				if (et.rallyHurt == null) {
					 et.rallyHurt = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleRallySoldierHurtDetail>();
				}
				foreach(var item in data.rallyHurt){ 
					if(et.rallyHurt.ContainsKey(item.Key)){
						et.rallyHurt[item.Key] = item.Value;
					}else{
						et.rallyHurt.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("rallyHurt"); 
			}
			if(data.HasRallyRoleName){
				et.rallyRoleName = data.rallyRoleName;
				if(ret)ET.ATTR.Add("rallyRoleName");
			}
			if(data.HasIsLeader){
				et.isLeader = data.isLeader;
				if(ret)ET.ATTR.Add("isLeader");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleReportEntity
	{
		public const string BattleReportChange = "BattleReportChange";
		public System.Int64 attackArmyCount;
		public System.Int64 defenseArmyCount;
		public System.Int64 attackIndex;
		public System.Int64 defenseIndex;
		public System.Int64 reportUniqueIndex;
		public System.Int64 damage;
		public System.Int64 beatBackDamage;
		public System.Int64 heal;
		public System.Int64 skillId;
		public System.Collections.Generic.List<System.Int64> addBuffs;
		public System.Collections.Generic.List<System.Int64> removeBuffs;
		public System.Int64 turn;

		public static HashSet<string> updateEntity(BattleReportEntity et ,SprotoType.BattleReport data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasAttackArmyCount){
				et.attackArmyCount = data.attackArmyCount;
				if(ret)ET.ATTR.Add("attackArmyCount");
			}
			if(data.HasDefenseArmyCount){
				et.defenseArmyCount = data.defenseArmyCount;
				if(ret)ET.ATTR.Add("defenseArmyCount");
			}
			if(data.HasAttackIndex){
				et.attackIndex = data.attackIndex;
				if(ret)ET.ATTR.Add("attackIndex");
			}
			if(data.HasDefenseIndex){
				et.defenseIndex = data.defenseIndex;
				if(ret)ET.ATTR.Add("defenseIndex");
			}
			if(data.HasReportUniqueIndex){
				et.reportUniqueIndex = data.reportUniqueIndex;
				if(ret)ET.ATTR.Add("reportUniqueIndex");
			}
			if(data.HasDamage){
				et.damage = data.damage;
				if(ret)ET.ATTR.Add("damage");
			}
			if(data.HasBeatBackDamage){
				et.beatBackDamage = data.beatBackDamage;
				if(ret)ET.ATTR.Add("beatBackDamage");
			}
			if(data.HasHeal){
				et.heal = data.heal;
				if(ret)ET.ATTR.Add("heal");
			}
			if(data.HasSkillId){
				et.skillId = data.skillId;
				if(ret)ET.ATTR.Add("skillId");
			}
			if(data.HasAddBuffs){
				et.addBuffs = data.addBuffs;
				if(ret)ET.ATTR.Add("addBuffs");
			}
			if(data.HasRemoveBuffs){
				et.removeBuffs = data.removeBuffs;
				if(ret)ET.ATTR.Add("removeBuffs");
			}
			if(data.HasTurn){
				et.turn = data.turn;
				if(ret)ET.ATTR.Add("turn");
			}
			return ET.ATTR;
		}
	}
	public partial class MessageRoleInfoEntity
	{
		public const string MessageRoleInfoChange = "MessageRoleInfoChange";
		public System.Int64 rid;
		public System.String name;
		public System.Int64 headId;
		public System.Int64 headFrameID;
		public System.String guildAbbName;

		public static HashSet<string> updateEntity(MessageRoleInfoEntity et ,SprotoType.MessageRoleInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasGuildAbbName){
				et.guildAbbName = data.guildAbbName;
				if(ret)ET.ATTR.Add("guildAbbName");
			}
			return ET.ATTR;
		}
	}
	public partial class BoardMessageInfoEntity
	{
		public const string BoardMessageInfoChange = "BoardMessageInfoChange";
		public System.Int64 messageIndex;
		public SprotoType.MessageRoleInfo roleInfo;
		public System.Int64 floorId;
		public System.Int64 replyMessageIndex;
		public System.Int64 sendTime;
		public System.String content;
		public System.Collections.Generic.List<SprotoType.BoardMessageInfo> subFloors;

		public static HashSet<string> updateEntity(BoardMessageInfoEntity et ,SprotoType.BoardMessageInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasMessageIndex){
				et.messageIndex = data.messageIndex;
				if(ret)ET.ATTR.Add("messageIndex");
			}
			if(data.HasRoleInfo){
				et.roleInfo = data.roleInfo;
				if(ret)ET.ATTR.Add("roleInfo");
			}
			if(data.HasFloorId){
				et.floorId = data.floorId;
				if(ret)ET.ATTR.Add("floorId");
			}
			if(data.HasReplyMessageIndex){
				et.replyMessageIndex = data.replyMessageIndex;
				if(ret)ET.ATTR.Add("replyMessageIndex");
			}
			if(data.HasSendTime){
				et.sendTime = data.sendTime;
				if(ret)ET.ATTR.Add("sendTime");
			}
			if(data.HasContent){
				et.content = data.content;
				if(ret)ET.ATTR.Add("content");
			}
			if(data.HasSubFloors){
				et.subFloors = data.subFloors;
				if(ret)ET.ATTR.Add("subFloors");
			}
			return ET.ATTR;
		}
	}
	public partial class BuildingGainInfoEntity
	{
		public const string BuildingGainInfoChange = "BuildingGainInfoChange";
		public System.Int64 type;
		public System.Int64 num;
		public System.Int64 changeTime;

		public static HashSet<string> updateEntity(BuildingGainInfoEntity et ,SprotoType.BuildingGainInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			if(data.HasChangeTime){
				et.changeTime = data.changeTime;
				if(ret)ET.ATTR.Add("changeTime");
			}
			return ET.ATTR;
		}
	}
	public partial class SkillInfoEntity
	{
		public const string SkillInfoChange = "SkillInfoChange";
		public System.Int64 skillId;
		public System.Int64 skillLevel;

		public static HashSet<string> updateEntity(SkillInfoEntity et ,SprotoType.SkillInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasSkillId){
				et.skillId = data.skillId;
				if(ret)ET.ATTR.Add("skillId");
			}
			if(data.HasSkillLevel){
				et.skillLevel = data.skillLevel;
				if(ret)ET.ATTR.Add("skillLevel");
			}
			return ET.ATTR;
		}
	}
	public partial class TransportResourceInfoEntity
	{
		public const string TransportResourceInfoChange = "TransportResourceInfoChange";
		public System.Int64 resourceTypeId;
		public System.Int64 load;

		public static HashSet<string> updateEntity(TransportResourceInfoEntity et ,SprotoType.TransportResourceInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasResourceTypeId){
				et.resourceTypeId = data.resourceTypeId;
				if(ret)ET.ATTR.Add("resourceTypeId");
			}
			if(data.HasLoad){
				et.load = data.load;
				if(ret)ET.ATTR.Add("load");
			}
			return ET.ATTR;
		}
	}
	public partial class CollectReportEntity
	{
		public const string CollectReportChange = "CollectReportChange";
		public System.Int64 resourceTypeId;
		public SprotoType.PosInfo pos;
		public System.Int64 resource;
		public System.Int64 extraResource;
		public System.Int64 type;

		public static HashSet<string> updateEntity(CollectReportEntity et ,SprotoType.CollectReport data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasResourceTypeId){
				et.resourceTypeId = data.resourceTypeId;
				if(ret)ET.ATTR.Add("resourceTypeId");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasResource){
				et.resource = data.resource;
				if(ret)ET.ATTR.Add("resource");
			}
			if(data.HasExtraResource){
				et.extraResource = data.extraResource;
				if(ret)ET.ATTR.Add("extraResource");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			return ET.ATTR;
		}
	}
	public partial class DiscoverReportInfoEntity
	{
		public const string DiscoverReportInfoChange = "DiscoverReportInfoChange";
		public SprotoType.PosInfo pos;
		public System.Int64 mapFixPointId;
		public System.Int64 strongHoldType;

		public static HashSet<string> updateEntity(DiscoverReportInfoEntity et ,SprotoType.DiscoverReportInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasMapFixPointId){
				et.mapFixPointId = data.mapFixPointId;
				if(ret)ET.ATTR.Add("mapFixPointId");
			}
			if(data.HasStrongHoldType){
				et.strongHoldType = data.strongHoldType;
				if(ret)ET.ATTR.Add("strongHoldType");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildEmailInfoEntity
	{
		public const string GuildEmailInfoChange = "GuildEmailInfoChange";
		public System.Int64 guildId;
		public System.Collections.Generic.List<System.Int64> signs;
		public System.String roleName;
		public System.Int64 roleHeadId;
		public System.Int64 roleHeadFrameId;
		public System.Collections.Generic.List<SprotoType.CurrencyInfo> buildCostGuildCurrencies;
		public System.Collections.Generic.List<SprotoType.RoleDonateInfo> roleDonates;
		public System.Collections.Generic.List<SprotoType.CurrencyInfo> transportResource;
		public System.Int64 inviteStatus;
		public System.Int64 technologyId;
		public System.String guildAbbName;
		public System.Int64 roleRid;
		public System.Int64 boardMessageIndex;
		public System.Int64 strongHoldId;
		public System.Int64 markerId;
		public System.String markerDesc;
		public SprotoType.PosInfo pos;
		public System.Collections.Generic.List<SprotoType.InactiveMembersInfo> inactiveMembers;

		public static HashSet<string> updateEntity(GuildEmailInfoEntity et ,SprotoType.GuildEmailInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasSigns){
				et.signs = data.signs;
				if(ret)ET.ATTR.Add("signs");
			}
			if(data.HasRoleName){
				et.roleName = data.roleName;
				if(ret)ET.ATTR.Add("roleName");
			}
			if(data.HasRoleHeadId){
				et.roleHeadId = data.roleHeadId;
				if(ret)ET.ATTR.Add("roleHeadId");
			}
			if(data.HasRoleHeadFrameId){
				et.roleHeadFrameId = data.roleHeadFrameId;
				if(ret)ET.ATTR.Add("roleHeadFrameId");
			}
			if(data.HasBuildCostGuildCurrencies){
				et.buildCostGuildCurrencies = data.buildCostGuildCurrencies;
				if(ret)ET.ATTR.Add("buildCostGuildCurrencies");
			}
			if(data.HasRoleDonates){
				et.roleDonates = data.roleDonates;
				if(ret)ET.ATTR.Add("roleDonates");
			}
			if(data.HasTransportResource){
				et.transportResource = data.transportResource;
				if(ret)ET.ATTR.Add("transportResource");
			}
			if(data.HasInviteStatus){
				et.inviteStatus = data.inviteStatus;
				if(ret)ET.ATTR.Add("inviteStatus");
			}
			if(data.HasTechnologyId){
				et.technologyId = data.technologyId;
				if(ret)ET.ATTR.Add("technologyId");
			}
			if(data.HasGuildAbbName){
				et.guildAbbName = data.guildAbbName;
				if(ret)ET.ATTR.Add("guildAbbName");
			}
			if(data.HasRoleRid){
				et.roleRid = data.roleRid;
				if(ret)ET.ATTR.Add("roleRid");
			}
			if(data.HasBoardMessageIndex){
				et.boardMessageIndex = data.boardMessageIndex;
				if(ret)ET.ATTR.Add("boardMessageIndex");
			}
			if(data.HasStrongHoldId){
				et.strongHoldId = data.strongHoldId;
				if(ret)ET.ATTR.Add("strongHoldId");
			}
			if(data.HasMarkerId){
				et.markerId = data.markerId;
				if(ret)ET.ATTR.Add("markerId");
			}
			if(data.HasMarkerDesc){
				et.markerDesc = data.markerDesc;
				if(ret)ET.ATTR.Add("markerDesc");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasInactiveMembers){
				et.inactiveMembers = data.inactiveMembers;
				if(ret)ET.ATTR.Add("inactiveMembers");
			}
			return ET.ATTR;
		}
	}
	public partial class SenderInfoEntity
	{
		public const string SenderInfoChange = "SenderInfoChange";
		public System.Int64 rid;
		public System.String name;
		public System.Int64 guildId;
		public System.String guildAbbr;
		public System.Int64 headId;
		public System.Int64 headFrameID;

		public static HashSet<string> updateEntity(SenderInfoEntity et ,SprotoType.SenderInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasGuildAbbr){
				et.guildAbbr = data.guildAbbr;
				if(ret)ET.ATTR.Add("guildAbbr");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			return ET.ATTR;
		}
	}
	public partial class ScoutReportInfoEntity
	{
		public const string ScoutReportInfoChange = "ScoutReportInfoChange";
		public System.Int64 targetType;
		public SprotoType.PosInfo pos;
		public System.String guildAbbName;
		public System.Int64 objectTypeId;
		public SprotoType.ScoutRoleInfo scoutRole;
		public System.Int64 cityWallDurable;
		public System.Int64 cityWallDurableLimit;
		public System.Boolean armyDisband;
		public System.Int64 robFood;
		public System.Int64 robWood;
		public System.Int64 robStone;
		public System.Int64 robGold;
		public SprotoType.DefendHeroInfo mainHero;
		public SprotoType.DefendHeroInfo deputyHero;
		public System.Int64 armySumType;
		public System.Int64 armySum;
		public System.Collections.Generic.List<SprotoType.SoldierInfo> soldiers;
		public System.Int64 reinforceArmySumType;
		public System.Collections.Generic.List<SprotoType.SoldierInfo> reinforceSoldiers;
		public SprotoType.DefendHeroInfo rallyMainHero;
		public SprotoType.DefendHeroInfo rallyDeputyHero;
		public System.Int64 rallyArmySumType;
		public System.Collections.Generic.List<SprotoType.SoldierInfo> rallySoldiers;
		public System.Int64 guardTowerLevel;
		public System.Int64 guardTowerHp;
		public System.Int64 guardTowerHpLimit;
		public System.Collections.Generic.List<System.Int64> guildFlagSigns;
		public System.Int64 robResourceType;
		public System.Int64 roleAge;

		public static HashSet<string> updateEntity(ScoutReportInfoEntity et ,SprotoType.ScoutReportInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTargetType){
				et.targetType = data.targetType;
				if(ret)ET.ATTR.Add("targetType");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasGuildAbbName){
				et.guildAbbName = data.guildAbbName;
				if(ret)ET.ATTR.Add("guildAbbName");
			}
			if(data.HasObjectTypeId){
				et.objectTypeId = data.objectTypeId;
				if(ret)ET.ATTR.Add("objectTypeId");
			}
			if(data.HasScoutRole){
				et.scoutRole = data.scoutRole;
				if(ret)ET.ATTR.Add("scoutRole");
			}
			if(data.HasCityWallDurable){
				et.cityWallDurable = data.cityWallDurable;
				if(ret)ET.ATTR.Add("cityWallDurable");
			}
			if(data.HasCityWallDurableLimit){
				et.cityWallDurableLimit = data.cityWallDurableLimit;
				if(ret)ET.ATTR.Add("cityWallDurableLimit");
			}
			if(data.HasArmyDisband){
				et.armyDisband = data.armyDisband;
				if(ret)ET.ATTR.Add("armyDisband");
			}
			if(data.HasRobFood){
				et.robFood = data.robFood;
				if(ret)ET.ATTR.Add("robFood");
			}
			if(data.HasRobWood){
				et.robWood = data.robWood;
				if(ret)ET.ATTR.Add("robWood");
			}
			if(data.HasRobStone){
				et.robStone = data.robStone;
				if(ret)ET.ATTR.Add("robStone");
			}
			if(data.HasRobGold){
				et.robGold = data.robGold;
				if(ret)ET.ATTR.Add("robGold");
			}
			if(data.HasMainHero){
				et.mainHero = data.mainHero;
				if(ret)ET.ATTR.Add("mainHero");
			}
			if(data.HasDeputyHero){
				et.deputyHero = data.deputyHero;
				if(ret)ET.ATTR.Add("deputyHero");
			}
			if(data.HasArmySumType){
				et.armySumType = data.armySumType;
				if(ret)ET.ATTR.Add("armySumType");
			}
			if(data.HasArmySum){
				et.armySum = data.armySum;
				if(ret)ET.ATTR.Add("armySum");
			}
			if(data.HasSoldiers){
				et.soldiers = data.soldiers;
				if(ret)ET.ATTR.Add("soldiers");
			}
			if(data.HasReinforceArmySumType){
				et.reinforceArmySumType = data.reinforceArmySumType;
				if(ret)ET.ATTR.Add("reinforceArmySumType");
			}
			if(data.HasReinforceSoldiers){
				et.reinforceSoldiers = data.reinforceSoldiers;
				if(ret)ET.ATTR.Add("reinforceSoldiers");
			}
			if(data.HasRallyMainHero){
				et.rallyMainHero = data.rallyMainHero;
				if(ret)ET.ATTR.Add("rallyMainHero");
			}
			if(data.HasRallyDeputyHero){
				et.rallyDeputyHero = data.rallyDeputyHero;
				if(ret)ET.ATTR.Add("rallyDeputyHero");
			}
			if(data.HasRallyArmySumType){
				et.rallyArmySumType = data.rallyArmySumType;
				if(ret)ET.ATTR.Add("rallyArmySumType");
			}
			if(data.HasRallySoldiers){
				et.rallySoldiers = data.rallySoldiers;
				if(ret)ET.ATTR.Add("rallySoldiers");
			}
			if(data.HasGuardTowerLevel){
				et.guardTowerLevel = data.guardTowerLevel;
				if(ret)ET.ATTR.Add("guardTowerLevel");
			}
			if(data.HasGuardTowerHp){
				et.guardTowerHp = data.guardTowerHp;
				if(ret)ET.ATTR.Add("guardTowerHp");
			}
			if(data.HasGuardTowerHpLimit){
				et.guardTowerHpLimit = data.guardTowerHpLimit;
				if(ret)ET.ATTR.Add("guardTowerHpLimit");
			}
			if(data.HasGuildFlagSigns){
				et.guildFlagSigns = data.guildFlagSigns;
				if(ret)ET.ATTR.Add("guildFlagSigns");
			}
			if(data.HasRobResourceType){
				et.robResourceType = data.robResourceType;
				if(ret)ET.ATTR.Add("robResourceType");
			}
			if(data.HasRoleAge){
				et.roleAge = data.roleAge;
				if(ret)ET.ATTR.Add("roleAge");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleReportExEntity
	{
		public const string BattleReportExChange = "BattleReportExChange";
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleReportExDetail> objectInfos;
		public System.Int64 battleBeginTime;
		public System.Int64 battleEndTime;
		public System.Int64 battleType;
		public SprotoType.RewardInfo rewardInfo;
		public System.Int64 winObjectIndex;
		public System.Int64 mainHeroExp;
		public System.Int64 deputyHeroExp;
		public System.Collections.Generic.List<SprotoType.BattleReinforceArmy> reinforceJoinArmy;
		public System.Collections.Generic.List<SprotoType.BattleReinforceArmy> reinforceLeaveArmy;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleReportWithObjectIndex> battleReport;

		public static HashSet<string> updateEntity(BattleReportExEntity et ,SprotoType.BattleReportEx data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasObjectInfos){

				if (et.objectInfos == null) {
					 et.objectInfos = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleReportExDetail>();
				}
				foreach(var item in data.objectInfos){ 
					if(et.objectInfos.ContainsKey(item.Key)){
						et.objectInfos[item.Key] = item.Value;
					}else{
						et.objectInfos.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("objectInfos"); 
			}
			if(data.HasBattleBeginTime){
				et.battleBeginTime = data.battleBeginTime;
				if(ret)ET.ATTR.Add("battleBeginTime");
			}
			if(data.HasBattleEndTime){
				et.battleEndTime = data.battleEndTime;
				if(ret)ET.ATTR.Add("battleEndTime");
			}
			if(data.HasBattleType){
				et.battleType = data.battleType;
				if(ret)ET.ATTR.Add("battleType");
			}
			if(data.HasRewardInfo){
				et.rewardInfo = data.rewardInfo;
				if(ret)ET.ATTR.Add("rewardInfo");
			}
			if(data.HasWinObjectIndex){
				et.winObjectIndex = data.winObjectIndex;
				if(ret)ET.ATTR.Add("winObjectIndex");
			}
			if(data.HasMainHeroExp){
				et.mainHeroExp = data.mainHeroExp;
				if(ret)ET.ATTR.Add("mainHeroExp");
			}
			if(data.HasDeputyHeroExp){
				et.deputyHeroExp = data.deputyHeroExp;
				if(ret)ET.ATTR.Add("deputyHeroExp");
			}
			if(data.HasReinforceJoinArmy){
				et.reinforceJoinArmy = data.reinforceJoinArmy;
				if(ret)ET.ATTR.Add("reinforceJoinArmy");
			}
			if(data.HasReinforceLeaveArmy){
				et.reinforceLeaveArmy = data.reinforceLeaveArmy;
				if(ret)ET.ATTR.Add("reinforceLeaveArmy");
			}
			if(data.HasBattleReport){

				if (et.battleReport == null) {
					 et.battleReport = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleReportWithObjectIndex>();
				}
				foreach(var item in data.battleReport){ 
					if(et.battleReport.ContainsKey(item.Key)){
						et.battleReport[item.Key] = item.Value;
					}else{
						et.battleReport.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("battleReport"); 
			}
			return ET.ATTR;
		}
	}
	public partial class RoleListEntity
	{
		public const string RoleListChange = "RoleListChange";
		public System.Int64 rid;
		public System.String name;
		public System.Int64 headId;
		public System.Int64 headFrameID;
		public SprotoType.RewardInfo rewardInfo;

		public static HashSet<string> updateEntity(RoleListEntity et ,SprotoType.RoleList data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasRewardInfo){
				et.rewardInfo = data.rewardInfo;
				if(ret)ET.ATTR.Add("rewardInfo");
			}
			return ET.ATTR;
		}
	}
	public partial class ShopItemEntity
	{
		public const string ShopItemChange = "ShopItemChange";
		public System.Int64 itemId;
		public System.Int64 buyCount;

		public static HashSet<string> updateEntity(ShopItemEntity et ,SprotoType.Expedition.ShopItem data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasBuyCount){
				et.buyCount = data.buyCount;
				if(ret)ET.ATTR.Add("buyCount");
			}
			return ET.ATTR;
		}
	}
	public partial class ConsumeCurrencyInfoEntity
	{
		public const string ConsumeCurrencyInfoChange = "ConsumeCurrencyInfoChange";
		public System.Int64 type;
		public System.Int64 num;

		public static HashSet<string> updateEntity(ConsumeCurrencyInfoEntity et ,SprotoType.ConsumeCurrencyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildCurrencyInfoEntity
	{
		public const string GuildCurrencyInfoChange = "GuildCurrencyInfoChange";
		public System.Int64 type;
		public System.Int64 num;
		public System.Int64 limit;
		public System.Int64 produce;
		public System.Int64 lastProduceTime;

		public static HashSet<string> updateEntity(GuildCurrencyInfoEntity et ,SprotoType.GuildCurrencyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			if(data.HasLimit){
				et.limit = data.limit;
				if(ret)ET.ATTR.Add("limit");
			}
			if(data.HasProduce){
				et.produce = data.produce;
				if(ret)ET.ATTR.Add("produce");
			}
			if(data.HasLastProduceTime){
				et.lastProduceTime = data.lastProduceTime;
				if(ret)ET.ATTR.Add("lastProduceTime");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildConsumeRecordInfoEntity
	{
		public const string GuildConsumeRecordInfoChange = "GuildConsumeRecordInfoChange";
		public System.Int64 roleHeadId;
		public System.String roleName;
		public System.Int64 consumeType;
		public System.Collections.Generic.List<System.Int64> consumeArgs;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ConsumeCurrencyInfo> consumeCurrencies;
		public System.Int64 consumeTime;
		public System.Int64 roleHeadFrameID;

		public static HashSet<string> updateEntity(GuildConsumeRecordInfoEntity et ,SprotoType.GuildConsumeRecordInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRoleHeadId){
				et.roleHeadId = data.roleHeadId;
				if(ret)ET.ATTR.Add("roleHeadId");
			}
			if(data.HasRoleName){
				et.roleName = data.roleName;
				if(ret)ET.ATTR.Add("roleName");
			}
			if(data.HasConsumeType){
				et.consumeType = data.consumeType;
				if(ret)ET.ATTR.Add("consumeType");
			}
			if(data.HasConsumeArgs){
				et.consumeArgs = data.consumeArgs;
				if(ret)ET.ATTR.Add("consumeArgs");
			}
			if(data.HasConsumeCurrencies){

				if (et.consumeCurrencies == null) {
					 et.consumeCurrencies = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ConsumeCurrencyInfo>();
				}
				foreach(var item in data.consumeCurrencies){ 
					if(et.consumeCurrencies.ContainsKey(item.Key)){
						et.consumeCurrencies[item.Key] = item.Value;
					}else{
						et.consumeCurrencies.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("consumeCurrencies"); 
			}
			if(data.HasConsumeTime){
				et.consumeTime = data.consumeTime;
				if(ret)ET.ATTR.Add("consumeTime");
			}
			if(data.HasRoleHeadFrameID){
				et.roleHeadFrameID = data.roleHeadFrameID;
				if(ret)ET.ATTR.Add("roleHeadFrameID");
			}
			return ET.ATTR;
		}
	}
	public partial class CurrencyInfoEntity
	{
		public const string CurrencyInfoChange = "CurrencyInfoChange";
		public System.Int64 type;
		public System.Int64 num;

		public static HashSet<string> updateEntity(CurrencyInfoEntity et ,SprotoType.CurrencyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public partial class RoleDonateInfoEntity
	{
		public const string RoleDonateInfoChange = "RoleDonateInfoChange";
		public System.String name;
		public System.Int64 donateNum;

		public static HashSet<string> updateEntity(RoleDonateInfoEntity et ,SprotoType.RoleDonateInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasDonateNum){
				et.donateNum = data.donateNum;
				if(ret)ET.ATTR.Add("donateNum");
			}
			return ET.ATTR;
		}
	}
	public partial class InactiveMembersInfoEntity
	{
		public const string InactiveMembersInfoChange = "InactiveMembersInfoChange";
		public System.Int64 rid;
		public System.String name;
		public System.Int64 headId;
		public System.Int64 headFrameID;
		public System.Int64 lastLogoutTime;

		public static HashSet<string> updateEntity(InactiveMembersInfoEntity et ,SprotoType.InactiveMembersInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasLastLogoutTime){
				et.lastLogoutTime = data.lastLogoutTime;
				if(ret)ET.ATTR.Add("lastLogoutTime");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildBuildInfoEntity
	{
		public const string GuildBuildInfoChange = "GuildBuildInfoChange";
		public System.Int64 buildIndex;
		public System.Int64 type;
		public SprotoType.PosInfo pos;
		public System.Int64 status;
		public System.Int64 durable;
		public System.Int64 buildProgress;
		public System.Boolean isReinforce;
		public System.Int64 buildProgressTime;
		public System.Int64 buildFinishTime;
		public System.Int64 durableLimit;
		public System.Int64 burnSpeed;
		public System.Int64 objectIndex;
		public System.Int64 burnTime;
		public System.Int64 durableRecoverTime;
		public System.Boolean isBattle;

		public static HashSet<string> updateEntity(GuildBuildInfoEntity et ,SprotoType.GuildBuildInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasBuildIndex){
				et.buildIndex = data.buildIndex;
				if(ret)ET.ATTR.Add("buildIndex");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasDurable){
				et.durable = data.durable;
				if(ret)ET.ATTR.Add("durable");
			}
			if(data.HasBuildProgress){
				et.buildProgress = data.buildProgress;
				if(ret)ET.ATTR.Add("buildProgress");
			}
			if(data.HasIsReinforce){
				et.isReinforce = data.isReinforce;
				if(ret)ET.ATTR.Add("isReinforce");
			}
			if(data.HasBuildProgressTime){
				et.buildProgressTime = data.buildProgressTime;
				if(ret)ET.ATTR.Add("buildProgressTime");
			}
			if(data.HasBuildFinishTime){
				et.buildFinishTime = data.buildFinishTime;
				if(ret)ET.ATTR.Add("buildFinishTime");
			}
			if(data.HasDurableLimit){
				et.durableLimit = data.durableLimit;
				if(ret)ET.ATTR.Add("durableLimit");
			}
			if(data.HasBurnSpeed){
				et.burnSpeed = data.burnSpeed;
				if(ret)ET.ATTR.Add("burnSpeed");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			if(data.HasBurnTime){
				et.burnTime = data.burnTime;
				if(ret)ET.ATTR.Add("burnTime");
			}
			if(data.HasDurableRecoverTime){
				et.durableRecoverTime = data.durableRecoverTime;
				if(ret)ET.ATTR.Add("durableRecoverTime");
			}
			if(data.HasIsBattle){
				et.isBattle = data.isBattle;
				if(ret)ET.ATTR.Add("isBattle");
			}
			return ET.ATTR;
		}
	}
	public partial class KillCountEntity
	{
		public const string KillCountChange = "KillCountChange";
		public System.Int64 level;
		public System.Int64 count;

		public static HashSet<string> updateEntity(KillCountEntity et ,SprotoType.KillCount data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			return ET.ATTR;
		}
	}
	public partial class HelpInfoEntity
	{
		public const string HelpInfoChange = "HelpInfoChange";
		public System.Int64 rid;

		public static HashSet<string> updateEntity(HelpInfoEntity et ,SprotoType.HelpInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			return ET.ATTR;
		}
	}
	public partial class TerritoryLineInfoEntity
	{
		public const string TerritoryLineInfoChange = "TerritoryLineInfoChange";
		public System.Collections.Generic.List<SprotoType.PosInfo> linePos;
		public System.Int64 direction;

		public static HashSet<string> updateEntity(TerritoryLineInfoEntity et ,SprotoType.TerritoryLineInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasLinePos){
				et.linePos = data.linePos;
				if(ret)ET.ATTR.Add("linePos");
			}
			if(data.HasDirection){
				et.direction = data.direction;
				if(ret)ET.ATTR.Add("direction");
			}
			return ET.ATTR;
		}
	}
	public partial class TalentTreesEntity
	{
		public const string TalentTreesChange = "TalentTreesChange";
		public System.Int64 index;
		public System.Collections.Generic.List<System.Int64> talentTree;
		public System.String name;

		public static HashSet<string> updateEntity(TalentTreesEntity et ,SprotoType.HeroInfo.TalentTrees data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasTalentTree){
				et.talentTree = data.talentTree;
				if(ret)ET.ATTR.Add("talentTree");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			return ET.ATTR;
		}
	}
	public partial class CityBuffEntity
	{
		public const string CityBuffChange = "CityBuffChange";
		public System.Int64 id;
		public System.Int64 expiredTime;

		public static HashSet<string> updateEntity(CityBuffEntity et ,SprotoType.CityBuff data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasExpiredTime){
				et.expiredTime = data.expiredTime;
				if(ret)ET.ATTR.Add("expiredTime");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleBuffDetailEntity
	{
		public const string BattleBuffDetailChange = "BattleBuffDetailChange";
		public System.Int64 buffId;
		public System.Boolean isNew;

		public static HashSet<string> updateEntity(BattleBuffDetailEntity et ,SprotoType.BattleBuffDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasBuffId){
				et.buffId = data.buffId;
				if(ret)ET.ATTR.Add("buffId");
			}
			if(data.HasIsNew){
				et.isNew = data.isNew;
				if(ret)ET.ATTR.Add("isNew");
			}
			return ET.ATTR;
		}
	}
	public partial class ArmyMarchInfoEntity
	{
		public const string ArmyMarchInfoChange = "ArmyMarchInfoChange";
		public System.Collections.Generic.List<SprotoType.PosInfo> path;
		public System.Int64 rid;
		public System.Int64 objectIndex;
		public System.Int64 guildId;
		public System.Boolean isDelete;

		public static HashSet<string> updateEntity(ArmyMarchInfoEntity et ,SprotoType.ArmyMarchInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasPath){
				et.path = data.path;
				if(ret)ET.ATTR.Add("path");
			}
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasIsDelete){
				et.isDelete = data.isDelete;
				if(ret)ET.ATTR.Add("isDelete");
			}
			return ET.ATTR;
		}
	}
	public partial class CollectSpeedInfoEntity
	{
		public const string CollectSpeedInfoChange = "CollectSpeedInfoChange";
		public System.Int64 collectSpeed;
		public System.Int64 collectTime;

		public static HashSet<string> updateEntity(CollectSpeedInfoEntity et ,SprotoType.CollectSpeedInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasCollectSpeed){
				et.collectSpeed = data.collectSpeed;
				if(ret)ET.ATTR.Add("collectSpeed");
			}
			if(data.HasCollectTime){
				et.collectTime = data.collectTime;
				if(ret)ET.ATTR.Add("collectTime");
			}
			return ET.ATTR;
		}
	}
	public partial class MysteryStoreGoodsEntity
	{
		public const string MysteryStoreGoodsChange = "MysteryStoreGoodsChange";
		public System.Int64 id;
		public System.Int64 num;
		public System.Int64 discount;
		public System.Boolean isBuy;
		public System.Int64 price;

		public static HashSet<string> updateEntity(MysteryStoreGoodsEntity et ,SprotoType.MysteryStore.MysteryStoreGoods data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			if(data.HasDiscount){
				et.discount = data.discount;
				if(ret)ET.ATTR.Add("discount");
			}
			if(data.HasIsBuy){
				et.isBuy = data.isBuy;
				if(ret)ET.ATTR.Add("isBuy");
			}
			if(data.HasPrice){
				et.price = data.price;
				if(ret)ET.ATTR.Add("price");
			}
			return ET.ATTR;
		}
	}
	public partial class SystemMsgEntity
	{
		public const string SystemMsgChange = "SystemMsgChange";
		public System.Int64 languageId;
		public System.Collections.Generic.List<System.String> args;

		public static HashSet<string> updateEntity(SystemMsgEntity et ,SprotoType.SystemMsg data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasLanguageId){
				et.languageId = data.languageId;
				if(ret)ET.ATTR.Add("languageId");
			}
			if(data.HasArgs){
				et.args = data.args;
				if(ret)ET.ATTR.Add("args");
			}
			return ET.ATTR;
		}
	}
	public partial class ItemsEntity
	{
		public const string ItemsChange = "ItemsChange";
		public System.Int64 itemId;
		public System.Int64 itemNum;

		public static HashSet<string> updateEntity(ItemsEntity et ,SprotoType.QueueInfo.Items data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasItemNum){
				et.itemNum = data.itemNum;
				if(ret)ET.ATTR.Add("itemNum");
			}
			return ET.ATTR;
		}
	}
	public partial class JoinRallyDetailEntity
	{
		public const string JoinRallyDetailChange = "JoinRallyDetailChange";
		public System.Int64 joinRid;
		public System.String joinName;
		public System.Int64 joinHeadId;
		public System.Int64 joinHeadFrameId;
		public SprotoType.PosInfo joinPos;
		public System.Int64 joinMainHeroId;
		public System.Int64 joinDeputyHeroId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> joinSoldiers;
		public System.Int64 joinTime;
		public System.Int64 joinArrivalTime;
		public System.Boolean joinDelete;
		public System.Int64 joinArmyIndex;
		public System.Int64 joinMainHeroLevel;
		public System.Int64 joinDeputyHeroLevel;

		public static HashSet<string> updateEntity(JoinRallyDetailEntity et ,SprotoType.JoinRallyDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasJoinRid){
				et.joinRid = data.joinRid;
				if(ret)ET.ATTR.Add("joinRid");
			}
			if(data.HasJoinName){
				et.joinName = data.joinName;
				if(ret)ET.ATTR.Add("joinName");
			}
			if(data.HasJoinHeadId){
				et.joinHeadId = data.joinHeadId;
				if(ret)ET.ATTR.Add("joinHeadId");
			}
			if(data.HasJoinHeadFrameId){
				et.joinHeadFrameId = data.joinHeadFrameId;
				if(ret)ET.ATTR.Add("joinHeadFrameId");
			}
			if(data.HasJoinPos){
				et.joinPos = data.joinPos;
				if(ret)ET.ATTR.Add("joinPos");
			}
			if(data.HasJoinMainHeroId){
				et.joinMainHeroId = data.joinMainHeroId;
				if(ret)ET.ATTR.Add("joinMainHeroId");
			}
			if(data.HasJoinDeputyHeroId){
				et.joinDeputyHeroId = data.joinDeputyHeroId;
				if(ret)ET.ATTR.Add("joinDeputyHeroId");
			}
			if(data.HasJoinSoldiers){

				if (et.joinSoldiers == null) {
					 et.joinSoldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.joinSoldiers){ 
					if(et.joinSoldiers.ContainsKey(item.Key)){
						et.joinSoldiers[item.Key] = item.Value;
					}else{
						et.joinSoldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("joinSoldiers"); 
			}
			if(data.HasJoinTime){
				et.joinTime = data.joinTime;
				if(ret)ET.ATTR.Add("joinTime");
			}
			if(data.HasJoinArrivalTime){
				et.joinArrivalTime = data.joinArrivalTime;
				if(ret)ET.ATTR.Add("joinArrivalTime");
			}
			if(data.HasJoinDelete){
				et.joinDelete = data.joinDelete;
				if(ret)ET.ATTR.Add("joinDelete");
			}
			if(data.HasJoinArmyIndex){
				et.joinArmyIndex = data.joinArmyIndex;
				if(ret)ET.ATTR.Add("joinArmyIndex");
			}
			if(data.HasJoinMainHeroLevel){
				et.joinMainHeroLevel = data.joinMainHeroLevel;
				if(ret)ET.ATTR.Add("joinMainHeroLevel");
			}
			if(data.HasJoinDeputyHeroLevel){
				et.joinDeputyHeroLevel = data.joinDeputyHeroLevel;
				if(ret)ET.ATTR.Add("joinDeputyHeroLevel");
			}
			return ET.ATTR;
		}
	}
	public partial class RallyTargetDetailEntity
	{
		public const string RallyTargetDetailChange = "RallyTargetDetailChange";
		public SprotoType.PosInfo rallyTargetPos;
		public System.String rallyTargetName;
		public System.String rallyTargetGuildName;
		public System.Int64 rallyTargetHeadId;
		public System.Int64 rallyTargetType;
		public System.Int64 rallyTargetMonsterId;
		public System.Int64 rallyTargetHeadFrameId;
		public System.Int64 rallyTargetHolyLandId;

		public static HashSet<string> updateEntity(RallyTargetDetailEntity et ,SprotoType.RallyTargetDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRallyTargetPos){
				et.rallyTargetPos = data.rallyTargetPos;
				if(ret)ET.ATTR.Add("rallyTargetPos");
			}
			if(data.HasRallyTargetName){
				et.rallyTargetName = data.rallyTargetName;
				if(ret)ET.ATTR.Add("rallyTargetName");
			}
			if(data.HasRallyTargetGuildName){
				et.rallyTargetGuildName = data.rallyTargetGuildName;
				if(ret)ET.ATTR.Add("rallyTargetGuildName");
			}
			if(data.HasRallyTargetHeadId){
				et.rallyTargetHeadId = data.rallyTargetHeadId;
				if(ret)ET.ATTR.Add("rallyTargetHeadId");
			}
			if(data.HasRallyTargetType){
				et.rallyTargetType = data.rallyTargetType;
				if(ret)ET.ATTR.Add("rallyTargetType");
			}
			if(data.HasRallyTargetMonsterId){
				et.rallyTargetMonsterId = data.rallyTargetMonsterId;
				if(ret)ET.ATTR.Add("rallyTargetMonsterId");
			}
			if(data.HasRallyTargetHeadFrameId){
				et.rallyTargetHeadFrameId = data.rallyTargetHeadFrameId;
				if(ret)ET.ATTR.Add("rallyTargetHeadFrameId");
			}
			if(data.HasRallyTargetHolyLandId){
				et.rallyTargetHolyLandId = data.rallyTargetHolyLandId;
				if(ret)ET.ATTR.Add("rallyTargetHolyLandId");
			}
			return ET.ATTR;
		}
	}
	public partial class ReinforceDetailEntity
	{
		public const string ReinforceDetailChange = "ReinforceDetailChange";
		public System.Int64 reinforceRid;
		public System.Int64 mainHeroId;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroId;
		public System.Int64 deputyHeroLevel;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Int64 arrivalTime;
		public System.Int64 reinforceTime;
		public System.Boolean reinforceDelete;
		public System.String reinforceName;
		public System.Int64 reinforceHeadId;
		public System.Int64 reinforceHeadFrameId;
		public System.Int64 armyIndex;

		public static HashSet<string> updateEntity(ReinforceDetailEntity et ,SprotoType.ReinforceDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasReinforceRid){
				et.reinforceRid = data.reinforceRid;
				if(ret)ET.ATTR.Add("reinforceRid");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasReinforceTime){
				et.reinforceTime = data.reinforceTime;
				if(ret)ET.ATTR.Add("reinforceTime");
			}
			if(data.HasReinforceDelete){
				et.reinforceDelete = data.reinforceDelete;
				if(ret)ET.ATTR.Add("reinforceDelete");
			}
			if(data.HasReinforceName){
				et.reinforceName = data.reinforceName;
				if(ret)ET.ATTR.Add("reinforceName");
			}
			if(data.HasReinforceHeadId){
				et.reinforceHeadId = data.reinforceHeadId;
				if(ret)ET.ATTR.Add("reinforceHeadId");
			}
			if(data.HasReinforceHeadFrameId){
				et.reinforceHeadFrameId = data.reinforceHeadFrameId;
				if(ret)ET.ATTR.Add("reinforceHeadFrameId");
			}
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			return ET.ATTR;
		}
	}
	public partial class RallyDetailEntity
	{
		public const string RallyDetailChange = "RallyDetailChange";
		public System.Int64 rallyRid;
		public System.String rallyName;
		public System.Int64 rallyHeadId;
		public System.Int64 rallyHeadFrameId;
		public SprotoType.PosInfo rallyPos;
		public System.Int64 rallyArmyCountMax;
		public System.Int64 rallyArmyCount;
		public System.Int64 rallyReadyTime;
		public System.Int64 rallyWaitTime;
		public System.Int64 rallyMarchTime;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.JoinRallyDetail> rallyJoinDetail;
		public System.Int64 rallyStartTime;
		public System.Int64 rallyObjectIndex;
		public SprotoType.RallyTargetDetail rallyTargetDetail;
		public System.Boolean rallyDelete;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ReinforceDetail> rallyReinforceDetail;
		public System.String rallyGuildName;
		public System.Collections.Generic.List<SprotoType.PosInfo> rallyPath;

		public static HashSet<string> updateEntity(RallyDetailEntity et ,SprotoType.RallyDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRallyRid){
				et.rallyRid = data.rallyRid;
				if(ret)ET.ATTR.Add("rallyRid");
			}
			if(data.HasRallyName){
				et.rallyName = data.rallyName;
				if(ret)ET.ATTR.Add("rallyName");
			}
			if(data.HasRallyHeadId){
				et.rallyHeadId = data.rallyHeadId;
				if(ret)ET.ATTR.Add("rallyHeadId");
			}
			if(data.HasRallyHeadFrameId){
				et.rallyHeadFrameId = data.rallyHeadFrameId;
				if(ret)ET.ATTR.Add("rallyHeadFrameId");
			}
			if(data.HasRallyPos){
				et.rallyPos = data.rallyPos;
				if(ret)ET.ATTR.Add("rallyPos");
			}
			if(data.HasRallyArmyCountMax){
				et.rallyArmyCountMax = data.rallyArmyCountMax;
				if(ret)ET.ATTR.Add("rallyArmyCountMax");
			}
			if(data.HasRallyArmyCount){
				et.rallyArmyCount = data.rallyArmyCount;
				if(ret)ET.ATTR.Add("rallyArmyCount");
			}
			if(data.HasRallyReadyTime){
				et.rallyReadyTime = data.rallyReadyTime;
				if(ret)ET.ATTR.Add("rallyReadyTime");
			}
			if(data.HasRallyWaitTime){
				et.rallyWaitTime = data.rallyWaitTime;
				if(ret)ET.ATTR.Add("rallyWaitTime");
			}
			if(data.HasRallyMarchTime){
				et.rallyMarchTime = data.rallyMarchTime;
				if(ret)ET.ATTR.Add("rallyMarchTime");
			}
			if(data.HasRallyJoinDetail){

				if (et.rallyJoinDetail == null) {
					 et.rallyJoinDetail = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.JoinRallyDetail>();
				}
				foreach(var item in data.rallyJoinDetail){ 
					if(et.rallyJoinDetail.ContainsKey(item.Key)){
						et.rallyJoinDetail[item.Key] = item.Value;
					}else{
						et.rallyJoinDetail.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("rallyJoinDetail"); 
			}
			if(data.HasRallyStartTime){
				et.rallyStartTime = data.rallyStartTime;
				if(ret)ET.ATTR.Add("rallyStartTime");
			}
			if(data.HasRallyObjectIndex){
				et.rallyObjectIndex = data.rallyObjectIndex;
				if(ret)ET.ATTR.Add("rallyObjectIndex");
			}
			if(data.HasRallyTargetDetail){
				et.rallyTargetDetail = data.rallyTargetDetail;
				if(ret)ET.ATTR.Add("rallyTargetDetail");
			}
			if(data.HasRallyDelete){
				et.rallyDelete = data.rallyDelete;
				if(ret)ET.ATTR.Add("rallyDelete");
			}
			if(data.HasRallyReinforceDetail){

				if (et.rallyReinforceDetail == null) {
					 et.rallyReinforceDetail = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ReinforceDetail>();
				}
				foreach(var item in data.rallyReinforceDetail){ 
					if(et.rallyReinforceDetail.ContainsKey(item.Key)){
						et.rallyReinforceDetail[item.Key] = item.Value;
					}else{
						et.rallyReinforceDetail.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("rallyReinforceDetail"); 
			}
			if(data.HasRallyGuildName){
				et.rallyGuildName = data.rallyGuildName;
				if(ret)ET.ATTR.Add("rallyGuildName");
			}
			if(data.HasRallyPath){
				et.rallyPath = data.rallyPath;
				if(ret)ET.ATTR.Add("rallyPath");
			}
			return ET.ATTR;
		}
	}
	public partial class RewardItemEntity
	{
		public const string RewardItemChange = "RewardItemChange";
		public System.Int64 itemId;
		public System.Int64 itemNum;

		public static HashSet<string> updateEntity(RewardItemEntity et ,SprotoType.RewardItem data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasItemNum){
				et.itemNum = data.itemNum;
				if(ret)ET.ATTR.Add("itemNum");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildGiftEntity
	{
		public const string GuildGiftChange = "GuildGiftChange";
		public System.Int64 giftType;
		public System.Int64 giftNum;

		public static HashSet<string> updateEntity(GuildGiftEntity et ,SprotoType.GuildGift data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGiftType){
				et.giftType = data.giftType;
				if(ret)ET.ATTR.Add("giftType");
			}
			if(data.HasGiftNum){
				et.giftNum = data.giftNum;
				if(ret)ET.ATTR.Add("giftNum");
			}
			return ET.ATTR;
		}
	}
	public partial class HerosEntity
	{
		public const string HerosChange = "HerosChange";
		public System.Int64 heroId;
		public System.Int64 num;
		public System.Int64 isNew;

		public static HashSet<string> updateEntity(HerosEntity et ,SprotoType.Heros data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasHeroId){
				et.heroId = data.heroId;
				if(ret)ET.ATTR.Add("heroId");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			if(data.HasIsNew){
				et.isNew = data.isNew;
				if(ret)ET.ATTR.Add("isNew");
			}
			return ET.ATTR;
		}
	}
	public partial class QueueInfoEntity
	{
		public const string QueueInfoChange = "QueueInfoChange";
		public System.Int64 queueIndex;
		public System.Boolean main;
		public System.Int64 finishTime;
		public System.Int64 buildingIndex;
		public System.Int64 expiredTime;
		public System.Int64 timerId;
		public System.Int64 type;
		public System.Int64 armyType;
		public System.Int64 newArmyLevel;
		public System.Int64 armyNum;
		public System.Int64 oldArmyLevel;
		public System.Int64 beginTime;
		public System.Int64 technologyType;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> treatmentSoldiers;
		public System.Int64 firstFinishTime;
		public System.Int64 healSpeedMulti;
		public System.Boolean requestGuildHelp;
		public System.Collections.Generic.List<SprotoType.QueueInfo.Items> produceItems;
		public System.Collections.Generic.List<SprotoType.QueueInfo.Items> completeItems;

		public static HashSet<string> updateEntity(QueueInfoEntity et ,SprotoType.QueueInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasQueueIndex){
				et.queueIndex = data.queueIndex;
				if(ret)ET.ATTR.Add("queueIndex");
			}
			if(data.HasMain){
				et.main = data.main;
				if(ret)ET.ATTR.Add("main");
			}
			if(data.HasFinishTime){
				et.finishTime = data.finishTime;
				if(ret)ET.ATTR.Add("finishTime");
			}
			if(data.HasBuildingIndex){
				et.buildingIndex = data.buildingIndex;
				if(ret)ET.ATTR.Add("buildingIndex");
			}
			if(data.HasExpiredTime){
				et.expiredTime = data.expiredTime;
				if(ret)ET.ATTR.Add("expiredTime");
			}
			if(data.HasTimerId){
				et.timerId = data.timerId;
				if(ret)ET.ATTR.Add("timerId");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasArmyType){
				et.armyType = data.armyType;
				if(ret)ET.ATTR.Add("armyType");
			}
			if(data.HasNewArmyLevel){
				et.newArmyLevel = data.newArmyLevel;
				if(ret)ET.ATTR.Add("newArmyLevel");
			}
			if(data.HasArmyNum){
				et.armyNum = data.armyNum;
				if(ret)ET.ATTR.Add("armyNum");
			}
			if(data.HasOldArmyLevel){
				et.oldArmyLevel = data.oldArmyLevel;
				if(ret)ET.ATTR.Add("oldArmyLevel");
			}
			if(data.HasBeginTime){
				et.beginTime = data.beginTime;
				if(ret)ET.ATTR.Add("beginTime");
			}
			if(data.HasTechnologyType){
				et.technologyType = data.technologyType;
				if(ret)ET.ATTR.Add("technologyType");
			}
			if(data.HasTreatmentSoldiers){

				if (et.treatmentSoldiers == null) {
					 et.treatmentSoldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.treatmentSoldiers){ 
					if(et.treatmentSoldiers.ContainsKey(item.Key)){
						et.treatmentSoldiers[item.Key] = item.Value;
					}else{
						et.treatmentSoldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("treatmentSoldiers"); 
			}
			if(data.HasFirstFinishTime){
				et.firstFinishTime = data.firstFinishTime;
				if(ret)ET.ATTR.Add("firstFinishTime");
			}
			if(data.HasHealSpeedMulti){
				et.healSpeedMulti = data.healSpeedMulti;
				if(ret)ET.ATTR.Add("healSpeedMulti");
			}
			if(data.HasRequestGuildHelp){
				et.requestGuildHelp = data.requestGuildHelp;
				if(ret)ET.ATTR.Add("requestGuildHelp");
			}
			if(data.HasProduceItems){
				et.produceItems = data.produceItems;
				if(ret)ET.ATTR.Add("produceItems");
			}
			if(data.HasCompleteItems){
				et.completeItems = data.completeItems;
				if(ret)ET.ATTR.Add("completeItems");
			}
			return ET.ATTR;
		}
	}
	public partial class TechnologyInfoEntity
	{
		public const string TechnologyInfoChange = "TechnologyInfoChange";
		public System.Int64 technologyType;
		public System.Int64 level;

		public static HashSet<string> updateEntity(TechnologyInfoEntity et ,SprotoType.TechnologyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTechnologyType){
				et.technologyType = data.technologyType;
				if(ret)ET.ATTR.Add("technologyType");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			return ET.ATTR;
		}
	}
	public partial class FinishTaskInfoEntity
	{
		public const string FinishTaskInfoChange = "FinishTaskInfoChange";
		public System.Int64 taskId;

		public static HashSet<string> updateEntity(FinishTaskInfoEntity et ,SprotoType.FinishTaskInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTaskId){
				et.taskId = data.taskId;
				if(ret)ET.ATTR.Add("taskId");
			}
			return ET.ATTR;
		}
	}
	public partial class TaskStatisticsEntity
	{
		public const string TaskStatisticsChange = "TaskStatisticsChange";
		public System.Int64 type;
		public System.Collections.Generic.List<SprotoType.Statistics> statistics;

		public static HashSet<string> updateEntity(TaskStatisticsEntity et ,SprotoType.TaskStatistics data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasStatistics){
				et.statistics = data.statistics;
				if(ret)ET.ATTR.Add("statistics");
			}
			return ET.ATTR;
		}
	}
	public partial class RoleStatisticsEntity
	{
		public const string RoleStatisticsChange = "RoleStatisticsChange";
		public System.Int64 type;
		public System.Int64 num;

		public static HashSet<string> updateEntity(RoleStatisticsEntity et ,SprotoType.RoleStatistics data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public partial class SoldierKillInfoEntity
	{
		public const string SoldierKillInfoChange = "SoldierKillInfoChange";
		public System.Int64 level;
		public System.Int64 num;

		public static HashSet<string> updateEntity(SoldierKillInfoEntity et ,SprotoType.RoleInfo.SoldierKillInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public partial class ChapterTaskInfoEntity
	{
		public const string ChapterTaskInfoChange = "ChapterTaskInfoChange";
		public System.Int64 taskId;
		public System.Int64 status;

		public static HashSet<string> updateEntity(ChapterTaskInfoEntity et ,SprotoType.ChapterTaskInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTaskId){
				et.taskId = data.taskId;
				if(ret)ET.ATTR.Add("taskId");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			return ET.ATTR;
		}
	}
	public partial class DenseFogInfoEntity
	{
		public const string DenseFogInfoChange = "DenseFogInfoChange";
		public System.Int64 index;
		public System.Int64 rule;

		public static HashSet<string> updateEntity(DenseFogInfoEntity et ,SprotoType.DenseFogInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasRule){
				et.rule = data.rule;
				if(ret)ET.ATTR.Add("rule");
			}
			return ET.ATTR;
		}
	}
	public partial class ReinforceArmyInfoEntity
	{
		public const string ReinforceArmyInfoChange = "ReinforceArmyInfoChange";
		public System.Int64 reinforceRid;
		public System.Int64 armyIndex;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Int64 mainHeroId;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroId;
		public System.Int64 deputyHeroLevel;
		public System.Int64 arrivalTime;
		public System.String name;
		public System.Int64 headId;
		public System.Int64 headFrameID;

		public static HashSet<string> updateEntity(ReinforceArmyInfoEntity et ,SprotoType.ReinforceArmyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasReinforceRid){
				et.reinforceRid = data.reinforceRid;
				if(ret)ET.ATTR.Add("reinforceRid");
			}
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			return ET.ATTR;
		}
	}
	public partial class VillageCaveInfoEntity
	{
		public const string VillageCaveInfoChange = "VillageCaveInfoChange";
		public System.Int64 index;
		public System.Int64 rule;

		public static HashSet<string> updateEntity(VillageCaveInfoEntity et ,SprotoType.VillageCaveInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasRule){
				et.rule = data.rule;
				if(ret)ET.ATTR.Add("rule");
			}
			return ET.ATTR;
		}
	}
	public partial class ActivityTimeInfoEntity
	{
		public const string ActivityTimeInfoChange = "ActivityTimeInfoChange";
		public System.Int64 activityId;
		public System.Int64 startTime;
		public System.Int64 endTime;
		public System.Collections.Generic.List<System.Int64> selfRank;

		public static HashSet<string> updateEntity(ActivityTimeInfoEntity et ,SprotoType.ActivityTimeInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasActivityId){
				et.activityId = data.activityId;
				if(ret)ET.ATTR.Add("activityId");
			}
			if(data.HasStartTime){
				et.startTime = data.startTime;
				if(ret)ET.ATTR.Add("startTime");
			}
			if(data.HasEndTime){
				et.endTime = data.endTime;
				if(ret)ET.ATTR.Add("endTime");
			}
			if(data.HasSelfRank){
				et.selfRank = data.selfRank;
				if(ret)ET.ATTR.Add("selfRank");
			}
			return ET.ATTR;
		}
	}
	public partial class ChatReadedUniqueIndexEntity
	{
		public const string ChatReadedUniqueIndexChange = "ChatReadedUniqueIndexChange";
		public System.Int64 uniqueIndex;
		public System.Int64 channelType;

		public static HashSet<string> updateEntity(ChatReadedUniqueIndexEntity et ,SprotoType.ChatReadedUniqueIndex data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasUniqueIndex){
				et.uniqueIndex = data.uniqueIndex;
				if(ret)ET.ATTR.Add("uniqueIndex");
			}
			if(data.HasChannelType){
				et.channelType = data.channelType;
				if(ret)ET.ATTR.Add("channelType");
			}
			return ET.ATTR;
		}
	}
	public partial class ChatNoDisturbInfoEntity
	{
		public const string ChatNoDisturbInfoChange = "ChatNoDisturbInfoChange";
		public System.Boolean chatNoDisturbFlag;
		public System.Int64 channelType;

		public static HashSet<string> updateEntity(ChatNoDisturbInfoEntity et ,SprotoType.ChatNoDisturbInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasChatNoDisturbFlag){
				et.chatNoDisturbFlag = data.chatNoDisturbFlag;
				if(ret)ET.ATTR.Add("chatNoDisturbFlag");
			}
			if(data.HasChannelType){
				et.channelType = data.channelType;
				if(ret)ET.ATTR.Add("channelType");
			}
			return ET.ATTR;
		}
	}
	public partial class ActivityEntity
	{
		public const string ActivityChange = "ActivityChange";
		public System.Int64 activityId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.ScheduleInfo> scheduleInfo;
		public System.Int64 startTime;
		public System.Collections.Generic.List<System.Int64> rewardId;
		public System.Int64 level;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.Exchange> exchange;
		public System.Int64 score;
		public System.Int64 rank;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.Rewards> rewards;
		public System.Boolean rewardBox;
		public System.Int64 stage;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.Ranks> ranks;
		public System.Int64 season;
		public System.Collections.Generic.List<System.Int64> ids;
		public System.Boolean activeReward;
		public System.Int64 times;
		public System.Int64 day;
		public System.Int64 count;
		public System.Int64 dayCount;
		public System.Int64 free;
		public System.Boolean discount;
		public System.Boolean isNew;

		public static HashSet<string> updateEntity(ActivityEntity et ,SprotoType.Activity data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasActivityId){
				et.activityId = data.activityId;
				if(ret)ET.ATTR.Add("activityId");
			}
			if(data.HasScheduleInfo){

				if (et.scheduleInfo == null) {
					 et.scheduleInfo = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.ScheduleInfo>();
				}
				foreach(var item in data.scheduleInfo){ 
					if(et.scheduleInfo.ContainsKey(item.Key)){
						et.scheduleInfo[item.Key] = item.Value;
					}else{
						et.scheduleInfo.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("scheduleInfo"); 
			}
			if(data.HasStartTime){
				et.startTime = data.startTime;
				if(ret)ET.ATTR.Add("startTime");
			}
			if(data.HasRewardId){
				et.rewardId = data.rewardId;
				if(ret)ET.ATTR.Add("rewardId");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasExchange){

				if (et.exchange == null) {
					 et.exchange = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.Exchange>();
				}
				foreach(var item in data.exchange){ 
					if(et.exchange.ContainsKey(item.Key)){
						et.exchange[item.Key] = item.Value;
					}else{
						et.exchange.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("exchange"); 
			}
			if(data.HasScore){
				et.score = data.score;
				if(ret)ET.ATTR.Add("score");
			}
			if(data.HasRank){
				et.rank = data.rank;
				if(ret)ET.ATTR.Add("rank");
			}
			if(data.HasRewards){

				if (et.rewards == null) {
					 et.rewards = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.Rewards>();
				}
				foreach(var item in data.rewards){ 
					if(et.rewards.ContainsKey(item.Key)){
						et.rewards[item.Key] = item.Value;
					}else{
						et.rewards.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("rewards"); 
			}
			if(data.HasRewardBox){
				et.rewardBox = data.rewardBox;
				if(ret)ET.ATTR.Add("rewardBox");
			}
			if(data.HasStage){
				et.stage = data.stage;
				if(ret)ET.ATTR.Add("stage");
			}
			if(data.HasRanks){

				if (et.ranks == null) {
					 et.ranks = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity.Ranks>();
				}
				foreach(var item in data.ranks){ 
					if(et.ranks.ContainsKey(item.Key)){
						et.ranks[item.Key] = item.Value;
					}else{
						et.ranks.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("ranks"); 
			}
			if(data.HasSeason){
				et.season = data.season;
				if(ret)ET.ATTR.Add("season");
			}
			if(data.HasIds){
				et.ids = data.ids;
				if(ret)ET.ATTR.Add("ids");
			}
			if(data.HasActiveReward){
				et.activeReward = data.activeReward;
				if(ret)ET.ATTR.Add("activeReward");
			}
			if(data.HasTimes){
				et.times = data.times;
				if(ret)ET.ATTR.Add("times");
			}
			if(data.HasDay){
				et.day = data.day;
				if(ret)ET.ATTR.Add("day");
			}
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			if(data.HasDayCount){
				et.dayCount = data.dayCount;
				if(ret)ET.ATTR.Add("dayCount");
			}
			if(data.HasFree){
				et.free = data.free;
				if(ret)ET.ATTR.Add("free");
			}
			if(data.HasDiscount){
				et.discount = data.discount;
				if(ret)ET.ATTR.Add("discount");
			}
			if(data.HasIsNew){
				et.isNew = data.isNew;
				if(ret)ET.ATTR.Add("isNew");
			}
			return ET.ATTR;
		}
	}
	public partial class MysteryStoreEntity
	{
		public const string MysteryStoreChange = "MysteryStoreChange";
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.MysteryStore.MysteryStoreGoods> mysteryStoreGoods;
		public System.Int64 leaveTime;
		public System.Int64 refreshCount;
		public System.Boolean freeRefresh;

		public static HashSet<string> updateEntity(MysteryStoreEntity et ,SprotoType.MysteryStore data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasMysteryStoreGoods){

				if (et.mysteryStoreGoods == null) {
					 et.mysteryStoreGoods = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.MysteryStore.MysteryStoreGoods>();
				}
				foreach(var item in data.mysteryStoreGoods){ 
					if(et.mysteryStoreGoods.ContainsKey(item.Key)){
						et.mysteryStoreGoods[item.Key] = item.Value;
					}else{
						et.mysteryStoreGoods.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("mysteryStoreGoods"); 
			}
			if(data.HasLeaveTime){
				et.leaveTime = data.leaveTime;
				if(ret)ET.ATTR.Add("leaveTime");
			}
			if(data.HasRefreshCount){
				et.refreshCount = data.refreshCount;
				if(ret)ET.ATTR.Add("refreshCount");
			}
			if(data.HasFreeRefresh){
				et.freeRefresh = data.freeRefresh;
				if(ret)ET.ATTR.Add("freeRefresh");
			}
			return ET.ATTR;
		}
	}
	public partial class RechargeEntity
	{
		public const string RechargeChange = "RechargeChange";
		public System.Int64 id;
		public System.Int64 count;

		public static HashSet<string> updateEntity(RechargeEntity et ,SprotoType.Recharge data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			return ET.ATTR;
		}
	}
	public partial class RechargeSaleEntity
	{
		public const string RechargeSaleChange = "RechargeSaleChange";
		public System.Int64 group;
		public System.Collections.Generic.List<System.Int64> ids;
		public System.Int64 buyTime;

		public static HashSet<string> updateEntity(RechargeSaleEntity et ,SprotoType.RechargeSale data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGroup){
				et.group = data.group;
				if(ret)ET.ATTR.Add("group");
			}
			if(data.HasIds){
				et.ids = data.ids;
				if(ret)ET.ATTR.Add("ids");
			}
			if(data.HasBuyTime){
				et.buyTime = data.buyTime;
				if(ret)ET.ATTR.Add("buyTime");
			}
			return ET.ATTR;
		}
	}
	public partial class SupplyEntity
	{
		public const string SupplyChange = "SupplyChange";
		public System.Int64 id;
		public System.Int64 expiredTime;
		public System.Boolean award;
		public System.Int64 awardTime;

		public static HashSet<string> updateEntity(SupplyEntity et ,SprotoType.Supply data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasExpiredTime){
				et.expiredTime = data.expiredTime;
				if(ret)ET.ATTR.Add("expiredTime");
			}
			if(data.HasAward){
				et.award = data.award;
				if(ret)ET.ATTR.Add("award");
			}
			if(data.HasAwardTime){
				et.awardTime = data.awardTime;
				if(ret)ET.ATTR.Add("awardTime");
			}
			return ET.ATTR;
		}
	}
	public partial class LimitTimePackageEntity
	{
		public const string LimitTimePackageChange = "LimitTimePackageChange";
		public System.Int64 index;
		public System.Int64 id;
		public System.Int64 expiredTime;

		public static HashSet<string> updateEntity(LimitTimePackageEntity et ,SprotoType.LimitTimePackage data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasExpiredTime){
				et.expiredTime = data.expiredTime;
				if(ret)ET.ATTR.Add("expiredTime");
			}
			return ET.ATTR;
		}
	}
	public partial class VipStoreEntity
	{
		public const string VipStoreChange = "VipStoreChange";
		public System.Int64 id;
		public System.Int64 count;

		public static HashSet<string> updateEntity(VipStoreEntity et ,SprotoType.VipStore data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			return ET.ATTR;
		}
	}
	public partial class ExpeditionEntity
	{
		public const string ExpeditionChange = "ExpeditionChange";
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Expedition.ShopItem> shopItem;
		public System.Int64 refreshCount;
		public System.Int64 headCount;

		public static HashSet<string> updateEntity(ExpeditionEntity et ,SprotoType.Expedition data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasShopItem){

				if (et.shopItem == null) {
					 et.shopItem = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Expedition.ShopItem>();
				}
				foreach(var item in data.shopItem){ 
					if(et.shopItem.ContainsKey(item.Key)){
						et.shopItem[item.Key] = item.Value;
					}else{
						et.shopItem.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("shopItem"); 
			}
			if(data.HasRefreshCount){
				et.refreshCount = data.refreshCount;
				if(ret)ET.ATTR.Add("refreshCount");
			}
			if(data.HasHeadCount){
				et.headCount = data.headCount;
				if(ret)ET.ATTR.Add("headCount");
			}
			return ET.ATTR;
		}
	}
	public partial class PushSettingEntity
	{
		public const string PushSettingChange = "PushSettingChange";
		public System.Int64 id;
		public System.Int64 open;

		public static HashSet<string> updateEntity(PushSettingEntity et ,SprotoType.PushSetting data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasOpen){
				et.open = data.open;
				if(ret)ET.ATTR.Add("open");
			}
			return ET.ATTR;
		}
	}
	public partial class ExpeditionInfoEntity
	{
		public const string ExpeditionInfoChange = "ExpeditionInfoChange";
		public System.Int64 id;
		public System.Int64 star;
		public System.Boolean reward;
		public System.Int64 finishTime;

		public static HashSet<string> updateEntity(ExpeditionInfoEntity et ,SprotoType.ExpeditionInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasStar){
				et.star = data.star;
				if(ret)ET.ATTR.Add("star");
			}
			if(data.HasReward){
				et.reward = data.reward;
				if(ret)ET.ATTR.Add("reward");
			}
			if(data.HasFinishTime){
				et.finishTime = data.finishTime;
				if(ret)ET.ATTR.Add("finishTime");
			}
			return ET.ATTR;
		}
	}
	public partial class MapMarkerInfoEntity
	{
		public const string MapMarkerInfoChange = "MapMarkerInfoChange";
		public System.Int64 markerIndex;
		public System.Int64 markerId;
		public System.String description;
		public System.String gameNode;
		public SprotoType.PosInfo pos;
		public System.Int64 markerTime;
		public System.Int64 status;
		public System.String createName;

		public static HashSet<string> updateEntity(MapMarkerInfoEntity et ,SprotoType.MapMarkerInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasMarkerIndex){
				et.markerIndex = data.markerIndex;
				if(ret)ET.ATTR.Add("markerIndex");
			}
			if(data.HasMarkerId){
				et.markerId = data.markerId;
				if(ret)ET.ATTR.Add("markerId");
			}
			if(data.HasDescription){
				et.description = data.description;
				if(ret)ET.ATTR.Add("description");
			}
			if(data.HasGameNode){
				et.gameNode = data.gameNode;
				if(ret)ET.ATTR.Add("gameNode");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasMarkerTime){
				et.markerTime = data.markerTime;
				if(ret)ET.ATTR.Add("markerTime");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasCreateName){
				et.createName = data.createName;
				if(ret)ET.ATTR.Add("createName");
			}
			return ET.ATTR;
		}
	}
	public partial class ScoutRoleInfoEntity
	{
		public const string ScoutRoleInfoChange = "ScoutRoleInfoChange";
		public System.Int64 rid;
		public System.String name;
		public System.Int64 headId;
		public System.Int64 headFrameID;

		public static HashSet<string> updateEntity(ScoutRoleInfoEntity et ,SprotoType.ScoutRoleInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			return ET.ATTR;
		}
	}
	public partial class DefendHeroInfoEntity
	{
		public const string DefendHeroInfoChange = "DefendHeroInfoChange";
		public System.Int64 heroId;
		public System.Collections.Generic.List<SprotoType.SkillInfo> skills;
		public System.Int64 star;

		public static HashSet<string> updateEntity(DefendHeroInfoEntity et ,SprotoType.DefendHeroInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasHeroId){
				et.heroId = data.heroId;
				if(ret)ET.ATTR.Add("heroId");
			}
			if(data.HasSkills){
				et.skills = data.skills;
				if(ret)ET.ATTR.Add("skills");
			}
			if(data.HasStar){
				et.star = data.star;
				if(ret)ET.ATTR.Add("star");
			}
			return ET.ATTR;
		}
	}
	public partial class StatisticsEntity
	{
		public const string StatisticsChange = "StatisticsChange";
		public System.Int64 arg;
		public System.Int64 num;

		public static HashSet<string> updateEntity(StatisticsEntity et ,SprotoType.Statistics data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasArg){
				et.arg = data.arg;
				if(ret)ET.ATTR.Add("arg");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public partial class DataInfoEntity
	{
		public const string DataInfoChange = "DataInfoChange";
		public System.Int64 condition;
		public System.Int64 count;

		public static HashSet<string> updateEntity(DataInfoEntity et ,SprotoType.Activity.ScheduleInfo.DataInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasCondition){
				et.condition = data.condition;
				if(ret)ET.ATTR.Add("condition");
			}
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			return ET.ATTR;
		}
	}
	public partial class HistoryRankEntity
	{
		public const string HistoryRankChange = "HistoryRankChange";
		public System.Int64 index;
		public System.Collections.Generic.List<SprotoType.HistoryInfo> historyInfo;
		public System.Int64 time;

		public static HashSet<string> updateEntity(HistoryRankEntity et ,SprotoType.Activity_GetHistoryRank.response.HistoryRank data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasHistoryInfo){
				et.historyInfo = data.historyInfo;
				if(ret)ET.ATTR.Add("historyInfo");
			}
			if(data.HasTime){
				et.time = data.time;
				if(ret)ET.ATTR.Add("time");
			}
			return ET.ATTR;
		}
	}
	public partial class RankListEntity
	{
		public const string RankListChange = "RankListChange";
		public System.Int64 index;
		public System.Collections.Generic.List<SprotoType.Activity_GetRank.response.RankList.RankInfo> ranks;

		public static HashSet<string> updateEntity(RankListEntity et ,SprotoType.Activity_GetRank.response.RankList data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasRanks){
				et.ranks = data.ranks;
				if(ret)ET.ATTR.Add("ranks");
			}
			return ET.ATTR;
		}
	}
	public partial class SelfRankEntity
	{
		public const string SelfRankChange = "SelfRankChange";
		public System.Int64 index;
		public System.Int64 rank;
		public System.Int64 score;

		public static HashSet<string> updateEntity(SelfRankEntity et ,SprotoType.Activity_GetRank.response.SelfRank data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasRank){
				et.rank = data.rank;
				if(ret)ET.ATTR.Add("rank");
			}
			if(data.HasScore){
				et.score = data.score;
				if(ret)ET.ATTR.Add("score");
			}
			return ET.ATTR;
		}
	}
	public partial class ArmyInfoEntity
	{
		public const string ArmyInfoChange = "ArmyInfoChange";
		public System.Int64 armyIndex;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Collections.Generic.List<SprotoType.ResourceCollectInfo> resourceLoads;
		public System.Int64 status;
		public SprotoType.ResourceCollectInfo collectResource;
		public System.Int64 preCostActionForce;
		public System.Int64 arrivalTime;
		public System.Collections.Generic.List<SprotoType.PosInfo> path;
		public System.Int64 targetType;
		public SprotoType.MarchTargetArg targetArg;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> minorSoldiers;
		public System.Int64 startTime;
		public System.Int64 objectIndex;
		public System.Int64 rid;
		public System.Int64 buildArmyIndex;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroLevel;
		public System.Int64 killMonsterReduceVit;

		public static HashSet<string> updateEntity(ArmyInfoEntity et ,SprotoType.ArmyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasResourceLoads){
				et.resourceLoads = data.resourceLoads;
				if(ret)ET.ATTR.Add("resourceLoads");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasCollectResource){
				et.collectResource = data.collectResource;
				if(ret)ET.ATTR.Add("collectResource");
			}
			if(data.HasPreCostActionForce){
				et.preCostActionForce = data.preCostActionForce;
				if(ret)ET.ATTR.Add("preCostActionForce");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasPath){
				et.path = data.path;
				if(ret)ET.ATTR.Add("path");
			}
			if(data.HasTargetType){
				et.targetType = data.targetType;
				if(ret)ET.ATTR.Add("targetType");
			}
			if(data.HasTargetArg){
				et.targetArg = data.targetArg;
				if(ret)ET.ATTR.Add("targetArg");
			}
			if(data.HasMinorSoldiers){

				if (et.minorSoldiers == null) {
					 et.minorSoldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.minorSoldiers){ 
					if(et.minorSoldiers.ContainsKey(item.Key)){
						et.minorSoldiers[item.Key] = item.Value;
					}else{
						et.minorSoldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("minorSoldiers"); 
			}
			if(data.HasStartTime){
				et.startTime = data.startTime;
				if(ret)ET.ATTR.Add("startTime");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasBuildArmyIndex){
				et.buildArmyIndex = data.buildArmyIndex;
				if(ret)ET.ATTR.Add("buildArmyIndex");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasKillMonsterReduceVit){
				et.killMonsterReduceVit = data.killMonsterReduceVit;
				if(ret)ET.ATTR.Add("killMonsterReduceVit");
			}
			return ET.ATTR;
		}
	}
	public partial class BattleDamageInfoEntity
	{
		public const string BattleDamageInfoChange = "BattleDamageInfoChange";
		public System.Int64 objectIndex;
		public System.Int64 damage;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleRemainSoldiers> battleRemainSoldiers;
		public System.Collections.Generic.List<SprotoType.SkillDamageHeal> skillInfo;
		public System.Int64 dotDamage;
		public System.Int64 armyRadius;
		public System.Int64 hotHeal;

		public static HashSet<string> updateEntity(BattleDamageInfoEntity et ,SprotoType.BattleDamageInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			if(data.HasDamage){
				et.damage = data.damage;
				if(ret)ET.ATTR.Add("damage");
			}
			if(data.HasBattleRemainSoldiers){

				if (et.battleRemainSoldiers == null) {
					 et.battleRemainSoldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.BattleRemainSoldiers>();
				}
				foreach(var item in data.battleRemainSoldiers){ 
					if(et.battleRemainSoldiers.ContainsKey(item.Key)){
						et.battleRemainSoldiers[item.Key] = item.Value;
					}else{
						et.battleRemainSoldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("battleRemainSoldiers"); 
			}
			if(data.HasSkillInfo){
				et.skillInfo = data.skillInfo;
				if(ret)ET.ATTR.Add("skillInfo");
			}
			if(data.HasDotDamage){
				et.dotDamage = data.dotDamage;
				if(ret)ET.ATTR.Add("dotDamage");
			}
			if(data.HasArmyRadius){
				et.armyRadius = data.armyRadius;
				if(ret)ET.ATTR.Add("armyRadius");
			}
			if(data.HasHotHeal){
				et.hotHeal = data.hotHeal;
				if(ret)ET.ATTR.Add("hotHeal");
			}
			return ET.ATTR;
		}
	}
	public partial class BuildingInfoEntity
	{
		public const string BuildingInfoChange = "BuildingInfoChange";
		public System.Int64 buildingIndex;
		public System.Int64 type;
		public System.Int64 level;
		public SprotoType.PosInfo pos;
		public System.Int64 finishTime;
		public System.Int64 version;
		public System.Int64 lastRewardTime;
		public System.Int64 lostHp;
		public System.Int64 beginBurnTime;
		public System.Int64 serviceTime;
		public System.Int64 lastBurnTime;
		public System.Int64 buildTime;
		public SprotoType.BuildingGainInfo buildingGainInfo;

		public static HashSet<string> updateEntity(BuildingInfoEntity et ,SprotoType.BuildingInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasBuildingIndex){
				et.buildingIndex = data.buildingIndex;
				if(ret)ET.ATTR.Add("buildingIndex");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasFinishTime){
				et.finishTime = data.finishTime;
				if(ret)ET.ATTR.Add("finishTime");
			}
			if(data.HasVersion){
				et.version = data.version;
				if(ret)ET.ATTR.Add("version");
			}
			if(data.HasLastRewardTime){
				et.lastRewardTime = data.lastRewardTime;
				if(ret)ET.ATTR.Add("lastRewardTime");
			}
			if(data.HasLostHp){
				et.lostHp = data.lostHp;
				if(ret)ET.ATTR.Add("lostHp");
			}
			if(data.HasBeginBurnTime){
				et.beginBurnTime = data.beginBurnTime;
				if(ret)ET.ATTR.Add("beginBurnTime");
			}
			if(data.HasServiceTime){
				et.serviceTime = data.serviceTime;
				if(ret)ET.ATTR.Add("serviceTime");
			}
			if(data.HasLastBurnTime){
				et.lastBurnTime = data.lastBurnTime;
				if(ret)ET.ATTR.Add("lastBurnTime");
			}
			if(data.HasBuildTime){
				et.buildTime = data.buildTime;
				if(ret)ET.ATTR.Add("buildTime");
			}
			if(data.HasBuildingGainInfo){
				et.buildingGainInfo = data.buildingGainInfo;
				if(ret)ET.ATTR.Add("buildingGainInfo");
			}
			return ET.ATTR;
		}
	}
	public partial class ChatInfoEntity
	{
		public const string ChatInfoChange = "ChatInfoChange";
		public System.String msg;
		public System.Int64 timeStamp;
		public System.Int64 toRid;

		public static HashSet<string> updateEntity(ChatInfoEntity et ,SprotoType.ChatInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasMsg){
				et.msg = data.msg;
				if(ret)ET.ATTR.Add("msg");
			}
			if(data.HasTimeStamp){
				et.timeStamp = data.timeStamp;
				if(ret)ET.ATTR.Add("timeStamp");
			}
			if(data.HasToRid){
				et.toRid = data.toRid;
				if(ret)ET.ATTR.Add("toRid");
			}
			return ET.ATTR;
		}
	}
	public partial class ChatRoleBriefEntity
	{
		public const string ChatRoleBriefChange = "ChatRoleBriefChange";
		public System.Int64 rid;
		public System.Int64 headId;
		public System.String name;
		public System.Int64 guildId;
		public System.String guildAbbr;
		public System.Int64 headFrameID;
		public System.String lastMsg;
		public System.Int64 lastMsgTS;
		public System.Int64 lastReadTS;
		public System.Int64 notReadCnt;

		public static HashSet<string> updateEntity(ChatRoleBriefEntity et ,SprotoType.ChatRoleBrief data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasGuildAbbr){
				et.guildAbbr = data.guildAbbr;
				if(ret)ET.ATTR.Add("guildAbbr");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasLastMsg){
				et.lastMsg = data.lastMsg;
				if(ret)ET.ATTR.Add("lastMsg");
			}
			if(data.HasLastMsgTS){
				et.lastMsgTS = data.lastMsgTS;
				if(ret)ET.ATTR.Add("lastMsgTS");
			}
			if(data.HasLastReadTS){
				et.lastReadTS = data.lastReadTS;
				if(ret)ET.ATTR.Add("lastReadTS");
			}
			if(data.HasNotReadCnt){
				et.notReadCnt = data.notReadCnt;
				if(ret)ET.ATTR.Add("notReadCnt");
			}
			return ET.ATTR;
		}
	}
	public partial class PushMsgInfoEntity
	{
		public const string PushMsgInfoChange = "PushMsgInfoChange";
		public System.Int64 channelType;
		public System.Int64 timeStamp;
		public System.String msg;
		public SprotoType.SystemMsg systemMsg;
		public System.Int64 uniqueIndex;
		public System.Int64 rid;
		public System.String name;
		public System.String guildName;
		public System.Int64 guildId;
		public System.Int64 headId;
		public System.Int64 headFrameID;
		public System.Int64 toRid;
		public System.Int64 notifyRid;

		public static HashSet<string> updateEntity(PushMsgInfoEntity et ,SprotoType.PushMsgInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasChannelType){
				et.channelType = data.channelType;
				if(ret)ET.ATTR.Add("channelType");
			}
			if(data.HasTimeStamp){
				et.timeStamp = data.timeStamp;
				if(ret)ET.ATTR.Add("timeStamp");
			}
			if(data.HasMsg){
				et.msg = data.msg;
				if(ret)ET.ATTR.Add("msg");
			}
			if(data.HasSystemMsg){
				et.systemMsg = data.systemMsg;
				if(ret)ET.ATTR.Add("systemMsg");
			}
			if(data.HasUniqueIndex){
				et.uniqueIndex = data.uniqueIndex;
				if(ret)ET.ATTR.Add("uniqueIndex");
			}
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasGuildName){
				et.guildName = data.guildName;
				if(ret)ET.ATTR.Add("guildName");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasToRid){
				et.toRid = data.toRid;
				if(ret)ET.ATTR.Add("toRid");
			}
			if(data.HasNotifyRid){
				et.notifyRid = data.notifyRid;
				if(ret)ET.ATTR.Add("notifyRid");
			}
			return ET.ATTR;
		}
	}
	public partial class EmailInfoEntity
	{
		public const string EmailInfoChange = "EmailInfoChange";
		public System.Int64 emailIndex;
		public System.Int64 emailId;
		public System.Int64 sendTime;
		public System.Int64 status;
		public System.Boolean takeEnclosure;
		public System.Boolean isCollect;
		public SprotoType.CollectReport resourceCollectReport;
		public System.Int64 subType;
		public SprotoType.RewardInfo rewards;
		public SprotoType.DiscoverReportInfo discoverReport;
		public System.Int64 acitonForceReturn;
		public System.String battleReportEx;
		public System.Collections.Generic.List<System.String> emailContents;
		public System.Collections.Generic.List<System.String> titleContents;
		public System.Collections.Generic.List<System.String> subTitleContents;
		public SprotoType.GuildEmailInfo guildEmail;
		public SprotoType.SenderInfo senderInfo;
		public SprotoType.ScoutReportInfo scoutReport;
		public System.Collections.Generic.List<System.String> reportSubTile;
		public SprotoType.BattleReportEx battleReportExContent;
		public System.Int64 reportStatus;
		public System.Int64 mainHeroId;
		public System.Collections.Generic.List<SprotoType.RoleList> roleList;

		public static HashSet<string> updateEntity(EmailInfoEntity et ,SprotoType.EmailInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasEmailIndex){
				et.emailIndex = data.emailIndex;
				if(ret)ET.ATTR.Add("emailIndex");
			}
			if(data.HasEmailId){
				et.emailId = data.emailId;
				if(ret)ET.ATTR.Add("emailId");
			}
			if(data.HasSendTime){
				et.sendTime = data.sendTime;
				if(ret)ET.ATTR.Add("sendTime");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasTakeEnclosure){
				et.takeEnclosure = data.takeEnclosure;
				if(ret)ET.ATTR.Add("takeEnclosure");
			}
			if(data.HasIsCollect){
				et.isCollect = data.isCollect;
				if(ret)ET.ATTR.Add("isCollect");
			}
			if(data.HasResourceCollectReport){
				et.resourceCollectReport = data.resourceCollectReport;
				if(ret)ET.ATTR.Add("resourceCollectReport");
			}
			if(data.HasSubType){
				et.subType = data.subType;
				if(ret)ET.ATTR.Add("subType");
			}
			if(data.HasRewards){
				et.rewards = data.rewards;
				if(ret)ET.ATTR.Add("rewards");
			}
			if(data.HasDiscoverReport){
				et.discoverReport = data.discoverReport;
				if(ret)ET.ATTR.Add("discoverReport");
			}
			if(data.HasAcitonForceReturn){
				et.acitonForceReturn = data.acitonForceReturn;
				if(ret)ET.ATTR.Add("acitonForceReturn");
			}
			if(data.HasBattleReportEx){
				et.battleReportEx = data.battleReportEx;
				if(ret)ET.ATTR.Add("battleReportEx");
			}
			if(data.HasEmailContents){
				et.emailContents = data.emailContents;
				if(ret)ET.ATTR.Add("emailContents");
			}
			if(data.HasTitleContents){
				et.titleContents = data.titleContents;
				if(ret)ET.ATTR.Add("titleContents");
			}
			if(data.HasSubTitleContents){
				et.subTitleContents = data.subTitleContents;
				if(ret)ET.ATTR.Add("subTitleContents");
			}
			if(data.HasGuildEmail){
				et.guildEmail = data.guildEmail;
				if(ret)ET.ATTR.Add("guildEmail");
			}
			if(data.HasSenderInfo){
				et.senderInfo = data.senderInfo;
				if(ret)ET.ATTR.Add("senderInfo");
			}
			if(data.HasScoutReport){
				et.scoutReport = data.scoutReport;
				if(ret)ET.ATTR.Add("scoutReport");
			}
			if(data.HasReportSubTile){
				et.reportSubTile = data.reportSubTile;
				if(ret)ET.ATTR.Add("reportSubTile");
			}
			if(data.HasBattleReportExContent){
				et.battleReportExContent = data.battleReportExContent;
				if(ret)ET.ATTR.Add("battleReportExContent");
			}
			if(data.HasReportStatus){
				et.reportStatus = data.reportStatus;
				if(ret)ET.ATTR.Add("reportStatus");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasRoleList){
				et.roleList = data.roleList;
				if(ret)ET.ATTR.Add("roleList");
			}
			return ET.ATTR;
		}
	}
	public partial class ReceiverEntity
	{
		public const string ReceiverChange = "ReceiverChange";
		public System.Int64 rid;
		public System.String gameNode;

		public static HashSet<string> updateEntity(ReceiverEntity et ,SprotoType.Receiver data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasGameNode){
				et.gameNode = data.gameNode;
				if(ret)ET.ATTR.Add("gameNode");
			}
			return ET.ATTR;
		}
	}
	public partial class TroopsEntity
	{
		public const string TroopsChange = "TroopsChange";
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Int64 armyIndex;

		public static HashSet<string> updateEntity(TroopsEntity et ,SprotoType.Expedition_ExpeditionChallenge.request.Troops data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildInfoEntity
	{
		public const string GuildInfoChange = "GuildInfoChange";
		public System.Int64 guildId;
		public System.String name;
		public System.String abbreviationName;
		public System.String notice;
		public System.Boolean needExamine;
		public System.Int64 languageId;
		public System.Collections.Generic.List<System.Int64> signs;
		public System.Int64 leaderRid;
		public System.String leaderName;
		public System.Int64 power;
		public System.Int64 territory;
		public System.Int64 giftLevel;
		public System.Int64 memberNum;
		public System.Int64 memberLimit;
		public System.Boolean isApply;
		public System.Boolean isSameGame;
		public System.Int64 leaderHeadId;
		public System.Int64 leaderHeadFrameID;
		public System.String welcomeEmail;
		public System.Boolean messageBoardRedDot;
		public System.Boolean welcomeEmailFlag;
		public System.Boolean territoryBuildFlag;

		public static HashSet<string> updateEntity(GuildInfoEntity et ,SprotoType.GuildInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasAbbreviationName){
				et.abbreviationName = data.abbreviationName;
				if(ret)ET.ATTR.Add("abbreviationName");
			}
			if(data.HasNotice){
				et.notice = data.notice;
				if(ret)ET.ATTR.Add("notice");
			}
			if(data.HasNeedExamine){
				et.needExamine = data.needExamine;
				if(ret)ET.ATTR.Add("needExamine");
			}
			if(data.HasLanguageId){
				et.languageId = data.languageId;
				if(ret)ET.ATTR.Add("languageId");
			}
			if(data.HasSigns){
				et.signs = data.signs;
				if(ret)ET.ATTR.Add("signs");
			}
			if(data.HasLeaderRid){
				et.leaderRid = data.leaderRid;
				if(ret)ET.ATTR.Add("leaderRid");
			}
			if(data.HasLeaderName){
				et.leaderName = data.leaderName;
				if(ret)ET.ATTR.Add("leaderName");
			}
			if(data.HasPower){
				et.power = data.power;
				if(ret)ET.ATTR.Add("power");
			}
			if(data.HasTerritory){
				et.territory = data.territory;
				if(ret)ET.ATTR.Add("territory");
			}
			if(data.HasGiftLevel){
				et.giftLevel = data.giftLevel;
				if(ret)ET.ATTR.Add("giftLevel");
			}
			if(data.HasMemberNum){
				et.memberNum = data.memberNum;
				if(ret)ET.ATTR.Add("memberNum");
			}
			if(data.HasMemberLimit){
				et.memberLimit = data.memberLimit;
				if(ret)ET.ATTR.Add("memberLimit");
			}
			if(data.HasIsApply){
				et.isApply = data.isApply;
				if(ret)ET.ATTR.Add("isApply");
			}
			if(data.HasIsSameGame){
				et.isSameGame = data.isSameGame;
				if(ret)ET.ATTR.Add("isSameGame");
			}
			if(data.HasLeaderHeadId){
				et.leaderHeadId = data.leaderHeadId;
				if(ret)ET.ATTR.Add("leaderHeadId");
			}
			if(data.HasLeaderHeadFrameID){
				et.leaderHeadFrameID = data.leaderHeadFrameID;
				if(ret)ET.ATTR.Add("leaderHeadFrameID");
			}
			if(data.HasWelcomeEmail){
				et.welcomeEmail = data.welcomeEmail;
				if(ret)ET.ATTR.Add("welcomeEmail");
			}
			if(data.HasMessageBoardRedDot){
				et.messageBoardRedDot = data.messageBoardRedDot;
				if(ret)ET.ATTR.Add("messageBoardRedDot");
			}
			if(data.HasWelcomeEmailFlag){
				et.welcomeEmailFlag = data.welcomeEmailFlag;
				if(ret)ET.ATTR.Add("welcomeEmailFlag");
			}
			if(data.HasTerritoryBuildFlag){
				et.territoryBuildFlag = data.territoryBuildFlag;
				if(ret)ET.ATTR.Add("territoryBuildFlag");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildMemberInfoEntity
	{
		public const string GuildMemberInfoChange = "GuildMemberInfoChange";
		public System.Int64 rid;
		public System.Int64 headId;
		public System.String name;
		public System.Int64 combatPower;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.KillCount> killCount;
		public System.Int64 guildJob;
		public System.Boolean online;
		public System.Int64 headFrameID;
		public System.Int64 cityObjectIndex;

		public static HashSet<string> updateEntity(GuildMemberInfoEntity et ,SprotoType.GuildMemberInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasCombatPower){
				et.combatPower = data.combatPower;
				if(ret)ET.ATTR.Add("combatPower");
			}
			if(data.HasKillCount){

				if (et.killCount == null) {
					 et.killCount = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.KillCount>();
				}
				foreach(var item in data.killCount){ 
					if(et.killCount.ContainsKey(item.Key)){
						et.killCount[item.Key] = item.Value;
					}else{
						et.killCount.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("killCount"); 
			}
			if(data.HasGuildJob){
				et.guildJob = data.guildJob;
				if(ret)ET.ATTR.Add("guildJob");
			}
			if(data.HasOnline){
				et.online = data.online;
				if(ret)ET.ATTR.Add("online");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasCityObjectIndex){
				et.cityObjectIndex = data.cityObjectIndex;
				if(ret)ET.ATTR.Add("cityObjectIndex");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildOfficerInfoEntity
	{
		public const string GuildOfficerInfoChange = "GuildOfficerInfoChange";
		public System.Int64 officerId;
		public System.Int64 rid;
		public System.Int64 appointTime;

		public static HashSet<string> updateEntity(GuildOfficerInfoEntity et ,SprotoType.GuildOfficerInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasOfficerId){
				et.officerId = data.officerId;
				if(ret)ET.ATTR.Add("officerId");
			}
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasAppointTime){
				et.appointTime = data.appointTime;
				if(ret)ET.ATTR.Add("appointTime");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildApplyInfoEntity
	{
		public const string GuildApplyInfoChange = "GuildApplyInfoChange";
		public System.Int64 rid;
		public System.String name;
		public System.Int64 headId;
		public System.Int64 combatPower;
		public System.Int64 killCount;
		public System.Int64 applyTime;
		public System.Int64 headFrameID;

		public static HashSet<string> updateEntity(GuildApplyInfoEntity et ,SprotoType.GuildApplyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasCombatPower){
				et.combatPower = data.combatPower;
				if(ret)ET.ATTR.Add("combatPower");
			}
			if(data.HasKillCount){
				et.killCount = data.killCount;
				if(ret)ET.ATTR.Add("killCount");
			}
			if(data.HasApplyTime){
				et.applyTime = data.applyTime;
				if(ret)ET.ATTR.Add("applyTime");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildResourceCenterInfoEntity
	{
		public const string GuildResourceCenterInfoChange = "GuildResourceCenterInfoChange";
		public SprotoType.GuildBuildInfo resourceCenter;
		public System.Int64 resource;
		public System.Int64 collectTime;
		public System.Int64 collectSpeed;

		public static HashSet<string> updateEntity(GuildResourceCenterInfoEntity et ,SprotoType.GuildResourceCenterInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasResourceCenter){
				et.resourceCenter = data.resourceCenter;
				if(ret)ET.ATTR.Add("resourceCenter");
			}
			if(data.HasResource){
				et.resource = data.resource;
				if(ret)ET.ATTR.Add("resource");
			}
			if(data.HasCollectTime){
				et.collectTime = data.collectTime;
				if(ret)ET.ATTR.Add("collectTime");
			}
			if(data.HasCollectSpeed){
				et.collectSpeed = data.collectSpeed;
				if(ret)ET.ATTR.Add("collectSpeed");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildFlagInfoEntity
	{
		public const string GuildFlagInfoChange = "GuildFlagInfoChange";
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.GuildBuildInfo> flags;
		public System.Int64 flagNum;
		public System.Int64 flagLimit;

		public static HashSet<string> updateEntity(GuildFlagInfoEntity et ,SprotoType.GuildFlagInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasFlags){

				if (et.flags == null) {
					 et.flags = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.GuildBuildInfo>();
				}
				foreach(var item in data.flags){ 
					if(et.flags.ContainsKey(item.Key)){
						et.flags[item.Key] = item.Value;
					}else{
						et.flags.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("flags"); 
			}
			if(data.HasFlagNum){
				et.flagNum = data.flagNum;
				if(ret)ET.ATTR.Add("flagNum");
			}
			if(data.HasFlagLimit){
				et.flagLimit = data.flagLimit;
				if(ret)ET.ATTR.Add("flagLimit");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildResourcePointInfoEntity
	{
		public const string GuildResourcePointInfoChange = "GuildResourcePointInfoChange";
		public System.Int64 foodPoint;
		public System.Int64 woodPoint;
		public System.Int64 stonePoint;
		public System.Int64 goldPoint;

		public static HashSet<string> updateEntity(GuildResourcePointInfoEntity et ,SprotoType.GuildResourcePointInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasFoodPoint){
				et.foodPoint = data.foodPoint;
				if(ret)ET.ATTR.Add("foodPoint");
			}
			if(data.HasWoodPoint){
				et.woodPoint = data.woodPoint;
				if(ret)ET.ATTR.Add("woodPoint");
			}
			if(data.HasStonePoint){
				et.stonePoint = data.stonePoint;
				if(ret)ET.ATTR.Add("stonePoint");
			}
			if(data.HasGoldPoint){
				et.goldPoint = data.goldPoint;
				if(ret)ET.ATTR.Add("goldPoint");
			}
			return ET.ATTR;
		}
	}
	public partial class RoleTerritoryGainInfoEntity
	{
		public const string RoleTerritoryGainInfoChange = "RoleTerritoryGainInfoChange";
		public System.Int64 type;
		public System.Int64 num;
		public System.Int64 territoryTime;
		public System.Int64 limit;

		public static HashSet<string> updateEntity(RoleTerritoryGainInfoEntity et ,SprotoType.RoleTerritoryGainInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			if(data.HasTerritoryTime){
				et.territoryTime = data.territoryTime;
				if(ret)ET.ATTR.Add("territoryTime");
			}
			if(data.HasLimit){
				et.limit = data.limit;
				if(ret)ET.ATTR.Add("limit");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildDepotInfoEntity
	{
		public const string GuildDepotInfoChange = "GuildDepotInfoChange";
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.GuildCurrencyInfo> currencies;
		public System.Collections.Generic.List<SprotoType.GuildConsumeRecordInfo> consumeRecords;

		public static HashSet<string> updateEntity(GuildDepotInfoEntity et ,SprotoType.GuildDepotInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasCurrencies){

				if (et.currencies == null) {
					 et.currencies = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.GuildCurrencyInfo>();
				}
				foreach(var item in data.currencies){ 
					if(et.currencies.ContainsKey(item.Key)){
						et.currencies[item.Key] = item.Value;
					}else{
						et.currencies.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("currencies"); 
			}
			if(data.HasConsumeRecords){
				et.consumeRecords = data.consumeRecords;
				if(ret)ET.ATTR.Add("consumeRecords");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildTreasureInfoEntity
	{
		public const string GuildTreasureInfoChange = "GuildTreasureInfoChange";
		public System.Int64 giftIndex;
		public System.Int64 treasureId;
		public System.Int64 sendTime;

		public static HashSet<string> updateEntity(GuildTreasureInfoEntity et ,SprotoType.GuildTreasureInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGiftIndex){
				et.giftIndex = data.giftIndex;
				if(ret)ET.ATTR.Add("giftIndex");
			}
			if(data.HasTreasureId){
				et.treasureId = data.treasureId;
				if(ret)ET.ATTR.Add("treasureId");
			}
			if(data.HasSendTime){
				et.sendTime = data.sendTime;
				if(ret)ET.ATTR.Add("sendTime");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildGiftInfoEntity
	{
		public const string GuildGiftInfoChange = "GuildGiftInfoChange";
		public System.Int64 giftIndex;
		public System.Int64 giftId;
		public System.Int64 status;
		public System.Int64 sendTime;
		public System.Int64 sendType;
		public System.String buyRoleName;
		public System.Int64 itemId;
		public System.Int64 itemNum;
		public System.Int64 packageNameId;

		public static HashSet<string> updateEntity(GuildGiftInfoEntity et ,SprotoType.GuildGiftInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGiftIndex){
				et.giftIndex = data.giftIndex;
				if(ret)ET.ATTR.Add("giftIndex");
			}
			if(data.HasGiftId){
				et.giftId = data.giftId;
				if(ret)ET.ATTR.Add("giftId");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasSendTime){
				et.sendTime = data.sendTime;
				if(ret)ET.ATTR.Add("sendTime");
			}
			if(data.HasSendType){
				et.sendType = data.sendType;
				if(ret)ET.ATTR.Add("sendType");
			}
			if(data.HasBuyRoleName){
				et.buyRoleName = data.buyRoleName;
				if(ret)ET.ATTR.Add("buyRoleName");
			}
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasItemNum){
				et.itemNum = data.itemNum;
				if(ret)ET.ATTR.Add("itemNum");
			}
			if(data.HasPackageNameId){
				et.packageNameId = data.packageNameId;
				if(ret)ET.ATTR.Add("packageNameId");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildHolyLandInfoEntity
	{
		public const string GuildHolyLandInfoChange = "GuildHolyLandInfoChange";
		public System.Int64 strongHoldId;
		public System.Int64 status;
		public SprotoType.PosInfo pos;

		public static HashSet<string> updateEntity(GuildHolyLandInfoEntity et ,SprotoType.GuildHolyLandInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasStrongHoldId){
				et.strongHoldId = data.strongHoldId;
				if(ret)ET.ATTR.Add("strongHoldId");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			return ET.ATTR;
		}
	}
	public partial class MemberPosInfoEntity
	{
		public const string MemberPosInfoChange = "MemberPosInfoChange";
		public System.Int64 rid;
		public SprotoType.PosInfo pos;

		public static HashSet<string> updateEntity(MemberPosInfoEntity et ,SprotoType.MemberPosInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			return ET.ATTR;
		}
	}
	public partial class RoleInfoEntity
	{
		public const string RoleInfoChange = "RoleInfoChange";
		public System.Int64 rid;
		public System.String name;
		public SprotoType.PosInfo pos;
		public System.Int64 country;
		public System.Int64 headId;
		public System.Int64 level;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo> buildQueue;
		public System.Int64 actionForce;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Int64 food;
		public System.Int64 wood;
		public System.Int64 stone;
		public System.Int64 gold;
		public System.Int64 denar;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo> armyQueue;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.TechnologyInfo> technologies;
		public System.Int64 buildVersion;
		public System.Int64 mainLineTaskId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.FinishTaskInfo> finishSideTasks;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.TaskStatistics> taskStatisticsSum;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.TaskStatistics> taskStatisticsDaily;
		public SprotoType.QueueInfo technologyQueue;
		public SprotoType.QueueInfo treatmentQueue;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> seriousInjured;
		public System.Int64 historyPower;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleStatistics> roleStatistics;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.SoldierKillInfo> soldierKills;
		public System.Int64 chapterId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ChapterTaskInfo> chapterTasks;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.DenseFogInfo> denseFog;
		public System.Int64 serverTime;
		public System.Int64 noviceGuideStep;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ReinforceArmyInfo> reinforces;
		public System.Boolean situStation;
		public System.Int64 barbarianLevel;
		public System.Int64 emailVersion;
		public System.Int64 lastActionForceTime;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.VillageCaveInfo> villageCaves;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.KillCount> killCount;
		public System.Int64 combatPower;
		public System.Int64 createTime;
		public System.Boolean isChangeAge;
		public System.Int64 activePoint;
		public System.Collections.Generic.List<System.Int64> activePointRewards;
		public System.Int64 silverFreeCount;
		public System.Int64 openNextSilverTime;
		public System.Int64 goldFreeCount;
		public System.Int64 addGoldFreeAddTime;
		public System.Int64 guildId;
		public System.Boolean guildInvite;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Int64 guildPoint;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.CityBuff> cityBuff;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ActivityTimeInfo> activityTimeInfo;
		public System.Int64 guildHelpPoint;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ChatReadedUniqueIndex> maxChatUniqueIndex;
		public System.Collections.Generic.List<System.Int64> headList;
		public System.Collections.Generic.List<System.Int64> headFrameList;
		public System.Int64 headFrameID;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ChatNoDisturbInfo> chatNoDisturbInfo;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity> activity;
		public System.Int64 guardTowerHp;
		public SprotoType.MysteryStore mysteryStore;
		public System.Int64 vip;
		public System.Int64 continuousLoginDay;
		public System.Boolean vipFreeBox;
		public System.Collections.Generic.List<System.Int64> vipSpecialBox;
		public System.Boolean vipExpFlag;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Recharge> recharge;
		public System.Int64 riseRoad;
		public System.Boolean freeDaily;
		public System.String guildName;
		public System.Boolean rechargeFirst;
		public System.Collections.Generic.List<System.Int64> dailyPackage;
		public System.Collections.Generic.List<System.Int64> riseRoadPackage;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RechargeSale> rechargeSale;
		public System.Boolean growthFund;
		public System.Collections.Generic.List<System.Int64> growthFundReward;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.Supply> supply;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.LimitTimePackage> limitTimePackage;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.VipStore> vipStore;
		public SprotoType.Expedition expedition;
		public System.Int64 expeditionCoin;
		public System.Int64 buyActionForceCount;
		public SprotoType.QueueInfo materialQueue;
		public System.String guildAbbName;
		public System.Int64 lastGuildDonateTime;
		public System.Int64 guildDonateCostDenar;
		public System.Int64 joinGuildTime;
		public System.Boolean praiseFlag;
		public System.Int64 silence;
		public System.Int64 gameId;
		public System.Int64 noviceGuideStepEx;
		public System.Int64 emailSendCntPerHour;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> historySoldiers;
		public System.Boolean denseFogOpenFlag;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.PushSetting> pushSetting;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ExpeditionInfo> expeditionInfo;
		public System.Int64 combatPowerType;
		public System.Int64 eventTrancking;
		public System.String gameNode;
		public System.Int64 mapIndex;
		public System.Int64 itemAddTroopsCapacity;
		public System.Int64 itemAddTroopsCapacityCount;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.MapMarkerInfo> markers;
		public System.Int64 lastLoginTime;
		public System.Int64 activityActivePoint;
		public System.Collections.Generic.List<System.String> abTestGroup;

		public static HashSet<string> updateEntity(RoleInfoEntity et ,SprotoType.RoleInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasCountry){
				et.country = data.country;
				if(ret)ET.ATTR.Add("country");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasBuildQueue){

				if (et.buildQueue == null) {
					 et.buildQueue = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo>();
				}
				foreach(var item in data.buildQueue){ 
					if(et.buildQueue.ContainsKey(item.Key)){
						et.buildQueue[item.Key] = item.Value;
					}else{
						et.buildQueue.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("buildQueue"); 
			}
			if(data.HasActionForce){
				et.actionForce = data.actionForce;
				if(ret)ET.ATTR.Add("actionForce");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasFood){
				et.food = data.food;
				if(ret)ET.ATTR.Add("food");
			}
			if(data.HasWood){
				et.wood = data.wood;
				if(ret)ET.ATTR.Add("wood");
			}
			if(data.HasStone){
				et.stone = data.stone;
				if(ret)ET.ATTR.Add("stone");
			}
			if(data.HasGold){
				et.gold = data.gold;
				if(ret)ET.ATTR.Add("gold");
			}
			if(data.HasDenar){
				et.denar = data.denar;
				if(ret)ET.ATTR.Add("denar");
			}
			if(data.HasArmyQueue){

				if (et.armyQueue == null) {
					 et.armyQueue = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo>();
				}
				foreach(var item in data.armyQueue){ 
					if(et.armyQueue.ContainsKey(item.Key)){
						et.armyQueue[item.Key] = item.Value;
					}else{
						et.armyQueue.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("armyQueue"); 
			}
			if(data.HasTechnologies){

				if (et.technologies == null) {
					 et.technologies = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.TechnologyInfo>();
				}
				foreach(var item in data.technologies){ 
					if(et.technologies.ContainsKey(item.Key)){
						et.technologies[item.Key] = item.Value;
					}else{
						et.technologies.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("technologies"); 
			}
			if(data.HasBuildVersion){
				et.buildVersion = data.buildVersion;
				if(ret)ET.ATTR.Add("buildVersion");
			}
			if(data.HasMainLineTaskId){
				et.mainLineTaskId = data.mainLineTaskId;
				if(ret)ET.ATTR.Add("mainLineTaskId");
			}
			if(data.HasFinishSideTasks){

				if (et.finishSideTasks == null) {
					 et.finishSideTasks = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.FinishTaskInfo>();
				}
				foreach(var item in data.finishSideTasks){ 
					if(et.finishSideTasks.ContainsKey(item.Key)){
						et.finishSideTasks[item.Key] = item.Value;
					}else{
						et.finishSideTasks.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("finishSideTasks"); 
			}
			if(data.HasTaskStatisticsSum){

				if (et.taskStatisticsSum == null) {
					 et.taskStatisticsSum = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.TaskStatistics>();
				}
				foreach(var item in data.taskStatisticsSum){ 
					if(et.taskStatisticsSum.ContainsKey(item.Key)){
						et.taskStatisticsSum[item.Key] = item.Value;
					}else{
						et.taskStatisticsSum.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("taskStatisticsSum"); 
			}
			if(data.HasTaskStatisticsDaily){

				if (et.taskStatisticsDaily == null) {
					 et.taskStatisticsDaily = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.TaskStatistics>();
				}
				foreach(var item in data.taskStatisticsDaily){ 
					if(et.taskStatisticsDaily.ContainsKey(item.Key)){
						et.taskStatisticsDaily[item.Key] = item.Value;
					}else{
						et.taskStatisticsDaily.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("taskStatisticsDaily"); 
			}
			if(data.HasTechnologyQueue){
				et.technologyQueue = data.technologyQueue;
				if(ret)ET.ATTR.Add("technologyQueue");
			}
			if(data.HasTreatmentQueue){
				et.treatmentQueue = data.treatmentQueue;
				if(ret)ET.ATTR.Add("treatmentQueue");
			}
			if(data.HasSeriousInjured){

				if (et.seriousInjured == null) {
					 et.seriousInjured = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.seriousInjured){ 
					if(et.seriousInjured.ContainsKey(item.Key)){
						et.seriousInjured[item.Key] = item.Value;
					}else{
						et.seriousInjured.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("seriousInjured"); 
			}
			if(data.HasHistoryPower){
				et.historyPower = data.historyPower;
				if(ret)ET.ATTR.Add("historyPower");
			}
			if(data.HasRoleStatistics){

				if (et.roleStatistics == null) {
					 et.roleStatistics = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleStatistics>();
				}
				foreach(var item in data.roleStatistics){ 
					if(et.roleStatistics.ContainsKey(item.Key)){
						et.roleStatistics[item.Key] = item.Value;
					}else{
						et.roleStatistics.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("roleStatistics"); 
			}
			if(data.HasSoldierKills){

				if (et.soldierKills == null) {
					 et.soldierKills = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.SoldierKillInfo>();
				}
				foreach(var item in data.soldierKills){ 
					if(et.soldierKills.ContainsKey(item.Key)){
						et.soldierKills[item.Key] = item.Value;
					}else{
						et.soldierKills.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldierKills"); 
			}
			if(data.HasChapterId){
				et.chapterId = data.chapterId;
				if(ret)ET.ATTR.Add("chapterId");
			}
			if(data.HasChapterTasks){

				if (et.chapterTasks == null) {
					 et.chapterTasks = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ChapterTaskInfo>();
				}
				foreach(var item in data.chapterTasks){ 
					if(et.chapterTasks.ContainsKey(item.Key)){
						et.chapterTasks[item.Key] = item.Value;
					}else{
						et.chapterTasks.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("chapterTasks"); 
			}
			if(data.HasDenseFog){

				if (et.denseFog == null) {
					 et.denseFog = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.DenseFogInfo>();
				}
				foreach(var item in data.denseFog){ 
					if(et.denseFog.ContainsKey(item.Key)){
						et.denseFog[item.Key] = item.Value;
					}else{
						et.denseFog.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("denseFog"); 
			}
			if(data.HasServerTime){
				et.serverTime = data.serverTime;
				if(ret)ET.ATTR.Add("serverTime");
			}
			if(data.HasNoviceGuideStep){
				et.noviceGuideStep = data.noviceGuideStep;
				if(ret)ET.ATTR.Add("noviceGuideStep");
			}
			if(data.HasReinforces){

				if (et.reinforces == null) {
					 et.reinforces = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ReinforceArmyInfo>();
				}
				foreach(var item in data.reinforces){ 
					if(et.reinforces.ContainsKey(item.Key)){
						et.reinforces[item.Key] = item.Value;
					}else{
						et.reinforces.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("reinforces"); 
			}
			if(data.HasSituStation){
				et.situStation = data.situStation;
				if(ret)ET.ATTR.Add("situStation");
			}
			if(data.HasBarbarianLevel){
				et.barbarianLevel = data.barbarianLevel;
				if(ret)ET.ATTR.Add("barbarianLevel");
			}
			if(data.HasEmailVersion){
				et.emailVersion = data.emailVersion;
				if(ret)ET.ATTR.Add("emailVersion");
			}
			if(data.HasLastActionForceTime){
				et.lastActionForceTime = data.lastActionForceTime;
				if(ret)ET.ATTR.Add("lastActionForceTime");
			}
			if(data.HasVillageCaves){

				if (et.villageCaves == null) {
					 et.villageCaves = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.VillageCaveInfo>();
				}
				foreach(var item in data.villageCaves){ 
					if(et.villageCaves.ContainsKey(item.Key)){
						et.villageCaves[item.Key] = item.Value;
					}else{
						et.villageCaves.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("villageCaves"); 
			}
			if(data.HasKillCount){

				if (et.killCount == null) {
					 et.killCount = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.KillCount>();
				}
				foreach(var item in data.killCount){ 
					if(et.killCount.ContainsKey(item.Key)){
						et.killCount[item.Key] = item.Value;
					}else{
						et.killCount.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("killCount"); 
			}
			if(data.HasCombatPower){
				et.combatPower = data.combatPower;
				if(ret)ET.ATTR.Add("combatPower");
			}
			if(data.HasCreateTime){
				et.createTime = data.createTime;
				if(ret)ET.ATTR.Add("createTime");
			}
			if(data.HasIsChangeAge){
				et.isChangeAge = data.isChangeAge;
				if(ret)ET.ATTR.Add("isChangeAge");
			}
			if(data.HasActivePoint){
				et.activePoint = data.activePoint;
				if(ret)ET.ATTR.Add("activePoint");
			}
			if(data.HasActivePointRewards){
				et.activePointRewards = data.activePointRewards;
				if(ret)ET.ATTR.Add("activePointRewards");
			}
			if(data.HasSilverFreeCount){
				et.silverFreeCount = data.silverFreeCount;
				if(ret)ET.ATTR.Add("silverFreeCount");
			}
			if(data.HasOpenNextSilverTime){
				et.openNextSilverTime = data.openNextSilverTime;
				if(ret)ET.ATTR.Add("openNextSilverTime");
			}
			if(data.HasGoldFreeCount){
				et.goldFreeCount = data.goldFreeCount;
				if(ret)ET.ATTR.Add("goldFreeCount");
			}
			if(data.HasAddGoldFreeAddTime){
				et.addGoldFreeAddTime = data.addGoldFreeAddTime;
				if(ret)ET.ATTR.Add("addGoldFreeAddTime");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasGuildInvite){
				et.guildInvite = data.guildInvite;
				if(ret)ET.ATTR.Add("guildInvite");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasGuildPoint){
				et.guildPoint = data.guildPoint;
				if(ret)ET.ATTR.Add("guildPoint");
			}
			if(data.HasCityBuff){

				if (et.cityBuff == null) {
					 et.cityBuff = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.CityBuff>();
				}
				foreach(var item in data.cityBuff){ 
					if(et.cityBuff.ContainsKey(item.Key)){
						et.cityBuff[item.Key] = item.Value;
					}else{
						et.cityBuff.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("cityBuff"); 
			}
			if(data.HasActivityTimeInfo){

				if (et.activityTimeInfo == null) {
					 et.activityTimeInfo = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ActivityTimeInfo>();
				}
				foreach(var item in data.activityTimeInfo){ 
					if(et.activityTimeInfo.ContainsKey(item.Key)){
						et.activityTimeInfo[item.Key] = item.Value;
					}else{
						et.activityTimeInfo.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("activityTimeInfo"); 
			}
			if(data.HasGuildHelpPoint){
				et.guildHelpPoint = data.guildHelpPoint;
				if(ret)ET.ATTR.Add("guildHelpPoint");
			}
			if(data.HasMaxChatUniqueIndex){

				if (et.maxChatUniqueIndex == null) {
					 et.maxChatUniqueIndex = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ChatReadedUniqueIndex>();
				}
				foreach(var item in data.maxChatUniqueIndex){ 
					if(et.maxChatUniqueIndex.ContainsKey(item.Key)){
						et.maxChatUniqueIndex[item.Key] = item.Value;
					}else{
						et.maxChatUniqueIndex.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("maxChatUniqueIndex"); 
			}
			if(data.HasHeadList){
				et.headList = data.headList;
				if(ret)ET.ATTR.Add("headList");
			}
			if(data.HasHeadFrameList){
				et.headFrameList = data.headFrameList;
				if(ret)ET.ATTR.Add("headFrameList");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasChatNoDisturbInfo){

				if (et.chatNoDisturbInfo == null) {
					 et.chatNoDisturbInfo = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ChatNoDisturbInfo>();
				}
				foreach(var item in data.chatNoDisturbInfo){ 
					if(et.chatNoDisturbInfo.ContainsKey(item.Key)){
						et.chatNoDisturbInfo[item.Key] = item.Value;
					}else{
						et.chatNoDisturbInfo.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("chatNoDisturbInfo"); 
			}
			if(data.HasActivity){

				if (et.activity == null) {
					 et.activity = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Activity>();
				}
				foreach(var item in data.activity){ 
					if(et.activity.ContainsKey(item.Key)){
						et.activity[item.Key] = item.Value;
					}else{
						et.activity.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("activity"); 
			}
			if(data.HasGuardTowerHp){
				et.guardTowerHp = data.guardTowerHp;
				if(ret)ET.ATTR.Add("guardTowerHp");
			}
			if(data.HasMysteryStore){
				et.mysteryStore = data.mysteryStore;
				if(ret)ET.ATTR.Add("mysteryStore");
			}
			if(data.HasVip){
				et.vip = data.vip;
				if(ret)ET.ATTR.Add("vip");
			}
			if(data.HasContinuousLoginDay){
				et.continuousLoginDay = data.continuousLoginDay;
				if(ret)ET.ATTR.Add("continuousLoginDay");
			}
			if(data.HasVipFreeBox){
				et.vipFreeBox = data.vipFreeBox;
				if(ret)ET.ATTR.Add("vipFreeBox");
			}
			if(data.HasVipSpecialBox){
				et.vipSpecialBox = data.vipSpecialBox;
				if(ret)ET.ATTR.Add("vipSpecialBox");
			}
			if(data.HasVipExpFlag){
				et.vipExpFlag = data.vipExpFlag;
				if(ret)ET.ATTR.Add("vipExpFlag");
			}
			if(data.HasRecharge){

				if (et.recharge == null) {
					 et.recharge = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Recharge>();
				}
				foreach(var item in data.recharge){ 
					if(et.recharge.ContainsKey(item.Key)){
						et.recharge[item.Key] = item.Value;
					}else{
						et.recharge.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("recharge"); 
			}
			if(data.HasRiseRoad){
				et.riseRoad = data.riseRoad;
				if(ret)ET.ATTR.Add("riseRoad");
			}
			if(data.HasFreeDaily){
				et.freeDaily = data.freeDaily;
				if(ret)ET.ATTR.Add("freeDaily");
			}
			if(data.HasGuildName){
				et.guildName = data.guildName;
				if(ret)ET.ATTR.Add("guildName");
			}
			if(data.HasRechargeFirst){
				et.rechargeFirst = data.rechargeFirst;
				if(ret)ET.ATTR.Add("rechargeFirst");
			}
			if(data.HasDailyPackage){
				et.dailyPackage = data.dailyPackage;
				if(ret)ET.ATTR.Add("dailyPackage");
			}
			if(data.HasRiseRoadPackage){
				et.riseRoadPackage = data.riseRoadPackage;
				if(ret)ET.ATTR.Add("riseRoadPackage");
			}
			if(data.HasRechargeSale){

				if (et.rechargeSale == null) {
					 et.rechargeSale = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RechargeSale>();
				}
				foreach(var item in data.rechargeSale){ 
					if(et.rechargeSale.ContainsKey(item.Key)){
						et.rechargeSale[item.Key] = item.Value;
					}else{
						et.rechargeSale.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("rechargeSale"); 
			}
			if(data.HasGrowthFund){
				et.growthFund = data.growthFund;
				if(ret)ET.ATTR.Add("growthFund");
			}
			if(data.HasGrowthFundReward){
				et.growthFundReward = data.growthFundReward;
				if(ret)ET.ATTR.Add("growthFundReward");
			}
			if(data.HasSupply){

				if (et.supply == null) {
					 et.supply = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.Supply>();
				}
				foreach(var item in data.supply){ 
					if(et.supply.ContainsKey(item.Key)){
						et.supply[item.Key] = item.Value;
					}else{
						et.supply.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("supply"); 
			}
			if(data.HasLimitTimePackage){

				if (et.limitTimePackage == null) {
					 et.limitTimePackage = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.LimitTimePackage>();
				}
				foreach(var item in data.limitTimePackage){ 
					if(et.limitTimePackage.ContainsKey(item.Key)){
						et.limitTimePackage[item.Key] = item.Value;
					}else{
						et.limitTimePackage.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("limitTimePackage"); 
			}
			if(data.HasVipStore){

				if (et.vipStore == null) {
					 et.vipStore = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.VipStore>();
				}
				foreach(var item in data.vipStore){ 
					if(et.vipStore.ContainsKey(item.Key)){
						et.vipStore[item.Key] = item.Value;
					}else{
						et.vipStore.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("vipStore"); 
			}
			if(data.HasExpedition){
				et.expedition = data.expedition;
				if(ret)ET.ATTR.Add("expedition");
			}
			if(data.HasExpeditionCoin){
				et.expeditionCoin = data.expeditionCoin;
				if(ret)ET.ATTR.Add("expeditionCoin");
			}
			if(data.HasBuyActionForceCount){
				et.buyActionForceCount = data.buyActionForceCount;
				if(ret)ET.ATTR.Add("buyActionForceCount");
			}
			if(data.HasMaterialQueue){
				et.materialQueue = data.materialQueue;
				if(ret)ET.ATTR.Add("materialQueue");
			}
			if(data.HasGuildAbbName){
				et.guildAbbName = data.guildAbbName;
				if(ret)ET.ATTR.Add("guildAbbName");
			}
			if(data.HasLastGuildDonateTime){
				et.lastGuildDonateTime = data.lastGuildDonateTime;
				if(ret)ET.ATTR.Add("lastGuildDonateTime");
			}
			if(data.HasGuildDonateCostDenar){
				et.guildDonateCostDenar = data.guildDonateCostDenar;
				if(ret)ET.ATTR.Add("guildDonateCostDenar");
			}
			if(data.HasJoinGuildTime){
				et.joinGuildTime = data.joinGuildTime;
				if(ret)ET.ATTR.Add("joinGuildTime");
			}
			if(data.HasPraiseFlag){
				et.praiseFlag = data.praiseFlag;
				if(ret)ET.ATTR.Add("praiseFlag");
			}
			if(data.HasSilence){
				et.silence = data.silence;
				if(ret)ET.ATTR.Add("silence");
			}
			if(data.HasGameId){
				et.gameId = data.gameId;
				if(ret)ET.ATTR.Add("gameId");
			}
			if(data.HasNoviceGuideStepEx){
				et.noviceGuideStepEx = data.noviceGuideStepEx;
				if(ret)ET.ATTR.Add("noviceGuideStepEx");
			}
			if(data.HasEmailSendCntPerHour){
				et.emailSendCntPerHour = data.emailSendCntPerHour;
				if(ret)ET.ATTR.Add("emailSendCntPerHour");
			}
			if(data.HasHistorySoldiers){

				if (et.historySoldiers == null) {
					 et.historySoldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.historySoldiers){ 
					if(et.historySoldiers.ContainsKey(item.Key)){
						et.historySoldiers[item.Key] = item.Value;
					}else{
						et.historySoldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("historySoldiers"); 
			}
			if(data.HasDenseFogOpenFlag){
				et.denseFogOpenFlag = data.denseFogOpenFlag;
				if(ret)ET.ATTR.Add("denseFogOpenFlag");
			}
			if(data.HasPushSetting){

				if (et.pushSetting == null) {
					 et.pushSetting = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.PushSetting>();
				}
				foreach(var item in data.pushSetting){ 
					if(et.pushSetting.ContainsKey(item.Key)){
						et.pushSetting[item.Key] = item.Value;
					}else{
						et.pushSetting.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("pushSetting"); 
			}
			if(data.HasExpeditionInfo){

				if (et.expeditionInfo == null) {
					 et.expeditionInfo = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ExpeditionInfo>();
				}
				foreach(var item in data.expeditionInfo){ 
					if(et.expeditionInfo.ContainsKey(item.Key)){
						et.expeditionInfo[item.Key] = item.Value;
					}else{
						et.expeditionInfo.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("expeditionInfo"); 
			}
			if(data.HasCombatPowerType){
				et.combatPowerType = data.combatPowerType;
				if(ret)ET.ATTR.Add("combatPowerType");
			}
			if(data.HasEventTrancking){
				et.eventTrancking = data.eventTrancking;
				if(ret)ET.ATTR.Add("eventTrancking");
			}
			if(data.HasGameNode){
				et.gameNode = data.gameNode;
				if(ret)ET.ATTR.Add("gameNode");
			}
			if(data.HasMapIndex){
				et.mapIndex = data.mapIndex;
				if(ret)ET.ATTR.Add("mapIndex");
			}
			if(data.HasItemAddTroopsCapacity){
				et.itemAddTroopsCapacity = data.itemAddTroopsCapacity;
				if(ret)ET.ATTR.Add("itemAddTroopsCapacity");
			}
			if(data.HasItemAddTroopsCapacityCount){
				et.itemAddTroopsCapacityCount = data.itemAddTroopsCapacityCount;
				if(ret)ET.ATTR.Add("itemAddTroopsCapacityCount");
			}
			if(data.HasMarkers){

				if (et.markers == null) {
					 et.markers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.MapMarkerInfo>();
				}
				foreach(var item in data.markers){ 
					if(et.markers.ContainsKey(item.Key)){
						et.markers[item.Key] = item.Value;
					}else{
						et.markers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("markers"); 
			}
			if(data.HasLastLoginTime){
				et.lastLoginTime = data.lastLoginTime;
				if(ret)ET.ATTR.Add("lastLoginTime");
			}
			if(data.HasActivityActivePoint){
				et.activityActivePoint = data.activityActivePoint;
				if(ret)ET.ATTR.Add("activityActivePoint");
			}
			if(data.HasAbTestGroup){
				et.abTestGroup = data.abTestGroup;
				if(ret)ET.ATTR.Add("abTestGroup");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildRequestHelpInfoEntity
	{
		public const string GuildRequestHelpInfoChange = "GuildRequestHelpInfoChange";
		public System.Int64 index;
		public System.Int64 rid;
		public System.Int64 type;
		public System.Collections.Generic.List<System.Int64> args;
		public System.Int64 helpNum;
		public System.Int64 helpLimit;
		public System.Int64 reduceTime;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.HelpInfo> helps;
		public System.Int64 needTime;
		public System.Int64 queueIndex;

		public static HashSet<string> updateEntity(GuildRequestHelpInfoEntity et ,SprotoType.GuildRequestHelpInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasArgs){
				et.args = data.args;
				if(ret)ET.ATTR.Add("args");
			}
			if(data.HasHelpNum){
				et.helpNum = data.helpNum;
				if(ret)ET.ATTR.Add("helpNum");
			}
			if(data.HasHelpLimit){
				et.helpLimit = data.helpLimit;
				if(ret)ET.ATTR.Add("helpLimit");
			}
			if(data.HasReduceTime){
				et.reduceTime = data.reduceTime;
				if(ret)ET.ATTR.Add("reduceTime");
			}
			if(data.HasHelps){

				if (et.helps == null) {
					 et.helps = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.HelpInfo>();
				}
				foreach(var item in data.helps){ 
					if(et.helps.ContainsKey(item.Key)){
						et.helps[item.Key] = item.Value;
					}else{
						et.helps.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("helps"); 
			}
			if(data.HasNeedTime){
				et.needTime = data.needTime;
				if(ret)ET.ATTR.Add("needTime");
			}
			if(data.HasQueueIndex){
				et.queueIndex = data.queueIndex;
				if(ret)ET.ATTR.Add("queueIndex");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildTechnologyInfoEntity
	{
		public const string GuildTechnologyInfoChange = "GuildTechnologyInfoChange";
		public System.Int64 type;
		public System.Int64 level;

		public static HashSet<string> updateEntity(GuildTechnologyInfoEntity et ,SprotoType.GuildTechnologyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			return ET.ATTR;
		}
	}
	public partial class itemEntity
	{
		public const string itemChange = "itemChange";
		public System.Int64 idItemType;
		public System.Int64 nCount;

		public static HashSet<string> updateEntity(itemEntity et ,SprotoType.item data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIdItemType){
				et.idItemType = data.idItemType;
				if(ret)ET.ATTR.Add("idItemType");
			}
			if(data.HasNCount){
				et.nCount = data.nCount;
				if(ret)ET.ATTR.Add("nCount");
			}
			return ET.ATTR;
		}
	}
	public partial class HeroInfoEntity
	{
		public const string HeroInfoChange = "HeroInfoChange";
		public System.Int64 heroId;
		public System.Int64 star;
		public System.Int64 starExp;
		public System.Int64 level;
		public System.Int64 exp;
		public System.Int64 summonTime;
		public System.Int64 soldierKillNum;
		public System.Int64 savageKillNum;
		public System.Collections.Generic.List<SprotoType.SkillInfo> skills;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.HeroInfo.TalentTrees> talentTrees;
		public System.Int64 talentIndex;
		public System.Int64 head;
		public System.Int64 breastPlate;
		public System.Int64 weapon;
		public System.Int64 gloves;
		public System.Int64 pants;
		public System.Int64 accessories1;
		public System.Int64 accessories2;
		public System.Int64 shoes;

		public static HashSet<string> updateEntity(HeroInfoEntity et ,SprotoType.HeroInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasHeroId){
				et.heroId = data.heroId;
				if(ret)ET.ATTR.Add("heroId");
			}
			if(data.HasStar){
				et.star = data.star;
				if(ret)ET.ATTR.Add("star");
			}
			if(data.HasStarExp){
				et.starExp = data.starExp;
				if(ret)ET.ATTR.Add("starExp");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasExp){
				et.exp = data.exp;
				if(ret)ET.ATTR.Add("exp");
			}
			if(data.HasSummonTime){
				et.summonTime = data.summonTime;
				if(ret)ET.ATTR.Add("summonTime");
			}
			if(data.HasSoldierKillNum){
				et.soldierKillNum = data.soldierKillNum;
				if(ret)ET.ATTR.Add("soldierKillNum");
			}
			if(data.HasSavageKillNum){
				et.savageKillNum = data.savageKillNum;
				if(ret)ET.ATTR.Add("savageKillNum");
			}
			if(data.HasSkills){
				et.skills = data.skills;
				if(ret)ET.ATTR.Add("skills");
			}
			if(data.HasTalentTrees){

				if (et.talentTrees == null) {
					 et.talentTrees = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.HeroInfo.TalentTrees>();
				}
				foreach(var item in data.talentTrees){ 
					if(et.talentTrees.ContainsKey(item.Key)){
						et.talentTrees[item.Key] = item.Value;
					}else{
						et.talentTrees.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("talentTrees"); 
			}
			if(data.HasTalentIndex){
				et.talentIndex = data.talentIndex;
				if(ret)ET.ATTR.Add("talentIndex");
			}
			if(data.HasHead){
				et.head = data.head;
				if(ret)ET.ATTR.Add("head");
			}
			if(data.HasBreastPlate){
				et.breastPlate = data.breastPlate;
				if(ret)ET.ATTR.Add("breastPlate");
			}
			if(data.HasWeapon){
				et.weapon = data.weapon;
				if(ret)ET.ATTR.Add("weapon");
			}
			if(data.HasGloves){
				et.gloves = data.gloves;
				if(ret)ET.ATTR.Add("gloves");
			}
			if(data.HasPants){
				et.pants = data.pants;
				if(ret)ET.ATTR.Add("pants");
			}
			if(data.HasAccessories1){
				et.accessories1 = data.accessories1;
				if(ret)ET.ATTR.Add("accessories1");
			}
			if(data.HasAccessories2){
				et.accessories2 = data.accessories2;
				if(ret)ET.ATTR.Add("accessories2");
			}
			if(data.HasShoes){
				et.shoes = data.shoes;
				if(ret)ET.ATTR.Add("shoes");
			}
			return ET.ATTR;
		}
	}
	public partial class ItemEntity
	{
		public const string ItemChange = "ItemChange";
		public System.Int64 itemId;
		public System.Int64 itemNum;

		public static HashSet<string> updateEntity(ItemEntity et ,SprotoType.Hero_HeroStarUp.request.Item data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasItemNum){
				et.itemNum = data.itemNum;
				if(ret)ET.ATTR.Add("itemNum");
			}
			return ET.ATTR;
		}
	}
	public partial class ItemInfoEntity
	{
		public const string ItemInfoChange = "ItemInfoChange";
		public System.Int64 itemIndex;
		public System.Int64 uniqueIndex;
		public System.Int64 itemId;
		public System.Int64 overlay;
		public System.Int64 exclusive;
		public System.Int64 heroId;

		public static HashSet<string> updateEntity(ItemInfoEntity et ,SprotoType.ItemInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasItemIndex){
				et.itemIndex = data.itemIndex;
				if(ret)ET.ATTR.Add("itemIndex");
			}
			if(data.HasUniqueIndex){
				et.uniqueIndex = data.uniqueIndex;
				if(ret)ET.ATTR.Add("uniqueIndex");
			}
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasOverlay){
				et.overlay = data.overlay;
				if(ret)ET.ATTR.Add("overlay");
			}
			if(data.HasExclusive){
				et.exclusive = data.exclusive;
				if(ret)ET.ATTR.Add("exclusive");
			}
			if(data.HasHeroId){
				et.heroId = data.heroId;
				if(ret)ET.ATTR.Add("heroId");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildBuildArmyInfoEntity
	{
		public const string GuildBuildArmyInfoChange = "GuildBuildArmyInfoChange";
		public System.Int64 buildArmyIndex;
		public System.Int64 rid;
		public System.Int64 armyIndex;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Int64 status;
		public System.Int64 arrivalTime;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroLevel;
		public System.String roleName;
		public System.Int64 roleHeadId;
		public System.Int64 roleHeadFrameId;

		public static HashSet<string> updateEntity(GuildBuildArmyInfoEntity et ,SprotoType.GuildBuildArmyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasBuildArmyIndex){
				et.buildArmyIndex = data.buildArmyIndex;
				if(ret)ET.ATTR.Add("buildArmyIndex");
			}
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasRoleName){
				et.roleName = data.roleName;
				if(ret)ET.ATTR.Add("roleName");
			}
			if(data.HasRoleHeadId){
				et.roleHeadId = data.roleHeadId;
				if(ret)ET.ATTR.Add("roleHeadId");
			}
			if(data.HasRoleHeadFrameId){
				et.roleHeadFrameId = data.roleHeadFrameId;
				if(ret)ET.ATTR.Add("roleHeadFrameId");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildTerritoryInfoEntity
	{
		public const string GuildTerritoryInfoChange = "GuildTerritoryInfoChange";
		public System.Int64 guildId;
		public System.Int64 colorId;
		public System.Collections.Generic.List<System.Int64> validTerritoryIds;
		public System.Collections.Generic.List<System.Int64> invalidTerritoryIds;
		public System.Collections.Generic.List<System.Int64> preOccupyTerritoryIds;

		public static HashSet<string> updateEntity(GuildTerritoryInfoEntity et ,SprotoType.GuildTerritoryInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasColorId){
				et.colorId = data.colorId;
				if(ret)ET.ATTR.Add("colorId");
			}
			if(data.HasValidTerritoryIds){
				et.validTerritoryIds = data.validTerritoryIds;
				if(ret)ET.ATTR.Add("validTerritoryIds");
			}
			if(data.HasInvalidTerritoryIds){
				et.invalidTerritoryIds = data.invalidTerritoryIds;
				if(ret)ET.ATTR.Add("invalidTerritoryIds");
			}
			if(data.HasPreOccupyTerritoryIds){
				et.preOccupyTerritoryIds = data.preOccupyTerritoryIds;
				if(ret)ET.ATTR.Add("preOccupyTerritoryIds");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildTerritoryLineInfoEntity
	{
		public const string GuildTerritoryLineInfoChange = "GuildTerritoryLineInfoChange";
		public System.Int64 guildId;
		public System.Int64 colorId;
		public System.Collections.Generic.List<SprotoType.TerritoryLineInfo> validLines;
		public System.Collections.Generic.List<SprotoType.TerritoryLineInfo> invalidLines;

		public static HashSet<string> updateEntity(GuildTerritoryLineInfoEntity et ,SprotoType.GuildTerritoryLineInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasColorId){
				et.colorId = data.colorId;
				if(ret)ET.ATTR.Add("colorId");
			}
			if(data.HasValidLines){
				et.validLines = data.validLines;
				if(ret)ET.ATTR.Add("validLines");
			}
			if(data.HasInvalidLines){
				et.invalidLines = data.invalidLines;
				if(ret)ET.ATTR.Add("invalidLines");
			}
			return ET.ATTR;
		}
	}
	public partial class HolyLandArmyInfoEntity
	{
		public const string HolyLandArmyInfoChange = "HolyLandArmyInfoChange";
		public System.Int64 buildArmyIndex;
		public System.Int64 rid;
		public System.Int64 armyIndex;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Int64 status;
		public System.Int64 arrivalTime;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroLevel;
		public System.String roleName;
		public System.Int64 roleHeadId;
		public System.Int64 roleHeadFrameId;

		public static HashSet<string> updateEntity(HolyLandArmyInfoEntity et ,SprotoType.HolyLandArmyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasBuildArmyIndex){
				et.buildArmyIndex = data.buildArmyIndex;
				if(ret)ET.ATTR.Add("buildArmyIndex");
			}
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasRoleName){
				et.roleName = data.roleName;
				if(ret)ET.ATTR.Add("roleName");
			}
			if(data.HasRoleHeadId){
				et.roleHeadId = data.roleHeadId;
				if(ret)ET.ATTR.Add("roleHeadId");
			}
			if(data.HasRoleHeadFrameId){
				et.roleHeadFrameId = data.roleHeadFrameId;
				if(ret)ET.ATTR.Add("roleHeadFrameId");
			}
			return ET.ATTR;
		}
	}
	public partial class MapObjectInfoEntity
	{
		public const string MapObjectInfoChange = "MapObjectInfoChange";
		public System.Int64 objectId;
		public System.Int64 objectType;
		public SprotoType.PosInfo objectPos;
		public System.Collections.Generic.List<SprotoType.PosInfo> objectPath;
		public System.Int64 troopsCapacity;
		public System.Int64 massTroopsCapacity;
		public System.Int64 armyLevel;
		public System.Int64 armyRid;
		public System.String armyName;
		public SprotoType.PosInfo attackerPos;
		public System.Int64 marchType;
		public System.String cityName;
		public System.Int64 cityRid;
		public System.Int64 cityLevel;
		public System.Int64 cityCountry;
		public System.Int64 status;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Int64 arrivalTime;
		public System.Int64 targetObjectIndex;
		public System.Int64 armyIndex;
		public System.Int64 armyCount;
		public System.Int64 objectPower;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.KillCount> killCount;
		public System.Int64 scoutsIndex;
		public System.Int64 startTime;
		public System.Int64 armyRadius;
		public System.Int64 targetAngle;
		public System.Int64 attackCount;
		public System.String guildAbbName;
		public System.Int64 beginBurnTime;
		public System.Int64 guildId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.CityBuff> cityBuff;
		public System.Collections.Generic.List<SprotoType.BattleBuffDetail> battleBuff;
		public System.Int64 headFrameID;
		public System.Int64 headId;
		public System.Int64 sp;
		public System.Int64 maxSp;
		public System.String guildFullName;
		public System.Collections.Generic.List<SprotoType.SkillInfo> mainHeroSkills;
		public System.Collections.Generic.List<SprotoType.SkillInfo> deputyHeroSkills;
		public System.Int64 collectRuneTime;
		public System.Int64 armyCountMax;
		public System.Boolean isRally;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.ArmyMarchInfo> armyMarchInfos;
		public System.Boolean isGuide;
		public System.Int64 guardTowerLevel;
		public System.Int64 cityPosTime;
		public System.Boolean isBattleLose;
		public System.Int64 refreshTime;
		public System.Int64 resourceAmount;
		public System.Int64 collectTime;
		public System.Int64 collectRid;
		public System.Int64 resourceId;
		public System.Int64 collectSpeed;
		public System.Int64 resourcePointId;
		public System.Int64 collectNum;
		public System.Collections.Generic.List<SprotoType.CollectSpeedInfo> collectSpeeds;
		public System.String resourceGuildAbbName;
		public System.Int64 monsterId;
		public System.Int64 guildBuildStatus;
		public System.Int64 durable;
		public System.Int64 durableLimit;
		public System.Int64 buildProgress;
		public System.Int64 buildProgressTime;
		public System.Int64 buildFinishTime;
		public System.Int64 needBuildTime;
		public System.Int64 buildBurnSpeed;
		public System.Int64 lastOutFireTime;
		public System.Int64 buildBurnTime;
		public System.Int64 buildDurableRecoverTime;
		public System.Collections.Generic.List<System.Int64> guildFlagSigns;
		public System.Int64 resourceCenterDeleteTime;
		public System.Int64 collectRoleNum;
		public System.Int64 runeId;
		public System.Int64 runeRefreshTime;
		public System.Int64 transportIndex;
		public System.Int64 strongHoldId;
		public System.Int64 holyLandStatus;
		public System.Int64 holyLandFinishTime;
		public System.String kingName;
		public System.Int64 monsterIndex;
		public System.Int64 mapIndex;

		public static HashSet<string> updateEntity(MapObjectInfoEntity et ,SprotoType.MapObjectInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasObjectId){
				et.objectId = data.objectId;
				if(ret)ET.ATTR.Add("objectId");
			}
			if(data.HasObjectType){
				et.objectType = data.objectType;
				if(ret)ET.ATTR.Add("objectType");
			}
			if(data.HasObjectPos){
				et.objectPos = data.objectPos;
				if(ret)ET.ATTR.Add("objectPos");
			}
			if(data.HasObjectPath){
				et.objectPath = data.objectPath;
				if(ret)ET.ATTR.Add("objectPath");
			}
			if(data.HasTroopsCapacity){
				et.troopsCapacity = data.troopsCapacity;
				if(ret)ET.ATTR.Add("troopsCapacity");
			}
			if(data.HasMassTroopsCapacity){
				et.massTroopsCapacity = data.massTroopsCapacity;
				if(ret)ET.ATTR.Add("massTroopsCapacity");
			}
			if(data.HasArmyLevel){
				et.armyLevel = data.armyLevel;
				if(ret)ET.ATTR.Add("armyLevel");
			}
			if(data.HasArmyRid){
				et.armyRid = data.armyRid;
				if(ret)ET.ATTR.Add("armyRid");
			}
			if(data.HasArmyName){
				et.armyName = data.armyName;
				if(ret)ET.ATTR.Add("armyName");
			}
			if(data.HasAttackerPos){
				et.attackerPos = data.attackerPos;
				if(ret)ET.ATTR.Add("attackerPos");
			}
			if(data.HasMarchType){
				et.marchType = data.marchType;
				if(ret)ET.ATTR.Add("marchType");
			}
			if(data.HasCityName){
				et.cityName = data.cityName;
				if(ret)ET.ATTR.Add("cityName");
			}
			if(data.HasCityRid){
				et.cityRid = data.cityRid;
				if(ret)ET.ATTR.Add("cityRid");
			}
			if(data.HasCityLevel){
				et.cityLevel = data.cityLevel;
				if(ret)ET.ATTR.Add("cityLevel");
			}
			if(data.HasCityCountry){
				et.cityCountry = data.cityCountry;
				if(ret)ET.ATTR.Add("cityCountry");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasTargetObjectIndex){
				et.targetObjectIndex = data.targetObjectIndex;
				if(ret)ET.ATTR.Add("targetObjectIndex");
			}
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasArmyCount){
				et.armyCount = data.armyCount;
				if(ret)ET.ATTR.Add("armyCount");
			}
			if(data.HasObjectPower){
				et.objectPower = data.objectPower;
				if(ret)ET.ATTR.Add("objectPower");
			}
			if(data.HasKillCount){

				if (et.killCount == null) {
					 et.killCount = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.KillCount>();
				}
				foreach(var item in data.killCount){ 
					if(et.killCount.ContainsKey(item.Key)){
						et.killCount[item.Key] = item.Value;
					}else{
						et.killCount.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("killCount"); 
			}
			if(data.HasScoutsIndex){
				et.scoutsIndex = data.scoutsIndex;
				if(ret)ET.ATTR.Add("scoutsIndex");
			}
			if(data.HasStartTime){
				et.startTime = data.startTime;
				if(ret)ET.ATTR.Add("startTime");
			}
			if(data.HasArmyRadius){
				et.armyRadius = data.armyRadius;
				if(ret)ET.ATTR.Add("armyRadius");
			}
			if(data.HasTargetAngle){
				et.targetAngle = data.targetAngle;
				if(ret)ET.ATTR.Add("targetAngle");
			}
			if(data.HasAttackCount){
				et.attackCount = data.attackCount;
				if(ret)ET.ATTR.Add("attackCount");
			}
			if(data.HasGuildAbbName){
				et.guildAbbName = data.guildAbbName;
				if(ret)ET.ATTR.Add("guildAbbName");
			}
			if(data.HasBeginBurnTime){
				et.beginBurnTime = data.beginBurnTime;
				if(ret)ET.ATTR.Add("beginBurnTime");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasCityBuff){

				if (et.cityBuff == null) {
					 et.cityBuff = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.CityBuff>();
				}
				foreach(var item in data.cityBuff){ 
					if(et.cityBuff.ContainsKey(item.Key)){
						et.cityBuff[item.Key] = item.Value;
					}else{
						et.cityBuff.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("cityBuff"); 
			}
			if(data.HasBattleBuff){
				et.battleBuff = data.battleBuff;
				if(ret)ET.ATTR.Add("battleBuff");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasSp){
				et.sp = data.sp;
				if(ret)ET.ATTR.Add("sp");
			}
			if(data.HasMaxSp){
				et.maxSp = data.maxSp;
				if(ret)ET.ATTR.Add("maxSp");
			}
			if(data.HasGuildFullName){
				et.guildFullName = data.guildFullName;
				if(ret)ET.ATTR.Add("guildFullName");
			}
			if(data.HasMainHeroSkills){
				et.mainHeroSkills = data.mainHeroSkills;
				if(ret)ET.ATTR.Add("mainHeroSkills");
			}
			if(data.HasDeputyHeroSkills){
				et.deputyHeroSkills = data.deputyHeroSkills;
				if(ret)ET.ATTR.Add("deputyHeroSkills");
			}
			if(data.HasCollectRuneTime){
				et.collectRuneTime = data.collectRuneTime;
				if(ret)ET.ATTR.Add("collectRuneTime");
			}
			if(data.HasArmyCountMax){
				et.armyCountMax = data.armyCountMax;
				if(ret)ET.ATTR.Add("armyCountMax");
			}
			if(data.HasIsRally){
				et.isRally = data.isRally;
				if(ret)ET.ATTR.Add("isRally");
			}
			if(data.HasArmyMarchInfos){

				if (et.armyMarchInfos == null) {
					 et.armyMarchInfos = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.ArmyMarchInfo>();
				}
				foreach(var item in data.armyMarchInfos){ 
					if(et.armyMarchInfos.ContainsKey(item.Key)){
						et.armyMarchInfos[item.Key] = item.Value;
					}else{
						et.armyMarchInfos.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("armyMarchInfos"); 
			}
			if(data.HasIsGuide){
				et.isGuide = data.isGuide;
				if(ret)ET.ATTR.Add("isGuide");
			}
			if(data.HasGuardTowerLevel){
				et.guardTowerLevel = data.guardTowerLevel;
				if(ret)ET.ATTR.Add("guardTowerLevel");
			}
			if(data.HasCityPosTime){
				et.cityPosTime = data.cityPosTime;
				if(ret)ET.ATTR.Add("cityPosTime");
			}
			if(data.HasIsBattleLose){
				et.isBattleLose = data.isBattleLose;
				if(ret)ET.ATTR.Add("isBattleLose");
			}
			if(data.HasRefreshTime){
				et.refreshTime = data.refreshTime;
				if(ret)ET.ATTR.Add("refreshTime");
			}
			if(data.HasResourceAmount){
				et.resourceAmount = data.resourceAmount;
				if(ret)ET.ATTR.Add("resourceAmount");
			}
			if(data.HasCollectTime){
				et.collectTime = data.collectTime;
				if(ret)ET.ATTR.Add("collectTime");
			}
			if(data.HasCollectRid){
				et.collectRid = data.collectRid;
				if(ret)ET.ATTR.Add("collectRid");
			}
			if(data.HasResourceId){
				et.resourceId = data.resourceId;
				if(ret)ET.ATTR.Add("resourceId");
			}
			if(data.HasCollectSpeed){
				et.collectSpeed = data.collectSpeed;
				if(ret)ET.ATTR.Add("collectSpeed");
			}
			if(data.HasResourcePointId){
				et.resourcePointId = data.resourcePointId;
				if(ret)ET.ATTR.Add("resourcePointId");
			}
			if(data.HasCollectNum){
				et.collectNum = data.collectNum;
				if(ret)ET.ATTR.Add("collectNum");
			}
			if(data.HasCollectSpeeds){
				et.collectSpeeds = data.collectSpeeds;
				if(ret)ET.ATTR.Add("collectSpeeds");
			}
			if(data.HasResourceGuildAbbName){
				et.resourceGuildAbbName = data.resourceGuildAbbName;
				if(ret)ET.ATTR.Add("resourceGuildAbbName");
			}
			if(data.HasMonsterId){
				et.monsterId = data.monsterId;
				if(ret)ET.ATTR.Add("monsterId");
			}
			if(data.HasGuildBuildStatus){
				et.guildBuildStatus = data.guildBuildStatus;
				if(ret)ET.ATTR.Add("guildBuildStatus");
			}
			if(data.HasDurable){
				et.durable = data.durable;
				if(ret)ET.ATTR.Add("durable");
			}
			if(data.HasDurableLimit){
				et.durableLimit = data.durableLimit;
				if(ret)ET.ATTR.Add("durableLimit");
			}
			if(data.HasBuildProgress){
				et.buildProgress = data.buildProgress;
				if(ret)ET.ATTR.Add("buildProgress");
			}
			if(data.HasBuildProgressTime){
				et.buildProgressTime = data.buildProgressTime;
				if(ret)ET.ATTR.Add("buildProgressTime");
			}
			if(data.HasBuildFinishTime){
				et.buildFinishTime = data.buildFinishTime;
				if(ret)ET.ATTR.Add("buildFinishTime");
			}
			if(data.HasNeedBuildTime){
				et.needBuildTime = data.needBuildTime;
				if(ret)ET.ATTR.Add("needBuildTime");
			}
			if(data.HasBuildBurnSpeed){
				et.buildBurnSpeed = data.buildBurnSpeed;
				if(ret)ET.ATTR.Add("buildBurnSpeed");
			}
			if(data.HasLastOutFireTime){
				et.lastOutFireTime = data.lastOutFireTime;
				if(ret)ET.ATTR.Add("lastOutFireTime");
			}
			if(data.HasBuildBurnTime){
				et.buildBurnTime = data.buildBurnTime;
				if(ret)ET.ATTR.Add("buildBurnTime");
			}
			if(data.HasBuildDurableRecoverTime){
				et.buildDurableRecoverTime = data.buildDurableRecoverTime;
				if(ret)ET.ATTR.Add("buildDurableRecoverTime");
			}
			if(data.HasGuildFlagSigns){
				et.guildFlagSigns = data.guildFlagSigns;
				if(ret)ET.ATTR.Add("guildFlagSigns");
			}
			if(data.HasResourceCenterDeleteTime){
				et.resourceCenterDeleteTime = data.resourceCenterDeleteTime;
				if(ret)ET.ATTR.Add("resourceCenterDeleteTime");
			}
			if(data.HasCollectRoleNum){
				et.collectRoleNum = data.collectRoleNum;
				if(ret)ET.ATTR.Add("collectRoleNum");
			}
			if(data.HasRuneId){
				et.runeId = data.runeId;
				if(ret)ET.ATTR.Add("runeId");
			}
			if(data.HasRuneRefreshTime){
				et.runeRefreshTime = data.runeRefreshTime;
				if(ret)ET.ATTR.Add("runeRefreshTime");
			}
			if(data.HasTransportIndex){
				et.transportIndex = data.transportIndex;
				if(ret)ET.ATTR.Add("transportIndex");
			}
			if(data.HasStrongHoldId){
				et.strongHoldId = data.strongHoldId;
				if(ret)ET.ATTR.Add("strongHoldId");
			}
			if(data.HasHolyLandStatus){
				et.holyLandStatus = data.holyLandStatus;
				if(ret)ET.ATTR.Add("holyLandStatus");
			}
			if(data.HasHolyLandFinishTime){
				et.holyLandFinishTime = data.holyLandFinishTime;
				if(ret)ET.ATTR.Add("holyLandFinishTime");
			}
			if(data.HasKingName){
				et.kingName = data.kingName;
				if(ret)ET.ATTR.Add("kingName");
			}
			if(data.HasMonsterIndex){
				et.monsterIndex = data.monsterIndex;
				if(ret)ET.ATTR.Add("monsterIndex");
			}
			if(data.HasMapIndex){
				et.mapIndex = data.mapIndex;
				if(ret)ET.ATTR.Add("mapIndex");
			}
			return ET.ATTR;
		}
	}
	public partial class BarbarianPosInfoEntity
	{
		public const string BarbarianPosInfoChange = "BarbarianPosInfoChange";
		public SprotoType.PosInfo pos;
		public System.Int64 objectId;

		public static HashSet<string> updateEntity(BarbarianPosInfoEntity et ,SprotoType.Map_SearchBarbarian.response.BarbarianPosInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasObjectId){
				et.objectId = data.objectId;
				if(ret)ET.ATTR.Add("objectId");
			}
			return ET.ATTR;
		}
	}
	public partial class ResourcePosInfoEntity
	{
		public const string ResourcePosInfoChange = "ResourcePosInfoChange";
		public System.Int64 resourceLevel;
		public SprotoType.PosInfo pos;
		public System.Int64 objectId;

		public static HashSet<string> updateEntity(ResourcePosInfoEntity et ,SprotoType.Map_SearchResource.response.ResourcePosInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasResourceLevel){
				et.resourceLevel = data.resourceLevel;
				if(ret)ET.ATTR.Add("resourceLevel");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasObjectId){
				et.objectId = data.objectId;
				if(ret)ET.ATTR.Add("objectId");
			}
			return ET.ATTR;
		}
	}
	public partial class RallyedDetailEntity
	{
		public const string RallyedDetailChange = "RallyedDetailChange";
		public System.Int64 rallyedIndex;
		public System.String rallyedName;
		public System.Int64 rallyedHeadId;
		public System.Int64 rallyedHeadFrameId;
		public SprotoType.PosInfo rallyedPos;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RallyDetail> rallyDetail;
		public System.Collections.Generic.List<SprotoType.ReinforceDetail> reinforceDetail;
		public System.Int64 rallyedReinforceMax;
		public System.Int64 rallyedType;
		public System.Int64 rallyTargetHolyLandId;

		public static HashSet<string> updateEntity(RallyedDetailEntity et ,SprotoType.RallyedDetail data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRallyedIndex){
				et.rallyedIndex = data.rallyedIndex;
				if(ret)ET.ATTR.Add("rallyedIndex");
			}
			if(data.HasRallyedName){
				et.rallyedName = data.rallyedName;
				if(ret)ET.ATTR.Add("rallyedName");
			}
			if(data.HasRallyedHeadId){
				et.rallyedHeadId = data.rallyedHeadId;
				if(ret)ET.ATTR.Add("rallyedHeadId");
			}
			if(data.HasRallyedHeadFrameId){
				et.rallyedHeadFrameId = data.rallyedHeadFrameId;
				if(ret)ET.ATTR.Add("rallyedHeadFrameId");
			}
			if(data.HasRallyedPos){
				et.rallyedPos = data.rallyedPos;
				if(ret)ET.ATTR.Add("rallyedPos");
			}
			if(data.HasRallyDetail){

				if (et.rallyDetail == null) {
					 et.rallyDetail = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RallyDetail>();
				}
				foreach(var item in data.rallyDetail){ 
					if(et.rallyDetail.ContainsKey(item.Key)){
						et.rallyDetail[item.Key] = item.Value;
					}else{
						et.rallyDetail.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("rallyDetail"); 
			}
			if(data.HasReinforceDetail){
				et.reinforceDetail = data.reinforceDetail;
				if(ret)ET.ATTR.Add("reinforceDetail");
			}
			if(data.HasRallyedReinforceMax){
				et.rallyedReinforceMax = data.rallyedReinforceMax;
				if(ret)ET.ATTR.Add("rallyedReinforceMax");
			}
			if(data.HasRallyedType){
				et.rallyedType = data.rallyedType;
				if(ret)ET.ATTR.Add("rallyedType");
			}
			if(data.HasRallyTargetHolyLandId){
				et.rallyTargetHolyLandId = data.rallyTargetHolyLandId;
				if(ret)ET.ATTR.Add("rallyTargetHolyLandId");
			}
			return ET.ATTR;
		}
	}
	public partial class RankInfoEntity
	{
		public const string RankInfoChange = "RankInfoChange";
		public System.Int64 rid;
		public System.Int64 guildId;
		public System.Int64 score;
		public System.Collections.Generic.List<System.Int64> signs;
		public System.String name;
		public System.String abbreviationName;
		public System.Int64 index;
		public System.String leaderName;
		public System.Int64 oldRank;
		public System.String guildName;
		public System.Int64 headFrameID;
		public System.Int64 headId;

		public static HashSet<string> updateEntity(RankInfoEntity et ,SprotoType.Rank_QueryRank.response.RankInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasGuildId){
				et.guildId = data.guildId;
				if(ret)ET.ATTR.Add("guildId");
			}
			if(data.HasScore){
				et.score = data.score;
				if(ret)ET.ATTR.Add("score");
			}
			if(data.HasSigns){
				et.signs = data.signs;
				if(ret)ET.ATTR.Add("signs");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasAbbreviationName){
				et.abbreviationName = data.abbreviationName;
				if(ret)ET.ATTR.Add("abbreviationName");
			}
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasLeaderName){
				et.leaderName = data.leaderName;
				if(ret)ET.ATTR.Add("leaderName");
			}
			if(data.HasOldRank){
				et.oldRank = data.oldRank;
				if(ret)ET.ATTR.Add("oldRank");
			}
			if(data.HasGuildName){
				et.guildName = data.guildName;
				if(ret)ET.ATTR.Add("guildName");
			}
			if(data.HasHeadFrameID){
				et.headFrameID = data.headFrameID;
				if(ret)ET.ATTR.Add("headFrameID");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			return ET.ATTR;
		}
	}
	public partial class EarlyWarningInfoEntity
	{
		public const string EarlyWarningInfoChange = "EarlyWarningInfoChange";
		public System.Int64 earlyWarningIndex;
		public System.Int64 earlyWarningType;
		public System.String scoutFromName;
		public System.Int64 scoutObjectType;
		public System.Int64 arrivalTime;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> attackSoldiers;
		public System.Boolean isDelete;
		public System.Boolean isShield;
		public System.Int64 mainHeroId;
		public System.Int64 mainHeroLevel;
		public System.Int64 deputyHeroId;
		public System.Int64 deputyHeroLevel;
		public System.Int64 armyIndex;
		public System.Collections.Generic.List<SprotoType.TransportResourceInfo> transportResourceInfo;
		public System.String transportName;
		public System.String guildAbbr;
		public System.Int64 holyLandId;
		public System.Boolean isRally;
		public System.Int64 objectIndex;

		public static HashSet<string> updateEntity(EarlyWarningInfoEntity et ,SprotoType.EarlyWarningInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasEarlyWarningIndex){
				et.earlyWarningIndex = data.earlyWarningIndex;
				if(ret)ET.ATTR.Add("earlyWarningIndex");
			}
			if(data.HasEarlyWarningType){
				et.earlyWarningType = data.earlyWarningType;
				if(ret)ET.ATTR.Add("earlyWarningType");
			}
			if(data.HasScoutFromName){
				et.scoutFromName = data.scoutFromName;
				if(ret)ET.ATTR.Add("scoutFromName");
			}
			if(data.HasScoutObjectType){
				et.scoutObjectType = data.scoutObjectType;
				if(ret)ET.ATTR.Add("scoutObjectType");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasAttackSoldiers){

				if (et.attackSoldiers == null) {
					 et.attackSoldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.attackSoldiers){ 
					if(et.attackSoldiers.ContainsKey(item.Key)){
						et.attackSoldiers[item.Key] = item.Value;
					}else{
						et.attackSoldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("attackSoldiers"); 
			}
			if(data.HasIsDelete){
				et.isDelete = data.isDelete;
				if(ret)ET.ATTR.Add("isDelete");
			}
			if(data.HasIsShield){
				et.isShield = data.isShield;
				if(ret)ET.ATTR.Add("isShield");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasMainHeroLevel){
				et.mainHeroLevel = data.mainHeroLevel;
				if(ret)ET.ATTR.Add("mainHeroLevel");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasDeputyHeroLevel){
				et.deputyHeroLevel = data.deputyHeroLevel;
				if(ret)ET.ATTR.Add("deputyHeroLevel");
			}
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasTransportResourceInfo){
				et.transportResourceInfo = data.transportResourceInfo;
				if(ret)ET.ATTR.Add("transportResourceInfo");
			}
			if(data.HasTransportName){
				et.transportName = data.transportName;
				if(ret)ET.ATTR.Add("transportName");
			}
			if(data.HasGuildAbbr){
				et.guildAbbr = data.guildAbbr;
				if(ret)ET.ATTR.Add("guildAbbr");
			}
			if(data.HasHolyLandId){
				et.holyLandId = data.holyLandId;
				if(ret)ET.ATTR.Add("holyLandId");
			}
			if(data.HasIsRally){
				et.isRally = data.isRally;
				if(ret)ET.ATTR.Add("isRally");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			return ET.ATTR;
		}
	}
	public partial class ReinforceRecordInfoEntity
	{
		public const string ReinforceRecordInfoChange = "ReinforceRecordInfoChange";
		public System.Int64 headId;
		public System.String name;
		public System.Int64 arrivalTime;
		public System.Int64 armyCount;

		public static HashSet<string> updateEntity(ReinforceRecordInfoEntity et ,SprotoType.ReinforceRecordInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasArmyCount){
				et.armyCount = data.armyCount;
				if(ret)ET.ATTR.Add("armyCount");
			}
			return ET.ATTR;
		}
	}
	public partial class MonumentListEntity
	{
		public const string MonumentListChange = "MonumentListChange";
		public System.Int64 id;
		public System.Boolean reward;
		public System.Boolean canReward;
		public System.Int64 count;
		public System.Collections.Generic.List<SprotoType.Role_GetMonument.response.MonumentList.GuildRank> guildRank;
		public System.Int64 finishTime;
		public System.Int64 serverCount;

		public static HashSet<string> updateEntity(MonumentListEntity et ,SprotoType.Role_GetMonument.response.MonumentList data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasReward){
				et.reward = data.reward;
				if(ret)ET.ATTR.Add("reward");
			}
			if(data.HasCanReward){
				et.canReward = data.canReward;
				if(ret)ET.ATTR.Add("canReward");
			}
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			if(data.HasGuildRank){
				et.guildRank = data.guildRank;
				if(ret)ET.ATTR.Add("guildRank");
			}
			if(data.HasFinishTime){
				et.finishTime = data.finishTime;
				if(ret)ET.ATTR.Add("finishTime");
			}
			if(data.HasServerCount){
				et.serverCount = data.serverCount;
				if(ret)ET.ATTR.Add("serverCount");
			}
			return ET.ATTR;
		}
	}
	public partial class ScoutsInfoEntity
	{
		public const string ScoutsInfoChange = "ScoutsInfoChange";
		public System.Int64 scoutsIndex;
		public SprotoType.PosInfo scoutsPos;
		public System.Int64 scoutsTargetIndex;
		public SprotoType.PosInfo scoutsTargetPos;
		public System.Int64 scoutsStatus;
		public System.Collections.Generic.List<SprotoType.PosInfo> scoutsPath;
		public System.Int64 startTime;
		public System.Int64 arrivalTime;
		public System.Int64 objectIndex;
		public System.Int64 denseFogNum;
		public System.Int64 leaveCityTime;

		public static HashSet<string> updateEntity(ScoutsInfoEntity et ,SprotoType.ScoutsInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasScoutsIndex){
				et.scoutsIndex = data.scoutsIndex;
				if(ret)ET.ATTR.Add("scoutsIndex");
			}
			if(data.HasScoutsPos){
				et.scoutsPos = data.scoutsPos;
				if(ret)ET.ATTR.Add("scoutsPos");
			}
			if(data.HasScoutsTargetIndex){
				et.scoutsTargetIndex = data.scoutsTargetIndex;
				if(ret)ET.ATTR.Add("scoutsTargetIndex");
			}
			if(data.HasScoutsTargetPos){
				et.scoutsTargetPos = data.scoutsTargetPos;
				if(ret)ET.ATTR.Add("scoutsTargetPos");
			}
			if(data.HasScoutsStatus){
				et.scoutsStatus = data.scoutsStatus;
				if(ret)ET.ATTR.Add("scoutsStatus");
			}
			if(data.HasScoutsPath){
				et.scoutsPath = data.scoutsPath;
				if(ret)ET.ATTR.Add("scoutsPath");
			}
			if(data.HasStartTime){
				et.startTime = data.startTime;
				if(ret)ET.ATTR.Add("startTime");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			if(data.HasDenseFogNum){
				et.denseFogNum = data.denseFogNum;
				if(ret)ET.ATTR.Add("denseFogNum");
			}
			if(data.HasLeaveCityTime){
				et.leaveCityTime = data.leaveCityTime;
				if(ret)ET.ATTR.Add("leaveCityTime");
			}
			return ET.ATTR;
		}
	}
	public partial class TaskInfoEntity
	{
		public const string TaskInfoChange = "TaskInfoChange";
		public System.Int64 taskId;
		public System.Int64 taskSchedule;

		public static HashSet<string> updateEntity(TaskInfoEntity et ,SprotoType.TaskInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTaskId){
				et.taskId = data.taskId;
				if(ret)ET.ATTR.Add("taskId");
			}
			if(data.HasTaskSchedule){
				et.taskSchedule = data.taskSchedule;
				if(ret)ET.ATTR.Add("taskSchedule");
			}
			return ET.ATTR;
		}
	}
	public partial class TransportInfoEntity
	{
		public const string TransportInfoChange = "TransportInfoChange";
		public System.Int64 transportIndex;
		public System.Collections.Generic.List<SprotoType.TransportResourceInfo> transportResourceInfo;
		public System.Collections.Generic.List<SprotoType.TransportResourceInfo> allResourceInfo;
		public System.Int64 arrivalTime;
		public System.Collections.Generic.List<SprotoType.PosInfo> path;
		public System.Int64 startTime;
		public System.Int64 objectIndex;
		public SprotoType.PosInfo targetPos;
		public System.Int64 targetObjectIndex;
		public System.String targetName;
		public System.Int64 transportStatus;
		public System.Int64 targetRid;

		public static HashSet<string> updateEntity(TransportInfoEntity et ,SprotoType.TransportInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTransportIndex){
				et.transportIndex = data.transportIndex;
				if(ret)ET.ATTR.Add("transportIndex");
			}
			if(data.HasTransportResourceInfo){
				et.transportResourceInfo = data.transportResourceInfo;
				if(ret)ET.ATTR.Add("transportResourceInfo");
			}
			if(data.HasAllResourceInfo){
				et.allResourceInfo = data.allResourceInfo;
				if(ret)ET.ATTR.Add("allResourceInfo");
			}
			if(data.HasArrivalTime){
				et.arrivalTime = data.arrivalTime;
				if(ret)ET.ATTR.Add("arrivalTime");
			}
			if(data.HasPath){
				et.path = data.path;
				if(ret)ET.ATTR.Add("path");
			}
			if(data.HasStartTime){
				et.startTime = data.startTime;
				if(ret)ET.ATTR.Add("startTime");
			}
			if(data.HasObjectIndex){
				et.objectIndex = data.objectIndex;
				if(ret)ET.ATTR.Add("objectIndex");
			}
			if(data.HasTargetPos){
				et.targetPos = data.targetPos;
				if(ret)ET.ATTR.Add("targetPos");
			}
			if(data.HasTargetObjectIndex){
				et.targetObjectIndex = data.targetObjectIndex;
				if(ret)ET.ATTR.Add("targetObjectIndex");
			}
			if(data.HasTargetName){
				et.targetName = data.targetName;
				if(ret)ET.ATTR.Add("targetName");
			}
			if(data.HasTransportStatus){
				et.transportStatus = data.transportStatus;
				if(ret)ET.ATTR.Add("transportStatus");
			}
			if(data.HasTargetRid){
				et.targetRid = data.targetRid;
				if(ret)ET.ATTR.Add("targetRid");
			}
			return ET.ATTR;
		}
	}
	public partial class HistoryInfoEntity
	{
		public const string HistoryInfoChange = "HistoryInfoChange";
		public System.String name;
		public System.String abbreviationName;
		public System.Int64 score;
		public System.Int64 rank;

		public static HashSet<string> updateEntity(HistoryInfoEntity et ,SprotoType.HistoryInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasAbbreviationName){
				et.abbreviationName = data.abbreviationName;
				if(ret)ET.ATTR.Add("abbreviationName");
			}
			if(data.HasScore){
				et.score = data.score;
				if(ret)ET.ATTR.Add("score");
			}
			if(data.HasRank){
				et.rank = data.rank;
				if(ret)ET.ATTR.Add("rank");
			}
			return ET.ATTR;
		}
	}
	public partial class GuildRankEntity
	{
		public const string GuildRankChange = "GuildRankChange";
		public System.Int64 count;
		public System.Collections.Generic.List<System.Int64> signs;
		public System.String guildName;
		public System.String abbreviationName;

		public static HashSet<string> updateEntity(GuildRankEntity et ,SprotoType.Role_GetMonument.response.MonumentList.GuildRank data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasCount){
				et.count = data.count;
				if(ret)ET.ATTR.Add("count");
			}
			if(data.HasSigns){
				et.signs = data.signs;
				if(ret)ET.ATTR.Add("signs");
			}
			if(data.HasGuildName){
				et.guildName = data.guildName;
				if(ret)ET.ATTR.Add("guildName");
			}
			if(data.HasAbbreviationName){
				et.abbreviationName = data.abbreviationName;
				if(ret)ET.ATTR.Add("abbreviationName");
			}
			return ET.ATTR;
		}
	}
}

