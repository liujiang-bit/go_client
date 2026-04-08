using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    public class TextureSheetAnimation : MonoBehaviour
    {
        public enum ENMU_SPRITE_STATE
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
            NUM
        }

        public enum ENUM_FADE_STATE
        {
            NONE,
            FADE_IN,
            FADE_OUT,
            NORMAL
        }

        public Formation.ENMU_SQUARE_STAT m_fState = Formation.ENMU_SQUARE_STAT.FIGHT;

        public SpriteStateGroup[] m_state;

        public SpriteStateGroup[] m_shadow_state;

        public int m_atk_additional_num = 8;

        private int m_atk_additional_num_real = 8;

        private int m_cur_full_direction;

        private int m_cur_direction;

        private int m_cur_state;

        private Sprite[] m_cur_sprite;

        private bool m_loop = true;

        public SpriteRenderer m_sprite_render;

        public SpriteRenderer m_shadow_sprite_render;

        public string[] m_enable_particle_path;

        public string m_attach_particle_path = string.Empty;

        public string m_attach_particle_path_big = string.Empty;

        public string m_attach_particle_path_small = string.Empty;

        public int m_attach_particle_trigger_frame = -1;

        public int m_attach_particle_trigger_state = -1;

        public Material m_fade_out_mat;

        public Material m_normal_mat;

        private bool m_enable_fade = true;

        private TextureSheetAnimation.ENUM_FADE_STATE m_fade_state;

        public float m_playing_random_offset = 0.016f;

        private int m_cur_frame;

        private int m_cur_additional_frame;

        private Color m_shadow_color = Color.white;

        public Formation.ENMU_SQUARE_STAT FormationState
        {
            get
            {
                return this.m_fState;
            }
            set
            {
                this.m_fState = value;
            }
        }

        public int CurFullDirection
        {
            get
            {
                return this.m_cur_full_direction;
            }
            set
            {
                this.m_cur_full_direction = value;
            }
        }

        public int CurDirection
        {
            get
            {
                return this.m_cur_direction;
            }
            set
            {
                this.m_cur_direction = value;
            }
        }

        public int CurState
        {
            get
            {
                return this.m_cur_state;
            }
            set
            {
                this.m_cur_state = value;
            }
        }

        public int CurFrame
        {
            get
            {
                return this.m_cur_frame;
            }
            set
            {
                this.m_cur_frame = value;
            }
        }

        private void Awake()
        {
            this.m_cur_state = 0;
            this.m_cur_sprite = this.m_state[0].m_direction[0].m_sprite_array;
            this.m_fade_state = TextureSheetAnimation.ENUM_FADE_STATE.NONE;
        }

        private void OnEnable()
        {
            this.m_cur_frame = UnityEngine.Random.Range(0, this.m_state[0].m_direction[0].m_sprite_array.Length);
            this.m_cur_additional_frame = 0;
            base.InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, this.m_playing_random_offset), this.m_state[this.m_cur_state].m_update_rate);
            if (this.m_normal_mat)
            {
                this.m_sprite_render.material = this.m_normal_mat;
            }
        }

        private void OnDisable()
        {
            base.CancelInvoke();
        }

        private void OnSpawn()
        {
            this.m_fade_state = TextureSheetAnimation.ENUM_FADE_STATE.NONE;
            this.m_cur_frame = 0;
            this.m_fState = Formation.ENMU_SQUARE_STAT.FIGHT;
        }

        public virtual void UpdateAnimation()
        {
            try
            {
                Sprite[] array = this.m_cur_sprite;
                int num;
                if (LodScalerMgr.instance.isGreatManySqureInScreen() && this.m_cur_state == 2)
                {
                    Sprite[] sprite_array = this.m_state[0].m_direction[this.m_cur_direction].m_sprite_array;
                    if (this.m_cur_frame == this.m_cur_sprite.Length - 1)
                    {
                        if (this.m_cur_additional_frame >= this.m_atk_additional_num_real - 1)
                        {
                            this.m_cur_additional_frame = 0;
                            this.m_atk_additional_num_real = UnityEngine.Random.Range(0, this.m_atk_additional_num + 1);
                            this.m_cur_frame = 0;
                            array = this.m_cur_sprite;
                            num = this.m_cur_frame;
                        }
                        else
                        {
                            num = this.m_cur_additional_frame % sprite_array.Length;
                            this.m_cur_additional_frame++;
                            array = sprite_array;
                        }
                    }
                    else
                    {
                        this.m_cur_frame++;
                        if (this.m_cur_frame > this.m_cur_sprite.Length - 1)
                        {
                            this.m_cur_frame = 0;
                        }
                        array = this.m_cur_sprite;
                        num = this.m_cur_frame;
                    }
                }
                else
                {
                    if (this.m_cur_frame == this.m_cur_sprite.Length - 1)
                    {
                        if (this.m_loop)
                        {
                            this.m_cur_frame = 0;
                        }
                    }
                    else if (this.m_cur_frame >= this.m_cur_sprite.Length)
                    {
                        this.m_cur_frame = 0;
                    }
                    else
                    {
                        this.m_cur_frame++;
                    }
                    num = this.m_cur_frame;
                    array = this.m_cur_sprite;
                }
                this.m_sprite_render.sprite = array[num];
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
            this.UpdateShadowSprite();
        }

        private void UpdateShadowSprite()
        {
            try
            {
                if (this.m_shadow_sprite_render && this.m_shadow_sprite_render.enabled)
                {
                    if (this.m_cur_state == 2)
                    {
                        if (this.m_cur_frame == this.m_cur_sprite.Length - 1)
                        {
                            Sprite[] sprite_array = this.m_shadow_state[0].m_direction[this.m_cur_direction].m_sprite_array;
                            this.m_shadow_sprite_render.sprite = sprite_array[this.m_cur_additional_frame % sprite_array.Length];
                        }
                        else
                        {
                            this.m_shadow_sprite_render.sprite = this.m_shadow_state[this.m_cur_state].m_direction[this.m_cur_direction].m_sprite_array[this.m_cur_frame];
                        }
                    }
                    else
                    {
                        this.m_shadow_sprite_render.sprite = this.m_shadow_state[this.m_cur_state].m_direction[this.m_cur_direction].m_sprite_array[this.m_cur_frame];
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        private int DirectionToIndex(int direction)
        {
            int num = direction / 45 % 8;
            this.m_cur_full_direction = num;
            if (num == 5)
            {
                num = 3;
            }
            else if (num == 6)
            {
                num = 2;
            }
            else if (num == 7)
            {
                num = 1;
            }
            return num;
        }

        private void SetSpriteArray(int direction, int state)
        {
            this.m_cur_sprite = this.m_state[this.m_cur_state].m_direction[this.m_cur_direction].m_sprite_array;
            base.CancelInvoke();
            base.InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, this.m_playing_random_offset), this.m_state[this.m_cur_state].m_update_rate);
        }

        public void SetUpdateRate(int state, float update_rate)
        {
            this.m_state[state].m_update_rate = update_rate;
            if (state == this.m_cur_state)
            {
                base.CancelInvoke();
                base.InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, this.m_playing_random_offset), this.m_state[this.m_cur_state].m_update_rate);
            }
        }

        public float GetUpdateRate(int state)
        {
            return this.m_state[state].m_update_rate;
        }

        public void SetState(int state, bool loop = true)
        {
            if (this.m_cur_state != state)
            {
                this.m_cur_state = state;
                this.m_loop = loop;
                this.m_cur_frame = 0;
                this.m_cur_additional_frame = 0;
                this.m_atk_additional_num_real = UnityEngine.Random.Range(0, this.m_atk_additional_num + 1);
                this.SetSpriteArray(this.m_cur_direction, this.m_cur_state);
            }
        }

        public void SetState(int state, int start_frame, bool loop = true)
        {
            if (this.m_cur_state != state)
            {
                this.m_cur_state = state;
                this.m_loop = loop;
                this.m_cur_frame = start_frame;
                this.m_cur_additional_frame = 0;
                this.m_atk_additional_num_real = UnityEngine.Random.Range(0, this.m_atk_additional_num + 1);
                this.SetSpriteArray(this.m_cur_direction, this.m_cur_state);
            }
        }

        public int GetFrameCount(int state)
        {
            return this.m_state[state].m_direction[this.m_cur_direction].m_sprite_array.Length;
        }

        public void SetDirection(float direction, bool rotate_sprite = false)
        {
            int num = this.ConvertToSpriteDirection(direction);
            if (num == 0 || num == 45 || num == 90 || num == 135 || num == 180 || num == 360)
            {
                this.m_sprite_render.flipX = false;
                this.m_shadow_sprite_render.flipX = false;
            }
            else
            {
                this.m_sprite_render.flipX = true;
                this.m_shadow_sprite_render.flipX = true;
            }
            if (rotate_sprite)
            {
                Vector3 eulerAngles = this.m_sprite_render.transform.eulerAngles;
                eulerAngles.y = direction - (float)num;
                this.m_sprite_render.transform.eulerAngles = eulerAngles;
            }
            int num2 = this.DirectionToIndex(num);
            if (this.m_cur_direction != num2)
            {
                this.m_cur_direction = num2;
                this.SetSpriteArray(this.m_cur_direction, this.m_cur_state);
            }
        }

        public void SetParticleParams(string path, int state, int frame)
        {
            bool flag = false;
            string[] enable_particle_path = this.m_enable_particle_path;
            for (int i = 0; i < enable_particle_path.Length; i++)
            {
                string a = enable_particle_path[i];
                if (a == path)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                return;
            }
            this.m_attach_particle_path = path;
            this.m_attach_particle_trigger_state = state;
            this.m_attach_particle_trigger_frame = frame;
        }

        public void SetCurLodForParticle(int lod)
        {
            if (lod == 2)
            {
                if (this.m_attach_particle_path_big != string.Empty)
                {
                    this.m_attach_particle_path = this.m_attach_particle_path_big;
                }
            }
            else if (lod <= 1 && this.m_attach_particle_path_small != string.Empty)
            {
                this.m_attach_particle_path = this.m_attach_particle_path_small;
            }
        }

        public void FadeIn()
        {
            if (LodScalerMgr.instance.isGreatManySqureInScreen())
            {
                return;
            }
            if (this.m_enable_fade)
            {
                TextureSheetAnimation.ENUM_FADE_STATE fade_state = this.m_fade_state;
                this.m_fade_state = TextureSheetAnimation.ENUM_FADE_STATE.FADE_IN;
                if (fade_state == TextureSheetAnimation.ENUM_FADE_STATE.NONE)
                {
                    this.m_sprite_render.material = this.m_fade_out_mat;
                    Vector4 vector = this.m_sprite_render.color;
                    Color color = new Color(vector.x, vector.y, vector.z, 0f);
                    this.m_sprite_render.color = color;
                    this.m_shadow_color.a = 0f;
                    this.m_shadow_sprite_render.color = this.m_shadow_color;
                    if (base.isActiveAndEnabled)
                    {
                        base.StartCoroutine(this.UpdateFade());
                    }
                }
            }
        }

        public bool Fadeout()
        {
            if (LodScalerMgr.instance.isGreatManySqureInScreen())
            {
                return false;
            }
            if (!this.m_enable_fade)
            {
                return false;
            }
            if (base.gameObject.activeInHierarchy)
            {
                TextureSheetAnimation.ENUM_FADE_STATE fade_state = this.m_fade_state;
                this.m_fade_state = TextureSheetAnimation.ENUM_FADE_STATE.FADE_OUT;
                if (fade_state == TextureSheetAnimation.ENUM_FADE_STATE.NONE)
                {
                    this.m_sprite_render.material = this.m_fade_out_mat;
                    Vector4 vector = this.m_sprite_render.color;
                    Color color = new Color(vector.x, vector.y, vector.z, 1f);
                    this.m_sprite_render.color = color;
                    this.m_shadow_color.a = 1f;
                    this.m_shadow_sprite_render.color = this.m_shadow_color;
                    if (base.isActiveAndEnabled)
                    {
                        base.StartCoroutine(this.UpdateFade());
                    }
                }
                return true;
            }
            return false;
        }

        private IEnumerator UpdateFade()
        {
            while (m_fade_state != 0)
            {
                try
                {
                    if (m_fade_state == ENUM_FADE_STATE.FADE_IN)
                    {
                        Color color = m_sprite_render.color;
                        color.a += Time.deltaTime * 3f;
                        if (color.a >= 1f)
                        {
                            color.a = 1f;
                            m_fade_state = ENUM_FADE_STATE.NONE;
                            m_sprite_render.color = color;
                            m_shadow_color.a = 1f;
                            m_shadow_sprite_render.color = m_shadow_color;
                            m_sprite_render.material = m_normal_mat;
                        }
                        else
                        {
                            m_sprite_render.color = color;
                            m_shadow_color.a = color.a;
                            m_shadow_sprite_render.color = m_shadow_color;
                        }
                    }
                    else if (m_fade_state == ENUM_FADE_STATE.FADE_OUT)
                    {
                        Vector4 vector = m_sprite_render.color;
                        Color color2 = new Color(vector.x, vector.y, vector.z, vector.w);
                        float num = Time.deltaTime * 3f;
                        color2.a -= num;
                        if (color2.a <= 0f)
                        {
                            color2.a = 0f;
                            m_sprite_render.color = color2;
                            m_shadow_color.a = 0f;
                            m_shadow_sprite_render.color = m_shadow_color;
                            m_fade_state = ENUM_FADE_STATE.NONE;
                        }
                        else
                        {
                            m_sprite_render.color = color2;
                            m_shadow_color.a = color2.a;
                            m_shadow_sprite_render.color = m_shadow_color;
                        }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
                yield return null;
            }
        }

        private int ConvertToSpriteDirection(float direction)
        {
            return (int)(direction + 22.5f) / 45 * 45;
        }
    }
}