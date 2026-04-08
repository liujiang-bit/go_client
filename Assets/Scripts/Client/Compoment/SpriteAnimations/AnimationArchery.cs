using Skyunion;
using System;
using UnityEngine;

namespace Client
{
    public sealed class AnimationArchery : AnimationBase
    {
        public override void UpdateAnimation()
        {
            // 记录还没叠加的帧
            int curFrame = CurFrame;
            base.UpdateAnimation();
            curFrame += ElapseFrame;
            try
            {
                if (!LevelDetailScalerManager.instance.isGreatManySqureInScreen() && this.m_attach_particle_path != string.Empty && base.CurState == this.m_attach_particle_trigger_state && base.CurFrame >= m_attach_particle_trigger_frame && ((curFrame - m_attach_particle_trigger_frame) <= ElapseFrame) && base.transform.GetComponent<Cell>().m_dummy)
                {
                    if(((float)UnityEngine.Random.Range(0.0f, 1.0f) > this.m_attach_particle_trigger))
                    {
                        return;
                    }
                    CoreUtils.assetService.Instantiate(m_attach_particle_path, (gameObject) =>
                    {
                        if (gameObject == null)
                            return;

                        if (this == null)
                        {
                            CoreUtils.assetService.Destroy(gameObject);
                            return;
                        }
                        gameObject.transform.position = base.transform.position;
                        Transform transform = gameObject.transform;
                        Vector3 eulerAngles = base.transform.GetComponent<Cell>().m_dummy.transform.eulerAngles;
                        transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
