using Client;
using UnityEngine;

[ExecuteInEditMode]
public class GridCollideTest : MonoBehaviour
{
	public float scale = 1f;

	private void Update()
	{
		GetComponent<GridCollideMgr>().SetScale(scale);
	}
}
