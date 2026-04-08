using Spine;
using Spine.Unity;
using System;
using UnityEngine;

namespace ROK
{
    public class SpineAni : MonoBehaviour
    {
        private SkeletonGraphic skeletonGraphic;

        private SkeletonAnimation skeletonAnimation;

        private string curAnimName = string.Empty;

        private void Awake()
        {
            this.skeletonGraphic = base.GetComponent<SkeletonGraphic>();
            this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
        }

        public void SetAlpha(float alpha)
        {
            if (this.skeletonGraphic != null)
            {
                Color color = this.skeletonGraphic.color;
                color.a = alpha;
                this.skeletonGraphic.color = color;
            }
        }

        public void SetColor(float r, float g, float b)
        {
            if (this.skeletonGraphic != null)
            {
                Color color = this.skeletonGraphic.color;
                color.r = r;
                color.g = g;
                color.b = b;
                this.skeletonGraphic.color = color;
            }
            if (this.skeletonAnimation != null)
            {
                this.skeletonAnimation.skeleton.R = r;
                this.skeletonAnimation.skeleton.G = g;
                this.skeletonAnimation.skeleton.B = b;
            }
        }

        public void Play(string ani_name)
        {
            Play(ani_name, true);
        }

        public float Play(string ani_name, bool loop)
        {
            TrackEntry trackEntry = null;
            if (this.skeletonGraphic != null)
            {
                this.skeletonGraphic.timeScale = 1f;
                Spine.Animation animation = this.skeletonGraphic.skeletonDataAsset.GetSkeletonData(false).FindAnimation(ani_name);
                if (animation != null)
                {
                    trackEntry = this.skeletonGraphic.AnimationState.SetAnimation(0, animation, loop);
                    this.skeletonGraphic.Update();
                }
            }
            if (this.skeletonAnimation != null)
            {
                this.skeletonAnimation.timeScale = 1f;
                Spine.Animation animation2 = this.skeletonAnimation.skeletonDataAsset.GetSkeletonData(false).FindAnimation(ani_name);
                if (animation2 != null)
                {
                    trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(0, animation2, loop);
                    this.skeletonAnimation.Update(0f);
                }
            }
            if (trackEntry != null)
            {
                this.curAnimName = ani_name;
                return trackEntry.Animation.Duration;
            }
            return 0f;
        }

        public void Stop()
        {
            if (this.skeletonGraphic != null)
            {
                this.skeletonGraphic.timeScale = 0f;
            }
            if (this.skeletonAnimation != null)
            {
                this.skeletonAnimation.timeScale = 0f;
            }
            this.curAnimName = string.Empty;
        }

        public string GetAnimName()
        {
            return this.curAnimName;
        }

        public float GetAnimaTime(string ani_name)
        {
            if (this.skeletonGraphic != null)
            {
                Spine.Animation animation = this.skeletonGraphic.skeletonDataAsset.GetSkeletonData(false).FindAnimation(ani_name);
                if (animation != null)
                {
                    return animation.Duration;
                }
            }
            if (this.skeletonAnimation != null)
            {
                Spine.Animation animation2 = this.skeletonAnimation.skeletonDataAsset.GetSkeletonData(false).FindAnimation(ani_name);
                if (animation2 != null)
                {
                    return animation2.Duration;
                }
            }
            return 0f;
        }
    }
}