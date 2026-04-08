using System;
using System.Collections.Generic;
using Client;
using Data;
using Game;
using Skyunion;
using SprotoType;

namespace Hotfix
{
    public class SquareHelper : TSingleton<SquareHelper>
    {
        private SoldierProxy m_SoldierProxy;
        private int hero_id_offset = 0;
        private Dictionary<Troops.ENUM_SQUARE_CATEGORY, List<int>> UnitId = new Dictionary<Troops.ENUM_SQUARE_CATEGORY, List<int>>();
        private List<TroopsCell> lsUnitDatas = new List<TroopsCell>();
        private List<TroopsHero> lsHeroDatas = new List<TroopsHero>();

        public void InitUnitId()
        {
            List<ArmsDefine> ls = CoreUtils.dataService.QueryRecords<ArmsDefine>();
            foreach (var item in ls)
            {
                if (!UnitId.ContainsKey((Troops.ENUM_SQUARE_CATEGORY) item.armsType))
                {
                    UnitId[(Troops.ENUM_SQUARE_CATEGORY) item.armsType] = new List<int>();
                }

                UnitId[(Troops.ENUM_SQUARE_CATEGORY) item.armsType].Add(item.ID);
            }
        }

        protected override void Init()
        {
            InitUnitId();
        }


        public string GetSquareString(List<TroopsCell> units, List<TroopsHero> heros, Troops.ENMU_MATRIX_TYPE type = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            string infantry_string = string.Empty;
            string cavalry_string = string.Empty;
            string archery_string = string.Empty;
            string siege_string = string.Empty;
            string square_string = string.Empty;
            string hero_string = string.Empty;
            foreach (var unit in units)
            {
                if (IsValueInTable(unit.unitId, Troops.ENUM_SQUARE_CATEGORY.Infantry))
                {
                    infantry_string = string.Format("{0}{1}{2}{3}{4}{5}{6}", infantry_string, unit.unitId,
                        Common.DATA_DELIMITER_LEVEL_0[0],
                        unit.unitCount, Common.DATA_DELIMITER_LEVEL_0[0], unit.unitMaxCount,
                        Common.DATA_DELIMITER_LEVEL_1[0]);
                }
                else if (IsValueInTable(unit.unitId, Troops.ENUM_SQUARE_CATEGORY.Cavalry))
                {
                    cavalry_string = string.Format("{0}{1}{2}{3}{4}{5}{6}", cavalry_string, unit.unitId,
                        Common.DATA_DELIMITER_LEVEL_0[0],
                        unit.unitCount, Common.DATA_DELIMITER_LEVEL_0[0], unit.unitMaxCount,
                        Common.DATA_DELIMITER_LEVEL_1[0]);
                }
                else if (IsValueInTable(unit.unitId, Troops.ENUM_SQUARE_CATEGORY.Archery))
                {
                    archery_string = string.Format("{0}{1}{2}{3}{4}{5}{6}", archery_string, unit.unitId,
                        Common.DATA_DELIMITER_LEVEL_0[0], unit.unitCount,
                        Common.DATA_DELIMITER_LEVEL_0[0], unit.unitMaxCount, Common.DATA_DELIMITER_LEVEL_1[0]);
                }
                else if (IsValueInTable(unit.unitId, Troops.ENUM_SQUARE_CATEGORY.Siege))
                {
                    siege_string = string.Format("{0}{1}{2}{3}{4}{5}{6}", siege_string, unit.unitId,
                        Common.DATA_DELIMITER_LEVEL_0[0], unit.unitCount,
                        Common.DATA_DELIMITER_LEVEL_0[0], unit.unitMaxCount, Common.DATA_DELIMITER_LEVEL_1[0]);
                }
            }


            foreach (var hero in heros)
            {
                hero_string = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", hero_string, hero.heroId + hero_id_offset,
                    Common.DATA_DELIMITER_LEVEL_0[0], "1", Common.DATA_DELIMITER_LEVEL_0[0], "1",
                    Common.DATA_DELIMITER_LEVEL_0[0], hero.label, Common.DATA_DELIMITER_LEVEL_1[0]);
            }

            if (!string.IsNullOrEmpty(hero_string))
            {
                square_string = string.Format("{0}{1}{2}{3}", 0, Common.DATA_DELIMITER_LEVEL_2[0], hero_string,
                Common.DATA_DELIMITER_LEVEL_3[0]);
            }
            if (!string.IsNullOrEmpty(cavalry_string))
            {
                cavalry_string = string.Format("{0}{1}{2}", 2, Common.DATA_DELIMITER_LEVEL_2[0],
                    cavalry_string);
                square_string = string.Format("{0}{1}{2}", square_string, cavalry_string,
                    Common.DATA_DELIMITER_LEVEL_3[0]);
            }

