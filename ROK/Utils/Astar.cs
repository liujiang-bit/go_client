using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class AStar
    {
        private class _Object
        {
            public int x
            {
                get;
                set;
            }

            public int y
            {
                get;
                set;
            }

            public double f
            {
                get;
                set;
            }

            public double g
            {
                get;
                set;
            }

            public int v
            {
                get;
                set;
            }

            public _Object p
            {
                get;
                set;
            }

            public bool used
            {
                get;
                set;
            }

            public _Object(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public List<Vector2> result = new List<Vector2>();

        private List<_Object> m_objPool = new List<_Object>();

        private List<_Object> m_objPoolUnused = new List<_Object>();

        private string find;

        private int m_getObjCount;

        private _Object[] m_tmpResult = new _Object[8];

        private int m_row;

        private int m_col;

        private Dictionary<int, byte> list = new Dictionary<int, byte>();

        private _Object GetObject(int x, int y)
        {
            _Object @object = null;
            int num = x + y * this.m_col;
            bool flag = this.list.ContainsKey(num);
            if (flag)
            {
                return @object;
            }
            int count = this.m_objPoolUnused.Count;
            if (count > 0)
            {
                @object = this.m_objPoolUnused[count - 1];
                @object.x = x;
                @object.y = y;
                @object.v = num;
                this.m_objPoolUnused.RemoveAt(count - 1);
            }
            else
            {
                @object = new _Object(x, y);
                @object.v = num;
                this.m_objPool.Add(@object);
            }
            this.m_getObjCount++;
            return @object;
        }

        private void ResetPool()
        {
            this.m_objPoolUnused.Clear();
            for (int i = 0; i < this.m_objPool.Count; i++)
            {
                _Object @object = this.m_objPool[i];
                _Object arg_33_0 = @object;
                int num = 0;
                @object.v = num;
                @object.y = num;
                arg_33_0.x = num;
                _Object arg_4B_0 = @object;
                double num2 = 0.0;
                @object.g = num2;
                arg_4B_0.f = num2;
                @object.p = null;
                this.m_objPoolUnused.Add(@object);
            }
        }

        private _Object[] diagonalSuccessors(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
        {
            if (xN)
            {
                if (xE && grid[N][E] == 0)
                {
                    result[i++] = this.GetObject(E, N);
                }
                if (xW && grid[N][W] == 0)
                {
                    result[i++] = this.GetObject(W, N);
                }
            }
            if (xS)
            {
                if (xE && grid[S][E] == 0)
                {
                    result[i++] = this.GetObject(E, S);
                }
                if (xW && grid[S][W] == 0)
                {
                    result[i++] = this.GetObject(W, S);
                }
            }
            return result;
        }

        private _Object[] diagonalSuccessorsFree(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
        {
            xN = (N > -1);
            xS = (S < rows);
            xE = (E < cols);
            xW = (W > -1);
            if (xE)
            {
                if (xN && grid[N][E] == 0)
                {
                    result[i++] = this.GetObject(E, N);
                }
                if (xS && grid[S][E] == 0)
                {
                    result[i++] = this.GetObject(E, S);
                }
            }
            if (xW)
            {
                if (xN && grid[N][W] == 0)
                {
                    result[i++] = this.GetObject(W, N);
                }
                if (xS && grid[S][W] == 0)
                {
                    result[i++] = this.GetObject(W, S);
                }
            }
            return result;
        }

        private _Object[] nothingToDo(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
        {
            return result;
        }

        private _Object[] successors(int x, int y, int[][] grid, int rows, int cols)
        {
            int num = y - 1;
            int num2 = y + 1;
            int num3 = x + 1;
            int num4 = x - 1;
            bool flag = num > -1 && grid[num][x] == 0;
            bool flag2 = num2 < rows && grid[num2][x] == 0;
            bool flag3 = num3 < cols && grid[y][num3] == 0;
            bool flag4 = num4 > -1 && grid[y][num4] == 0;
            int i = 0;
            for (int j = 0; j < this.m_tmpResult.Length; j++)
            {
                this.m_tmpResult[j] = null;
            }
            _Object[] tmpResult = this.m_tmpResult;
            if (flag)
            {
                tmpResult[i++] = this.GetObject(x, num);
            }
            if (flag3)
            {
                tmpResult[i++] = this.GetObject(num3, y);
            }
            if (flag2)
            {
                tmpResult[i++] = this.GetObject(x, num2);
            }
            if (flag4)
            {
                tmpResult[i++] = this.GetObject(num4, y);
            }
            return (!(this.find == "Diagonal") && !(this.find == "Euclidean")) ? ((!(this.find == "DiagonalFree") && !(this.find == "EuclideanFree")) ? this.nothingToDo(flag, flag2, flag3, flag4, num, num2, num3, num4, grid, rows, cols, tmpResult, i) : this.diagonalSuccessorsFree(flag, flag2, flag3, flag4, num, num2, num3, num4, grid, rows, cols, tmpResult, i)) : this.diagonalSuccessors(flag, flag2, flag3, flag4, num, num2, num3, num4, grid, rows, cols, tmpResult, i);
        }

        private double diagonal(_Object start, _Object end)
        {
            return (double)Math.Max(Math.Abs(start.x - end.x), Math.Abs(start.y - end.y));
        }

        private double euclidean(_Object start, _Object end)
        {
            int num = start.x - end.x;
            int num2 = start.y - end.y;
            return Math.Sqrt((double)(num * num + num2 * num2));
        }

        private double manhattan(_Object start, _Object end)
        {
            return (double)(Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y));
        }

        public List<Vector2> GetPath(int[][] grid, int[] s, int[] e, string f)
        {
            this.result.Clear();
            this.list.Clear();
            this.m_getObjCount = 0;
            if (grid[e[1]][e[0]] == 1)
            {
                return this.result;
            }
            this.find = ((f != null) ? f : "Diagonal");
            int num = grid[0].Length;
            int num2 = grid.Length;
            this.m_col = num;
            this.m_row = num2;
            int num3 = num * num2;
            int num4 = 1;
            List<_Object> list = new List<_Object>();
            list.Add(this.GetObject(s[0], s[1]));
            list[0].f = 0.0;
            list[0].g = 0.0;
            list[0].v = s[0] + s[1] * num;
            _Object @object = new _Object(0, 0);
            _Object object2 = new _Object(e[0], e[1]);
            object2.v = e[0] + e[1] * num;
            int num5 = 0;
            do
            {
                double num6 = (double)num3;
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
                _Object object3 = list[index];
                list.RemoveAt(index);
                if (object3.v != object2.v)
                {
                    num4--;
                    _Object[] array = this.successors(object3.x, object3.y, grid, num2, num);
                    int i = 0;
                    int num7 = array.Length;
                    while (i < num7)
                    {
                        if (array[i] != null)
                        {
                            (@object = array[i]).p = object3;
                            _Object arg_1C3_0 = @object;
                            double num8 = 0.0;
                            @object.g = num8;
                            arg_1C3_0.f = num8;
                            double num9 = this.diagonal(@object, object3);
                            double num10 = this.diagonal(@object, object2);
                            _Object arg_1FC_0 = @object;
                            num8 = object3.g + num9;
                            @object.g = num8;
                            arg_1FC_0.f = num8 + num10;
                            list.Add(@object);
                            this.list.Add(@object.v, 0);
                            num4++;
                        }
                        i++;
                    }
                }
                else
                {
                    num4 = 0;
                    float num11 = 999f;
                    do
                    {
                        if (object3.p != null)
                        {
                            float num12;
                            if (object3.p.x - object3.x == 0)
                            {
                                num12 = 888f;
                            }
                            else
                            {
                                num12 = Mathf.Floor((float)((object3.p.y - object3.y) / (object3.p.x - object3.x))) * 100f;
                            }
                            if (Math.Abs(num11 - num12) > 0.001f)
                            {
                                num11 = num12;
                                this.result.Add(new Vector2((float)object3.x, (float)object3.y));
                            }
                        }
                        else
                        {
                            this.result.Add(new Vector2((float)object3.x, (float)object3.y));
                        }
                    }
                    while ((object3 = object3.p) != null);
                    this.result.Reverse();
                }
            }
            while (num4 != 0);
            this.ResetPool();
            return this.result;
        }
    }
}