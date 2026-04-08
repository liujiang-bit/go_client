
using UnityEngine;

/// <summary>
/// 挂载在城市建筑的UI界面上
/// </summary>
public class SpriteUI : MonoBehaviour
{
	public void SetLayer(string layer)
	{
		int layer2 = LayerMask.NameToLayer(layer);
		base.gameObject.layer = layer2;
		GameObject gameObject = base.gameObject.transform.Find("sprite").gameObject;
		if (!(gameObject == null))
		{
			gameObject.layer = layer2;
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
				gameObject2.layer = layer2;
			}
		}
	}

	public void SetScale(float scale)
	{
		base.gameObject.transform.localScale = new Vector3(scale, scale, 1f);
	}

	public void SetOrderLayer(int layer)
	{
		GameObject gameObject = base.gameObject.transform.Find("sprite").gameObject;
		if (!(gameObject == null))
		{
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer + 1;
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
				gameObject2.GetComponent<SpriteRenderer>().sortingOrder = layer + 2;
			}
		}
	}

	/// <summary>
	/// 替换材质球  可用不可能
	/// </summary>
	/// <param name="isNormal"></param>
	/// <param name="inMask"></param>
	public void SetMaterialsNormal(bool isNormal, bool inMask)
	{
		GameObject gameObject = base.gameObject.transform.Find("sprite").gameObject;
		if (gameObject == null)
		{
			return;
		}
		string asset_name = "city_building_texture_normal";
		string asset_name2 = "city_building_texture_normal_no_mask";
		if (!isNormal)
		{
			asset_name = "city_building_texture_gray";
			asset_name2 = "city_building_texture_gray_no_mask";
		}
        Material sharedMaterial = null;// ResourceMgr.GetInstance().LoadByType(asset_name, typeof(Material)) as Material;
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		component.sharedMaterial = sharedMaterial;
		component.maskInteraction = (inMask ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.None);
		if (gameObject.transform.childCount <= 0)
		{
			return;
		}
	//	Material sharedMaterial2 = ResourceMgr.GetInstance().LoadByType(asset_name2, typeof(Material)) as Material;
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			Transform child = gameObject.transform.GetChild(i);
			if (child.gameObject.GetComponent<MaskSpriteOld>() != null)
			{
				child.GetComponent<SpriteRenderer>().sharedMaterial = sharedMaterial;
				continue;
			}
		//	child.GetComponent<SpriteRenderer>().sharedMaterial = sharedMaterial2;
			child.GetComponent<SpriteRenderer>().maskInteraction = (inMask ? SpriteMaskInteraction.VisibleInsideMask : SpriteMaskInteraction.None);
		}
	}
}
