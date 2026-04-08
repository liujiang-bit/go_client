using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using Skyunion;
using UnityEngine;
/// <summary>
/// 地图内寻路 挂载在CityBuildingContainer上
/// </summary>
public class TownSearch : MonoBehaviour
{
	private int[] m_map;

	private int m_size;

	private Astar m_astar;

	private int[][] m_grids;

	private string m_mapMd5 = string.Empty;

#if UNITY_EDITOR_OSX
	private bool m_debug = false;
#else
	private bool m_debug = false;
#endif
	

	public static float CITY_GRID_SIZE = 0.1f;
	
	//半个格单位
	public static float CITY_GRID_SIZE_HALF = 0.05F;
	
	//相互的
	public static int CITY_GRID_SIZE_RECIPROCAL = 10;
	
	private Transform m_cityBuildingContainer;

	private int MakeGridKey(int x, int y, int size)
	{
		int num = Mathf.CeilToInt(((float)size - 1f) * 0.5f);
		int num2 = y + num;
		int num3 = x + num + 1;
		return num2 * size + num3;
	}

	public void Draw(int[] gridMap, int[] astarMap,List<Vector2> paths)
	{
		int num = gridMap.Length;
		int citysize = (int)Mathf.Pow(num, 0.5f);
		int halfSize = Mathf.FloorToInt((float)citysize * 0.5f);
		GameObject gameObject = new GameObject("m1");
		gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		
		int[] pathIndex = new int[paths.Count];

		for (int i = 0; i < pathIndex.Length; i++)
		{
			pathIndex[i] = MakeGridKey((int)paths[i].x, (int)paths[i].y, citysize);
//			Debug.LogFormat(" index {0}  x:{1}  y:{2}",pathIndex[i],paths[i].x,paths[i].y);
		}

		float h = Random.Range(0f, 360f);
		float s = 100f;
		float v = 100f;
		Color randColor = Color.HSVToRGB(h, s, v);

		
		
		for (int i = -halfSize; i <= halfSize; i++)
		{
			for (int j = -halfSize; j <= halfSize; j++)
			{
				int index = MakeGridKey(j, i, citysize);

				if (index >= gridMap.Length)
				{
					break;
				}
				Color color = Color.green;
				if (pathIndex.Contains(index))
				{
					color = Color.blue;
				}
				else
				{
					int num4 = gridMap[index];
					
					if (num4 != 5)
					{
						color = Color.red;
					}
				}

				
				int r = i;
				int c = j;
				
				string name = "g_" + r.ToString() + "_" + c.ToString();


				Transform tile = gameObject.transform.Find(name);
				if (!tile)
				{
					CoreUtils.assetService.Instantiate("building_move_bottom", (gameObject2) =>
					{
						gameObject2.name = name;
						gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
						gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
						gameObject2.transform.localPosition = new Vector3((float) c * 0.1f, 0.01f, (float) r * 0.1f);
						Renderer component = gameObject2.GetComponent<Renderer>();
						component.material.color = color;
					});
				}
				else
				{
					Renderer component = tile.GetComponent<Renderer>();
					if (component.material.color == Color.green)
					{
						component.material.color = color;
					}
				}


			}
		}

		if (astarMap ==null)
		{
			return;
		}
		int num5 = astarMap.Length;
		if (num5 <= 0)
		{
			return;

			astarMap = m_map;
			num5 = m_map.Length;
		}
		int citySize2 = (int)Mathf.Pow(num5, 0.5f);
		int halfSize2 = Mathf.FloorToInt((float)citySize2 * 0.5f);
		GameObject gameObject3 = new GameObject("m2");
		gameObject3.transform.SetParent(base.transform, worldPositionStays: false);
		for (int k = -halfSize2; k <= halfSize2; k++)
		{
			for (int l = -halfSize2; l <= halfSize2; l++)
			{
				int index2 = MakeGridKey(l, k, citySize2);
				int num8 = (int)(double)astarMap[index2];
				Color color2 = Color.blue;
				if (num8 == 1)
				{
					color2 = Color.red;
				}
//				color2.a = 0.5f;
				int r = k;
				int c = l;
				CoreUtils.assetService.Instantiate("building_move_bottom", (gameObject4) =>
				{
					gameObject4.name = "g_" + c + "_" + r;
					gameObject4.transform.SetParent(gameObject3.transform, worldPositionStays: false);
//					gameObject4.transform.localScale = new Vector3(0.5f, 0.001f, 0.5f);
					gameObject4.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					gameObject4.transform.localPosition = new Vector3((float)c * 0.1f, 0.01f, (float)r * 0.1f);
					Renderer component2 = gameObject4.GetComponent<Renderer>();
					component2.material.color = color2;
				});
			}
		}
	}

	public void SetFinderMap(int[] map, int size,Transform cityContainer)
	{
		m_map = map;
		m_size = size;
		m_cityBuildingContainer = cityContainer;
		int num = size * size;
		m_grids = new int[size][];
		if (size * size != map.Length)
		{
			if (num > map.Length)
			{
				num = map.Length;
			}
			CoreUtils.logService.Error("SetFinderMap: size invalid ---- size = " + size + ", map.length = " + map.Length);
		}
		for (int i = 0; i < size; i++)
		{
			m_grids[i] = new int[size];
		}

		if (m_debug)
		{
			StringBuilder sb = new StringBuilder();
			for (int j = 1; j < num; j++)
			{
				int row = (j - 1) / size;
				int col = (j - 1) % size;
				//fix zj
				if (map[j] == 5 || map[j] == 8)
				{
					m_grids[row][col] = 0;
				}
				else
				{
					m_grids[row][col] = 1;
				}

				sb.Append(m_grids[row][col]);

				if (col == 0)
				{
					sb.AppendLine();
				}

				//m_grids[num2][num3] = (int)(double)map[j];
			}
			Debug.Log("设置线路*****");
			Debug.Log(sb);
		}
		else
		{
			for (int j = 1; j < num; j++)
			{
				int row = (j - 1) / size;
				int col = (j - 1) % size;
				//fix zj
				if (map[j] == 5 || map[j] == 8)
				{
					m_grids[row][col] = 0;
				}
				else
				{
					m_grids[row][col] = 1;
				}
			}
		}
	}


