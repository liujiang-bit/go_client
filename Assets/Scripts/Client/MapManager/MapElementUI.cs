using System;
using Client;
using UnityEngine;

/// <summary>
/// 地图上界面元素显示 /prefabs_ui_template/
/// </summary>
public class MapElementUI : MonoBehaviour
{
	public enum ElementUIType
	{
		Null,
		CastleTitle,
		CastleFight,
		Troop,
		NPCTitle,
		RssTitle,
		RssStatus,
		PassTitle,
		PassFight,
		FortressTitle,
		FortressFight,
		AsBuildingName,
		AsBuildingFight,
		AsBuildingHP,
		Village,
		Scout,
		Cave,
		CastleTitle2,
		CastleTitle3,
		CastleTitle4,
		GuildMarker,
		AsFlagName,
		AsFlagFight,
		AsFlagHP,
		NpcCastle,
		TroopDefeat,
		Vein,
		Rune,
		MapMarker,
	}

	public enum FadeType
	{
		AllFadeIn = 1,
		AllFadeOut,
		DirectIn,
		DirectOut
	}

	private float Offset_X;

	private float Offset_Y;

	private float CameraHeight;

	private float DepthScale = 0.2f;

	private Camera UICamera;

	private Camera WorldCamera;

	private RectTransform rect_transform;

	private float HalfHeight = 375f;

	private float CurScale;

	private float ConfigScale;

	private float ConfigScale3D;

	private UIZoomCurve ZoomCurve;

	private Canvas UICanvas;

	private RectTransform canvas_rect;

	private CanvasGroup canvasGroup;

	public RectTransform FightLine;

	private bool InBattle;

	private float CurSmoothRate = 1f;

	private float RadiusSmoothRate = 1f;

	private float SmoothTime = 0.33f;

	private float SmoothSpeed = 0.01f;

	private float CurLineDis;

	private bool AllFadeIn;

	private bool AllFadeOut;

	private float Dir_X;

	private float Dir_Y;

	private float Dir_Z;

	private float Old_Fight_Offset_X;

	private float Old_Fight_Offset_Y;

	private float Old_Fight_Offset_Z;

	private float WorldX;

	private float WorldY;

	private float WorldZ;

	private float TroopRadius;

	private float LastTroopRadius;

	private ElementUIType UIType;

	private bool is3D;

	public void SetIs3D(bool is3D)
	{
		this.is3D = is3D;
		if (is3D)
		{
			base.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
			base.transform.localEulerAngles = new Vector3(45f, 0f, 0f);
		}
		else
		{
			base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		}
	}

