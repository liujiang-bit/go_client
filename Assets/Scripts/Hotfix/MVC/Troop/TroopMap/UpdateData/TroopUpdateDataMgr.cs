using System.Collections.Generic;

namespace Hotfix
{
    public enum UpdateDataType
    {
        None=0,
        //大地图
        Map,
        //副本
        BattleCopy,
    }

    public class TroopUpdateDataMgr: IBaseHandler, IUpdateDataHandler
    {
        private readonly  Dictionary<UpdateDataType,ITroopUpdateData> dicTroopUpdateData=new Dictionary<UpdateDataType, ITroopUpdateData>();
        private readonly  Dictionary<UpdateDataType,IMonsterUpdateData> dicMonsterUpdateData=new Dictionary<UpdateDataType, IMonsterUpdateData>();
        private readonly  Dictionary<UpdateDataType, IBuildingFightUpdateData> dicBuildingFightUpdateData=new Dictionary<UpdateDataType, IBuildingFightUpdateData>();  
        
        public void Init()
        {
            dicTroopUpdateData[UpdateDataType.Map] = new TroopUpdateData();
            dicMonsterUpdateData[UpdateDataType.Map]=new MonsterUpdateData();  
            dicBuildingFightUpdateData[UpdateDataType.Map]= new BuildingFightUpdateData();
        }

        public ITroopUpdateData UpdateTroopData()
        {
            return dicTroopUpdateData[UpdateDataType.Map];
        }

        public IMonsterUpdateData UpdateMonsterData()
        {
            return dicMonsterUpdateData[UpdateDataType.Map];
        }

        public IBuildingFightUpdateData UpdateBuildingFightData()
        {
            return dicBuildingFightUpdateData[UpdateDataType.Map];
        }

        public void Clear()
        {        
            foreach(var child in dicTroopUpdateData)
            {
                child.Value.ClearData();
            }
            foreach (var child in dicMonsterUpdateData)
            {
                child.Value.ClearData();
            }
        }
    }
}