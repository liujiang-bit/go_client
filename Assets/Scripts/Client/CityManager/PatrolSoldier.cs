using System;
using Skyunion;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 城墙上巡逻的士兵
/// </summary>
[ExecuteInEditMode]
public class PatrolSoldier : MonoBehaviour
{
	public bool m_debug;

	private bool m_isShow;

    /// <summary>
    /// CityWall上的24个巡逻点位
    /// </summary>
    [Header("巡逻点")]
    [FormerlySerializedAs("m_dummyArry")]
    public GameObject[] m_seq_points;

	/// <summary>
	/// 士兵数量
	/// </summary>
	public int m_posNum = 7;

	/// <summary>
	/// 被攻击的时候的点位数量
	/// </summary>
	public int m_segmentNum = 12;

	//出生点位
	private GameObject[] m_pos;
	
	/// <summary>
	/// 被攻击时候的点位
	/// </summary>
	private GameObject[][] m_segment;

	/// <summary>
	/// 巡逻分段路径
	/// </summary>
	private GameObject[][] m_path;

	private bool m_isInited;

	private void Start()
	{
		if (!m_isInited)
		{
			Init();
		}
	}

	private void Init()
	{
		m_pos = new GameObject[m_posNum];
		m_pos[0] = m_seq_points[0];
		m_pos[1] = m_seq_points[2];
		m_pos[2] = m_seq_points[9];
		m_pos[3] = m_seq_points[10];
		m_pos[4] = m_seq_points[12];
		m_pos[5] = m_seq_points[14];
		m_pos[6] = m_seq_points[16];
		m_segment = new GameObject[12][];
		for (int i = 0; i < m_segmentNum; i++)
		{
			m_segment[i] = new GameObject[2];
		}
		//半段    18 -23为最长的3面强的中心2点位
		m_segment[0][0] = m_seq_points[0];
		m_segment[0][1] = m_seq_points[18];
				
		m_segment[1][0] = m_seq_points[1];
		m_segment[1][1] = m_seq_points[19];
		m_segment[2][0] = m_seq_points[2];
		m_segment[2][1] = m_seq_points[3];
		m_segment[3][0] = m_seq_points[4];
		m_segment[3][1] = m_seq_points[5];
		m_segment[4][0] = m_seq_points[6];
		m_segment[4][1] = m_seq_points[7];
		m_segment[5][0] = m_seq_points[8];
		m_segment[5][1] = m_seq_points[9];
		m_segment[6][0] = m_seq_points[10];
		m_segment[6][1] = m_seq_points[20];
		m_segment[7][0] = m_seq_points[11];
		m_segment[7][1] = m_seq_points[21];
		m_segment[8][0] = m_seq_points[12];
		m_segment[8][1] = m_seq_points[13];
		m_segment[9][0] = m_seq_points[14];
		m_segment[9][1] = m_seq_points[22];
		m_segment[10][0] = m_seq_points[15];
		m_segment[10][1] = m_seq_points[23];
		m_segment[11][0] = m_seq_points[16];
		m_segment[11][1] = m_seq_points[17];
		m_path = new GameObject[7][];
		m_path[0] = new GameObject[2];
		m_path[0][0] = m_seq_points[0];
		m_path[0][1] = m_seq_points[1];
		m_path[1] = new GameObject[4];
		m_path[1][0] = m_seq_points[2];
		m_path[1][1] = m_seq_points[3];
		m_path[1][2] = m_seq_points[4];
		m_path[1][3] = m_seq_points[5];
		m_path[2] = new GameObject[4];
		m_path[2][0] = m_seq_points[9];
		m_path[2][1] = m_seq_points[8];
		m_path[2][2] = m_seq_points[7];
		m_path[2][3] = m_seq_points[6];
		m_path[3] = new GameObject[2];
		m_path[3][0] = m_seq_points[10];
		m_path[3][1] = m_seq_points[11];
		m_path[4] = new GameObject[2];
		m_path[4][0] = m_seq_points[12];
		m_path[4][1] = m_seq_points[13];
		m_path[5] = new GameObject[2];
		m_path[5][0] = m_seq_points[14];
		m_path[5][1] = m_seq_points[15];
		m_path[6] = new GameObject[2];
		m_path[6][0] = m_seq_points[16];
		m_path[6][1] = m_seq_points[17];
		m_isInited = true;
	}

	private void Update()
	{
		try
		{
			if (!m_isInited)
			{
				Init();
			}
		}
		catch (Exception e)
		{
			CoreUtils.logService.Error(e.ToString());
		}
	}

    //获取士兵初始位置
    public Vector3 GetSoldierPos(int point)
	{
		int num = point - 1;
		if (m_pos != null)
		{
			GameObject gameObject = m_pos[num];
			if ((bool)gameObject)
			{
				return gameObject.transform.localPosition + base.transform.parent.localPosition;
			}
		}
		return Vector3.zero;
	}

    /// <summary>
    /// 获取士兵生成点位的巡逻路径
    /// </summary>
    /// <param name="self"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector2[] GetSoldierPath(int point)
	{
		int num = point - 1;
		if (!m_isInited)
		{
			Init();
		}
		if (num < 0 || m_path == null || num > m_path.Length)
		{
			return null;
		}
		GameObject[] array = m_path[num];
		Vector2[] array2 = new Vector2[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			Vector3 vector = array[i].transform.localPosition + base.transform.parent.localPosition;
			array2[i] = new Vector2(vector.x, vector.z);
		}
		return array2;
	}

	private void GetSegmentRange(int segment, out Vector2 startPos, out Vector2 endPos)
	{
		startPos = Vector2.zero;
		endPos = Vector2.zero;
		int num = segment - 1;
		if (num >= 0 && num <= m_segment.Length)
		{
			GameObject[] array = m_segment[num];
			Vector3 vector = array[0].transform.localPosition + base.transform.parent.localPosition;
			Vector3 vector2 = array[1].transform.localPosition + base.transform.parent.localPosition;
			startPos = new Vector2(vector.x, vector.z);
			endPos = new Vector2(vector2.x, vector2.z);
		}
	}

	private float GetYPosition(int point)
	{
		Vector3 soldierPos = GetSoldierPos(1);
		return soldierPos.y;
	}
}
