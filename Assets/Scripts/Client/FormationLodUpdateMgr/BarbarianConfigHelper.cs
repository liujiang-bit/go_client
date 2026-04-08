using System;
using UnityEngine;

namespace Client
{
    [ExecuteInEditMode]
    public class BarbarianConfigHelper : MonoBehaviour
    {
        [Serializable]
        public class UnitDummyConfig
        {
            public Transform unitDummy;

            public string unitPrefab = string.Empty;

            public int unitType;
        }

        public UnitDummyConfig[] m_HeroUnitDummys;

        public UnitDummyConfig[] m_UnitDummys;

        public bool m_isDebug;

        private void Start()
        {
        }
    }
}