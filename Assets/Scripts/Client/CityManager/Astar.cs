using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Astar
    {
        private class Grid
        {
            public Grid p;
            public int x;
            public int y;
            public double f;
            public double g;
            public int h;
            public Grid(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public List<Vector2> result = new List<Vector2>();

        private List<Grid> m_objPool = new List<Grid>();

        private List<Grid> m_objPoolUnused = new List<Grid>();

        private string find;

        private int m_getObjCount;

        private Grid[] m_tmpResult = new Grid[8];

        private int m_row;

        private int m_col;

        private Dictionary<int, byte> list = new Dictionary<int, byte>();

        private Grid GetGrid(int x, int y)
        {
            Grid @object = null;
            int num = x + y * m_col;
            if (list.ContainsKey(num))
            {
                return @object;
            }
            int count = m_objPoolUnused.Count;
            if (count > 0)
            {
                @object = m_objPoolUnused[count - 1];
                @object.x = x;
                @object.y = y;
                @object.h = num;
                m_objPoolUnused.RemoveAt(count - 1);
            }
            else
            {
                @object = new Grid(x, y);
                @object.h = num;
                m_objPool.Add(@object);
            }
            m_getObjCount++;
            return @object;
        }

        private void ResetPool()
        {
            m_objPoolUnused.Clear();
            for (int i = 0; i < m_objPool.Count; i++)
            {
                Grid @object = m_objPool[i];
                Grid object2 = @object;
                int num2 = @object.h = 0;
                num2 = (object2.x = (@object.y = num2));
                double num7 = @object.f = (@object.g = 0.0);
                @object.p = null;
                m_objPoolUnused.Add(@object);
            }
        }

        private Grid[] diagonalSuccessors(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, Grid[] result, int i)
        {
            if (xN)
            {
                if (xE && grid[N][E] == 0)
                {
                    result[i++] = GetGrid(E, N);
                }
                if (xW && grid[N][W] == 0)
                {
                    result[i++] = GetGrid(W, N);
                }
            }
            if (xS)
            {
                if (xE && grid[S][E] == 0)
                {
                    result[i++] = GetGrid(E, S);
                }
                if (xW && grid[S][W] == 0)
                {
                    result[i++] = GetGrid(W, S);
                }
            }
            return result;
        }

        private Grid[] diagonalSuccessorsFree(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, Grid[] result, int i)
        {
            xN = (N > -1);
            xS = (S < rows);
            xE = (E < cols);
            xW = (W > -1);
            if (xE)
            {
                if (xN && grid[N][E] == 0)
                {
                    result[i++] = GetGrid(E, N);
                }
                if (xS && grid[S][E] == 0)
                {
                    result[i++] = GetGrid(E, S);
                }
            }
            if (xW)
            {
                if (xN && grid[N][W] == 0)
                {
                    result[i++] = GetGrid(W, N);
                }
                if (xS && grid[S][W] == 0)
                {
                    result[i++] = GetGrid(W, S);
                }
            }
            return result;
        }

        private Grid[] nothingToDo(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, Grid[] result, int i)
        {
            return result;
        }

        private Grid[] successors(int x, int y, int[][] grid, int rows, int cols)
        {
            int num = y - 1;
            int num2 = y + 1;
            int num3 = x + 1;
            int num4 = x - 1;
            bool flag = num > -1 && grid[num][x] == 0;
            bool flag2 = num2 < rows && grid[num2][x] == 0;
            bool flag3 = num3 < cols && grid[y][num3] == 0;
            bool flag4 = num4 > -1 && grid[y][num4] == 0;
            int num5 = 0;
            for (int i = 0; i < m_tmpResult.Length; i++)
            {
                m_tmpResult[i] = null;
            }
            Grid[] tmpResult = m_tmpResult;
            if (flag)
            {
                tmpResult[num5++] = GetGrid(x, num);
            }
            if (flag3)
            {
                tmpResult[num5++] = GetGrid(num3, y);
            }
            if (flag2)
            {
                tmpResult[num5++] = GetGrid(x, num2);
            }
            if (flag4)
            {
                tmpResult[num5++] = GetGrid(num4, y);
            }
            return (!(find == "Diagonal") && !(find == "Euclidean")) ? ((!(find == "DiagonalFree") && !(find == "EuclideanFree")) ? nothingToDo(flag, flag2, flag3, flag4, num, num2, num3, num4, grid, rows, cols, tmpResult, num5) : diagonalSuccessorsFree(flag, flag2, flag3, flag4, num, num2, num3, num4, grid, rows, cols, tmpResult, num5)) : diagonalSuccessors(flag, flag2, flag3, flag4, num, num2, num3, num4, grid, rows, cols, tmpResult, num5);
        }

        private double diagonal(Grid start, Grid end)
        {
            return Math.Max(Math.Abs(start.x - end.x), Math.Abs(start.y - end.y));
        }

        private double euclidean(Grid start, Grid end)
        {
            int num = start.x - end.x;
            int num2 = start.y - end.y;
            return Math.Sqrt(num * num + num2 * num2);
        }

        private double manhattan(Grid start, Grid end)
        {
            return Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
        }

        public List<Vector2> GetPath(int[][] grid, int[] s, int[] e, string f)
        {
            result.Clear();
            this.list.Clear();
            m_getObjCount = 0;
            if (grid[e[1]][e[0]] == 1)
            {
                return result;
            }
            find = ((f != null) ? f : "Diagonal");
            int num = grid[0].Length;
            int num2 = grid.Length;
            m_col = num;
            m_row = num2;
            int num3 = num * num2;
            int num4 = 1;
            List<Grid> list = new List<Grid>();
            list.Add(GetGrid(s[0], s[1]));
            list[0].f = 0.0;
            list[0].g = 0.0;
            list[0].h = s[0] + s[1] * num;
            new Grid(0, 0);
            Grid @object = new Grid(e[0], e[1]);
            @object.h = e[0] + e[1] * num;
            int num5 = 0;
            do
            {
                double num6 = num3;
                int index = 0;
                num5++;
                for (int i = 0; i < num4; i++)
                {
                    if (list[i].f < num6)
                    {
                        num6 = list[i].f;
                        index = i;
                    }
                }
                Grid object2 = list[index];
                list.RemoveAt(index);
                if (object2.h != @object.h)
                {
                    num4--;
                    Grid[] array = successors(object2.x, object2.y, grid, num2, num);
                    int i = 0;
                    for (int num7 = array.Length; i < num7; i++)
                    {
                        if (array[i] != null)
                        {
                            Grid object3;
                            (object3 = array[i]).p = object2;
                            double num10 = object3.f = (object3.g = 0.0);
                            double num11 = diagonal(object3, object2);
                            double num12 = diagonal(object3, @object);
                            Grid object4 = object3;
                            num10 = (object3.g = object2.g + num11);
                            object4.f = num10 + num12;
                            list.Add(object3);
                            this.list.Add(object3.h, 0);
                            num4++;
                        }
                    }
                    continue;
                }
                num4 = 0;
                float num14 = 999f;
                do
                {
                    if (object2.p != null)
                    {
                        float num15 = (object2.p.x - object2.x != 0) ? (Mathf.Floor((object2.p.y - object2.y) / (object2.p.x - object2.x)) * 100f) : 888f;
                        if (Math.Abs(num14 - num15) > 0.001f)
                        {
                            num14 = num15;
                            result.Add(new Vector2(object2.x, object2.y));
                        }
                    }
                    else
                    {
                        result.Add(new Vector2(object2.x, object2.y));
                    }
                }
                while ((object2 = object2.p) != null);
                result.Reverse();
            }
            while (num4 != 0);
            ResetPool();
            return result;
        }
    }
}