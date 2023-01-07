using UnityEngine;

namespace KOTF.Core.Services
{
    public class AnimationService : IService
    {
        private const string IS_ATTACKING = "IsAttacking";
        private readonly int _isAttackingHash = Animator.StringToHash(IS_ATTACKING);

        public void TriggerAttackAnimation(Animator animator, bool value)
        {
            animator.SetBool(_isAttackingHash, value);
        }
    }
}
