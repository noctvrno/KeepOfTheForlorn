using System;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class AuxiliaryAnalogAttributeModifier : AnalogAttributeModifier
    {
        [field: SerializeField]
        public override float Threshold { get; set; }
    }

    [Serializable]
    public class AnalogAttributeModifier : IAttributeModifier
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public GatedAttribute<float> Attribute { get; private set; }

        [field: NonSerialized]
        public virtual float Threshold { get; set; }

        [field: SerializeField]
        [Tooltip("The value that is used to modify the Attribute's Value.")]
        public float Value { get; set; }

        [field: SerializeField]
        [Tooltip(
            "The time interval between each update. [ms]\n Default value: 0.016f which corresponds to a rate of 60 Hz.")]
        public float Rate { get; set; } = 0.016f;

        public void Initialize(GatedAttribute<float> attribute)
        {
            Attribute = attribute;
        }

        public void Initialize(GatedAttribute<float> attribute, float threshold)
        {
            Attribute = attribute;
            Threshold = threshold;
        }
    }

    [Serializable]
    public class DiscreteAttributeModifier : IAttributeModifier
    {
        public Guid Guid { get; } = new();
        public GatedAttribute<float> Attribute { get; private set; }

        [field: SerializeField]
        public float Value { get; set; }

        [field: NonSerialized]
        public float Threshold { get; set; }

        public void Initialize(GatedAttribute<float> attribute)
        {
            Attribute = attribute;
        }

        public void Initialize(GatedAttribute<float> attribute, float threshold)
        {
            Attribute = attribute;
            Threshold = threshold;
        }
    }

    public interface IAttributeModifier
    {
        Guid Guid { get; }
        GatedAttribute<float> Attribute { get; }
        float Value { get; set; }
        float Threshold { get; set; }
    }
}