	public void Awake()
	{
		WorldCamera = Client.WorldCamera.Instance().GetCamera();
		rect_transform = GetComponent<RectTransform>();
		GameObject gameObject = GameObject.Find("UIRoot");
		UICanvas = gameObject.GetComponent<Canvas>();
		GameObject gameObject2 = gameObject.transform.Find("UICam").gameObject;
		UICamera = gameObject2.GetComponent<Camera>();
		canvas_rect = gameObject.GetComponent<RectTransform>();
		
		GameObject gameObject3 = GameObject.Find("UIRoot/Container/HUDLayer").gameObject;
		if (gameObject3 != null)
		{
			ZoomCurve = gameObject3.GetComponent<UIZoomCurve>();
		}
		
		canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 1f;
		}
		else
		{
			canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
		}
	}

	private void Start()
	{
	}

    public void ResetInitData()
    {
        CurScale = 0f;
    }

	public void InitData()
	{
		Dir_X = 0f;
		Dir_Y = 0f;
		Dir_Z = 0f;
		Old_Fight_Offset_X = 0f;
		Old_Fight_Offset_Y = 0f;
		CameraHeight = 0f;
		TroopRadius = 0f;
		LastTroopRadius = 0f;
		CurScale = 0f;
	}

	private void Update()
	{
		try
		{
			float lodDistance = Common.GetLodDistance();
			if (lodDistance != CameraHeight || Client.WorldCamera.Instance().IsSlipping() )
			{
				CameraHeight = lodDistance;
				ConfigScale = ZoomCurve.AllUIScaleCurve.Evaluate(lodDistance);
				if (is3D)	
				{
					ConfigScale3D = ZoomCurve.AllUIScaleCurve3D.Evaluate(lodDistance);
					SetScale3D();
				}
				else
				{
					SetScale();
				}
				if (!InBattle)
				{
					if (is3D)
					{
						SetPosition3D(WorldX, WorldY, WorldZ);
					}
					else
					{
						SetPosition(WorldX, WorldY, WorldZ);
					}
				}
			}
			if (CurSmoothRate < 1f)
			{
				CurSmoothRate += Time.deltaTime * (1f / SmoothTime);
				if (CurSmoothRate > 1f)
				{
					CurSmoothRate = 1f;
				}
			}
			if (RadiusSmoothRate < 1f)
			{
				RadiusSmoothRate += Time.deltaTime * (1f / SmoothTime);
				if (RadiusSmoothRate > 1f)
				{
					RadiusSmoothRate = 1f;
				}
			}
			UpdateAlpha();
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}

	private void UpdateAlpha()
	{
		if (canvasGroup == null)
		{
			return;
		}
		if (AllFadeIn)
		{
			float num = canvasGroup.alpha + Time.deltaTime * (1f / SmoothTime);
			if (num > 1f)
			{
				num = 1f;
				AllFadeIn = false;
			}
			canvasGroup.alpha = num;
		}
		if (AllFadeOut)
		{
			float num2 = canvasGroup.alpha - Time.deltaTime * (1f / SmoothTime);
			if (num2 < 0f)
			{
				num2 = 0f;
				AllFadeOut = false;
			}
			canvasGroup.alpha = num2;
		}
	}

	public void SetUIType(int ui_type)
	{
		UIType = (ElementUIType)ui_type;
	}


	public void SetScale()
	{
        Vector3 vector = UICamera.WorldToViewportPoint(base.transform.position);
        float num = DepthScale * (WorldCamera.fieldOfView / 30f);
		float num2 = (0f - (Mathf.Clamp01(vector.y) - 0.5f)) / 0.5f;
		CurScale = ConfigScale + num * num2;

		if (CurScale<0)
		{
//			CurScale = CurScale * -1;
			CurScale = 0;
//			Debug.LogWarning("MapElementUI SetScale() CurScale<0 建议排查 ConfigScale 这个的配置");
//			Debug.LogWarning("num * num2"+ num * num2 +" DepthScale" +DepthScale+" 错误 fieldOfView:"+WorldCamera.fieldOfView+" ConfigScale: "+ConfigScale+"  CurScale: "+CurScale+" height: "+Common.GetLodDistance());
		}
		base.transform.localScale = new Vector3(CurScale, CurScale, 1f);
	}

	public void SetScale3D()
	{
		CurScale = ConfigScale3D;
		base.transform.localScale = new Vector3(CurScale, CurScale, 1f);
    }

	public void SetPosition(float world_x, float world_y, float world_z)
	{
		WorldX = world_x;
		WorldY = world_y;
		WorldZ = world_z;
		if (!(ZoomCurve == null))
		{
			float num = GetOffset();
			if (UIType == ElementUIType.Troop)
			{
				num += GetNormalRadiusOffset();
				float num2 = (1f - CurSmoothRate) * ZoomCurve.TroopFightLineCurve.Evaluate(CameraHeight);
				float num3 = (0f - num2) * Dir_X;
				float num4 = (0f - num2) * Dir_Y;
				float num5 = (0f - num2) * Dir_Z;
				world_x += num3;
				world_y += num4;
				world_z += num5;
				num *= CurSmoothRate;
			}
			else if (UIType == ElementUIType.CastleFight)
			{
				float num6 = ZoomCurve.CastleFightEndOffsetCurve.Evaluate(CameraHeight) - num;
				num += (1f - CurSmoothRate) * num6;
			}
			else if (UIType == ElementUIType.PassFight)
			{
				float num7 = ZoomCurve.PassFightEndOffsetCurve.Evaluate(CameraHeight) - num;
				num += (1f - CurSmoothRate) * num7;
			}
			else if (UIType == ElementUIType.FortressFight)
			{
				float num8 = ZoomCurve.FortressFightEndOffsetCurve.Evaluate(CameraHeight) - num;
				num += (1f - CurSmoothRate) * num8;
			}
			else if (UIType == ElementUIType.AsBuildingFight)
			{
				float num9 = ZoomCurve.AsBuildingFightEndOffsetCurve.Evaluate(CameraHeight) - num;
				num += (1f - CurSmoothRate) * num9;
			}
			else if (UIType == ElementUIType.AsFlagFight)
			{
				float num10 = ZoomCurve.AsFlagFightEndOffsetCurve.Evaluate(CameraHeight) - num;
				num += (1f - CurSmoothRate) * num10;
			}
			rect_transform.anchoredPosition = WorldToUIPos(world_x, world_y, world_z, num);
			if (CurScale == 0f)
			{
				ConfigScale = ZoomCurve.AllUIScaleCurve.Evaluate(Common.GetLodDistance());
				SetScale();
			}
			if (FightLine != null && CurSmoothRate < 1f)
			{
				float num11 = (1f - CurSmoothRate) * CurLineDis;
				//SetRectTransformSizeDelta(FightLine, num11 * (1f / CurScale), 4f * (1f / CurScale));
				SetRectTransformSizeDelta(FightLine, num11, 8f);
			}
			Old_Fight_Offset_X = 0f;
			Old_Fight_Offset_Y = 0f;
		}
	}
	
	public static void SetRectTransformSizeDelta(RectTransform rt, float x, float y)
	{
		rt.sizeDelta = new Vector2(x, y);
	}

	public void SetPosition3D(float world_x, float world_y, float world_z)
	{
		WorldX = world_x;
		WorldY = world_y;
		WorldZ = world_z;
		if (!(ZoomCurve == null))
		{
			float offset = GetOffset();
			rect_transform.position = new Vector3(world_x, world_y + offset, world_z + offset);
			if (CurScale == 0f)
			{
				ConfigScale3D = ZoomCurve.AllUIScaleCurve3D.Evaluate(Common.GetLodDistance());
				SetScale3D();
			}
		}
	}

	public Vector2 WorldToUIPos(float world_x, float world_y, float world_z, float offset = 0f)
	{
		if (offset != 0f)
		{
			world_z += offset;
			world_y += offset;
			if (UIType == ElementUIType.CastleTitle || UIType == ElementUIType.CastleTitle2 || UIType == ElementUIType.CastleTitle3 || UIType == ElementUIType.CastleTitle4)
			{
				//主城标题会跳一下先注释掉  1044f
				if (CameraHeight < 3000f)
				{
					world_y -= offset;
				}
			}
			else if (UIType == ElementUIType.PassTitle)
			{
				world_y -= offset;
			}
		}

		Vector2 tmpAnchoredPos;
		Vector2 tmpScreenPos = RectTransformUtility.WorldToScreenPoint(Client.WorldCamera.Instance().GetCamera(), new Vector3(world_x, world_y, world_z));
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_rect, tmpScreenPos, UICamera, out tmpAnchoredPos);
		return tmpAnchoredPos;
//		Vector3 vector = Client.WorldCamera.Instance().GetCamera().WorldToViewportPoint(new Vector3(world_x, world_y, world_z));
//		Vector2 sizeDelta = canvas_rect.sizeDelta;
//		float x = sizeDelta.x;
//		Vector2 sizeDelta2 = canvas_rect.sizeDelta;
//		float y = sizeDelta2.y;
//		float x2 = vector.x * x;
//		float y2 = vector.y * y;
//		return new Vector2(x2, y2);
	}

	public Vector2 SetBattlePos(float dir_x, float dir_y, float dir_z, float world_x, float world_y, float world_z, float stanceOffset = 0)
	{
		if (FightLine == null || ZoomCurve == null)
		{
			return Vector2.zero;
		}
        float num = 0f;
		float num2 = 0f;
		float offset = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		if (UIType == ElementUIType.Troop)
		{
			num = ZoomCurve.TroopFightLineCurve.Evaluate(CameraHeight) + GetFightRadiusOffset();
 
            if (CurSmoothRate == 1f && (Dir_X != 0f || Dir_Y != 0f || Dir_Z != 0f))
			{
				int num6 = (int)Mathf.Ceil(dir_x * 100f);
				int num7 = (int)Mathf.Ceil(dir_y * 100f);
				int num8 = (int)Mathf.Ceil(dir_z * 100f);
				int num9 = (int)Mathf.Ceil(Dir_X * 100f);
				int num10 = (int)Mathf.Ceil(Dir_Y * 100f);
				int num11 = (int)Mathf.Ceil(Dir_Z * 100f);
				if (num6 != num9 || num7 != num10 || num8 != num11)
				{
					CurSmoothRate = 0f;
					Old_Fight_Offset_X = (0f - num) * Dir_X;
					Old_Fight_Offset_Y = (0f - num) * Dir_Y;
					Old_Fight_Offset_Z = (0f - num) * Dir_Z;
				}
			}
			if (Old_Fight_Offset_X != 0f || Old_Fight_Offset_Y != 0f || Old_Fight_Offset_Z != 0f)
			{
				num3 = (1f - CurSmoothRate) * Old_Fight_Offset_X;
				num4 = (1f - CurSmoothRate) * Old_Fight_Offset_Y;
				num5 = (1f - CurSmoothRate) * Old_Fight_Offset_Z;
			}
			else
			{
				offset = (1f - CurSmoothRate) * GetOffset();
			}
        }
		else if (UIType == ElementUIType.CastleFight)
		{
			num2 = GetOffset();
			num = ZoomCurve.CastleFightEndOffsetCurve.Evaluate(CameraHeight) - num2;
		}
		else if (UIType == ElementUIType.PassFight)
		{
			num2 = GetOffset();
			num = ZoomCurve.PassFightEndOffsetCurve.Evaluate(CameraHeight) - num2;
		}
		else if (UIType == ElementUIType.FortressFight)
		{
			num2 = GetOffset();
			num = ZoomCurve.FortressFightEndOffsetCurve.Evaluate(CameraHeight) - num2;
		}
		else if (UIType == ElementUIType.AsBuildingFight)
		{
			num2 = GetOffset();
			num = ZoomCurve.AsBuildingFightEndOffsetCurve.Evaluate(CameraHeight) - num2;
		}
		else if (UIType == ElementUIType.AsFlagFight)
		{
			num2 = GetOffset();
			num = ZoomCurve.AsFlagFightEndOffsetCurve.Evaluate(CameraHeight) - num2;
		}
        float num12 = CurSmoothRate * num + num2 + CurSmoothRate * stanceOffset;
		float num13 = (0f - num12) * dir_x + num3;
		float num14 = (0f - num12) * dir_y + num4;
		float num15 = (0f - num12) * dir_z + num5;

		Vector2 vector = WorldToUIPos(world_x + num13, world_y + num14, world_z + num15, offset);
        rect_transform.anchoredPosition = vector;
        if (dir_x != 0f || dir_y != 0f || dir_z != 0f)
		{
            if (CurScale == 0f)
			{
				ConfigScale = ZoomCurve.AllUIScaleCurve.Evaluate(Common.GetLodDistance());
				SetScale();
			}

			Vector2 vector2 = WorldToUIPos(world_x, world_y, world_z, num2);
			float angle = GetAngle(vector, vector2);
			UICommon.SetLocalEulerAngles(FightLine.gameObject, angle);
			float num16 = Vector2.Distance(vector, vector2);
            //UICommon.SetRectTransformSizeDelta(FightLine, num16 * (1f / CurScale), 3f * (1f / CurScale));
            UICommon.SetRectTransformSizeDelta(FightLine, num16, 8f);
            CurLineDis = num16;
			Dir_X = dir_x;
			Dir_Y = dir_y;
			Dir_Z = dir_z;
		}

		return vector;
	}

	public void SetUIStatus(bool in_battle, float smooth_rate)
	{
		InBattle = in_battle;
		CurSmoothRate = smooth_rate;
	}

	public void SetUIFadeShow(int fade_type)
	{
		switch (fade_type)
		{
		case 1:
			AllFadeIn = true;
			AllFadeOut = false;
			// if (canvasGroup != null && AllFadeIn == false)
			// {
			// 	canvasGroup.alpha = 0f;
				// AllFadeIn = true;
			// }
			break;
		case 2:
			AllFadeIn = false;
			AllFadeOut = true;
			break;
		case 3:
            AllFadeIn = false;
            AllFadeOut = false;
            if (canvasGroup != null)
			{
				canvasGroup.alpha = 1f;
			}
			break;
		case 4:
            AllFadeIn = false;
            AllFadeOut = false;
            if (canvasGroup != null)
			{
				canvasGroup.alpha = 0f;
			}
			break;
		}
	}

	public float GetOffset()
	{
		if (UIType == ElementUIType.CastleTitle)
		{
			return ZoomCurve.CastleTitleOffsetCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.CastleTitle2)
		{
			return ZoomCurve.CastleTitleOffsetCurve2.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.CastleTitle3)
		{
			return ZoomCurve.CastleTitleOffsetCurve3.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.CastleTitle4)
		{
			return ZoomCurve.CastleTitleOffsetCurve4.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.GuildMarker)
		{
			return ZoomCurve.GuildMarkerOffsetCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.CastleFight)
		{
			return ZoomCurve.CastleFightStartOffsetCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.Troop)
		{
			return ZoomCurve.TroopCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.NPCTitle)
		{
			return ZoomCurve.NpcTroopCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.RssTitle)
		{
			return ZoomCurve.RssTitleCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.RssStatus)
		{
			return ZoomCurve.RssStatusCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.PassTitle)
		{
			return ZoomCurve.PassTitleCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.PassFight)
		{
			return ZoomCurve.PassFightStartOffsetCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.FortressTitle)
		{
			return ZoomCurve.FortressTitleCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.FortressFight)
		{
			return ZoomCurve.FortressFightStartOffsetCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.AsBuildingName)
		{
			return ZoomCurve.AsBuildingNameCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.AsBuildingFight)
		{
			return ZoomCurve.AsBuildingFightStartOffsetCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.AsBuildingHP)
		{
			return ZoomCurve.AsBuildingHpCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.AsFlagName)
		{
			return ZoomCurve.AsFlagNameCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.AsFlagFight)
		{
			return ZoomCurve.AsFlagFightStartOffsetCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.AsFlagHP)
		{
			return ZoomCurve.AsFlagHpCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.Village)
		{
			return ZoomCurve.VillageCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.Scout)
		{
			return ZoomCurve.ScoutCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.Cave)
		{
			return ZoomCurve.CaveCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.NpcCastle)
		{
			return ZoomCurve.NpcCastleCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.TroopDefeat)
		{
			return ZoomCurve.TroopDefeatCurve.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.Vein)
		{
			return ZoomCurve.VeinCure.Evaluate(CameraHeight);
		}
		if (UIType == ElementUIType.Rune)
		{
			return ZoomCurve.CaveCurve.Evaluate(CameraHeight);
		}
		return 0f;
	}

	public float GetAngle(Vector2 pos1, Vector2 pos2)
	{
		float num;
		if (pos1.x == pos2.x)
		{
			num = 90f;
		}
		else
		{
			float num2 = Mathf.Atan(Mathf.Abs(pos1.y - pos2.y) / Mathf.Abs(pos1.x - pos2.x));
			num = 57.29578f * num2;
		}
		if (pos1.y > pos2.y)
		{
			num = ((!(pos1.x < pos2.x)) ? (num + 180f) : (0f - num));
		}
		else if (pos1.x > pos2.x)
		{
			num = 180f - num;
		}
		return num;
	}

	public void SetTroopRadius(float radius, bool play_ani)
	{
		LastTroopRadius = TroopRadius;
		TroopRadius = radius;
		if (play_ani)
		{
			RadiusSmoothRate = 0f;
		}
		else
		{
			RadiusSmoothRate = 1f;
		}
	}

	private float GetNormalRadiusOffset()
	{
		float num = ZoomCurve.TroopRadiusCure.Evaluate(LastTroopRadius);
		float num2 = ZoomCurve.TroopRadiusCure.Evaluate(TroopRadius);
		return num + (num2 - num) * RadiusSmoothRate;
	}

	private float GetFightRadiusOffset()
	{
		float num = ZoomCurve.TroopFightRadiusCure.Evaluate(LastTroopRadius);
		float num2 = ZoomCurve.TroopFightRadiusCure.Evaluate(TroopRadius);
		return num + (num2 - num) * RadiusSmoothRate;
	}

	#region 方便美术调整曲线
