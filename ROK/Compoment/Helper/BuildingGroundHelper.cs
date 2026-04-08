using System;
using UnityEngine;

namespace ROK
{
    public class BuildingGroundHelper : MonoBehaviour
    {
        public enum BuildingState
        {
            BUILDING_NONE,
            BUILDING_IN_CITY,
            BUILDING_IN_MAP
        }

        public GameObject m_floor_tile;

        public GameObject m_ground;

        private BuildingGroundHelper.BuildingState m_cur_state;

        public static void BuildingGroundChangeS(BuildingGroundHelper bgh, int bState)
        {
            if (bgh != null)
            {
                bgh.BuildingGroundChange((BuildingGroundHelper.BuildingState)bState);
            }
        }

        private void BuildingGroundChange(BuildingGroundHelper.BuildingState bs)
        {
            if (this.m_cur_state == bs)
            {
                return;
            }
            if (bs == BuildingGroundHelper.BuildingState.BUILDING_IN_CITY)
            {
                this.m_floor_tile.SetActive(true);
                this.m_ground.SetActive(false);
            }
            else if (bs == BuildingGroundHelper.BuildingState.BUILDING_IN_MAP)
            {
                this.m_floor_tile.SetActive(false);
                this.m_ground.SetActive(true);
            }
            else
            {
                this.m_floor_tile.SetActive(false);
                this.m_ground.SetActive(false);
            }
            this.m_cur_state = bs;
        }
    }
}