using System.Collections.Generic;
using System.IO;
using System.Linq;
using KOTF.Core.Gameplay.Character;
using KOTF.Utils.Path;
using UnityEngine;

namespace KOTF.Core.Services
{
    public class AnimationService : IService
    {
        private const string IS_ATTACKING = "IsAttacking";
        private readonly int _isAttackingHash = Animator.StringToHash(IS_ATTACKING);

        private const string CHAIN = "Chain";

        public void InitializeDynamics(CharacterBase host)
        {
            InitializeAnimatorParameters(host);
            InitializeAnimationTransitionConditions(host);
        }

        private void InitializeAnimatorParameters(CharacterBase host)
        {

        }

        private void InitializeAnimationTransitionConditions(CharacterBase host)
        {
            // We need to get all weapons instead. We must replace before final commit.
            var animations = new LinkedList<AnimationClip>(Resources.LoadAll<AnimationClip>(Path.Combine(PathUtils.WEAPON_ANIMATIONS, host.WieldedWeapon.Name)));
            if (animations.Any() != true)
                return;

            LinkedListNode<AnimationClip> animationNode = animations.First;
            while (animationNode?.Next != null)
            {
                // Get all attack transitions and apply the conditions.
                string parameterName = $"{CHAIN}_{animationNode.Value.name}_{animationNode.Next.Value.name}";

                animationNode = animationNode.Next;
            }
        }

        public void TriggerAttackAnimation(Animator animator, bool value)
        {
            animator.SetBool(_isAttackingHash, value);
        }
    }
}
