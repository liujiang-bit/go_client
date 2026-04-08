using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class MatrixRowInfo
    {
        private Dictionary<int, int> m_org_unit_sum = new Dictionary<int, int>();

        private Dictionary<int, int> m_cur_unit_sum = new Dictionary<int, int>();

        private Dictionary<int, int> m_unit_priority = new Dictionary<int, int>();

        private List<List<int>> m_last_unit_map = new List<List<int>>();

        private int m_row_category;

        private Troops.ENMU_MATRIX_TYPE m_formation_type;

        private int m_org_row_number;

        private int m_cur_row_number;

        public int rowCategory
        {
            get
            {
                return m_row_category;
            }
            set
            {
            }
        }

        public int orgRowNumber
        {
            get
            {
                return m_org_row_number;
            }
            set
            {
                m_org_row_number = value;
            }
        }

        public int curRowNumber
        {
            get
            {
                return m_cur_row_number;
            }
            set
            {
                m_cur_row_number = value;
            }
        }

        public MatrixRowInfo(string info_str, int org_row_number, int row_category, Troops.ENMU_MATRIX_TYPE formationType = Troops.ENMU_MATRIX_TYPE.COMMON)
        {
            m_org_row_number = org_row_number;
            m_cur_row_number = m_org_row_number;
            m_row_category = row_category;
            m_formation_type = formationType;
            if (m_formation_type != Troops.ENMU_MATRIX_TYPE.BARBARIAN)
            {
                UpdateUnitNumber(info_str);
            }
        }

        public Dictionary<string, int> GetDisplayUnitNumber(int lod_level)
        {
            return new Dictionary<string, int>();
        }

        public bool UpdateUnitNumber(string info_str)
        {
            m_org_unit_sum.Clear();
            m_cur_unit_sum.Clear();
            m_last_unit_map.Clear();
            m_unit_priority.Clear();
            string[] array = info_str.Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i != array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                int key = int.Parse(array2[0]);
                int value = int.Parse(array2[1]);
                int value2 = int.Parse(array2[2]);
                if (array2.Length >= 4)
                {
                    m_unit_priority.Add(key, int.Parse(array2[3]));
                }
                m_org_unit_sum.Add(key, value2);
                m_cur_unit_sum.Add(key, value);
            }
            int num = GetRawRowNumberByUnitNumber(GetRawUnitNumberSum(m_cur_unit_sum, m_row_category, m_formation_type), m_row_category, m_formation_type);
            if (num > m_org_row_number)
            {
                num = m_org_row_number;
            }
            if (num != m_cur_row_number)
            {
                m_cur_row_number = num;
                return true;
            }
            return false;
        }

        public int GetUnitPriority(int unit_type)
        {
            int result = 1;
            if (m_unit_priority.ContainsKey(unit_type))
            {
                result = m_unit_priority[unit_type];
            }
            return result;
        }

        private static int GetRawUnitNumberSum(Dictionary<int, int> dict_unit_sum, int unit_category, Troops.ENMU_MATRIX_TYPE ftype)
        {
            int num = 0;
            foreach (int value in dict_unit_sum.Values)
            {
                num += value;
            }
            return GetDisplayUnitNumberByUnitCount(num, unit_category, ftype);
        }

        private static int GetUnitSum(Dictionary<int, int> dict_unit_sum)
        {
            int num = 0;
            foreach (int value in dict_unit_sum.Values)
            {
                num += value;
            }
            return num;
        }

        public int GetDisplayUnitNum()
        {
            if (m_last_unit_map.Count == 0)
            {
                GetDisplayUnitMap();
            }
            int num = 0;
            for (int i = 0; i < m_last_unit_map.Count; i++)
            {
                for (int j = 0; j < m_last_unit_map[i].Count; j++)
                {
                    num++;
                }
            }
            return num;
        }

        public List<List<int>> GetDisplayUnitMap()
        {
            if (m_last_unit_map.Count != 0)
            {
                List<List<int>> list = new List<List<int>>();
                for (int i = 0; i != m_last_unit_map.Count; i++)
                {
                    List<int> list2 = new List<int>();
                    for (int j = 0; j != m_last_unit_map[i].Count; j++)
                    {
                        list2.Add(m_last_unit_map[i][j]);
                    }
                    list.Add(list2);
                }
                return list;
            }
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            int unitSum = GetUnitSum(m_cur_unit_sum);
            int num = GetRawUnitNumberSum(m_cur_unit_sum, m_row_category, m_formation_type);
            int num2 = CellDatas.GetInstance().ReadUnitDisplayNumberInRow(m_row_category, m_formation_type);
            int num3 = m_cur_row_number * num2;
            if (num > num3)
            {
                num = num3;
            }
            foreach (int key2 in m_cur_unit_sum.Keys)
            {
                if (unitSum == 0)
                {
                    dictionary.Add(key2, 0);
                }
                else
                {
                    dictionary.Add(key2, Mathf.RoundToInt((float)m_cur_unit_sum[key2] / (float)unitSum * (float)num));
                }
            }
            IOrderedEnumerable<KeyValuePair<int, int>> orderedEnumerable = from pair in dictionary
                                                                           orderby pair.Value descending
                                                                           select pair;
            int num4 = num;
            dictionary = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> item in orderedEnumerable)
            {
                if (num4 <= 0)
                {
                    dictionary.Add(item.Key, 0);
                }
                else
                {
                    int value = 0;
                    if (num4 >= item.Value && item.Value >= 1)
                    {
                        num4 -= item.Value;
                        value = item.Value;
                    }
                    else if (num4 != 0)
                    {
                        value = 1;
                        num4--;
                    }
                    dictionary.Add(item.Key, value);
                }
            }
            int num5 = dictionary.Keys.Max();
            if (num != 0 && dictionary[num5] == 0)
            {
                dictionary[num5] = 1;
                int num6 = num5;
                foreach (KeyValuePair<int, int> item2 in dictionary)
                {
                    if (item2.Key != num5 && dictionary[item2.Key] > 0)
                    {
                        num6 = item2.Key;
                        break;
                    }
                }
                Dictionary<int, int> dictionary2;
                int key;
                (dictionary2 = dictionary)[key = num6] = dictionary2[key] - 1;
            }
            List<int> list3 = new List<int>();
            foreach (KeyValuePair<int, int> item3 in dictionary)
            {
                for (int k = 0; k != item3.Value; k++)
                {
                    list3.Add(item3.Key);
                }
            }
            int num7 = Mathf.CeilToInt((float)list3.Count / (float)num2);
            int num8 = (num7 != 0) ? Mathf.RoundToInt((float)list3.Count / (float)num7) : 0;
            list3 = (from item in list3
                     orderby UnityEngine.Random.value
                     select item).ToList();
            List<List<int>> list4 = new List<List<int>>();
            for (int l = 0; l <= num7 - 2; l++)
            {
                list4.Add(list3.GetRange(l * num8, num8));
            }
            if (num7 != 0)
            {
                int num9 = (num7 - 1) * num8;
                list4.Add(list3.GetRange(num9, list3.Count - num9));
            }
            for (int m = 0; m != list4.Count; m++)
            {
                List<int> list5 = new List<int>();
                for (int n = 0; n != list4[m].Count; n++)
                {
                    list5.Add(list4[m][n]);
                }
                m_last_unit_map.Add(list5);
            }
            return list4;
        }

        private int GetLodXRowNumber(int lod_level, int lod2_row_number)
        {
            if (lod_level == 2)
            {
                return lod2_row_number;
            }
            return lod2_row_number * (3 - lod_level);
        }

        public int GetLodXUnitNumber(int lod_level, int lod2_unit_number)
        {
            if (lod_level == 2)
            {
                return lod2_unit_number;
            }
            return lod2_unit_number * (3 - lod_level) + 1;
        }

        public static int GetRawRowNumberByUnitNumber(int unit_number, int unit_category, Troops.ENMU_MATRIX_TYPE fType)
        {
            return Mathf.CeilToInt((float)unit_number / (float)CellDatas.GetInstance().ReadUnitDisplayNumberInRow(unit_category, fType));
        }

        public static int GetDisplayUnitNumberByString(string info_str, int unit_category, Troops.ENMU_MATRIX_TYPE ftype)
        {
            string[] array = info_str.Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int i = 0; i != array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                dictionary.Add(int.Parse(array2[0]), int.Parse(array2[1]));
            }
            return GetRawUnitNumberSum(dictionary, unit_category, ftype);
        }

        public static int GetDisplayUnitNumberByUnitCount(int unit_count, int unit_category, Troops.ENMU_MATRIX_TYPE fType)
        {
            return CellDatas.GetInstance().ReadUnitDisplayNumberByUnitSum(unit_category, unit_count, fType);
        }
    }
}