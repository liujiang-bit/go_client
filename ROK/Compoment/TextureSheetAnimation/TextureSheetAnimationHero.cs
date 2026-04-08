using Skyunion;
using System;
using UnityEngine;

namespace ROK
{
    public class TextureSheetAnimationHero : TextureSheetAnimation
    {
        [Serializable]
        public class SkillAniParamItem
        {
            public int m_state;

            public int m_start_frame;

            public int m_end_frame;
        }

        public TextureSheetAnimationHero.SkillAniParamItem[] m_skillAniParam;

        public int m_skillAniChargeFrame;

        public string m_skillAniChargeEffectName;

        private int m_skillAniCurIdx = -1;

        private int m_skillAniCurFrame = -1;

        private int m_skillAniCurTotalFrame;

        public void StartSkillAni()
        {
            this.m_skillAniCurIdx = 0;
            this.m_skillAniCurFrame = -1;
            this.m_skillAniCurTotalFrame = 0;
        }

        public override void UpdateAnimation()
        {
            try
            {
                if (LodScalerMgr.instance.isGreatManySqureInScreen())
                {
                    base.UpdateAnimation();
                }
                else if (this.m_skillAniParam.Length > 0 && this.m_skillAniCurIdx >= 0)
                {
                    if (base.FormationState == Formation.ENMU_SQUARE_STAT.FIGHT)
                    {
                        TextureSheetAnimationHero.SkillAniParamItem skillAniParamItem = this.m_skillAniParam[this.m_skillAniCurIdx];
                        if (this.m_skillAniCurFrame == -1)
                        {
                            this.m_skillAniCurFrame = skillAniParamItem.m_start_frame;
                        }
                        else
                        {
                            this.m_skillAniCurFrame++;
                        }
                        if (this.m_skillAniCurFrame > skillAniParamItem.m_end_frame)
                        {
                            this.m_skillAniCurFrame = -1;
                            this.m_skillAniCurIdx++;
                        }
                        else if (skillAniParamItem.m_state != -1)
                        {
                            Sprite[] sprite_array = this.m_state[skillAniParamItem.m_state].m_direction[base.CurDirection].m_sprite_array;
                            this.m_sprite_render.sprite = sprite_array[this.m_skillAniCurFrame];
                        }
                        if (this.m_skillAniCurIdx >= this.m_skillAniParam.Length)
                        {
                            this.m_skillAniCurIdx = -1;
                        }
                        if (this.m_skillAniCurTotalFrame == this.m_skillAniChargeFrame)
                        {
                            CoreUtils.assetService.Instantiate(this.m_attach_particle_path, (GameObject gameObject) =>
                            {
                                if (gameObject)
                                {
                                    gameObject.transform.position = base.gameObject.transform.position;
                                }
                            });
                        }
                        this.m_skillAniCurTotalFrame++;
                    }
                    else
                    {
                        this.m_skillAniCurIdx = -1;
                        this.m_skillAniCurFrame = -1;
                        this.m_skillAniCurTotalFrame = 0;
                    }
                }
                else
                {
                    base.UpdateAnimation();
                    if (this.m_attach_particle_path != string.Empty && base.CurState == this.m_attach_particle_trigger_state && base.CurFrame == this.m_attach_particle_trigger_frame && base.transform.GetComponent<Unit>().m_dummy)
                    {
                        CoreUtils.assetService.Instantiate(this.m_attach_particle_path, (GameObject gameObject2) =>
                        {
                            if (gameObject2 != null)
                            {
                                gameObject2.transform.position = base.transform.position;
                                gameObject2.transform.eulerAngles = new Vector3(0f, base.transform.GetComponent<Unit>().m_dummy.transform.eulerAngles.y, 0f);
                            }
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