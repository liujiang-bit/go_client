using UnityEngine;
using System.Collections.Generic;
using System;
using Client;
using Data;
using Hotfix;
using Skyunion;
using SprotoType;
namespace Game
{
	public class ET
	{
        public static HashSet<string> ATTR = new HashSet<string>();
        
    }

	//客户端扩展
	public partial class MapObjectInfoEntity
	{
		public GameObject gameobject;
		public bool isInView;
		public int gridX = -1;
		public int gridY = -1;
		public bool isLoading = false;
		//建筑
		public bool isFight = true;
		public int attackTargetId;
		
		//半径
		public float radius;
		/// <summary>
		/// 服务器移除时间
		/// </summary>
		public long delTime = 0;

		/////////rss/////////
		public VillageRewardDataDefine villageRewardDataDefine;
		public ResourceGatherTypeDefine resourceGatherTypeDefine;
		public MapFixPointDefine mapFixPointDefine;
		public string name;
		public bool isShowHud;
		public string showHudIcon;
		public bool villageState = false;
		public bool onExploreMode = false;//是否在探索模式下
		
		public RssType rssType = RssType.None;  //objectType
		public RssPointState rssPointStateType = RssPointState.Uncollected;
				
		//////MonsterData////////
		public MonsterDefine monsterDefine;
		public MonsterTroopsDefine monsterTroopsDefine;
		public MonsterTroopsAttrDefine monsterTroopsAttrDefine;
		public MonsterPointDefine monsterPointDefine;
		public MonsterRefreshLevelDefine monsterRefreshLevelDefine;
		public HeroDefine heroDefine;
		public List<Vector2> path= new List<Vector2>();
		public Vector2 pos=Vector2.zero;
		public int HP;
		public int HPMax;
		public bool IsShowRssHud = false;
		public string showHudicon;
		public AudioHandler m_attackAudioHandler;
		public string shootTextDes;
		public int atkId;
		public ArmyStatus monsterArmyStatus;
        public bool IsCollectRune { get; set; } = false;
		public int autoMoveIndex { get; set; }

		public void ClearMovePath()
		{
			autoMoveIndex = 0;
			path.Clear();
		}
		private Vector2 GetV2Pos()
        {
	        return new Vector2(objectPos.x / 100 , objectPos.y / 100);
        }
        private List<Vector2> GetV2MovePath()
        {
	        var movePath = new List<Vector2>();
	        if (objectPath != null)
	        {
		        foreach (var v in objectPath)
		        {
			        movePath.Add(new Vector2(v.x / 100, v.y / 100));
		        }
	        }
	        return movePath;
        }
        private void CalcPosAndIndex(out Vector2 resultPos,out int resultIndex)
        {
            TroopHelp.CalcPosAndIndex(out resultPos, out resultIndex
                , status
                , GetV2Pos()
                , GetV2MovePath()
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
	}
}