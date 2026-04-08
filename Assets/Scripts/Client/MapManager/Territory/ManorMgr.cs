using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 领地管理器
/// </summary>
///
///
///
public class ManorData
{
	public float uvStep;

	public string type;

	public List<ManorItem> list;

	public float width;

	public Material mat;
}

public class ManorLod3Data
{
	public Vector2[] points;

	public Color color;

	public byte dir=0;
}

public class ManorMgr
{
	public static int GridSizeX = 18;

	public static int GridSizeY = 18;

	private static Transform m_manorLineRoot;
	
	private static Transform m_manorTacticalLineRoot;

	public static Transform manorStrategicLineRoot
	{
		get
		{
			if (m_manorLineRoot == null)
			{
				m_manorLineRoot = new GameObject("AllTerritoryStrategicLine_4-8").transform;
			}
			return m_manorLineRoot;
		}
	}
	
	public static Transform manorTacticalLineRoot
	{
		get
		{
			if (m_manorTacticalLineRoot == null)
			{
				m_manorTacticalLineRoot = new GameObject("AllTerritoryTacticalLine_0_3").transform;
			}
			return m_manorTacticalLineRoot;
		}
	}

	public static void SetGridSize(float x, float y)
	{
		GridSizeX = (int)x;
		GridSizeY = (int)y;
	}

	public static void UpdateTerritoryS(List<ManorData> manorInfos)
	{
		UpdateManor(manorInfos);
	}

	//设置假领地数据
	public static void UpdateFakeTerritoryS(List<ManorData> manorInfos, List<ManorItem> fake_manor_item_table)
	{
		UpdateManor(manorInfos, fake_manor_item_table);
	}

	private static void UpdateManor(List<ManorData>  manorInfos, List<ManorItem> fake_manor_item_table = null)
	{
		if (manorStrategicLineRoot.gameObject.activeSelf)
		{
			return;
		}
		
		ManorLineMgr.GetInstance().ClearAllLine(clearTactical: true, clearStrategic: false);
		int num = manorInfos.Count;
		for (int i = 0; i < num ; i++)
		{
 			ManorData territoryInfo = manorInfos[i] as ManorData;
			string a = territoryInfo.type;
			List<ManorItem> territoryList = territoryInfo.list;
			float uvStep = (float)(double)territoryInfo.uvStep;
			float width = (float)(double)territoryInfo.width; 
		

			int num2 = territoryList.Count;
			Dictionary<int, List<ManorItem>> dictionary = new Dictionary<int, List<ManorItem>>();
			for (int j = 0; j < num2 ; j++)
			{
				ManorItem territoryItem = territoryList[j];
//				int num3 = (int)(double)territoryItem.allianceId;
//				int num4 = (int)(double)territoryItem["gridPosX"];
//				int num5 = (int)(double)territoryItem["gridPosY"];
//				int start_x = num4 * GridSizeX;
//				int start_y = num5 * GridSizeY;
//				int end_x = num4 * GridSizeX + GridSizeX;
//				int end_y = num5 * GridSizeY + GridSizeY;
				if (dictionary.ContainsKey(territoryItem.allianceId))
				{
					dictionary[territoryItem.allianceId].Add(territoryItem);
					continue;
				}
				List<ManorItem> list = new List<ManorItem>();
				list.Add(territoryItem);
				dictionary.Add(territoryItem.allianceId, list);
			}
			if (fake_manor_item_table != null && a == "active")
			{
				for (int k = 0; k < fake_manor_item_table.Count ; k++)
				{
					ManorItem territoryItem = fake_manor_item_table[k];
//					int allianceId = (int)(double)territoryItem["allianceId"];
//					int gridX = (int)(double)territoryItem["gridPosX"];
//					int gridY = (int)(double)territoryItem["gridPosY"];
//					int start_x2 = gridX * GridSizeX;
//					int start_y2 = gridY * GridSizeY;
//					int end_x2 = gridX * GridSizeX + GridSizeX;
//					int end_y2 = gridY * GridSizeY + GridSizeY;
//					Color color2 = (Color)territoryItem["lineColor"];
//					TerritoryItem item2 = new TerritoryItem(allianceId, color2, start_x2, start_y2, end_x2, end_y2);
					if (dictionary.ContainsKey(territoryItem.allianceId))
					{
						dictionary[territoryItem.allianceId].Add(territoryItem);
						continue;
					}
					List<ManorItem> list2 = new List<ManorItem>();
					list2.Add(territoryItem);
					dictionary.Add(territoryItem.allianceId, list2);
				}
			}
			foreach (List<ManorItem> value in dictionary.Values)
			{
				CreateManorLine(value, uvStep, width,a);
			}
		}
	}

