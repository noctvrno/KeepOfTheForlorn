using KOTF.Core.Input;
using UnityEngine;

namespace KOTF.Core.Wrappers
{
    public class KotfAnimationClip
    {
        public AnimationClip AnimationClip { get; }
        public ActionType ActionType { get; }
        public string ParameterName { get; }

        public KotfAnimationClip(AnimationClip animationClip, ActionType actionType, string parameterName)
        {
            AnimationClip = animationClip;
            ParameterName = parameterName;
            ActionType = actionType;
        }
    }
}
