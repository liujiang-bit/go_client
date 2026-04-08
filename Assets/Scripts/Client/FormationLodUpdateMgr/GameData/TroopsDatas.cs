using System.Collections.Generic;
using Skyunion;
using UnityEngine;

namespace Client
{
    public class TroopsCell
    {
        public int unitId;
        public int unitCount;
        public int unitMaxCount;

        public int unitType;
        public int unitLevel;
        public int unitserverId;
    }

    public class TroopsHero
    {
        public int heroId;
        public string label;
    }

    public class TroopsData
    {
        public int type;
        public int num;
        public float numRowWidth;
        public float spacing;

        public TroopsData(int t, int n)
        {
            this.type = t;
            this.num = n;
        }

        public TroopsData(int t, float n)
        {
            this.type = t;
            this.spacing = n;
        }

        public TroopsData()
        {
        }
    }


    public class DatabyNumberbySum
    {
        public int type;
        public int range;
        public int rangeMax;
        public int num;
    }


    public class Matrix_Prefab
    {
        public int id;
        public string name;
    }

    public class MatrixOffset
    {
        public int type;
        public float x_offset;
        public float z_offset;
    }

    public class TroopsDatas : TSingleton<TroopsDatas>
    {
        private Dictionary<int, List<TroopsData>> dicRowData;
        private Dictionary<int, List<TroopsData>> dicRowWidthData;
        private Dictionary<int, List<TroopsData>> dicForwardSpacingData;
        private Dictionary<int, List<TroopsData>> dicBackward_spacingData;

        private Dictionary<int, List<DatabyNumberbySum>> dicNumberbySumData;
        private Dictionary<int, Matrix_Prefab> dicSquare_prefab = new Dictionary<int, Matrix_Prefab>();
        private Dictionary<int, List<MatrixOffset>> dicSquareOffset;

        private TroopsDatas()
        {
            dicRowData = new Dictionary<int, List<TroopsData>>(5);
            dicRowWidthData = new Dictionary<int, List<TroopsData>>(5);
            dicForwardSpacingData = new Dictionary<int, List<TroopsData>>(5);
            dicBackward_spacingData = new Dictionary<int, List<TroopsData>>(5);
            dicSquareOffset = new Dictionary<int, List<MatrixOffset>>();
            dicNumberbySumData = new Dictionary<int, List<DatabyNumberbySum>>(5);
        }


        public void Clear()
        {
            dicRowData.Clear();
            dicRowWidthData.Clear();
            dicForwardSpacingData.Clear();
            dicBackward_spacingData.Clear();
            dicSquareOffset.Clear();
            dicNumberbySumData.Clear();
        }


        public void InitPrefabs(Matrix_Prefab prefabs)
        {
            if (!dicSquare_prefab.ContainsKey(prefabs.id))
            {
                dicSquare_prefab.Add(prefabs.id, prefabs);
            }
        }

        public void InitCfgRowData(Troops.ENMU_MATRIX_TYPE group, int type, int num)
        {
            if (!dicRowData.ContainsKey((int) group))
            {
                dicRowData[(int) group] = new List<TroopsData>();
            }

            TroopsData data = new TroopsData(type, num);
            dicRowData[(int) group].Add(data);
        }

        public void InitCfgRowWidthData(Troops.ENMU_MATRIX_TYPE group, int type, float num)
        {
            if (!dicRowWidthData.ContainsKey((int) group))
            {
                dicRowWidthData[(int) group] = new List<TroopsData>();
            }

            TroopsData data = new TroopsData();
            data.type = type;
            data.numRowWidth = num;
            dicRowWidthData[(int) group].Add(data);
        }


        public void InitCfgForwardSpacingData(Troops.ENMU_MATRIX_TYPE group, int type, float num)
        {
            if (!dicForwardSpacingData.ContainsKey((int) group))
            {
                dicForwardSpacingData[(int) group] = new List<TroopsData>();
            }

            TroopsData data = new TroopsData(type, num);
            dicForwardSpacingData[(int) group].Add(data);
        }

