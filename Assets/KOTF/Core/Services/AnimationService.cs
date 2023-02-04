using System.Collections.Generic;
using System.Linq;
using KOTF.Core.Gameplay.Character;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace KOTF.Core.Services
{
    public class AnimationService : IService
    {
        private HashSet<string> _parameterNames = new();

        public void ValidateAnimator(CharacterBase host)
        {
            AnimatorController controller =
                AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(host.Animator.runtimeAnimatorController));

            if (controller == null)
            {
                Debug.LogError("Could not find an associated AnimatorController for the host.");
                return;
            }

            _parameterNames = controller.parameters.Select(x => x.name).ToHashSet();
            ValidateTransitionConditions(controller);
        }

        private void ValidateTransitionConditions(AnimatorController controller)
        {
            var transitions = controller.layers
                .SelectMany(x => x.stateMachine.states)
                .SelectMany(x => x.state.transitions);

            foreach (var transition in transitions)
            {
                AddTriggerParameter(controller, transition);
                AddTriggerConditionToTransition(transition);
            }
        }

        private void AddTriggerParameter(AnimatorController controller, AnimatorTransitionBase transition)
        {
            if (_parameterNames.Contains(transition.destinationState.name))
                return;

            controller.AddParameter(transition.destinationState.name, AnimatorControllerParameterType.Trigger);
        }

        private void AddTriggerConditionToTransition(AnimatorTransitionBase transition)
        {
            if (transition.conditions.Any(x => _parameterNames.Contains(x.parameter)))
                return;

            transition.AddCondition(AnimatorConditionMode.If, 0.0f, transition.destinationState.name);
        }

        public void TriggerAttackAnimation(Animator animator, bool value)
        {

        }
    }
}
