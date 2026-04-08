using System;
using System.Collections.Generic;
using Client;
using Skyunion;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 挂载rebel_1  1.反叛军上  2.常驻居民citizen  archery_patrol_t   spec_troop_city_patrol_
/// </summary>
public class People : MonoBehaviour
{
    public enum ENMU_CITIZEN_STAT
    {
        IDLE,
        MOVE,
        FIGHT,
        RUN,
        WORK,
        CARRY,
        BUILD,
        FIREFIGHTING,
        HIDE,
        DEAD,
        NUMBER
    }

    public enum ENMU_CARRY_RESOURCE_TYPE
    {
        FOOD,
        WOOD,
        STONE,
        GOLD,
        BUCKET,
        NONE,
        NUM
    }
    
    //路径
    public List<Vector2> WorldPaths = new List<Vector2>(2);
    

    private Vector2 m_start_pos = Vector2.zero;

    private Vector2 m_target_pos = Vector2.zero;

    private float m_move_timer;

    private float m_move_time;

    private float m_move_speed = 1f;

    private bool m_has_footprints = true;

    private bool m_citizen_active = true;

    private CellClone m_unit_dummy;

    private CellBase m_unit;

    public GameObject[] m_resource_icon;

    private float carry_resoruce_forward_offset = 0.03f;

    private float carry_resoruce_up_offset = 0.088f;

    public GameObject m_footprint_gameobject;

    public ENMU_CITIZEN_STAT m_state;


    public bool IsStop()
    {
        return m_move_time <= 0f;
    }

    public static void InitCitizenS(People self, GameObject unit_path, Color unit_color)
    {
        self.InitCitizen(unit_path, unit_color);
    }

    public static void SetUnitActiveS(People self, bool active)
    {
        self.SetUnitActive(active);
    }

    private void SetUnitActive(bool active)
    {
        m_citizen_active = active;
        m_unit.gameObject.SetActive(active);
        if (m_has_footprints)
        {
            m_footprint_gameobject.SetActive(active);
        }

        if (!active)
        {
            for (int i = 0; i != m_resource_icon.Length; i++)
            {
                m_resource_icon[i].SetActive(value: false);
            }
        }
    }

    /// <summary>
    /// 设置是否有脚印
    /// </summary>
    /// <param name="self"></param>
    /// <param name="active"></param>
    public static void SetUnitFootprintsActiveS(People self, bool active)
    {
        if (self != null)
        {
            self.SetUnitFootprintsActive(active);
        }
    }

    private void SetUnitFootprintsActive(bool active)
    {
        m_has_footprints = active;
        if (m_footprint_gameobject != null)
        {
            m_footprint_gameobject.SetActive(active);
        }
    }

    /// <summary>
    /// 城墙上 archery_infantry_t1  士兵等级大于100的时候   spec_troop_city_patrol_ 弓箭手
    /// 城内 resident_t  resident_fm_t
    /// </summary>
    /// <param name="unit_path"></param>
    /// <param name="unit_color"></param>
    private void InitCitizen(GameObject gameObject, Color unit_color)
    {
        CellBase component = gameObject.GetComponent<CellBase>();
        component.GetComponent<SpriteRenderer>().color = unit_color;
        gameObject.GetComponent<AnimationBase>().FadeIn();
        m_unit_dummy = base.transform.GetComponent<CellClone>();
        m_unit_dummy.m_unit = component;
        m_unit = component;
        component.InitDummy(m_unit_dummy);
        component.SetSpriteLoigicalState(AnimationBase.ENMU_SPRITE_STATE.IDLE);
        component.ChangeMoveState(CellBase.MOVE_STATE.STATIC);
        component.unitType = 0;
    }

    public static void SetStateS(People self, ENMU_CITIZEN_STAT state, Vector2 current_pos, Vector2 target_pos,
        float move_speed = 2f, ENMU_CARRY_RESOURCE_TYPE carry_resource_type = ENMU_CARRY_RESOURCE_TYPE.NONE)
    {
        self.SetState(state, current_pos, target_pos, move_speed, carry_resource_type);
    }

