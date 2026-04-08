using System;

namespace Client
{
    internal struct CellData
    {
        public int m_type;

        public int m_max_number_in_row;

        public float m_foward_spacing;

        public float m_backward_spacing;

        public float m_row_width;

        public CellNumByTotalData[] m_number_by_sum_array;
    }
}