	private static void CreateManorLine(List<ManorItem> territory_list, float uvStep, float width,string typename)
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = int.MaxValue;
		int num4 = int.MinValue;
		for (int i = 0; i != territory_list.Count; i++)
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
		ManorMapItem territoryMapItem = new ManorMapItem(num2 - num, num4 - num3, num, num3);
		for (int j = 0; j != territory_list.Count; j++)
		{
			territoryMapItem.AddManorItem(territory_list[j]);
		}
		territoryMapItem.CalculateManorLine();
		List<Pos2> outterTerritoryLine = territoryMapItem.GetOutterManorLine();
		Vector2[] array = new Vector2[outterTerritoryLine.Count];
		for (int k = 0; k != array.Length; k++)
		{
			ref Vector2 reference = ref array[k];
			Pos2 pos = outterTerritoryLine[outterTerritoryLine.Count - 1 - k];
			float x = pos.x;
			Pos2 pos2 = outterTerritoryLine[outterTerritoryLine.Count - 1 - k];
			reference = new Vector2(x, pos2.y);
		}
		ManorLineMgr.GetInstance().CreateLine(array, territoryMapItem.lineColor, uvStep, width,false,typename);
		List<List<Pos2>> innerTerritoryLines = territoryMapItem.GetInnerManorLines();
		for (int l = 0; l != innerTerritoryLines.Count; l++)
		{
			Vector2[] array2 = new Vector2[innerTerritoryLines[l].Count];
			int count = innerTerritoryLines[l].Count;
			for (int m = 0; m != innerTerritoryLines[l].Count; m++)
			{
				ref Vector2 reference2 = ref array2[m];
				Pos2 pos3 = innerTerritoryLines[l][count - 1 - m];
				float x2 = pos3.x;
				Pos2 pos4 = innerTerritoryLines[l][count - 1 - m];
				reference2 = new Vector2(x2, pos4.y);
			}
			ManorLineMgr.GetInstance().CreateLine(array2, territoryMapItem.lineColor, uvStep, width,false,typename);
		}
	}

	public static void ClearAllLine_S(bool clearTactical, bool clearStrategic)
	{
		Debug.Log("领土 清空");
		ManorLineMgr.GetInstance().ClearAllLine(clearTactical, clearStrategic);
	}

	public static void CreateLineFromCache_S(Vector2[] vector2_array, float uvStep, float width, Color line_color,byte dir = 0)
	{
		// if (vector2_array.Length > 3)
		// {
		// 	Vector2 vector = vector2_array[vector2_array.Length - 2];
		// 	Vector2 a = vector2_array[0];
		// 	Vector2 b = vector2_array[1];
		// 	if (Mathf.Abs(vector.x - b.x) > 0.001f && Mathf.Abs(vector.y - b.y) > 0.001f)
		// 	{
		// 		List<Vector2> list = new List<Vector2>(vector2_array);
		// 		Vector2 item = list[0] = (a + b) * 0.5f;
		// 		list.Add(item);
		// 		list.Add(a);
		// 		vector2_array = list.ToArray();
		// 	}
		// }

		CheckLineClockwise(ref vector2_array,dir);
		
		
		// if (dir==2)
		// {
		// 	// vector2_array = vector2_array.Reverse().ToArray();
		// 	Array.Reverse(vector2_array);
		// }
		//

		ManorLineMgr.GetInstance().CreateLine(vector2_array, line_color, uvStep, width, isStrategic: true,"",dir);
	}


	//检查线是否为为顺时针 dir  1 左侧 2 右侧
	private static void CheckLineClockwise(ref Vector2[] vector2_array,byte dir)
	{
		
		float d = 0;
		for (int j = 0; j < vector2_array.Length -1; j++)
		{
		    d+= -(0.5f*(vector2_array[j+1].y+vector2_array[j].y)*(vector2_array[j+1].x-vector2_array[j].x));
		}
		
		if (d<0 && dir==2)
		{
		    //逆时针  转回来
		    Array.Reverse(vector2_array);
		}
		
		// var sp = vector2_array[1];
		// var ep = vector2_array[2];
		//
		// if (sp.x == 954)
		// {
		// 	Debug.Log(sp);
		// }
		//
		// var pdir = sp - ep;
		// var pn = pdir.normalized;
		// if (pn.x<-0.5)
		// {
		// 	//left
		// 	if (dir==2)
		// 	{
		// 		Array.Reverse(vector2_array);
		// 	}
		// 	
		// }else if (pn.x > 0.5)
		// {
		// 	//right
		// 	if (dir==2)
		// 	{
		// 		Array.Reverse(vector2_array);
		// 	}
		// }else if (pn.y > 0.5)
		// {
		// 	//up
		// 	
		// 	if (dir==2)
		// 	{
		// 		Array.Reverse(vector2_array);
		// 	}
		// }else if (pn.y<-0.5)
		// {
		// 	//down
		// 	if (dir==1)
		// 	{
		// 		Array.Reverse(vector2_array);
		// 	}
		// }
	}

	public static void SetStrategicShow_S(bool isShowHight,bool isShowLow)
	{
		manorStrategicLineRoot.gameObject.SetActive(isShowHight);
		manorTacticalLineRoot.gameObject.SetActive(isShowLow);
		ManorLineMgr.GetInstance().SetCanUpdate(isShowHight);
	}
	
	public static void SetStrategicShowLODMENU_S(bool isShow)
	{
		manorStrategicLineRoot.gameObject.SetActive(isShow);
		manorTacticalLineRoot.gameObject.SetActive(false);
		ManorLineMgr.GetInstance().SetCanUpdate(true);
	}
}
