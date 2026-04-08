using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 野蛮人布阵
    /// </summary>
    public class Barbarian : MonoBehaviour
    {
        public BarbarianConfig[] m_squareRows;

        public List<float> m_initRowZ = new List<float>();

        private Dictionary<int, Dictionary<int, int[]>> m_squareInfo = new Dictionary<int, Dictionary<int, int[]>>();

        private int m_curFirstRowIdx = 1;

        private List<int> m_heros = new List<int>();

        public MatrixRow HeroRow => m_squareRows[0].GetComponent<MatrixRow>();

        private void OnSpawn()
        {
            for (int i = 0; i < m_initRowZ.Count; i++)
            {
                Vector3 localPosition = m_squareRows[i].transform.localPosition;
                localPosition.z = m_initRowZ[i];
                m_squareRows[i].transform.localPosition = localPosition;
            }
            m_squareInfo.Clear();
            m_heros.Clear();
        }

        private void ParseFormationData(string square_info)
        {
            string[] array = square_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            //array[1].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length == 1)
                return;

            for (int i = 1; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                int unitType = int.Parse(array2[0]);
                string[] array3 = array2[1].Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, int[]> dictionary;
                if (m_squareInfo.ContainsKey(unitType))
                {
                    dictionary = m_squareInfo[unitType];
                }
                else
                {
                    dictionary = new Dictionary<int, int[]>();
                    m_squareInfo.Add(unitType, dictionary);
                }
                for (int j = 0; j < array3.Length; j++)
                {
                    string[] array4 = array3[j].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                    int unitId = int.Parse(array4[0]);
                    if (unitType == 0)
                    {
                        m_heros.Insert(j, unitId);
                    }
                    int[] array5;
                    if (dictionary.ContainsKey(unitId))
                    {
                        array5 = dictionary[unitId];
                    }
                    else
                    {
                        array5 = new int[2];
                        dictionary.Add(unitId, array5);
                    }
                    array5[0] = int.Parse(array4[1]);
                    array5[1] = int.Parse(array4[2]);
                }
            }
        }

        public int GetAllUnitTotalNumByCategory(int category)
        {
            int num = 0;
            for (int i = 1; i < m_squareRows.Length; i++)
            {
                num += m_squareRows[i].UnitTotalNumByCategory(category);
            }
            return num;
        }

        public int GetAllUnitNumByCategory(int category)
        {
            int num = 0;
            for (int i = 1; i < m_squareRows.Length; i++)
            {
                num += m_squareRows[i].UnitNumByCategory(category);
            }
            return num;
        }

        public void DieUnitByCategory(int category, int num)
        {
            int num2 = num;
            for (int i = 1; i < m_squareRows.Length; i++)
            {
                if (num2 <= 0)
                {
                    break;
                }
                int num3 = m_squareRows[i].DieUnits(category, num2);
                num2 -= num3;
            }
        }

        public void InitFormationData(string square_info)
        {
            if (m_initRowZ.Count == 0)
            {
                for (int i = 0; i < m_squareRows.Length; i++)
                {
                    List<float> initRowZ = m_initRowZ;
                    int index = i;
                    Vector3 localPosition = m_squareRows[i].transform.localPosition;
                    initRowZ.Insert(index, localPosition.z);
                }
            }
            ParseFormationData(square_info);
            for (int j = 0; j < m_heros.Count; j++)
            {
                m_squareRows[0].m_UnitDummys[j].unitType = m_heros[j];
                m_squareRows[0].m_UnitDummys[j].unitPrefab = CellDatas.GetInstance().ReadUnitPrefabPathByType(m_heros[j]);
            }
        }

        public void SetFormationData(string square_info)
        {
            ParseFormationData(square_info);
            foreach (int key in m_squareInfo.Keys)
            {
                Dictionary<int, int[]> dictionary = m_squareInfo[key];
                int num = 0;
                int num2 = 0;
                foreach (KeyValuePair<int, int[]> item in dictionary)
                {
                    num += item.Value[1];
                    num2 += item.Value[0];
                }
                int allUnitTotalNumByCategory = GetAllUnitTotalNumByCategory(key);
                float num3 = (float)num2 / (float)num;
                int num4 = Mathf.CeilToInt((float)allUnitTotalNumByCategory * num3);
                int allUnitNumByCategory = GetAllUnitNumByCategory(key);
                if (allUnitNumByCategory > num4)
                {
                    DieUnitByCategory(key, allUnitNumByCategory - num4);
                }
            }
            CheckAndUpdateRowClear();
        }

        private void CheckAndUpdateRowClear()
        {
            int num = m_squareRows[1].AllUnitNum();
            if (num > 0)
            {
                return;
            }
            int num2 = 0;
            for (int i = 2; i < m_squareRows.Length; i++)
            {
                int num3 = m_squareRows[i].AllUnitNum();
                if (num3 > 0)
                {
                    num2 = i;
                    break;
                }
            }
            if (num2 == m_curFirstRowIdx || num2 >= m_squareRows.Length)
            {
                return;
            }
            m_curFirstRowIdx = num2;
            Vector3 localPosition = m_squareRows[1].transform.localPosition;
            float z = localPosition.z;
            GameObject gameObject = m_squareRows[num2].gameObject;
            float num4 = z;
            Vector3 localPosition2 = gameObject.transform.localPosition;
            float num5 = num4 - localPosition2.z;
            for (int j = num2; j < m_squareRows.Length; j++)
            {
                Transform transform = m_squareRows[j].transform;
                MatrixRow component = transform.GetComponent<MatrixRow>();
                component.ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                if (j == num2)
                {
                    component.SetIsFirstRow(isFirst: true);
                    int num6 = m_squareRows[j].m_UnitDummys.Length;
                    int num7 = Mathf.FloorToInt((float)num6 * 0.5f + 0.5f);
                    for (int k = 0; k < num6; k++)
                    {
                        Vector3 localPosition3 = m_squareRows[j].m_UnitDummys[k].unitDummy.localPosition;
                        if (k < num7)
                        {
                            localPosition3.x -= 0.3f;
                        }
                        else
                        {
                            localPosition3.x += 0.65f;
                        }
                        m_squareRows[j].m_UnitDummys[k].unitDummy.localPosition = localPosition3;
                    }
                }
                Vector3 localPosition4 = transform.localPosition;
                localPosition4.z += num5;
                transform.localPosition = localPosition4;
            }
            Troops component2 = GetComponent<Troops>();
            component2.UpdateLod();
        }
    }
}