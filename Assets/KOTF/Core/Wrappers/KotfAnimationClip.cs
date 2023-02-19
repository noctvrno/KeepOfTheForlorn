using System;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Input;
using System.Linq;
using UnityEngine;

namespace KOTF.Core.Wrappers
{
    public class KotfAnimationClip
    {
        public AnimationClip AnimationClip { get; }
        protected CharacterBase Host { get; }
        public string ParameterName { get; }

        public KotfAnimationClip(AnimationClip animationClip, CharacterBase host, string parameterName)
        {
            AnimationClip = animationClip;
            Host = host;
            ParameterName = parameterName;
        }

        public static KotfAnimationClip Create(CharacterBase host, ActionType actionType, string parameter)
        {
            AnimationClip animationClip =
                host.AnimatorController.animationClips.FirstOrDefault(x => x.name.Equals(parameter));

            return actionType switch
            {
                ActionType.Attack => new AttackAnimationClip(animationClip, host, parameter),
                _ => new KotfAnimationClip(animationClip, host, parameter)
            };
        }
    }

    public class AttackAnimationClip : KotfAnimationClip
    {
        public int DowntimeFrames { get; private set; }

        public AttackAnimationClip(AnimationClip animationClip, CharacterBase host, string parameterName)
            : base(animationClip, host, parameterName)
        {
            InitializeDowntimeFrames();
        }

        private void InitializeDowntimeFrames()
        {
            // Get the OnExitAttackWindow AnimationEvent and find out the frame it is situated at with AnimationEvent.time * AnimationClip.frames
            // Subtract accordingly to obtain DowntimeFrames.
            float totalFrames = AnimationClip.frameRate * AnimationClip.length;
            float exitAttackWindowTime = AnimationClip.events
                .FirstOrDefault(x => x.functionName.Equals(nameof(Host.OnExitAttackWindow)))?.time ?? 0.0f;

            DowntimeFrames = Convert.ToInt32(totalFrames - exitAttackWindowTime * AnimationClip.frameRate);
        }
    }
}
