using System;
using System.Collections.Generic;
using System.Linq;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Input;
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
        private Animator _animator;
        private readonly Array _actionTypes = Enum.GetValues(typeof(ActionType));
        private Dictionary<ActionType, List<string>> _actionTypeToParameterNames = new();

        public void ValidateAnimator(CharacterBase host)
        {
            ValidateAttackAnimator(host);
        }

        private void ValidateAttackAnimator(CharacterBase host)
        {
            _animator = host.Animator;
            AnimatorController controller =
                AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(_animator.runtimeAnimatorController));

            if (controller == null)
            {
                Debug.LogError("Could not find an associated AnimatorController for the host.");
                return;
            }

            InitializeAnimatorParameters(controller);
            ValidateTransitionConditions(controller);
        }

        private void InitializeAnimatorParameters(AnimatorController controller)
        {
            foreach (var parameter in controller.parameters.OrderBy(x => x.name).Select(x => x.name))
            {
                TryAddAnimatorParameter(parameter);
            }
        }

        private bool TryAddAnimatorParameter(string parameter)
        {
            ActionType actionType = ParseParameterToActionType(parameter);

            if (!_actionTypeToParameterNames.TryGetValue(actionType, out var value))
            {
                _actionTypeToParameterNames.Add(actionType, new List<string> { parameter });
                return true;
            }

            if (value.Exists(x => x.Equals(parameter)))
                return false;

            value.Add(parameter);
            return true;
        }

        private ActionType ParseParameterToActionType(string parameter)
        {
            foreach (object actionTypeValue in _actionTypes)
            {
                string actionTypeStr = actionTypeValue.ToString();
                if (parameter.Contains(actionTypeStr) && Enum.TryParse(actionTypeStr, out ActionType actionType))
                    return actionType;
            }

            throw new ArgumentException($"Could not parse {parameter} to {nameof(ActionType)}");
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
            if (!TryAddAnimatorParameter(transition.destinationState.name))
                return;

            controller.AddParameter(transition.destinationState.name, AnimatorControllerParameterType.Trigger);
        }

        private void AddTriggerConditionToTransition(AnimatorTransitionBase transition)
        {
            if (transition.conditions.Any(x => _actionTypeToParameterNames.SelectMany(x => x.Value).Contains(x.parameter)))
                return;

            transition.AddCondition(AnimatorConditionMode.If, 0.0f, transition.destinationState.name);
        }

        public void TriggerAnimation(ActionType actionType, int animationParameterIndex = 0)
        {
            if (!_actionTypeToParameterNames.TryGetValue(actionType, out List<string> parameters))
            {
                Debug.LogError($"Could not find {actionType} among {nameof(_actionTypeToParameterNames)}");
                return;
            }

            _animator.SetTrigger(parameters[animationParameterIndex % parameters.Count]);
        }
    }
}