	public string GetMapMd5()
	{
		return m_mapMd5;
	}



	public void SetMapMd5(string md5)
	{
		m_mapMd5 = md5;
	}

	
	public static List<Vector2> GetPathS(TownSearch self, Vector2 s, Vector2 e)
	{
		return self.GetPath(s, e);
	}

	
	public Vector2 GetRandPos()
	{
		int hsize = (m_size - 6) / 2;

		int x = 0;
		int y = 0;
		int index = 0;
		do
		{
			x = Random.Range(-hsize, hsize);
			y = Random.Range(-hsize, hsize);
			index = MakeGridKey(x, y,m_size);
		} while (index < 0 || index > m_map.Length - 1 || m_map[index] != 5);

		if (m_debug)
		{
//			Debug.Log("生成随机位置"+x+"  "+y);
		}
		return new Vector2(x,
			y);
	}



	public Vector3 ConvertTileToLocal(float xLogic, float yLogic,bool isTrain = false)
	{
		return new Vector3(xLogic * CITY_GRID_SIZE + CITY_GRID_SIZE_HALF, isTrain?0.1f:0.05f,
			yLogic * CITY_GRID_SIZE + CITY_GRID_SIZE_HALF);
	}

	public Vector2 ConvertLocalToTile(Vector3 pos)
	{
		return new Vector2(Mathf.Floor(pos.x * CITY_GRID_SIZE_RECIPROCAL -CITY_GRID_SIZE_HALF),
			Mathf.Floor(pos.z * CITY_GRID_SIZE_RECIPROCAL - CITY_GRID_SIZE_HALF));
	}

	public Vector2 MakeWorldPosFormLocal(float x, float y,bool isTrain = false)
	{
		var p = new Vector3(x,isTrain?0.1f:0.05f, y);
		var l = m_cityBuildingContainer.transform.TransformPoint(p);
		return new Vector2(l.x, l.z);
	}

	public bool CheckEndPointCanWalk( Vector2 e)
	{
//		int index = MakeGridKey((int)e.x, (int)e.y, m_size);
		int num = Mathf.FloorToInt((float)m_size * 0.5f);
		int[] e2 = new int[2]
		{
			Mathf.RoundToInt(e.x) + num,
			Mathf.RoundToInt(e.y) + num
		};
//		Debug.Log(index +"  "+m_map[index]+"  "+m_grids[e2[1]][e2[0]]);
		if (m_grids[e2[1]][e2[0]]==0)
		{
			return true;
		}

		return false;
	}


//	public Vector3 ConvertCityObjTileToLocal(float xLogic, float yLogic)
//	{
//		return new Vector3(xLogic * CITY_GRID_SIZE - CITY_GRID_SIZE_HALF, 0.01f,
//			yLogic * CITY_GRID_SIZE - CITY_GRID_SIZE_HALF);
//	}

	public List<Vector2> GetPath(Vector2 s, Vector2 e)
	{
		int size = m_size;
		List<Vector2> list;
		if (size <= 0)
		{
			list = new List<Vector2>();
			list.Add(s);
			list.Add(e);
			return list;
		}
		int num = Mathf.FloorToInt((float)size * 0.5f);
		int[] s2 = new int[2]
		{
			Mathf.RoundToInt(s.x) + num,
			Mathf.RoundToInt(s.y) + num
		};
		int[] e2 = new int[2]
		{
			Mathf.RoundToInt(e.x) + num,
			Mathf.RoundToInt(e.y) + num
		};
		if (m_astar == null)
		{
			m_astar = new Astar();
		}
		list = m_astar.GetPath(m_grids, s2, e2, "DiagonalFree");
		
		if (m_debug)
		{
			
			if (list.Count == 0)
			{
				Debug.Log("find path "+s.x+" ,"+s.y+" ,"+e.x+" ,"+e.y+" = "+ list.Count);
//				List<Vector2> debugList = new List<Vector2>(2);
//				debugList.Add(s);
//				debugList.Add(e);
//				Draw(m_map,null,debugList);
			}
			
		}
		for (int i = 0; i < list.Count; i++)
		{
			Vector2 value = list[i];
			value.x -= num;
			value.y -= num;
			list[i] = value;
		}
		return list;
	}

	public List<Vector2> ConverTilePathToWorldPath(List<Vector2> paths,bool isTrain = false)
	{
		List<Vector2> wpaths = new List<Vector2>(paths.Count);
		
		if (paths.Count > 0)
		{
			for (int j = 0; j < paths.Count; j++)
			{
				var startLocal = ConvertTileToLocal(paths[j].x, paths[j].y,isTrain);
				var startPos = MakeWorldPosFormLocal(startLocal.x, startLocal.z,isTrain);
				wpaths.Add(startPos);
			}
		}

		return wpaths;
	}

	public List<Vector2> GetFindWorldPaths(Vector2 s, Vector2 e,bool isTrain = false)
	{
		List<Vector2> paths = GetPath(s, e);
		return ConverTilePathToWorldPath(paths,isTrain);
	}
}
