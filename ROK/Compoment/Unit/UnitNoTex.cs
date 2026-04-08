using Skyunion;
using System;
using UnityEngine;

namespace ROK
{
    public class UnitNoTex : UnitBase
    {
        public float[] m_graph_mid_lod_array;

        public float[] m_graph_high_lod_array;

        public UnitDummy m_dummy;

        public float m_chase_move_speed = 1.5f;

        private float m_chase_move_speed_real = 1f;

        public float m_real_chase_move_speed_lower_limit = 1f;

        public float m_chase_rot_speed = 20f;

        private Vector2 m_current_unit_direction = Vector2.zero;

        private float m_delay_chase_time;

        private float m_delay_chase_timer;

        private float m_delay_chase_speed = 1f;

        private Vector2 m_delay_chase_direction = Vector2.zero;

        private Vector3 m_euler = new Vector3(0f, 0f, 0f);

        private float m_alignment_threshold = 0.2f;

        private GameObject m_levelup_effect;

        public string m_die_particle = string.Empty;

        private float chase_direction_frame;

        private float chase_direction;

        private GameObject m_bindObj;

        public GameObject m_bindObjPos;

        private TextureSheetAnimation.ENMU_SPRITE_STATE m_cur_sprite_logical_state = TextureSheetAnimation.ENMU_SPRITE_STATE.MOVE;

        private UnitBase.MOVE_STATE m_move_state;

        public string m_skillParticleName = string.Empty;

        private GameObject m_skillParticle;

        public GameObject m_skillParticleDummy;

        private bool m_isMoveAtk;

        private bool m_syncBindObjPos;

        private bool m_isRotating;

        private void Awake()
        {
            if (GraphicSettingMgr.GetGraphicLevel() == GraphicSettingMgr.GraphicLevel.MEDIUM)
            {
                this.m_lod_array = this.m_graph_mid_lod_array;
            }
            else if (GraphicSettingMgr.GetGraphicLevel() == GraphicSettingMgr.GraphicLevel.HIGH)
            {
                this.m_lod_array = this.m_graph_high_lod_array;
            }
        }

        public void SetBindGameObject(GameObject bindObj)
        {
            this.m_bindObj = bindObj;
            if (this.m_bindObj != null && this.m_bindObjPos != null)
            {
                this.m_bindObj.transform.position = this.m_bindObjPos.transform.position;
            }
        }

        public override void InitDummy(UnitDummy dummy)
        {
            this.m_dummy = dummy;
            base.transform.parent = this.m_dummy.transform;
            base.transform.localPosition = Vector3.zero;
            base.transform.eulerAngles = this.m_euler;
            this.m_move_state = UnitBase.MOVE_STATE.STATIC;
        }

        public override void SetMoveSpeed(float moveSpeed)
        {
            this.m_chase_move_speed_real = this.m_chase_move_speed * moveSpeed;
            if (this.m_chase_move_speed_real <= this.m_real_chase_move_speed_lower_limit)
            {
                this.m_chase_move_speed_real = this.m_real_chase_move_speed_lower_limit;
            }
        }

