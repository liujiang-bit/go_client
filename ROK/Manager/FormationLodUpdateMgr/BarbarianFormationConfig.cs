using Skyunion;
using System;
using UnityEngine;

namespace ROK
{
    [ExecuteInEditMode]
    public class BarbarianFormationConfig : MonoBehaviour
    {
        [Serializable]
        public class UnitDummyConfig
        {
            public Transform unitDummy;

            public string unitPrefab = string.Empty;

            public int unitType;

            public string cloneToPrefab = string.Empty;
        }

        public BarbarianFormationConfig.UnitDummyConfig[] m_UnitDummys;

        public int m_rowNum;

        public int RowNum
        {
            get
            {
                return this.m_rowNum;
            }
        }

        public int UnitTotalNumByCategory(int category)
        {
            int num = 0;
            for (int i = 0; i < this.m_UnitDummys.Length; i++)
            {
                if (this.m_UnitDummys[i].unitType != 0)
                {
                    if ((this.m_UnitDummys[i].unitType - 1) % 4 + 1 == category)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int UnitNumByCategory(int category)
        {
            int num = 0;
            for (int i = 0; i < this.m_UnitDummys.Length; i++)
            {
                if (this.m_UnitDummys[i].unitType != 0)
                {
                    if ((this.m_UnitDummys[i].unitType - 1) % 4 + 1 == category)
                    {
                        UnitDummy component = this.m_UnitDummys[i].unitDummy.GetComponent<UnitDummy>();
                        UnitBase unit = component.m_unit;
                        if (unit != null)
                        {
                            num++;
                        }
                    }
                }
            }
            return num;
        }

        public int DieUnits(int category, int num)
        {
            int num2 = num;
            int num3 = 0;
            while (num3 < this.m_UnitDummys.Length && num2 > 0)
            {
                if ((this.m_UnitDummys[num3].unitType - 1) % 4 + 1 == category)
                {
                    UnitDummy component = this.m_UnitDummys[num3].unitDummy.GetComponent<UnitDummy>();
                    UnitBase unit = component.m_unit;
                    if (unit != null)
                    {
                        CoreUtils.assetService.Destroy(unit.gameObject);
                        unit.PlayDeadParticle();
                        component.m_unit = null;
                        num2--;
                    }
                }
                num3++;
            }
            return num - num2;
        }

        public int AllUnitNum()
        {
            int num = 0;
            for (int i = 0; i < this.m_UnitDummys.Length; i++)
            {
                if (this.m_UnitDummys[i].unitType != 0)
                {
                    UnitDummy component = this.m_UnitDummys[i].unitDummy.GetComponent<UnitDummy>();
                    UnitBase unit = component.m_unit;
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