        public void InitCfgBackwardSpacingData(Troops.ENMU_MATRIX_TYPE group, int type, float num)
        {
            if (!dicBackward_spacingData.ContainsKey((int) group))
            {
                dicBackward_spacingData[(int) group] = new List<TroopsData>();
            }

            TroopsData data = new TroopsData(type, num);
            dicBackward_spacingData[(int) group].Add(data);
        }

        public void InitSquareOffset(Troops.ENMU_MATRIX_TYPE group, int type, float x, float y)
        {
            if (!dicSquareOffset.ContainsKey((int) group))
            {
                dicSquareOffset[(int) group] = new List<MatrixOffset>();
            }

            MatrixOffset data = new MatrixOffset();
            data.type = type;
            data.x_offset = x;
            data.z_offset = y;
            dicSquareOffset[(int) group].Add(data);
        }

        public void InitNumberBySumData(Troops.ENMU_MATRIX_TYPE group, int type, int range, int rangeMax, int num)
        {
            if (!dicNumberbySumData.ContainsKey((int) group))
            {
                dicNumberbySumData[(int) group] = new List<DatabyNumberbySum>();
            }

            DatabyNumberbySum data = new DatabyNumberbySum();
            data.type = type;
            data.range = range;
            data.num = num;
            data.rangeMax = rangeMax;
            dicNumberbySumData[(int) group].Add(data);
        }
        public bool IsHaveListData(int type)
        {
            return this.dicRowData.ContainsKey(type);
        }

        public List<TroopsData> GetListData(Troops.ENMU_MATRIX_TYPE type)
        {
            List<TroopsData> ls;
            if (this.dicRowData.TryGetValue((int) type, out ls))
            {
                return ls;
            }

            return null;
        }

        public bool IsHaveListRow_Width(int type)
        {
            return this.dicRowWidthData.ContainsKey(type);
        }

        public List<TroopsData> GetListRow_Width(Troops.ENMU_MATRIX_TYPE type)
        {
            List<TroopsData> ls;
            if (this.dicRowWidthData.TryGetValue((int) type, out ls))
            {
                return ls;
            }

            return null;
        }

        public bool IsHaveGetListForwardSpacingData(int type)
        {
            return this.dicForwardSpacingData.ContainsKey(type);
        }

        public List<TroopsData> GetListForwardSpacingData(Troops.ENMU_MATRIX_TYPE type)
        {
            List<TroopsData> ls;
            if (this.dicForwardSpacingData.TryGetValue((int) type, out ls))
            {
                return ls;
            }

            return null;
        }

        public bool IsHaveGetListBackward_spacingData(int type)
        {
            return this.dicBackward_spacingData.ContainsKey(type);
        }

        public List<TroopsData> GetListBackward_spacingData(Troops.ENMU_MATRIX_TYPE type)
        {
            List<TroopsData> ls;
            if (this.dicBackward_spacingData.TryGetValue((int) type, out ls))
            {
                return ls;
            }

            return null;
        }

        public bool IsHaveGetListDatabyNumberbyData(int type)
        {
            return this.dicNumberbySumData.ContainsKey(type);
        }

        public List<DatabyNumberbySum> GetListDatabyNumberbyData(Troops.ENMU_MATRIX_TYPE type)
        {
            List<DatabyNumberbySum> ls;
            if (this.dicNumberbySumData.TryGetValue((int) type, out ls))
            {
                return ls;
            }

            return null;
        }

        public Dictionary<int, Matrix_Prefab> GetUnitPrefabName()
        {
            return this.dicSquare_prefab;
        }

        public List<MatrixOffset> GetSquareOffsets(int type)
        {
            if (this.dicSquareOffset.ContainsKey(type))
            {
                return this.dicSquareOffset[type];
            }
           return new List<MatrixOffset>();
        }
    }
}