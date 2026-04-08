using Skyunion;
using System;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(AnimationBase))]
    public class Cell : CellBase
    {
        public float[] m_graph_mid_lod_array;

        public float[] m_graph_high_lod_array;

        public CellClone m_dummy;

        private AnimationBase m_sprite;

        public GameObject m_shadow_sprite;

        public Transform m_shadow_lod1;

        public float m_chase_move_speed = 1.5f;

        private float m_chase_move_speed_real = 1f;

        private CellBase.CHASE_MODE m_chase_mode = CellBase.CHASE_MODE.ORIGIN_SPRITE;

        public float m_real_chase_move_speed_lower_limit = 1f;

        public float m_chase_rot_speed = 20f;

        private Vector2 m_current_unit_direction = Vector2.zero;

        private Vector2 m_current_sprite_direction = Vector2.zero;

        private float m_delay_chase_time;

        private float m_delay_chase_timer;

        private float m_delay_chase_speed = 1f;

        private Vector2 m_delay_chase_direction = Vector2.zero;

        private Vector3 m_euler = new Vector3(45f, 0f, 0f);

        private float m_alignment_threshold = 0.2f;

        private GameObject m_levelup_effect;

        public string m_die_particle = string.Empty;

        private float chase_direction_frame;

        private float chase_direction;

        private CellBase.MOVE_STATE m_move_state;

        private AnimationBase.ENMU_SPRITE_STATE m_cur_sprite_logical_state = AnimationBase.ENMU_SPRITE_STATE.MOVE;

        private bool m_isMoveAtk;

        private bool m_waitStatic;

        private bool m_isRotating;

        private static bool s_isPutDebugLog;

        private bool is_great_many_squares;

        private bool is_need_swicth_shadow;

        private float m_spriteAniOnceTime;

        public AnimationBase sprite
        {
            get
            {
                return this.m_sprite;
            }
        }

        private void Awake()
        {
            this.m_sprite = base.GetComponent<AnimationBase>();
            if (CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.MEDIUM)
            {
                this.m_lod_array = this.m_graph_mid_lod_array;
            }
            else if (CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.HIGH)
            {
                this.m_lod_array = this.m_graph_high_lod_array;
            }
        }

        public override void SetDummy(CellClone dummy)
        {
            this.m_dummy = dummy;
        }

        public override void InitDummy(CellClone dummy)
        {
            this.m_dummy = dummy;
            base.transform.parent = this.m_dummy.transform;
            base.transform.localPosition = Vector3.zero;
            base.transform.eulerAngles = this.m_euler;
            this.m_move_state = CellBase.MOVE_STATE.STATIC;
        }

        public override void SetMoveSpeedForce(float moveSpeed)
        {
            this.m_chase_move_speed_real = moveSpeed;
        }

        public override void SetMoveSpeed(float moveSpeed)
        {
            this.m_chase_move_speed_real = this.m_chase_move_speed * moveSpeed;
            if (this.m_chase_move_speed_real <= this.m_real_chase_move_speed_lower_limit)
            {
                this.m_chase_move_speed_real = this.m_real_chase_move_speed_lower_limit;
            }
        }

        public override void ChangeMoveState(CellBase.MOVE_STATE state, bool isMoveAtk = false, float delay_chase_time = 0f, float delay_chase_speed = 1f)
        {
            if (this.m_move_state == CellBase.MOVE_STATE.UNBOUND)
            {
                return;
            }
            switch (state)
            {
                case CellBase.MOVE_STATE.STATIC:
                    {
                        base.transform.parent = this.m_dummy.transform;
                        base.transform.localPosition = Vector3.zero;
                        base.transform.eulerAngles = this.m_euler;
                        this.m_current_unit_direction = new Vector2(this.m_dummy.transform.forward.x, this.m_dummy.transform.forward.z);
                        this.m_current_sprite_direction = this.m_current_unit_direction;
                        float angle = Common.GetAngle360(Vector3.forward, this.m_dummy.transform.forward);
                        this.m_sprite.SetDirection(angle, false);
                        this.m_waitStatic = false;
                        if (this.m_isMoveAtk)
                        {
                            this.m_waitStatic = true;
                            this.m_move_state = CellBase.MOVE_STATE.CHASE;
                            this.SetSpriteState(AnimationBase.ENMU_SPRITE_STATE.MOVE);
                        }
                        else
                        {
                            this.m_move_state = CellBase.MOVE_STATE.STATIC;
                            this.SetSpriteState(this.m_cur_sprite_logical_state);
                        }
                        break;
                    }
                case CellBase.MOVE_STATE.CHASE:
                    this.m_isMoveAtk = isMoveAtk;
                    this.m_move_state = CellBase.MOVE_STATE.CHASE;
                    if (this.m_dummy.isActiveAndEnabled)
                    {
                        base.transform.parent = null;
                    }
                    this.SetSpriteState(AnimationBase.ENMU_SPRITE_STATE.MOVE);
                    if (this.m_chase_mode == CellBase.CHASE_MODE.ORIGIN_SPRITE)
                    {
                        this.m_isRotating = true;
                    }
                    break;
                case CellBase.MOVE_STATE.UNBOUND:
                    this.m_move_state = CellBase.MOVE_STATE.UNBOUND;
                    if (this.m_dummy.isActiveAndEnabled)
                    {
                        base.transform.parent = null;
                    }
                    this.SetSpriteState(this.m_cur_sprite_logical_state);
                    break;
                default:
                    if (this.m_waitStatic)
                    {
                        this.m_isMoveAtk = false;
                        this.ChangeMoveState(CellBase.MOVE_STATE.STATIC, false, 0f, 1f);
                    }
                    break;
            }
        }

        private void SetSpriteState(AnimationBase.ENMU_SPRITE_STATE state)
        {
            if (this.m_spriteAniOnceTime <= 0f)
            {
                int start_frame = UnityEngine.Random.Range(0, this.m_sprite.GetFrameCount((int)state));
                this.m_sprite.SetState((int)state, start_frame, true);
            }
        }

        public override void SetSpriteLoigicalState(AnimationBase.ENMU_SPRITE_STATE state)
        {
            this.m_cur_sprite_logical_state = state;
        }

        private bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {

            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parrallel
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }

        private void ChaseModeRotateSprite_()
        {
            bool flag1 = false;
            bool flag2 = false;

            Vector3 intersection = Vector3.zero;
            LineLineIntersection(out intersection,
                                 base.transform.position,
                                 new Vector3(this.m_current_unit_direction.x, 0f, this.m_current_unit_direction.y),
                                 this.m_dummy.transform.position,
                                 this.m_dummy.transform.forward);

            float currentDis = (intersection - base.transform.position).magnitude;
            float targetDis = (intersection - this.m_dummy.transform.position).magnitude;
            float tempDis = Mathf.Lerp(currentDis, targetDis, 1 * Time.unscaledDeltaTime);
            if (currentDis - targetDis < 0.5f)
            {
                tempDis = targetDis;
                flag1 = true;
            }

            Vector3 currentDir = base.transform.position - intersection;
            Vector3 targetDir = this.m_dummy.transform.position - intersection;
            Vector3 tempDir = Vector3.RotateTowards(currentDir, targetDir, 1 * Time.unscaledDeltaTime, 0.0F);
            if (Vector3.Angle(currentDir, targetDir) < 0.5f)
            {
                tempDir = targetDir;
                flag2 = true;
            }
            
            Vector3 tempPos = intersection + (tempDir.normalized * tempDis);
            Vector3 translation = tempPos - base.transform.position;
            Vector3 myDir = (intersection - tempPos).normalized;
            if (flag1 && flag2)
            {
                translation = this.m_dummy.transform.position - base.transform.position;
                myDir = this.m_dummy.transform.forward;
            }

            float angle = Common.GetAngle360(Vector3.forward, myDir);
            this.m_sprite.SetDirection(angle, false);
            this.m_current_unit_direction = new Vector2(myDir.x, myDir.z);
            this.m_current_sprite_direction = new Vector2(myDir.x, myDir.z);

            if (flag1 && flag2)
            {
                this.ChangeMoveState(CellBase.MOVE_STATE.STATIC, false, 0f, 1f);
            }
            else
            {
                base.transform.Translate(translation, Space.World);
            }
        }

        private void ChaseModeRotateSprite()
        {
            Vector3 vector = this.m_dummy.transform.position - base.transform.position;
            Vector3 normalized = vector.normalized;
            float maxRadiansDelta = 0.0174532924f * this.m_chase_rot_speed * Time.unscaledDeltaTime;
            Vector3 vector2 = new Vector3(this.m_current_unit_direction.x, 0f, this.m_current_unit_direction.y);
            Vector3 vector3 = vector2.normalized;
            Vector3 vector4 = new Vector3(this.m_current_sprite_direction.x, 0f, this.m_current_sprite_direction.y);
            Vector3 vector5 = vector4.normalized;
            Vector3 vector6 = normalized;
            float num = this.m_chase_move_speed_real * Time.unscaledDeltaTime * 8f;
            if (vector.magnitude <= num && this.m_dummy != null)
            {
                Troops componentInParent = this.m_dummy.gameObject.GetComponentInParent<Troops>();
                if (componentInParent != null && componentInParent.m_formation_state == Troops.ENMU_SQUARE_STAT.MOVE)
                {
                    vector6 = componentInParent.transform.forward;
                }
            }
            float num2 = this.m_chase_move_speed_real * Time.unscaledDeltaTime * 8f;
            if (vector.magnitude <= num2)
            {
                vector3 = normalized;
            }
            else if (vector3 != normalized)
            {
                vector3 = Vector3.RotateTowards(vector3, normalized, maxRadiansDelta, 3.40282347E+38f);
            }
            if (vector5 != vector6)
            {
                vector5 = Vector3.RotateTowards(vector5, vector6, maxRadiansDelta, 3.40282347E+38f);
            }
            
            Vector3 translation = vector3 * Time.unscaledDeltaTime * this.m_chase_move_speed_real;
            float angle = Common.GetAngle360(Vector3.forward, vector5);
            this.m_sprite.SetDirection(angle, false);
            this.m_current_unit_direction = new Vector2(vector3.x, vector3.z);
            this.m_current_sprite_direction = new Vector2(vector5.x, vector5.z);
            if (translation.magnitude == 0f || vector.magnitude == 0f || translation.magnitude >= vector.magnitude)
            {
                this.ChangeMoveState(CellBase.MOVE_STATE.STATIC, false, 0f, 1f);
            }
            else
            {
                //Debug.LogError("移动"+translation.magnitude+"**"+ vector.magnitude+"***");
                base.transform.SetParent(this.m_dummy.transform);
                base.transform.Translate(translation, Space.World);
            }
        }

        private void ChaseModeOriginSprite()
        {
            if (this.m_dummy == null)
            {
                if (!Cell.s_isPutDebugLog)
                {
                    string str = string.Empty;
                    if (base.gameObject != null)
                    {
                        str = base.gameObject.name;
                    }
                    Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSprite11_" + str + "\"}");
                    Cell.s_isPutDebugLog = true; 
                    Debug.LogError(base.transform.name);
                }
                return;
            }
            Vector3 vector = this.m_dummy.transform.position - base.transform.position;
            if (vector.magnitude == 0f)
            {
                this.ChangeMoveState(CellBase.MOVE_STATE.STATIC, false, 0f, 1f);
            }
            else
            {
                if (vector.magnitude > 12f && this.m_chase_move_speed_real < vector.magnitude)
                {
                    this.m_chase_move_speed_real = vector.magnitude;
                }
                float num = this.m_chase_move_speed_real * Time.unscaledDeltaTime;
                if (num > vector.magnitude)
                {
                    num = vector.magnitude;
                }
                base.transform.SetParent(this.m_dummy.transform);
                base.transform.Translate(vector.normalized * num, Space.World);
               // Debug.LogError("移动了");
            }
        }

        private void ChaseModeOriginSpriteDir()
        {
            if (this.m_isRotating)
            {
                if (this.m_dummy == null)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        string str = string.Empty;
                        if (base.gameObject != null)
                        {
                            str = base.gameObject.name;
                        }
                        Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir11_" + str + "\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                    return;
                }
                if (this.m_sprite == null)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir22\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                    return;
                }
                Vector3 target = Vector3.zero;
                float num = 0f;
                float num2 = 0f;
                try
                {
                    target = this.m_dummy.transform.forward;
                    num = this.GetAngle180(this.m_current_sprite_direction, new Vector2(target.x, target.z));
                    num2 = 360f * Time.unscaledDeltaTime;
                }
                catch (Exception var_4_E5)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir33\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                    return;
                }
                try
                {
                    if (num2 > Mathf.Abs(num))
                    {
                        num2 = num;
                        this.m_isRotating = false;
                    }
                }
                catch (Exception var_5_120)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir88\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                    return;
                }
                float maxRadiansDelta = num2 * 0.0174532924f;
                Vector3 vector = Vector3.zero;
                try
                {
                    vector = new Vector3(this.m_current_sprite_direction.x, 0f, this.m_current_sprite_direction.y);
                    vector = Vector3.RotateTowards(vector, target, maxRadiansDelta, 3.40282347E+38f);
                }
                catch (Exception var_8_189)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir44\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                    return;
                }
                float direction = 0f;
                try
                {
                    direction = Common.GetAngle360(Vector3.forward, vector);
                }
                catch (Exception var_10_1C4)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir55\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                    return;
                }
                try
                {
                    this.m_sprite.SetDirection(direction, false);
                }
                catch (Exception var_11_1F8)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        //Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir66\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                    return;
                }
                try
                {
                    this.m_current_sprite_direction.x = vector.x;
                    this.m_current_sprite_direction.y = vector.z;
                }
                catch (Exception var_12_242)
                {
                    if (!Cell.s_isPutDebugLog)
                    {
                        Debug.LogError("{\"MSG_ID\":\"CSError_Track_Record_KEY\",\"RecodeKey\":\"ChaseModeOriginSpriteDir77\"}");
                        Cell.s_isPutDebugLog = true;
                    }
                }
            }
        }

        private float GetAngle180(Vector2 from, Vector2 to)
        {
            float num = Vector2.Angle(from, to);
            return (Vector3.Cross(from, to).z <= 0f) ? num : (-num);
        }

        private void Update()
        {
            try
            {
                this.ChaseModeOriginSpriteDir();
                switch (this.m_move_state)
                {
                    case CellBase.MOVE_STATE.CHASE:
                        if (this.m_chase_mode == CellBase.CHASE_MODE.ORIGIN_SPRITE)
                        {
                            this.ChaseModeOriginSprite();
                        }
                        else
                        {
                            this.ChaseModeRotateSprite();
                            //优化围击表现
                            //this.ChaseModeRotateSprite_();
                        }
                        break;
                }
                this.UpdateSpriteAniOnce();
                this.UpdateShadow();
                //临时矫正处理
                //后续需要优化
                if (base.transform.eulerAngles != this.m_euler)
                {
                    base.transform.eulerAngles = this.m_euler;
                }
            }
            catch (Exception var_2_68)
            {
            }
        }

        private void UpdateShadow()
        {
            if (this.is_great_many_squares != LevelDetailScalerManager.instance.isGreatManySqureInScreen())
            {
                this.is_great_many_squares = LevelDetailScalerManager.instance.isGreatManySqureInScreen();
                this.is_need_swicth_shadow = true;
            }
            if (this.is_need_swicth_shadow)
            {
                this.is_need_swicth_shadow = false;
                if (this.is_great_many_squares)
                {
                    this.SwitchToShadowLod1();
                }
                else
                {
                    this.UpdateShadowLod();
                }
            }
        }

        private void SwitchToShadowLod1()
        {
            if (this.m_shadow_sprite && this.m_shadow_sprite.gameObject.activeSelf)
            {
                this.m_shadow_sprite.gameObject.SetActive(false);
            }
            if (this.m_shadow_lod1 && this.m_shadow_lod1.gameObject.activeSelf)
            {
                this.m_shadow_lod1.gameObject.SetActive(false);
            }
        }

        private void SwitchToShadow()
        {
            if (this.m_shadow_sprite && !this.m_shadow_sprite.gameObject.activeSelf)
            {
                this.m_shadow_sprite.gameObject.SetActive(true);
            }
            if (this.m_shadow_lod1 && this.m_shadow_lod1.gameObject.activeSelf)
            {
                this.m_shadow_lod1.gameObject.SetActive(false);
            }
        }

        public override void UpdateLod()
        {
            base.UpdateLod();
           //float lodDistance = Common.GetLodDistance();
            float unitScale = LevelDetailScalerManager.instance.unitScale;
            base.transform.localScale = new Vector3(unitScale, unitScale, unitScale);
            if (base.IsLodChanged())
            {
                this.UpdateShadowLod();
            }
        }

        private void UpdateShadowLod()
        {
            if (this.is_great_many_squares)
            {
                return;
            }
            int currentLodLevel = base.GetCurrentLodLevel();
            if (currentLodLevel == 0)
            {
                if (this.m_shadow_sprite)
                {
                    this.m_shadow_sprite.gameObject.SetActive(true);
                }
                if (this.m_shadow_lod1)
                {
                    this.m_shadow_lod1.gameObject.SetActive(false);
                }
            }
            else if (currentLodLevel == 1)
            {
                if (this.m_shadow_sprite)
                {
                    this.m_shadow_sprite.gameObject.SetActive(false);
                }
                if (this.m_shadow_lod1)
                {
                    this.m_shadow_lod1.gameObject.SetActive(true);
                }
            }
        }

        public override void PlayDeadParticle()
        {
            if (m_die_particle.Equals(string.Empty))
                return;

            string text = this.m_die_particle;
            //if (CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.LOW)
            //{
            //    text += "_low";
            //}
            var pos = transform.position;
            var rotate = m_dummy.transform.rotation;
            float num = 1f / (float)(3 - base.GetCurrentLodLevel());
            var scale = new Vector3(num, num, num);
            CoreUtils.assetService.Instantiate(text, (gameObject) =>
            {
                if (gameObject)
                {
                    gameObject.transform.position = pos;
                    gameObject.transform.rotation = rotate;
                    
                    gameObject.transform.localScale = scale;
                }
            });
        }

        public override void SetLevelupEffect(string effectName)
        {
            if (this.m_levelup_effect == null)
            {
                CoreUtils.assetService.Instantiate(effectName, (gameObject) =>
                {
                    this.m_levelup_effect = gameObject;
                    if (this.m_levelup_effect != null)
                    {
                        this.m_levelup_effect.transform.SetParent(base.transform);
                        this.m_levelup_effect.transform.localPosition = Vector3.zero;
                    }
                });
            }
        }

        private new void OnSpawn()
        {
            this.m_chase_mode = CellBase.CHASE_MODE.ORIGIN_SPRITE;
            this.m_dummy = null;
            base.GetComponent<AnimationBase>().enabled = true;
            base.enabled = true;
            this.UpdateShadowLod();
            base.OnSpawn();
        }

        private new void OnDespawn()
        {
            if (this.m_levelup_effect != null)
            {
                if (this.m_levelup_effect.transform.parent == base.transform)
                {
                    CoreUtils.assetService.Destroy(this.m_levelup_effect);
                }
                this.m_levelup_effect = null;
            }
            if (this.m_shadow_lod1 != null && this.m_shadow_lod1.gameObject.activeInHierarchy)
            {
                this.m_shadow_lod1.GetComponent<CellShadowLod1>().m_fade_state = CellShadowLod1.ENUM_FADE_STATE.NONE;
            }
            if (base.unitType == 9999)
            {
                Transform transform = base.transform.Find("light");
                if (transform != null)
                {
                    transform.GetComponent<NightingParticle>().DestroyParticle();
                }
            }
            this.m_isMoveAtk = false;
            this.m_isRotating = false;
            base.OnDespawn();
        }

        public override void FadeIn()
        {
            this.m_sprite.FadeIn();
            if (this.m_shadow_lod1.gameObject.activeInHierarchy)
            {
                CellShadowLod1 component = this.m_shadow_lod1.gameObject.GetComponent<CellShadowLod1>();
                if (component != null)
                {
                    component.FadeIn();
                }
            }
        }

        public override void FadeOut()
        {
            this.m_sprite.Fadeout();
            CellShadowLod1 component = this.m_shadow_lod1.gameObject.GetComponent<CellShadowLod1>();
            if (component != null)
            {
                component.Fadeout();
            }
            if (base.unitType == 9999)
            {
                Transform transform = base.transform.Find("light");
                if (transform != null)
                {
                    transform.GetComponent<NightingParticle>().StopParticle();
                }
            }
        }

        public override void SetChaseMode(CellBase.CHASE_MODE chase_mode)
        {
            this.m_chase_mode = chase_mode;
        }

        public override void PlaySpriteAniOnce(AnimationBase.ENMU_SPRITE_STATE state)
        {
            this.m_sprite.SetState((int)state, false);
            this.m_spriteAniOnceTime = (float)this.m_sprite.GetFrameCount((int)state) * this.m_sprite.GetUpdateRate((int)state);
        }

        private void PlaySpriteAniOnceEnd()
        {
            this.m_sprite.SetState((int)this.m_cur_sprite_logical_state, true);
        }

        private void UpdateSpriteAniOnce()
        {
            if (this.m_spriteAniOnceTime > 0f)
            {
                this.m_spriteAniOnceTime -= Time.unscaledDeltaTime;
                if (this.m_spriteAniOnceTime <= 0f)
                {
                    this.m_spriteAniOnceTime = 0f;
                    this.PlaySpriteAniOnceEnd();
                }
            }
        }
    }
}