            if (!string.IsNullOrEmpty(infantry_string))
            {
                infantry_string = string.Format("{0}{1}{2}", 1, Common.DATA_DELIMITER_LEVEL_2[0],
                    infantry_string);
                square_string = string.Format("{0}{1}{2}", square_string, infantry_string,
                    Common.DATA_DELIMITER_LEVEL_3[0]);
            }

            if (!string.IsNullOrEmpty(archery_string))
            {
                archery_string = string.Format("{0}{1}{2}", 3, Common.DATA_DELIMITER_LEVEL_2[0],
                    archery_string);
                square_string = string.Format("{0}{1}{2}", square_string, archery_string,
                    Common.DATA_DELIMITER_LEVEL_3[0]);
            }

            if (!string.IsNullOrEmpty(siege_string))
            {
                siege_string = string.Format("{0}{1}{2}", 4, Common.DATA_DELIMITER_LEVEL_2[0],
                    siege_string);
                square_string = string.Format("{0}{1}{2}", square_string, siege_string,
                    Common.DATA_DELIMITER_LEVEL_3[0]);
            }

            square_string = string.Format("{0}{1}{2}", (int)type, Common.DATA_DELIMITER_LEVEL_3[0],
                square_string);

            return square_string;
        }

        public void InitUnitPrefabDict()
        {
            string prefab_dict_str = string.Empty;
            foreach (var item in TroopsDatas.Instance.GetUnitPrefabName().Values)
            {
                prefab_dict_str = string.Format("{0}{1}{2}{3}", prefab_dict_str, item.id,
                    Common.DATA_DELIMITER_LEVEL_0[0], item.name);
                if (!string.IsNullOrEmpty(prefab_dict_str))
                {
                    prefab_dict_str = string.Format("{0}{1}", prefab_dict_str, Common.DATA_DELIMITER_LEVEL_1[0]);
                }
            }

            CellDatas.InitUnitPrefabDict_S(prefab_dict_str);
        }


        private bool IsValueInTable(int v, Troops.ENUM_SQUARE_CATEGORY type)
        {
            foreach (var id in UnitId[type])
            {
                if (v == id)
                {
                    return true;
                }
            }

            return false;
        }

        public string GetMapCreateTroopDes(int heroId, int viceId, Dictionary<Int64, SoldierInfo> soldiers, Troops.ENMU_MATRIX_TYPE type= Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            lsUnitDatas.Clear();
            lsHeroDatas.Clear();
            if (m_SoldierProxy == null)
            {
                m_SoldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            }

            if (soldiers != null)
            {         
                foreach (var info in soldiers.Values)
                {
                    TroopsCell data = new TroopsCell();
                    if (type == Troops.ENMU_MATRIX_TYPE.BARBARIAN)
                    {
                        data.unitId = (int) info.id;
                    }
                    else
                    {
                        data.unitId = (int)info.id; //m_SoldierProxy.GetTemplateId((int) info.type, (int) info.level);
                    }
 
                    data.unitCount = (int) info.num;
                    data.unitMaxCount = (int) info.num;
                    data.unitType = (int) info.type;
                    data.unitLevel = (int) info.level;
                    data.unitserverId = (int) info.id;
                    lsUnitDatas.Add(data);
                }
            }


            int count = 1;
            if (viceId != 0)
            {
                count = 2;
            }

            int lable = 1;
            if (count >= 2)
            {
                lable = 0;
            }

            for (int i = 0; i < count; i++)
            {
                TroopsHero heroData = new TroopsHero();
                switch (i)
                {
                    case 0:
                        heroData.heroId = heroId;
                        heroData.label = "0";
                        break;
                    case 1:
                        heroData.heroId = viceId;
                        heroData.label ="2";
                        break;
                }

                lsHeroDatas.Add(heroData);
            }

            string des = GetSquareString(lsUnitDatas, lsHeroDatas, type);
            return des;
        }


        public string GetBarbarianDes(Troops.ENMU_MATRIX_TYPE type = Troops.ENMU_MATRIX_TYPE.COMMON,
            int troopsId = 0)
        {
            lsUnitDatas.Clear();
            lsHeroDatas.Clear();
            string des = GetSquareString(lsUnitDatas, lsHeroDatas, type);
            return des;
        }
    }
}