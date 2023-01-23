using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using KOTF.Core.Gameplay.Character;
using KOTF.Utils.Path;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace KOTF.Core.Services
{
    /// <summary>
    /// The AnimationService contains useful actions that pertain to specific or general animations (Idle, Walk, Attack, Chain Attack etc.)
    /// </summary>
    public class AnimationService : IService
    {
        private const string IS_ATTACKING = "IsAttacking";
        private readonly int _isAttackingHash = Animator.StringToHash(IS_ATTACKING);

        private HashSet<string> _chainAttackParameters = new();

        public void ValidateAnimator(CharacterBase host)
        {
            ValidateAttackAnimator(host);
        }

        private void ValidateAttackAnimator(CharacterBase host)
        {
            AnimatorController controller =
                AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(host.Animator.runtimeAnimatorController));

            if (controller == null)
                Debug.LogWarning("Could not find an associated AnimatorController for the host.");

            ValidateAttackAnimationParameters(host, controller);
            ValidateAttackAnimationTransitionConditions(controller);
        }

        private void ValidateAttackAnimationParameters(CharacterBase host, AnimatorController animatorController)
        {
            HashSet<string> parameterNames = animatorController.parameters.Select(x => x.name).ToHashSet();

            // We need to get all weapons instead.
            var animations = new LinkedList<AnimationClip>(
                Resources.LoadAll<AnimationClip>(Path.Combine(PathUtils.WEAPON_ANIMATIONS, host.WieldedWeapon.name)));

            if (animations.Any() != true)
                return;

            LinkedListNode<AnimationClip> animationNode = animations.First;
            while (animationNode?.Next != null)
            {
                // Get all attack transitions and apply the conditions.
                string parameterName = $"{animationNode.Value.name}_{animationNode.Next.Value.name}";
                _chainAttackParameters.Add(parameterName);
                if (parameterNames.Contains(parameterName))
                    return;

                animatorController.AddParameter(parameterName, AnimatorControllerParameterType.Trigger);
                animationNode = animationNode.Next;
            }
        }

        private void ValidateAttackAnimationTransitionConditions(AnimatorController animatorController)
        {
            if (_chainAttackParameters.Count == 0)
                return;

            var transitions = animatorController.layers
                .SelectMany(x => x.stateMachine.states)
                .SelectMany(x => x.state.transitions)
                .ToList();

            foreach (string chainAttackParameter in _chainAttackParameters)
            {
                string destinationState = GetDestinationStateFromTransitionName(chainAttackParameter);
                if (string.IsNullOrEmpty(destinationState))
                    continue;

                AnimatorStateTransition transition = transitions.FirstOrDefault(x => x.destinationState.name.Equals(destinationState));
                if (transition == null)
                    continue;

                AddTriggerConditionToTransition(transition, chainAttackParameter);
            }
        }

        private void AddTriggerConditionToTransition(AnimatorTransitionBase transition, string chainAttackParameter)
        {
            if (transition.conditions.Any(x => _chainAttackParameters.Contains(x.parameter)))
                return;

            transition.AddCondition(AnimatorConditionMode.If, 0.0f, chainAttackParameter);
        }

        private string GetDestinationStateFromTransitionName(string transitionName)
        {
            Regex regex = new Regex("[^_]*$");
            MatchCollection matches = regex.Matches(transitionName);
            return matches.Count == 0 ? string.Empty : matches[0].Value;
        }

        public void TriggerAttackAnimation(Animator animator, bool value)
        {
            animator.SetBool(_isAttackingHash, value);
        }
    }
}
