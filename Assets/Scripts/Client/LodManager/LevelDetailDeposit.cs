using System;
using Client;
using UnityEngine;

public class LevelDetailDeposit : LevelDetailCastle
{
	public float m_fade_in_time = 2f;

	private float m_fade_in_timer;

	private bool m_is_fade_in;

	public GameObject m_ground_object;

	public void FadeIn()
	{
		m_is_fade_in = true;
	}

	private void Update()
	{
		try
		{
			if (m_is_fade_in)
			{
				m_fade_in_timer += Time.deltaTime;
				if (m_fade_in_timer >= m_fade_in_time)
				{
					m_fade_in_timer = 0f;
					m_is_fade_in = false;
					m_tile_collide.SetTargetAlpha(0f);
				}
				else
				{
					float targetAlpha = 1f - m_fade_in_timer / m_fade_in_time;
					m_tile_collide.SetTargetAlpha(targetAlpha);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e);
		}
	}

	private new void OnSpawn()
	{
		UpdateGroundObject();
		base.OnSpawn();
	}

	private new void OnDespawn()
	{
		m_tile_collide.SetTargetAlpha(1f);
		m_fade_in_timer = 0f;
		m_is_fade_in = false;
		base.OnDespawn();
	}

	public override void UpdateLod()
	{
		if (IsLodChanged())
		{
			UpdateGroundObject();
		}
		base.UpdateLod();
	}

	private void UpdateGroundObject()
	{
		if (m_ground_object != null)
		{
			switch (GetCurrentLodLevel())
			{
			case 0:
				m_ground_object.SetActive(value: true);
				break;
			case 1:
				m_ground_object.SetActive(value: false);
				break;
			}
		}
	}
}
