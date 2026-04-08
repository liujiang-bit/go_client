using UnityEngine;

public static class LinePolygon
{
	public static Mesh CreateMesh(Vector2[] points, float width, float uvStep = 0f)
	{
		Vector2[] uvs = new Vector2[points.Length << 1];
		Mesh mesh = new Mesh();
		mesh.vertices = CreateLineVerticies(points, width, uvStep, ref uvs);
		mesh.triangles = CreateTriangles(points.Length);
		mesh.uv = uvs;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		return mesh;
	}

	public static void CreateMesh(Mesh mesh, Vector2[] points, float width, Color c, float uvStep = 0f)
	{
		mesh.Clear();
		Vector2[] uvs = new Vector2[points.Length << 1];
		mesh.vertices = CreateLineVerticies(points, width, uvStep, ref uvs);
		mesh.triangles = CreateTriangles(points.Length);
		mesh.uv = uvs;
		Color[] array = new Color[mesh.vertices.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = c;
		}
		mesh.colors = array;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	public static Vector3[] ConvertVector2ToVector3(Vector2[] points)
	{
		int num = points.Length;
		Vector3[] array = new Vector3[num];
		for (int i = 0; i < num; i++)
		{
			Vector2 vector = points[i];
			array[i] = new Vector3(vector.x, 0f, vector.y);
		}
		return array;
	}

	public static Vector3[] CreateLineVerticies(Vector2[] points, float width, float uvStep, ref Vector2[] uvs)
	{
		if (points.Length < 2)
		{
			return null;
		}
		bool flag = uvStep > 0f;
		int num = points.Length << 1;
		Vector3[] array = new Vector3[num];
		float num2 = 0f;
		float f = Mathf.Atan2(points[1].y - points[0].y, points[1].x - points[0].x);
		float num3 = Mathf.Sin(f);
		float num4 = Mathf.Cos(f);
		CalculateUpDownPoints(points[0], num3, num4, width, out Vector3 p, out Vector3 p2);
		array[0] = p;
		array[1] = p2;
		if (flag)
		{
			num2 = ((points[0].x != points[1].x) ? (points[0].x - Mathf.Floor(points[0].x / uvStep) * uvStep) : (points[0].y - Mathf.Floor(points[0].y / uvStep) * uvStep));
			float x = num2 / uvStep;
			uvs[0] = new Vector2(x, 0f);
			uvs[1] = new Vector2(x, 1f);
		}
		else
		{
			uvs[0] = new Vector2(0.1f, 0f);
			uvs[1] = new Vector2(0.1f, 1f);
		}
		Vector3 p3;
		Vector3 p4;
		float f2;
		float num5;
		float num6;
		for (int i = 1; i < points.Length - 1; i++)
		{
			f2 = Mathf.Atan2(points[i + 1].y - points[i].y, points[i + 1].x - points[i].x);
			num5 = Mathf.Sin(f2);
			num6 = Mathf.Cos(f2);
			CalculateUpDownPoints(points[i], num5, num6, width, out p3, out p4);
			CalculateMidiumPoint(p, p2, p3, p4, num3, num4, num5, num6, out Vector3 p5, out Vector3 p6);
			int num7 = i << 1;
			array[num7] = p5;
			array[num7 + 1] = p6;
			if (flag)
			{
				num2 += Mathf.Abs(points[i].x - points[i - 1].x) + Mathf.Abs(points[i].y - points[i - 1].y);
				float x2 = num2 / uvStep;
				uvs[num7] = new Vector2(x2, 0f);
				uvs[num7 + 1] = new Vector2(x2, 1f);
			}
			else
			{
				uvs[num7] = new Vector2(0.1f, 0f);
				uvs[num7 + 1] = new Vector2(0.1f, 1f);
			}
			num3 = num5;
			num4 = num6;
			p = p3;
			p2 = p4;
		}
		int num8 = points.Length;
		f2 = Mathf.Atan2(points[num8 - 1].y - points[num8 - 2].y, points[num8 - 1].x - points[num8 - 2].x);
		num5 = Mathf.Sin(f2);
		num6 = Mathf.Cos(f2);
		CalculateUpDownPoints(points[num8 - 1], num5, num6, width, out p3, out p4);
		array[num - 2] = p3;
		array[num - 1] = p4;
		if (flag)
		{
			num2 += Mathf.Abs(points[points.Length - 1].x - points[points.Length - 2].x) + Mathf.Abs(points[points.Length - 1].y - points[points.Length - 2].y);
			float x3 = num2 / uvStep;
			uvs[num - 2] = new Vector2(x3, 0f);
			uvs[num - 1] = new Vector2(x3, 1f);
		}
		else
		{
			uvs[num - 2] = new Vector2(0.1f, 0f);
			uvs[num - 1] = new Vector2(0.1f, 1f);
		}
		return array;
	}

	public static int[] CreateTriangles(int point_count)
	{
		int[] array = new int[(point_count - 1) * 6];
		for (int i = 0; i < point_count - 1; i++)
		{
			int num = i * 6;
			int num2 = array[num] = i << 1;
			array[num + 1] = num2 + 1;
			array[num + 2] = num2 + 2;
			array[num + 3] = num2 + 2;
			array[num + 4] = num2 + 1;
			array[num + 5] = num2 + 3;
		}
		return array;
	}

	public static void CalculateUpDownPoints(Vector2 p, float sin_theta, float cos_theta, float width, out Vector3 p1, out Vector3 p2)
	{
		float num = width * cos_theta;
		float num2 = width * sin_theta;
		Vector3 vector = new Vector3(p.x, 0f, p.y);
		Vector3 vector2 = new Vector3(p.x - 2f * num2, 0f, p.y + 2f * num);
		p1 = vector;
		p2 = vector2;
	}

	public static void CalculateMidiumPoint(Vector3 p1_updown_0, Vector3 p1_updown_1, Vector3 p2_updown_0, Vector3 p2_updown_1, float sin_theta1, float cos_theta1, float sin_theta2, float cos_theta2, out Vector3 p1, out Vector3 p2)
	{
		float num = (0f - sin_theta1) * cos_theta2 + sin_theta2 * cos_theta1;
		if (num == 0f)
		{
			p1 = (p1_updown_0 + p2_updown_0) * 0.5f;
			p2 = (p1_updown_1 + p2_updown_1) * 0.5f;
		}
		Vector3 vector = p1_updown_0;
		Vector3 vector2 = p1_updown_1;
		Vector3 vector3 = p2_updown_0;
		Vector3 vector4 = p2_updown_1;
		float num2 = (vector.x * sin_theta1 - vector.z * cos_theta1) * cos_theta2 - (vector3.x * sin_theta2 - vector3.z * cos_theta2) * cos_theta1;
		float num3 = (vector.x * sin_theta1 - vector.z * cos_theta1) * (0f - sin_theta2) + (vector3.x * sin_theta2 - vector3.z * cos_theta2) * sin_theta1;
		float num4 = (vector2.x * sin_theta1 - vector2.z * cos_theta1) * cos_theta2 - (vector4.x * sin_theta2 - vector4.z * cos_theta2) * cos_theta1;
		float num5 = (vector2.x * sin_theta1 - vector2.z * cos_theta1) * (0f - sin_theta2) + (vector4.x * sin_theta2 - vector4.z * cos_theta2) * sin_theta1;
		p1 = new Vector3(num2 / (0f - num), 0f, num3 / num);
		p2 = new Vector3(num4 / (0f - num), 0f, num5 / num);
	}
}
