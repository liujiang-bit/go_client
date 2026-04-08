using System;
using UnityEngine;

namespace ROK
{
    [ExecuteInEditMode]
    public class BarbarianFormationConfigHelper : MonoBehaviour
    {
        [Serializable]
        public class UnitDummyConfig
        {
            public Transform unitDummy;

            public string unitPrefab = string.Empty;

            public int unitType;
        }

        public BarbarianFormationConfigHelper.UnitDummyConfig[] m_HeroUnitDummys;

        public BarbarianFormationConfigHelper.UnitDummyConfig[] m_UnitDummys;

        public bool m_isDebug;

        private void Start()
        {
        }
    }
}
