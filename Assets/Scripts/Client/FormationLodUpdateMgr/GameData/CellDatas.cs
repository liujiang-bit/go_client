using System;
using System.Collections.Generic;


namespace Client
{
    public class CellDatas
    {
        public static readonly CellDatas m_instance = new CellDatas();

        private static MatrixData[] m_square_data_array = new MatrixData[Enum.GetNames(typeof(Troops.ENMU_MATRIX_TYPE)).Length];

        private static HeroOffsetData[] m_hero_data_array = new HeroOffsetData[Enum.GetNames(typeof(Troops.ENMU_MATRIX_TYPE)).Length];

        private Dictionary<int, string> m_unit_prefab_dict = new Dictionary<int, string>();

        public static CellDatas GetInstance()
        {
            return m_instance;
        }

        public static void InitSquareData_S()
        {
            GetInstance().InitSquareData();
        }

        private void InitSquareData()
        {
            //for (int i = 0; i != m_square_data_array.Length; i++)
            //{
            //    UnitData[] array = new UnitData[5];
            //    m_square_data_array[i].m_unit_data = array;
            //    m_square_data_array[i].m_square_type = (Formation.ENMU_SQUARE_TYPE)i;
            //    string table_name = GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "max_number_in_row";
            //    LuaTable gameDataTable = GetGameDataTable(table_name);
            //    for (int j = 1; j != gameDataTable.length() + 1; j++)
            //    {
            //        LuaTable luaTable = (LuaTable)gameDataTable[j];
            //        int num = (int)(double)luaTable["type"];
            //        array[num].m_max_number_in_row = (int)(double)luaTable["num"];
            //    }
            //    table_name = GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "row_width";
            //    gameDataTable = GetGameDataTable(table_name);
            //    for (int k = 1; k != gameDataTable.length() + 1; k++)
            //    {
            //        LuaTable luaTable2 = (LuaTable)gameDataTable[k];
            //        int num2 = (int)(double)luaTable2["type"];
            //        array[num2].m_row_width = (float)(double)luaTable2["row_width"];
            //    }
            //    table_name = GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "forward_spacing";
            //    gameDataTable = GetGameDataTable(table_name);
            //    for (int l = 1; l != gameDataTable.length() + 1; l++)
            //    {
            //        LuaTable luaTable3 = (LuaTable)gameDataTable[l];
            //        int num3 = (int)(double)luaTable3["type"];
            //        array[num3].m_foward_spacing = (float)(double)luaTable3["spacing"];
            //    }
            //    table_name = GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "backward_spacing";
            //    gameDataTable = GetGameDataTable(table_name);
            //    for (int m = 1; m != gameDataTable.length() + 1; m++)
            //    {
            //        LuaTable luaTable4 = (LuaTable)gameDataTable[m];
            //        int num4 = (int)(double)luaTable4["type"];
            //        array[num4].m_backward_spacing = (float)(double)luaTable4["spacing"];
            //    }
            //    table_name = GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "number_by_sum";
            //    gameDataTable = GetGameDataTable(table_name);
            //    Dictionary<int, List<UnitNumberBySumData>> dictionary = new Dictionary<int, List<UnitNumberBySumData>>();
            //    for (int n = 1; n != gameDataTable.length() + 1; n++)
            //    {
            //        LuaTable luaTable5 = (LuaTable)gameDataTable[n];
            //        int key = (int)(double)luaTable5["type"];
            //        UnitNumberBySumData item = default(UnitNumberBySumData);
            //        item.number = (int)(double)luaTable5["num"];
            //        LuaTable luaTable6 = (LuaTable)luaTable5["range"];
            //        item.m_low_end = (int)(double)luaTable6[1];
            //        if (luaTable6.length() == 2)
            //        {
            //            item.m_up_end = (int)(double)luaTable6[2];
            //        }
            //        else
            //        {
            //            item.m_up_end = int.MaxValue;
            //        }
            //        if (dictionary.ContainsKey(key))
            //        {
            //            dictionary[key].Add(item);
            //            continue;
            //        }
            //        List<UnitNumberBySumData> list = new List<UnitNumberBySumData>();
            //        list.Add(item);
            //        dictionary.Add(key, list);
            //    }
            //    for (int num5 = 0; num5 != 5; num5++)
            //    {
            //        array[num5].m_number_by_sum_array = dictionary[num5].ToArray();
            //    }
            //    table_name = GetSquareConfigTabName((Formation.ENMU_SQUARE_TYPE)i) + "offset";
            //    gameDataTable = GetGameDataTable(table_name);
            //    HeroOffsetData heroOffsetData = default(HeroOffsetData);
            //    heroOffsetData.m_square_type = (Formation.ENMU_SQUARE_TYPE)i;
            //    heroOffsetData.m_heroOffset_data = new HeroOffsetItem[gameDataTable.length()];
            //    m_hero_data_array[i] = heroOffsetData;
            //    for (int num6 = 1; num6 <= gameDataTable.length(); num6++)
            //    {
            //        LuaTable luaTable7 = (LuaTable)gameDataTable[num6];
            //        HeroOffsetItem heroOffsetItem = default(HeroOffsetItem);
            //        heroOffsetItem.m_type = (int)(double)luaTable7["type"];
            //        heroOffsetItem.m_x_offset = (float)(double)luaTable7["x_offset"];
            //        heroOffsetItem.m_z_offset = (float)(double)luaTable7["z_offset"];
            //        heroOffsetData.m_heroOffset_data[num6 - 1] = heroOffsetItem;
            //    }
            //}

            for (int i = 0; i < m_square_data_array.Length; i++)
            {
                CellData[] array = new CellData[5];
                CellDatas.m_square_data_array[i].m_unit_data = array;
                CellDatas.m_square_data_array[i].m_square_type = (Troops.ENMU_MATRIX_TYPE)i;

                if (TroopsDatas.Instance.IsHaveListData(i))
                {
                    foreach (var data in TroopsDatas.Instance.GetListData((Troops.ENMU_MATRIX_TYPE)i))
                    {
                        int type = data.type;
                        array[type].m_max_number_in_row = data.num;
                    }
                }

                if (TroopsDatas.Instance.IsHaveListRow_Width(i))
                {
                    foreach (var data in TroopsDatas.Instance.GetListRow_Width((Troops.ENMU_MATRIX_TYPE)i))
                    {
                        int type = data.type;
                        array[type].m_row_width = data.numRowWidth;
                    }
                }

                if (TroopsDatas.Instance.IsHaveGetListForwardSpacingData(i))
                {
                    foreach (var data in TroopsDatas.Instance.GetListForwardSpacingData((Troops.ENMU_MATRIX_TYPE)i))
                    {
                        int type = data.type;
                        array[type].m_foward_spacing = data.spacing;
                    }
                }


                if (TroopsDatas.Instance.IsHaveGetListBackward_spacingData(i))
                {
                    foreach (var data in TroopsDatas.Instance.GetListBackward_spacingData((Troops.ENMU_MATRIX_TYPE)i))
                    {
                        int type = data.type;
                        array[type].m_backward_spacing = data.spacing;
                    }
                }


                Dictionary<int, List<CellNumByTotalData>>
                    dictionary = new Dictionary<int, List<CellNumByTotalData>>();

                if (TroopsDatas.Instance.IsHaveGetListDatabyNumberbyData(i))
                {
                    foreach (var data in TroopsDatas.Instance.GetListDatabyNumberbyData((Troops.ENMU_MATRIX_TYPE)i))
                    {
                        int key = data.type;
                        CellNumByTotalData item = default(CellNumByTotalData);
                        item.number = data.num;
                        item.m_low_end = data.range;
                        if (data.rangeMax > 0)
                        {
                            item.m_up_end = data.rangeMax;
                        }
                        else
                        {
                            item.m_up_end = int.MaxValue;
                        }

                        if (dictionary.ContainsKey(key))
                        {
                            dictionary[key].Add(item);
                        }
                        else
                        {
                            dictionary.Add(key, new List<CellNumByTotalData>
                            {
                                item
                            });
                        }
                    }

                    for (int num5 = 0; num5 < 5; num5++)
                    {
                        array[num5].m_number_by_sum_array = dictionary[num5].ToArray();
                    }
                }

                HeroOffsetData heroOffsetData = default(HeroOffsetData);
                heroOffsetData.m_square_type = (Troops.ENMU_MATRIX_TYPE)i + 1;
                heroOffsetData.m_heroOffset_data = new HeroOffsetItem[3];
                CellDatas.m_hero_data_array[i] = heroOffsetData;
                foreach (var data in TroopsDatas.Instance.GetSquareOffsets(i + 1))
                {
                    HeroOffsetItem heroOffsetItem = default(HeroOffsetItem);
                    heroOffsetItem.m_type = data.type;
                    heroOffsetItem.m_x_offset = data.x_offset;
                    heroOffsetItem.m_z_offset = data.z_offset;
                    heroOffsetData.m_heroOffset_data[data.type - 1] = heroOffsetItem;
                }
            }
        }

