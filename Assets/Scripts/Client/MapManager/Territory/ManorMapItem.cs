using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;
/// <summary>
/// 领地数据
/// </summary>
public class ManorMapItem
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

	public Color lineColor => m_line_color;

	public ManorMapItem(int width, int height, int x_min, int y_min)
	{
		m_width = width + 2;
		m_height = height + 2;
		m_x_min = x_min;
		m_y_min = y_min;
		m_map = new BitArray(m_width * m_height);
		m_discard_map = new BitArray(m_width * m_height);
	}

	private bool IsManorVertex(Pos2 pos)
	{
		if (pos.x < 0 || pos.x >= m_width || pos.y < 0 || pos.y >= m_height)
		{
			return false;
		}
		return m_map[pos.x + pos.y * m_width];
	}

	private bool IsManorVertex(int x, int y)
	{
		if (x < 0 || x >= m_width || y < 0 || y >= m_height)
		{
			return false;
		}
		return m_map[x + y * m_width];
	}

	private void SetAsManorVertex(int x, int y)
	{
		m_map[x + y * m_width] = true;
	}

	private void SetAsDiscardVertex(int x, int y)
	{
		m_discard_map[x + y * m_width] = true;
	}

	private void SetAsDiscardVertex(Pos2 pos)
	{
		SetAsDiscardVertex(pos.x, pos.y);
	}

	private bool IsDiscardVertex(Pos2 pos)
	{
		return IsDiscardVertex(pos.x, pos.y);
	}

	private bool IsDiscardVertex(int x, int y)
	{
		if (x < 0 || x >= m_width || y < 0 || y >= m_height)
		{
			return false;
		}
		return m_discard_map[x + y * m_width];
	}

	public void AddManorItem(ManorItem item)
	{
		int num = item.endPosX - item.startPosX;
		int num2 = item.endPosY - item.startPosY;
		int num3 = item.startPosX - m_x_min;
		int num4 = item.startPosY - m_y_min;
		m_line_color = item.color;
		for (int i = 0; i <= num2; i++)
		{
			for (int j = 0; j <= num; j++)
			{
				SetAsManorVertex(num3 + j, num4 + i);
			}
		}
	}

	public void CalculateManorLine()
	{
		CalculateOutterManorLine();
		CalculateInnerManorLine();
	}

	private void CalculateOutterManorLine()
	{
		Pos2 pos = default(Pos2);
		bool flag = false;
		for (int i = 0; i != m_height; i++)
		{
			for (int j = 0; j != m_width; j++)
			{
				if (IsManorVertex(new Pos2(j, i)))
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
		m_outter_territory_line.Add(GetWorldPos(pos));
		SetAsDiscardVertex(pos);
		GetNextPos(pos, 270f, m_outter_territory_line, pos);
	}

	private void CalculateInnerManorLine()
	{
		for (int i = 0; i != m_height; i++)
		{
			for (int j = 0; j != m_width; j++)
			{
				Pos2 pos = new Pos2(j, i);
				if (!IsManorVertex(pos) && IsManorVertex(j, i - 1) && !IsDiscardVertex(j, i - 1))
				{
					pos = new Pos2(j, i - 1);
					List<Pos2> list = new List<Pos2>();
					m_inner_territory_line_list.Add(list);
					list.Add(GetWorldPos(pos));
					SetAsDiscardVertex(pos);
					GetNextPos(pos, 0f, list, pos);
				}
			}
		}
	}

	public List<Pos2> GetOutterManorLine()
	{
		return m_outter_territory_line;
	}

	public List<List<Pos2>> GetInnerManorLines()
	{
		return m_inner_territory_line_list;
	}


	private Dictionary<float, Vector2> m_cache = new Dictionary<float, Vector2>();
	private void GetNextPos(Pos2 current_pos, float search_direction, List<Pos2> territory_line_list, Pos2 start_pos)
	{
		int num = 0;
		Pos2 pos;
		while (true)
		{
			if (num != 4)
			{
				var d = (search_direction + 90f * (float) num)%360;
				Vector2 vector = Vector2.zero;
				if (!m_cache.TryGetValue(d,out vector))
				{
					vector = Common.DegreeToVector2(d);
					m_cache.Add(d,vector);
				}
				pos = new Pos2(current_pos.x + (int)vector.x, current_pos.y + (int)vector.y);
				if (IsManorVertex(pos))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		SetAsDiscardVertex(pos);
		if (pos == start_pos)
		{
			Pos2 p = territory_line_list[territory_line_list.Count - 1];
			Pos2 s = territory_line_list[0];
			if (territory_line_list.Count==4 && s.x%18==1)
			{
				s.x = p.x;
				territory_line_list[0] = s;
			}
			
			Pos2 item = (p + s) / 2;
			territory_line_list.Insert(0, item);
			territory_line_list.Add(item);
			return;
		}
		if (num != 1)
		{
			search_direction += 90f * (float)(num - 1);
			territory_line_list.Add(GetWorldPos(current_pos));
		}
		GetNextPos(pos, search_direction, territory_line_list, start_pos);
	}

	private Pos2 GetWorldPos(Pos2 local_pos)
	{
		return new Pos2(local_pos.x + m_x_min, local_pos.y + m_y_min);
	}
}
