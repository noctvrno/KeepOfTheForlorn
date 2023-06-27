using System;
using System.Collections.Generic;
using System.Linq;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Input;
using KOTF.Core.Wrappers;
using KOTF.Utils.Extensions;
using UnityEditor.Animations;
using UnityEngine;

namespace KOTF.Core.Services
{
    /// <summary>
    /// The CharacterAnimationHandler contains useful actions that pertain to specific or general animations (Idle, Walk, Attack, Chain Attack etc.)
    /// </summary>
    public class CharacterAnimationHandler
    {
        private readonly CharacterBase _host;
        private readonly Array _actionTypes = Enum.GetValues(typeof(ActionType));
        private Dictionary<ActionType, List<KotfAnimationClip>> _actionTypeToAnimationClips = new();

        public CharacterAnimationHandler(CharacterBase host)
        {
            _host = host;
        }

        public void ValidateAnimator()
        {
            ValidateAttack();
            ValidateParry();
        }

        private void ValidateAttack()
        {
            InitializeAnimationClips();
            ValidateTransitionConditions();
            InitializeAttackAnimationEvents();
        }

        private void ValidateParry()
        {
            InitializeParryAnimationEvents();
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
            AnimationClip animationClip =
                _host.AnimatorController.animationClips.FirstOrDefault(x => x.name.Equals(parameter));

            if (!_actionTypeToAnimationClips.TryGetValue(actionType, out List<KotfAnimationClip> animationClips))
            {
                _actionTypeToAnimationClips.Add(actionType,
                    new List<KotfAnimationClip> { new(animationClip, actionType, parameter) });

                return true;
            }

            if (animationClips.Exists(x => x.ParameterName.Equals(parameter)))
                return false;

            animationClips.Add(new KotfAnimationClip(animationClip, actionType, parameter));
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

        private void InitializeAttackAnimationEvents()
        {
            GetAnimationClips(ActionType.Attack)?
                .Select(x => x.AnimationClip)
                .ForEach(attackAnimationClip =>
                {
                    AddAnimationEvent(attackAnimationClip, nameof(_host.OnExitAttackAnimation),
                        attackAnimationClip.length);

                    if (_host is not IChainCapable chainCapableCharacter)
                        return;

                    AnimationEvent onExitAttackWindowEvent =
                        attackAnimationClip.events.FirstOrDefault(x =>
                            x.functionName.Equals(nameof(_host.OnExitAttackWindow)));

                    if (onExitAttackWindowEvent == null)
                        return;

                    float exitAttackWindowFrame =
                        attackAnimationClip.frameRate * onExitAttackWindowEvent.time;

                    float chainAttackEndFrameRate =
                        exitAttackWindowFrame + chainCapableCharacter.WieldedWeapon.ChainAttackFrame;

                    AddAnimationEvent(attackAnimationClip,
                        nameof(chainCapableCharacter.OnExitChainPossibility),
                        chainAttackEndFrameRate / attackAnimationClip.frameRate);
                });
        }

        private void InitializeParryAnimationEvents()
        {
            GetAnimationClips(ActionType.Parry)
                ?.Select(x => x.AnimationClip)
                .ForEach(parryAnimationClip =>
                {
                    AddAnimationEvent(parryAnimationClip, nameof(_host.OnExitParryWindow),
                        _host.ParryWindowFrames / parryAnimationClip.frameRate);
                });
        }

        private static void AddAnimationEvent(AnimationClip animationClip, string functionName, float time)
        {
            animationClip.AddEvent(new AnimationEvent
            {
                functionName = functionName,
                time = time
            });
        }

        private List<KotfAnimationClip> GetAnimationClips(ActionType actionType)
        {
            return _actionTypeToAnimationClips.TryGetValue(actionType, out var value) ? value : null;
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
