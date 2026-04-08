using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class TerritoryMgr
    {
        public static int GridSizeX = 18;

        public static int GridSizeY = 18;

        public static int checkDis = 1;

        private static Transform m_territoryLineRoot;

        public static Transform territoryLineRoot
        {
            get
            {
                if (TerritoryMgr.m_territoryLineRoot == null)
                {
                    TerritoryMgr.m_territoryLineRoot = new GameObject("AllTerritoryLine").transform;
                }
                return TerritoryMgr.m_territoryLineRoot;
            }
        }

        public static void SetGridSize(float x, float y, float dis)
        {
            TerritoryMgr.GridSizeX = (int)x;
            TerritoryMgr.GridSizeY = (int)y;
            TerritoryMgr.checkDis = (int)dis;
        }

        public static void UpdateTerritoryS(Material mat)
        {
            TerritoryMgr.UpdateTerritory(mat);
        }

        public static void SetShowTerritoryLine(bool isShow)
        {
            TerritoryLineMgr.GetInstance().SetTacticalLineShow(isShow);
        }

        public static void UpdateFakeTerritoryS(Material mat)
        {
            TerritoryMgr.UpdateTerritory(mat);
        }

        private static void UpdateTerritory(Material mat)
        {
            //TerritoryLineMgr.GetInstance().ClearAllLine(true, false);
            //int num = territoryInfos.length();
            //for (int i = 1; i < num + 1; i++)
            //{
            //	LuaTable luaTable = territoryInfos[i] as LuaTable;
            //	string a = luaTable["type"] as string;
            //	LuaTable luaTable2 = luaTable["list"] as LuaTable;
            //	float uvStep = (float)((double)luaTable["uvStep"]);
            //	float width = (float)((double)luaTable["width"]);
            //	Material mat2 = mat;
            //	Material material = luaTable["mat"] as Material;
            //	if (material != null)
            //	{
            //		mat2 = material;
            //	}
            //	int num2 = luaTable2.length();
            //	Dictionary<int, List<TerritoryItem>> dictionary = new Dictionary<int, List<TerritoryItem>>();
            //	for (int j = 1; j < num2 + 1; j++)
            //	{
            //		LuaTable luaTable3 = (LuaTable)luaTable2[j];
            //		int num3 = (int)((double)luaTable3["allianceId"]);
            //		int num4 = (int)((double)luaTable3["gridPosX"]);
            //		int num5 = (int)((double)luaTable3["gridPosY"]);
            //		int start_x = num4 * TerritoryMgr.GridSizeX;
            //		int start_y = num5 * TerritoryMgr.GridSizeY;
            //		int end_x = num4 * TerritoryMgr.GridSizeX + TerritoryMgr.GridSizeX;
            //		int end_y = num5 * TerritoryMgr.GridSizeY + TerritoryMgr.GridSizeY;
            //		Color color = (Color)luaTable3["lineColor"];
            //		TerritoryItem item = new TerritoryItem(num3, color, start_x, start_y, end_x, end_y);
            //		if (dictionary.ContainsKey(num3))
            //		{
            //			dictionary[num3].Add(item);
            //		}
            //		else
            //		{
            //			dictionary.Add(num3, new List<TerritoryItem>
            //			{
            //				item
            //			});
            //		}
            //	}
            //	if (fake_territory_item_table != null && a == "active")
            //	{
            //		for (int k = 1; k < fake_territory_item_table.length() + 1; k++)
            //		{
            //			LuaTable luaTable4 = (LuaTable)fake_territory_item_table[k];
            //			int num6 = (int)((double)luaTable4["allianceId"]);
            //			int num7 = (int)((double)luaTable4["gridPosX"]);
            //			int num8 = (int)((double)luaTable4["gridPosY"]);
            //			int start_x2 = num7 * TerritoryMgr.GridSizeX;
            //			int start_y2 = num8 * TerritoryMgr.GridSizeY;
            //			int end_x2 = num7 * TerritoryMgr.GridSizeX + TerritoryMgr.GridSizeX;
            //			int end_y2 = num8 * TerritoryMgr.GridSizeY + TerritoryMgr.GridSizeY;
            //			Color color2 = (Color)luaTable4["lineColor"];
            //			TerritoryItem item2 = new TerritoryItem(num6, color2, start_x2, start_y2, end_x2, end_y2);
            //			if (dictionary.ContainsKey(num6))
            //			{
            //				dictionary[num6].Add(item2);
            //			}
            //			else
            //			{
            //				dictionary.Add(num6, new List<TerritoryItem>
            //				{
            //					item2
            //				});
            //			}
            //		}
            //	}
            //	foreach (List<TerritoryItem> current in dictionary.Values)
            //	{
            //		TerritoryMgr.CreateTerritoryLine(current, uvStep, width, mat2);
            //	}
            //}
        }

        private static void CreateTerritoryLine(List<TerritoryItem> territory_list, float uvStep, float width, Material mat)
        {
            int num = 2147483647;
            int num2 = -2147483648;
            int num3 = 2147483647;
            int num4 = -2147483648;
            for (int i = 0; i < territory_list.Count; i++)
            {
                int startPosX = territory_list[i].startPosX;
                int startPosY = territory_list[i].startPosY;
                int endPosX = territory_list[i].endPosX;
                int endPosY = territory_list[i].endPosY;
                if (startPosX < num)
                {
                    num = startPosX;
                }
                if (startPosY < num3)
                {
                    num3 = startPosY;
                }
                if (endPosX > num2)
                {
                    num2 = endPosX;
                }
                if (endPosY > num4)
                {
                    num4 = endPosY;
                }
            }
            NewTerritoryMapItem newTerritoryMapItem = new NewTerritoryMapItem(num2 - num, num4 - num3, num, num3);
            for (int j = 0; j < territory_list.Count; j++)
            {
                newTerritoryMapItem.AddTerritoryItem(territory_list[j]);
            }
            List<List<Vector2>> posList = newTerritoryMapItem.GetPosList();
            for (int k = 0; k < posList.Count; k++)
            {
                Vector2[] pos_array = posList[k].ToArray();
                TerritoryLineMgr.GetInstance().CreateLine(pos_array, newTerritoryMapItem.lineColor, uvStep, width, mat, false);
            }
        }

        public static void ClearAllLine_S(bool clearTactical, bool clearStrategic)
        {
            TerritoryLineMgr.GetInstance().ClearAllLine(clearTactical, clearStrategic);
        }

        public static void CreateLineFromCache_S(Vector2[] vector2_array, float uvStep, float width, Material mat, Color line_color)
        {
            if (vector2_array.Length > 3)
            {
                Vector2 vector = vector2_array[vector2_array.Length - 2];
                Vector2 a = vector2_array[0];
                Vector2 b = vector2_array[1];
                if (Mathf.Abs(vector.x - b.x) > 0.001f && Mathf.Abs(vector.y - b.y) > 0.001f)
                {
                    List<Vector2> list = new List<Vector2>(vector2_array);
                    Vector2 vector2 = (a + b) * 0.5f;
                    list[0] = vector2;
                    list.Add(vector2);
                    vector2_array = list.ToArray();
                }
            }
            TerritoryLineMgr.GetInstance().CreateLine(vector2_array, line_color, uvStep, width, mat, true);
        }

        public static void SetStrategicShow_S(bool isShow)
        {
            TerritoryMgr.territoryLineRoot.gameObject.SetActive(isShow);
            TerritoryLineMgr.GetInstance().SetCanUpdate(isShow);
        }
    }
}