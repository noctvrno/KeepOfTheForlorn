using System;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public abstract class AttributeModifierBase
    {
        public IAttribute<float> Attribute { get; set; }

        [field: SerializeField]
        public float Value { get; set; }

        [field: SerializeField]
        public float Frequency { get; set; }

        public abstract void Modify();
    }
}
