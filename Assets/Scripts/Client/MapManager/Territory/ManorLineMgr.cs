using System;
using System.Collections.Generic;
using Skyunion;
using UnityEngine;
/// <summary>
/// 领地线条管理
/// </summary>
public class ManorLineMgr
{
	private static readonly ManorLineMgr m_instance = new ManorLineMgr();

	private const string m_line_prefab_path = "troop/territory_line";

	private List<GameObject> m_territory_line_list_tactical = new List<GameObject>();

	private List<GameObject> m_territory_line_list_strategic = new List<GameObject>();
	
	private HashSet<Vector2[]> m_loading = new HashSet<Vector2[]>();

	public static ManorLineMgr GetInstance()
	{
		return m_instance;
	}

	public void CreateLine(Vector2[] pos_array, Color color, float uvStep, float width, bool isStrategic = false,string typename="",int dir =0)
	{
		float smooth_distance = 2f;

		if (m_loading.Contains(pos_array))
		{
			return;
		}

		m_loading.Add(pos_array);
		
		CoreUtils.assetService.Instantiate("territory_line_obj", (gameObject) =>
		{
			if (!m_loading.Contains(pos_array))
			{
				CoreUtils.assetService.Destroy(gameObject);
				return;
			}
			
			m_loading.Remove(pos_array);
			if (gameObject!=null)
			{
				
				gameObject.name = String.Format("t_{0}_{1}_{2}",pos_array[1],pos_array[2],dir);
				
				if (isStrategic)
				{
					gameObject.transform.parent = ManorMgr.manorStrategicLineRoot;
				}
				else
				{
					gameObject.transform.parent = ManorMgr.manorTacticalLineRoot;
				}

				gameObject.transform.position = Vector3.zero;
				LodMonorLine component = gameObject.GetComponent<LodMonorLine>();
				component.Init(pos_array, color, uvStep, width, smooth_distance,typename);
				if (isStrategic)
				{
					m_territory_line_list_strategic.Add(gameObject);
				}
				else
				{
					m_territory_line_list_tactical.Add(gameObject);
				}
			}
			
		});
	
	}

	public void ClearAllLine(bool clearTactical = true, bool clearStrategic = true)
	{
		m_loading.Clear();
		
		if (clearTactical)
		{
			for (int i = 0; i != m_territory_line_list_tactical.Count; i++)
			{
				if (m_territory_line_list_tactical[i].gameObject != null)
				{
					CoreUtils.assetService.Destroy(m_territory_line_list_tactical[i].gameObject);
				}
			}
			m_territory_line_list_tactical.Clear();
		}
		if (!clearStrategic)
		{
			return;
		}
		for (int j = 0; j != m_territory_line_list_strategic.Count; j++)
		{
			if (m_territory_line_list_strategic[j].gameObject != null)
			{
				CoreUtils.assetService.Destroy(m_territory_line_list_strategic[j].gameObject);
			}
		}
		m_territory_line_list_strategic.Clear();
	}

	public void SetCanUpdate(bool canUpdate)
	{
		for (int i = 0; i != m_territory_line_list_strategic.Count; i++)
		{
			GameObject gameObject = m_territory_line_list_strategic[i];
			if (gameObject != null)
			{
				LodMonorLine component = gameObject.GetComponent<LodMonorLine>();
				component.SetCanUpdate(canUpdate);
			}
		}
	}
}
