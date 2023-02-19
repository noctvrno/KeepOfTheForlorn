using System;
using System.Collections.Generic;
using System.Linq;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Input;
using KOTF.Core.Wrappers;
using UnityEditor.Animations;
using UnityEngine;

namespace KOTF.Core.Services
{
    /// <summary>
    /// The AnimationService contains useful actions that pertain to specific or general animations (Idle, Walk, Attack, Chain Attack etc.)
    /// </summary>
    public class AnimationService : IService
    {
        private CharacterBase _host;
        private readonly Array _actionTypes = Enum.GetValues(typeof(ActionType));
        private Dictionary<ActionType, List<KotfAnimationClip>> _actionTypeToAnimationClips = new();

        public void ValidateAnimator(CharacterBase host)
        {
            ValidateAttackAnimator(host);
        }

        private void ValidateAttackAnimator(CharacterBase host)
        {
            _host = host;

            InitializeAnimationClips();
            ValidateTransitionConditions();
        }

        private void InitializeAnimationClips()
        {
            foreach (var parameter in _host.AnimatorController.parameters.OrderBy(x => x.name).Select(x => x.name))
            {
                TryAddAnimationClip(parameter);
            }
        }

        private bool TryAddAnimationClip(string parameter)
        {
            ActionType actionType = ParseParameterToActionType(parameter);

            if (!_actionTypeToAnimationClips.TryGetValue(actionType, out List<KotfAnimationClip> animationClips))
            {
                _actionTypeToAnimationClips.Add(actionType,
                    new List<KotfAnimationClip> { KotfAnimationClip.Create(_host, actionType, parameter) });

                return true;
            }

            if (animationClips.Exists(x => x.ParameterName.Equals(parameter)))
                return false;

            animationClips.Add(KotfAnimationClip.Create(_host, actionType, parameter));
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

        private void ValidateTransitionConditions()
        {
            var transitions = _host.AnimatorController.layers
                .SelectMany(x => x.stateMachine.states)
                .SelectMany(x => x.state.transitions);

            foreach (var transition in transitions)
            {
                AddTriggerParameter(transition);
                AddTriggerConditionToTransition(transition);
            }
        }

        private void AddTriggerParameter(AnimatorTransitionBase transition)
        {
            if (!TryAddAnimationClip(transition.destinationState.name))
                return;

            _host.AnimatorController.AddParameter(transition.destinationState.name, AnimatorControllerParameterType.Trigger);
        }

        private void AddTriggerConditionToTransition(AnimatorTransitionBase transition)
        {
            if (transition.conditions.Any(x => _actionTypeToAnimationClips
                    .SelectMany(x => x.Value)
                    .Select(x => x.ParameterName)
                    .Contains(x.parameter)))
            {
                return;
            }

            transition.AddCondition(AnimatorConditionMode.If, 0.0f, transition.destinationState.name);
        }

        public void TriggerAnimation(ActionType actionType, int animationParameterIndex = 0)
        {
            if (!_actionTypeToAnimationClips.TryGetValue(actionType, out List<KotfAnimationClip> animationClips))
            {
                Debug.LogError($"Could not find {actionType} among {nameof(_actionTypeToAnimationClips)}");
                return;
            }

            _host.Animator.SetTrigger(animationClips[animationParameterIndex % animationClips.Count].ParameterName);
        }
    }
}
