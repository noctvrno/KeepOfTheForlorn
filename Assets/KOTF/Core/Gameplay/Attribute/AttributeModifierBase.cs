using System;
using KOTF.Core.Wrappers;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public abstract class AttributeModifierBase
    {
        public KotfGameObject Host { get; protected set; }

        public virtual IAttribute<float> Attribute { get; protected set; }

        public virtual float Threshold { get; set; }

        [field: SerializeField]
        [Tooltip("The value that is being changed per each update.")]
        public float ValuePerUpdate { get; set; }

        [field: SerializeField]
        [Tooltip("The time interval between each update. [ms]\nA value of 0 means that the update will happen instantly.")]
        public float UpdateRate { get; set; }

        public virtual void Initialize(KotfGameObject host, IAttribute<float> attribute)
        {
            Host = host;
            Attribute = attribute;
        }

        public abstract void Modify();
    }
}
