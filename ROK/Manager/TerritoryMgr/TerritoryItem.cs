using System;
using UnityEngine;

namespace ROK
{
    public class TerritoryItem
    {
        private int m_alliance_id;

        private Color m_color = Color.black;

        private int m_start_pos_x;

        private int m_start_pos_y;

        private int m_end_pos_x;

        private int m_end_pos_y;

        public int allianceId
        {
            get
            {
                return this.m_alliance_id;
            }
            set
            {
                this.m_alliance_id = value;
            }
        }

        public Color color
        {
            get
            {
                return this.m_color;
            }
            set
            {
                this.m_color = value;
            }
        }

        public int startPosX
        {
            get
            {
                return this.m_start_pos_x;
            }
            set
            {
                this.m_start_pos_x = value;
            }
        }

        public int startPosY
        {
            get
            {
                return this.m_start_pos_y;
            }
            set
            {
                this.m_start_pos_y = value;
            }
        }

        public int endPosX
        {
            get
            {
                return this.m_end_pos_x;
            }
            set
            {
                this.m_end_pos_x = value;
            }
        }

        public int endPosY
        {
            get
            {
                return this.m_end_pos_y;
            }
            set
            {
                this.m_end_pos_y = value;
            }
        }

        public TerritoryItem(int _alliance_id, Color _color, int _start_x, int _start_y, int _end_x, int _end_y)
        {
            this.allianceId = _alliance_id;
            this.color = _color;
            this.startPosX = _start_x;
            this.startPosY = _start_y;
            this.endPosX = _end_x;
            this.endPosY = _end_y;
        }
    }
}