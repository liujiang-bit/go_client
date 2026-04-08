using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class FormationBarbarian : MonoBehaviour
    {
        public BarbarianFormationConfig[] m_squareRows;

        public List<float> m_initRowZ = new List<float>();

        private Dictionary<int, Dictionary<int, int[]>> m_squareInfo = new Dictionary<int, Dictionary<int, int[]>>();

        private int m_curFirstRowIdx = 1;

        private List<int> m_heros = new List<int>();

        public SquareRow HeroRow
        {
            get
            {
                return this.m_squareRows[0].GetComponent<SquareRow>();
            }
        }

        private void OnSpawn()
        {
            for (int i = 0; i < this.m_initRowZ.Count; i++)
            {
                Vector3 localPosition = this.m_squareRows[i].transform.localPosition;
                localPosition.z = this.m_initRowZ[i];
                this.m_squareRows[i].transform.localPosition = localPosition;
            }
            this.m_squareInfo.Clear();
            this.m_heros.Clear();
        }

        private void ParseFormationData(string square_info)
        {
            string[] array = square_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            string[] array2 = array[1].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < array.Length; i++)
            {
                string[] array3 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                int num = int.Parse(array3[0]);
                string[] array4 = array3[1].Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, int[]> dictionary;
                if (this.m_squareInfo.ContainsKey(num))
                {
                    dictionary = this.m_squareInfo[num];
                }
                else
                {
                    dictionary = new Dictionary<int, int[]>();
                    this.m_squareInfo.Add(num, dictionary);
                }
                for (int j = 0; j < array4.Length; j++)
                {
                    string[] array5 = array4[j].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                    int num2 = int.Parse(array5[0]);
                    if (num == 0)
                    {
                        this.m_heros.Insert(j, num2);
                    }
                    int[] array6;
                    if (dictionary.ContainsKey(num2))
                    {
                        array6 = dictionary[num2];
                    }
                    else
                    {
                        array6 = new int[2];
                        dictionary.Add(num2, array6);
                    }
                    array6[0] = int.Parse(array5[1]);
                    array6[1] = int.Parse(array5[2]);
                }
            }
        }

        public int GetAllUnitTotalNumByCategory(int category)
        {
            int num = 0;
            for (int i = 1; i < this.m_squareRows.Length; i++)
            {
                num += this.m_squareRows[i].UnitTotalNumByCategory(category);
            }
            return num;
        }

        public int GetAllUnitNumByCategory(int category)
        {
            int num = 0;
            for (int i = 1; i < this.m_squareRows.Length; i++)
            {
                num += this.m_squareRows[i].UnitNumByCategory(category);
            }
            return num;
        }

        public void DieUnitByCategory(int category, int num)
        {
            int num2 = num;
            int num3 = 1;
            while (num3 < this.m_squareRows.Length && num2 > 0)
            {
                int num4 = this.m_squareRows[num3].DieUnits(category, num2);
                num2 -= num4;
                num3++;
            }
        }

        public void InitFormationData(string square_info)
        {
            if (this.m_initRowZ.Count == 0)
            {
                for (int i = 0; i < this.m_squareRows.Length; i++)
                {
                    this.m_initRowZ.Insert(i, this.m_squareRows[i].transform.localPosition.z);
                }
            }
            this.ParseFormationData(square_info);
            for (int j = 0; j < this.m_heros.Count; j++)
            {
                this.m_squareRows[0].m_UnitDummys[j].unitType = this.m_heros[j];
                this.m_squareRows[0].m_UnitDummys[j].unitPrefab = UnityGameDatas.GetInstance().ReadUnitPrefabPathByType(this.m_heros[j]);
            }
        }

        public void SetFormationData(string square_info)
        {
            this.ParseFormationData(square_info);
            foreach (int current in this.m_squareInfo.Keys)
            {
                Dictionary<int, int[]> dictionary = this.m_squareInfo[current];
                int num = 0;
                int num2 = 0;
                foreach (KeyValuePair<int, int[]> current2 in dictionary)
                {
                    num += current2.Value[1];
                    num2 += current2.Value[0];
                }
                int allUnitTotalNumByCategory = this.GetAllUnitTotalNumByCategory(current);
                float num3 = (float)num2 / (float)num;
                int num4 = Mathf.CeilToInt((float)allUnitTotalNumByCategory * num3);
                int allUnitNumByCategory = this.GetAllUnitNumByCategory(current);
                if (allUnitNumByCategory > num4)
                {
                    this.DieUnitByCategory(current, allUnitNumByCategory - num4);
                }
            }
            this.CheckAndUpdateRowClear();
        }

        private void CheckAndUpdateRowClear()
        {
            int num = this.m_squareRows[1].AllUnitNum();
            if (num > 0)
            {
                return;
            }
            int num2 = 0;
            for (int i = 2; i < this.m_squareRows.Length; i++)
            {
                int num3 = this.m_squareRows[i].AllUnitNum();
                if (num3 > 0)
                {
                    num2 = i;
                    break;
                }
            }
            if (num2 == this.m_curFirstRowIdx || num2 >= this.m_squareRows.Length)
            {
                return;
            }
            this.m_curFirstRowIdx = num2;
            float z = this.m_squareRows[1].transform.localPosition.z;
            GameObject gameObject = this.m_squareRows[num2].gameObject;
            float num4 = z - gameObject.transform.localPosition.z;
            for (int j = num2; j < this.m_squareRows.Length; j++)
            {
                Transform transform = this.m_squareRows[j].transform;
                SquareRow component = transform.GetComponent<SquareRow>();
                component.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                if (j == num2)
                {
                    component.SetIsFirstRow(true);
                    int num5 = this.m_squareRows[j].m_UnitDummys.Length;
                    int num6 = Mathf.FloorToInt((float)num5 * 0.5f + 0.5f);
                    for (int k = 0; k < num5; k++)
                    {
                        Vector3 localPosition = this.m_squareRows[j].m_UnitDummys[k].unitDummy.localPosition;
                        if (k < num6)
                        {
                            localPosition.x -= 0.3f;
                        }
                        else
                        {
                            localPosition.x += 0.65f;
                        }
                        this.m_squareRows[j].m_UnitDummys[k].unitDummy.localPosition = localPosition;
                    }
                }
                Vector3 localPosition2 = transform.localPosition;
                localPosition2.z += num4;
                transform.localPosition = localPosition2;
            }
            Formation component2 = base.GetComponent<Formation>();
            component2.UpdateLod();
        }
    }
}