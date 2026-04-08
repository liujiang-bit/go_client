using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    public class CloudPlane : MonoBehaviour
    {
        public void PlayCloudAnimation()
        {
            base.gameObject.SetActive(true);
            base.StartCoroutine(this.DoPlayCloudAnimation());
        }

        [DebuggerHidden]
        private IEnumerator DoPlayCloudAnimation()
        {
            Animation animation = null;
            float animation_length = 0f;
            try
            {
                animation = base.transform.GetComponent<Animation>();
                animation_length = animation[animation.clip.name].length;
                animation[animation.clip.name].time = 0f;
                animation[animation.clip.name].speed = 1f;
                animation.Play();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
            yield return new WaitForSeconds(animation_length);
            try
            {
                animation[animation.clip.name].time = animation_length;
                animation[animation.clip.name].speed = -1f;
                animation.Play();
            }
            catch (Exception e2)
            {
                UnityEngine.Debug.LogException(e2);
            }
        }
    }
}