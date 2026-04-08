using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class TerritoryMapItem
    {
        private BitArray m_map;

        private BitArray m_discard_map;

        private int m_width;

        private int m_height;

        private int m_x_min;

        private int m_y_min;

        private Color m_line_color = Color.blue;

        private List<Pos2> m_outter_territory_line = new List<Pos2>();

        private List<List<Pos2>> m_inner_territory_line_list = new List<List<Pos2>>();

        public Color lineColor
        {
            get
            {
                return this.m_line_color;
            }
        }

        public TerritoryMapItem(int width, int height, int x_min, int y_min)
        {
            this.m_width = width + TerritoryMgr.checkDis;
            this.m_height = height + TerritoryMgr.checkDis;
            this.m_x_min = x_min;
            this.m_y_min = y_min;
            this.m_map = new BitArray(this.m_width / TerritoryMgr.checkDis * (this.m_height / TerritoryMgr.checkDis));
            this.m_discard_map = new BitArray(this.m_width / TerritoryMgr.checkDis * (this.m_height / TerritoryMgr.checkDis));
        }

        private bool IsTerritoryVertex(Pos2 pos)
        {
            return pos.x >= 0 && pos.x < this.m_width && pos.y >= 0 && pos.y < this.m_height && this.m_map[pos.x / TerritoryMgr.checkDis + pos.y / TerritoryMgr.checkDis * (this.m_width / TerritoryMgr.checkDis)];
        }

        private bool IsTerritoryVertex(int x, int y)
        {
            return x >= 0 && x < this.m_width && y >= 0 && y < this.m_height && this.m_map[x / TerritoryMgr.checkDis + y / TerritoryMgr.checkDis * (this.m_width / TerritoryMgr.checkDis)];
        }

        private void SetAsTerritoryVertex(int x, int y)
        {
            this.m_map[x / TerritoryMgr.checkDis + y / TerritoryMgr.checkDis * (this.m_width / TerritoryMgr.checkDis)] = true;
        }

        private void SetAsDiscardVertex(int x, int y)
        {
            this.m_discard_map[x / TerritoryMgr.checkDis + y / TerritoryMgr.checkDis * (this.m_width / TerritoryMgr.checkDis)] = true;
        }

        private void SetAsDiscardVertex(Pos2 pos)
        {
            this.SetAsDiscardVertex(pos.x, pos.y);
        }

        private bool IsDiscardVertex(Pos2 pos)
        {
            return this.IsDiscardVertex(pos.x, pos.y);
        }

        private bool IsDiscardVertex(int x, int y)
        {
            return x >= 0 && x < this.m_width && y >= 0 && y < this.m_height && this.m_discard_map[x / TerritoryMgr.checkDis + y / TerritoryMgr.checkDis * (this.m_width / TerritoryMgr.checkDis)];
        }

        public void AddTerritoryItem(TerritoryItem item)
        {
            int num = item.endPosX - item.startPosX;
            int num2 = item.endPosY - item.startPosY;
            int num3 = item.startPosX - this.m_x_min;
            int num4 = item.startPosY - this.m_y_min;
            this.m_line_color = item.color;
            for (int i = 0; i <= num2; i += TerritoryMgr.checkDis)
            {
                for (int j = 0; j <= num; j += TerritoryMgr.checkDis)
                {
                    this.SetAsTerritoryVertex(num3 + j, num4 + i);
                }
            }
        }

        public void CalculateTerritoryLine()
        {
            this.CalculateOutterTerritoryLine();
            this.CalculateInnerTerritoryLine();
        }

        private void CalculateOutterTerritoryLine()
        {
            Pos2 pos = default(Pos2);
            bool flag = false;
            for (int i = 0; i < this.m_height; i += TerritoryMgr.checkDis)
            {
                for (int j = 0; j < this.m_width; j += TerritoryMgr.checkDis)
                {
                    if (this.IsTerritoryVertex(new Pos2(j, i)))
                    {
                        pos.x = j;
                        pos.y = i;
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    break;
                }
            }
            this.m_outter_territory_line.Add(this.GetWorldPos(pos));
            this.SetAsDiscardVertex(pos);
            this.GetNextPos(pos, 270f, this.m_outter_territory_line, pos, false);
        }

        private void CalculateInnerTerritoryLine()
        {
            for (int i = 0; i < this.m_height; i += TerritoryMgr.checkDis)
            {
                for (int j = 0; j < this.m_width; j += TerritoryMgr.checkDis)
                {
                    Pos2 pos = new Pos2(j, i);
                    if (!this.IsTerritoryVertex(pos) && this.IsTerritoryVertex(j, i - TerritoryMgr.checkDis) && !this.IsDiscardVertex(j, i - TerritoryMgr.checkDis))
                    {
                        pos = new Pos2(j, i - TerritoryMgr.checkDis);
                        List<Pos2> list = new List<Pos2>();
                        this.m_inner_territory_line_list.Add(list);
                        list.Add(this.GetWorldPos(pos));
                        this.SetAsDiscardVertex(pos);
                        this.GetNextPos(pos, 0f, list, pos, true);
                    }
                }
            }
        }

        public List<Pos2> GetOutterTerritoryLine()
        {
            return this.m_outter_territory_line;
        }

        public List<List<Pos2>> GetInnerTerritoryLines()
        {
            return this.m_inner_territory_line_list;
        }

        private void GetNextPos(Pos2 current_pos, float search_direction, List<Pos2> territory_line_list, Pos2 start_pos, bool isInner = false)
        {
            int i = 0;
            while (i < 4)
            {
                Vector2 vector = Common.DegreeToVector2(search_direction + 90f * (float)i) * (float)TerritoryMgr.checkDis;
                Pos2 pos = new Pos2(current_pos.x + (int)vector.x, current_pos.y + (int)vector.y);
                if (this.IsTerritoryVertex(pos))
                {
                    this.SetAsDiscardVertex(pos);
                    if (pos == start_pos)
                    {
                        if (isInner)
                        {
                            territory_line_list[0] = new Pos2(territory_line_list[0].x - TerritoryMgr.checkDis, territory_line_list[0].y);
                        }
                        Pos2 p = territory_line_list[territory_line_list.Count - 1];
                        Pos2 item = (p + territory_line_list[0]) / 2;
                        territory_line_list.Insert(0, item);
                        territory_line_list.Add(item);
                        return;
                    }
                    if (i != 1)
                    {
                        search_direction += 90f * (float)(i - 1);
                        territory_line_list.Add(this.GetWorldPos(current_pos));
                    }
                    this.GetNextPos(pos, search_direction, territory_line_list, start_pos, isInner);
                    return;
                }
                else
                {
                    i++;
                }
            }
        }

        private Pos2 GetWorldPos(Pos2 local_pos)
        {
            return new Pos2(local_pos.x + this.m_x_min, local_pos.y + this.m_y_min);
        }
    }
}