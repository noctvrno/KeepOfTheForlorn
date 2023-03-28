using System;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class AttributeDiminisher : AttributeModifierBase
    {
        public override void Modify()
        {
            if (Frequency == 0.0f)
            {
                Attribute.Value -= Value;
                return;
            }

            // Modify based on Frequency.
        }
    }

    [Serializable]
    public class AttributeEnhancer : AttributeModifierBase
    {
        public override void Modify()
        {
            if (Frequency == 0.0f)
            {
                Attribute.Value += Value;
                return;
            }

            // Modify based on Frequency.
        }
    }

    [Serializable]
    public class AttributeModifier
    {
        [field: SerializeField]
        public AttributeDiminisher AttributeDiminisher { get; set; }

        [field: SerializeField]
        public AttributeEnhancer AttributeEnhancer { get; set; }

        public void Diminish()
        {
            AttributeDiminisher.Modify();
        }

        public void Enhance()
        {
            AttributeEnhancer.Modify();
        }
    }
}
