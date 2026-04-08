using Skyunion;
using System;
using UnityEngine;

namespace ROK
{
    public class TextureSheetAnimationArchery : TextureSheetAnimation
    {
        public override void UpdateAnimation()
        {
            base.UpdateAnimation();
            try
            {
                if (!LodScalerMgr.instance.isGreatManySqureInScreen() && this.m_attach_particle_path != string.Empty && base.CurState == this.m_attach_particle_trigger_state && base.CurFrame == this.m_attach_particle_trigger_frame && base.transform.GetComponent<Unit>().m_dummy)
                {
                    CoreUtils.assetService.Instantiate(this.m_attach_particle_path, (GameObject gameObject) =>
                    {
                        if (gameObject)
                        {
                            gameObject.transform.position = base.transform.position;
                            gameObject.transform.eulerAngles = new Vector3(0f, base.transform.GetComponent<Unit>().m_dummy.transform.eulerAngles.y, 0f);
                        }
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
