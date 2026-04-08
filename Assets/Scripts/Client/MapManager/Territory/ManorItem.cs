using UnityEngine;
/// <summary>
/// 领地数据
/// </summary>
public class ManorItem
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
			return m_alliance_id;
		}
		set
		{
			m_alliance_id = value;
		}
	}

	public Color color
	{
		get
		{
			return m_color;
		}
		set
		{
			m_color = value;
		}
	}

	public int startPosX
	{
		get
		{
			return m_start_pos_x;
		}
		set
		{
			m_start_pos_x = value;
		}
	}

	public int startPosY
	{
		get
		{
			return m_start_pos_y;
		}
		set
		{
			m_start_pos_y = value;
		}
	}

	public int endPosX
	{
		get
		{
			return m_end_pos_x;
		}
		set
		{
			m_end_pos_x = value;
		}
	}

	public int endPosY
	{
		get
		{
			return m_end_pos_y;
		}
		set
		{
			m_end_pos_y = value;
		}
	}

	public ManorItem(int _alliance_id, Color _color, int _start_x, int _start_y, int _end_x, int _end_y)
	{
		allianceId = _alliance_id;
		color = _color;
		startPosX = _start_x;
		startPosY = _start_y;
		endPosX = _end_x;
		endPosY = _end_y;
	}
}
