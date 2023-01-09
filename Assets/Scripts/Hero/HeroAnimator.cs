using Spine.Unity;
using UnityEngine;

namespace Scripts.Hero
{
    public class HeroAnimator : MonoBehaviour
    {
        private const string WalkAnimationName = "walk";
        private const string IdleAnimationName = "idle";
        private const string SlidingAnimationName = "hoverboard";

        public SkeletonAnimation SkeletonAnimation;

        public void PlayMoving() => 
            SkeletonAnimation.AnimationName = WalkAnimationName;

        public bool IsMoving() =>
            SkeletonAnimation.AnimationName == WalkAnimationName;

        public void PlayIdle() => 
            SkeletonAnimation.AnimationName = IdleAnimationName;

        public bool IsIdle() =>
            SkeletonAnimation.AnimationName == IdleAnimationName;

        public void PlaySliding() => 
            SkeletonAnimation.AnimationName = SlidingAnimationName;

        public bool IsSliding() =>
            SkeletonAnimation.AnimationName == SlidingAnimationName;
    }
}