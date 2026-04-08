using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ROK
{
    public class SquarePositionProvider
    {
        private class RectPosition
        {
            public float m_area;

            public Vector4 m_rect;

            public Vector3 m_pos;

            public float m_fixX;
        }

        private Formation m_formation;

        private Formation.ENMU_SQUARE_TYPE m_square_type;

        private List<int> m_unusedPos = new List<int>();

        private float m_area_offset_start;

        private float m_area_offset_end;

        private List<SquarePositionProvider.RectPosition> m_rectPos = new List<SquarePositionProvider.RectPosition>();

        private int m_unusedPosNum;

        private int m_rowNum;

        private GameObject m_test_plane;

        private float m_area;

        private float m_width;

        private float m_height;

        private int m_max_num;

        private int m_org_max_num;

        private int m_cur_lod_level = -1;

        private bool m_isAtk;

        private float m_hero_height;

        public void SetSquareType(Formation.ENMU_SQUARE_TYPE square_type)
        {
            this.m_square_type = square_type;
        }

        public Formation.ENMU_SQUARE_TYPE GetSquareType()
        {
            return this.m_square_type;
        }

        public void SetFormationObject(Formation f)
        {
            this.m_formation = f;
        }

        public Vector3 GetUnitPosition(int cur_row, int cur_sub_row, int row_count, int cur_col, int col_count, int row_category)
        {
            if (this.m_square_type != Formation.ENMU_SQUARE_TYPE.COMMON && this.m_square_type != Formation.ENMU_SQUARE_TYPE.RALLY)
            {
                return Vector3.zero;
            }
            if (row_category == 0)
            {
                return Vector3.zero;
            }
            float num = (float)(cur_sub_row - Mathf.FloorToInt((float)(row_count / 2)));
            if (row_count % 2 == 0)
            {
                num += 0.5f;
            }
            float num2 = (float)(cur_col - Mathf.FloorToInt((float)(col_count / 2)));
            if (col_count % 2 == 0)
            {
                num2 += 0.5f;
            }
            int num3 = UnityGameDatas.GetInstance().ReadUnitDisplayNumberInRow(row_category, Formation.ENMU_SQUARE_TYPE.COMMON);
            float num4 = UnityGameDatas.GetInstance().ReadUnitRowWidthByCategory(row_category, Formation.ENMU_SQUARE_TYPE.COMMON) / (float)num3;
            float num5 = UnityGameDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(row_category, Formation.ENMU_SQUARE_TYPE.COMMON) + UnityGameDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(row_category, Formation.ENMU_SQUARE_TYPE.COMMON);
            float z = (float)cur_row * num5 + num5 / (float)row_count * num;
            return new Vector3(num4 * num2, 0f, z);
        }

        public void Reset()
        {
            this.m_unusedPos.Clear();
            this.m_unusedPosNum = 0;
            this.m_rectPos.Clear();
            this.m_max_num = 0;
            this.m_org_max_num = 0;
            this.m_area_offset_start = 0f;
            this.m_height = 0f;
            this.m_width = 0f;
            this.m_area = 0f;
            this.m_rowNum = 0;
        }

        public void SetAreaOffset(float start, float end)
        {
            this.m_area_offset_start = start;
            this.m_area_offset_end = end;
        }

        public void AddArea(float area)
        {
            this.m_area += area;
            float num = 0.8f;
            this.m_height = Mathf.Sqrt(this.m_area / num);
            this.m_width = this.m_height * num;
        }

        public void AddRowNum(int rowNum)
        {
            this.m_rowNum += rowNum;
        }

        public float GetWidth()
        {
            return this.m_width;
        }

        public float GetHeight()
        {
            return this.m_height;
        }

        public void AddNum(int num)
        {
            this.m_org_max_num += num;
            this.CalcLodMaxNum();
        }

        private void CalcLodMaxNum()
        {
            if (this.m_cur_lod_level == 2)
            {
                this.m_max_num = this.m_org_max_num;
            }
            else
            {
                this.m_max_num = this.m_org_max_num * (3 - this.m_cur_lod_level) + this.m_rowNum;
            }
        }

        public void SetLodLevel(int lod_level)
        {
            if (this.m_cur_lod_level != lod_level)
            {
                this.m_cur_lod_level = lod_level;
                this.CalcLodMaxNum();
            }
        }

        public void LodChanged(int lod_level)
        {
            this.m_cur_lod_level = lod_level;
            this.CalcLodMaxNum();
            this.UpdateSquareMap();
        }

        public void UpdateSquareMap()
        {
            if (this.m_max_num <= 0)
            {
                return;
            }
            float num = Mathf.Sqrt(this.m_area / (float)this.m_max_num * 0.5f);
            float singleHeight = num * 2f;
            this.m_unusedPos.Clear();
            this.m_rectPos.Clear();
            this.BlockRect(this.m_width, this.m_height, num, singleHeight);
            for (int i = 0; i < this.m_rectPos.Count; i++)
            {
                this.m_unusedPos.Add(i);
            }
        }

        public void SetAtkState(bool isAtk)
        {
            if (isAtk)
            {
                float num = UnityGameDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(0, this.m_square_type);
                float num2 = UnityGameDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(0, this.m_square_type);
                this.m_hero_height = num + num2;
                if (!this.m_isAtk)
                {
                    this.m_height += this.m_hero_height;
                }
            }
            else
            {
                if (this.m_isAtk)
                {
                    this.m_height -= this.m_hero_height;
                }
                this.m_hero_height = 0f;
            }
            this.m_isAtk = isAtk;
        }

        private void BlockRect(float width, float height, float singleWidth, float singleHeight)
        {
            List<SquarePositionProvider.RectPosition> list = new List<SquarePositionProvider.RectPosition>();
            list.Add(new SquarePositionProvider.RectPosition
            {
                m_rect = new Vector4(0f, 0f, width, height),
                m_area = width * height
            });
            List<SquarePositionProvider.RectPosition> list2 = new List<SquarePositionProvider.RectPosition>();
            int max_num = this.m_max_num;
            int num = 1;
            float num2 = singleWidth * 1f;
            float num3 = singleHeight * 1f;
            while (list.Count > 0 && num < max_num)
            {
                Vector4 rect = list[0].m_rect;
                list.RemoveAt(0);
                int num4 = 0;
                if (rect.z - rect.x <= num2)
                {
                    num4 = -1;
                }
                else if (rect.w - rect.y <= num3)
                {
                    num4 = 0;
                }
                else if (rect.z - rect.x < rect.w - rect.y)
                {
                    num4 = -1;
                }
                int num5 = UnityEngine.Random.Range(1, 3);
                if (max_num - num == 1)
                {
                    num5 = 1;
                }
                num5 = num5 * 2 + num4;
                Vector4 zero = Vector4.zero;
                Vector4 zero2 = Vector4.zero;
                Vector4 zero3 = Vector4.zero;
                SquarePositionProvider.RectPosition rectPosition = new SquarePositionProvider.RectPosition();
                SquarePositionProvider.RectPosition rectPosition2 = new SquarePositionProvider.RectPosition();
                SquarePositionProvider.RectPosition rectPosition3 = new SquarePositionProvider.RectPosition();
                switch (num5)
                {
                    case 1:
                        {
                            float num6 = 0.5f;
                            zero.Set(rect.x, rect.y, rect.z, rect.y + (rect.w - rect.y) * num6);
                            zero2.Set(rect.x, zero.w, rect.z, rect.w);
                            rectPosition.m_rect = zero;
                            rectPosition.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition2.m_rect = zero2;
                            rectPosition2.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            num++;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition);
                            }
                            else
                            {
                                list.Add(rectPosition);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            break;
                        }
                    case 2:
                        {
                            float num7 = 0.5f;
                            zero.Set(rect.x, rect.y, rect.x + (rect.z - rect.x) * num7, rect.w);
                            zero2.Set(zero.z, rect.y, rect.z, rect.w);
                            rectPosition.m_rect = zero;
                            rectPosition.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition2.m_rect = zero2;
                            rectPosition2.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            num++;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition);
                            }
                            else
                            {
                                list.Add(rectPosition);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            break;
                        }
                    case 3:
                        {
                            float num8 = 0.333f;
                            float num9 = 0.666f;
                            zero.Set(rect.x, rect.y, rect.z, rect.y + (rect.w - rect.y) * num8);
                            zero2.Set(rect.x, zero.w, rect.z, rect.y + (rect.w - rect.y) * num9);
                            zero3.Set(rect.x, zero2.w, rect.z, rect.w);
                            rectPosition.m_rect = zero;
                            rectPosition.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition2.m_rect = zero2;
                            rectPosition2.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            rectPosition3.m_rect = zero3;
                            rectPosition3.m_area = (zero3.z - zero3.x) * (zero3.w - zero3.y);
                            num += 2;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition);
                            }
                            else
                            {
                                list.Add(rectPosition);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            if (zero3.z - zero3.x <= num2 && zero3.w - zero3.y <= num3)
                            {
                                list2.Add(rectPosition3);
                            }
                            else
                            {
                                list.Add(rectPosition3);
                            }
                            break;
                        }
                    default:
                        {
                            float num10 = 0.333f;
                            float num11 = 0.666f;
                            zero.Set(rect.x, rect.y, rect.x + (rect.z - rect.x) * num10, rect.w);
                            zero2.Set(zero.z, rect.y, rect.x + (rect.z - rect.x) * num11, rect.w);
                            zero3.Set(zero2.z, rect.y, rect.z, rect.w);
                            rectPosition.m_rect = zero;
                            rectPosition.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition2.m_rect = zero2;
                            rectPosition2.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            rectPosition3.m_rect = zero3;
                            rectPosition3.m_area = (zero3.z - zero3.x) * (zero3.w - zero3.y);
                            num += 2;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition);
                            }
                            else
                            {
                                list.Add(rectPosition);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            if (zero3.z - zero3.x <= num2 && zero3.w - zero3.y <= num3)
                            {
                                list2.Add(rectPosition3);
                            }
                            else
                            {
                                list.Add(rectPosition3);
                            }
                            break;
                        }
                }
                list = (from rp in list
                        orderby rp.m_area descending
                        select rp).ToList<SquarePositionProvider.RectPosition>();
            }
            foreach (SquarePositionProvider.RectPosition current in list)
            {
                list2.Add(current);
            }
            List<SquarePositionProvider.RectPosition> list3 = new List<SquarePositionProvider.RectPosition>();
            foreach (SquarePositionProvider.RectPosition current2 in list2)
            {
                if (current2.m_rect.y == 0f)
                {
                    list3.Add(current2);
                }
            }
            list3 = (from rp in list3
                     orderby rp.m_rect.x
                     select rp).ToList<SquarePositionProvider.RectPosition>();
            float num12 = width * 0.5f;
            for (int i = 0; i < list3.Count; i++)
            {
                SquarePositionProvider.RectPosition rectPosition4 = list3[i];
                rectPosition4.m_fixX = 0.5f * ((i >= Mathf.RoundToInt((float)list3.Count * 0.5f)) ? 1f : -1f);
            }
            float num13 = width * 0.5f;
            float num14 = height * 0.5f;
            for (int j = 0; j < list2.Count; j++)
            {
                SquarePositionProvider.RectPosition rectPosition5 = list2[j];
                Vector4 rect2 = rectPosition5.m_rect;
                Vector3 zero4 = Vector3.zero;
                zero4.x = (rect2.z + rect2.x) * 0.5f - num13;
                zero4.y = 0f;
                zero4.z = num14 - (rect2.w + rect2.y) * 0.5f;
                rectPosition5.m_pos = zero4;
                this.m_rectPos.Add(rectPosition5);
            }
            this.m_rectPos = (from rp in this.m_rectPos
                              orderby rp.m_area descending
                              select rp).ToList<SquarePositionProvider.RectPosition>();
        }

        private Vector3 FetchPos(Vector3 considerPos)
        {
            Vector3 zero = Vector3.zero;
            if (this.m_rectPos == null || this.m_rectPos.Count == 0)
            {
                return zero;
            }
            int index = 0;
            float num = 9999f;
            int index2 = 0;
            for (int i = 0; i < this.m_unusedPos.Count; i++)
            {
                int num2 = this.m_unusedPos[i];
                Vector3 pos = this.m_rectPos[num2].m_pos;
                float magnitude = (pos - considerPos).magnitude;
                if (magnitude < num)
                {
                    num = magnitude;
                    index = num2;
                    index2 = i;
                }
            }
            zero.Set(this.m_rectPos[index].m_pos.x, this.m_rectPos[index].m_pos.y, this.m_rectPos[index].m_pos.z);
            if (this.m_unusedPos.Count > 0)
            {
                this.m_unusedPos.RemoveAt(index2);
            }
            Vector4 rect = this.m_rectPos[index].m_rect;
            float num3 = (rect.z - rect.x) * 0.25f;
            float num4 = (rect.w - rect.y) * 0.1f;
            if (this.m_isAtk && this.m_rectPos[index].m_fixX != 0f)
            {
                zero.x += this.m_rectPos[index].m_fixX;
            }
            else
            {
                zero.x += UnityEngine.Random.Range(-num3, num3);
            }
            zero.z += UnityEngine.Random.Range(-num4, num4);
            if (this.m_isAtk)
            {
                zero.z += this.m_hero_height;
            }
            return zero;
        }

        private Vector3 FetchPos(int category)
        {
            Vector3 zero = Vector3.zero;
            if (this.m_rectPos == null || this.m_rectPos.Count == 0)
            {
                return zero;
            }
            int index = 0;
            if (this.m_unusedPos.Count > 0)
            {
                int index2 = UnityEngine.Random.Range(0, this.m_unusedPos.Count);
                if (category == 2)
                {
                    index2 = 0;
                }
                index = this.m_unusedPos[index2];
                this.m_unusedPos.RemoveAt(index2);
            }
            zero.Set(this.m_rectPos[index].m_pos.x, this.m_rectPos[index].m_pos.y, this.m_rectPos[index].m_pos.z);
            Vector4 rect = this.m_rectPos[index].m_rect;
            float num = (rect.z - rect.x) * 0.15f;
            float num2 = (rect.w - rect.y) * 0.1f;
            if (this.m_isAtk && this.m_rectPos[index].m_fixX != 0f)
            {
                zero.x += this.m_rectPos[index].m_fixX;
            }
            else
            {
                zero.x += UnityEngine.Random.Range(-num, num);
            }
            if (category == 2)
            {
                zero.z += UnityEngine.Random.Range(-num2, 0f);
            }
            else
            {
                zero.z += UnityEngine.Random.Range(0f, num2);
            }
            if (this.m_isAtk)
            {
                zero.z += this.m_hero_height;
            }
            return zero;
        }

        public Vector3 GetUnitPosition(Vector3 considerPos)
        {
            Vector3 zero = Vector3.zero;
            if (this.m_square_type != Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                return zero;
            }
            return this.FetchPos(considerPos);
        }

        public Vector3 GetUnitPosition(int category)
        {
            Vector3 zero = Vector3.zero;
            if (this.m_square_type != Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                return zero;
            }
            return this.FetchPos(category);
        }
    }
}