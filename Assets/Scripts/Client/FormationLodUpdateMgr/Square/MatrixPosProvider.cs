using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class MatrixPosProvider
    {
        private class RectPosition
        {
            public float m_area;

            public Vector4 m_rect;

            public Vector3 m_pos;

            public float m_fixX;
        }

        private Troops m_formation;

        private Troops.ENMU_MATRIX_TYPE m_square_type;

        private List<int> m_unusedPos = new List<int>();

        private float m_area_offset_start;

        private float m_area_offset_end;

        private List<RectPosition> m_rectPos = new List<RectPosition>();

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

        public void SetSquareType(Troops.ENMU_MATRIX_TYPE square_type)
        {
            m_square_type = square_type;
        }

        public Troops.ENMU_MATRIX_TYPE GetSquareType()
        {
            return m_square_type;
        }

        public void SetFormationObject(Troops f)
        {
            m_formation = f;
        }

        public Vector3 GetUnitPosition(int cur_row, int cur_sub_row, int row_count, int cur_col, int col_count, int row_category)
        {
            if (m_square_type != 0 && m_square_type != Troops.ENMU_MATRIX_TYPE.RALLY)
            {
                return Vector3.zero;
            }
            if (row_category == 0)
            {
                return Vector3.zero;
            }
            float num = cur_sub_row - Mathf.FloorToInt(row_count / 2);
            if (row_count % 2 == 0)
            {
                num += 0.5f;
            }
            float num2 = cur_col - Mathf.FloorToInt(col_count / 2);
            if (col_count % 2 == 0)
            {
                num2 += 0.5f;
            }
            int num3 = CellDatas.GetInstance().ReadUnitDisplayNumberInRow(row_category);
            float num4 = CellDatas.GetInstance().ReadUnitRowWidthByCategory(row_category) / (float)num3;
            float num5 = CellDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(row_category) + CellDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(row_category);
            float z = (float)cur_row * num5 + num5 / (float)row_count * num;
            return new Vector3(num4 * num2, 0f, z);
        }

        public void Reset()
        {
            m_unusedPos.Clear();
            m_unusedPosNum = 0;
            m_rectPos.Clear();
            m_max_num = 0;
            m_org_max_num = 0;
            m_area_offset_start = 0f;
            m_height = 0f;
            m_width = 0f;
            m_area = 0f;
            m_rowNum = 0;
        }

        public void SetAreaOffset(float start, float end)
        {
            m_area_offset_start = start;
            m_area_offset_end = end;
        }

        public void AddArea(float area)
        {
            m_area += area;
            float num = 0.8f;
            m_height = Mathf.Sqrt(m_area / num);
            m_width = m_height * num;
        }

        public void AddRowNum(int rowNum)
        {
            m_rowNum += rowNum;
        }

        public float GetWidth()
        {
            return m_width;
        }

        public float GetHeight()
        {
            return m_height;
        }

        public void AddNum(int num)
        {
            m_org_max_num += num;
            CalcLodMaxNum();
        }

        private void CalcLodMaxNum()
        {
            if (m_cur_lod_level == 2)
            {
                m_max_num = m_org_max_num;
            }
            else
            {
                m_max_num = m_org_max_num * (3 - m_cur_lod_level) + m_rowNum;
            }
        }

        public void SetLodLevel(int lod_level)
        {
            if (m_cur_lod_level != lod_level)
            {
                m_cur_lod_level = lod_level;
                CalcLodMaxNum();
            }
        }

        public void LodChanged(int lod_level)
        {
            m_cur_lod_level = lod_level;
            CalcLodMaxNum();
            UpdateSquareMap();
        }

        public void UpdateSquareMap()
        {
            if (m_max_num > 0)
            {
                float num = Mathf.Sqrt(m_area / (float)m_max_num * 0.5f);
                float singleHeight = num * 2f;
                m_unusedPos.Clear();
                m_rectPos.Clear();
                BlockRect(m_width, m_height, num, singleHeight);
                for (int i = 0; i < m_rectPos.Count; i++)
                {
                    m_unusedPos.Add(i);
                }
            }
        }

        public void SetAtkState(bool isAtk)
        {
            if (isAtk)
            {
                float num = CellDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(0, m_square_type);
                float num2 = CellDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(0, m_square_type);
                m_hero_height = num + num2;
                if (!m_isAtk)
                {
                    m_height += m_hero_height;
                }
            }
            else
            {
                if (m_isAtk)
                {
                    m_height -= m_hero_height;
                }
                m_hero_height = 0f;
            }
            m_isAtk = isAtk;
        }

        private void BlockRect(float width, float height, float singleWidth, float singleHeight)
        {
            List<RectPosition> list = new List<RectPosition>();
            RectPosition rectPosition = new RectPosition();
            rectPosition.m_rect = new Vector4(0f, 0f, width, height);
            rectPosition.m_area = width * height;
            list.Add(rectPosition);
            List<RectPosition> list2 = new List<RectPosition>();
            int max_num = m_max_num;
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
                int num5 = Random.Range(1, 3);
                if (max_num - num == 1)
                {
                    num5 = 1;
                }
                num5 = num5 * 2 + num4;
                Vector4 zero = Vector4.zero;
                Vector4 zero2 = Vector4.zero;
                Vector4 zero3 = Vector4.zero;
                RectPosition rectPosition2 = new RectPosition();
                RectPosition rectPosition3 = new RectPosition();
                RectPosition rectPosition4 = new RectPosition();
                switch (num5)
                {
                    case 1:
                        {
                            float num11 = 0.5f;
                            zero.Set(rect.x, rect.y, rect.z, rect.y + (rect.w - rect.y) * num11);
                            zero2.Set(rect.x, zero.w, rect.z, rect.w);
                            rectPosition2.m_rect = zero;
                            rectPosition2.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition3.m_rect = zero2;
                            rectPosition3.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            num++;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition3);
                            }
                            else
                            {
                                list.Add(rectPosition3);
                            }
                            break;
                        }
                    case 2:
                        {
                            float num10 = 0.5f;
                            zero.Set(rect.x, rect.y, rect.x + (rect.z - rect.x) * num10, rect.w);
                            zero2.Set(zero.z, rect.y, rect.z, rect.w);
                            rectPosition2.m_rect = zero;
                            rectPosition2.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition3.m_rect = zero2;
                            rectPosition3.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            num++;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition3);
                            }
                            else
                            {
                                list.Add(rectPosition3);
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
                            rectPosition2.m_rect = zero;
                            rectPosition2.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition3.m_rect = zero2;
                            rectPosition3.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            rectPosition4.m_rect = zero3;
                            rectPosition4.m_area = (zero3.z - zero3.x) * (zero3.w - zero3.y);
                            num += 2;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition3);
                            }
                            else
                            {
                                list.Add(rectPosition3);
                            }
                            if (zero3.z - zero3.x <= num2 && zero3.w - zero3.y <= num3)
                            {
                                list2.Add(rectPosition4);
                            }
                            else
                            {
                                list.Add(rectPosition4);
                            }
                            break;
                        }
                    default:
                        {
                            float num6 = 0.333f;
                            float num7 = 0.666f;
                            zero.Set(rect.x, rect.y, rect.x + (rect.z - rect.x) * num6, rect.w);
                            zero2.Set(zero.z, rect.y, rect.x + (rect.z - rect.x) * num7, rect.w);
                            zero3.Set(zero2.z, rect.y, rect.z, rect.w);
                            rectPosition2.m_rect = zero;
                            rectPosition2.m_area = (zero.z - zero.x) * (zero.w - zero.y);
                            rectPosition3.m_rect = zero2;
                            rectPosition3.m_area = (zero2.z - zero2.x) * (zero2.w - zero2.y);
                            rectPosition4.m_rect = zero3;
                            rectPosition4.m_area = (zero3.z - zero3.x) * (zero3.w - zero3.y);
                            num += 2;
                            if (zero.z - zero.x <= num2 && zero.w - zero.y <= num3)
                            {
                                list2.Add(rectPosition2);
                            }
                            else
                            {
                                list.Add(rectPosition2);
                            }
                            if (zero2.z - zero2.x <= num2 && zero2.w - zero2.y <= num3)
                            {
                                list2.Add(rectPosition3);
                            }
                            else
                            {
                                list.Add(rectPosition3);
                            }
                            if (zero3.z - zero3.x <= num2 && zero3.w - zero3.y <= num3)
                            {
                                list2.Add(rectPosition4);
                            }
                            else
                            {
                                list.Add(rectPosition4);
                            }
                            break;
                        }
                }
                list = (from rp in list
                        orderby rp.m_area descending
                        select rp).ToList();
            }
            foreach (RectPosition item in list)
            {
                list2.Add(item);
            }
            List<RectPosition> list3 = new List<RectPosition>();
            foreach (RectPosition item2 in list2)
            {
                if (item2.m_rect.y == 0f)
                {
                    list3.Add(item2);
                }
            }
            list3 = (from rp in list3
                     orderby rp.m_rect.x
                     select rp).ToList();
            for (int i = 0; i < list3.Count; i++)
            {
                RectPosition rectPosition5 = list3[i];
                rectPosition5.m_fixX = 0.5f * ((i >= Mathf.RoundToInt((float)list3.Count * 0.5f)) ? 1f : (-1f));
            }
            float num12 = width * 0.5f;
            float num13 = height * 0.5f;
            for (int j = 0; j < list2.Count; j++)
            {
                RectPosition rectPosition6 = list2[j];
                Vector4 rect2 = rectPosition6.m_rect;
                Vector3 zero4 = Vector3.zero;
                zero4.x = (rect2.z + rect2.x) * 0.5f - num12;
                zero4.y = 0f;
                zero4.z = num13 - (rect2.w + rect2.y) * 0.5f;
                rectPosition6.m_pos = zero4;
                m_rectPos.Add(rectPosition6);
            }
            m_rectPos = (from rp in m_rectPos
                         orderby rp.m_area descending
                         select rp).ToList();
        }

        private Vector3 FetchPos(Vector3 considerPos)
        {
            Vector3 zero = Vector3.zero;
            if (m_rectPos == null || m_rectPos.Count == 0)
            {
                return zero;
            }
            int index = 0;
            float num = 9999f;
            int index2 = 0;
            for (int i = 0; i < m_unusedPos.Count; i++)
            {
                int num2 = m_unusedPos[i];
                Vector3 pos = m_rectPos[num2].m_pos;
                float magnitude = (pos - considerPos).magnitude;
                if (magnitude < num)
                {
                    num = magnitude;
                    index = num2;
                    index2 = i;
                }
            }
            zero.Set(m_rectPos[index].m_pos.x, m_rectPos[index].m_pos.y, m_rectPos[index].m_pos.z);
            if (m_unusedPos.Count > 0)
            {
                m_unusedPos.RemoveAt(index2);
            }
            Vector4 rect = m_rectPos[index].m_rect;
            float num3 = (rect.z - rect.x) * 0.25f;
            float num4 = (rect.w - rect.y) * 0.1f;
            if (m_isAtk && m_rectPos[index].m_fixX != 0f)
            {
                zero.x += m_rectPos[index].m_fixX;
            }
            else
            {
                zero.x += Random.Range(0f - num3, num3);
            }
            zero.z += Random.Range(0f - num4, num4);
            if (m_isAtk)
            {
                zero.z += m_hero_height;
            }
            return zero;
        }

        private Vector3 FetchPos(int category)
        {
            Vector3 zero = Vector3.zero;
            if (m_rectPos == null || m_rectPos.Count == 0)
            {
                return zero;
            }
            int index = 0;
            if (m_unusedPos.Count > 0)
            {
                int index2 = Random.Range(0, m_unusedPos.Count);
                if (category == 2)
                {
                    index2 = 0;
                }
                index = m_unusedPos[index2];
                m_unusedPos.RemoveAt(index2);
            }
            zero.Set(m_rectPos[index].m_pos.x, m_rectPos[index].m_pos.y, m_rectPos[index].m_pos.z);
            Vector4 rect = m_rectPos[index].m_rect;
            float num = (rect.z - rect.x) * 0.15f;
            float num2 = (rect.w - rect.y) * 0.1f;
            if (m_isAtk && m_rectPos[index].m_fixX != 0f)
            {
                zero.x += m_rectPos[index].m_fixX;
            }
            else
            {
                zero.x += Random.Range(0f - num, num);
            }
            if (category == 2)
            {
                zero.z += Random.Range(0f - num2, 0f);
            }
            else
            {
                zero.z += Random.Range(0f, num2);
            }
            if (m_isAtk)
            {
                zero.z += m_hero_height;
            }
            return zero;
        }

        public Vector3 GetUnitPosition(Vector3 considerPos)
        {
            Vector3 zero = Vector3.zero;
            if (m_square_type != Troops.ENMU_MATRIX_TYPE.BARBARIAN)
            {
                return zero;
            }
            return FetchPos(considerPos);
        }

        public Vector3 GetUnitPosition(int category)
        {
            Vector3 zero = Vector3.zero;
            if (m_square_type != Troops.ENMU_MATRIX_TYPE.BARBARIAN)
            {
                return zero;
            }
            return FetchPos(category);
        }
    }
}