    private void SetState(ENMU_CITIZEN_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f,
        ENMU_CARRY_RESOURCE_TYPE carry_resource_type = ENMU_CARRY_RESOURCE_TYPE.NONE)
    {
        SetUnitState(state, current_pos, target_pos, move_speed);
        Transform transform = base.transform;
        float x = current_pos.x;
        Vector3 position = base.transform.position;
        transform.position = new Vector3(x, position.y, current_pos.y);
        m_start_pos = current_pos;
        m_target_pos = target_pos;
        Vector2 normalized = (m_target_pos - m_start_pos).normalized;
        if (state != 0 || !(current_pos == target_pos))
        {
            Transform transform2 = base.transform;
            float x2 = normalized.x;
            Vector3 position2 = base.transform.position;
            transform2.forward = new Vector3(x2, position2.y, normalized.y);
        }

        if (state == ENMU_CITIZEN_STAT.MOVE || state == ENMU_CITIZEN_STAT.CARRY || state == ENMU_CITIZEN_STAT.RUN)
        {
            m_move_timer = 0f;
            if (move_speed == 0f)
            {
                Debug.LogWarning("Square : invalid move_speed");
                move_speed = 1f;
            }

            m_move_speed = move_speed;
            m_move_time = (m_target_pos - m_start_pos).magnitude / m_move_speed;
        }

        m_state = state;
        for (int i = 0; i != m_resource_icon.Length; i++)
        {
            if (i == (int) carry_resource_type)
            {
                m_resource_icon[i].SetActive(m_citizen_active);
                m_resource_icon[i].transform.position = base.transform.position +
                                                        base.transform.forward * carry_resoruce_forward_offset +
                                                        base.transform.up * carry_resoruce_up_offset;
                m_resource_icon[i].transform.eulerAngles = new Vector3(45f, 0f, 0f);
            }
            else
            {
                m_resource_icon[i].SetActive(value: false);
            }
        }

        switch (state)
        {
            case ENMU_CITIZEN_STAT.FIREFIGHTING:
                m_unit.gameObject.GetComponent<AnimationBase>().SetParticleParams("build_3012", 7, 3);
                return;
            case ENMU_CITIZEN_STAT.BUILD:
                m_unit.gameObject.GetComponent<AnimationBase>().SetParticleParams("build_3010", 6, 2);
                return;
        }

        if (m_unit != null)
        {
            m_unit.gameObject.GetComponent<AnimationBase>().SetParticleParams(string.Empty, -1, -1);
        }
    }

    public static void ResetUnitPosS(People self)
    {
        self.ResetUnitPos();
    }

    public void ResetUnitPos()
    {
        Vector3 position = base.gameObject.transform.position;
        float x = position.x;
        Vector3 position2 = base.gameObject.transform.position;
        m_start_pos = new Vector2(x, position2.z);
        m_move_time = (m_target_pos - m_start_pos).magnitude / m_move_speed;
        m_move_timer = 0f;
        m_unit.ChangeMoveState(CellBase.MOVE_STATE.STATIC);
    }

