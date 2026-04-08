using Skyunion;
using System;
using UnityEngine;

namespace Client
{
    public sealed class AnimationHero : AnimationBase
    {
        [Serializable]
        public class SkillAniParamItem
        {
            public int m_state;

            public int m_start_frame;

            public int m_end_frame;
        }

        public SkillAniParamItem[] m_skillAniParam;

        public int m_skillAniChargeFrame;

        public string m_skillAniChargeEffectName;

        private int m_skillAniCurIdx = -1;

        private int m_skillAniCurFrame = -1;

        private int m_skillAniCurTotalFrame;

        public float m_skillInterval = -1.0f;
        public float m_skillIntervalMin = 0.0f;
        public float m_skillIntervalMax = 0.0f;
        private Vector3 m_skillAngles;
        private Vector3 m_skillPos;
        private Vector3 m_focoSkillPos;

        /// <summary>
        /// 技能蓄力光效名称
        /// </summary>
        public string m_skillFocoEffectName = string.Empty;
        /// <summary>
        /// 技能蓄力光效延迟时间（毫秒）
        /// </summary>
        public float m_skillFocoDelayTime = 0f;
        /// <summary>
        /// 技能蓄力光效定时器
        /// </summary>
        private Timer m_skillFocoDelayTimer;

        public void StartSkillAni(Vector3 pos)
        {
            m_skillAniCurIdx = 0;
            m_skillAniCurFrame = -1;
            m_skillAniCurTotalFrame = 0;
            m_focoSkillPos = base.transform.position;
            m_skillPos = pos.Equals(Vector3.zero) ? base.transform.position : pos;
            m_skillAngles = base.transform.GetComponent<Cell>().m_dummy.transform.eulerAngles;

            CancelSkillFocoDelayTimer();

            if (m_skillFocoEffectName != string.Empty)
            {
                Vector3 focoSkillPos = m_focoSkillPos;
                Vector3 skillAngles = m_skillAngles;

                m_skillFocoDelayTimer = Timer.Register(m_skillFocoDelayTime / 1000.0f, () =>
                {
                    if (this == null)
                    {
                        CancelSkillFocoDelayTimer();
                        return;
                    }

                    CoreUtils.assetService.Instantiate(m_skillFocoEffectName, (gameObject) =>
                    {
                        if (gameObject == null)
                        {
                            return;
                        }

                        if (this == null)
                        {
                            CoreUtils.assetService.Destroy(gameObject);
                            return;
                        }

                        gameObject.transform.position = focoSkillPos;
                        gameObject.transform.eulerAngles = new Vector3(0f, skillAngles.y, 0f);
                    });

                    CancelSkillFocoDelayTimer();
                });
            }
        }

        private void CancelSkillFocoDelayTimer()
        {
            if (m_skillFocoDelayTimer != null)
            {
                m_skillFocoDelayTimer.Cancel();
                m_skillFocoDelayTimer = null;
            }
        }

        public override void UpdateAnimation()
        {
            try
            {
                if(CurState == (int)ENMU_SPRITE_STATE.FIGHT && m_skillIntervalMax != 0.0f)
                {
                    if(m_skillInterval < -0.0f)
                    {
                        m_skillInterval = UnityEngine.Random.Range(m_skillIntervalMin, m_skillIntervalMax) + Time.realtimeSinceStartup;
                    }
                    else
                    {
                        if (Time.realtimeSinceStartup > m_skillInterval)
                        {
                            StartSkillAni(Vector3.zero);
                            m_skillInterval = UnityEngine.Random.Range(m_skillIntervalMin, m_skillIntervalMax) + Time.realtimeSinceStartup;
                        }
                    }
                }
                else
                {
                    m_skillInterval = -1.0f;
                }

                if (LevelDetailScalerManager.instance.isGreatManySqureInScreen())
                {
                    base.UpdateAnimation();
                }
                else if (m_skillAniParam.Length > 0 && m_skillAniCurIdx >= 0)
                {
                    SkillAniParamItem skillAniParamItem = m_skillAniParam[m_skillAniCurIdx];
                    if (m_skillAniCurFrame == -1)
                    {
                        m_skillAniCurFrame = skillAniParamItem.m_start_frame;
                    }
                    else
                    {
                        m_skillAniCurFrame++;
                    }
                    if (m_skillAniCurFrame > skillAniParamItem.m_end_frame)
                    {
                        m_skillAniCurFrame = -1;
                        m_skillAniCurIdx++;
                    }
                    else if (skillAniParamItem.m_state != -1)
                    {
                        Sprite[] sprite_array = m_state[skillAniParamItem.m_state].m_direction[base.CurDirection].m_sprite_array;
                        m_sprite_render.sprite = sprite_array[m_skillAniCurFrame];
                    }
                    if (m_skillAniCurIdx >= m_skillAniParam.Length)
                    {
                        m_skillAniCurIdx = -1;
                    }
                    if (m_skillAniCurTotalFrame == m_skillAniChargeFrame)
                    {
                        var effect_pos = m_skillPos;
                        Vector3 eulerAngles = m_skillAngles;
                        CoreUtils.assetService.Instantiate(m_skillAniChargeEffectName, (gameObject)=>
                        {
                            if (gameObject == null)
                                return;

                            if(this == null)
                            {
                                CoreUtils.assetService.Destroy(gameObject);
                                return;
                            }
                            gameObject.transform.position = effect_pos;
                            Transform transform = gameObject.transform;
                            transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
                        });
                    }
                    m_skillAniCurTotalFrame++;
                }
                else
                {
                    // 记录还没叠加的帧
                    int curFrame = CurFrame;
                    base.UpdateAnimation();
                    curFrame += ElapseFrame;

                    if (m_attach_particle_path != string.Empty && base.CurState == m_attach_particle_trigger_state && base.CurFrame >= m_attach_particle_trigger_frame && ((curFrame- m_attach_particle_trigger_frame) <= ElapseFrame) && (bool)base.transform.GetComponent<Cell>().m_dummy)
                    {
                        var effect_pos = base.transform.position;
                        Vector3 eulerAngles = base.transform.GetComponent<Cell>().m_dummy.transform.eulerAngles;
                        CoreUtils.assetService.Instantiate(m_attach_particle_path, (gameObject) =>
                        {
                            if (gameObject == null)
                                return;

                            if (this == null)
                            {
                                CoreUtils.assetService.Destroy(gameObject);
                                return;
                            }
                            gameObject.transform.position = effect_pos;
                            Transform transform = gameObject.transform;
                            transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}