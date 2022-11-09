using Fusion;
using Fusion.KCC;
using UnityEngine;
using Fusion.Animations;

namespace FusionExamples.Tanknarok
{
    public sealed class CharacterAnimationController : AnimationController
    {
        public Animator animator;
        protected override void Awake()
        {
            base.Awake();
            SetAnimator(animator);
        }

        protected override void OnFixedUpdate()
        {
            // animator.SetBool("run", true);
        }
    }
}