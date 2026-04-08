
using System;
using UnityEngine;

/// <summary>
/// 挂载在CityBuildingContainer
/// </summary>
public class TownBuildingContainer : MonoBehaviour
{

	public AnimationCurve m_age_change_curve;
	
	private Action<bool> m_applicationFocus;

	private float GetChangeBuildingNum(float wave)
	{
		return m_age_change_curve.Evaluate(wave);
	}

	public static float GetChangeBuildingNumS(TownBuildingContainer self, float wave)
	{
		if (self != null)
		{
			return self.GetChangeBuildingNum(wave);
		}
		return 0f;
	}

	public void SetApplicationFocus(Action<bool> func)
	{
		m_applicationFocus = func;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (m_applicationFocus!=null)
		{
			m_applicationFocus(hasFocus);
		}
	}

}
