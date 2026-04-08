using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ROK
{
    public class NewTerritoryMapItem
    {
        private struct vertex
        {
            public int row;

            public int col;

            public Vector2 pos;

            public vertex(int row, int col)
            {
                this.row = row;
                this.col = col;
                this.pos = new Vector2((float)(col * NewTerritoryMapItem.TerritoryItemWidth + NewTerritoryMapItem.m_x_min), (float)(row * NewTerritoryMapItem.TerritoryItemWidth + NewTerritoryMapItem.m_y_min));
            }

            public static bool operator ==(NewTerritoryMapItem.vertex a, NewTerritoryMapItem.vertex b)
            {
                return a.row == b.row && a.col == b.col;
            }

            public static bool operator !=(NewTerritoryMapItem.vertex a, NewTerritoryMapItem.vertex b)
            {
                return !(a == b);
            }
        }

        private struct edge
        {
            public NewTerritoryMapItem.vertex start;

            public NewTerritoryMapItem.vertex end;

            public int dir;

            public edge(NewTerritoryMapItem.vertex start, NewTerritoryMapItem.vertex end)
            {
                this.start = start;
                this.end = end;
                this.dir = -1;
                if (end.col == start.col && end.row > start.row)
                {
                    this.dir = 0;
                }
                else if (end.col == start.col && end.row < start.row)
                {
                    this.dir = 2;
                }
                else if (end.row == start.row && end.col > start.col)
                {
                    this.dir = 1;
                }
                else if (end.row == start.row && end.col < start.col)
                {
                    this.dir = 3;
                }
            }
        }

        private static int[,] m_map;

        private static int m_width = 0;

        private static int m_height = 0;

        private static int m_x_min = 0;

        private static int m_y_min = 0;

        private static int TerritoryItemWidth = TerritoryMgr.checkDis * 2;

        private Color m_line_color = Color.blue;

        private Dictionary<Vector2, List<NewTerritoryMapItem.edge>> edge_List = new Dictionary<Vector2, List<NewTerritoryMapItem.edge>>();

        private List<List<Vector2>> line_list = new List<List<Vector2>>();

        public Color lineColor
        {
            get
            {
                return this.m_line_color;
            }
        }

        public NewTerritoryMapItem(int width, int height, int x_min, int y_min)
        {
            NewTerritoryMapItem.m_width = width / NewTerritoryMapItem.TerritoryItemWidth;
            NewTerritoryMapItem.m_height = height / NewTerritoryMapItem.TerritoryItemWidth;
            NewTerritoryMapItem.m_x_min = x_min;
            NewTerritoryMapItem.m_y_min = y_min;
            NewTerritoryMapItem.m_map = new int[NewTerritoryMapItem.m_height, NewTerritoryMapItem.m_width];
        }

        public void AddTerritoryItem(TerritoryItem item)
        {
            int num = (item.startPosX - NewTerritoryMapItem.m_x_min) / NewTerritoryMapItem.TerritoryItemWidth;
            int num2 = (item.startPosY - NewTerritoryMapItem.m_y_min) / NewTerritoryMapItem.TerritoryItemWidth;
            this.m_line_color = item.color;
            NewTerritoryMapItem.m_map[num2, num] = 1;
        }

        public List<List<Vector2>> GetPosList()
        {
            this.GetEdgeList();
            this.GetLineList();
            return this.line_list;
        }

        public static bool isIn(int row, int col)
        {
            return row >= 0 && col >= 0 && row < NewTerritoryMapItem.m_height && col < NewTerritoryMapItem.m_width && NewTerritoryMapItem.m_map[row, col] == 1;
        }

        private void GetEdgeList()
        {
            for (int i = 0; i < NewTerritoryMapItem.m_height; i++)
            {
                for (int j = 0; j < NewTerritoryMapItem.m_width; j++)
                {
                    if (NewTerritoryMapItem.isIn(i, j))
                    {
                        if (!this.CheckInSide(i, j, i + 1, j))
                        {
                            NewTerritoryMapItem.vertex start = new NewTerritoryMapItem.vertex(i, j);
                            NewTerritoryMapItem.vertex end = new NewTerritoryMapItem.vertex(i + 1, j);
                            NewTerritoryMapItem.edge e = new NewTerritoryMapItem.edge(start, end);
                            this.CheckAddEdge(e);
                        }
                        if (!this.CheckInSide(i + 1, j, i + 1, j + 1))
                        {
                            NewTerritoryMapItem.vertex start2 = new NewTerritoryMapItem.vertex(i + 1, j);
                            NewTerritoryMapItem.vertex end2 = new NewTerritoryMapItem.vertex(i + 1, j + 1);
                            NewTerritoryMapItem.edge e2 = new NewTerritoryMapItem.edge(start2, end2);
                            this.CheckAddEdge(e2);
                        }
                        if (!this.CheckInSide(i + 1, j + 1, i, j + 1))
                        {
                            NewTerritoryMapItem.vertex start3 = new NewTerritoryMapItem.vertex(i + 1, j + 1);
                            NewTerritoryMapItem.vertex end3 = new NewTerritoryMapItem.vertex(i, j + 1);
                            NewTerritoryMapItem.edge e3 = new NewTerritoryMapItem.edge(start3, end3);
                            this.CheckAddEdge(e3);
                        }
                        if (!this.CheckInSide(i, j + 1, i, j))
                        {
                            NewTerritoryMapItem.vertex start4 = new NewTerritoryMapItem.vertex(i, j + 1);
                            NewTerritoryMapItem.vertex end4 = new NewTerritoryMapItem.vertex(i, j);
                            NewTerritoryMapItem.edge e4 = new NewTerritoryMapItem.edge(start4, end4);
                            this.CheckAddEdge(e4);
                        }
                    }
                }
            }
        }

        private void CheckAddEdge(NewTerritoryMapItem.edge e)
        {
            if (!this.edge_List.ContainsKey(e.start.pos))
            {
                this.edge_List[e.start.pos] = new List<NewTerritoryMapItem.edge>();
            }
            this.edge_List[e.start.pos].Add(e);
        }

        private bool CheckInSide(int row1, int col1, int row2, int col2)
        {
            bool result = false;
            int num = col2 - col1;
            int num2 = row2 - row1;
            if (num == 0)
            {
                if (num2 > 0)
                {
                    if (NewTerritoryMapItem.isIn(row1, col1) && NewTerritoryMapItem.isIn(row1, col1 - 1))
                    {
                        result = true;
                    }
                }
                else if (NewTerritoryMapItem.isIn(row1 - 1, col1) && NewTerritoryMapItem.isIn(row1 - 1, col1 - 1))
                {
                    result = true;
                }
            }
            else if (num2 == 0)
            {
                if (num > 0)
                {
                    if (NewTerritoryMapItem.isIn(row1, col1) && NewTerritoryMapItem.isIn(row1 - 1, col1))
                    {
                        result = true;
                    }
                }
                else if (NewTerritoryMapItem.isIn(row1, col1 - 1) && NewTerritoryMapItem.isIn(row1 - 1, col1 - 1))
                {
                    result = true;
                }
            }
            return result;
        }

        private void GetLineList()
        {
            while (this.edge_List.Count > 0)
            {
                List<Vector2> list = new List<Vector2>();
                Vector2 vector = this.edge_List.Keys.First<Vector2>();
                list.Add(vector);
                NewTerritoryMapItem.edge item = this.edge_List[vector][0];
                if (this.edge_List[vector].Count == 1)
                {
                    this.edge_List.Remove(vector);
                }
                else
                {
                    this.edge_List[vector].Remove(item);
                }
                Vector2 pos = item.end.pos;
                list.Add(pos);
                while (vector != pos)
                {
                    List<NewTerritoryMapItem.edge> list2 = this.edge_List[pos];
                    if (list2.Count == 1)
                    {
                        bool flag = false;
                        if (item.dir == list2[0].dir)
                        {
                            flag = true;
                        }
                        item = list2[0];
                        this.edge_List.Remove(pos);
                        pos = list2[0].end.pos;
                        if (!flag)
                        {
                            list.Add(pos);
                        }
                        else
                        {
                            list[list.Count - 1] = pos;
                        }
                    }
                    else
                    {
                        NewTerritoryMapItem.edge edge = list2[0];
                        NewTerritoryMapItem.edge edge2 = list2[1];
                        Vector2 vector2 = item.end.pos - item.start.pos;
                        Vector2 vector3 = edge.end.pos - edge.start.pos;
                        if (Vector3.Cross(new Vector3(vector2.x, 0f, vector2.y), new Vector3(vector3.x, 0f, vector3.y)).y < 0f)
                        {
                            bool flag2 = false;
                            if (item.dir == edge.dir)
                            {
                                flag2 = true;
                            }
                            item = edge;
                            list2.Remove(item);
                            pos = edge.end.pos;
                            if (!flag2)
                            {
                                list.Add(pos);
                            }
                            else
                            {
                                list[list.Count - 1] = pos;
                            }
                        }
                        else
                        {
                            bool flag3 = false;
                            if (item.dir == edge2.dir)
                            {
                                flag3 = true;
                            }
                            item = edge2;
                            list2.Remove(item);
                            pos = edge2.end.pos;
                            if (!flag3)
                            {
                                list.Add(pos);
                            }
                            else
                            {
                                list[list.Count - 1] = pos;
                            }
                        }
                    }
                }
                list.Reverse();
                list.RemoveAt(list.Count - 1);
                Vector2 a = list[list.Count - 1];
                Vector2 b = list[0];
                Vector2 item2 = (a + b) / 2f;
                list.Insert(0, item2);
                list.Add(item2);
                this.line_list.Add(list);
            }
        }

        public void LogMap()
        {
            for (int i = 0; i < NewTerritoryMapItem.m_map.GetLength(0); i++)
            {
                string text = string.Empty;
                for (int j = 0; j < NewTerritoryMapItem.m_map.GetLength(1); j++)
                {
                    text = text + NewTerritoryMapItem.m_map[i, j].ToString() + " ";
                }
                Debug.Log(text);
            }
        }
    }
}