        public int ReadUnitDisplayNumberInRow(int unit_type, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            int num = 0;
            switch (unit_type)
            {
                case 0:
                    num = 1;
                    break;
                case 1:
                case 5:
                case 9:
                case 13:
                case 17:
                case 101:
                case 102:
                case 105:
                case 106:
                case 113:
                case 114:
                    num = 1;
                    break;
                case 2:
                case 6:
                case 10:
                case 14:
                case 18:
                case 107:
                case 108:
                case 109:
                case 110:
                    num = 2;
                    break;
                case 3:
                case 7:
                case 11:
                case 15:
                case 19:
                case 103:
                case 104:
                case 111:
                case 112:
                case 115:
                case 116:
                    num = 3;
                    break;
                case 4:
                case 8:
                case 12:
                case 16:
                case 20:
                    num = 4;
                    break;
            }
            return m_square_data_array[(int)formationType].m_unit_data[num].m_max_number_in_row;
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

        public int ReadUnitDisplayNumberByUnitSum(int unit_category, int number, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            CellNumByTotalData[] number_by_sum_array = m_square_data_array[(int)formationType].m_unit_data[unit_category].m_number_by_sum_array;
            for (int i = 0; i != number_by_sum_array.Length; i++)
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
            m_instance.m_unit_prefab_dict.Clear();
            string[] array = info.Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i != array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                int key = int.Parse(array2[0]);
                if (!m_instance.m_unit_prefab_dict.ContainsKey(key) && array2.Length==2)
                {
                    m_instance.m_unit_prefab_dict.Add(key, array2[1]);
                }
            }
        }

        public string ReadUnitPrefabPathByType(int type)
        {
            if (m_unit_prefab_dict.ContainsKey(type))
            {
                return m_unit_prefab_dict[type];
            }
            return string.Empty;
        }

        public float ReadUnitRowForwardSpacingByCategory(int category, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            return m_square_data_array[(int)formationType].m_unit_data[category].m_foward_spacing;
        }

        public float ReadUnitRowBackwardSpacingByCategory(int category, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            return m_square_data_array[(int)formationType].m_unit_data[category].m_backward_spacing;
        }

        public float ReadUnitRowWidthByCategory(int category, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            return m_square_data_array[(int)formationType].m_unit_data[category].m_row_width;
        }

        public float ReadHeroXOffset(int type, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            return m_hero_data_array[(int)formationType].m_heroOffset_data[type - 1].m_x_offset;
        }

        public float ReadHeroZOffset(int type, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            return m_hero_data_array[(int)formationType].m_heroOffset_data[type - 1].m_z_offset;
        }
    }
}