    private void SetUnitState(ENMU_CITIZEN_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
    {
        Vector2 zero = Vector2.zero;
        switch (state)
        {
            case ENMU_CITIZEN_STAT.IDLE:
                m_unit.SetMoveSpeed(m_move_speed);
                m_unit.ChangeMoveState(CellBase.MOVE_STATE.CHASE);
                break;
            case ENMU_CITIZEN_STAT.MOVE:
            case ENMU_CITIZEN_STAT.RUN:
            case ENMU_CITIZEN_STAT.CARRY:
                if (move_speed == 0f)
                {
                    Debug.LogWarning("Square : invalid move_speed");
                    move_speed = 1f;
                }

                m_move_speed = move_speed;
                m_unit.SetMoveSpeed(m_move_speed);
                m_unit.ChangeMoveState(CellBase.MOVE_STATE.CHASE);
                break;
            case ENMU_CITIZEN_STAT.DEAD:
                m_unit.ChangeMoveState(CellBase.MOVE_STATE.UNBOUND);
                if (m_unit.isActiveAndEnabled)
                {
                    m_unit.PlayDeadParticle();
                }

                CoreUtils.assetService.Destroy(base.gameObject);
                break;
            default:
                m_unit.ChangeMoveState(CellBase.MOVE_STATE.CHASE);
                break;
        }

        if (m_unit != null)
        {
            m_unit.SetSpriteLoigicalState(GetUnitSpriteState(state));
        }
    }

    private AnimationBase.ENMU_SPRITE_STATE GetUnitSpriteState(ENMU_CITIZEN_STAT state)
    {
        switch (state)
        {
            case ENMU_CITIZEN_STAT.IDLE:
                return AnimationBase.ENMU_SPRITE_STATE.IDLE;
            case ENMU_CITIZEN_STAT.MOVE:
                return AnimationBase.ENMU_SPRITE_STATE.MOVE;
            case ENMU_CITIZEN_STAT.FIGHT:
                return AnimationBase.ENMU_SPRITE_STATE.FIGHT;
            case ENMU_CITIZEN_STAT.RUN:
                return AnimationBase.ENMU_SPRITE_STATE.RUN;
            case ENMU_CITIZEN_STAT.WORK:
                return AnimationBase.ENMU_SPRITE_STATE.WORK;
            case ENMU_CITIZEN_STAT.CARRY:
                return AnimationBase.ENMU_SPRITE_STATE.CARRY;
            case ENMU_CITIZEN_STAT.BUILD:
                return AnimationBase.ENMU_SPRITE_STATE.BUILD;
            case ENMU_CITIZEN_STAT.FIREFIGHTING:
                return AnimationBase.ENMU_SPRITE_STATE.FIREFIGHTING;
            case ENMU_CITIZEN_STAT.HIDE:
                return AnimationBase.ENMU_SPRITE_STATE.HIDE;
            default:
                return AnimationBase.ENMU_SPRITE_STATE.IDLE;
        }
    }

    private void OnDespawn()
    {
        if (m_unit != null)
        {
            CoreUtils.assetService.Destroy(m_unit.gameObject);
        }

        m_unit = null;
        m_unit_dummy = null;
    }

    public static void FadeOutS(People self)
    {
        if (self != null)
        {
            self.FadeOut();
        }
    }

    public void FadeOut()
    {
        if (m_unit.gameObject.GetComponent<AnimationBase>().Fadeout())
        {
            base.transform.SetParent(null, worldPositionStays: true);
            CoreUtils.assetService.Destroy(base.gameObject, 0.35f);
        }
        else
        {
            CoreUtils.assetService.Destroy(base.gameObject);
        }
    }

    private void Update()
    {
        try
        {
            if ((m_state == ENMU_CITIZEN_STAT.MOVE || m_state == ENMU_CITIZEN_STAT.CARRY ||
                 m_state == ENMU_CITIZEN_STAT.RUN) && m_move_time != 0f)
            {
                float num = m_move_timer / m_move_time;
                if (num >= 1f)
                {
                    Transform transform = base.transform;
                    float x = m_target_pos.x;
                    Vector3 position = base.transform.position;
                    transform.position = new Vector3(x, position.y, m_target_pos.y);
                    m_move_time = 0f;
                }
                else
                {
                    Vector2 vector = Vector2.Lerp(m_start_pos, m_target_pos, num);
                    Transform transform2 = base.transform;
                    float x2 = vector.x;
                    Vector3 position2 = base.transform.position;
                    transform2.position = new Vector3(x2, position2.y, vector.y);
                    m_move_timer += Time.deltaTime;
                }
            }
        }
        catch (Exception e)
        {
//			Unity3dUtils.PlatformDebugExceptionLog(e);
        }
    }
}