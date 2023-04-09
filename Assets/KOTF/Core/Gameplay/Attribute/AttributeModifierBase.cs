using System;
using KOTF.Core.Services;
using KOTF.Core.Wrappers;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public abstract class AttributeModifierBase
    {
        public virtual CoroutineService CoroutineService { get; private set; }
        public virtual IAttribute<float> Attribute { get; protected set; }

        public virtual float Threshold { get; set; }

        [field: SerializeField]
        [Tooltip("The value that is being changed per each update.")]
        public float ValuePerUpdate { get; set; }

        [field: SerializeField]
        [Tooltip("The time interval between each update. [ms]\nA value of 0 means that the update will happen instantly.")]
        public float UpdateRate { get; set; }

        public void Initialize(CoroutineService coroutineService, IAttribute<float> attribute)
        {
            CoroutineService = coroutineService;
            Attribute = attribute;
        }

        public void Initialize(CoroutineService coroutineService, IAttribute<float> attribute, float threshold)
        {
            Initialize(coroutineService, attribute);
            Threshold = threshold;
        }

        public abstract void Modify();
    }
}