#if UNITY_EDITOR

	private float GetCurveValue()
	{
		return ConfigScale;
	}
	private float GetDepthValue()
	{
		Vector3 vector = UICamera.WorldToViewportPoint(base.transform.position);
		float num = DepthScale * (WorldCamera.fieldOfView / 30f);
		float num2 = (0f - (Mathf.Clamp01(vector.y) - 0.5f)) / 0.5f;
		return num * num2;
	}
	private float GetCurValue()
	{
		return CurScale;
	}
	static public void drawString(string text, Vector3 worldPos, Color? colour = null) {
		UnityEditor.Handles.BeginGUI();
 
		var restoreColor = GUI.color;
 
		if (colour.HasValue) GUI.color = colour.Value;
		var view = UnityEditor.SceneView.currentDrawingSceneView;
		Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
 
		if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
		{
			GUI.color = restoreColor;
			UnityEditor.Handles.EndGUI();
			return;
		}
 
		Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
		GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
		GUI.color = restoreColor;
		UnityEditor.Handles.EndGUI();
	}
	private void OnDrawGizmos()
	{
		if (canvasGroup!=null&&canvasGroup.alpha >= 1)
		{
			var text = "Scale:" + GetCurValue().ToString("0.000") + "|Curve:" + GetCurveValue().ToString("0.000") + "|Depth:" + GetDepthValue().ToString("0.000");
			drawString(text,transform.position,Color.blue);
		}
	}
#endif

	#endregion
}
