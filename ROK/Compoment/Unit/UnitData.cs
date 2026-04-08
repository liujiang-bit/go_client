using System;

namespace ROK
{
    internal struct UnitData
    {
        public int m_type;

        public int m_max_number_in_row;

        public float m_foward_spacing;

        public float m_backward_spacing;

        public float m_row_width;

        public UnitNumberBySumData[] m_number_by_sum_array;
    }
}