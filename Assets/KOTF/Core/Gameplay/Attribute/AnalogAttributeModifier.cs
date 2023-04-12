﻿using System;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class AnalogAttributeModifier : IAttributeModifier
    {
        public IAttribute<float> Attribute { get; private set; }

        public float Threshold { get; set; }

        [field: SerializeField]
        [Tooltip("The value that is being changed per each update.")]
        public float Value { get; set; }

        [field: SerializeField]
        [Tooltip("The time interval between each update. [ms]\nA value of 0 means that the update will happen instantly.")]
        public float Rate { get; set; }

        public void Initialize(IAttribute<float> attribute)
        {
            Attribute = attribute;
        }

        public void Initialize(IAttribute<float> attribute, float threshold)
        {
            Initialize(attribute);
            Threshold = threshold;
        }
    }

    [Serializable]
    public class DiscreteAttributeModifier : IAttributeModifier
    {
        public IAttribute<float> Attribute { get; private set; }

        [field: SerializeField]
        public float Value { get; set; }

        public void Initialize(IAttribute<float> attribute)
        {
            Attribute = attribute;
        }
    }

    public interface IAttributeModifier
    {
        IAttribute<float> Attribute { get; }
        float Value { get; set; }

        void Initialize(IAttribute<float> attribute);
    }
}
