using Client;
using UnityEngine;
/// <summary>
/// 领地线条Lod
/// </summary>
public class LodMonorLine : LevelDetailBase
{
	public AnimationCurve m_scale_curve;

	public float m_tile_factor;

	public Material m_activeMaterial;

	public Material m_inActiveMaterial;

	public Material m_disbleMaterial;

	public MeshFilter m_meshFilter;

	public MeshRenderer m_meshRenderer;

	private bool m_inited;

	private Vector2[] m_posArray;

	private Color m_color;

	private float m_uvStep;

	private float m_width;

	private float m_smoothDistance;

	private float m_lodWidth;

	private float m_oldLodWidth = -1f;

	private Material m_usingMaterial;

	private Vector2[] m_smoothPosArray;

	private bool m_canUpdate = true;

	private string typeName = "";

	public override void UpdateLod()
	{
		if (m_inited)
		{
			UpdateWidth();
			base.UpdateLod();
		}
	}

	public void Init(Vector2[] pos_array, Color color, float uvStep, float width, float smooth_distance,string typename)
	{
		m_inited = true;
		m_color = color;
		m_uvStep = uvStep;
		m_width = width;
		m_smoothDistance = smooth_distance;
		m_posArray = pos_array;
		typeName = typename;
		UpdateWidth();
	}

	public void UpdateWidth()
	{
		float lodDistance = Common.GetLodDistance();
		m_lodWidth = Mathf.Floor(m_scale_curve.Evaluate(lodDistance));
		if (!m_canUpdate || m_lodWidth == m_oldLodWidth)
		{
			return;
		}
		m_oldLodWidth = m_lodWidth;
		float num = m_width * m_lodWidth;
		float num2 = 0f;
		if (m_uvStep > 0f)
		{
			num2 = m_uvStep * m_lodWidth * m_tile_factor;
			if (num2 > 4f)
			{
				num2 = 0f;
			}
		}
		Vector2[] points;
		if (num2 <= 0f && num >= 2f)
		{
			points = m_posArray;
		}
		else
		{
			if (m_smoothPosArray == null)
			{
				m_smoothPosArray = Common.SmoothLine(m_posArray, m_smoothDistance, 2);
			}
			points = m_smoothPosArray;
		}
		Material material = !(num2 <= 0f) ? m_inActiveMaterial : m_activeMaterial;

		if (m_color== Color.white && !(num2 <= 0f))
		{
			material = m_disbleMaterial;
		}

		if (typeName == "inactive" && (lodDistance>3000 || lodDistance<250))
		{
			num2 = 0;
			num = 0;
		}
		
		//Debug.Log(this.typeName+"  lod  "+lodDistance+"  num2:"+num2+"  "+num+"  "+m_color);
		if (m_usingMaterial != material)
		{
			m_usingMaterial = material;
			m_meshRenderer.sharedMaterial = material;
		}
		LinePolygon.CreateMesh(m_meshFilter.mesh, points, num, m_color, num2);
	}

	public void SetCanUpdate(bool canUpdate)
	{
		m_canUpdate = canUpdate;
	}
}
