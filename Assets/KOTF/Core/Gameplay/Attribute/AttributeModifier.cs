using System;
using System.Collections;
using KOTF.Core.Wrappers;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class UnrestrictedAttributeEnhancer : AttributeEnhancer
    {
        public new GatedAttribute<float> Attribute { get; set; } // Unity does not support covariant return types...

        public void Initialize(KotfGameObject host, GatedAttribute<float> attribute) // Unity does not support covariant parameter types...
        {
            Host = host;
            Attribute = attribute;
            Threshold = Attribute.MaximumValue;
        }
    }

    [Serializable]
    public class AttributeDiminisher : AttributeModifierBase
    {
        public override void Modify()
        {
            if (UpdateRate == 0.0f)
            {
                Attribute.Value -= ValuePerUpdate;
                return;
            }

            // Modify based on UpdateRate.
        }
    }

    [Serializable]
    public class AttributeEnhancer : AttributeModifierBase
    {
        public override void Modify()
        {
            if (UpdateRate == 0.0f)
            {
                Attribute.Value += ValuePerUpdate;
                return;
            }

            // Modify based on UpdateRate.
            Host.StartCoroutine(Update());
        }

        private IEnumerator Update()
        {
            yield return new WaitForSeconds(UpdateRate);
            Attribute.Value += ValuePerUpdate;
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
