using Skyunion;
using System;
using UnityEngine;

namespace Client
{
    [ExecuteInEditMode]
    public class BarbarianConfig : MonoBehaviour
    {
        [Serializable]
        public class UnitDummyConfig
        {
            public Transform unitDummy;

            public string unitPrefab = string.Empty;

            public int unitType;
        }

        public UnitDummyConfig[] m_UnitDummys;

        public int m_rowNum;
        public int RowNum => m_rowNum;

        public int UnitTotalNumByCategory(int category)
        {
            int num = 0;
            for (int i = 0; i < m_UnitDummys.Length; i++)
            {
                if (m_UnitDummys[i].unitType != 0 && (m_UnitDummys[i].unitType - 1) % 4 + 1 == category)
                {
                    num++;
                }
            }
            return num;
        }

        public int UnitNumByCategory(int category)
        {
            int num = 0;
            for (int i = 0; i < m_UnitDummys.Length; i++)
            {
                if (m_UnitDummys[i].unitType != 0 && (m_UnitDummys[i].unitType - 1) % 4 + 1 == category)
                {
                    CellClone component = m_UnitDummys[i].unitDummy.GetComponent<CellClone>();
                    CellBase unit = component.m_unit;
                    if (unit != null)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int DieUnits(int category, int num)
        {
            int num2 = num;
            for (int i = 0; i < m_UnitDummys.Length; i++)
            {
                if (num2 <= 0)
                {
                    break;
                }
                if ((m_UnitDummys[i].unitType - 1) % 4 + 1 == category)
                {
                    CellClone component = m_UnitDummys[i].unitDummy.GetComponent<CellClone>();
                    CellBase unit = component.m_unit;
                    if (unit != null)
                    {
                        unit.PlayDeadParticle();
                        CoreUtils.assetService.Destroy(unit.gameObject);
                        component.m_unit = null;
                        num2--;
                    }
                }
            }
            return num - num2;
        }

        public int AllUnitNum()
        {
            int num = 0;
            for (int i = 0; i < m_UnitDummys.Length; i++)
            {
                if (m_UnitDummys[i].unitType != 0)
                {
                    CellClone component = m_UnitDummys[i].unitDummy.GetComponent<CellClone>();
                    CellBase unit = component.m_unit;
                    if (unit != null)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        private void Start()
        {
        }
    }
}