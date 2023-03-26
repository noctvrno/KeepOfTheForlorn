using System;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class AttributeModifier
    {
        // The attribute that is being altered. The alteration can happen with a frequency, so it's best to have it as a float.
        public IAttribute<float> Attribute { get; set; }

        // The value with which the attribute is diminished.
        [SerializeField] private float _diminishingValue;

        // The frequency at which the diminishing will take place.
        [SerializeField] private float _diminishingFrequency;

        // The value with which the attribute is enhanced.
        [SerializeField] private float _enhancingValue;

        // The frequency at which the diminishing will take place.
        [SerializeField] private float _enhancingFrequency;

        public void Diminish()
        {
            if (_diminishingFrequency == 0.0f)
            {
                Attribute.Value -= _diminishingValue;
                return;
            }

            // Diminish the attribute's value over time.
        }

        public void Enhance()
        {
            if (_enhancingFrequency == 0.0f)
            {
                Attribute.Value += _enhancingValue;
                return;
            }

            // Enhance the attribute's value over time.
        }
    }
}
