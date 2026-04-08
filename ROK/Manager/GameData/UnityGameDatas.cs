using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class UnityGameDatas
    {
        public static readonly UnityGameDatas m_instance = new UnityGameDatas();

        private static SquareData[] m_square_data_array = new SquareData[Enum.GetNames(typeof(Formation.ENMU_SQUARE_TYPE)).Length];

        private static HeroOffsetData[] m_hero_data_array = new HeroOffsetData[Enum.GetNames(typeof(Formation.ENMU_SQUARE_TYPE)).Length];

        private static int bar_color_width = 4;

        private static int bar_color_height = 4;

        private Dictionary<int, string> m_unit_prefab_dict = new Dictionary<int, string>();

        public static UnityGameDatas GetInstance()
        {
            return UnityGameDatas.m_instance;
        }

        public static void InitSquareData_S()
        {
            UnityGameDatas.GetInstance().InitSquareData();
        }

        public static void InitBarColorTexSize_S(int width, int height)
        {
            UnityGameDatas.GetInstance().InitBarColorTexSize(width, height);
        }

        private void InitBarColorTexSize(int width, int height)
        {
            UnityGameDatas.bar_color_width = width;
            UnityGameDatas.bar_color_height = height;
            Shader.SetGlobalFloat("_BarColorTexSizeWidth", (float)width);
            Shader.SetGlobalFloat("_BarColorTexSizeHeight", (float)height);
            Shader.SetGlobalFloat("_CityTransparency", 1f);
        }

        private void InitSquareData()
        {
            //for (int i = 0; i < UnityGameDatas.m_square_data_array.Length; i++)
            //{
            //	UnitData[] array = new UnitData[5];
            //	UnityGameDatas.m_square_data_array[i].m_unit_data = array;
            //	UnityGameDatas.m_square_data_array[i].m_square_type = (Formation.ENMU_SQUARE_TYPE)i;
            //	string table_name = this.GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "max_number_in_row";
            //	LuaTable gameDataTable = this.GetGameDataTable(table_name);
            //	for (int j = 1; j < gameDataTable.length() + 1; j++)
            //	{
            //		LuaTable luaTable = (LuaTable)gameDataTable[j];
            //		int num = (int)((double)luaTable["type"]);
            //		array[num].m_max_number_in_row = (int)((double)luaTable["num"]);
            //	}
            //	table_name = this.GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "row_width";
            //	gameDataTable = this.GetGameDataTable(table_name);
            //	for (int k = 1; k < gameDataTable.length() + 1; k++)
            //	{
            //		LuaTable luaTable2 = (LuaTable)gameDataTable[k];
            //		int num2 = (int)((double)luaTable2["type"]);
            //		array[num2].m_row_width = (float)((double)luaTable2["row_width"]);
            //	}
            //	table_name = this.GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "forward_spacing";
            //	gameDataTable = this.GetGameDataTable(table_name);
            //	for (int l = 1; l < gameDataTable.length() + 1; l++)
            //	{
            //		LuaTable luaTable3 = (LuaTable)gameDataTable[l];
            //		int num3 = (int)((double)luaTable3["type"]);
            //		array[num3].m_foward_spacing = (float)((double)luaTable3["spacing"]);
            //	}
            //	table_name = this.GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "backward_spacing";
            //	gameDataTable = this.GetGameDataTable(table_name);
            //	for (int m = 1; m < gameDataTable.length() + 1; m++)
            //	{
            //		LuaTable luaTable4 = (LuaTable)gameDataTable[m];
            //		int num4 = (int)((double)luaTable4["type"]);
            //		array[num4].m_backward_spacing = (float)((double)luaTable4["spacing"]);
            //	}
            //	table_name = this.GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "number_by_sum";
            //	gameDataTable = this.GetGameDataTable(table_name);
            //	Dictionary<int, List<UnitNumberBySumData>> dictionary = new Dictionary<int, List<UnitNumberBySumData>>();
            //	for (int n = 1; n < gameDataTable.length() + 1; n++)
            //	{
            //		LuaTable luaTable5 = (LuaTable)gameDataTable[n];
            //		int key = (int)((double)luaTable5["type"]);
            //		UnitNumberBySumData item = default(UnitNumberBySumData);
            //		item.number = (int)((double)luaTable5["num"]);
            //		LuaTable luaTable6 = (LuaTable)luaTable5["range"];
            //		item.m_low_end = (int)((double)luaTable6[1]);
            //		if (luaTable6.length() == 2)
            //		{
            //			item.m_up_end = (int)((double)luaTable6[2]);
            //		}
            //		else
            //		{
            //			item.m_up_end = 2147483647;
            //		}
            //		if (dictionary.ContainsKey(key))
            //		{
            //			dictionary[key].Add(item);
            //		}
            //		else
            //		{
            //			dictionary.Add(key, new List<UnitNumberBySumData>
            //			{
            //				item
            //			});
            //		}
            //	}
            //	for (int num5 = 0; num5 < 5; num5++)
            //	{
            //		array[num5].m_number_by_sum_array = dictionary[num5].ToArray();
            //	}
            //	table_name = this.GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "offset";
            //	gameDataTable = this.GetGameDataTable(table_name);
            //	HeroOffsetData heroOffsetData = default(HeroOffsetData);
            //	heroOffsetData.m_square_type = (Formation.ENMU_SQUARE_TYPE)i;
            //	heroOffsetData.m_heroOffset_data = new HeroOffsetItem[gameDataTable.length()];
            //	UnityGameDatas.m_hero_data_array[i] = heroOffsetData;
            //	for (int num6 = 1; num6 <= gameDataTable.length(); num6++)
            //	{
            //		LuaTable luaTable7 = (LuaTable)gameDataTable[num6];
            //		HeroOffsetItem heroOffsetItem = default(HeroOffsetItem);
            //		heroOffsetItem.m_type = (int)((double)luaTable7["type"]);
            //		heroOffsetItem.m_x_offset = (float)((double)luaTable7["x_offset"]);
            //		heroOffsetItem.m_z_offset = (float)((double)luaTable7["z_offset"]);
            //		heroOffsetData.m_heroOffset_data[num6 - 1] = heroOffsetItem;
            //	}
            //}
        }

        public string GetSquareConfigTabName(Formation.ENMU_SQUARE_TYPE formationType)
        {
            if (formationType == Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                return "BarbarianSquare_";
            }
            if (formationType == Formation.ENMU_SQUARE_TYPE.RALLY)
            {
                return "square_rally_";
            }
            if (formationType == Formation.ENMU_SQUARE_TYPE.SHAMAN_GUARDIAN)
            {
                return "square_shaman_";
            }
            return "square_";
        }

        public int ReadUnitDisplayNumberInRow(int unit_type, Formation.ENMU_SQUARE_TYPE formationType = Formation.ENMU_SQUARE_TYPE.COMMON)
        {
            int num = 0;
            if (unit_type == 0)
            {
                num = 1;
            }
            else if (unit_type == 1 || unit_type == 5 || unit_type == 9 || unit_type == 13 || unit_type == 17 || unit_type == 101 || unit_type == 102 || unit_type == 105 || unit_type == 106 || unit_type == 113 || unit_type == 114)
            {
                num = 1;
            }
            else if (unit_type == 2 || unit_type == 6 || unit_type == 10 || unit_type == 14 || unit_type == 18 || unit_type == 107 || unit_type == 108 || unit_type == 109 || unit_type == 110)
            {
                num = 2;
            }
            else if (unit_type == 3 || unit_type == 7 || unit_type == 11 || unit_type == 15 || unit_type == 19 || unit_type == 103 || unit_type == 104 || unit_type == 111 || unit_type == 112 || unit_type == 115 || unit_type == 116)
            {
                num = 3;
            }
            else if (unit_type == 4 || unit_type == 8 || unit_type == 12 || unit_type == 16 || unit_type == 20)
            {
                num = 4;
            }
            return UnityGameDatas.m_square_data_array[(int)formationType].m_unit_data[num].m_max_number_in_row;
        }

        public int ReadUnitLodMultiplier(int unit_category, int lod_level)
        {
            if (unit_category == 0)
            {
                return 1;
            }
            return 3 - lod_level;
        }

        public int ReadUnitLodAddend(int unit_category, int lod_level)
        {
            if (unit_category == 0)
            {
                return 0;
            }
            return 1;
        }

        public int ReadUnitDisplayNumberByUnitSum(int unit_category, int number, Formation.ENMU_SQUARE_TYPE formationType = Formation.ENMU_SQUARE_TYPE.COMMON)
        {
            UnitNumberBySumData[] number_by_sum_array = UnityGameDatas.m_square_data_array[(int)formationType].m_unit_data[unit_category].m_number_by_sum_array;
            for (int i = 0; i < number_by_sum_array.Length; i++)
            {
                if (number >= number_by_sum_array[i].m_low_end && number < number_by_sum_array[i].m_up_end)
                {
                    return number_by_sum_array[i].number;
                }
            }
            return 0;
        }

        public static void InitUnitPrefabDict_S(string info)
        {
            UnityGameDatas.m_instance.m_unit_prefab_dict.Clear();
            string[] array = info.Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                int key = int.Parse(array2[0]);
                if (!UnityGameDatas.m_instance.m_unit_prefab_dict.ContainsKey(key))
                {
                    UnityGameDatas.m_instance.m_unit_prefab_dict.Add(key, array2[1]);
                }
            }
        }

        public string ReadUnitPrefabPathByType(int type)
        {
            if (this.m_unit_prefab_dict.ContainsKey(type))
            {
                return this.m_unit_prefab_dict[type];
            }
            return string.Empty;
        }

        public float ReadUnitRowForwardSpacingByCategory(int category, Formation.ENMU_SQUARE_TYPE formationType = Formation.ENMU_SQUARE_TYPE.COMMON)
        {
            return UnityGameDatas.m_square_data_array[(int)formationType].m_unit_data[category].m_foward_spacing;
        }

        public float ReadUnitRowBackwardSpacingByCategory(int category, Formation.ENMU_SQUARE_TYPE formationType = Formation.ENMU_SQUARE_TYPE.COMMON)
        {
            return UnityGameDatas.m_square_data_array[(int)formationType].m_unit_data[category].m_backward_spacing;
        }

        public float ReadUnitRowWidthByCategory(int category, Formation.ENMU_SQUARE_TYPE formationType = Formation.ENMU_SQUARE_TYPE.COMMON)
        {
            return UnityGameDatas.m_square_data_array[(int)formationType].m_unit_data[category].m_row_width;
        }

        //> todo
        //public LuaTable GetGameDataTable(string table_name)
        //{
        //	table_name = "GameDatas." + table_name;
        //	return GameRoot.GetLuaSvrStatic().luaState.getTable(table_name);
        //}

        public float ReadHeroXOffset(int type, Formation.ENMU_SQUARE_TYPE formationType = Formation.ENMU_SQUARE_TYPE.COMMON)
        {
            return UnityGameDatas.m_hero_data_array[(int)formationType].m_heroOffset_data[type - 1].m_x_offset;
        }

        public float ReadHeroZOffset(int type, Formation.ENMU_SQUARE_TYPE formationType = Formation.ENMU_SQUARE_TYPE.COMMON)
        {
            return UnityGameDatas.m_hero_data_array[(int)formationType].m_heroOffset_data[type - 1].m_z_offset;
        }
    }
}