        public override void ChangeMoveState(UnitBase.MOVE_STATE state, bool isMoveAtk = false, float delay_chase_time = 0f, float delay_chase_speed = 1f)
        {
            if (this.m_move_state == UnitBase.MOVE_STATE.UNBOUND)
            {
                return;
            }
            switch (state)
            {
                case UnitBase.MOVE_STATE.STATIC:
                    this.m_move_state = UnitBase.MOVE_STATE.STATIC;
                    if (this.m_dummy)
                    {
                        base.transform.parent = this.m_dummy.transform;
                        base.transform.localPosition = Vector3.zero;
                        base.transform.localEulerAngles = Vector3.zero;
                        this.m_current_unit_direction = new Vector2(this.m_dummy.transform.forward.x, this.m_dummy.transform.forward.z);
                    }
                    this.SetSpriteState(this.m_cur_sprite_logical_state);
                    break;
                case UnitBase.MOVE_STATE.CHASE:
                    this.m_isMoveAtk = isMoveAtk;
                    this.m_move_state = UnitBase.MOVE_STATE.CHASE;
                    this.m_isRotating = true;
                    try
                    {
                        if (this.m_dummy.isActiveAndEnabled)
                        {
                            base.transform.parent = null;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("UnitNoTex: chase dummy is null");
                        Debug.LogException(e);
                    }
                    try
                    {
                        this.SetSpriteState(TextureSheetAnimation.ENMU_SPRITE_STATE.MOVE);
                    }
                    catch (Exception e2)
                    {
                        Debug.Log("UnitNoTex: chase dummy SetSpriteState is null");
                        Debug.LogException(e2);
                    }
                    break;
                case UnitBase.MOVE_STATE.UNBOUND:
                    this.m_move_state = UnitBase.MOVE_STATE.UNBOUND;
                    if (this.m_dummy.isActiveAndEnabled)
                    {
                        base.transform.parent = null;
                    }
                    this.SetSpriteState(this.m_cur_sprite_logical_state);
                    break;
            }
        }

        private void SetSpriteState(TextureSheetAnimation.ENMU_SPRITE_STATE state)
        {
            Animation component = base.GetComponent<Animation>();
            if (component == null)
            {
                return;
            }
            if (this.m_syncBindObjPos)
            {
                return;
            }
            string name = base.gameObject.name;
            if (state == TextureSheetAnimation.ENMU_SPRITE_STATE.MOVE)
            {
                if (!component.isPlaying)
                {
                    component.Play(name + "_move");
                }
            }
            else if (component.isPlaying)
            {
                component.Stop();
            }
        }

        public void PlaySkillAni()
        {
            Animation component = base.GetComponent<Animation>();
            if (component == null)
            {
                return;
            }
            if (component.isPlaying)
            {
                component.Stop();
            }
            string text = base.gameObject.name + "_attack";
            UICommon.ResetAnimation(base.gameObject, text);
            component.Play(text);
            this.m_syncBindObjPos = true;
        }

        public void SkillAniEnd()
        {
            this.SetSpriteState(this.m_cur_sprite_logical_state);
            this.m_syncBindObjPos = false;
        }

        public void ClearSkillEffect()
        {
            if (this.m_skillParticle != null)
            {
                CoreUtils.assetService.Destroy(this.m_skillParticle);
            }
        }

        public void TriggerSkillEffect()
        {
            if (this.m_skillParticle != null)
            {
                CoreUtils.assetService.Destroy(this.m_skillParticle);
            }
            if (this.m_skillParticleName != string.Empty && this.m_skillParticleDummy != null)
            {
                CoreUtils.assetService.Instantiate(m_skillParticleName, (gameObject) =>
                {
                    this.m_skillParticle = gameObject;
                    this.m_skillParticle.transform.SetParent(this.m_skillParticleDummy.transform, false);
                    this.m_skillParticle.transform.localPosition = Vector3.zero;
                });

            }
        }

        public override void SetSpriteLoigicalState(TextureSheetAnimation.ENMU_SPRITE_STATE state)
        {
            this.m_cur_sprite_logical_state = state;
        }

        private float GetAngle(Vector2 from, Vector2 to)
        {
            float num = Vector2.Angle(from, to);
            return (Vector3.Cross(from, to).z <= 0f) ? num : (-num);
        }

        private void RotateUnit()
        {
            if (this.m_isRotating)
            {
                float angle = this.GetAngle(new Vector2(base.transform.forward.x, base.transform.forward.z), new Vector2(this.m_dummy.transform.forward.x, this.m_dummy.transform.forward.z));
                float num = angle / Mathf.Abs(angle);
                float num2 = 90f * Time.unscaledDeltaTime;
                if (num2 > Mathf.Abs(angle))
                {
                    num2 = angle;
                    this.m_isRotating = false;
                }
                else
                {
                    num2 *= num;
                }
                Vector3 eulerAngles = base.transform.eulerAngles;
                eulerAngles.y += num2;
                base.transform.eulerAngles = eulerAngles;
            }
        }

        private void Update()
        {
            try
            {
                this.RotateUnit();
                switch (this.m_move_state)
                {
                    case UnitBase.MOVE_STATE.CHASE:
                        {
                            Vector3 vector = this.m_dummy.transform.position - base.transform.position;
                            if (vector.magnitude == 0f)
                            {
                                this.ChangeMoveState(UnitBase.MOVE_STATE.STATIC, false, 0f, 1f);
                            }
                            else
                            {
                                float num = this.m_chase_move_speed_real * Time.unscaledDeltaTime;
                                if (num > vector.magnitude)
                                {
                                    num = vector.magnitude;
                                }
                                base.transform.Translate(vector.normalized * num, Space.World);
                            }
                            break;
                        }
                }
                this.SyncBindObjPos(false);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void SyncBindObjPos(bool isForce = false)
        {
            if ((isForce || this.m_syncBindObjPos) && this.m_bindObj != null && this.m_bindObjPos != null)
            {
                this.m_bindObj.transform.position = this.m_bindObjPos.transform.position;
            }
        }

        public override void UpdateLod()
        {
            base.UpdateLod();
            float unitModelScale = LodScalerMgr.instance.unitModelScale;
            base.transform.localScale = new Vector3(unitModelScale, unitModelScale, unitModelScale);
            this.SyncBindObjPos(true);
        }

        private void UpdateShadowLod()
        {
        }

        public override void PlayDeadParticle()
        {
            Animation component = base.GetComponent<Animation>();
            if (component == null)
            {
                return;
            }
            component.Play(base.gameObject.name + "_die");
        }

        public void DeadAniEnd()
        {
        }

        public override void SetLevelupEffect(string effectName)
        {
            if (this.m_levelup_effect == null)
            {
                CoreUtils.assetService.Instantiate(m_skillParticleName, (gameObject) =>
                {
                    this.m_levelup_effect = gameObject;
                    this.m_levelup_effect.transform.SetParent(base.transform);
                    this.m_levelup_effect.transform.localPosition = Vector3.zero;
                });
            }
        }

        private new void OnSpawn()
        {
            base.GetComponent<TextureSheetAnimation>().enabled = true;
            base.enabled = true;
            this.m_bindObj = null;
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
            base.OnDespawn();
        }

        private void OnEnable()
        {
            string text = base.gameObject.name + "_die";
            Animation component = base.GetComponent<Animation>();
            if (component != null && component[text] != null)
            {
                component.Play(text);
                component[text].time = 0f;
                component.Sample();
                component.Stop(text);
            }
        }

        public override void FadeIn()
        {
        }

        public override void FadeOut()
        {
        }
    }
}